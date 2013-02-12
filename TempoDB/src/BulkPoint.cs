using Newtonsoft.Json;


namespace TempoDB
{
    public abstract class BulkPoint
    {
        [JsonProperty(PropertyName="v")]
        public double Value { get; protected set; }
    }
}
