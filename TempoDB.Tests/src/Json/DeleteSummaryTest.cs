using Newtonsoft.Json;
using NUnit.Framework;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    class DeleteSummaryTest
    {
        [Test]
        public void Deserialize()
        {
            var json = @"{""deleted"":1}";
            var expected = new DeleteSummary(1);
            var summary = JsonConvert.DeserializeObject<DeleteSummary>(json);
            Assert.AreEqual(expected, summary);
        }

        [Test]
        public void Serialize()
        {
            var summary = new DeleteSummary(1);
            var expected = @"{""deleted"":1}";
            var json = JsonConvert.SerializeObject(summary);
            Assert.AreEqual(expected, json);
        }
    }
}
