using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class MultiRollupTest
    {
        private static Period p1 = Period.FromMinutes(1);
        private static Period p2 = Period.FromMinutes(1);
        private static Period p3 = Period.FromMinutes(2);

        private static Fold[] f1 = new Fold[] { Fold.Sum, Fold.Mean };
        private static Fold[] f2 = new Fold[] { Fold.Mean, Fold.Sum };
        private static Fold[] f3 = new Fold[] { Fold.Sum };

        [Test]
        public void Equality()
        {
            var rollup1 = new MultiRollup(p1, f1);
            var rollup2 = new MultiRollup(p2, f2);
            Assert.AreEqual(rollup1, rollup2);
        }

        [Test]
        public void Inequality_Period()
        {
            var rollup1 = new MultiRollup(p1, f1);
            var rollup2 = new MultiRollup(p3, f2);
            Assert.AreNotEqual(rollup1, rollup2);
        }

        [Test]
        public void Inequality_Folds()
        {
            var rollup1 = new MultiRollup(p1, f1);
            var rollup2 = new MultiRollup(p2, f3);
            Assert.AreNotEqual(rollup1, rollup2);
        }

        [Test]
        public void Inequality_Null()
        {
            var rollup1 = new MultiRollup(p1, f1);
            Assert.AreNotEqual(rollup1, null);
        }
    }
}
