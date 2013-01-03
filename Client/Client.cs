using System;
using System.Collections.Generic;
using System.Net;
using Client.Model;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace Client
{
	/// <summary>
	///  Stores the session information for authenticating and accessing TempoDB.
	///  Your api key and secret is required. The Client also allows you to specify
	///  the hostname, port, and protocol (http or https). This is used if you are on
	///  a private cluster. The default hostname and port should work for the standard cluster.
	///  <p/>
	///  All access to data is made through a client instance.
	/// </summary>
	public class Client
	{
		/// <summary>
		/// How often the monitoring thread checks for connections to close. </summary>
		private const int DEFAULT_TIMEOUT_MILLIS = 30000; // 30 seconds

		private readonly string _host;
		private readonly string _key;
		private readonly int _port;
		private readonly string _secret;
		private readonly bool _secure;
		private readonly string _version;

        private RestClient _restClient; 

		//	private readonly DateTimeFormatter iso8601 = DateTimeFormat.forPattern("yyyy-MM-dd'T'HH:mm:ss.SSSZ");

		///  <param name="key"> Api key </param>
		///  <param name="secret"> Api secret </param>
		///  <param name="host"> Hostname of the api server </param>
		///  <param name="port"> Port that the api server is listening on </param>
        ///  <param name="version"> The API version</param>
        ///  <param name="secure"> Uses http if false, https if true </param>
        ///  <param name="restClient"> Optional Rest Client.  </param>
		public Client(string key, string secret, string host = "api.tempo-db.com", int port = 443, string version = "v1", bool secure = true, RestClient restClient = null)
		{
			_key = key;
			_secret = secret;
			_host = host;
			_port = port;
			_version = version;
			_secure = secure;
            _restClient = restClient;
		}

		/// <summary>
		/// Executes the rest request, where no response is expected.
		/// </summary>
		/// <param name="request">The request to be executed</param>
		public void Execute(RestRequest request)
		{
			RestClient client = GetRestClient();
			IRestResponse response = client.Execute(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception(string.Format("Service call failed with HTTP response [{0}].",
				                                  Enum.GetName(typeof (HttpStatusCode), response.StatusCode)));
			}
		}

		/// <summary>
		/// Executes the rest request, where the JSON response is expected to be deserialized.
		/// </summary>
		/// <typeparam name="T">The type of the response to be deserialized.</typeparam>
		/// <param name="request">The request to be executed.</param>
		/// <returns>The response, deserialized as type T</returns>
		public T Execute<T>(RestRequest request) where T : new()
		{
			RestClient client = GetRestClient();

			IRestResponse<T> response = client.Execute<T>(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception(string.Format("Service call failed with HTTP response [{0}].",
				                                  Enum.GetName(typeof (HttpStatusCode), response.StatusCode)));
			}
			if (response.ResponseStatus != ResponseStatus.Completed)
			{
				throw new Exception(
					string.Format(
						"Response deserialization failed with status [{0} - {1}]. If no response is expected, call Execute() with parameter 'expectJsonRespone=false'",
						Enum.GetName(typeof (ResponseStatus), response.ResponseStatus),
						response.ErrorMessage));
			}
			return response.Data;
		}

		private RestClient GetRestClient()
		{
            if (_restClient == null)
            {

                string protocol = _secure ? "https://" : "http://";
                string portString = (_port == 80) ? "" : ":" + _port;
                string baseUrl = protocol + _host + portString + "/" + _version;

                var client = new RestClient
                                {
                                    BaseUrl = baseUrl,
                                    Authenticator = new HttpBasicAuthenticator(_key, _secret)
                                };

                client.AddHandler("*", new JsonDeserializer());
                return client;
            }
            else
            {
                return _restClient;
            }
		}


		/// <summary>
		///  Creates a new series in the database with a key.
		/// </summary>
		///  <param name="key"> A user-defined key for the series </param>
		///  <returns> A Series </returns>
		public Series CreateSeries(string key = "")
		{
			const string url = "/series/";

			var body = new Dictionary<string, string>();
			body.Add("key", key);
			RestRequest request = BuildRequest(url, Method.POST, body);
			return Execute<Series>(request);
		}

		/// <summary>
		///  Updates metadata for a Series
		/// </summary>
		///  <param name="series"> The series to update. </param>
		///  <returns> The updated Series </returns>
		public Series UpdateSeries(Series series)
		{
			const string url = "/series/id/{id}/";
			RestRequest request = BuildRequest(url, Method.PUT, series);
			request.AddUrlSegment("id", series.Id);
			return Execute<Series>(request);
		}


		/// <summary>
		///  Gets a series by Key
		/// </summary>
		///  <returns> A list of Series with the matching Key </returns>
		public Series GetSeriesByKey(string key)
		{
			const string url = "/series/key/{key}";

			RestRequest request = BuildRequest(url, Method.GET);
			request.AddUrlSegment("key", key);

			return Execute<Series>(request);
		}


		/// <summary>
		///  Gets a series by Id
		/// </summary>
		///  <returns> The Series that matches the specified Id </returns>
		public Series GetSeriesById(string id)
		{
			const string url = "/series/id/{id}";

			RestRequest request = BuildRequest(url, Method.GET);
			request.AddUrlSegment("id", id);

			return Execute<Series>(request);
		}

		/// <summary>
		///  Lists all of the series in the database, with an optional filter.
		/// </summary>
		///  <param name="filter">Filter criteria to limit the list of results.</param>
		///  <returns> A list of Series containing only those that match the specified filter criteria </returns>
		public List<Series> ListSeries(Filter filter = null)
		{
			if (filter == null)
			{
				filter = new Filter();
			}

			const string url = "/series";
			RestRequest request = BuildRequest(url, Method.GET);
			ApplyFilterToRequest(request, filter);
			var result = Execute<List<Series>>(request);
			return result;
		}


		/// <summary>
		///  Writes a DataSet by id
		/// </summary>
		///  <param name="seriesId"> The id of the series into which the data points will be added</param>
		///  <param name="data"> A list of DataPoints to write </param>
		public void WriteById(string seriesId, IList<DataPoint> data)
		{
			WriteDataPoints(SeriesProperty.Id, seriesId, data);
		}

		/// <summary>
		///  Writes a DataSet by key
		/// </summary>
		///  <param name="seriesKey"> The key of the series into which the data points will be added</param>
		///  <param name="data"> A list of DataPoints to write </param>
		public void WriteByKey(string seriesKey, IList<DataPoint> data)
		{
			WriteDataPoints(SeriesProperty.Key, seriesKey, data);
		}


		private void WriteDataPoints(string seriesProperty, string propertyValue, IList<DataPoint> data)
		{
			const string url = "/series/{property}/{value}/data/";
			RestRequest request = BuildRequest(url, Method.POST, data);
			request.AddUrlSegment("property", seriesProperty);
			request.AddUrlSegment("value", propertyValue);
			Execute(request);
		}


		/// <summary>
		///  Writes a set of datapoints for different series for the same timestamp
		/// </summary>
		///  <param name="dataSet"> A BulkDataSet to write </param>
		public virtual void WriteBulkData(BulkDataSet dataSet)
		{
			const string url = "/data/";
			RestRequest request = BuildRequest(url, Method.POST, dataSet);
			Execute(request);
		}

        /// <summary>
        ///  Increments a DataSet by id
        /// </summary>
        ///  <param name="seriesId"> The id of the series into which the data points will be incremented</param>
        ///  <param name="data"> A list of DataPoints to increment </param>
        public void IncrementById(string seriesId, IList<DataPoint> data)
        {
            IncrementDataPoints(SeriesProperty.Id, seriesId, data);
        }

        /// <summary>
        ///  Increments a DataSet by key
        /// </summary>
        ///  <param name="seriesKey"> The key of the series into which the data points will be incremented</param>
        ///  <param name="data"> A list of DataPoints to incremented </param>
        public void IncrementByKey(string seriesKey, IList<DataPoint> data)
        {
            IncrementDataPoints(SeriesProperty.Key, seriesKey, data);
        }


        private void IncrementDataPoints(string seriesProperty, string propertyValue, IList<DataPoint> data)
        {
            const string url = "/series/{property}/{value}/increment/";
            RestRequest request = BuildRequest(url, Method.POST, data);
            request.AddUrlSegment("property", seriesProperty);
            request.AddUrlSegment("value", propertyValue);
            Execute(request);
        }


        /// <summary>
        ///  Increments a set of datapoints for different series for the same timestamp
        /// </summary>
        ///  <param name="dataSet"> A BulkDataSet to increments </param>
        public virtual void IncrementBulkData(BulkDataSet dataSet)
        {
            const string url = "/increment/";
            RestRequest request = BuildRequest(url, Method.POST, dataSet);
            Execute(request);
        }

        /// <summary>
		///  Deletes a range of data by id
		/// </summary>
		///  <param name="seriesId"> The id of the series into which the data points will be deleted</param>
		///  <param name="start"> Start of range </param>
        ///  <param name="end"> End of range </param>
        public void DeleteById(string seriesId, DateTime start, DateTime end)
        {
            DeleteDataPoints("id", seriesId, start, end);
        }

        /// <summary>
        ///  Deletes a range of data by key
        /// </summary>
        ///  <param name="seriesKey"> The key of the series into which the data points will be deleted</param>
        ///  <param name="start"> Start of range </param>
        ///  <param name="end"> End of range </param>
        public void DeleteByKey(string seriesKey, DateTime start, DateTime end)
        {
            DeleteDataPoints("key", seriesKey, start, end);
        }

        private void DeleteDataPoints(string seriesProperty, string propertyValue, DateTime start, DateTime end)
        {
            const string url = "/series/{property}/{value}/data";
            RestRequest request = BuildRequest(url, Method.DELETE);
            request.AddUrlSegment("property", seriesProperty);
            request.AddUrlSegment("value", propertyValue);
            request.AddParameter(QueryStringParameter.Start, TempoDateTimeConvertor.ConvertDateTimeToString(start));
            request.AddParameter(QueryStringParameter.End, TempoDateTimeConvertor.ConvertDateTimeToString(end));
            Execute(request);
        }


		private RestRequest BuildRequest(string url, Method method, object body = null)
		{
			var request = new RestRequest
			              	{
			              		Method = method,
			              		Resource = url,
			              		Timeout = DEFAULT_TIMEOUT_MILLIS,
			              		RequestFormat = DataFormat.Json,
			              		JsonSerializer = new JsonSerializer()
			              	};

			request.AddHeader("Accept-Encoding", "gzip,deflate");

			if (body != null)
			{
				request.AddBody(body);
			}

			return request;
		}

		/// <summary>
		///  Reads a DataSet by id
		/// </summary>
		///  <param name="seriesId"> The id of the series </param>
		///  <param name="start"> The start time of the range </param>
		///  <param name="end"> The end time of the range </param>
		///  <param name="interval"> An interval for the rollup. (e.g. 1min, 15min, 1hour, 1day, 1month) </param>
		///  <param name="function"> A function for the rollup. (e.g. min, max, sum, avg, stddev, count) </param>
		///  <returns> A DataSet </returns>
		public virtual DataSet ReadById(string seriesId, DateTime start, DateTime end, string interval = null,
		                                string function = null)
		{
			return ReadDataSet(SeriesProperty.Id, seriesId, start, end, interval, function);
		}


		/// <summary>
		///  Reads a DataSet by key
		/// </summary>
		///  <param name="seriesKey"> The key of the series </param>
		///  <param name="start"> The start time of the range </param>
		///  <param name="end"> The end time of the range </param>
		///  <param name="interval"> An interval for the rollup. (e.g. 1min, 15min, 1hour, 1day, 1month) </param>
		///  <param name="function"> A function for the rollup. (e.g. min, max, sum, avg, stddev, count) </param>
		///  <returns> A DataSet </returns>
		public virtual DataSet ReadByKey(string seriesKey, DateTime start, DateTime end, string interval = null,
		                                 string function = null)
		{
			return ReadDataSet(SeriesProperty.Key, seriesKey, start, end, interval, function);
		}

		private DataSet ReadDataSet(string seriesProperty, string propertyValue, DateTime start, DateTime end,
		                            string interval = null, string function = null)
		{
			RestRequest request = BuildRequest("/series/{property}/{value}/data", Method.GET);

			request.AddUrlSegment("property", seriesProperty);
			request.AddUrlSegment("value", propertyValue);

			AddReadParameters(request, start, end, interval, function);

			var result = Execute<DataSet>(request);
			return result;
		}


		/// <summary>
		///  Reads a list of DataSet by the provided filter and rolluped by the interval
		/// </summary>
		///  <param name="start"> The start time of the range </param>
		///  <param name="end"> The end time of the range </param>
		///  <param name="filter"> A Filter instance to filter the series </param>
		///  <param name="interval"> An interval for the rollup. (e.g. 1min, 15min, 1hour, 1day, 1month) </param>
		///  <param name="function"> A function for the rollup. (e.g. min, max, sum, avg, stddev, count) </param>
		///  <returns> A list of DataSets </returns>
		public virtual IList<DataSet> ReadMultipleSeries(DateTime start, DateTime end, Filter filter, string interval = null,
		                                                 string function = null)
		{
			RestRequest request = BuildRequest("/data/", Method.GET);

			AddReadParameters(request, start, end, interval, function);

			if (filter != null)
			{
				ApplyFilterToRequest(request, filter);
			}

			return Execute<List<DataSet>>(request);
		}

		private static void AddReadParameters(IRestRequest request, DateTime start, DateTime end, string interval = null,
		                                      string function = null)
		{
			request.AddParameter(QueryStringParameter.Start,  TempoDateTimeConvertor.ConvertDateTimeToString(start));
			request.AddParameter(QueryStringParameter.End, TempoDateTimeConvertor.ConvertDateTimeToString(end));

			if (!string.IsNullOrEmpty(interval))
			{
				request.AddParameter(QueryStringParameter.Interval, interval);
			}

			if (!string.IsNullOrEmpty(function))
			{
				request.AddParameter(QueryStringParameter.Function, function);
			}
		}

		private static void ApplyFilterToRequest(IRestRequest request, Filter filter)
		{
			foreach (string id in filter.Ids)
			{
				request.AddParameter("id", id);
			}
			foreach (string key in filter.Keys)
			{
				request.AddParameter("key", key);
			}
			foreach (string tag in filter.Tags)
			{
				request.AddParameter("tag", tag);
			}
			foreach (var attribute in filter.Attributes)
			{
				request.AddParameter(string.Format("attr[{0}]", attribute.Key), attribute.Value);
			}
		}
	}
}
