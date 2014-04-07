using Newtonsoft.Json;
using System;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class MultiRollup
    {
        private Fold[] folds;
        private NodaTime.Period period;

        [JsonProperty(PropertyName="folds", Required=Required.Always)]
        public Fold[] Folds
        {
            get { return folds; }
            private set { this.folds = value; }
        }

        [JsonProperty(PropertyName="period", Required=Required.Always)]
        public NodaTime.Period Period
        {
            get { return period; }
            private set { this.period = value; }
        }

        public MultiRollup(NodaTime.Period period, Fold[] folds)
        {
            this.period = period;
            this.folds = folds;
        }

        public override string ToString()
        {
            return string.Format("MultiRollup(folds={0}, period={1})", Folds, Period);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            MultiRollup other = obj as MultiRollup;
            return other != null &&
                FoldsEquals(Folds, other.Folds) &&
                Period.Equals(other.Period);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = GetFoldsHashCode(hash, Folds);
            hash = HashCodeHelper.Hash(hash, Period);
            return hash;
        }

        private bool FoldsEquals(Fold[] folds, Fold[] other)
        {
            return other != null && folds != null &&
                folds.Count() == other.Count() &&
                !folds.Except(other).Any();
        }

        private int GetFoldsHashCode(int hash, Fold[] folds)
        {
            if(folds != null)
            {
                foreach(Fold fold in folds)
                {
                    hash = hash ^ fold.GetHashCode();
                }
            }
            return hash;
        }
    }
}
