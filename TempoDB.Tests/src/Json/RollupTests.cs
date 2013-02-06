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
                ""interval"":""PT1H"",
                ""tz"":""UTC""
            }";
            var periodConverter = new PeriodConverter();
            var zoneConverter = new DateTimeZoneConverter();
            var rollup = JsonConvert.DeserializeObject<Rollup>(json, periodConverter, zoneConverter);
            var expected = new Rollup("sum", Period.FromHours(1));
            Assert.AreEqual(expected, rollup);
        }

        [Test]
        public void NonUtc()
        {
            var json = @"{
                ""function"":""sum"",
                ""interval"":""PT1M"",
                ""tz"":""America/Chicago""
            }";
            var periodConverter = new PeriodConverter();
            var zoneConverter = new DateTimeZoneConverter();
            var rollup = JsonConvert.DeserializeObject<Rollup>(json, periodConverter, zoneConverter);
            var expected = new Rollup("sum", Period.FromMinutes(1), DateTimeZoneProviders.Tzdb["America/Chicago"]);
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
            var zoneConverter = new DateTimeZoneConverter();
            var json = JsonConvert.SerializeObject(rollup, periodConverter, zoneConverter);
            var expected = @"{""function"":""mean"",""interval"":""PT1M"",""tz"":""UTC""}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void NonUtc()
        {
            var rollup = new Rollup("mean", Period.FromHours(1), DateTimeZoneProviders.Tzdb["America/Chicago"]);
            var periodConverter = new PeriodConverter();
            var zoneConverter = new DateTimeZoneConverter();
            var json = JsonConvert.SerializeObject(rollup, periodConverter, zoneConverter);
            var expected = @"{""function"":""mean"",""interval"":""PT1H"",""tz"":""America/Chicago""}";
            Assert.AreEqual(expected, json);
        }
    }
}
