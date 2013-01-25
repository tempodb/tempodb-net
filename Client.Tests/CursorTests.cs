using Client;
using Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace Client.Tests
{
    [TestFixture]
    public class SegmentTests
    {
        [Test]
        public void Constructor()
        {
            List<DataPoint> data = new List<DataPoint> { new DataPoint(new DateTime(2012, 3, 27), 12.34) };
            Segment segment = new Segment(data, "next");
            Assert.AreEqual(data, segment.Data);
            Assert.AreEqual("next", segment.NextUrl);
        }

        [Test]
        public void Deserialize()
        {
            string content = "[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34}]";
            RestSharp.RestResponse response = new RestSharp.RestResponse {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = content
            };
            RestSharp.Parameter link = new RestSharp.Parameter {
                Name = "Link",
                Value = "</v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02>; rel=\"next\""
            };
            response.Headers.Add(link);

            Segment segment = new Segment(response);
            Segment expected = new Segment(new List<DataPoint>{new DataPoint(new DateTime(2012, 3, 27), 12.34)}, "/v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02");
            Assert.AreEqual(expected.Data, segment.Data);
            Assert.AreEqual(expected.NextUrl, segment.NextUrl);
        }

        [Test]
        public void Iterator()
        {
            string content = "[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}]";
            RestSharp.RestResponse response = new RestSharp.RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = content
            };

            Segment segment = new Segment(response);
            List<DataPoint> expected = new List<DataPoint> {
                new DataPoint(new DateTime(2012, 3, 27, 0, 0, 0), 12.34),
                new DataPoint(new DateTime(2012, 3, 27, 1, 0, 0), 23.45)
            };

            List<DataPoint> output = new List<DataPoint>();
            foreach(DataPoint dp in segment)
            {
                output.Add(dp);
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        public void SmokeTest()
        {
            Client client = new Client("key", "secret", "localhost", 4242, "v1", false);
            DateTime start = new DateTime(2012, 1, 1);
            DateTime end = new DateTime(2012, 2, 1);
            string interval = "raw";
            int count = 0;

            Cursor datapoints = client.ReadByKey2("myagley-1", start, end, interval);
            foreach(DataPoint dp in datapoints)
            {
                count = count + 1;
                Console.WriteLine(dp);
            }
            Console.WriteLine(count);
        }
    }
}
