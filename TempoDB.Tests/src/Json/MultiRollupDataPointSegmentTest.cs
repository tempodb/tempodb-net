using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests
{
    [TestFixture]
    public class MultiRollupDataPointSegmentTest
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = @"{
                    ""rollup"":{
                        ""folds"":[""sum"",""max""],
                        ""period"":""PT1H""
                    },
                    ""tz"":""UTC"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""max"":23.45,""sum"":12.34}},
                        {""t"":""2012-01-01T01:00:00.000+00:00"",""v"":{""max"":34.56,""sum"":23.45}},
                        {""t"":""2012-01-01T02:00:00.000+00:00"",""v"":{""max"":45.67,""sum"":34.56}}
                    ],
                    ""series"":{""id"":""id1"",""key"":""sum"",""name"":"""",""tags"":[],""attributes"":{}}
                }";

                var zone = DateTimeZone.Utc;
                var converter = new MultiRollupDataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<MultiRollupDataPointSegment>(json, converter);
                var expectedRollup = new MultiRollup(Period.FromHours(1), new Fold[] { Fold.Sum, Fold.Max });
                var expectedDataPoints = new List<MultiDataPoint> {
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0)), new Dictionary<string, double> {{"sum", 12.34}, {"max", 23.45}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 1, 0, 0)), new Dictionary<string, double> {{"sum", 23.45}, {"max", 34.56}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 2, 0, 0)), new Dictionary<string, double> {{"sum", 34.56}, {"max", 45.67}})
                };
                Assert.AreEqual(expectedRollup, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }

            [Test]
            public void TimeZoneTest()
            {
                var json = @"{
                    ""rollup"":{
                        ""folds"":[""sum"",""max""],
                        ""period"":""PT1H""
                    },
                    ""tz"":""America/Chicago"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""max"":23.45,""sum"":12.34}},
                        {""t"":""2012-01-01T01:00:00.000+00:00"",""v"":{""max"":34.56,""sum"":23.45}},
                        {""t"":""2012-01-01T02:00:00.000+00:00"",""v"":{""max"":45.67,""sum"":34.56}}
                    ],
                    ""series"":[
                        {""id"":""id1"",""key"":""sum"",""name"":"""",""tags"":[],""attributes"":{}},
                        {""id"":""id2"",""key"":""max"",""name"":"""",""tags"":[],""attributes"":{}}
                    ]
                }";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new MultiRollupDataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<MultiRollupDataPointSegment>(json, converter);

                var expectedRollup = new MultiRollup(Period.FromHours(1), new Fold[] { Fold.Sum, Fold.Max });
                var expectedDataPoints = new List<MultiDataPoint> {
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 18, 0, 0)), new Dictionary<string, double> {{"sum", 12.34}, {"max", 23.45}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 19, 0, 0)), new Dictionary<string, double> {{"sum", 23.45}, {"max", 34.56}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 20, 0, 0)), new Dictionary<string, double> {{"sum", 34.56}, {"max", 45.67}})
                };
                Assert.AreEqual(expectedRollup, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }
        }
    }
}
