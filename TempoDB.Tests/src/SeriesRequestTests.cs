using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;


namespace TempoDB.Tests
{
    [TestFixture]
    class CreateSeries
    {
        private Series series = new Series("id1", "key1");

        [Test]
        public void SmokeTest()
        {
            var json = @"{
                ""id"":""id1"",
                ""key"":""key1"",
                ""name"":"""",
                ""tags"":[],
                ""attributes"":{}
            }";

            var response = new RestResponse {
                Content = json,
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            var result = client.CreateSeries(series);
            var expected = new Result<Series>(series, true);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RequestMethod()
        {
            var response = new RestResponse {
                Content = "",
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.POST)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = new RestResponse {
                Content = "",
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = new RestResponse {
                Content = "",
                StatusCode = HttpStatusCode.OK
            };
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.CreateSeries(series);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameterByPattern(req.Parameters, "application/json", "key1"))));
        }
    }
}
