using System.Collections.Generic;


namespace Client.Model
{

    /// <summary>
    ///  Represents a filter on the set of Series. This is used to query a set of series with specific
    ///  properties. A filter can include ids, keys, tags and attributes. An empty filter is created
    ///  and filter predicates are added. For example a filter to return series with keys myagley-1 and
    ///  myagley-2 looks like this:
    ///  <pre>
    ///  {@code
    ///  Filter filter = new Filter();
    ///  filter.addKey("myagley-1");
    ///  filter.addKey("myagley-1");
    ///  }
    ///  </pre>
    /// </summary>
    public class Filter
    {
        private readonly IList<string> _ids = new List<string>();
        private readonly IList<string> _keys = new List<string>();
        private readonly IList<string> _tags = new List<string>();
        private readonly IDictionary<string, string> _attributes = new Dictionary<string, string>();

        /// <summary>
        ///  Adds an id to the filter.
        /// </summary>
        ///  <param name="id"> The id to add </param>
        public void AddId(string id)
        {
            _ids.Add(id);
        }

        /// <summary>
        ///  Adds a key to the filter.
        /// </summary>
        ///  <param name="key"> The key to add </param>
        public void AddKey(string key)
        {
            _keys.Add(key);
        }

        /// <summary>
        ///  Adds a tag to the filter.
        /// </summary>
        ///  <param name="tag"> The tag to add </param>
        public void AddTag(string tag)
        {
            _tags.Add(tag);
        }

        /// <summary>
        ///  Adds an attribute to the filter.
        /// </summary>
        ///  <param name="key"> The attribute key </param>
        ///  <param name="value"> The attribute value </param>

        public void AddAttribute(string key, string value)
        {
            _attributes[key] = value;
        }

        public IList<string> Ids
        {
            get
            {
                return _ids;
            }
        }
        public IList<string> Keys
        {
            get
            {
                return _keys;
            }
        }
        public IList<string> Tags
        {
            get
            {
                return _tags;
            }
        }
        public IDictionary<string, string> Attributes
        {
            get
            {
                return _attributes;
            }
        }
    }
}
