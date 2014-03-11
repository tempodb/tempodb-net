using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using TempoDB.Utility;


namespace TempoDB
{
    public class Status
    {
        private int code;
        private IList<string> messages;

        [JsonProperty(PropertyName="status")]
        public int Code
        {
            get { return code; }
            private set { code = value; }
        }

        [JsonProperty(PropertyName="messages")]
        public IList<string> Messages
        {
            get { return messages; }
            private set { messages = value; }
        }

        public Status(int code, IList<string> messages)
        {
            Code = code;
            Messages = messages;
        }

        public override string ToString()
        {
            return string.Format("Status(code={0}, messages={1})", Code, Messages);
        }

        public override bool Equals(Object obj)
        {
            Status other = obj as Status;
            return other != null &&
                Code.Equals(other.Code) &&
                MessagesEquals(Messages, other.Messages);
        }

        private bool MessagesEquals(IList<string> messages, IList<string> other)
        {
            return messages != null && other != null &&
                messages.Count == other.Count &&
                !messages.Except(other).Any();
        }

        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, Code);
            hash = HashCodeHelper.Hash(hash, Messages);
            return hash;
        }
    }
}
