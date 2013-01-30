using RestSharp;
using System.Collections.Generic;


namespace Client.Model
{
    public class SegmentEnumerator
    {
        private Segment segment;
        private Client client;

        public SegmentEnumerator(Client client, Segment segment)
        {
            this.client = client;
            this.segment = segment;
        }

        public IEnumerator<Segment> GetEnumerator()
        {
            yield return segment;
            while(segment.NextUrl != null)
            {
                var request = client.BuildRequest(segment.NextUrl, Method.GET);
                var response = client.Execute(request);
                segment = Segment.FromResponse(response);
                yield return segment;
            }
        }
    }
}
