using System.Collections.Generic;


namespace TempoDB
{
    public class Cursor<T>
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

    public class SegmentEnumerator<T>
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
            while(segment.NextUrl != null)
            {
                // Add rest call here
                yield return segment;
            }
        }
    }

    public class Segment<T>
    {
        private IList<T> data;
        private string next;

        public string NextUrl
        {
            get { return next; }
            private set { this.next = value; }
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
