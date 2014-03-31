using Newtonsoft.Json;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class Predicate
    {
        private string function;
        private NodaTime.Period period;

        [JsonProperty(PropertyName="function")]
        public string Function
        {
            get { return function; }
            private set { this.function = value; }
        }

        [JsonProperty(PropertyName="period")]
        public NodaTime.Period Period
        {
            get { return period; }
            private set { this.period = value; }
        }

        public Predicate(NodaTime.Period period, string function)
        {
            this.function = function;
            this.period = period;
        }

        public override string ToString()
        {
            return string.Format("Predicate(period={0}, function=\"{1}\")", Period, Function);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            Predicate other = obj as Predicate;
            return new EqualsBuilder()
                .Append(Function, other.Function)
                .Append(Period, other.Period)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Function);
            hash = HashCodeHelper.Hash(hash, Period);
            return hash;
        }
    }
}

