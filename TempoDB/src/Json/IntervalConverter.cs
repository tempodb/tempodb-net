using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;


namespace TempoDB.Json
{
    public class IntervalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Interval).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType != typeof(Interval))
                {
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                }
                return null;
            }

            JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
            if(!FieldExists("start", obj))
            {
                throw new ArgumentException("Missing required field for interval: start");
            }

            if(!FieldExists("end", obj))
            {
                throw new ArgumentException("Missing required field for interval: end");
            }

            Instant start = Instant.FromDateTimeUtc(obj["start"].ToObject<DateTime>());
            Instant end = Instant.FromDateTimeUtc(obj["end"].ToObject<DateTime>());
            return new Interval(start, end);
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
