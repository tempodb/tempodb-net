using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    class DataPointFoundTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;

        private static ZonedDateTime start = zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
        private static ZonedDateTime end1 = zone.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0));
        private static ZonedDateTime end2 = zone.AtStrictly(new LocalDateTime(2012, 3, 29, 0, 0, 0));
        private static Interval interval1 = new Interval(start.ToInstant(), end1.ToInstant());
        private static Interval interval2 = new Interval(start.ToInstant(), end1.ToInstant());
        private static Interval interval3 = new Interval(start.ToInstant(), end2.ToInstant());

        private static DataPoint dp1 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0)), 12.34);
        private static DataPoint dp2 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0)), 12.34);
        private static DataPoint dp3 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 1, 0)), 10.34);

        [Test]
        public void Equality()
        {
            var dpf1 = new DataPointFound(interval1, dp1);
            var dpf2 = new DataPointFound(interval2, dp2);
            Assert.AreEqual(dpf1, dpf2);
        }

        [Test]
        public void Inequality_Interval()
        {
            var dpf1 = new DataPointFound(interval1, dp1);
            var dpf2 = new DataPointFound(interval3, dp2);
            Assert.AreNotEqual(dpf1, dpf2);
        }

        [Test]
        public void Inequality_DataPoint()
        {
            var dpf1 = new DataPointFound(interval1, dp1);
            var dpf2 = new DataPointFound(interval2, dp3);
            Assert.AreNotEqual(dpf1, dpf2);
        }

        [Test]
        public void Inequality_Null()
        {
            var dpf1 = new DataPointFound(interval1, dp1);
            Assert.AreNotEqual(dpf1, null);
        }
    }
}
