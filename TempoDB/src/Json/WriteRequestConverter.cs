using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Text;
using System;


namespace TempoDB.Json
{
    public class WriteRequestConverter : JsonConverter
    {
        public WriteRequestConverter() { }

        public override bool CanConvert(Type objectType)
        {
            return typeof(WriteRequest).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(value == null)
            {
                throw new ArgumentNullException("value");
            }

            if(!(value is WriteRequest))
            {
                throw new ArgumentException(string.Format("Unexpected value when converting. Expected {0}, got {1}.", typeof(WriteRequest).FullName, value.GetType().FullName));
            }

            var request = (WriteRequest)value;
            writer.WriteStartArray();
            foreach(WritableDataPoint wdp in request)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("key");
                serializer.Serialize(writer, wdp.Series.Key);
                writer.WritePropertyName("t");
                serializer.Serialize(writer, wdp.Timestamp);
                writer.WritePropertyName("v");
                serializer.Serialize(writer, wdp.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}

