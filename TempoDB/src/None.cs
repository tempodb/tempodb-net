using RestSharp;
using TempoDB.Utility;


namespace TempoDB
{
    public class None : Model
    {
        protected internal static None FromResponse(IRestResponse response)
        {
            return new None();
        }

        public override bool Equals(object obj)
        {
            var other = obj as None;
            return other != null;
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            return hash;
        }
    }
}
