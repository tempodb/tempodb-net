using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB.Json
{
    [TestFixture]
    public class SeriesTests
    {
        class Deserialize
        {
            [Test]
            public void BasicSeries()
            {
                var json = @"{
                    ""id"":""id1"",
                    ""key"":""key1"",
                    ""name"":""name1"",
                    ""tags"":[],
                    ""attributes"":{}
                }";
                var series = JsonConvert.DeserializeObject<Series>(json);
                var expected = new Series("id1", "key1", "name1");
                Assert.AreEqual(expected, series);
            }

            [Test]
            public void WithTags()
            {
                var json = @"{
                    ""id"":""id1"",
                    ""key"":""key1"",
                    ""name"":""name1"",
                    ""tags"":[""tag1"",""tag2""],
                    ""attributes"":{}
                }";
                var series = JsonConvert.DeserializeObject<Series>(json);
                var expected = new Series("id1", "key1", "name1", new HashSet<string> { "tag2", "tag1" });
                Assert.AreEqual(expected, series);
            }

            [Test]
            public void WithAttributes()
            {
                var json = @"{
                    ""id"":""id1"",
                    ""key"":""key1"",
                    ""name"":""name1"",
                    ""tags"":[],
                    ""attributes"":{""key1"":""value1"",""key2"":""value2""}
                }";
                var series = JsonConvert.DeserializeObject<Series>(json);
                var attributes = new Dictionary<string, string> { { "key2", "value2" }, { "key1", "value1" } };
                var expected = new Series("id1", "key1", "name1", new HashSet<string>(), attributes);
                Assert.AreEqual(expected, series);
            }

            [Test]
            public void FullSeries()
            {
                var json = @"{
                    ""id"":""id1"",
                    ""key"":""key1"",
                    ""name"":""name1"",
                    ""tags"":[""tag1"",""tag2"",""tag3"",""tag4""],
                    ""attributes"":{""key1"":""value1"",""key2"":""value2""}
                }";
                var series = JsonConvert.DeserializeObject<Series>(json);
                var tags = new HashSet<string> { "tag1", "tag2", "tag3", "tag4" };
                var attributes = new Dictionary<string, string> { { "key2", "value2" }, { "key1", "value1" } };
                var expected = new Series("id1", "key1", "name1", tags, attributes);
                Assert.AreEqual(expected, series);
            }

            [Test]
            public void NoId()
            {
                var json = @"{
                    ""key"":""key1"",
                    ""name"":""name1"",
                    ""tags"":[],
                    ""attributes"":{}
                }";
                var series = JsonConvert.DeserializeObject<Series>(json);
                var expected = new Series("id1", "key1", "name1");
                Assert.AreEqual(expected, series);
            }
       }

        class Serialize
        {
            [Test]
            public void BasicSeries()
            {
                var series = new Series("id1", "key1", "name1");
                var json = JsonConvert.SerializeObject(series);
                var expected = @"{""id"":""id1"",""key"":""key1"",""name"":""name1"",""tags"":[],""attributes"":{}}";
                Assert.AreEqual(expected, json);
            }

            [Test]
            public void WithTags()
            {
                var tags = new HashSet<string> { "tag1", "tag2" };
                var series = new Series("id1", "key1", "name1", tags);
                var json = JsonConvert.SerializeObject(series);
                var expected = @"{""id"":""id1"",""key"":""key1"",""name"":""name1"",""tags"":[""tag1"",""tag2""],""attributes"":{}}";
                Assert.AreEqual(expected, json);
            }

            [Test]
            public void WithAttributes()
            {
                var tags = new HashSet<string>();
                var attributes = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
                var series = new Series("id1", "key1", "name1", tags, attributes);
                var json = JsonConvert.SerializeObject(series);
                var expected = @"{""id"":""id1"",""key"":""key1"",""name"":""name1"",""tags"":[],""attributes"":{""key1"":""value1"",""key2"":""value2""}}";
                Assert.AreEqual(expected, json);
            }

            [Test]
            public void FullSeries()
            {
                var tags = new HashSet<string> { "tag1", "tag2", "tag3", "tag4" };
                var attributes = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
                var series = new Series("id1", "key1", "name1", tags, attributes);
                var json = JsonConvert.SerializeObject(series);
                var expected = @"{""id"":""id1"",""key"":""key1"",""name"":""name1"",""tags"":[""tag1"",""tag2"",""tag3"",""tag4""],""attributes"":{""key1"":""value1"",""key2"":""value2""}}";
                Assert.AreEqual(expected, json);
            }
        }
    }
}
