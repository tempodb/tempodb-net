using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    public class DeleteSeriesByFilterTest
    {
        private static string json = @"{""deleted"":127}";
        private static Filter filter = new Filter();

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json);
            var client = TestCommon.GetClient(response);

            var result = client.DeleteSeries(filter);
            var expected = new Response<DeleteSummary>(new DeleteSummary(127), 200);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.DeleteSeries(filter);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.DELETE)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.DeleteSeries(filter);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            Filter filter = new Filter()
                .AddKeys("key1")
                .AddTags("tag1")
                .AddTags("tag1")
                .AddAttributes("key1", "value1")
                .AddAttributes("key2", "value2");

            client.DeleteSeries(filter);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key1]", "value1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key2]", "value2"))));
        }
    }
}
