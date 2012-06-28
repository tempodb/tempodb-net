using System;
using Client.Model;
using MbUnit.Framework;

namespace Client.Tests
{
	[TestFixture]
	public class SeriesTests
	{

		private const string API_KEY = "your-api-key";
		private const string API_SECRET = "your-api-secret";
		private const string TEST_SERIES_ID_1 = "existing-series-id-1";
		private const string TEST_SERIES_ID_2 = "existing-series-id-2";
		private const string TEST_SERIES_KEY_1 = "existing-series-key-1";
		private const string TEST_SERIES_KEY_2 = "existing-series-key-2";
		private const string TEST_SERIES_TAG = "existing-series-tag";
		private const string TEST_ATTRIBUTE_KEY = "existing-attribute-key";
		private const string TEST_ATTRIBUTE_VALUE = "existing-attribute-value";


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
		public void ItShouldCreateSeriesWithKey()
		{
			var key = Guid.NewGuid().ToString();
			var client = GetClient();
			var series = client.CreateSeries(key);
			Assert.IsNotNull(series);
			Assert.AreEqual(key, series.Key);
		}


		// Unreleased functionality
		//[Test]
		//public void ItShouldCreateSeriesWithComplexKey()
		//{
		//    const string key = "building:1.floor:2.pressure.1";
		//    var client = GetClient();
		//    var series = client.CreateSeries(key);
		//    Assert.IsNotNull(series);
		//    Assert.AreEqual(key, series.Key);
		//}



		[Test]
		public void ItShouldCreateSeriesWithoutKey()
		{
			var client = GetClient();
			var series = client.CreateSeries();
			Assert.IsNotNull(series);
			Assert.AreEqual(series.Id, series.Key);
		}

		[Test]
		public void ItShouldGetSeriesByKey()
		{
			var client = GetClient();
			var series = client.GetSeriesByKey(TEST_SERIES_KEY_1);
			Assert.IsNotNull(series);
			Assert.AreEqual(series.Id, TEST_SERIES_ID_1);
		}

		[Test]
		public void ItShouldGetSeriesById()
		{
			var client = GetClient();
			var series = client.GetSeriesById(TEST_SERIES_ID_1);
			Assert.IsNotNull(series);
			Assert.AreEqual(series.Key, TEST_SERIES_KEY_1);
		}

		[Test]
		public void ItShouldListUnfilteredSeries()
		{
			var client = GetClient();
			var series = client.ListSeries();
			Assert.IsNotEmpty(series);
		}


		[Test]
		public void ItShouldListSeriesFilteredByKey()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddKey(TEST_SERIES_KEY_1);
			filter.AddKey(TEST_SERIES_KEY_2);
			var series = client.ListSeries(filter);
			Assert.Count(2,series);
		}

		[Test]
		public void ItShouldListSeriesFilteredById()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddId(TEST_SERIES_ID_1);
			filter.AddId(TEST_SERIES_ID_2);
			var series = client.ListSeries(filter);
			Assert.Count(2, series);
		}
		
		[Test]
		public void ItShouldListSeriesFilteredByAttribute()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddAttribute(TEST_ATTRIBUTE_KEY,TEST_ATTRIBUTE_VALUE);
			var series = client.ListSeries(filter);
			Assert.Count(1, series);
		}

		[Test]
		public void ItShouldListSeriesFilteredByTag()
		{
			var client = GetClient();
			var filter = new Filter();
			filter.AddTag(TEST_SERIES_TAG);
			var series = client.ListSeries(filter);
			Assert.Count(1, series);
		}


		[Test]
		public void ItShouldUpdateSeries()
		{
			var client = GetClient();
			var series = client.GetSeriesById(TEST_SERIES_ID_1);
			string newAttributeName = Guid.NewGuid().ToString();
			string newAttributeValue = Guid.NewGuid().ToString();
			series.Attributes.Add(newAttributeName, newAttributeValue);

			string newTag = Guid.NewGuid().ToString();
			series.Tags.Add(newTag);

			var updatedSeries = client.UpdateSeries(series);
				
			Assert.AreEqual(series.Attributes.Count, updatedSeries.Attributes.Count);
			Assert.AreEqual(series.Tags.Count, updatedSeries.Tags.Count);

		}
	}
}