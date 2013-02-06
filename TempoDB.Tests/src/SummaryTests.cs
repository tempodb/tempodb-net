using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class SummaryTests
    {
        [Test]
        public void Equality()
        {
            var summary1 = new Summary { { "mean", 2.0 }, { "min", 1.0 } };
            var summary2 = new Summary { { "min", 1.0 }, { "mean", 2.0 } };
            Assert.AreEqual(summary1, summary2);
        }
    }
}
