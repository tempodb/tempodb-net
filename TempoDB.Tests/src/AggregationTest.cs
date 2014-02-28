using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class AggregationTest
    {
        [Test]
        public void Equality()
        {
            var aggregation1 = new Aggregation(Fold.Sum);
            var aggregation2 = new Aggregation(Fold.Sum);
            Assert.AreEqual(aggregation1, aggregation2);
        }

        [Test]
        public void NotEquals()
        {
            var aggregation1 = new Aggregation(Fold.Sum);
            var aggregation2 = new Aggregation(Fold.Mean);
            Assert.AreNotEqual(aggregation1, aggregation2);
        }
    }
}
