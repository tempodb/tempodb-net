using Newtonsoft.Json;
using NodaTime;
using System;
using TempoDB.Utility;

namespace TempoDB
{
    public class DataPointFound : Model
    {
        private Interval interval;
        private DataPoint datapoint;

        [JsonProperty(PropertyName="interval")]
        public Interval Interval
        {
            get { return interval; }
            private set { interval = value; }
        }

        [JsonProperty(PropertyName="found")]
        public DataPoint DataPoint
        {
            get { return datapoint; }
            private set { datapoint = value; }
        }

        public DataPointFound(Interval interval, DataPoint datapoint)
        {
            Interval = interval;
            DataPoint = datapoint;
        }

        public override string ToString()
        {
            return string.Format("DataPointFound(interval={0}, datapoint={1})", Interval, DataPoint);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            DataPointFound other = obj as DataPointFound;
            return new EqualsBuilder()
                .Append(Interval, other.Interval)
                .Append(DataPoint, other.DataPoint)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Interval);
            hash = HashCodeHelper.Hash(hash, DataPoint);
            return hash;
        }
    }
}
