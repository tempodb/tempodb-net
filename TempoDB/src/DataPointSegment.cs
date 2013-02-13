using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TempoDB.Utility;


namespace TempoDB
{
    public class DataPointSegment : Model
    {
        [JsonProperty(PropertyName="rollup")]
        public Rollup Rollup { get; private set; }

        [JsonProperty(PropertyName="data")]
        public IList<DataPoint> DataPoints { get; private set; }

        public DataPointSegment(IList<DataPoint> datapoints, Rollup rollup)
        {
            DataPoints = datapoints;
            Rollup = rollup;
        }

        public override bool Equals(Object obj)
        {
            var other = obj as DataPointSegment;
            return other != null &
                DataPoints.Equals(other.DataPoints) &&
                Rollup.Equals(other.Rollup);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, DataPoints);
            hash = HashCodeHelper.Hash(hash, Rollup);
            return hash;
        }
    }
}
