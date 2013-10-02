using Newtonsoft.Json;
using System;

namespace Client.Model
{
    /// <summary>
    ///  The abstract parent class representing a datapoint used in a multi write. Multi
    ///  writing allows values for different series to be written for multiple timestamps in one
    ///  Rest call. The series can be referenced by series id or series key. The two subclasses
    ///  represent these two options.
    /// </summary>

    public abstract class MultiPoint
    {
        [JsonProperty(PropertyName = "t")]
        public DateTime Timestamp { get; protected set; }

        [JsonProperty(PropertyName = "v")]
        public object Value { get; protected set; }
    }
}
