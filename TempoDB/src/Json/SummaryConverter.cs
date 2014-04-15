using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;


namespace TempoDB.Json
{
    public class SummaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Summary).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType != typeof(Summary))
                {
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                }
                return null;
            }

            JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
            if(!FieldExists("start", obj))
            {
                throw new ArgumentException("Missing required field for summary: start");
            }

            if(!FieldExists("end", obj))
            {
                throw new ArgumentException("Missing required field for summary: end");
            }

            if(!FieldExists("series", obj))
            {
                throw new ArgumentException("Missing required field for summary: series");
            }

            if(!FieldExists("summary", obj))
            {
                throw new ArgumentException("Missing required field for summary: series");
            }

            DateTimeZone zone = DateTimeZone.Utc;
            if(FieldExists("tz", obj))
            {
                string zoneId = (string)obj["tz"];
                zone = string.IsNullOrEmpty(zoneId) ? DateTimeZone.Utc : DateTimeZoneProviders.Tzdb[zoneId];
            }

            Series series = obj["series"].ToObject<Series>(serializer);
            Instant start = Instant.FromDateTimeOffset(DateTimeOffset.Parse(obj["start"].ToString()));
            Instant end = Instant.FromDateTimeOffset(DateTimeOffset.Parse(obj["end"].ToString()));
            Dictionary<string, double> data = obj["summary"].ToObject<Dictionary<string, double>>(serializer);
            return new Summary(series, new Interval(start, end), zone, data);
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
