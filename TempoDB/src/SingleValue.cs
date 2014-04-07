using Newtonsoft.Json;
using NodaTime;
using RestSharp;
using System;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class SingleValue : Model
    {
        private static SingleValueConverter converter = new SingleValueConverter();

        private Series series;
        private DataPoint datapoint;

        [JsonProperty(PropertyName="series")]
        public Series Series
        {
            get { return series; }
            private set { series = value; }
        }

        [JsonProperty(PropertyName="data")]
        public DataPoint DataPoint
        {
            get { return this.datapoint; }
            private set { this.datapoint = value; }
        }

        public SingleValue(Series series, DataPoint datapoint)
        {
            Series = series;
            DataPoint = datapoint;
        }

        protected internal static SingleValue FromResponse(IRestResponse response)
        {
            var value = JsonConvert.DeserializeObject<SingleValue>(response.Content, converter);
            return value;
        }

        public override string ToString()
        {
            return string.Format("SingleValue(series={0}, datapoint={1})", Series.ToString(), DataPoint.ToString());
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            SingleValue other = obj as SingleValue;
            return new EqualsBuilder()
                .Append(Series, other.Series)
                .Append(DataPoint, other.DataPoint)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Series);
            hash = HashCodeHelper.Hash(hash, DataPoint);
            return hash;
        }
    }
}

