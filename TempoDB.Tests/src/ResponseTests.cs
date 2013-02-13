using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void Constructor()
        {
            var series = new Series("id1", "key1");
            var response = new Response<Series>(series, 201);
            Assert.AreEqual(series, response.Value);
            Assert.AreEqual(true, response.Success);
            Assert.AreEqual("", response.Message);
        }
    }
}
