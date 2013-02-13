using Newtonsoft.Json;
using NodaTime;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class DataPoint : Model
    {
        private ZonedDateTime timestamp;
        private double value;

        [JsonProperty(PropertyName="t")]
        public ZonedDateTime Timestamp
        {
            get { return timestamp; }
            private set { timestamp = value; }
        }

        [JsonProperty(PropertyName="v")]
        public double Value
        {
            get { return this.value; }
            private set { this.value = value; }
        }

        public DataPoint(ZonedDateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("DataPoint({0}, {1})", Timestamp, Value);
        }

        public override bool Equals(Object obj)
        {
            DataPoint other = obj as DataPoint;
            return other != null &&
                Timestamp.Equals(other.Timestamp) &&
                Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Timestamp);
            hash = HashCodeHelper.Hash(hash, Value);
            return hash;
        }
    }
}
