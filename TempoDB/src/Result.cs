using RestSharp;


namespace TempoDB
{
    public class Result<T> where T : class
    {
        private T value;
        private bool success;
        private int code;
        private string message;

        public T Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        public bool Success
        {
            get { return success; }
            private set { this.success = value; }
        }

        public int Code
        {
            get { return code; }
            private set { this.code = value; }
        }

        public string Message
        {
            get { return message; }
            private set { this.message = value; }
        }

        public Result(IRestResponse response)
        {
            int code = (int)response.StatusCode;
            Success = (code / 100) == 2;
            if(Success == true)
            {
                Value = newValueFromResponse(response);
            }
            else
            {
                Message = response.Content;
            }
        }

        public Result(T value, bool success, string message="")
        {
            Value = value;
            Success = success;
            Message = message;
        }

        private T newValueFromResponse(IRestResponse response)
        {
            return null;
        }
    }
}
