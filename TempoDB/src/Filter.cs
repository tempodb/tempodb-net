using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class Filter
    {
        private HashSet<string> keys;
        private HashSet<string> tags;
        private IDictionary<string, string> attributes;

        public Filter(HashSet<string> keys=null, HashSet<string> tags=null, IDictionary<string, string> attributes=null)
        {
            Keys = keys == null ? new HashSet<string>() : keys;
            Tags = tags == null ? new HashSet<string>() : tags;
            Attributes = attributes == null ? new Dictionary<string, string>() : attributes;
        }

        public HashSet<string> Keys
        {
            get { return this.keys; }
            private set { this.keys = value; }
        }

        public HashSet<string> Tags
        {
            get { return this.tags; }
            private set { this.tags = value; }
        }

        public IDictionary<string, string> Attributes
        {
            get { return this.attributes; }
            private set { this.attributes = value; }
        }

        public Filter AddKeys(string key)
        {
            this.keys.Add(key);
            return this;
        }

        public Filter AddKeys(params string[] keys)
        {
            foreach(string key in keys)
            {
                this.keys.Add(key);
            }
            return this;
        }

        public Filter AddKeys(HashSet<string> keys)
        {
            foreach(string key in keys)
            {
                this.keys.Add(key);
            }
            return this;
        }

        public Filter AddTags(string tag)
        {
            this.tags.Add(tag);
            return this;
        }

        public Filter AddTags(params string[] tags)
        {
            foreach(string tag in tags)
            {
                this.tags.Add(tag);
            }
            return this;
        }
        public Filter AddTags(HashSet<string> tags)
        {
            foreach(string tag in tags)
            {
                this.tags.Add(tag);
            }
            return this;
        }

        public Filter AddAttributes(string key, string value)
        {
            this.attributes.Add(key, value);
            return this;
        }

        public Filter AddAttributes(IDictionary<string, string> attributes)
        {
            foreach(var item in attributes)
            {
                this.attributes.Add(item.Key, item.Value);
            }
            return this;
        }

        public override string ToString()
        {
            return string.Format("Filter: \n\tkeys:\t{0}\n\ttags:\t{1}\n\tattr:\t{2}", Keys, Tags, Attributes);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Filter;
            return other != null &&
                SetEquals(Keys, other.Keys) &&
                SetEquals(Tags, other.Tags) &&
                AttributesEquals(Attributes, other.Attributes);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = GetSetHashCode(hash, Keys);
            hash = GetSetHashCode(hash, Tags);
            hash = GetAttributesHashCode(hash, Attributes);
            return hash;
        }

        private bool SetEquals(HashSet<string> tags, HashSet<string> other)
        {
            return other != null && tags != null && tags.SetEquals(other);
        }

        private int GetSetHashCode(int hash, HashSet<string> tags)
        {
            if(tags != null)
            {
                foreach(string tag in tags)
                {
                    hash = hash ^ tag.GetHashCode();
                }
            }
            return hash;
        }

        private bool AttributesEquals(IDictionary<string, string> attributes, IDictionary<string, string> other)
        {
            return attributes != null && other != null &&
                attributes.Count == other.Count &&
                !attributes.Except(other).Any();
        }

        private int GetAttributesHashCode(int hash, IDictionary<string, string> attributes)
        {
            if(attributes != null)
            {
                foreach(KeyValuePair<string, string> kvp in attributes)
                {
                    hash = hash ^ string.Format("k:{0}v{1}", kvp.Key, kvp.Value).GetHashCode();
                }
            }
            return hash;
        }
    }
}
