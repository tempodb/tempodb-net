using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;


namespace TempoDB.Json
{
    public class SingleValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SingleValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType != typeof(SingleValue))
                {
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                }
                return null;
            }

            SingleValue target = null;

            JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
            if(obj != null)
            {
                DateTimeZone zone = DateTimeZone.Utc;
                if(FieldExists("tz", obj))
                {
                    string zoneId = (string)obj["tz"];
                    zone = string.IsNullOrEmpty(zoneId) ? DateTimeZone.Utc : DateTimeZoneProviders.Tzdb[zoneId];
                }

                var datetimeConverter = new ZonedDateTimeConverter(zone);
                DataPoint datapoint = null;
                if(FieldExists("data", obj))
                {
                    serializer.Converters.Add(datetimeConverter);
                    datapoint = obj["data"].ToObject<DataPoint>(serializer);
                }

                Series series = null;
                if(FieldExists("series", obj))
                {
                    series = obj["series"].ToObject<Series>(serializer);
                }

                target = new SingleValue(series, datapoint);
            }
            return target;
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
