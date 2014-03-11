using NodaTime;
using NUnit.Framework;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    class WriteRequestTest
    {
        static JsonSerializer serializer = new JsonSerializer();
        static Series series = new Series("key1");

        [Test]
        public void SerializeUtc()
        {
            var zone = DateTimeZone.Utc;
            var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
            var wr = new WriteRequest().Add(series, datapoint);
            var expected = @"[{""key"":""key1"",""t"":""2012-01-01T00:00:01+00:00"",""v"":12.34}]";
            var json = serializer.Serialize(wr);
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void SerializeTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
            var wr = new WriteRequest().Add(series, datapoint);
            var json = serializer.Serialize(wr);
            var expected = @"[{""key"":""key1"",""t"":""2012-01-01T00:00:01-06:00"",""v"":12.34}]";
            Assert.AreEqual(expected, json);
        }
    }
}
