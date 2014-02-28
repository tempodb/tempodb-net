using NUnit.Framework;
using System.Collections.Generic;


namespace TempoDB.Tests
{
    public class FilterTest
    {
        [Test]
        public void Equality()
        {
            Filter f1 = new Filter()
                .AddTags("tag")
                .AddAttributes("key", "value");

            Filter f2 = new Filter()
                .AddTags("tag")
                .AddAttributes("key", "value");

            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void testNotEquals_Keys()
        {
            Filter f1 = new Filter();
            Filter f2 = new Filter();
            f1.AddKeys("key1");
            f1.AddKeys("key2");
            Assert.AreNotEqual(f1, f2);
        }

        [Test]
        public void testNotEquals_Tags()
        {
            Filter f1 = new Filter();
            Filter f2 = new Filter();
            f1.AddTags("tag1");
            f1.AddTags("tag2");
            Assert.AreNotEqual(f1, f2);
        }

        [Test]
        public void testNotEquals_Attributes()
        {
            Filter f1 = new Filter();
            Filter f2 = new Filter();
            f1.AddAttributes("key", "value1");
            Assert.AreNotEqual(f1, f2);
        }

        [Test]
        public void testNotEquals_Null()
        {
            Filter f1 = new Filter();
            Assert.AreNotEqual(f1, null);
        }

        [Test]
        public void testAddKeys()
        {
            Filter f1 = new Filter().AddKeys("key1").AddKeys("key2").AddKeys("key3");
            Filter f2 = new Filter().AddKeys("key1").AddKeys("key2", "key3");
            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void testAddKeysSet()
        {
            Filter f1 = new Filter().AddKeys("key1").AddKeys("key2").AddKeys("key3");
            HashSet<string> keys = new HashSet<string>();
            keys.Add("key2");
            keys.Add("key3");
            Filter f2 = new Filter().AddKeys("key1").AddKeys(keys);
            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void testAddTags()
        {
            Filter f1 = new Filter().AddTags("tag1").AddTags("tag2").AddTags("tag3");
            Filter f2 = new Filter().AddTags("tag1").AddTags("tag2", "tag3");
            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void testAddTagsSet()
        {
            Filter f1 = new Filter().AddTags("tag1").AddTags("tag2").AddTags("tag3");
            HashSet<string> tags = new HashSet<string>();
            tags.Add("tag2");
            tags.Add("tag3");
            Filter f2 = new Filter().AddTags("tag1").AddTags(tags);
            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void testAddAttributes()
        {
            Filter f1 = new Filter().AddAttributes("key1", "value1").AddAttributes("key2", "value2").AddAttributes("key3", "value3");
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("key2", "value2");
            attributes.Add("key3", "value3");
            Filter f2 = new Filter().AddAttributes("key1", "value1").AddAttributes(attributes);
            Assert.AreEqual(f1, f2);
        }
    }
}
