using Newtonsoft.Json;


namespace TempoDB
{
    public class BulkKeyPoint : BulkPoint
    {
        [JsonProperty(PropertyName="key")]
        public string Key { get; private set; }

        public BulkKeyPoint(string key, double value)
        {
            Key = key;
            Value = value;
        }
    }
}
