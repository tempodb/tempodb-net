using Newtonsoft.Json;
using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class DeleteSummary : Model
    {
        private int deleted;

        [JsonProperty(PropertyName="deleted")]
        public int Deleted
        {
            get { return deleted; }
            private set { this.deleted = value; }
        }

        public DeleteSummary(int deleted)
        {
            Deleted = deleted;
        }

        public override string ToString()
        {
            return string.Format("DeleteSummary(deleted={0})", Deleted);
        }

        public override bool Equals(Object obj)
        {
            DeleteSummary other = obj as DeleteSummary;
            return other != null &&
                Deleted.Equals(other.Deleted);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Deleted);
            return hash;
        }
    }
}
