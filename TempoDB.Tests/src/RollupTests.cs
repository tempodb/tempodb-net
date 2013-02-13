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
        }

        [Test]
        public void Equality()
        {
            var rollup1 = new Rollup("sum", Period.FromMinutes(1));
            var rollup2 = new Rollup("sum", Period.FromMinutes(1));
            Assert.AreEqual(rollup1, rollup2);
        }
    }
}
