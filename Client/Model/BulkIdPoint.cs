using Newtonsoft.Json;


namespace Client.Model
{
    /// <summary>
    ///  Represents a datapoint for a series referenced by id. This class is used to represent
    ///  datapoints in a bulk write.
    /// </summary>
    public class BulkIdPoint : BulkPoint
    {
        ///  <param name="id"> The id of the Series </param>
        ///  <param name="value"> The datapoint value </param>
        public BulkIdPoint(string id, double value)
        {
            Id = id;
            Value = value;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }
    }

}
