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

                var result = client.WriteDataPointsById("id1", datapoints);
                var expected = new Response<None>(new None(), 200);
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

                client.WriteDataPointsById("id1", datapoints);

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

                client.WriteDataPointsById("id1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/id/{id}/data/")));
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

                client.WriteDataPointsById("id1", datapoints);

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

                client.WriteDataPointsById("id1", datapoints);

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

                var result = client.WriteDataPointsByKey("key1", datapoints);
                var expected = new Response<None>(new None(), 200);
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

                client.WriteDataPointsByKey("key1", datapoints);

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

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/data/")));
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

                client.WriteDataPointsByKey("key1", datapoints);

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

                client.WriteDataPointsByKey("key1", datapoints);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json))));
            }
        }

        [TestFixture]
        class Bulk
        {
            private static DateTimeZone zone = DateTimeZone.Utc;
            private List<BulkDataSet> datapoints1 = new List<BulkDataSet> {
                new BulkDataSet(
                    zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 1, 0)),
                    new List<BulkPoint> {
                        new BulkIdPoint("id1", 12.34),
                        new BulkKeyPoint("key1", 23.45)
                    }
                )
            };
            private string json1 = @"{""t"":""2012-01-01T00:01:00+00:00"",""data"":[{""id"":""id1"",""v"":12.34},{""key"":""key1"",""v"":23.45}]}";
            private string json2 = @"{""t"":""2012-01-01T00:02:00+00:00"",""data"":[{""id"":""id1"",""v"":34.56},{""key"":""key1"",""v"":45.67}]}";

            private List<BulkDataSet> datapoints2 = new List<BulkDataSet> {
                new BulkDataSet(
                    zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 1, 0)),
                    new List<BulkPoint> {
                        new BulkIdPoint("id1", 12.34),
                        new BulkKeyPoint("key1", 23.45)
                    }
                ),
                new BulkDataSet(
                    zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 2, 0)),
                    new List<BulkPoint> {
                        new BulkIdPoint("id1", 34.56),
                        new BulkKeyPoint("key1", 45.67)
                    }
                )
            };

            [Test]
            public void SmokeTest()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.WriteBulkData(datapoints1);
                var expected = new Response<None>(new None(), 200);
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void SmokeTest2()
            {
                var response = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.WriteBulkData(datapoints2);
                var expected = new Response<None>(new None(), 200);
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void MultiCall()
            {
                var response1 = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                var response2 = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                RestResponse[] responses = { response1, response2 };

                var calls = 0;
                List<RestRequest> requests = new List<RestRequest>();
                var mockclient = new Mock<RestClient>();
                mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>()))
                    .Returns(() => responses[calls])
                    .Callback<RestRequest>((request) => { calls++; requests.Add(request); });
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.WriteBulkData(datapoints2);

                Assert.IsTrue(TestCommon.ContainsParameter(requests[0].Parameters, "application/json", json1));
                Assert.IsTrue(TestCommon.ContainsParameter(requests[1].Parameters, "application/json", json2));
            }

            [Test]
            public void Failure()
            {
                var response1 = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.Forbidden
                };
                var response2 = new RestResponse {
                    Content = "",
                    StatusCode = HttpStatusCode.OK
                };
                RestResponse[] responses = { response1, response2 };

                var calls = 0;
                List<RestRequest> requests = new List<RestRequest>();
                var mockclient = new Mock<RestClient>();
                mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>()))
                    .Returns(() => responses[calls])
                    .Callback<RestRequest>((request) => { calls++; requests.Add(request); });
                var client = TestCommon.GetClient(mockclient.Object);

                var result = client.WriteBulkData(datapoints2);
                var expected = new Response<None>(new None(), 403);
                Assert.IsFalse(result.Success);
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

                client.WriteBulkData(datapoints1);

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

                client.WriteBulkData(datapoints1);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/data/")));
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

                client.WriteBulkData(datapoints1);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json1))));
            }
        }
    }
}
