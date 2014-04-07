using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TempoDB.Exceptions;


namespace TempoDB.Tests
{
    [TestFixture]
    class ReadMultiDataPointsByFilterTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private static string json = @"{
            ""rollup"":{
                ""fold"":""sum"",
                ""period"":""PT1H""
            },
            ""tz"":""UTC"",
            ""data"":[
                {""t"":""2012-01-01T00:00:00.000+00:00"",""v"":{""key2"":23.45,""key1"":12.34}}
            ],
            ""series"":[
                {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
            ]
        }";

        private static string json1 = @"{
            ""rollup"":null,
            ""tz"":""UTC"",
            ""data"":[
                {""t"":""2012-03-27T00:00:00.000+00:00"",""v"":{""key2"":23.45,""key1"":12.34}},
                {""t"":""2012-03-27T01:00:00.000+00:00"",""v"":{""key2"":34.56,""key1"":23.45}}
            ],
            ""series"":[
                {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
            ]
        }";

        private static string json2 = @"{
            ""rollup"":null,
            ""tz"":""UTC"",
            ""data"":[
                {""t"":""2012-03-27T02:00:00.000+00:00"",""v"":{""key2"":45.67,""key1"":34.56}}
            ],
            ""series"":[
                {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
            ]
        }";

        private static string jsonTz = @"{
            ""rollup"":null,
            ""tz"":""America/Chicago"",
            ""data"":[
                {""t"":""2012-03-27T00:00:00.000-05:00"",""v"":{""key2"":23.45,""key1"":12.34}},
                {""t"":""2012-03-27T01:00:00.000-05:00"",""v"":{""key2"":34.56,""key1"":23.45}}
            ],
            ""series"":[
                {""id"":""id1"",""key"":""key1"",""name"":"""",""tags"":[],""attributes"":{}},
                {""id"":""id2"",""key"":""key2"",""name"":"""",""tags"":[],""attributes"":{}}
            ]
        }";

        private static ZonedDateTime start = zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
        private static ZonedDateTime end = zone.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0));
        private static Interval interval = new Interval(start.ToInstant(), end.ToInstant());
        private static Filter filter = new Filter().AddKeys("key1").AddKeys("key2");

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json1);
            var client = TestCommon.GetClient(response);

            var result = client.ReadDataPoints(filter, interval);

            var expected = new List<MultiDataPoint> {
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 1, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}})
            };
            var output = new List<MultiDataPoint>();
            foreach(MultiDataPoint dp in result.Value.DataPoints)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void SmokeTestTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var response = TestCommon.GetResponse(200, jsonTz);
            var client = TestCommon.GetClient(response);

            var result = client.ReadDataPoints(filter, interval, zone);

            var expected = new List<MultiDataPoint> {
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 1, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}})
            };
            var output = new List<MultiDataPoint>();
            foreach(MultiDataPoint dp in result.Value.DataPoints)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void MultipleSegmentSmokeTest()
        {
            var response1 = TestCommon.GetResponse(200, json1);
            response1.Headers.Add(new Parameter {
                Name = "Link",
                Value = "</v1/segment/?key=key1&start=2012-03-27T00:02:00.000-05:00&end=2012-03-28>; rel=\"next\""
            });
            var response2 = TestCommon.GetResponse(200, json2);

            var calls = 0;
            RestResponse[] responses = { response1, response2 };
            var mockclient = new Mock<RestClient>();
            mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(() => responses[calls]).Callback(() => calls++);

            var client = TestCommon.GetClient(mockclient.Object);
            var result = client.ReadDataPoints(filter, interval);

            var expected = new List<MultiDataPoint> {
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0)), new Dictionary<string, double> {{"key1", 12.34}, {"key2", 23.45}}),
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 1, 0, 0)), new Dictionary<string, double> {{"key1", 23.45}, {"key2", 34.56}}),
                new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 2, 0, 0)), new Dictionary<string, double> {{"key1", 34.56}, {"key2", 45.67}})
            };
            var output = new List<MultiDataPoint>();
            foreach(MultiDataPoint dp in result.Value.DataPoints)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadDataPoints(filter, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadDataPoints(filter, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/multi/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);
            var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
            var interval = new Interval(start.ToInstant(), end.ToInstant());

            client.ReadDataPoints(filter, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key2"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
        }

        [Test]
        public void RequestParametersRollup()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);
            var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
            var interval = new Interval(start.ToInstant(), end.ToInstant());

            var rollup = new Rollup(Period.FromMinutes(1), Fold.Mean);
            client.ReadDataPoints(filter, interval, rollup:rollup);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key2"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "rollup.period", "PT1M"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "rollup.fold", "mean"))));
        }

        [Test]
        [ExpectedException(typeof(TempoDBException))]
        public void Error()
        {
            var response = TestCommon.GetResponse(403, "You are forbidden");
            var client = TestCommon.GetClient(response);

            client.ReadDataPoints(filter, interval);
        }
    }
}
