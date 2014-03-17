using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Utility;


namespace TempoDB
{
    public class Response<T> where T : Model
    {
        public T Value { get; private set; }

        public bool Success { get; private set; }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public MultiStatus MultiStatus { get; private set; }

        public State State
        {
            get
            {
                State state = State.Failure;
                if((Code / 100) == 2)
                {
                    state = Code == 207 ? State.PartialSuccess : State.Success;
                }
                return state;
            }
        }

        public Response(IRestResponse response)
        {
            Code = (int)response.StatusCode;
            Success = IsSuccessful(Code);
            if(Success == true)
            {
                Value = newValueFromResponse(response);
                Message = "";
            }
            else
            {
                Message = response.Content;
            }
        }

        public Response(T value, int code, string message="", MultiStatus multistatus=null)
        {
            Value = value;
            Code = code;
            Success = IsSuccessful(code);
            Message = message;
            MultiStatus = multistatus;
        }

        public static bool IsSuccessful(int code)
        {
            return (code / 100) == 2;
        }

        private T newValueFromResponse(IRestResponse response)
        {
            if(typeof(T) == typeof(Series))
            {
                return Series.FromResponse(response) as T;
            }
            else if(typeof(T) == typeof(Nothing))
            {
                return Nothing.FromResponse(response) as T;
            }
            else if(typeof(T) == typeof(Segment<Series>))
            {
                var series = JsonConvert.DeserializeObject<List<Series>>(response.Content);
                var nextUrl = HttpHelper.GetLinkFromHeaders("next", response);
                var segment = new Segment<Series>(series, nextUrl);
                return segment as T;
            }
            else if((typeof(T) == typeof(Segment<DataPoint>)) || (typeof(T) == typeof(DataPointSegment)))
            {
               return DataPointSegment.FromResponse(response) as T;
            }

            throw new Exception("Unknown T: " + typeof(T).ToString());
        }

        public override bool Equals(object obj)
        {
            var other = obj as Response<T>;
            return other != null &&
                Value.Equals(other.Value) &&
                Code.Equals(other.Code) &&
                Success.Equals(other.Success) &&
                Message.Equals(other.Message);
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Value);
            hash = HashCodeHelper.Hash(hash, Code);
            hash = HashCodeHelper.Hash(hash, Success);
            hash = HashCodeHelper.Hash(hash, Message);
            return hash;
        }
    }
}
