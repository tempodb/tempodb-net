using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class MultiStatus
    {
        private IList<Status> statuses;

        [JsonProperty(PropertyName="multistatus")]
        public IList<Status> Statuses
        {
            get { return statuses; }
            private set { this.statuses = value; }
        }

        public MultiStatus(IList<Status> statuses)
        {
            Statuses = statuses;
        }

        public IEnumerator GetEnumerator()
        {
            foreach(Status status in statuses)
            {
                yield return status;
            }
        }

        public override string ToString()
        {
            return string.Format("MultiStatus(statuses={0})", Statuses);
        }

        public override bool Equals(Object obj)
        {
            MultiStatus other = obj as MultiStatus;
            return other != null &&
                StatusesEquals(Statuses, other.Statuses);
        }

        public bool StatusesEquals(IList<Status> statuses, IList<Status> other)
        {
            return statuses != null && other != null &&
                statuses.Count == other.Count &&
                statuses.Zip(other, (first, second) => first.Equals(second)).All(item => item);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Statuses);
            return hash;
        }
    }
}
