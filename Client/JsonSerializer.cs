using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Client
{

	public class JsonDeserializer : IDeserializer
	{
		public T Deserialize<T>(IRestResponse response)
		{
			return JsonConvert.DeserializeObject<T>(response.Content);
		}

		public string RootElement { get; set; }
		public string Namespace { get; set; }
		public string DateFormat { get; set; }
	}


	public class JsonSerializer : ISerializer
	{
		public JsonSerializer()
		{
			ContentType = "application/json";
		}

		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj, new TempoDateTimeConvertor());
		}

		public string RootElement { get; set; }
		public string Namespace { get; set; }
		public string DateFormat { get; set; }
		public string ContentType { get; set; }
	}

	public class TempoDateTimeConvertor : DateTimeConverterBase
	{

		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			writer.WriteValue(ConvertDateTimeToString(value));
		}

		public static string ConvertDateTimeToString(object value)
		{
			return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
		    return DateTime.Parse(reader.Value.ToString());
		}
	}
}