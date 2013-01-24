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
}
