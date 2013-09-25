using Newtonsoft.Json;
using System;

namespace Client.Model
{
  /// <summary>
  ///  Represents a datapoint for a series referenced by id. This class is used to represent
  ///  datapoints in a multi write.
  /// </summary>
  public class MultiIdPoint : MultiPoint
  {
    ///  <param name="id"> The id of the Series </param>
    ///  <param name="timestamp"> The timestamp of the datapoint </param>
    ///  <param name="value"> The datapoint value </param>
    public MultiIdPoint(string id, DateTime timestamp, double value)
    {
      Id = id;
      Timestamp = timestamp;
      Value = value;
    }

    public MultiIdPoint(string id, DateTime timestamp, long value)
    {
      Id = id;
      Timestamp = timestamp;
      Value = value;
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; private set; }
  }
}
