using Client;
using Client.Model;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Client.Tests
{
    [TestFixture]
    public class CursorTests
    {
        [TestFixture]
        class SegmentTests
        {
            [Test]
            public void Constructor()
            {
                List<DataPoint> data = new List<DataPoint> { new DataPoint(new DateTime(2012, 3, 27), 12.34) };
                Segment segment = new Segment(data, "next");
                Assert.AreEqual(data, segment.Data);
                Assert.AreEqual("next", segment.NextUrl);
            }

            [Test]
            public void Deserialize()
            {
                string content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34}],\"rollup\":null}";
                RestSharp.RestResponse response = new RestSharp.RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = content
                };
                RestSharp.Parameter link = new RestSharp.Parameter {
                    Name = "Link",
                    Value = "</v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02>; rel=\"next\""
                };
                response.Headers.Add(link);

                Segment segment = Segment.FromResponse(response);
                Segment expected = new Segment(new List<DataPoint>{new DataPoint(new DateTime(2012, 3, 27), 12.34)}, "/v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02");
                Assert.AreEqual(expected.Data, segment.Data);
                Assert.AreEqual(expected.NextUrl, segment.NextUrl);
            }

            [Test]
            public void Iterator()
            {
                string content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}";
                RestSharp.RestResponse response = new RestSharp.RestResponse
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = content
                };

                Segment segment = Segment.FromResponse(response);
                List<DataPoint> expected = new List<DataPoint> {
                    new DataPoint(new DateTime(2012, 3, 27, 0, 0, 0), 12.34),
                    new DataPoint(new DateTime(2012, 3, 27, 1, 0, 0), 23.45)
                };

                List<DataPoint> output = new List<DataPoint>();
                foreach(DataPoint dp in segment)
                {
                    output.Add(dp);
                }
                Assert.AreEqual(expected, output);
            }
        }

        [TestFixture]
        class RequestTests
        {
            [Test]
            public void SingleSegmentSmokeTest()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = new Mock<RestClient>();
                mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var cursor = client.ReadCursorByKey("key1", new DateTime(2012, 3, 27), new DateTime(2012, 3, 28));

                var expected = new List<DataPoint> {
                    new DataPoint(new DateTime(2012, 3, 27, 0, 0, 0), 12.34),
                    new DataPoint(new DateTime(2012, 3, 27, 1, 0, 0), 23.45)
                };
                var output = new List<DataPoint>();
                foreach(DataPoint dp in cursor)
                {
                    output.Add(dp);
                }
                Assert.AreEqual(expected, output);
            }

            [Test]
            public void MultipleSegmentSmokeTest()
            {
                var response1 = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                response1.Headers.Add(new Parameter {
                    Name = "Link",
                    Value = "</v1/series/key/key1/data/segment/?start=2012-03-27T00:02:00.000-05:00&end=2012-03-28>; rel=\"next\""
                });
                var response2 = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T02:00:00.000-05:00\",\"v\":34.56}],\"rollup\":null}"
                };
                var calls = 0;
                RestResponse[] responses = { response1, response2 };
                var mockclient = new Mock<RestClient>();
                mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(() => responses[calls]).Callback(() => calls++);

                var client = TestCommon.GetClient(mockclient.Object);
                var cursor = client.ReadCursorByKey("key1", new DateTime(2012, 3, 27), new DateTime(2012, 3, 28));

                var expected = new List<DataPoint> {
                    new DataPoint(new DateTime(2012, 3, 27, 0, 0, 0), 12.34),
                    new DataPoint(new DateTime(2012, 3, 27, 1, 0, 0), 23.45),
                    new DataPoint(new DateTime(2012, 3, 27, 2, 0, 0), 34.56)
                };
                var output = new List<DataPoint>();
                foreach(DataPoint dp in cursor)
                {
                    output.Add(dp);
                }
                Assert.AreEqual(expected, output);
            }

            [Test]
            public void RequestMethod()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                client.ReadCursorByKey("key1", new DateTime(2012, 3, 27), new DateTime(2012, 3, 28));

                Expression<Func<RestRequest, bool>> assertion = req => req.Method == Method.GET;
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(assertion)));
            }

            [Test]
            public void RequestStartTime()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = new DateTime(2012, 6, 23);
                var end = new DateTime(2012, 6, 24);

                client.ReadCursorByKey("testkey", start, end, IntervalParameter.Raw());

                Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-06-23T00:00:00.000-05:00");
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(assertion)));
            }

            [Test]
            public void RequestEndTime()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);
                var start = new DateTime(2012, 6, 23);
                var end = new DateTime(2012, 6, 24);

                client.ReadCursorByKey("testkey", start, end, IntervalParameter.Raw());

                Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-06-24T00:00:00.000-05:00");
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(assertion)));
            }

            [Test]
            public void RequestUrl()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.ReadCursorByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/{property}/{value}/data/segment/")));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "key"))));
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "testkey"))));
            }

            [Test]
            public void RequestInterval()
            {
                var response = new RestResponse {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}"
                };
                var mockclient = TestCommon.GetMockRestClient(response);
                var client = TestCommon.GetClient(mockclient.Object);

                client.ReadCursorByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

                Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "interval", "raw");
                mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(assertion)));
            }
        }
    }
}
