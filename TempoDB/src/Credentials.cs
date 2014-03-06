using System;
using TempoDB.Utility;


namespace TempoDB
{
    public class Credentials
    {
        private string key;
        private string secret;

        public string Key
        {
            get { return key; }
            private set { key = value; }
        }

        public string Secret
        {
            get { return secret; }
            private set { secret = value; }
        }

        public Credentials(string key, string secret)
        {
            Key = key;
            Secret = secret;
        }

        public override string ToString()
        {
            return string.Format("Credentials(key={0}, secret={1})", Key, Secret);
        }

        public override bool Equals(Object obj)
        {
            Credentials other = obj as Credentials;
            return other != null &&
                Key.Equals(other.Key) &&
                Secret.Equals(other.Secret);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Key);
            hash = HashCodeHelper.Hash(hash, Secret);
            return hash;
        }
    }
}
