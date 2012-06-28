using System;
using Client.Model;
using MbUnit.Framework;

namespace Client.Tests
{
	[TestFixture]
	public class ReadTests
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
		public void ItShouldReadSeriesDataByKey()
		{
			var client = GetClient();
			var results = client.ReadByKey(TEST_SERIES_KEY_1, new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
			Assert.IsNotEmpty(results.Data);
		}

		[Test]
		public void ItShouldReadSeriesDataById()
		{
			var client = GetClient();
			var results = client.ReadById(TEST_SERIES_ID, new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
			Assert.IsNotEmpty(results.Data);
		}

		[Test]
		public void ItShouldReadRawData()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddKey(TEST_SERIES_KEY_1);
			filter.AddKey(TEST_SERIES_KEY_2);
			var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());
			Assert.IsNotEmpty(results);
		}


		[Test]
		public void ItShouldReadSeriesDataByKeyWithFoldingFunction()
		{
			var client = GetClient();
			var results = client.ReadByKey(TEST_SERIES_KEY_1, new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1), FoldingFunction.Sum);
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
			Assert.IsNotEmpty(results.Data);
		}

		[Test]
		public void ItShouldReadSeriesDataByIdWithFoldingFunction()
		{
			var client = GetClient();
			var results = client.ReadById(TEST_SERIES_ID, new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1), FoldingFunction.Count);
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
			Assert.IsNotEmpty(results.Data);
		}

		[Test]
		public void ItShouldReadRawDataWithFoldingFunction()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddKey(TEST_SERIES_KEY_1);
			filter.AddKey(TEST_SERIES_KEY_2);
			var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Hours(1), FoldingFunction.Max);
			Assert.IsNotEmpty(results);
		}
	}
}