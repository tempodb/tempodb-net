using NUnit.Framework;
using RestSharp;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    public class SeriesTest
    {
        [Test]
        public void Defaults()
        {
            var series1 = new Series("key1");
            Assert.AreEqual(0, series1.Tags.Count);
            Assert.AreEqual(0, series1.Attributes.Count);
        }

        [Test]
        public void Equality()
        {
            var series1 = new Series("my-key", "name");
            var series2 = new Series("my-key", "name");
            series1.Tags.Add("tag1");
            series1.Tags.Add("tag2");
            series2.Tags.Add("tag2");
            series2.Tags.Add("tag1");

            series1.Attributes.Add("key1", "value1");
            series1.Attributes.Add("key2", "value2");
            series2.Attributes.Add("key2", "value2");
            series2.Attributes.Add("key1", "value1");

            Assert.AreEqual(series1, series2);
            Assert.AreEqual(series1.GetHashCode(), series2.GetHashCode());
        }

        [Test]
        public void UnequalityTags()
        {
            var series1 = new Series("key");
            var series2 = new Series("key");
            series1.Tags.Add("tag1");
            series2.Tags.Add("tag2");

            Assert.AreNotEqual(series1, series2);
        }

        [Test]
        public void UnequalityAttributes()
        {
            var series1 = new Series("key");
            var series2 = new Series("key");
            series1.Attributes.Add("key", "value1");
            series2.Attributes.Add("key", "value2");

            Assert.AreNotEqual(series1, series2);
        }

        [Test]
        public void FromResponse()
        {
            var response = new RestResponse {
                StatusCode = HttpStatusCode.OK,
                Content = @"{""id"":""id1"",""key"":""key1"",""name"":""Name1"",""tags"":[],""attributes"":{}}"
            };
            var series = Series.FromResponse(response);
            var expected = new Series("key1", "Name1");
            Assert.AreEqual(expected, series);
        }
    }
}
