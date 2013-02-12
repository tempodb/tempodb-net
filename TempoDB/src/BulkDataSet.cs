using Newtonsoft.Json;
using NodaTime;
using System.Collections.Generic;


namespace TempoDB
{
    public class BulkDataSet
    {
        [JsonProperty(PropertyName="t")]
        public ZonedDateTime Timestamp { get; private set; }

        [JsonProperty(PropertyName="data")]
        public IList<BulkPoint> Data { get; private set; }

        public BulkDataSet(ZonedDateTime timestamp, IList<BulkPoint> data)
        {
            Timestamp = timestamp;
            Data = data;
        }
    }
}
