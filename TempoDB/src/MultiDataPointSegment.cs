using Newtonsoft.Json;
using NodaTime;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class MultiDataPointSegment : Segment<MultiDataPoint>
    {
        private static MultiDataPointSegmentConverter converter = new MultiDataPointSegmentConverter();

        [JsonProperty(PropertyName="rollup")]
        public Rollup Rollup { get; private set; }

        [JsonProperty(PropertyName="tz")]
        public DateTimeZone TimeZone { get; private set; }

        public MultiDataPointSegment(IList<MultiDataPoint> datapoints, string next, DateTimeZone tz, Rollup rollup) : base(datapoints, next)
        {
            TimeZone = tz;
            Rollup = rollup;
        }

        protected internal static MultiDataPointSegment FromResponse(IRestResponse response)
        {
            var segment = JsonConvert.DeserializeObject<MultiDataPointSegment>(response.Content, converter);
            segment.NextUrl = HttpHelper.GetLinkFromHeaders("next", response);
            return segment;
        }

        public override bool Equals(Object obj)
        {
            var other = obj as MultiDataPointSegment;
            return other != null &
                Data.Equals(other.Data) &&
                NextUrl.Equals(other.NextUrl) &&
                TimeZone.Equals(other.TimeZone) &&
                Rollup.Equals(other.Rollup);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Data);
            hash = HashCodeHelper.Hash(hash, NextUrl);
            hash = HashCodeHelper.Hash(hash, TimeZone);
            hash = HashCodeHelper.Hash(hash, Rollup);
            return hash;
        }
    }
}

