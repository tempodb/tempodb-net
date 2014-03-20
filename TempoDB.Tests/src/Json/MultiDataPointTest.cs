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
    public class MultiDataPointTest
    {
        class Deserialize
        {
            string json = @"{""t"":""2012-03-27T00:00:00.000Z"",""v"":{""key2"":23.45,""key1"":12.34}}";

            [Test]
            public void UtcTest()
            {
                var zone = DateTimeZone.Utc;
                var converter = new ZonedDateTimeConverter();
                var datapoint = JsonConvert.DeserializeObject<MultiDataPoint>(json, converter);
                var expected = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}});
                Assert.AreEqual(expected, datapoint);
            }

            [Test]
            public void TimeZoneTest()
            {
                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new ZonedDateTimeConverter(zone);
                var datapoint = JsonConvert.DeserializeObject<MultiDataPoint>(json, converter);
                var expected = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 26, 19, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}});
                Assert.AreEqual(expected, datapoint);
            }
        }
    }
}
