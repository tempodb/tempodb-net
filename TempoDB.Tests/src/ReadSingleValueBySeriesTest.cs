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
    class ReadSingleValueBySeriesTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private Series series = new Series("key1");
        private static ZonedDateTime timestamp = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));

        private static string json = "{" +
            "\"series\":{" +
                "\"id\":\"id1\"," +
                "\"key\":\"key1\"," +
                "\"name\":\"\"," +
                "\"tags\":[]," +
                "\"attributes\":{}" +
            "}," +
            "\"data\":{" +
                "\"t\":\"2012-01-01T00:00:01.000Z\"," +
                "\"v\":12.34" +
            "}," +
            "\"tz\":\"UTC\"" +
        "}";

        private static string jsonTz = "{" +
            "\"series\":{" +
                "\"id\":\"id1\"," +
                "\"key\":\"key1\"," +
                "\"name\":\"\"," +
                "\"tags\":[]," +
                "\"attributes\":{}" +
            "}," +
            "\"data\":{" +
                "\"t\":\"2012-01-01T00:00:01.000-06:00\"," +
                "\"v\":12.34" +
            "}," +
            "\"tz\":\"America/Chicago\"" +
        "}";

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var timestamp = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));
            var datapoint = new DataPoint(timestamp, 12.34);
            var result = client.ReadSingleValue(series, timestamp, zone, Direction.Exact);
            var expected = new Response<SingleValue>(new SingleValue(series, datapoint), 200);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SmokeTestTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var response = TestCommon.GetResponse(200, jsonTz);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var timestamp = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));
            var datapoint = new DataPoint(timestamp, 12.34);
            var result = client.ReadSingleValue(series, timestamp, zone, Direction.Exact);
            var expected = new Response<SingleValue>(new SingleValue(series, datapoint), 200);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSingleValue(series, timestamp, zone, Direction.Exact);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSingleValue(series, timestamp, zone, Direction.Exact);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/single/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSingleValue(series, timestamp, zone, Direction.Nearest);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "ts", "2012-01-01T00:00:01+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "direction", "nearest"))));
        }
    }
}
