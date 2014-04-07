using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    public class MultiRollupTest
    {
        [Test]
        public void UtcTest()
        {
            string json = "{" +
                "\"period\":\"PT1H\"," +
                "\"folds\":[\"max\",\"sum\"]" +
            "}";
            var foldConverter = new FoldConverter();
            var periodConverter = new PeriodConverter();

            var rollup = JsonConvert.DeserializeObject<MultiRollup>(json, foldConverter, periodConverter);
            var expected = new MultiRollup(Period.FromHours(1), new Fold[] { Fold.Max, Fold.Sum });
            Assert.AreEqual(expected, rollup);
        }
    }
}
