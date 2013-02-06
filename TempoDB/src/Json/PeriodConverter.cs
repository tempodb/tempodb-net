using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Text;
using System;


namespace TempoDB.Json
{
    public class PeriodConverter : JsonConverter
    {
        private PeriodPattern pattern;

        public PeriodConverter()
        {
            this.pattern = PeriodPattern.NormalizingIsoPattern;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Period).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!(value is Period))
            {
                throw new ArgumentException(string.Format("Unexpected value when converting. Expected {0}, got {1}.", typeof(Period).FullName, value.GetType().FullName));
            }

            var period = (Period)value;
            writer.WriteValue(pattern.Format(period));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(Period))
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));

                return null;
            }

            var periodText = reader.Value.ToString();
            if (string.IsNullOrEmpty(periodText) && objectType == typeof(Period))
                return null;

            var period = pattern.Parse(periodText).Value;
            return period;
        }
    }
}
