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
    class ReadSingleValueByFilterTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;

        private static string json = "[{" +
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
        "}]";

        private static string json1 = "[" +
            "{" +
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
            "}," +
            "{" +
                "\"series\":{" +
                    "\"id\":\"id2\"," +
                    "\"key\":\"key2\"," +
                    "\"name\":\"\"," +
                    "\"tags\":[]," +
                    "\"attributes\":{}" +
                "}," +
                "\"data\":{" +
                    "\"t\":\"2012-01-01T00:00:01.000Z\"," +
                    "\"v\":23.45" +
                "}," +
                "\"tz\":\"UTC\"" +
            "}" +
        "]";

        private static string json2 = "[" +
            "{" +
                "\"series\":{" +
                    "\"id\":\"id3\"," +
                    "\"key\":\"key3\"," +
                    "\"name\":\"\"," +
                    "\"tags\":[]," +
                    "\"attributes\":{}" +
                "}," +
                "\"data\":{" +
                    "\"t\":\"2012-01-01T00:00:01.000Z\"," +
                    "\"v\":34.56" +
                "}," +
                "\"tz\":\"UTC\"" +
            "}" +
        "]";

        private static string jsonTz = "[{" +
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
        "}]";

        private static Series series1 = new Series("key1");
        private static Series series2 = new Series("key2");
        private static Series series3 = new Series("key3");
        private static ZonedDateTime timestamp = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));
        private static Filter filter = new Filter();

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json);
            var client = TestCommon.GetClient(response);

            var result = client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            var expected = new List<SingleValue> {
                new SingleValue(series1, new DataPoint(timestamp, 12.34))
            };
            var output = new List<SingleValue>();
            foreach(SingleValue value in result.Value)
            {
                output.Add(value);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void SmokeTestTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var response = TestCommon.GetResponse(200, jsonTz);
            var client = TestCommon.GetClient(response);

            var result = client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            var expected = new List<SingleValue> {
                new SingleValue(series1, new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34))
            };
            var output = new List<SingleValue>();
            foreach(SingleValue value in result.Value)
            {
                output.Add(value);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void MultipleSegmentSmokeTest()
        {
            var response1 = TestCommon.GetResponse(200, json1);
            response1.Headers.Add(new Parameter {
                Name = "Link",
                Value = "</v1/single/?key=key1&key=2&key=3&ts=2012-01-01T00:00:01+00:00&direction=exact&tz=UTC>; rel=\"next\""
            });
            var response2 = TestCommon.GetResponse(200, json2);

            var calls = 0;
            RestResponse[] responses = { response1, response2 };
            var mockclient = new Mock<RestClient>();
            mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(() => responses[calls]).Callback(() => calls++);

            var client = TestCommon.GetClient(mockclient.Object);
            var result = client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            var expected = new List<SingleValue> {
                new SingleValue(series1, new DataPoint(timestamp, 12.34)),
                new SingleValue(series2, new DataPoint(timestamp, 23.45)),
                new SingleValue(series3, new DataPoint(timestamp, 34.56))
            };
            var output = new List<SingleValue>();
            foreach(SingleValue value in result.Value)
            {
                output.Add(value);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/single/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var filter = new Filter()
                .AddKeys("key1")
                .AddTags("tag1")
                .AddAttributes("key1", "value1");

            client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key1]", "value1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "ts", "2012-01-01T00:00:01+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "direction", "exact"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
        }

        [Test]
        [ExpectedException(typeof(TempoDBException))]
        public void Error()
        {
            var response = TestCommon.GetResponse(403, "You are forbidden");
            var client = TestCommon.GetClient(response);

            var result = client.ReadSingleValue(filter, timestamp, zone, Direction.Exact);

            var output = new List<SingleValue>();
            foreach(SingleValue value in result.Value)
            {
                output.Add(value);
            }
        }
    }
}
