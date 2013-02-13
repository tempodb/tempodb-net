using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    class Deserialize
    {
        [Test]
        public void BasicRollup()
        {
            var json = @"{
                ""function"":""sum"",
                ""interval"":""PT1H""
            }";
            var periodConverter = new PeriodConverter();
            var rollup = JsonConvert.DeserializeObject<Rollup>(json, periodConverter);
            var expected = new Rollup("sum", Period.FromHours(1));
            Assert.AreEqual(expected, rollup);
        }

        [Test]
        public void NonUtc()
        {
            var json = @"{
                ""function"":""sum"",
                ""interval"":""PT1M""
            }";
            var periodConverter = new PeriodConverter();
            var zoneConverter = new DateTimeZoneConverter();
            var rollup = JsonConvert.DeserializeObject<Rollup>(json, periodConverter);
            var expected = new Rollup("sum", Period.FromMinutes(1));
            Assert.AreEqual(expected, rollup);
        }
    }

    class Serialize
    {
        [Test]
        public void DefaultTz()
        {
            var rollup = new Rollup("mean", Period.FromMinutes(1));
            var periodConverter = new PeriodConverter();
            var json = JsonConvert.SerializeObject(rollup, periodConverter);
            var expected = @"{""function"":""mean"",""interval"":""PT1M""}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void NonUtc()
        {
            var rollup = new Rollup("mean", Period.FromHours(1));
            var periodConverter = new PeriodConverter();
            var json = JsonConvert.SerializeObject(rollup, periodConverter);
            var expected = @"{""function"":""mean"",""interval"":""PT1H""}";
            Assert.AreEqual(expected, json);
        }
    }
}
