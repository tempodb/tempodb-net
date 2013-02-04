using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Json
{
    [TestFixture]
    public class DataPointTests
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var zone = DateTimeZone.Utc;
                var converter = new ZonedDateTimeConverter();
                var datapoint = JsonConvert.DeserializeObject<DataPoint>("{\"t\":\"2012-01-01T00:00:01.000+00:00\",\"v\":12.34}", converter);
                var expected = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                Assert.AreEqual(expected, datapoint);
            }

            [Test]
            public void TimeZoneTest()
            {
                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new ZonedDateTimeConverter(zone);
                var datapoint = JsonConvert.DeserializeObject<DataPoint>("{\"t\":\"2012-01-01T00:00:01.000-06:00\",\"v\":12.34}", converter);
                var expected = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                Assert.AreEqual(expected, datapoint);
            }
        }

        class Serialize
        {
            [Test]
            public void UtcTest()
            {
                var zone = DateTimeZone.Utc;
                var converter = new ZonedDateTimeConverter();
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                var json = JsonConvert.SerializeObject(datapoint, converter);
                var expected = @"{""t"":""2012-01-01T00:00:01+00:00"",""v"":12.34}";
                Assert.AreEqual(expected, json);
            }

            [Test]
            public void TimeZoneTest()
            {
                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new ZonedDateTimeConverter();
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
                var json = JsonConvert.SerializeObject(datapoint, converter);
                var expected = @"{""t"":""2012-01-01T00:00:01-06:00"",""v"":12.34}";
                Assert.AreEqual(expected, json);
            }
        }
    }
}
