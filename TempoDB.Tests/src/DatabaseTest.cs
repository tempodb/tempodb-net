using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class DatabaseTest
    {
        [Test]
        public void Equality()
        {
            var d1 = new Database("id1");
            var d2 = new Database("id1");
            Assert.AreEqual(d1, d2);
        }

        [Test]
        public void Inequality()
        {
            var d1 = new Database("id1");
            var d2 = new Database("id2");
            Assert.AreNotEqual(d1, d2);
        }

        [Test]
        public void InequalityNull()
        {
            var d1 = new Database("id1");
            Assert.AreNotEqual(d1, null);
        }
    }
}
