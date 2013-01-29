using Client.Json;
using Client.Model;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Client.Tests
{
    [TestFixture]
    class JsonDeserializationTests
    {
        static JsonDeserializer deserializer = new JsonDeserializer();

        [TestFixture]
        class DataPointTests
        {
            [Test]
            public void SmokeTest()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"t\":\"2012-01-01T00:00:01.000+00:00\",\"v\":12.34}"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<DataPoint>(jsonResponse);
                Assert.AreEqual(new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1).ToLocalTime(), 12.34), result);
            }

            [Test]
            public void Long()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"t\":\"2012-01-01T00:00:01.000+00:00\",\"v\":1234}"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<DataPoint>(jsonResponse);
                Assert.AreEqual(new DataPoint(new DateTime(2012, 1, 1, 0, 0, 1).ToLocalTime(), 1234.0), result);
            }
        }

        [TestFixture]
        class SeriesTests
        {
            [Test]
            public void SmokeTest()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":\"\",\"attributes\":{},\"tags\":[]}"
                };
                var series = new Series
                {
                    Id = "series-id",
                    Key = "series-key"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<Series>(jsonResponse);
                Assert.AreEqual(series, result);
            }

            [Test]
            public void Name()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":\"series-name\",\"attributes\":{},\"tags\":[]}"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<Series>(jsonResponse);
                Assert.AreEqual("series-name", result.Name);
            }

            [Test]
            public void Tags()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":\"\",\"attributes\":{},\"tags\":[\"tag1\",\"tag2\"]}"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<Series>(jsonResponse);
                Assert.AreEqual(2, result.Tags.Count);
                Assert.AreEqual("tag1", result.Tags[0]);
                Assert.AreEqual("tag2", result.Tags[1]);
            }

            [Test]
            public void Attributes()
            {
                var jsonResponse = new RestResponse
                {
                    Content = "{\"id\":\"series-id\",\"key\":\"series-key\",\"name\":\"\",\"attributes\":{\"key1\":\"val1\",\"key2\":\"val2\"},\"tags\":[]}"
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<Series>(jsonResponse);
                Assert.AreEqual(2, result.Attributes.Count);
                Assert.AreEqual("val1", result.Attributes["key1"]);
                Assert.AreEqual("val2", result.Attributes["key2"]);

            }
        }

        [TestFixture]
        class DataSetTests
        {
            static string jsonDataset = @"{
                  ""series"": {
                    ""id"": ""01868c1a2aaf416ea6cd8edd65e7a4b8"",
                    ""key"": ""key1"",
                    ""name"": """",
                    ""tags"": [
                      ""temp""
                    ],
                    ""attributes"": {
                      ""temp"": ""1""
                    }
                  },
                  ""start"": ""2012-01-08T00:00:00.000+0000"",
                  ""end"": ""2012-01-09T00:00:00.000+0000"",
                  ""data"": [
                    {""t"": ""2012-01-08T00:00:00.000+0000"", ""v"": 4.00},
                    {""t"": ""2012-01-08T06:00:00.000+0000"", ""v"": 3.00},
                    {""t"": ""2012-01-08T12:00:00.000+0000"", ""v"": 2.00},
                    {""t"": ""2012-01-08T18:00:00.000+0000"", ""v"": 3.00}
                  ],
                  ""summary"": {
                    ""mean"": 3.00,
                    ""sum"": 12.00,
                    ""min"": 2.00,
                    ""max"": 4.00,
                    ""stddev"": 0.8165,
                    ""ss"": 2.00,
                    ""count"": 4
                  }
                }";

            [Test]
            public void SmokeTest()
            {
                var jsonResponse = new RestResponse
                {
                    Content = DataSetTests.jsonDataset
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<DataSet>(jsonResponse);
                Assert.AreEqual(new DateTime(2012, 1, 8).ToLocalTime(), result.Start);
                Assert.AreEqual(new DateTime(2012, 1, 9).ToLocalTime(), result.End);
                Assert.AreEqual("01868c1a2aaf416ea6cd8edd65e7a4b8", result.Series.Id);
                Assert.AreEqual(4, result.Data.Count);
                Assert.AreEqual(new DataPoint(new DateTime(2012, 1, 8).ToLocalTime(), 4.0), result.Data[0]);
                Assert.AreEqual(new DataPoint(new DateTime(2012, 1, 8, 6, 0, 0).ToLocalTime(), 3.0), result.Data[1]);
                Assert.AreEqual(3.0, result.Summary["mean"]);
                Assert.AreEqual(12.0, result.Summary["sum"]);
            }

            [Test]
            public void EmptySummary()
            {
                var emptySummary = jsonDataset.Substring(0, jsonDataset.LastIndexOf("\"mean\""));
                emptySummary += "}}";
                var jsonResponse = new RestResponse
                {
                    Content = emptySummary
                };

                var result = JsonDeserializationTests.deserializer.Deserialize<DataSet>(jsonResponse);
                Assert.AreEqual(0, result.Summary.Count);
            }
        }
    }
}
