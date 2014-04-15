using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class ReadSummaryBySeriesTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private Series series = new Series("key1");
        private static ZonedDateTime start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
        private static ZonedDateTime end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
        private static Interval interval = new Interval(start.ToInstant(), end.ToInstant());
        private static Dictionary<string, double> data = new Dictionary<string, double> { { "max", 12.34}, { "min", 23.45 } };

        private static string json = "{" +
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

        private static string jsonTz = "{" +
            "\"summary\":{" +
                "\"max\":12.34," +
                "\"min\":23.45" +
            "}," +
            "\"tz\":\"America/Chicago\"," +
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

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.ReadSummary(series, interval);
            var expected = new Response<Summary>(new Summary(series, interval, zone, data), 200);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SmokeTestTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var response = TestCommon.GetResponse(200, jsonTz);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
            var interval = new Interval(start.ToInstant(), end.ToInstant());

            var result = client.ReadSummary(series, interval, zone);
            var expected = new Response<Summary>(new Summary(series, interval, zone, data), 200);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSummary(series, interval, zone);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSummary(series, interval, zone);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/summary/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSummary(series, interval, zone);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
        }
    }
}
