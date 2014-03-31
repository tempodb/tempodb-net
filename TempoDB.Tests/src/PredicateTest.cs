using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class PredicateTest
    {

        [Test]
        public void Equality()
        {
            var p1 = new Predicate(Period.FromMinutes(1), "max");
            var p2 = new Predicate(Period.FromMinutes(1), "max");
            Assert.AreEqual(p1, p2);
        }

        [Test]
        public void Inequality_Period()
        {
            var p1 = new Predicate(Period.FromMinutes(1), "max");
            var p2 = new Predicate(Period.FromMinutes(2), "max");
            Assert.AreNotEqual(p1, p2);
        }

        [Test]
        public void Inequality_Function()
        {
            var p1 = new Predicate(Period.FromMinutes(1), "max");
            var p2 = new Predicate(Period.FromMinutes(1), "min");
            Assert.AreNotEqual(p1, p2);
        }

        [Test]
        public void Inequality_Null()
        {
            var p1 = new Predicate(Period.FromMinutes(1), "max");
            Assert.AreNotEqual(p1, null);
        }
    }
}
