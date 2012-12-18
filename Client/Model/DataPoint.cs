using System;
using Newtonsoft.Json;

namespace Client.Model
{
	/// <summary>
	///  Represents one timestamp/value pair. This class uses a Joda Time DateTime.
	/// </summary>
	public class DataPoint
	{
		public DataPoint()
		{
		}
	
		///  <param name="timestamp"> DateTime representing the data point's timstamp </param>
		///  <param name="value"> The value of the measurement (long or double) </param>
		public DataPoint(DateTime timestamp, double value)
		{
			Timestamp = timestamp;
			Value = value;
		}

        [JsonProperty(PropertyName = "t")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "v")]
        public double Value { get; set; }
	
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

        public override Int32 GetHashCode()
        {
            return Timestamp.GetHashCode() ^ Value.GetHashCode();
        }

	}




}