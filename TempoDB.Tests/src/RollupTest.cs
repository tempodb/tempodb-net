using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class RollupTest
    {
        [Test]
        public void Defaults()
        {
            var period = Period.FromMinutes(1);
            var rollup = new Rollup(period, Fold.Sum);
            Assert.AreEqual(Fold.Sum, rollup.Fold);
            Assert.AreEqual(period, rollup.Period);
        }

        [Test]
        public void Equality()
        {
            var rollup1 = new Rollup(Period.FromMinutes(1), Fold.Sum);
            var rollup2 = new Rollup(Period.FromMinutes(1), Fold.Sum);
            Assert.AreEqual(rollup1, rollup2);
        }
    }
}
