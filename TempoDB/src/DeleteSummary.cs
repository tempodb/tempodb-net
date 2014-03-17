using Newtonsoft.Json;
using RestSharp;
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

        protected internal static DeleteSummary FromResponse(IRestResponse response)
        {
            var summary = JsonConvert.DeserializeObject<DeleteSummary>(response.Content);
            return summary;
        }

        public override string ToString()
        {
            return string.Format("DeleteSummary(deleted={0})", Deleted);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            DeleteSummary other = obj as DeleteSummary;
            return new EqualsBuilder()
                .Append(Deleted, other.Deleted)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Deleted);
            return hash;
        }
    }
}
