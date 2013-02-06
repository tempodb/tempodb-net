using System.Collections.Generic;


namespace TempoDB
{
    public class Filter
    {
        private HashSet<string> ids;
        private HashSet<string> keys;
        private HashSet<string> tags;
        private IDictionary<string, string> attributes;

        public HashSet<string> Ids
        {
            get { return this.ids; }
            private set { this.ids = value; }
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

        public Filter(HashSet<string> ids=null, HashSet<string> keys=null, HashSet<string> tags=null, IDictionary<string, string> attributes=null)
        {
            Ids = ids == null ? new HashSet<string>() : ids;
            Keys = keys == null ? new HashSet<string>() : keys;
            Tags = tags == null ? new HashSet<string>() : tags;
            Attributes = attributes == null ? new Dictionary<string, string>() : attributes;
        }
    }
}
