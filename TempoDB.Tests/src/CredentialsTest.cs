using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class CredentialsTest
    {
        [Test]
        public void Equality()
        {
            var c1 = new Credentials("key1", "secret1");
            var c2 = new Credentials("key1", "secret1");
            Assert.AreEqual(c1, c2);
        }

        [Test]
        public void Inequality_Key()
        {
            var c1 = new Credentials("key1", "secret1");
            var c2 = new Credentials("key2", "secret1");
            Assert.AreNotEqual(c1, c2);
        }

        [Test]
        public void Inequality_Secret()
        {
            var c1 = new Credentials("key1", "secret1");
            var c2 = new Credentials("key1", "secret2");
            Assert.AreNotEqual(c1, c2);
        }

        [Test]
        public void InequalityNull()
        {
            var c1 = new Credentials("key1", "secret1");
            Assert.AreNotEqual(c1, null);
        }
    }
}
