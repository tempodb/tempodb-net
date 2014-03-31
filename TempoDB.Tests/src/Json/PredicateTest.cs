using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NUnit.Framework;
using System;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    public class PredicateTest
    {
        [Test]
        public void Deserialize()
        {
            var json = @"{
                ""function"":""max"",
                ""period"":""PT1H""
            }";
            var periodConverter = new PeriodConverter();
            var predicate = JsonConvert.DeserializeObject<Predicate>(json, periodConverter);
            var expected = new Predicate(Period.FromHours(1), "max");
            Assert.AreEqual(expected, predicate);
        }

        [Test]
        public void Serialize()
        {
            var predicate = new Predicate(Period.FromMinutes(1), "max");
            var periodConverter = new PeriodConverter();
            var json = JsonConvert.SerializeObject(predicate, periodConverter);
            var expected = @"{""function"":""max"",""period"":""PT1M""}";
            Assert.AreEqual(expected, json);
        }
    }
}
