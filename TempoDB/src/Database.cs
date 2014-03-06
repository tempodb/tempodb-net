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
            Database other = obj as Database;
            return other != null &&
                Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Id);
            return hash;
        }
    }
}
