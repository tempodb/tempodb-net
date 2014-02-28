using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TempoDB.Json
{
    public class FoldConverter : JsonConverter
    {
        public FoldConverter() {}

        public override bool CanConvert(Type objectType)
        {
            return typeof(Fold).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if(!(value is Fold))
            {
                throw new ArgumentException(string.Format("Unexpected value when converting. Expected {0}, got {1}.", typeof(Fold).FullName, value.GetType().FullName));
            }

            var fold = (Fold)value;
            writer.WriteValue(fold.ToString("g").ToLower());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(Fold))
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));
                return null;
            }

            var foldText = reader.Value.ToString();
            if (string.IsNullOrEmpty(foldText) && objectType == typeof(Fold))
                return null;

            Fold fold;
            Enum.TryParse(foldText, true, out fold);
            return fold;
        }
    }
}
