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
            Aggregation other = obj as Aggregation;
            return other != null &&
                Fold.Equals(other.Fold);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Fold);
            return hash;
        }
    }
}
