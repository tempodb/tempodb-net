using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    [TestFixture]
    class StatusTest
    {
        [Test]
        public void Equality()
        {
            var s1 = new Status(1, new List<string> { "message" });
            var s2 = new Status(1, new List<string> { "message" });
            Assert.AreEqual(s1, s2);
        }

        [Test]
        public void Inequality_Code()
        {
            var s1 = new Status(1, new List<string> { "message" });
            var s2 = new Status(2, new List<string> { "message" });
            Assert.AreNotEqual(s1, s2);
        }

        [Test]
        public void Inequality_Messages()
        {
            var s1 = new Status(1, new List<string> { "message" });
            var s2 = new Status(1, new List<string> { "message1" });
            Assert.AreNotEqual(s1, s2);
        }

        [Test]
        public void Inequality_Null()
        {
            var s1 = new Status(1, new List<string> { "message" });
            Assert.AreNotEqual(s1, null);
        }
    }
}
