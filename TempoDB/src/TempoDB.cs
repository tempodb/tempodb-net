using NodaTime;
using NodaTime.Text;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Exceptions;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class TempoDB
    {
        private Database database;
        private Credentials credentials;
        private string host;
        private int port;
        private bool secure;
        private string version;
        private RestClient client;

        private JsonSerializer serializer = new JsonSerializer();
        private string clientVersion = string.Format("tempodb-net/{0}", typeof(TempoDB).Assembly.GetName().Version.ToString());
        private const int DefaultTimeoutMillis = 50000;  // 50 seconds

        public TempoDB(Database database, Credentials credentials, string host="api.tempo-db.com", int port=443, string version="v1", bool secure=true, RestClient client=null)
        {
            Database = database;
            Credentials = credentials;
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

        public Response<Nothing> DeleteDataPoints(Series series, Interval interval)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyIntervalToRequest(request, interval);
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

        public Cursor<DataPointFound> FindDataPoints(Series series, Interval interval, Predicate predicate, DateTimeZone zone=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/find/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyIntervalToRequest(request, interval);
            ApplyTimeZoneToRequest(request, zone);
            ApplyPredicateToRequest(request, predicate);

            var response = Execute<DataPointFoundSegment>(request, typeof(DataPointFoundSegment));

            Cursor<DataPointFound> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<DataPointFound>(this, response.Value, typeof(DataPointFoundSegment));
                cursor = new Cursor<DataPointFound>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
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

        public Cursor<Series> GetSeries(Filter filter)
        {
            var url = "/{version}/series/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            ApplyFilterToRequest(request, filter);
            var response = Execute<Segment<Series>>(request);

            Cursor<Series> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<Series>(this, response.Value, typeof(Segment<Series>));
                cursor = new Cursor<Series>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Cursor<DataPoint> ReadDataPoints(Series series, Interval interval, DateTimeZone zone=null, Rollup rollup=null, Interpolation interpolation=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/data/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyInterpolationToRequest(request, interpolation);
            ApplyIntervalToRequest(request, interval);
            ApplyTimeZoneToRequest(request, zone);
            ApplyRollupToRequest(request, rollup);

            var response = Execute<DataPointSegment>(request, typeof(DataPointSegment));

            Cursor<DataPoint> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value, typeof(DataPointSegment));
                cursor = new Cursor<DataPoint>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Cursor<MultiDataPoint> ReadMultiRollupDataPoints(Series series, Interval interval, DateTimeZone zone, MultiRollup rollup, Interpolation interpolation=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/data/rollups/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyIntervalToRequest(request, interval);
            ApplyInterpolationToRequest(request, interpolation);
            ApplyMultiRollupToRequest(request, rollup);
            ApplyTimeZoneToRequest(request, zone);

            var response = Execute<MultiRollupDataPointSegment>(request, typeof(MultiRollupDataPointSegment));

            Cursor<MultiDataPoint> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<MultiDataPoint>(this, response.Value, typeof(MultiRollupDataPointSegment));
                cursor = new Cursor<MultiDataPoint>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Cursor<DataPoint> ReadDataPoints(Filter filter, Interval interval, Aggregation aggregation, DateTimeZone zone=null, Rollup rollup=null, Interpolation interpolation=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/segment/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            ApplyFilterToRequest(request, filter);
            ApplyInterpolationToRequest(request, interpolation);
            ApplyIntervalToRequest(request, interval);
            ApplyAggregationToRequest(request, aggregation);
            ApplyTimeZoneToRequest(request, zone);
            ApplyRollupToRequest(request, rollup);

            var response = Execute<DataPointSegment>(request, typeof(DataPointSegment));

            Cursor<DataPoint> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<DataPoint>(this, response.Value, typeof(DataPointSegment));
                cursor = new Cursor<DataPoint>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Cursor<MultiDataPoint> ReadMultiDataPoints(Filter filter, Interval interval, DateTimeZone zone=null, Rollup rollup=null, Interpolation interpolation=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/multi/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            ApplyFilterToRequest(request, filter);
            ApplyInterpolationToRequest(request, interpolation);
            ApplyIntervalToRequest(request, interval);
            ApplyTimeZoneToRequest(request, zone);
            ApplyRollupToRequest(request, rollup);

            var response = Execute<MultiDataPointSegment>(request, typeof(MultiDataPointSegment));

            Cursor<MultiDataPoint> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<MultiDataPoint>(this, response.Value, typeof(MultiDataPointSegment));
                cursor = new Cursor<MultiDataPoint>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Response<SingleValue> ReadSingleValue(Series series, ZonedDateTime timestamp, DateTimeZone zone=null, Direction direction=Direction.Exact)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/single/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyDirectionToRequest(request, direction);
            ApplyTimestampToRequest(request, timestamp);
            ApplyTimeZoneToRequest(request, zone);
            var response = Execute<SingleValue>(request);
            return response;
        }

        public Cursor<SingleValue> ReadSingleValue(Filter filter, ZonedDateTime timestamp, DateTimeZone zone=null, Direction direction=Direction.Exact)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/single/";
            var request = BuildRequest(url, Method.GET);
            ApplyFilterToRequest(request, filter);
            ApplyDirectionToRequest(request, direction);
            ApplyTimestampToRequest(request, timestamp);
            ApplyTimeZoneToRequest(request, zone);
            var response = Execute<Segment<SingleValue>>(request);

            Cursor<SingleValue> cursor = null;
            if(response.State == State.Success)
            {
                var segments = new SegmentEnumerator<SingleValue>(this, response.Value, typeof(Segment<SingleValue>));
                cursor = new Cursor<SingleValue>(segments);
            }
            else
            {
                throw new TempoDBException(string.Format("API Error: {0} - {1}", response.Code, response.Message));
            }
            return cursor;
        }

        public Response<Summary> ReadSummary(Series series, Interval interval, DateTimeZone zone=null)
        {
            if(zone == null) zone = DateTimeZone.Utc;
            var url = "/{version}/series/key/{key}/summary/";
            var request = BuildRequest(url, Method.GET);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            ApplyIntervalToRequest(request, interval);
            ApplyTimeZoneToRequest(request, zone);
            var response = Execute<Summary>(request);
            return response;
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

        public Response<Nothing> WriteDataPoints(Series series, IList<DataPoint> data)
        {
            var url = "/{version}/series/key/{key}/data/";
            var request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("version", Version);
            request.AddUrlSegment("key", series.Key);
            var response = Execute<Nothing>(request);
            return response;
        }

        public Response<Nothing> WriteDataPoints(WriteRequest writerequest)
        {
            var url = "/{version}/multi/";
            var request = BuildRequest(url, Method.POST, writerequest);
            request.AddUrlSegment("version", Version);
            var response = Execute<Nothing>(request);
            return response;
        }

        public Response<T> Execute<T>(RestRequest request) where T : Model
        {
            return Execute<T>(request, typeof(T));
        }

        public Response<T> Execute<T>(RestRequest request, Type type) where T : Model
        {
            var response = new Response<T>(Client.Execute(request), type);
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

        private static void ApplyAggregationToRequest(IRestRequest request, Aggregation aggregation)
        {
            if(aggregation != null)
            {
                request.AddParameter("aggregation.fold", aggregation.Fold.ToString().ToLower());
            }
        }

        private static void ApplyDirectionToRequest(IRestRequest request, Direction direction)
        {
            request.AddParameter("direction", direction.ToString().ToLower());
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

        private static void ApplyInterpolationToRequest(IRestRequest request, Interpolation interpolation)
        {
            if(interpolation != null)
            {
                request.AddParameter("interpolation.period", PeriodPattern.NormalizingIsoPattern.Format(interpolation.Period));
                request.AddParameter("interpolation.function", interpolation.Function.ToString().ToLower());
            }
        }

        private static void ApplyIntervalToRequest(IRestRequest request, Interval interval)
        {
            var zone = DateTimeZone.Utc;
            request.AddParameter("start", ZonedDateTimeConverter.ToString(interval.Start.InZone(zone)));
            request.AddParameter("end", ZonedDateTimeConverter.ToString(interval.End.InZone(zone)));
        }

        private static void ApplyMultiRollupToRequest(IRestRequest request, MultiRollup rollup)
        {
            if(rollup != null)
            {
                foreach(Fold fold in rollup.Folds)
                {
                    request.AddParameter("rollup.fold", fold.ToString().ToLower());
                }
                request.AddParameter("rollup.period", PeriodPattern.NormalizingIsoPattern.Format(rollup.Period));
            }
        }

        private static void ApplyPredicateToRequest(IRestRequest request, Predicate predicate)
        {
            if(predicate != null)
            {
                request.AddParameter("predicate.period", PeriodPattern.NormalizingIsoPattern.Format(predicate.Period));
                request.AddParameter("predicate.function", predicate.Function.ToLower());
            }
        }

        private static void ApplyRollupToRequest(IRestRequest request, Rollup rollup)
        {
            if(rollup != null)
            {
                request.AddParameter("rollup.period", PeriodPattern.NormalizingIsoPattern.Format(rollup.Period));
                request.AddParameter("rollup.fold", rollup.Fold.ToString().ToLower());
            }
        }

        private static void ApplyTimestampToRequest(IRestRequest request, ZonedDateTime timestamp)
        {
            request.AddParameter("ts", ZonedDateTimeConverter.ToString(timestamp));
        }

        private static void ApplyTimeZoneToRequest(IRestRequest request, DateTimeZone zone)
        {
            var tz = zone == null ? DateTimeZone.Utc : zone;
            request.AddParameter("tz", tz.Id);
        }

        public Database Database
        {
            get { return this.database; }
            private set { this.database = value; }
        }

        public Credentials Credentials
        {
            get { return this.credentials; }
            private set { this.credentials = value; }
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
                        Authenticator = new HttpBasicAuthenticator(Credentials.Key, Credentials.Secret)
                    };
                    Client = client;
                }
                return this.client;
            }
            private set { this.client = value; }
        }
    }
}
