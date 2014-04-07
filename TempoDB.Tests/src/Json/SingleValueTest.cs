using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    public class SingleValueTest
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = "{" +
                    "\"series\":{" +
                        "\"id\":\"id1\"," +
                        "\"key\":\"key1\"," +
                        "\"name\":\"\"," +
                        "\"tags\":[]," +
                        "\"attributes\":{}" +
                    "}," +
                    "\"data\":{" +
                        "\"t\":\"2012-01-01T00:00:01.000Z\"," +
                        "\"v\":12.34" +
                    "}," +
                    "\"tz\":\"UTC\"" +
                "}";

                var zone = DateTimeZone.Utc;
                var converter = new SingleValueConverter();
                var value = JsonConvert.DeserializeObject<SingleValue>(json, converter);
                var series = new Series("key1");
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                var expected = new SingleValue(series, datapoint);
                Assert.AreEqual(expected, value);
            }

            [Test]
            public void TimeZoneTest()
            {
                var json = "{" +
                    "\"series\":{" +
                        "\"id\":\"id1\"," +
                        "\"key\":\"key1\"," +
                        "\"name\":\"\"," +
                        "\"tags\":[]," +
                        "\"attributes\":{}" +
                    "}," +
                    "\"data\":{" +
                        "\"t\":\"2012-01-01T00:00:01.000-06:00\"," +
                        "\"v\":12.34" +
                    "}," +
                    "\"tz\":\"America/Chicago\"" +
                "}";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new SingleValueConverter();
                var value = JsonConvert.DeserializeObject<SingleValue>(json, converter);
                var series = new Series("key1");
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                var expected = new SingleValue(series, datapoint);
                Assert.AreEqual(expected, value);
            }
        }
    }
}
