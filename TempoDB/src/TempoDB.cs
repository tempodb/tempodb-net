using NodaTime;
using NodaTime.Text;
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

        public Response<Nothing> DeleteDataPoints(Series series, ZonedDateTime start, ZonedDateTime end)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            request.AddParameter("start", ZonedDateTimeConverter.ToString(start));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(end));
            var response = Execute<Nothing>(request);
            return response;
        }

        public Response<Nothing> DeleteSeries(Series series)
        {
            var url = "/{version}/series/key/{key}/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            var response = Execute<Nothing>(request);
            return response;
        }

        public Response<DeleteSummary> DeleteSeries(Filter filter)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.DELETE);
            ApplyFilterToRequest(request, filter);
            var response = Execute<DeleteSummary>(request);
            return response;
        }

        public Response<DeleteSummary> DeleteAllSeries()
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddParameter("allow_truncation", "true");
            var response = Execute<DeleteSummary>(request);
            return response;
        }

        public Response<Series> GetSeries(string key)
        {
            var url = "/{version}/series/key/{key}/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Cursor<Series>> GetSeries(Filter filter)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.GET);
            ApplyFilterToRequest(request, filter);
            var response = Execute<Segment<Series>>(request);

            Cursor<Series> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<Series>(this, response.Value);
                cursor = new Cursor<Series>(segments);
            }
            return new Response<Cursor<Series>>(cursor, response.Code, response.Message);
        }

        public Response<QueryResult> ReadDataPoints(Series series, Interval interval, DateTimeZone zone=null, Rollup rollup=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/data/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyIntervalToRequest(request, interval);
            ApplyTimeZoneToRequest(request, zone);
            ApplyRollupToRequest(request, rollup);

            var response = Execute<DataPointSegment>(request);

            QueryResult query = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value);
                var cursor = new Cursor<DataPoint>(segments);
                query = new QueryResult(this, cursor, response.Value.Rollup);
            }
            return new Response<QueryResult>(query, response.Code, response.Message);
        }

        public Response<QueryResult> ReadDataPoints(Filter filter, Interval interval, Aggregation aggregation, DateTimeZone zone=null, Rollup rollup=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            ApplyFilterToRequest(request, filter);
            ApplyIntervalToRequest(request, interval);
            ApplyAggregationToRequest(request, aggregation);
            ApplyTimeZoneToRequest(request, zone);
            ApplyRollupToRequest(request, rollup);

            var response = Execute<DataPointSegment>(request);

            QueryResult query = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value);
                var cursor = new Cursor<DataPoint>(segments);
                query = new QueryResult(this, cursor, response.Value.Rollup);
            }
            return new Response<QueryResult>(query, response.Code, response.Message);
        }

        public Response<Series> UpdateSeries(Series series)
        {
            var url = "/{version}/series/key/{key}/";
            var request = BuildRequest(url, Method.PUT, series);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            var response = Execute<Series>(request);
            return response;
        }

        public Response<Nothing> WriteDataPointsByKey(string key, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", key);
            var response = Execute<Nothing>(request);
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

        private static void ApplyTimeZoneToRequest(IRestRequest request, DateTimeZone zone)
        {
            var tz = zone == null ? DateTimeZone.Utc : zone;
            request.AddParameter("tz", tz.Id);
        }

        private static void ApplyRollupToRequest(IRestRequest request, Rollup rollup)
        {
            if(rollup != null)
            {
                request.AddParameter("rollup.period", PeriodPattern.NormalizingIsoPattern.Format(rollup.Period));
                request.AddParameter("rollup.fold", rollup.Fold.ToString().ToLower());
            }
        }

        private static void ApplyAggregationToRequest(IRestRequest request, Aggregation aggregation)
        {
            if(aggregation != null)
            {
                request.AddParameter("aggregation.fold", aggregation.Fold.ToString().ToLower());
            }
        }

        private static void ApplyIntervalToRequest(IRestRequest request, Interval interval)
        {
            var zone = DateTimeZone.Utc;
            request.AddParameter("start", ZonedDateTimeConverter.ToString(interval.Start.InZone(zone)));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(interval.End.InZone(zone)));
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
