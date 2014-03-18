using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TempoDB.Exceptions;


namespace TempoDB.Tests
{
    [TestFixture]
    class GetSeriesByFilter
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
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.GetSeries(new Filter());

            var resultList = new List<Series>();
            foreach(Series s in result.Value)
            {
                resultList.Add(s);
            }

            Assert.AreEqual(series, resultList);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.GetSeries(new Filter());

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.GetSeries(new Filter());

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
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
            client.GetSeries(filter);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key1]", "value1"))));
        }

        [Test]
        [ExpectedException(typeof(TempoDBException))]
        public void Error()
        {
            var response = TestCommon.GetResponse(403, "You are forbidden");
            var client = TestCommon.GetClient(response);

            var result = client.GetSeries(new Filter());
        }
    }
}

