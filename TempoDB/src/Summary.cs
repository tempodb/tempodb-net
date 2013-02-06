using System;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class Summary : Dictionary<string, double>
    {
        /// public override bool Equals(object obj)
        /// {
        ///     var other = obj as Summary;
        ///     return other != null &&
        ///         this.Count == other.Count &&
        ///         !this.Except(other).Any();
        /// }

        /// public override int GetHashCode()
        /// {
        ///     int hash = HashCodeHelper.Initialize();
        ///     foreach(KeyValuePair<string, double> kvp in this)
        ///     {
        ///         hash = hash ^ string.Format("k:{0}v{1}", kvp.Key, kvp.Value).GetHashCode();
        ///     }
        ///     return hash;
        /// }
    }
}
