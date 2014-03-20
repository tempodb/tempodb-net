using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class QueryResult<T> : Model where T: Model
    {
        public Cursor<T> DataPoints { get; private set; }
        public Rollup Rollup { get; private set; }
        private TempoDB Client { get; set; }

        internal QueryResult(TempoDB client, Cursor<T> datapoints, Rollup rollup)
        {
            Client = client;
            DataPoints = datapoints;
            Rollup = rollup;
        }

        public override bool Equals(Object obj)
        {
            QueryResult<T> other = obj as QueryResult<T>;
            return other != null &&
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
