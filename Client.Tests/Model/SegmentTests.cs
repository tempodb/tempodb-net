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
            string content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34}],\"rollup\":null}";
            RestSharp.RestResponse response = new RestSharp.RestResponse {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = content
            };
            RestSharp.Parameter link = new RestSharp.Parameter {
                Name = "Link",
                Value = "</v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02>; rel=\"next\""
            };
            response.Headers.Add(link);

            Segment segment = Segment.FromResponse(response);
            Segment expected = new Segment(new List<DataPoint>{new DataPoint(new DateTime(2012, 3, 27), 12.34)}, "/v1/series/key/key1/data/segment/?start=2012-01-01&end=2012-01-02");
            Assert.AreEqual(expected.Data, segment.Data);
            Assert.AreEqual(expected.NextUrl, segment.NextUrl);
        }

        [Test]
        public void Iterator()
        {
            string content = "{\"data\":[{\"t\":\"2012-03-27T00:00:00.000-05:00\",\"v\":12.34},{\"t\":\"2012-03-27T01:00:00.000-05:00\",\"v\":23.45}],\"rollup\":null}";
            RestSharp.RestResponse response = new RestSharp.RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = content
            };

            Segment segment = Segment.FromResponse(response);
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
    }
}
