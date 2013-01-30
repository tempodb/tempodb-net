using System.Collections.Generic;


namespace Client.Model
{
    public class Cursor
    {
        private SegmentEnumerator segments;

        public Cursor(SegmentEnumerator segments)
        {
            this.segments = segments;
        }

        public IEnumerator<DataPoint> GetEnumerator()
        {
            foreach(Segment segment in segments)
            {
                foreach(DataPoint dp in segment)
                {
                    yield return dp;
                }
            }
        }
    }
}
