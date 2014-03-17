using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class WriteRequestTests
    {
        [TestFixture]
        class ByKey
        {
            private static DateTimeZone zone = DateTimeZone.Utc;
            private List<DataPoint> datapoints = new List<DataPoint> {
                new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 1, 0)), 12.34),
                new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 2, 0)), 23.45)
            };
            private string json = @"[{""t"":""2012-01-01T00:01:00+00:00"",""v"":12.34},{""t"":""2012-01-01T00:02:00+00:00"",""v"":23.45}]";

            [Test]
            public void SmokeTest()
            {
                var response = TestCommon.GetResponse(200, "");
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.WriteDataPointsByKey("key1", datapoints);
                var expected = new Response<Nothing>(new Nothing(), 200);
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void RequestMethod()
            {
                var response = TestCommon.GetResponse(200, "");
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = TestCommon.GetResponse(200, "");
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/data/")));
            }

            [Test]
            public void RequestParameters()
            {
                var response = TestCommon.GetResponse(200, "");
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "key", "key1"))));
            }

            [Test]
            public void RequestBody()
            {
                var response = TestCommon.GetResponse(200, "");
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json))));
            }
        }
    }
}
