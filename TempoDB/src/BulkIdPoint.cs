using Newtonsoft.Json;


namespace TempoDB
{
    public class BulkIdPoint : BulkPoint
    {
        [JsonProperty(PropertyName="id")]
        public string Id { get; private set; }

        public BulkIdPoint(string id, double value)
        {
            Id = id;
            Value = value;
        }
    }
}
