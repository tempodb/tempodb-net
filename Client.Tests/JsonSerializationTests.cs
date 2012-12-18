using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Model;
using MbUnit.Framework;
using Moq;
using RestSharp;

namespace Client.Tests
{
    [TestFixture]
    public class JsonSerializationTests
    {
        public static JsonSerializer serializer = new JsonSerializer();

        [TestFixture]
        public class DataPointTests
        {
            [Test]
            public void SmokeTest()
            {
                var dp = new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1), 12.34);
                var result = JsonSerializationTests.serializer.Serialize(dp);
                Assert.AreEqual("{\"t\":\"2012-01-01T00:00:01.000-08:00\",\"v\":12.34}", result);
            }

            [Test]
            public void EmptyValue()
            {
                var dp = new DataPoint();
                var result = JsonSerializationTests.serializer.Serialize(dp);

                Assert.Contains(result, ":0.0");
            }
        }

        [TestFixture]
        public class SeriesTests
        {
            [Test]
            public void SmokeTest()
            {
                var series = new Series
                {
                    Id = "series-id",
                    Key = "series-key"
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":null,\"attributes\":null,\"tags\":null}", result);
            }

            [Test]
            public void Name()
            {
                var series = new Series
                {
                    Id = "series-id",
                    Key = "series-key",
                    Name = "series-name"
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":\"series-name\",\"attributes\":null,\"tags\":null}", result);
            }

            [Test]
            public void Tags()
            {
                Series series = new Series
                {
                    Id = "series-id",
                    Key = "series-key",
                    Tags = new List<string>{ "tag1", "tag2" }
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":null,\"attributes\":null,\"tags\":[\"tag1\",\"tag2\"]}", result);
            }

            [Test]
            public void Attributes()
            {
                Series series = new Series
                {
                    Id = "series-id",
                    Key = "series-key",
                    Attributes = new Dictionary<string,string>{ { "key1", "val1" }, { "key2", "val2" } }
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":null,\"attributes\":{\"key1\":\"val1\",\"key2\":\"val2\"},\"tags\":null}", result);
            }

            [Test]
            public void EmptyTags()
            {
                Series series = new Series
                {
                    Id = "series-id",
                    Key = "series-key",
                    Tags = new List<string>()
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":null,\"attributes\":null,\"tags\":[]}", result);
            }

            [Test]
            public void EmptyAttributes()
            {
                Series series = new Series
                {
                    Id = "series-id",
                    Key = "series-key",
                    Attributes = new Dictionary<string, string>()
                };

                var result = JsonSerializationTests.serializer.Serialize(series);
                Assert.AreEqual("{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":null,\"attributes\":{},\"tags\":null}", result);
            }
        }

        [TestFixture]
        public class BulkIdTests
        {
            [Test]
            public void SmokeTest()
            {
                BulkPoint bdp = new BulkIdPoint("point-id", 12.34);
                var result = JsonSerializationTests.serializer.Serialize(bdp);

                Assert.AreEqual("{\"id\":\"point-id\",\"v\":12.34}", result);
            }
        }

        [TestFixture]
        public class BulkKeyTests
        {
            [Test]
            public void SmokeTest()
            {
                BulkPoint bdp = new BulkKeyPoint("point-key", 12.34);
                var result = JsonSerializationTests.serializer.Serialize(bdp);

                Assert.AreEqual("{\"key\":\"point-key\",\"v\":12.34}", result);
            }
        }

        [TestFixture]
        public class BulkDataSetTests
        {
            [Test]
            public void SmokeTest()
            {
                var data = new List<BulkPoint>
                {
                    new BulkIdPoint("id1", 12.34),
                    new BulkKeyPoint("mykey", 56.78),
                    new BulkIdPoint("id2", 90.12)
                };
                var bds = new BulkDataSet(new DateTime(2012, 1, 1), data);
                var result = JsonSerializationTests.serializer.Serialize(bds);

                Assert.AreEqual("{\"t\":\"2012-01-01T00:00:00.000-08:00\",\"data\":[{\"id\":\"id1\",\"v\":12.34},{\"key\":\"mykey\",\"v\":56.78},{\"id\":\"id2\",\"v\":90.12}]}", result);                    
            }
        }
    }
}
