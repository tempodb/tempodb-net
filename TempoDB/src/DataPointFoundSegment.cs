using Newtonsoft.Json;
using NodaTime;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class DataPointFoundSegment : Segment<DataPointFound>
    {
        private static DataPointFoundSegmentConverter converter = new DataPointFoundSegmentConverter();

        [JsonProperty(PropertyName="predicate")]
        public Predicate Predicate { get; private set; }

        [JsonProperty(PropertyName="tz")]
        public DateTimeZone TimeZone { get; private set; }

        public DataPointFoundSegment(IList<DataPointFound> datapoints, string next, DateTimeZone tz, Predicate predicate) : base(datapoints, next)
        {
            Predicate = predicate;
            TimeZone = tz;
        }

        protected internal static DataPointFoundSegment FromResponse(IRestResponse response)
        {
            var segment = JsonConvert.DeserializeObject<DataPointFoundSegment>(response.Content, converter);
            segment.NextUrl = HttpHelper.GetLinkFromHeaders("next", response);
            return segment;
        }

        public override bool Equals(Object obj)
        {
            var other = obj as DataPointFoundSegment;
            return other != null &
                Data.Equals(other.Data) &&
                NextUrl.Equals(other.NextUrl) &&
                TimeZone.Equals(other.TimeZone) &&
                Predicate.Equals(other.Predicate);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Data);
            hash = HashCodeHelper.Hash(hash, NextUrl);
            hash = HashCodeHelper.Hash(hash, TimeZone);
            hash = HashCodeHelper.Hash(hash, Predicate);
            return hash;
        }
    }
}
