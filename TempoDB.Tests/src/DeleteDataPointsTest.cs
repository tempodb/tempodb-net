using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class DeleteRequestTests
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private static ZonedDateTime start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
        private static ZonedDateTime end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
        private static Interval interval = new Interval(start.ToInstant(), end.ToInstant());
        private static Series series = new Series("key1");

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.DeleteDataPoints(series, interval);
            var expected = new Response<Nothing>(new Nothing(), 200);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.DeleteDataPoints(series, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.DELETE)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.DeleteDataPoints(series, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/data/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, "");
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.DeleteDataPoints(series, interval);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
        }
    }
}
