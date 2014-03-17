using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class Aggregation
    {
        private Fold fold;

        public Fold Fold
        {
            get { return fold; }
            private set { this.fold = value; }
        }

        public Aggregation(Fold fold)
        {
            this.fold = fold;
        }

        public override string ToString()
        {
            return string.Format("Aggregation(fold={0})", Fold);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            Aggregation other = obj as Aggregation;
            return new EqualsBuilder()
                .Append(Fold, other.Fold)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Fold);
            return hash;
        }
    }
}
