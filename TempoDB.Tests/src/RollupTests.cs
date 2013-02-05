using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class RollupTests
    {
        [Test]
        public void Defaults()
        {
            var period = Period.FromMinutes(1);
            var rollup = new Rollup("sum", period);
            Assert.AreEqual("sum", rollup.Fold);
            Assert.AreEqual(period, rollup.Period);
            Assert.AreEqual(DateTimeZone.Utc, rollup.Zone);
        }

        [Test]
        public void Full()
        {
            var period = Period.FromMinutes(1);
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var rollup = new Rollup("sum", period, zone);
            Assert.AreEqual("sum", rollup.Fold);
            Assert.AreEqual(period, rollup.Period);
            Assert.AreEqual(zone, rollup.Zone);
        }

        [Test]
        public void Equality()
        {
            var rollup1 = new Rollup("sum", Period.FromMinutes(1), DateTimeZoneProviders.Tzdb["America/Chicago"]);
            var rollup2 = new Rollup("sum", Period.FromMinutes(1), DateTimeZoneProviders.Tzdb["America/Chicago"]);
            Assert.AreEqual(rollup1, rollup2);
        }
    }
}
