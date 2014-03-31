using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;


namespace TempoDB.Json
{
    public class DataPointFoundSegmentConverter : JsonConverter
    {
        private static IntervalConverter intervalConverter = new IntervalConverter();
        private static PeriodConverter periodConverter = new PeriodConverter();

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataPointFoundSegment).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType != typeof(DataPointFoundSegment))
                {
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                }
                return null;
            }

            DataPointFoundSegment target = null;

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
                List<DataPointFound> datapoints = new List<DataPointFound>();
                if(FieldExists("data", obj))
                {
                    serializer.Converters.Add(datetimeConverter);
                    serializer.Converters.Add(intervalConverter);
                    datapoints = obj["data"].ToObject<List<DataPointFound>>(serializer);
                }

                Predicate predicate = null;
                if(FieldExists("predicate", obj))
                {
                    serializer.Converters.Add(periodConverter);
                    predicate = obj["predicate"].ToObject<Predicate>(serializer);
                }

                target = new DataPointFoundSegment(datapoints, null, zone, predicate);
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
