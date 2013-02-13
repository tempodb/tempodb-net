using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Utility;


namespace TempoDB
{
    public class Cursor<T> : Model where T: Model
    {
        private SegmentEnumerator<T> segments;

        public Cursor(SegmentEnumerator<T> segments)
        {
            this.segments = segments;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(Segment<T> segment in segments)
            {
                foreach(T item in segment)
                {
                    yield return item;
                }
            }
        }
    }

    public class SegmentEnumerator<T> where T: Model
    {
        private Segment<T> segment;
        private TempoDB client;

        public SegmentEnumerator(TempoDB client, Segment<T> initial)
        {
            this.client = client;
            this.segment = initial;
        }

        public IEnumerator<Segment<T>> GetEnumerator()
        {
            yield return segment;
            while(String.IsNullOrEmpty(segment.NextUrl) == false)
            {
                // Add rest call here
                var request = client.BuildRequest(segment.NextUrl, Method.GET);
                var result = client.Execute<Segment<T>>(request);
                if(result.Success)
                {
                    segment = result.Value;
                    yield return segment;
                }
                else
                {
                    throw new Exception("API Error");
                }
            }
        }
    }

    public class Segment<T> : Model where T: Model
    {
        private IList<T> data;
        private string next;

        [JsonIgnore]
        public string NextUrl
        {
            get { return next; }
            set { this.next = value; }
        }

        [JsonProperty(PropertyName="data")]
        public IList<T> Data
        {
            get { return data; }
            protected set { this.data = value; }
        }

        public Segment(IList<T> data, string next)
        {
            this.data = data;
            this.next = next;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(T item in data)
            {
                yield return item;
            }
        }
    }
}
