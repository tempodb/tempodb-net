using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    public class CreateSeriesTest
    {
        private Series series = new Series("key1", "name1");
        private Series series1 = new Series("key1", "name1");
        private string body = "{\"key\":\"key1\",\"name\":\"name1\",\"tags\":[],\"attributes\":{}}";
        private string json = @"{
            ""id"":""id1"",
            ""key"":""key1"",
            ""name"":""name1"",
            ""tags"":[],
            ""attributes"":{}
        }";

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json);
            var client = TestCommon.GetClient(response);

            var result = client.CreateSeries(series);
            var expected = new Response<Series>(series1, 200);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
        }

        [Test]
        public void RequestBody()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", body))));
        }
    }
}
