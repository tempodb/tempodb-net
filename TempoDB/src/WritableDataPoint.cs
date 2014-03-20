using NodaTime;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class WritableDataPoint
    {
        private Series series;
        private ZonedDateTime timestamp;
        private double value;

        public Series Series
        {
            get { return series; }
            private set { this.series = value; }
        }

        public ZonedDateTime Timestamp
        {
            get { return timestamp; }
            private set { this.timestamp = value; }
        }

        public double Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        public WritableDataPoint(Series series, ZonedDateTime timestamp, double value)
        {
            Series = series;
            Timestamp = timestamp;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("WritableDataPoint(series={0}, timestamp={1}, value={2})", Series, Timestamp, Value);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            WritableDataPoint other = obj as WritableDataPoint;
            return new EqualsBuilder()
                .Append(Series, other.Series)
                .Append(Timestamp, other.Timestamp)
                .Append(Value, other.Value)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Series);
            hash = HashCodeHelper.Hash(hash, Timestamp);
            hash = HashCodeHelper.Hash(hash, Value);
            return hash;
        }
    }
}
