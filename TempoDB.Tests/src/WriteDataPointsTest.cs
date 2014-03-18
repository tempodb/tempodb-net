using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class WriteDataPointsTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private static WriteRequest wr = new WriteRequest()
            .Add(new Series("key1"), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 0, 0, 0)), 12.34))
            .Add(new Series("key2"), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 1, 0, 0)), 23.45));

        private static string json = @"[" +
            @"{""key"":""key1"",""t"":""2012-03-27T05:00:00+00:00"",""v"":12.34}," +
            @"{""key"":""key2"",""t"":""2012-03-27T05:01:00+00:00"",""v"":23.45}" +
        @"]";
        private static string multistatus_json = @"{""multistatus"":[{""status"":403,""messages"":[""Forbidden""]}]}";

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.WriteDataPoints(wr);
            var expected = new Response<Nothing>(new Nothing(), 200);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.WriteDataPoints(wr);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.WriteDataPoints(wr);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/multi/")));
        }

        [Test]
        public void RequestBody()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.WriteDataPoints(wr);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "application/json", json))));
        }

        [Test]
        public void MultiStatus()
        {
            var response = TestCommon.GetResponse(207, multistatus_json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.WriteDataPoints(wr);
            var multistatus = new MultiStatus(new List<Status> { new Status(403, new List<string> { "Forbidden" }) });
            var expected = new Response<Nothing>(null, 207, "", multistatus);
            Assert.AreEqual(expected, result);
        }
    }
}
