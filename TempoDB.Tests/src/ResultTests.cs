using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Constructor()
        {
            var series = new Series("id1", "key1");
            var result = new Result<Series>(series, true);
            Assert.AreEqual(series, result.Value);
            Assert.AreEqual(true, result.Success);
        }
    }
}
