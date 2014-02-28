using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;


namespace TempoDB.Json
{
    public class DataPointSegmentConverter : JsonConverter
    {
        private static FoldConverter foldConverter = new FoldConverter();
        private static PeriodConverter periodConverter = new PeriodConverter();

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataPointSegment).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType != typeof(DataPointSegment))
                {
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                }
                return null;
            }

            DataPointSegment target = null;

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
                List<DataPoint> datapoints = new List<DataPoint>();
                if(FieldExists("data", obj))
                {
                    serializer.Converters.Add(datetimeConverter);
                    datapoints = obj["data"].ToObject<List<DataPoint>>(serializer);
                }

                Rollup rollup = null;
                if(FieldExists("rollup", obj))
                {
                    serializer.Converters.Add(foldConverter);
                    serializer.Converters.Add(periodConverter);
                    rollup = obj["rollup"].ToObject<Rollup>(serializer);
                }

                target = new DataPointSegment(datapoints, null, zone, rollup);
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
