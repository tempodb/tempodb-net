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
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            return hash;
        }
    }
}
