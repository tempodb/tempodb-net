using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    class DeleteSummaryTest
    {
        [Test]
        public void Equality()
        {
            var d1 = new DeleteSummary(1);
            var d2 = new DeleteSummary(1);
            Assert.AreEqual(d1, d2);
        }

        [Test]
        public void Inequality()
        {
            var d1 = new DeleteSummary(1);
            var d2 = new DeleteSummary(2);
            Assert.AreNotEqual(d1, d2);
        }

        [Test]
        public void Inequality_Null()
        {
            var d1 = new DeleteSummary(1);
            Assert.AreNotEqual(d1, null);
        }
    }
}
