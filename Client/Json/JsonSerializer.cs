using Newtonsoft.Json;
using RestSharp.Serializers;


namespace Client.Json
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new DateTimeConvertor());
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}
