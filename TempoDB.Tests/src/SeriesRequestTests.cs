using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class CreateSeries
    {
        private Series series = new Series("key1");

        [Test]
        public void SmokeTest()
        {
            var json = @"{
                ""key"":""key1"",
                ""name"":"""",
                ""tags"":[],
                ""attributes"":{}
            }";

            var response = new RestResponse {
                Content = json,
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.CreateSeries(series);
            var expected = new Response<Series>(series, 200);
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

            client.CreateSeries(series);

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

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
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

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "key1"))));
        }
    }

    [TestFixture]
    class UpdateSeries
    {
        private Series series = new Series("key1");
        private string json = @"{
            ""key"":""key1"",
            ""name"":"""",
            ""tags"":[],
            ""attributes"":{}
        }";

        [Test]
        public void SmokeTest()
        {
            var response = new RestResponse {
                Content = json,
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.UpdateSeries(series);
            var expected = new Response<Series>(series, 200);

            Assert.AreEqual(expected, result);
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

            client.UpdateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.PUT)));
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

            client.UpdateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/")));
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

            client.UpdateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
        }
    }

    [TestFixture]
    class FilterSeries
    {
        private List<Series> series = new List<Series> { new Series("key1"), new Series("key2") };
        private string json = @"[{
            ""id"":""id1"",
            ""key"":""key1"",
            ""name"":"""",
            ""tags"":[],
            ""attributes"":{}
        },{
            ""id"":""id2"",
            ""key"":""key2"",
            ""name"":"""",
            ""tags"":[],
            ""attributes"":{}
        }]";

        [Test]
        public void SmokeTest()
        {
            var response = new RestResponse {
                Content = json,
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.FilterSeries(new Filter());

            var cursor = new Cursor<Series>(new SegmentEnumerator<Series>(null, new Segment<Series>(series, null)));
            var expected = new Response<Cursor<Series>>(cursor, 200);

            var resultList = new List<Series>();
            foreach(Series s in result.Value)
            {
                resultList.Add(s);
            }

            Assert.IsTrue(result.Success);
            Assert.AreEqual(series, resultList);
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

            client.FilterSeries(new Filter());

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

            client.FilterSeries(new Filter());

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
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

            var filter = new Filter();
            filter.Keys.Add("key1");
            filter.Tags.Add("tag1");
            filter.Attributes.Add("key1", "value1");
            client.FilterSeries(filter);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key1]", "value1"))));
        }
    }
}
