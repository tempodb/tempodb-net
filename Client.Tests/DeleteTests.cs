using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Model;
using RestSharp;
using System.Linq.Expressions;
using Moq;

namespace Client.Tests
{
    /// [TestFixture]
    /// class DeleteTests
    /// {
    ///     [Test]
    ///     public void SmokeTest()
    ///     {
    ///         var mockclient = TestCommon.GetMockRestClient();
    ///         var client = TestCommon.GetClient(mockclient.Object);


    ///         client.DeleteById("series-id", new DateTime(2012, 1, 1), new DateTime(2012, 1, 1, 1, 0, 0));

    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.DELETE)));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/series/{property}/{value}/data")));           
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00.000-08:00"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-01T01:00:00.000-08:00"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "id"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "series-id"))));
    ///     }

    ///     [Test]
    ///     public void Key()
    ///     {
    ///         var mockclient = TestCommon.GetMockRestClient();
    ///         var client = TestCommon.GetClient(mockclient.Object);


    ///         client.DeleteByKey("series-key", new DateTime(2012, 1, 1), new DateTime(2012, 1, 1, 1, 0, 0));

    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.DELETE)));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/series/{property}/{value}/data")));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00.000-08:00"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-01T01:00:00.000-08:00"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "property", "key"))));
    ///         mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "value", "series-key"))));
    ///     }
    /// }
}
