using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Client.Model;
using MbUnit.Framework;
using Moq;
using RestSharp;

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

		private Client GetClient(RestClient restClient = null)
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

        private Mock<RestClient>  GetMockRestClient<T>(T response, Expression<Func<RestRequest,bool>> requestValidator = null) where T : new()
        {
            if (requestValidator == null)
                requestValidator = req => true;

            var res = new RestSharp.RestResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = RestSharp.ResponseStatus.Completed,
                Data = response
            };

            var restClient = new Mock<RestSharp.RestClient>();
            restClient.Setup(cl => cl.Execute<T>(It.Is<RestRequest>(requestValidator))).Returns(res);
            return restClient;
        }


		[Test]
		public void ItShouldReadSeriesDataByKey()
		{
            Series series = new Series
            {
                Key = "testkey"
            };
            DataSet ret = new DataSet
            {
                Series = series
            };

            var client = GetClient(GetMockRestClient<DataSet>(ret).Object);
			var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
            Assert.AreEqual("testkey", results.Series.Key);
		}

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestMethod()
        {
            Expression<Func<RestRequest, bool>> assertion =  req => req.Method == Method.GET;

            var client = GetClient(GetMockRestClient<DataSet>(new DataSet(),assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestStartTime()
        {
            Expression<Func<RestRequest, bool>> assertion = req => ContainsParameter(req.Parameters, "start", "2012-06-23T00:00:00.000-07:00");
            
            var client = GetClient(GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestEndTime()
        {
            Expression<Func<RestRequest, bool>> assertion = req => ContainsParameter(req.Parameters, "end", "2012-06-24T00:00:00.000-07:00");

            var client = GetClient(GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestUrl()
        {
            Expression<Func<RestRequest, bool>> assertion = req => req.Resource == "/series/{property}/{value}/data" && 
                ContainsParameter(req.Parameters, "property", "key") && 
                ContainsParameter(req.Parameters, "value", "testkey");

            var client = GetClient(GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }
        
        [Test]
        public void ItShouldReadSeriesDataByKey_RequestInterval()
        {
            Expression<Func<RestRequest, bool>> assertion = req => ContainsParameter(req.Parameters, "interval", "raw");

            var client = GetClient(GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
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



            var restClient = GetMockRestClient<List<DataSet>>(ret, req => req.Method == Method.GET);

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

        public static bool ContainsParameter(List<Parameter> parameters, string name, string value)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToString() == name && parameter.Value.ToString() == value) return true;
            }
            return false;
        }
	}
}