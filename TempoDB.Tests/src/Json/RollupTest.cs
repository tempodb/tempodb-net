using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    public class RollupTest
    {
        class Deserialize
        {
            [Test]
            public void BasicRollup()
            {
                var json = @"{
                    ""fold"":""sum"",
                    ""period"":""PT1H""
                }";
                var foldConverter = new FoldConverter();
                var periodConverter = new PeriodConverter();
                var rollup = JsonConvert.DeserializeObject<Rollup>(json, foldConverter, periodConverter);
                var expected = new Rollup(Fold.Sum, Period.FromHours(1));
                Assert.AreEqual(expected, rollup);
            }

            [Test]
            public void NonUtc()
            {
                var json = @"{
                    ""fold"":""sum"",
                    ""period"":""PT1M""
                }";
                var foldConverter = new FoldConverter();
                var periodConverter = new PeriodConverter();
                var rollup = JsonConvert.DeserializeObject<Rollup>(json, foldConverter, periodConverter);
                var expected = new Rollup(Fold.Sum, Period.FromMinutes(1));
                Assert.AreEqual(expected, rollup);
            }
        }

        class Serialize
        {
            [Test]
            public void DefaultTz()
            {
                var rollup = new Rollup(Fold.Mean, Period.FromMinutes(1));
                var foldConverter = new FoldConverter();
                var periodConverter = new PeriodConverter();
                var json = JsonConvert.SerializeObject(rollup, foldConverter, periodConverter);
                var expected = @"{""fold"":""mean"",""period"":""PT1M""}";
                Assert.AreEqual(expected, json);
            }

            [Test]
            public void NonUtc()
            {
                var rollup = new Rollup(Fold.Mean, Period.FromHours(1));
                var foldConverter = new FoldConverter();
                var periodConverter = new PeriodConverter();
                var json = JsonConvert.SerializeObject(rollup, foldConverter, periodConverter);
                var expected = @"{""fold"":""mean"",""period"":""PT1H""}";
                Assert.AreEqual(expected, json);
            }
        }
    }
}
