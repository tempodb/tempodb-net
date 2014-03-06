using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    class MultiDataPointTest
    {
        static DateTimeZone zone = DateTimeZone.Utc;
        static Series s1 = new Series("key1");
        static Series s2 = new Series("key1");
        static Series s3 = new Series("key2");

        static ZonedDateTime t1 = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));
        static ZonedDateTime t2 = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1));
        static ZonedDateTime t3 = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 1, 0, 1));

        [Test]
        public void Equality()
        {
            var mdp1 = new MultiDataPoint(s1, t1, 12.34);
            var mdp2 = new MultiDataPoint(s2, t2, 12.34);
            Assert.AreEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Series()
        {
            var mdp1 = new MultiDataPoint(s1, t1, 12.34);
            var mdp2 = new MultiDataPoint(s3, t2, 12.34);
            Assert.AreNotEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Timestamp()
        {
            var mdp1 = new MultiDataPoint(s1, t1, 12.34);
            var mdp2 = new MultiDataPoint(s2, t3, 12.34);
            Assert.AreNotEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Value()
        {
            var mdp1 = new MultiDataPoint(s1, t1, 12.34);
            var mdp2 = new MultiDataPoint(s2, t2, 23.45);
            Assert.AreNotEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Null()
        {
            var mdp1 = new MultiDataPoint(s1, t1, 12.34);
            Assert.AreNotEqual(mdp1, null);
        }
    }
}
