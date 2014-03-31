using Newtonsoft.Json;
using RestSharp.Serializers;


namespace TempoDB.Json
{
    public class JsonSerializer : ISerializer
    {
        private DateTimeZoneConverter zoneConverter = new DateTimeZoneConverter();
        private FoldConverter foldConverter = new FoldConverter();
        private IntervalConverter intervalConverter = new IntervalConverter();
        private PeriodConverter periodConverter = new PeriodConverter();
        private WriteRequestConverter writeRequestConverter = new WriteRequestConverter();
        private ZonedDateTimeConverter datetimeConverter = new ZonedDateTimeConverter();

        public JsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, datetimeConverter, foldConverter, intervalConverter, periodConverter, writeRequestConverter, zoneConverter);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}
