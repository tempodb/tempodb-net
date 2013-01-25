using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Client.Model
{

    /// <summary>
    ///  Set of data to send for a bulk write. This encapsulates the timestamp and list of BulkPoints
    /// </summary>
    public class BulkDataSet
    {
        ///  <param name="timestamp"> The timestamp to write the datapoints at </param>
        ///  <param name="data"> A list of BulkPoints to write </param>

        public BulkDataSet(DateTime timestamp, List<BulkPoint> data)
        {
            Timestamp = timestamp;
            Data = data;
        }

        [JsonProperty(PropertyName = "t")]
        public DateTime Timestamp { get; private set; }

        [JsonProperty(PropertyName = "data")]
        public List<BulkPoint> Data { get; private set; }
    }
}
