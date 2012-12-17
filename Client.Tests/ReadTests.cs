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
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);

            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

            Expression<Func<RestRequest, bool>> assertion = req => req.Method == Method.GET;
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestStartTime()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());
            
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-06-23T00:00:00.000-07:00");
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestEndTime()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-06-24T00:00:00.000-07:00");
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestUrl()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);

            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => req.Resource == "/series/{property}/{value}/data")));
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "key"))));
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "testkey"))));

        }
        
        [Test]
        public void ItShouldReadSeriesDataByKey_RequestInterval()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "interval", "raw");
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void ItShouldReadSeriesDataById_RequestUrl()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);

            var results = client.ReadById("testid", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Raw());

            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => req.Resource == "/series/{property}/{value}/data")));
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "id"))));
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "testid"))));
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
            
            var mockclient = TestCommon.GetMockRestClient<List<DataSet>>(new List<DataSet>());
            var client = TestCommon.GetClient(mockclient.Object);

            var filter = new Filter();
            filter.AddKey("series1");
            filter.AddKey("series2");
            var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());

            Expression<Func<RestRequest, bool>> assertion = req => req.Resource == "/data/";
            mockclient.Verify(cl => cl.Execute<List<DataSet>>(It.Is<RestRequest>(assertion)));

        }

        [Test]
        public void ItShouldReadMultipleSeries_RequestFilter()
        {
            var mockclient = TestCommon.GetMockRestClient<List<DataSet>>(new List<DataSet>());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var filter = new Filter();
            filter.AddKey("series1");
            filter.AddKey("series2");
            var results = client.ReadMultipleSeries(new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), filter, IntervalParameter.Raw());

            mockclient.Verify(cl => cl.Execute<List<DataSet>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "series1"))));
            mockclient.Verify(cl => cl.Execute<List<DataSet>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "series2"))));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestInterval1Hour()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1));

            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "interval", "1hour");
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }

        [Test]
        public void ItShouldReadSeriesDataByKey_RequestFunction()
        {
            var mockclient = TestCommon.GetMockRestClient<DataSet>(new DataSet());
            var client = TestCommon.GetClient(mockclient.Object);
            
            var results = client.ReadByKey("testkey", new DateTime(2012, 06, 23), new DateTime(2012, 06, 24), IntervalParameter.Hours(1), FoldingFunction.Sum);
            
            Expression<Func<RestRequest, bool>> assertion = req => TestCommon.ContainsParameter(req.Parameters, "function", "sum");
            mockclient.Verify(cl => cl.Execute<DataSet>(It.Is<RestRequest>(assertion)));
        }
	}
}