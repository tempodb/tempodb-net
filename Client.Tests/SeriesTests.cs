using System;
using System.Collections.Generic;
using Client.Model;
using Moq;
using RestSharp;


namespace Client.Tests
{
    /// [TestFixture]
    /// class CreateSeries
    /// {
    ///     [Test]
    ///     public void SmokeTest()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.CreateSeries("series-key");

    ///         Assert.AreEqual("series-id", results.Id);
    ///         Assert.AreEqual("series-key", results.Key);
    ///     }

    ///     [Test]
    ///     public void NoKey()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.CreateSeries();

    ///         Assert.AreEqual("series-id", results.Id);
    ///         Assert.AreEqual(null, results.Key);
    ///     }

    ///     [Test]
    ///     public void RequestMethod()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.CreateSeries("series-key");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Method == Method.POST)));
    ///     }

    ///     [Test]
    ///     public void RequestUrl()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.CreateSeries("series-key");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Resource == "/series/")));
    ///     }

    ///     [Test]
    ///     public void RequestParameters()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.CreateSeries("series-key");
    ///         
    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "series-key"))));
    ///     }
    ///     
    /// }

    /// [TestFixture]
    /// class GetSeries
    /// {
    ///     [Test]
    ///     public void SmokeTest()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesById("series-id");

    ///         Assert.AreEqual("series-id", results.Id);
    ///     }

    ///     [Test]
    ///     public void RequestMethod()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesById("series-id");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Method == Method.GET)));
    ///     }

    ///     [Test]
    ///     public void RequestUrl()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesById("series-id");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Resource == "/series/id/{id}")));
    ///     }

    ///     [Test]
    ///     public void RequestParameters()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Id = "series-id"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesById("series-id");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "id", "series-id"))));
    ///     }

    ///     [Test]
    ///     public void KeySmokeTest()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesByKey("series-key");

    ///         Assert.AreEqual("series-key", results.Key);
    ///     }

    ///     [Test]
    ///     public void KeyRequestMethod()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesByKey("series-key");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Method == Method.GET)));
    ///     }

    ///     [Test]
    ///     public void KeyRequestUrl()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesByKey("series-key");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Resource == "/series/key/{key}")));
    ///     }

    ///     [Test]
    ///     public void KeyRequestParameters()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<Series>(series);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var results = client.GetSeriesByKey("series-key");

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "series-key"))));
    ///     }

    /// }


    /// [TestFixture]
    /// class ListSeries
    /// {
    ///     [Test]
    ///     public void SmokeTest()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<List<Series>>(new List<Series>{series});
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.ListSeries();

    ///         Assert.IsNotEmpty(result);
    ///         Assert.AreEqual(1, result.Count);
    ///     }

    ///     [Test]
    ///     public void RequestMethod()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<List<Series>>(new List<Series> { series });
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.ListSeries();

    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => req.Method == Method.GET)));
    ///     }

    ///     [Test]
    ///     public void RequestUrl()
    ///     {
    ///         var series = new Series
    ///         {
    ///             Key = "series-key"
    ///         };
    ///         var mockclient = TestCommon.GetMockRestClient<List<Series>>(new List<Series> { series });
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.ListSeries();

    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => req.Resource == "/series")));
    ///     }

    ///     [Test]
    ///     public void Filter()
    ///     {
    ///         var mockclient = TestCommon.GetMockRestClient<List<Series>>(new List<Series> {});
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var filter = new Filter();
    ///         filter.AddAttribute("key1", "value1");
    ///         filter.AddAttribute("key2", "value2");
    ///         filter.AddId("id1");
    ///         filter.AddId("id2");
    ///         filter.AddTag("tag1");
    ///         filter.AddTag("tag2");

    ///         var result = client.ListSeries(filter);

    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key1]", "value1"))));
    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "attr[key2]", "value2"))));
    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag1"))));
    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tag", "tag2"))));
    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "id", "id1"))));
    ///         mockclient.Verify(cl => cl.Execute<List<Series>>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "id", "id2"))));
    ///     }

    /// }

    /// [TestFixture]
    /// class UpdateSeries
    /// {
    ///     [Test]
    ///     public void SmokeTest()
    ///     {
    ///         var seriesResponse = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key",
    ///             Tags = new List<string> { "updated" }
    ///         };

    ///         var mockclient = TestCommon.GetMockRestClient<Series>(seriesResponse);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.UpdateSeries(seriesResponse);

    ///         Assert.AreEqual(1, result.Tags.Count);
    ///         Assert.AreEqual("updated", result.Tags[0]);
    ///     }

    ///     [Test]
    ///     public void RequestMethod()
    ///     {
    ///         var seriesResponse = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key",
    ///             Tags = new List<string> { "updated" }
    ///         };

    ///         var mockclient = TestCommon.GetMockRestClient<Series>(seriesResponse);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.UpdateSeries(seriesResponse);

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Method == Method.PUT)));
    ///     }

    ///     [Test]
    ///     public void RequestUrl()
    ///     {
    ///         var seriesResponse = new Series
    ///         {
    ///             Id = "series-id",
    ///             Key = "series-key",
    ///             Tags = new List<string> { "updated" }
    ///         };

    ///         var mockclient = TestCommon.GetMockRestClient<Series>(seriesResponse);
    ///         var client = TestCommon.GetClient(mockclient.Object);

    ///         var result = client.UpdateSeries(seriesResponse);

    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => req.Resource == "/series/id/{id}/")));
    ///         mockclient.Verify(cl => cl.Execute<Series>(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "id", "series-id"))));
    ///     }
    /// }

	
}
