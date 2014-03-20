using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class MultiDataPoint : Model
    {
        private ZonedDateTime timestamp;
        private IDictionary<string, double> data;

        [JsonProperty(PropertyName="t")]
        public ZonedDateTime Timestamp
        {
            get { return timestamp; }
            private set { timestamp = value; }
        }

        [JsonProperty(PropertyName="v")]
        public IDictionary<string, double> Data
        {
            get { return this.data; }
            private set { this.data = value; }
        }

        public MultiDataPoint(ZonedDateTime timestamp, IDictionary<string, double> data)
        {
            Timestamp = timestamp;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("MultiDataPoint(t={0}, data={1})", Timestamp, Data);
        }

        public override bool Equals(Object obj)
        {
            var other = obj as MultiDataPoint;
            return other != null &&
                Timestamp.Equals(other.Timestamp) &&
                DataEquals(Data, other.Data);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Timestamp);
            hash = GetDataHashCode(hash, Data);
            return hash;
        }

        private bool DataEquals(IDictionary<string, double> data, IDictionary<string, double> other)
        {
            return data != null && other != null &&
                data.Count == other.Count &&
                !data.Except(other).Any();
        }

        private int GetDataHashCode(int hash, IDictionary<string, double> data)
        {
            if(data != null)
            {
                foreach(KeyValuePair<string, double> kvp in data)
                {
                    hash = hash ^ string.Format("k:{0}v{1}", kvp.Key, kvp.Value).GetHashCode();
                }
            }
            return hash;
        }
    }
}
