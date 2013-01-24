using Client.Model;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Client.Tests
{
    [TestFixture]
    public class WriteTests
    {
        [Test]
        public void RequestMethod()
        {
            var mockclient = TestCommon.GetMockRestClient();
            var client = TestCommon.GetClient(mockclient.Object);

            var data = new List<DataPoint>();
            double valueToAdd = new Random().NextDouble() * 1000D;
            data.Add(new DataPoint(DateTime.Now, valueToAdd));
            client.WriteByKey("testkey", data);

            Expression<Func<RestRequest, bool>> assertion = req => req.Method == Method.POST;
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void IncludesPoints()
        {
            var mockclient = TestCommon.GetMockRestClient();
            var client = TestCommon.GetClient(mockclient.Object);

            var data = new List<DataPoint>();
            data.Add(new DataPoint(new DateTime(2012,12,12), 12.34));
            data.Add(new DataPoint(new DateTime(2012, 12, 12, 0, 0, 1), 56.78));
            data.Add(new DataPoint(new DateTime(2012, 12, 12, 0, 0, 2), 90.12));
            client.WriteByKey("testkey", data);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "12.34"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "56.78"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "90.12"))));
        }

        [Test]
        public void RequestCount()
        {
            var numPoints = 100;

            var mockClient = TestCommon.GetMockRestClient();
            var client = TestCommon.GetClient(mockClient.Object);

            var baseDateTime = new DateTime(2012, 06, 23);
            for (int i = 0; i < numPoints; i++)
            {
                var points = new List<BulkPoint>
                {
                    new BulkKeyPoint("testkey1", 12.555D * new Random().NextDouble()),
                    new BulkKeyPoint("testkey2", 555D * new Random().NextDouble())
                };

                var dataSet = new BulkDataSet(baseDateTime.AddMinutes(5*i), points);
                client.WriteBulkData(dataSet);
            }
            mockClient.Verify(cl => cl.Execute(It.IsAny<RestRequest>()), Times.Exactly(100));
        }
    }
}
