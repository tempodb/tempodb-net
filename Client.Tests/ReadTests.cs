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

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(ret).Object);
			var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Series);
            Assert.AreEqual("testkey", results.Series.Key);
		}

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestMethod()
        {
            Expression<Func<RestRequest, bool>> assertion =  req => req.Method == Method.GET;

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(),assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestStartTime()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-06-23T00:00:00.000-07:00");
            
            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestEndTime()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-06-24T00:00:00.000-07:00");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestUrl()
        {
            Expression<Func<RestRequest, bool>> assertion = req => req.Resource == "/series/{property}/{value}/data" && 
                TestCommon.ContainsParameter(req.Parameters, "property", "key") && 
                TestCommon.ContainsParameter(req.Parameters, "value", "testkey");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }
        
        [Test]
        public void ItShouldReadSeriesDataByKey_RequestInterval()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "interval", "raw");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataById_RequestUrl()
        {
            Expression<Func<RestRequest, bool>> assertion = req => req.Resource == "/series/{property}/{value}/data" &&
                TestCommon.ContainsParameter(req.Parameters, "property", "id") &&
                TestCommon.ContainsParameter(req.Parameters, "value", "testid");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadById("testid", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
        }

		[Test]
		public void ItShouldReadMultipleSeries_Response()
		{
            List<DataSet> ret = new List<DataSet> { 
            new DataSet
            {
                Series = new Series { Key = "series1" } 
            },
            new DataSet
            {
                Series = new Series { Key = "series2" }
            }};

            var restClient = TestCommon.GetMockRestClient<List<DataSet>>(ret).Object;
            var client = TestCommon.GetClient(restClient);
            var filter = new Filter();
            filter.AddKey("series1");
            filter.AddKey("series2");
            var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("series1", results[0].Series.Key);
            Assert.AreEqual("series2", results[1].Series.Key);
		}

        [Test]
        public void ItShouldReadMultipleSeries_RequestUrl()
        {
            Expression<Func<RestRequest, bool>> assertion = req => req.Resource == "/data/";
            
            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<List<DataSet>>(new List<DataSet>(), assertion).Object);
            var filter = new Filter();
            filter.AddKey("series1");
            filter.AddKey("series2");
            var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());       
        }

        [Test]
        public void ItShouldReadMultipleSeries_RequestFilter()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "key", "series1") &&
                TestCommon.ContainsParameter(req.Parameters, "key", "series2");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<List<DataSet>>(new List<DataSet>(), assertion).Object);
            var filter = new Filter();
            filter.AddKey("series1");
            filter.AddKey("series2");
            var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestInterval1Hour()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "interval", "1hour");
           
            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestFunction()
        {
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "function", "sum");

            var client = TestCommon.GetClient(TestCommon.GetMockRestClient<DataSet>(new DataSet(), assertion).Object);
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1), FoldingFunction.Sum);
        }

        
	}
}