using System;
using System.Collections.Generic;
using Client.Model;
using MbUnit.Framework;
using RestSharp;
using System.Linq.Expressions;
using Moq;

namespace Client.Tests
{
	[TestFixture]
	public class WriteTests
	{
        private const string API_KEY = "fddc9934f6784a739cc82e2833521218";
        private const string API_SECRET = "6d1f4fae625b4847968b472d3feb5ba5";
        private const string TEST_SERIES_ID = "17b836c0635844a686249969c5b768c6";
        private const string TEST_SERIES_KEY_1 = "asdf";
        private const string TEST_SERIES_KEY_2 = "my_favorite_series";

		private Client GetClient()
		{
			return new ClientBuilder()
								.Host("api.tempo-db.com")
								.Key(API_KEY)
								.Port(443)
								.Secret(API_SECRET)
								.Secure(true)
								.Build();
		}

		[Test]
		public void ItShouldAddDataPointToSeriesByKey_RequestMethod()
		{
            Expression<Func<RestRequest, bool>> assertion = req => req.Method == Method.POST;

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient(assertion).Object);
			var data = new List<DataPoint>();
			double valueToAdd = new Random().NextDouble()*1000D;
			data.Add(new DataPoint(DateTime.Now, valueToAdd));
			client.WriteByKey("testkey", data);
		}

        [Test]
		public void ItShouldAddDataPointToSeriesByKey_IncludesPoints()
		{
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "12.34") &&
                TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "56.78") &&
                TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "90.12");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient(assertion).Object);
			var data = new List<DataPoint>();
			data.Add(new DataPoint(new DateTime(2012,12,12), 12.34));
            data.Add(new DataPoint(new DateTime(2012, 12, 12, 0, 0, 1), 56.78));
            data.Add(new DataPoint(new DateTime(2012, 12, 12, 0, 0, 2), 90.12));
			client.WriteByKey("testkey", data);
		}

        
		
		[Test]
		public void ItShouldAddBulkDataToMultipleSeries_RequestCount()
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