using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class Series
    {
        private string id;
        private string key;
        private string name;
        private HashSet<string> tags;
        private IDictionary<string, string> attributes;

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id
        {
            get { return this.id; }
            private set { this.id = value; }
        }

        [JsonProperty(PropertyName = "key", Required = Required.Always)]
        public string Key
        {
            get { return this.key; }
            private set { this.key = value; }
        }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }

        [JsonProperty(PropertyName = "tags", Required = Required.Always)]
        public HashSet<string> Tags
        {
            get { return this.tags; }
            private set { this.tags = value; }
        }

        [JsonProperty(PropertyName = "attributes", Required = Required.Always)]
        public IDictionary<string, string> Attributes
        {
            get { return this.attributes; }
            private set { this.attributes = value; }
        }

        public Series(string id, string key, string name="", HashSet<string> tags=null, Dictionary<string, string> attributes=null)
        {
            Id = id;
            Key = key;
            Name = name;
            Tags = tags == null ? new HashSet<string>() : tags;
            Attributes = attributes == null ? new Dictionary<string, string>() : attributes;
        }

        public override string ToString()
        {
            return string.Format("Series: \n\tid:\t{0}\n\tkey:\t{1}\n\tname:\t{2}\n\ttags:\t{3}\n\tattr:\t{4}", Id, Key, Name, Tags, Attributes);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Series;
            return other != null &&
                Id.Equals(other.Id) &&
                Key.Equals(other.Key) &&
                Name.Equals(other.Name) &&
                TagsEquals(Tags, other.Tags) &&
                AttributesEquals(Attributes, other.Attributes);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Id);
            hash = HashCodeHelper.Hash(hash, Key);
            hash = HashCodeHelper.Hash(hash, Name);
            hash = GetTagsHashCode(hash, Tags);
            hash = GetAttributesHashCode(hash, Attributes);
            return hash;
        }

        private bool TagsEquals(HashSet<string> tags, HashSet<string> other)
        {
            return other != null && tags != null && tags.SetEquals(other);
        }

        private int GetTagsHashCode(int hash, HashSet<string> tags)
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
