using Newtonsoft.Json;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    class Rollup
    {
        private string fold;
        private NodaTime.Period period;
        private NodaTime.DateTimeZone zone;

        [JsonProperty(PropertyName="function", Required=Required.Always)]
        public string Fold
        {
            get { return fold; }
            private set { this.fold = value; }
        }

        [JsonProperty(PropertyName="interval", Required=Required.Always)]
        public NodaTime.Period Period
        {
            get { return period; }
            private set { this.period = value; }
        }

        [JsonProperty(PropertyName="tz", Required=Required.Always)]
        public NodaTime.DateTimeZone Zone
        {
            get { return zone; }
            private set { this.zone = value; }
        }

        public Rollup(string fold, NodaTime.Period period, NodaTime.DateTimeZone zone=null)
        {
            this.fold = fold;
            this.period = period;
            this.zone = zone == null ? NodaTime.DateTimeZone.Utc : zone;
        }

        public override string ToString()
        {
            return string.Format("Rollup({0}, {1}, {2})", Fold, Period, Zone);
        }

        public override bool Equals(Object obj)
        {
            Rollup other = obj as Rollup;
            return other != null &&
                Fold.Equals(other.Fold) &&
                Period.Equals(other.Period) &&
                Zone.Equals(other.Zone);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Fold);
            hash = HashCodeHelper.Hash(hash, Period);
            hash = HashCodeHelper.Hash(hash, Zone);
            return hash;
        }
    }
}
