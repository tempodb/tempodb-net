using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Moq;
using RestSharp;


namespace Client.Tests
{

    class TestCommon
    {
        public static Client GetClient(RestClient restClient)
        {
            return new Client("api-key", "api-secret", restClient: restClient);
        }

        public static Mock<RestClient> GetMockRestClient<T>(T response) where T : new()
        {
            var res = new RestSharp.RestResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = RestSharp.ResponseStatus.Completed,
                Data = response
            };

            var restClient = new Mock<RestClient>();
            restClient.Setup(cl => cl.Execute<T>(It.IsAny<RestRequest>())).Returns(res);
            return restClient;
        }

        public static Mock<RestClient> GetMockRestClient()
        {
            var res = new RestSharp.RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            var restClient = new Mock<RestClient>();
            restClient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(res);
            return restClient;
        }

        public static bool ContainsParameter(List<Parameter> parameters, string name, string value)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToString() == name && parameter.Value.ToString() == value) return true;
            }
            return false;
        }

        public static bool ContainsParameterByPattern(List<Parameter> parameters, string name, string value)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToString() == name && parameter.Value.ToString().Contains(value)) return true;
            }
            return false;
        }
    }
}
