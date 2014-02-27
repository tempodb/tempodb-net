using RestSharp;
using TempoDB.Utility;


namespace TempoDB
{
    public class Nothing : Model
    {
        protected internal static Nothing FromResponse(IRestResponse response)
        {
            return new Nothing();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Nothing;
            return other != null;
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            return hash;
        }
    }
}
