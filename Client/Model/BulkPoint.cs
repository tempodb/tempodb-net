using Newtonsoft.Json;


namespace Client.Model
{
    /// <summary>
    ///  The abstract parent class representing a datapoint used in a bulk write. Bulk
    ///  writing allows values for different series to be written for the same timestamp in one
    ///  Rest call. The series can be referenced by series id or series key. The two subclasses
    ///  represent these two options.
    /// </summary>

    public abstract class BulkPoint
    {
        [JsonProperty(PropertyName = "v")]
        public double Value { get; set; }
    }
}
