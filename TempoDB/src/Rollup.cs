using Newtonsoft.Json;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class Rollup
    {
        private string fold;
        private NodaTime.Period period;

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
        public Rollup(string fold, NodaTime.Period period)
        {
            this.fold = fold;
            this.period = period;
        }

        public override string ToString()
        {
            return string.Format("Rollup({0}, {1})", Fold, Period);
        }

        public override bool Equals(Object obj)
        {
            Rollup other = obj as Rollup;
            return other != null &&
                Fold.Equals(other.Fold) &&
                Period.Equals(other.Period);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Fold);
            hash = HashCodeHelper.Hash(hash, Period);
            return hash;
        }
    }
}
