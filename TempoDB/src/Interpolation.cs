using NodaTime;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public enum InterpolationFunction
    {
        ZOH,
        Linear
    }

    public class Interpolation
    {
        private Period period;
        private InterpolationFunction function;

        public Period Period
        {
            get { return this.period; }
            private set { this.period = value; }
        }

        public InterpolationFunction Function
        {
            get { return this.function; }
            private set { this.function = value; }
        }

        public Interpolation(Period period, InterpolationFunction function)
        {
            Period = period;
            Function = function;
        }

        public override string ToString()
        {
            return string.Format("Interpolation(period={0},function={1})", Period, Function.ToString().ToLower());
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            Interpolation other = obj as Interpolation;
            return new EqualsBuilder()
                .Append(Period, other.Period)
                .Append(Function, other.Function)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Period);
            hash = HashCodeHelper.Hash(hash, Function);
            return hash;
        }
    }
}
