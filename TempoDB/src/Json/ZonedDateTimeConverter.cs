using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Text;
using System;


namespace TempoDB.Json
{
    public class ZonedDateTimeConverter : JsonConverter
    {
        private DateTimeZone zone;
        private LocalDateTimePattern datetimePattern;
        private OffsetPattern offsetPattern;

        public ZonedDateTimeConverter()
        {
            this.zone = DateTimeZone.Utc;
            this.datetimePattern = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-ddTHH:mm:ss.FFF");
            this.offsetPattern = OffsetPattern.CreateWithInvariantCulture("+HH:mm");
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
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!(value is ZonedDateTime))
            {
                throw new ArgumentException(string.Format("Unexpected value when converting. Expected {0}, got {1}.", typeof(ZonedDateTime).FullName, value.GetType().FullName));
            }
            var datetime = (ZonedDateTime)value;
            var localdatetime = datetime.LocalDateTime;
            var offset = datetime.Offset;
            writer.WriteValue(String.Format("{0}{1}", datetimePattern.Format(localdatetime), offsetPattern.Format(offset)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
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
            var datetime = new ZonedDateTime(offsetDateTime.ToInstant(), zone);
            return datetime;
        }
    }
}
