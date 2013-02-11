using NodaTime;
using RestSharp;
using System.Collections.Generic;
using TempoDB.Json;


namespace TempoDB
{
    public class TempoDB
    {
        private string key;
        private string secret;
        private string host;
        private int port;
        private bool secure;
        private string version;
        private RestClient client;

        private JsonSerializer serializer = new JsonSerializer();
        private string clientVersion = string.Format("tempodb-net/{0}", typeof(TempoDB).Assembly.GetName().Version.ToString());
        private const int DefaultTimeoutMillis = 50000;  // 50 seconds

        public TempoDB(string key, string secret, string host="api.tempo-db.com", int port=443, string version="v1", bool secure=true, RestClient client=null)
        {
            Key = key;
            Secret = secret;
            Host = host;
            Port = port;
            Version = version;
            Secure = secure;
            Client = client;
        }

        public Result<Series> CreateSeries(Series series)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.POST, series);
            request.AddUrlSegment("version", Version);
            var result = Execute<Series>(request);
            return result;
        }

        public Result<Series> GetSeriesById(string id)
        {
            var url = "/{version}/series/id/{id}/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var result = Execute<Series>(request);
            return result;
        }

        public Result<Series> GetSeriesByKey(string key)
        {
            var url = "/{version}/series/key/{key}/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var result = Execute<Series>(request);
            return result;
        }

        public Result<Series> UpdateSeries(Series series)
        {
            var url = "/{version}/series/id/{id}/";
            var request = BuildRequest(url, Method.PUT, series);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", series.Id);
            var result = Execute<Series>(request);
            return result;
        }

        public Result<None> WriteDataPointsById(string id, IList<DataPoint> data)
        {
            var url = "/{version}/series/id/{id}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var result = Execute<None>(request);
            return result;
        }

        public Result<None> WriteDataPointsByKey(string key, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var result = Execute<None>(request);
            return result;
        }

        public Result<None> IncrementDataPointsById(string id, IList<DataPoint> data)
        {
            var url = "/{version}/series/id/{id}/increment/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var result = Execute<None>(request);
            return result;
        }

        public Result<None> IncrementDataPointsByKey(string key, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/increment/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var result = Execute<None>(request);
            return result;
        }

        public Result<None> DeleteDataPointsById(string id, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/id/{id}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));
            var result = Execute<None>(request);
            return result;
        }

        public Result<None> DeleteDataPointsByKey(string key, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));
            var result = Execute<None>(request);
            return result;
        }

        public Result<T> Execute<T>(RestRequest request) where T : Model
        {
            IRestResponse response = client.Execute(request);
            var result = new Result<T>(response);
            return result;
        }

        private RestRequest BuildRequest(string url, Method method, object body=null)
        {
            var request = new RestRequest {
                Method = method,
                Resource = url,
                Timeout = DefaultTimeoutMillis,
                RequestFormat = DataFormat.Json,
                JsonSerializer = serializer
            };
            request.AddHeader("Accept-Encoding", "gzip,deflate");
            request.AddHeader("User-Agent", clientVersion);

            if(body != null)
            {
                request.AddBody(body);
            }
            return request;
        }

        public string Key
        {
            get { return this.key; }
            private set { this.key = value; }
        }

        public string Secret
        {
            get { return this.secret; }
            private set { this.secret = value; }
        }

        public string Host
        {
            get { return this.host; }
            private set { this.host = value; }
        }

        public int Port
        {
            get { return this.port; }
            private set { this.port = value; }
        }

        public string Version
        {
            get { return this.version; }
            private set { this.version = value; }
        }

        public bool Secure
        {
            get { return this.secure; }
            private set { this.secure = value; }
        }

        public RestClient Client
        {
            get
            {
                if(this.client == null)
                {
                    string protocol = Secure ? "https://" : "http://";
                    string portString = Port == 80 ? "" : ":" + Port;
                    string baseUrl = protocol + Host + portString;

                    var client = new RestClient {
                        BaseUrl = baseUrl,
                        Authenticator = new HttpBasicAuthenticator(Key, Secret)
                    };
                    Client = client;
                }
                return this.client;
            }
            private set { this.client = value; }
        }
    }
}
