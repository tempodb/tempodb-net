using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests
{
    [TestFixture]
    public class MultiDataPointSegmentTest
    {
        class Deserialize
        {
            [Test]
            public void UtcTest()
            {
                var json = @"{
                    ""rollup"":{
                        ""fold"":""sum"",
                        ""period"":""PT1H""
                    },
                    ""tz"":""UTC"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""key2"":23.45,""key1"":12.34}},
                        {""t"":""2012-01-01T01:00:00.000+00:00"",""v"":{""key2"":34.56,""key1"":23.45}},
                        {""t"":""2012-01-01T02:00:00.000+00:00"",""v"":{""key2"":45.67,""key1"":34.56}}
                    ],
                    ""series"":[
                        {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                        {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
                    ]
                }";

                var zone = DateTimeZone.Utc;
                var converter = new MultiDataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<MultiDataPointSegment>(json, converter);
                var expectedRollup = new Rollup(Fold.Sum, Period.FromHours(1));
                var expectedDataPoints = new List<MultiDataPoint> {
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 1, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 2, 0, 0)), new Dictionary<string, double> {{"key1", 34.56}, {"key2", 45.67}})
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
                        ""fold"":""sum"",
                        ""period"":""PT1H""
                    },
                    ""tz"":""America/Chicago"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""key2"":23.45,""key1"":12.34}},
                        {""t"":""2012-01-01T01:00:00.000+00:00"",""v"":{""key2"":34.56,""key1"":23.45}},
                        {""t"":""2012-01-01T02:00:00.000+00:00"",""v"":{""key2"":45.67,""key1"":34.56}}
                    ],
                    ""series"":[
                        {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                        {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
                    ]
                }";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new MultiDataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<MultiDataPointSegment>(json, converter);

                var expectedRollup = new Rollup(Fold.Sum, Period.FromHours(1));
                var expectedDataPoints = new List<MultiDataPoint> {
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 18, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 19, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 20, 0, 0)), new Dictionary<string, double> {{"key1", 34.56}, {"key2", 45.67}})
                };
                Assert.AreEqual(expectedRollup, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }

            [Test]
            public void RollupNull()
            {
                var json = @"{
                    ""rollup"":null,
                    ""tz"":""America/Chicago"",
                    ""data"":[
                        {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""key2"":23.45,""key1"":12.34}},
                        {""t"":""2012-01-01T01:00:00.000+00:00"",""v"":{""key2"":34.56,""key1"":23.45}},
                        {""t"":""2012-01-01T02:00:00.000+00:00"",""v"":{""key2"":45.67,""key1"":34.56}}
                    ],
                    ""series"":[
                        {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                        {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
                    ]
                }";

                var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
                var converter = new MultiDataPointSegmentConverter();

                var segment = JsonConvert.DeserializeObject<MultiDataPointSegment>(json, converter);

                var expectedDataPoints = new List<MultiDataPoint> {
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 18, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 19, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}}),
                    new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2011, 12, 31, 20, 0, 0)), new Dictionary<string, double> {{"key1", 34.56}, {"key2", 45.67}})
                };
                Assert.AreEqual(null, segment.Rollup);
                Assert.AreEqual(expectedDataPoints, segment.Data);
                Assert.AreEqual(zone, segment.TimeZone);
            }
        }
    }
}
