using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    class SingleValueTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private static DataPoint dp1 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
        private static DataPoint dp2 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
        private static DataPoint dp3 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 2, 1)), 10.34);

        private static Series s1 = new Series("key1");
        private static Series s2 = new Series("key1");
        private static Series s3 = new Series("key2");

        [Test]
        public void Equality()
        {
            var sv1 = new SingleValue(s1, dp1);
            var sv2 = new SingleValue(s2, dp2);
            Assert.AreEqual(sv1, sv2);
        }

        [Test]
        public void Inequality_Series()
        {
            var sv1 = new SingleValue(s1, dp1);
            var sv2 = new SingleValue(s3, dp2);
            Assert.AreNotEqual(sv1, sv2);
        }

        [Test]
        public void Unequality_Value()
        {
            var sv1 = new SingleValue(s1, dp1);
            var sv2 = new SingleValue(s2, dp3);
            Assert.AreNotEqual(sv1, sv2);
        }

        [Test]
        public void Unequality_Null()
        {
            var sv1 = new SingleValue(s1, dp1);
            Assert.AreNotEqual(sv1, null);
        }
    }
}
