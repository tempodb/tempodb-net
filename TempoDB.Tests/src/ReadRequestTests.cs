using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class ReadRequestTests
    {
        [TestFixture]
        class ById
        {
            private static DateTimeZone zone = DateTimeZone.Utc;
            private static string json = @"{
                ""rollup"":{
                    ""function"":""sum"",
                    ""interval"":""PT1H"",
                    ""tz"":""UTC""
                },
                ""data"":[
                    {""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                ]
            }";

            /// [Test]
            /// public void SmokeTest()
            /// {
            ///     var response = new RestResponse {
            ///         Content = json,
            ///         StatusCode = HttpStatusCode.OK
            ///     };
            ///     var mockclient = TestCommon.GetMockRestClient(response);
            ///     var client = TestCommon.GetClient(mockclient.Object);
            ///     var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            ///     var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

            ///     var result = client.ReadDataPointsById("id1", start, end);
            ///     var expected = new Result<None>(new None(), true);
            ///     Assert.AreEqual(expected, result);
            /// }

            [Test]
            public void RequestMethod()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsById("id1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsById("id1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/id/{id}/data/")));
            }

            [Test]
            public void RequestParameters()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsById("id1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "id", "id1"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            }
        }

        [TestFixture]
        class ByKey
        {
            private static DateTimeZone zone = DateTimeZone.Utc;
            private static string json = @"{
                ""rollup"":{
                    ""function"":""sum"",
                    ""interval"":""PT1H"",
                    ""tz"":""UTC""
                },
                ""data"":[
                    {""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                ]
            }";

            /// [Test]
            /// public void SmokeTest()
            /// {
            ///     var response = new RestResponse {
            ///         Content = json,
            ///         StatusCode = HttpStatusCode.OK
            ///     };
            ///     var mockclient = TestCommon.GetMockRestClient(response);
            ///     var client = TestCommon.GetClient(mockclient.Object);
            ///     var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            ///     var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

            ///     var result = client.ReadDataPointsByKey("key1", start, end);
            ///     var expected = new Result<None>(new None(), true);
            ///     Assert.AreEqual(expected, result);
            /// }

            [Test]
            public void RequestMethod()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsByKey("key1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsByKey("key1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/data/")));
            }

            [Test]
            public void RequestParameters()
            {
                var response = new RestResponse {
                    Content = json,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));

                client.ReadDataPointsByKey("key1", start, end);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            }
        }
    }
}
