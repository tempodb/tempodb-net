using Newtonsoft.Json;


namespace Client.Model
{
    /// <summary>
    ///  Represents a datapoint for a series referenced by id. This class is used to represent
    ///  datapoints in a bulk write.
    /// </summary>
    public class BulkKeyPoint : BulkPoint
    {
        ///  <param name="key"> The key of the Series </param>
        ///  <param name="value"> The datapoint value </param>
        public BulkKeyPoint(string key, double value)
        {
            Key = key;
            Value = value;
        }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; private set; }
    }
}
