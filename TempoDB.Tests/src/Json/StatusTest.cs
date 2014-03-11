using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Tests.Json
{
    [TestFixture]
    public class StatusTest
    {
        [Test]
        public void Deserialize()
        {
            var json = @"{""status"":1,""messages"":[""message1""]}";
            var status = JsonConvert.DeserializeObject<Status>(json);
            var expected = new Status(1, new List<string> { "message1" });
            Assert.AreEqual(status, expected);
        }

        [Test]
        public void Deserialize_EmptyMessages()
        {
            var json = @"{""status"":1,""messages"":[]}";
            var status = JsonConvert.DeserializeObject<Status>(json);
            var expected = new Status(1, new List<string>());
            Assert.AreEqual(status, expected);
        }
    }
}
