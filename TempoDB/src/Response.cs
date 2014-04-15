using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using TempoDB.Json;
using TempoDB.Utility;


namespace TempoDB
{
    public class Response<T> where T : Model
    {
        private static SingleValueConverter singlevalueConverter = new SingleValueConverter();

        public T Value { get; private set; }

        public bool Success { get; private set; }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public MultiStatus MultiStatus { get; private set; }

        public State State
        {
            get { return getState(Code); }
        }

        public Response(IRestResponse response) : this(response, typeof(T)) { }

        public Response(IRestResponse response, Type type)
        {
            T value = null;
            int code = (int)response.StatusCode;
            string message = response.StatusDescription;
            MultiStatus multistatus = null;

            switch(getState(code))
            {
                case State.Success:
                    value = newValueFromResponse(response, type);
                    break;
                case State.PartialSuccess:
                    multistatus = JsonConvert.DeserializeObject<MultiStatus>(response.Content);
                    break;
                case State.Failure:
                    message = messageFromResponse(response);
                    break;
            }

            Value = value;
            Code = code;
            Message = message;
            MultiStatus = multistatus;
        }

        public Response(T v, int code, string message="", MultiStatus multistatus=null)
        {
            Value = v;
            Code = code;
            Success = IsSuccessful(code);
            Message = message;
            MultiStatus = multistatus;
        }

        public static bool IsSuccessful(int code)
        {
            return (code / 100) == 2;
        }

        private static State getState(int code)
        {
            State state = State.Failure;
            if((code / 100) == 2)
            {
                state = code == 207 ? State.PartialSuccess : State.Success;
            }
            return state;
        }

        private T newValueFromResponse(IRestResponse response, Type type)
        {
            if(type == typeof(Series))
            {
                return Series.FromResponse(response) as T;
            }
            else if(type == typeof(DeleteSummary))
            {
                return DeleteSummary.FromResponse(response) as T;
            }
            else if(type == typeof(Nothing))
            {
                return Nothing.FromResponse(response) as T;
            }
            else if(type == typeof(SingleValue))
            {
                return SingleValue.FromResponse(response) as T;
            }
            else if(type == typeof(Summary))
            {
                return Summary.FromResponse(response) as T;
            }
            else if(type == typeof(Segment<Series>))
            {
                var series = JsonConvert.DeserializeObject<List<Series>>(response.Content);
                var nextUrl = HttpHelper.GetLinkFromHeaders("next", response);
                var segment = new Segment<Series>(series, nextUrl);
                return segment as T;
            }
            else if(type == typeof(Segment<SingleValue>))
            {
                var value = JsonConvert.DeserializeObject<List<SingleValue>>(response.Content, singlevalueConverter);
                var nextUrl = HttpHelper.GetLinkFromHeaders("next", response);
                var segment = new Segment<SingleValue>(value, nextUrl);
                return segment as T;
            }
            else if(type == typeof(DataPointSegment))
            {
               return DataPointSegment.FromResponse(response) as T;
            }
            else if(type == typeof(MultiDataPointSegment))
            {
               return MultiDataPointSegment.FromResponse(response) as T;
            }
            else if(type == typeof(DataPointFoundSegment))
            {
               return DataPointFoundSegment.FromResponse(response) as T;
            }
            else if(type == typeof(MultiRollupDataPointSegment))
            {
               return MultiRollupDataPointSegment.FromResponse(response) as T;
            }

            throw new Exception("Unknown T: " + type.ToString());
        }

        private string messageFromResponse(IRestResponse response)
        {
            string message = response.Content;
            if(message.Equals(null) || message.Equals(""))
            {
                message = response.StatusDescription;
            }
            return message;
        }

        public override bool Equals(object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if(obj.GetType() != GetType()) { return false; }

            var other = obj as Response<T>;
            return new EqualsBuilder()
                .Append(Value, other.Value)
                .Append(Code, other.Code)
                .Append(Message, other.Message)
                .Append(MultiStatus, other.MultiStatus)
                .IsEquals();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Value);
            hash = HashCodeHelper.Hash(hash, Code);
            hash = HashCodeHelper.Hash(hash, Message);
            hash = HashCodeHelper.Hash(hash, State);
            hash = HashCodeHelper.Hash(hash, MultiStatus);
            return hash;
        }
    }
}
