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
    public class SummaryTests
    {

        [Test]
        public void SmokeTest()
        {
            var expected = new Summary {
                {"mean", 3.0},
                {"sum", 12.0},
                {"min", 2.0},
                {"max", 4.0},
                {"stddev", 0.8165},
                {"ss", 2.0},
                {"count", 4}
            };

            var mockclient = TestCommon.GetMockRestClient<Summary>(expected);
            var client = TestCommon.GetClient(mockclient.Object);
            var summary = client.ReadSummaryByKey("key1", new DateTime(2012, 3, 27), new DateTime(2012, 3, 28));
            Assert.AreEqual(expected, summary);
        }

        [Test]
        public void RequestMethod()
        {
            var mockclient = TestCommon.GetMockRestClient<Summary>(new Summary());
            var client = TestCommon.GetClient(mockclient.Object);
            client.ReadSummaryByKey("key1", new DateTime(2012, 3, 27), new DateTime(2012, 3, 28));

            Expression<Func<RestRequest, bool>> assertion = req => req.Method == Method.GET;
            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void RequestStartTime()
        {
            var mockclient = TestCommon.GetMockRestClient<Summary>(new Summary());
            var client = TestCommon.GetClient(mockclient.Object);
            var start = new DateTime(2012, 6, 23);
            var end = new DateTime(2012, 6, 24);

            client.ReadSummaryByKey("testkey", start, end);

            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-06-23T00:00:00.000-05:00");
            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void RequestEndTime()
        {
            var mockclient = TestCommon.GetMockRestClient<Summary>(new Summary());
            var client = TestCommon.GetClient(mockclient.Object);
            var start = new DateTime(2012, 6, 23);
            var end = new DateTime(2012, 6, 24);

            client.ReadSummaryByKey("testkey", start, end);

            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-06-24T00:00:00.000-05:00");
            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void RequestUrl()
        {
            var mockclient = TestCommon.GetMockRestClient<Summary>(new Summary());
            var client = TestCommon.GetClient(mockclient.Object);

            client.ReadSummaryByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24));

            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(req => req.Resource == "/{version}/series/{property}/{value}/data/summary/")));
            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "key"))));
            mockclient.Verify(cl => cl.Execute<Summary>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "testkey"))));
        }
    }
}
