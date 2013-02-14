using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests
{
    [TestFixture]
    public class DataPointSegmentTests
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = @"{
                    ""rollup"":{
                        ""function"":""sum"",
                        ""interval"":""PT1H""
                    },
                    ""tz"":""UTC"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                    ]
                }";

                var zone = DateTimeZone.Utc;
                var converter = new DataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<DataPointSegment>(json, converter);
                var expectedRollup = new Rollup("sum", Period.FromHours(1));
                var expectedDataPoints = new List<DataPoint> {
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34)
                };
                /// Assert.AreEqual(expectedRollup, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }

            [Test]
            public void TimeZoneTest()
            {
                var json = @"{
                    ""rollup"":{
                        ""function"":""sum"",
                        ""interval"":""PT1H""
                    },
                    ""tz"":""America/Chicago"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                    ]
                }";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new DataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<DataPointSegment>(json, converter);

                var expectedRollup = new Rollup("sum", Period.FromHours(1));
                var expectedDataPoints = new List<DataPoint> {
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 18, 0, 1)), 12.34)
                };
                /// Assert.AreEqual(expectedRollup, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }
        }
    }
}
