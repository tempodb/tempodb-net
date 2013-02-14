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
                    string zoneId = obj["tz"].ToString();
                    zone = string.IsNullOrEmpty(zoneId) ? DateTimeZone.Utc : DateTimeZoneProviders.Tzdb[zoneId];
                }

                var datetimeConverter = new ZonedDateTimeConverter(zone);
                List<DataPoint> datapoints = new List<DataPoint>();
                if(FieldExists("data", obj))
                {
                    /// Figure out how to do this without converting to a string
                    var datapointsString = obj["data"].ToString();
                    if(string.IsNullOrEmpty(datapointsString) == false)
                    {
                        datapoints = JsonConvert.DeserializeObject<List<DataPoint>>(datapointsString, datetimeConverter);
                    }
                }

                Rollup rollup = null;
                if(FieldExists("rollup", obj))
                {
                    var rollupString = obj["rollup"].ToString();
                    if(string.IsNullOrEmpty(rollupString) == false)
                    {
                        rollup = JsonConvert.DeserializeObject<Rollup>(rollupString, periodConverter);
                    }
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
