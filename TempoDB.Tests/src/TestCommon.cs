using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace TempoDB.Tests
{
    public static class TestCommon
    {
        public static TempoDB GetClient(RestClient restClient)
        {
            return new TempoDB("api-key", "api-secret", client: restClient);
        }

        public static TempoDB GetClient(RestResponse response)
        {
            var mockClient = GetMockRestClient(response);
            TempoDB client = GetClient(mockClient.Object);
            return client;
        }

        public static Mock<RestClient> GetMockRestClient(RestResponse response)
        {
            var client = new Mock<RestClient>();
            client.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(response);
            return client;
        }

        public static RestResponse GetResponse(int expectedStatus, string expectedBody)
        {
            var response = new RestResponse {
                Content = expectedBody,
                StatusCode = (HttpStatusCode)expectedStatus,
                StatusDescription = ""
            };
            return response;
        }

        public static bool ContainsParameter(IList<Parameter> parameters, string name, string value)
        {
            IEnumerable<Parameter> enumerable = parameters;
            return enumerable.Any(parameter => parameter.Name.ToString() == name && parameter.Value.ToString() == value);
        }

        public static bool ContainsParameterByPattern(IList<Parameter> parameters, string name, string value)
        {
            IEnumerable<Parameter> enumerable = parameters;
            return enumerable.Any(parameter => parameter.Name.ToString() == name && parameter.Value.ToString().Contains(value));
        }
    }
}
