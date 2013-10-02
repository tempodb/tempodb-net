using Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Client.Tests
{
    [TestFixture]
    class DataPointModelTests
    {
        [Test]
        public void Unequality_Timestamp()
        {
            var dp1 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);
            var dp2 = new DataPoint(new DateTime(2012, 1, 1, 0, 2, 0), 12.34);

            Assert.AreNotEqual(dp1, dp2);
        }

        [Test]
        public void Equality()
        {
            var dp1 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);
            var dp2 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);

            Assert.AreEqual(dp1, dp2);
        }

        [Test]
        public void Unequality_Value()
        {
            var dp1 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);
            var dp2 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 10.34);

            Assert.AreNotEqual(dp1, dp2);
        }

        [Test]
        public void Unequality_Null()
        {
            var dp1 = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);

            Assert.AreNotEqual(dp1, null);
        }

        [Test]
        public void Equality_Null()
        {
            var dp1 = new DataPoint();

            Assert.AreEqual(false, dp1.Equals(null));
        }
    }

    [TestFixture]
    class MultiPointModelTests
    {
        [Test]
        public void Double_Or_Long()
        {
          var dp1 = new MultiIdPoint("id1", new DateTime(2013, 9, 25, 0, 0, 1), 12.34);
          var dp2 = new MultiIdPoint("id2", new DateTime(2013, 9, 25, 0, 0, 1), 1000L);

          Assert.AreEqual(dp1.Value, 12.34);
          Assert.AreEqual(dp1.Value is double, true);
          Assert.AreEqual(dp2.Value, 1000L);
          Assert.AreEqual(dp2.Value is long, true);
        }

        public void Multi_List()
        {
          var dp1 = new MultiIdPoint("id1", new DateTime(2013, 9, 25, 0, 0, 1), 12.24);
          var dp2 = new MultiKeyPoint("key1", new DateTime(2013, 9, 25, 0, 0, 1), 1000L);

          var list = new List<MultiPoint>{ dp1, dp2 };
          Assert.AreEqual(list.Count, 2);
        }
    }
}
