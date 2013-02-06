using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Text;
using System;


namespace TempoDB.Json
{
    public class DateTimeZoneConverter : JsonConverter
    {
        public DateTimeZoneConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTimeZone).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!(value is DateTimeZone))
            {
                throw new ArgumentException(string.Format("Unexpected value when converting. Expected {0}, got {1}.", typeof(Period).FullName, value.GetType().FullName));
            }
            var zone = (DateTimeZone)value;
            writer.WriteValue(zone.Id);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(DateTimeZone))
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));

                return null;
            }

            var timeZoneId = reader.Value.ToString();
            if (string.IsNullOrEmpty(timeZoneId) && objectType == typeof(DateTimeZone))
                return null;

            var zone = DateTimeZoneProviders.Tzdb[timeZoneId];
            return zone;
        }
    }
}
