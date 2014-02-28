using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class ReadRequestTests
    {
        [TestFixture]
        class ByKey
        {
            private static DateTimeZone zone = DateTimeZone.Utc;
            private static string json = @"{
                ""rollup"":{
                    ""function"":""sum"",
                    ""interval"":""PT1H""
                },
                ""tz"":""UTC"",
                ""data"":[
                    {""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                ]
            }";

            private static string json1 = @"{
                ""data"":[
                    {""t"":""2012-03-27T00:00:00.000-05:00"",""v"":12.34},
                    {""t"":""2012-03-27T00:01:00.000-05:00"",""v"":23.45}
                ],
                ""tz"":""UTC"",
                ""rollup"":null
            }";

            private static string json2 = @"{
                ""data"":[
                    {""t"":""2012-03-27T00:02:00.000-05:00"",""v"":34.56}
                ],
                ""tz"":""UTC"",
                ""rollup"":null
            }";

            [Test]
            public void SmokeTest()
            {
                var response = new RestResponse {
                    Content = json1,
                    StatusCode = HttpStatusCode.OK
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0));

                var result = client.ReadDataPointsByKey("key1", start, end);

                var expected = new List<DataPoint> {
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 0, 0)), 12.34),
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 1, 0)), 23.45)
                };
                var output = new List<DataPoint>();
                foreach(DataPoint dp in result.Value.DataPoints)
                {
                    output.Add(dp);
                }

                Assert.AreEqual(expected, output);
            }

            [Test]
            public void MultipleSegmentSmokeTest()
            {
                var response1 = new RestResponse {
                    Content = json1,
                    StatusCode = HttpStatusCode.OK
                };
                response1.Headers.Add(new Parameter {
                    Name = "Link",
                    Value = "</v1/series/key/key1/data/segment/?start=2012-03-27T00:02:00.000-05:00&end=2012-03-28>; rel=\"next\""
                });
                var response2 = new RestResponse {
                    Content = json2,
                    StatusCode = HttpStatusCode.OK
                };

                var calls = 0;
                RestResponse[] responses = { response1, response2 };
                var mockclient = new Mock<RestClient>();
                mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(() => responses[calls]).Callback(() => calls++);

                var client = TestCommon.GetClient(mockclient.Object);
                var start = zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
                var end = zone.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0));
                var result = client.ReadDataPointsByKey("key1", start, end);

                var expected = new List<DataPoint> {
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 0, 0)), 12.34),
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 1, 0)), 23.45),
                    new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 2, 0)), 34.56)
                };
                var output = new List<DataPoint>();
                foreach(DataPoint dp in result.Value.DataPoints)
                {
                    output.Add(dp);
                }

                Assert.AreEqual(expected, output);
            }

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

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/data/segment/")));
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

                var rollup = new Rollup(Fold.Mean, Period.FromMinutes(1));
                client.ReadDataPointsByKey("key1", start, end, rollup:rollup);

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "interval", "PT1M"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "function", "mean"))));
            }
        }
    }
}
