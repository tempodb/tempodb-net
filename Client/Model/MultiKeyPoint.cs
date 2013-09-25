using Newtonsoft.Json;
using System;

namespace Client.Model
{
  /// <summary>
  ///  Represents a datapoint for a series referenced by key. This class is used to represent
  ///  datapoints in a multi write.
  /// </summary>
  public class MultiKeyPoint : MultiPoint
  {
    ///  <param name="key"> The key of the Series </param>
    ///  <param name="timestamp"> The timestamp of the datapoint </param>
    ///  <param name="value"> The datapoint value </param>
    public MultiKeyPoint(string key, DateTime timestamp, double value)
    {
      Key = key;
      Timestamp = timestamp;
      Value = value;
    }

    public MultiKeyPoint(string key, DateTime timestamp, long value)
    {
      Key = key;
      Timestamp = timestamp;
      Value = value;
    }

    [JsonProperty(PropertyName = "key")]
    public string Key { get; private set; }
  }
}
