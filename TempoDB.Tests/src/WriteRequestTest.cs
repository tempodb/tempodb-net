using NodaTime;
using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    [TestFixture]
    class WriteRequestTest
    {
        static DateTimeZone zone = DateTimeZone.Utc;
        static Series series1 = new Series("key1");
        static Series series2 = new Series("key1");
        static Series series3 = new Series("key2");

        static DataPoint dp1 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
        static DataPoint dp2 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), 12.34);
        static DataPoint dp3 = new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 1, 0, 1)), 23.45);

        [Test]
        public void Equality()
        {
            var wr1 = new WriteRequest().Add(series1, dp1);
            var wr2 = new WriteRequest().Add(series2, dp2);
            Assert.AreEqual(wr1, wr2);
        }

        [Test]
        public void Inequality_Series()
        {
            var wr1 = new WriteRequest().Add(series1, dp1);
            var wr2 = new WriteRequest().Add(series3, dp2);
            Assert.AreNotEqual(wr1, wr2);
        }

        [Test]
        public void Inequality_DataPoint()
        {
            var wr1 = new WriteRequest().Add(series1, dp1);
            var wr2 = new WriteRequest().Add(series2, dp3);
            Assert.AreNotEqual(wr1, wr2);
        }

        [Test]
        public void Inequality_Null()
        {
            var wr1 = new WriteRequest().Add(series1, dp1);
            Assert.AreNotEqual(wr1, null);
        }

        [Test]
        public void AddMultiple()
        {
            var dps = new List<DataPoint>();
            dps.Add(dp2);
            dps.Add(dp3);

            WriteRequest wr1 = new WriteRequest().Add(series1, dps);
            WriteRequest wr2 = new WriteRequest().Add(series1, dp2).Add(series1, dp3);
            Assert.AreEqual(wr1, wr2);
        }
    }
}
