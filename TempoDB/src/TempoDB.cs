using NodaTime;
using RestSharp;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


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

        public Response<Series> CreateSeries(Series series)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.POST, series);
            request.AddUrlSegment("version", Version);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Series> GetSeriesById(string id)
        {
            var url = "/{version}/series/id/{id}/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Series> GetSeriesByKey(string key)
        {
            var url = "/{version}/series/key/{key}/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Series> UpdateSeries(Series series)
        {
            var url = "/{version}/series/id/{id}/";
            var request = BuildRequest(url, Method.PUT, series);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", series.Id);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Cursor<Series>> FilterSeries(Filter filter)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.GET);
            ApplyFilterToRequest(request, filter);
            var response = Execute<Segment<Series>>(request);

            Cursor<Series> cursor = null;
            if(response.Success)
            {
                var segments = new SegmentEnumerator<Series>(this, response.Value);
                cursor = new Cursor<Series>(segments);
            }
            return new Response<Cursor<Series>>(cursor, response.Code, response.Message);
        }

        public Response<None> WriteDataPointsById(string id, IList<DataPoint> data)
        {
            var url = "/{version}/series/id/{id}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var response = Execute<None>(request);
            return response;
        }

        public Response<None> WriteDataPointsByKey(string key, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var response = Execute<None>(request);
            return response;
        }

        public Response<None> WriteBulkData(IList<BulkDataSet> data)
        {
            var url = "/{version}/data/";
            Response<None> response = null;
            foreach(BulkDataSet dataset in data)
            {
                var request = BuildRequest(url, Method.POST, dataset);
                request.AddUrlSegment("version", Version);
                response = Execute<None>(request);
                if(response.Success != true)
                {
                    /// An error occurred, stop writing and alert the user.
                    return response;
                }
            }
            return response;
        }

        public Response<None> IncrementDataPointsById(string id, IList<DataPoint> data)
        {
            var url = "/{version}/series/id/{id}/increment/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            var response = Execute<None>(request);
            return response;
        }

        public Response<None> IncrementDataPointsByKey(string key, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/increment/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var response = Execute<None>(request);
            return response;
        }

        public Response<None> IncrementBulkData(IList<BulkDataSet> data)
        {
            var url = "/{version}/increment/";
            Response<None> response = null;
            foreach(BulkDataSet dataset in data)
            {
                var request = BuildRequest(url, Method.POST, dataset);
                request.AddUrlSegment("version", Version);
                response = Execute<None>(request);
                if(response.Success != true)
                {
                    /// An error occurred, stop writing and alert the user.
                    return response;
                }
            }
            return response;
        }

        public Response<QueryResult> ReadDataPointsById(string id, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/id/{id}/data/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));

            var response = Execute<DataPointSegment>(request);

            QueryResult query = null;
            if(response.Success)
            {
                var rollup = response.Value.Rollup;
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value);
                var cursor = new Cursor<DataPoint>(segments);
                query = new QueryResult(this, cursor, rollup);
            }
            return new Response<QueryResult>(query, response.Code, response.Message);
        }

        public Response<QueryResult> ReadDataPointsByKey(string key, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/key/{key}/data/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));

            var response = Execute<DataPointSegment>(request);

            QueryResult query = null;
            if(response.Success)
            {
                var rollup = response.Value.Rollup;
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value);
                var cursor = new Cursor<DataPoint>(segments);
                query = new QueryResult(this, cursor, rollup);
            }
            return new Response<QueryResult>(query, response.Code, response.Message);
        }

        public Response<None> DeleteDataPointsById(string id, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/id/{id}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("id", id);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));
            var response = Execute<None>(request);
            return response;
        }

        public Response<None> DeleteDataPointsByKey(string key, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));
            var response = Execute<None>(request);
            return response;
        }

        public Response<T> Execute<T>(RestRequest request) where T : Model
        {
            var response = new Response<T>(Client.Execute(request));
            return response;
        }

        public RestRequest BuildRequest(string url, Method method, object body=null)
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

        private static void ApplyFilterToRequest(IRestRequest request, Filter filter)
        {
            if(filter != null)
            {
                foreach(string id in filter.Ids)
                {
                    request.AddParameter("id", id);
                }
                foreach(string key in filter.Keys)
                {
                    request.AddParameter("key", key);
                }
                foreach(string tag in filter.Tags)
                {
                    request.AddParameter("tag", tag);
                }
                foreach(var attribute in filter.Attributes)
                {
                    request.AddParameter(string.Format("attr[{0}]", attribute.Key), attribute.Value);
                }
            }
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
