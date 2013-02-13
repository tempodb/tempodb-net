using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class DataPointSegment : Segment<DataPoint>
    {
        private static ZonedDateTimeConverter datetimeConverter = new ZonedDateTimeConverter();
        private static PeriodConverter periodConverter = new PeriodConverter();
        private static DateTimeZoneConverter zoneConverter = new DateTimeZoneConverter();

        [JsonProperty(PropertyName="rollup")]
        public Rollup Rollup { get; private set; }

        public DataPointSegment(IList<DataPoint> datapoints, string next, Rollup rollup) : base(datapoints, next)
        {
            Rollup = rollup;
        }

        protected internal static DataPointSegment FromResponse(IRestResponse response)
        {
            var segment = JsonConvert.DeserializeObject<DataPointSegment>(response.Content, datetimeConverter, periodConverter, zoneConverter);
            segment.NextUrl = HttpHelper.GetLinkFromHeaders("next", response);
            return segment;
        }

        public override bool Equals(Object obj)
        {
            var other = obj as DataPointSegment;
            return other != null &
                Data.Equals(other.Data) &&
                NextUrl.Equals(other.NextUrl) &&
                Rollup.Equals(other.Rollup);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Data);
            hash = HashCodeHelper.Hash(hash, NextUrl);
            hash = HashCodeHelper.Hash(hash, Rollup);
            return hash;
        }
    }
}
