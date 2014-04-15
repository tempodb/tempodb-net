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
    public class SummaryTest
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = "{" +
                    "\"summary\":{" +
                        "\"max\":12.34," +
                        "\"min\":23.45" +
                    "}," +
                    "\"tz\":\"UTC\"," +
                    "\"start\":\"2012-01-01T00:00:00.000Z\"," +
                    "\"end\":\"2012-01-02T00:00:00.000Z\"," +
                    "\"series\":{" +
                        "\"id\":\"id1\"," +
                        "\"key\":\"key1\"," +
                        "\"name\":\"\"," +
                        "\"tags\":[]," +
                        "\"attributes\":{}" +
                    "}" +
                "}";

                var zone = DateTimeZone.Utc;
                var converter = new SummaryConverter();
                var value = JsonConvert.DeserializeObject<Summary>(json, converter);
                var series = new Series("key1");
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
                var data = new Dictionary<string, double> { { "max", 12.34 }, { "min", 23.45 } };
                var expected = new Summary(series, new Interval(start.ToInstant(), end.ToInstant()), zone, data);
                Assert.AreEqual(expected, value);
            }

            [Test]
            public void TimeZoneTest()
            {
                var json = "{" +
                    "\"summary\":{" +
                        "\"max\":12.34," +
                        "\"min\":23.45" +
                    "}," +
                    "\"tz\":\"UTC\"," +
                    "\"start\":\"2012-01-01T00:00:00.000-06:00\"," +
                    "\"end\":\"2012-01-02T00:00:00.000-06:00\"," +
                    "\"series\":{" +
                        "\"id\":\"id1\"," +
                        "\"key\":\"key1\"," +
                        "\"name\":\"\"," +
                        "\"tags\":[]," +
                        "\"attributes\":{}" +
                    "}" +
                "}";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new SummaryConverter();
                var value = JsonConvert.DeserializeObject<Summary>(json, converter);
                var series = new Series("key1");
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
                var data = new Dictionary<string, double> { { "max", 12.34 }, { "min", 23.45 } };
                var expected = new Summary(series, new Interval(start.ToInstant(), end.ToInstant()), zone, data);
                Assert.AreEqual(expected, value);
            }
        }
    }
}
