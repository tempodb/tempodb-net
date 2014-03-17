using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class Database
    {
        private string id;

        public string Id
        {
            get { return id; }
            private set { id = value; }
        }

        public Database(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("Database(id={0})", Id);
        }

        public override bool Equals(Object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            Database other = obj as Database;
            return new EqualsBuilder()
                .Append(Id, other.Id)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Id);
            return hash;
        }
    }
}
