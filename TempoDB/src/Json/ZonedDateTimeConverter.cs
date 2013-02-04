using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using System;


namespace TempoDB.Json
{
    public class ZonedDateTimeConverter : JsonConverter
    {
        private DateTimeZone zone;

        public ZonedDateTimeConverter()
        {
            this.zone = DateTimeZone.Utc;
        }

        public ZonedDateTimeConverter(DateTimeZone zone)
        {
            this.zone = zone;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ZonedDateTime).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue("blah");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(ZonedDateTime?))
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));

                return null;
            }

            var offsetDateTimeText = reader.Value.ToString();
            if (string.IsNullOrEmpty(offsetDateTimeText) && objectType == typeof(ZonedDateTime?))
                return null;

            var offsetDateTime = OffsetDateTime.FromDateTimeOffset(DateTimeOffset.Parse(offsetDateTimeText));
            return new ZonedDateTime(offsetDateTime.ToInstant(), zone);
        }
    }
}
