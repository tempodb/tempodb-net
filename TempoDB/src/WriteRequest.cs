using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class WriteRequest : IEnumerable
    {
        private List<WritableDataPoint> data;

        public WriteRequest()
        {
            this.data = new List<WritableDataPoint>();
        }

        public WriteRequest Add(Series series, DataPoint datapoint)
        {
            this.data.Add(new WritableDataPoint(series, datapoint.Timestamp, datapoint.Value));
            return this;
        }

        public WriteRequest Add(Series series, IList<DataPoint> datapoints)
        {
            foreach(DataPoint datapoint in datapoints)
            {
                WritableDataPoint mdp = new WritableDataPoint(series, datapoint.Timestamp, datapoint.Value);
                data.Add(mdp);
            }
            return this;
        }

        public IEnumerator GetEnumerator()
        {
            foreach(WritableDataPoint mdp in data)
            {
                yield return mdp;
            }
        }

        public override bool Equals(Object obj)
        {
            WriteRequest other = obj as WriteRequest;
            return other != null &&
                data.Equals(other.data);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, data);
            return hash;
        }
    }
}
