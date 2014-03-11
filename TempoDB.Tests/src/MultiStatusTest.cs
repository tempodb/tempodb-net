using NodaTime;
using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    [TestFixture]
    class MultiStatusTest
    {
        static List<Status> statuses1 = new List<Status> { new Status(1, new List<string> { "message" }) };
        static List<Status> statuses2 = new List<Status> { new Status(1, new List<string> { "message" }) };
        static List<Status> statuses3 = new List<Status> { new Status(100, new List<string> { "message" }) };

        [Test]
        public void Equality()
        {
            var ms1 = new MultiStatus(statuses1);
            var ms2 = new MultiStatus(statuses2);
            Assert.AreEqual(ms1, ms2);
        }

        [Test]
        public void Equality_EmptyStatuses()
        {
            var ms1 = new MultiStatus(new List<Status>());
            var ms2 = new MultiStatus(new List<Status>());
            Assert.AreEqual(ms1, ms2);
        }

        [Test]
        public void Inequality()
        {
            var ms1 = new MultiStatus(statuses1);
            var ms2 = new MultiStatus(statuses3);
            Assert.AreNotEqual(ms1, ms2);
        }

        [Test]
        public void Inequality_Null()
        {
            var ms1 = new MultiStatus(statuses1);
            Assert.AreNotEqual(ms1, null);
        }
    }
}
