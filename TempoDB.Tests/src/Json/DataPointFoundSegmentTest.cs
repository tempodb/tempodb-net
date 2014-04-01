using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests
{
    [TestFixture]
    public class DataPointFoundSegmentTest
    {

        private static DateTimeZone utc = DateTimeZone.Utc;

        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = @"{
                    ""tz"":""UTC"",
                    ""data"":[
                        {""interval"":{""start"":""2012-03-28T00:00:00.000+00:00"",""end"":""2012-03-29T00:00:00.000+00:00""},""found"":{""t"":""2012-03-28T23:59:00.000Z"",""v"":2879.0}}
                    ],
                    ""predicate"":{
                        ""period"":""P1D"",
                        ""function"":""max""
                    }
                }";

                var zone = DateTimeZone.Utc;
                var converter = new DataPointFoundSegmentConverter();

                var segment = JsonConvert.DeserializeObject<DataPointFoundSegment>(json, converter);

                var expectedPredicate = new Predicate(Period.FromDays(1), "max");
                var interval = new Interval(utc.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0)).ToInstant(), utc.AtStrictly(new LocalDateTime(2012, 3, 29, 0, 0, 0)).ToInstant());
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 28, 23, 59, 0)), 2879.0);
                var expectedDataPoints = new List<DataPointFound> {
                    new DataPointFound(interval, datapoint)
                };
                Assert.AreEqual(expectedPredicate, segment.Predicate);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }

            [Test]
            public void TimeZoneTest()
            {
                var json = @"{
                    ""tz"":""America/Chicago"",
                    ""data"":[
                        {""interval"":{""start"":""2012-03-28T00:00:00.000Z"",""end"":""2012-03-29T00:00:00.000Z""},""found"":{""t"":""2012-03-28T23:59:00.000-05:00"",""v"":2879.0}}
                    ],
                    ""predicate"":{
                        ""period"":""P1D"",
                        ""function"":""max""
                    }
                }";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new DataPointFoundSegmentConverter();

                var segment = JsonConvert.DeserializeObject<DataPointFoundSegment>(json, converter);

                var expectedPredicate = new Predicate(Period.FromDays(1), "max");
                var interval = new Interval(utc.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0)).ToInstant(), utc.AtStrictly(new LocalDateTime(2012, 3, 29, 0, 0, 0)).ToInstant());
                var datapoint = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 28, 23, 59, 0)), 2879.0);
                var expectedDataPoints = new List<DataPointFound> {
                    new DataPointFound(interval, datapoint)
                };
                Assert.AreEqual(expectedPredicate, segment.Predicate);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }
        }
    }
}
