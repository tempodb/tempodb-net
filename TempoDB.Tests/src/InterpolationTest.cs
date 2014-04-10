using NodaTime;
using NUnit.Framework;


namespace TempoDB.Tests
{
    [TestFixture]
    public class InterpolationTest
    {
        private static Period p1 = Period.FromMinutes(1);
        private static Period p2 = Period.FromMinutes(1);
        private static Period p3 = Period.FromMinutes(2);

        private static InterpolationFunction f1 = InterpolationFunction.Linear;
        private static InterpolationFunction f2 = InterpolationFunction.Linear;
        private static InterpolationFunction f3 = InterpolationFunction.ZOH;

        [Test]
        public void Equality()
        {
            var interp1 = new Interpolation(p1, f1);
            var interp2 = new Interpolation(p2, f2);
            Assert.AreEqual(interp1, interp2);
        }

        [Test]
        public void Inequality_Period()
        {
            var interp1 = new Interpolation(p1, f1);
            var interp2 = new Interpolation(p3, f2);
            Assert.AreNotEqual(interp1, interp2);
        }

        [Test]
        public void Inequality_Function()
        {
            var interp1 = new Interpolation(p1, f1);
            var interp2 = new Interpolation(p2, f3);
            Assert.AreNotEqual(interp1, interp2);
        }

        [Test]
        public void Inequality_Null()
        {
            var interp1 = new Interpolation(p1, f1);
            Assert.AreNotEqual(interp1, null);
        }
    }
}
