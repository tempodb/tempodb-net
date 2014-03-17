using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    [TestFixture]
    public class ResponseTest
    {
        private Series series1 = new Series("key1");
        private Series series2 = new Series("key1");
        private Series series3 = new Series("key2");

        [Test]
        public void Constructor()
        {
            var response = new Response<Series>(series1, 200, "message");
            Assert.AreEqual(series1, response.Value);
            Assert.AreEqual(200, response.Code);
            Assert.AreEqual("message", response.Message);
        }

        [Test]
        public void Equality()
        {
            var r1 = new Response<Series>(series1, 200, "message");
            var r2 = new Response<Series>(series2, 200, "message");
            Assert.AreEqual(r1, r2);
        }

        [Test]
        public void Inequality()
        {
            var r1 = new Response<Series>(series1, 200, "message");
            var r2 = new Response<Series>(series3, 200, "message");
            var r3 = new Response<Series>(series1, 100, "message");
            var r4 = new Response<Series>(series1, 200, "message1");
            Assert.AreNotEqual(r1, r2);
            Assert.AreNotEqual(r1, r3);
            Assert.AreNotEqual(r1, r4);
        }

        [Test]
        public void SuccessfulRequest()
        {
            var response = TestCommon.GetResponse(200, "");
            var result = new Response<Nothing>(response);
            var expected = new Response<Nothing>(new Nothing(), 200, "", null);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(result.State, State.Success);
        }

        [Test]
        public void FailedRequest_Body()
        {
            var response = TestCommon.GetResponse(403, "You are forbidden");
            var result = new Response<Nothing>(response);
            var expected = new Response<Nothing>(null, 403, "You are forbidden", null);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(result.State, State.Failure);
        }

        [Test]
        public void PartialSuccess()
        {
            string json = @"{""multistatus"":[{""status"":403,""messages"":[""Forbidden""]}]}";
            var response = TestCommon.GetResponse(207, json);
            var result = new Response<Nothing>(response);

            MultiStatus multistatus = new MultiStatus(new List<Status> { new Status(403, new List<string> { "Forbidden" }) });
            var expected = new Response<Nothing>(null, 207, "", multistatus);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(result.State, State.PartialSuccess);
        }
    }
}
