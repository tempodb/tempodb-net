using System;
using System.Collections.Generic;
using Client.Model;
using MbUnit.Framework;

namespace Client.Tests
{
	[TestFixture]
	public class WriteTests
	{
		private const string API_KEY = "your-api-key";
		private const string API_SECRET = "your-api-secret";
		private const string TEST_SERIES_ID = "existing-series-id";
		private const string TEST_SERIES_KEY_1 = "existing-series-key-1";
		private const string TEST_SERIES_KEY_2 = "existing-series-key-2";

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
		public void ItShouldAddDataPointToSeriesByKey()
		{
			var client = GetClient();
			var data = new List<DataPoint>();
			double valueToAdd = new Random().NextDouble()*1000D;
			data.Add(new DataPoint(DateTime.Now, valueToAdd));
			client.WriteByKey(TEST_SERIES_KEY_1, data);

		}

		[Test]
		public void ItShouldAddDataPointToSeriesById()
		{
			var client = GetClient();
			var data = new List<DataPoint>();
			double valueToAdd = new Random().NextDouble() * 1000D;
			data.Add(new DataPoint(DateTime.Now, valueToAdd));
			client.WriteById(TEST_SERIES_ID, data);

		}

		[Test]
		public void ItShouldAddBulkDataToMultipleSeries()
		{
			var baseDateTime = new DateTime(2012,06,23);
			var client = GetClient();
			for (int i = 0; i < 100; i++)
			{
				var points = new List<BulkPoint>
			             	{
								new BulkKeyPoint(TEST_SERIES_KEY_1, 12.555D * new Random().NextDouble()), 
								new BulkKeyPoint(TEST_SERIES_KEY_2, 555D * new Random().NextDouble())
							};

				var dataSet = new BulkDataSet(baseDateTime.AddMinutes(5*i), points);
				client.WriteBulkData(dataSet);
			}
		}
	}
}