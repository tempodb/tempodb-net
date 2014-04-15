using Newtonsoft.Json;
using NodaTime;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class Summary : Model, IDictionary<string, double>
    {
        private Series series;
        private Interval interval;
        private IDictionary<string, double> data;
        private DateTimeZone timezone;

        public Series Series
        {
            get { return this.series; }
            private set { this.series = value; }
        }

        public Interval Interval
        {
            get { return this.interval; }
            private set { this.interval = value; }
        }

        public DateTimeZone TimeZone
        {
            get { return this.timezone; }
            private set { this.timezone = value; }
        }

        public Summary(Series series, Interval interval, DateTimeZone timezone, Dictionary<string, double> data)
        {
            Series = series;
            Interval = interval;
            TimeZone = timezone;
            this.data = data;
        }

        public int Count { get { return data.Count; } }
        public bool IsReadOnly { get { return data.IsReadOnly; } }
        public double this[string key] { get { return data[key]; } set { data[key] = value; } }
        public ICollection<string> Keys { get { return data.Keys; } }
        public ICollection<double> Values { get { return data.Values; } }
        public void Add(KeyValuePair<string, double> item) { data.Add(item); }
        public void Add(string key, double value) { data.Add(key, value); }
        public void Clear() { data.Clear(); }
        public bool Contains(KeyValuePair<string, double> item) { return data.Contains(item); }
        public bool ContainsKey(string key) { return data.ContainsKey(key); }
        public void CopyTo(KeyValuePair<string, double>[] array, int arrayIndex) { data.CopyTo(array, arrayIndex); }
        public IEnumerator<KeyValuePair<string, double>> GetEnumerator() { return data.GetEnumerator(); }
        public bool Remove(KeyValuePair<string, double> item) { return data.Remove(item); }
        public bool Remove(string key) { return data.Remove(key); }
        public bool TryGetValue(string key, out double value) { return data.TryGetValue(key, out value); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        protected internal static Summary FromResponse(IRestResponse response)
        {
            var summary = JsonConvert.DeserializeObject<Summary>(response.Content);
            return summary;
        }

        public override string ToString()
        {
            return string.Format("Summary(series={0}, interval={1}, timezone={2}, data={3})", Series, Interval, TimeZone, data);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Summary;
            return other != null &&
                Series.Equals(other.Series) &&
                Interval.Equals(other.Interval) &&
                TimeZone.Equals(other.TimeZone) &&
                DataEquals(data, other.data);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Series);
            hash = HashCodeHelper.Hash(hash, Interval);
            hash = HashCodeHelper.Hash(hash, TimeZone);
            hash = GetDataHashCode(hash, data);
            return hash;
        }

        private bool DataEquals(IDictionary<string, double> data, IDictionary<string, double> other)
        {
            return data != null && other != null &&
                data.Count == other.Count &&
                !data.Except(other).Any();
        }

        private int GetDataHashCode(int hash, IDictionary<string, double> data)
        {
            if(data != null)
            {
                foreach(KeyValuePair<string, double> kvp in data)
                {
                    hash = hash ^ string.Format("k:{0}v{1}", kvp.Key, kvp.Value).GetHashCode();
                }
            }
            return hash;
        }
    }
}
