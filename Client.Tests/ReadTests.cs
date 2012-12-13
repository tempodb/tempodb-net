using System;
using System.Collections.Generic;
using Client.Model;
using MbUnit.Framework;
using Moq;

namespace Client.Tests
{
	[TestFixture]
	public class ReadTests
	{
        private const string API_KEY = "fddc9934f6784a739cc82e2833521218";
        private const string API_SECRET = "6d1f4fae625b4847968b472d3feb5ba5";
        private const string TEST_SERIES_ID = "17b836c0635844a686249969c5b768c6";
        private const string TEST_SERIES_KEY_1 = "asdf";
        private const string TEST_SERIES_KEY_2 = "my_favorite_series";

		private Client GetClient(RestSharp.RestClient restClient = null)
		{
			return new ClientBuilder()
								.Host("api.tempo-db.com")
								.Key(API_KEY)
								.Port(443)
								.Secret(API_SECRET)
								.Secure(true)
                                .RestClient(restClient)
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
            List<DataSet> ret = new List<DataSet>();
            ret.Add(new DataSet());

            var res = new RestSharp.RestResponse<List<DataSet>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = RestSharp.ResponseStatus.Completed,
                Data = ret
            };

            var restClient = new Mock<RestSharp.RestClient>();
            restClient.Setup(cl => cl.Execute<List<DataSet>>(It.IsAny<RestSharp.RestRequest>())).Returns(res);

            var client = GetClient(restClient.Object);
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