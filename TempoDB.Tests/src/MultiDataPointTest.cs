using NodaTime;
using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    [TestFixture]
    class MultiDataPointTest
    {
        DateTimeZone zone = DateTimeZone.Utc;

        [Test]
        public void Equality()
        {
            var data1 = new Dictionary<string, double> {
                {"key1", 12.34},
                {"key2", 23.45}
            };
            var data2 = new Dictionary<string, double> {
                {"key2", 23.45},
                {"key1", 12.34}
            };
            var mdp1 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data1);
            var mdp2 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data2);
            Assert.AreEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Timestamp()
        {
            var data1 = new Dictionary<string, double> {
                {"key1", 12.34},
                {"key2", 23.45}
            };
            var data2 = new Dictionary<string, double> {
                {"key2", 23.45},
                {"key1", 12.34}
            };
            var mdp1 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data1);
            var mdp2 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 1)), data2);
            Assert.AreNotEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Data()
        {
            var data1 = new Dictionary<string, double> {
                {"key1", 12.34},
                {"key2", 23.45}
            };
            var data2 = new Dictionary<string, double> {
                {"key2", 23.45},
                {"key1", 34.56}
            };
            var mdp1 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data1);
            var mdp2 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data2);
            Assert.AreNotEqual(mdp1, mdp2);
        }

        [Test]
        public void Inequality_Null()
        {
            var data1 = new Dictionary<string, double> {
                {"key1", 12.34},
                {"key2", 23.45}
            };
            var mdp1 = new MultiDataPoint(zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 1)), data1);
            Assert.AreNotEqual(mdp1, null);
        }
    }
}
