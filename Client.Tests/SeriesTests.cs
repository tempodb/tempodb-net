using System;
using Client.Model;
using MbUnit.Framework;

namespace Client.Tests
{
	[TestFixture]
	public class SeriesTests
	{

        private const string API_KEY = "fddc9934f6784a739cc82e2833521218";
        private const string API_SECRET = "6d1f4fae625b4847968b472d3feb5ba5";
        private const string TEST_SERIES_ID_1 = "17b836c0635844a686249969c5b768c6";
        private const string TEST_SERIES_KEY_1 = "asdf";
        private const string TEST_SERIES_KEY_2 = "my_favorite_series";


        private const string TEST_SERIES_ID_2 = "1adba64a618745b28c6e7762f88f3722";
        private const string TEST_SERIES_TAG = "temp";
        private const string TEST_ATTRIBUTE_KEY = "sensor";
        private const string TEST_ATTRIBUTE_VALUE = "1";

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