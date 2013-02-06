using Newtonsoft.Json;
using RestSharp.Serializers;


namespace TempoDB.Json
{
    public class JsonSerializer : ISerializer
    {
        private ZonedDateTimeConverter datetimeConverter = new ZonedDateTimeConverter();
        private PeriodConverter periodConverter = new PeriodConverter();
        private DateTimeZoneConverter zoneConverter = new DateTimeZoneConverter();

        public JsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, datetimeConverter, periodConverter, zoneConverter);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}
