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
    public class DataPointFoundTest
    {
        class Deserialize
        {
            static IntervalConverter intervalConverter = new IntervalConverter();
            static DateTimeZone utc = DateTimeZone.Utc;

            [Test]
            public void UtcTest()
            {
                var zone = DateTimeZone.Utc;
                var converter = new ZonedDateTimeConverter();
                var json = @"{""interval"":{""start"":""2012-03-28T00:00:00.000Z"",""end"":""2012-03-29T00:00:00.000Z""},""found"":{""t"":""2012-03-28T23:59:00.000Z"",""v"":2879.0}}";

                var found = JsonConvert.DeserializeObject<DataPointFound>(json, converter, intervalConverter);
                var interval = new Interval(utc.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0)).ToInstant(), utc.AtStrictly(new LocalDateTime(2012, 3, 29, 0, 0, 0)).ToInstant());
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 28, 23, 59, 0)), 2879.0);
                var expected = new DataPointFound(interval, datapoint);
                Assert.AreEqual(expected, found);
            }

            [Test]
            public void TimeZoneTest()
            {
                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new ZonedDateTimeConverter(zone);
                var json = @"{""interval"":{""start"":""2012-03-28T00:00:00.000Z"",""end"":""2012-03-29T00:00:00.000Z""},""found"":{""t"":""2012-03-28T23:59:00.000-05:00"",""v"":2879.0}}";

                var found = JsonConvert.DeserializeObject<DataPointFound>(json, converter, intervalConverter);
                var interval = new Interval(utc.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0)).ToInstant(), utc.AtStrictly(new LocalDateTime(2012, 3, 29, 0, 0, 0)).ToInstant());
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 28, 23, 59, 0)), 2879.0);
                var expected = new DataPointFound(interval, datapoint);
                Assert.AreEqual(expected, found);
            }
        }
    }
}
