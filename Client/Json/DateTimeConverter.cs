using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;


namespace Client.Json
{
    public class DateTimeConvertor : DateTimeConverterBase
    {

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(ConvertDateTimeToString(value));
        }

        public static string ConvertDateTimeToString(object value)
        {
            return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }
    }
}
