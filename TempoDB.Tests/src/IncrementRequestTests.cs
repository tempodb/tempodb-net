using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class IncrementRequestTests
    {
        [TestFixture]
        class ById
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
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.IncrementDataPointsById("id1", datapoints);
                var expected = new Result<None>(new None(), true);
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void RequestMethod()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsById("id1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsById("id1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/id/{id}/increment/")));
            }

            [Test]
            public void RequestParameters()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsById("id1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "id", "id1"))));
            }

            [Test]
            public void RequestBody()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsById("id1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json))));
            }
        }

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
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.IncrementDataPointsByKey("key1", datapoints);
                var expected = new Result<None>(new None(), true);
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void RequestMethod()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/increment/")));
            }

            [Test]
            public void RequestParameters()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "key", "key1"))));
            }

            [Test]
            public void RequestBody()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.IncrementDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json))));
            }
        }
    }
}
