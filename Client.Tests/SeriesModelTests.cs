using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Model;
using MbUnit.Framework;

namespace Client.Tests
{
    [TestFixture]
    class SeriesModelTests
    {
        [Test]
        public void Equality()
        {
            var s1 = new Series
            {
                Id = "my-id",
                Key = "my-key"
            };
            var s2 = new Series
            {
                Id = "my-id",
                Key = "my-key"
            };

            Assert.AreEqual(s1, s2);
        }

        [Test]
        public void Equality_WithMetaData()
        {
            var s1 = new Series
            {
                Id = "my-id",
                Key = "my-key",
                Tags = new List<string> { "tag1", "tag2" }
            };
            var s2 = new Series
            {
                Id = "my-id",
                Key = "my-key",
                Attributes = new Dictionary<string,string> { { "key", "val" } }
            };

            Assert.AreEqual(s1, s2);
        }

        [Test]
        public void Unequality_Id()
        {
            var s1 = new Series
            {
                Id = "my-id",
                Key = "my-key"
            };
            var s2 = new Series
            {
                Id = "my-other-id",
                Key = "my-key"
            };

            Assert.AreNotEqual(s1, s2);
        }
        
        [Test]
        public void Unequality_Key()
        {
            var s1 = new Series
            {
                Id = "my-id",
                Key = "my-key"
            };
            var s2 = new Series
            {
                Id = "my-id",
                Key = "my-other-key"
            };

            Assert.AreNotEqual(s1, s2);
        }
    }
}
