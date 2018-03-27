using System;
using System.Runtime.Serialization;

namespace Simple
{
    [Serializable]
    internal class ParseException : Exception
    {
        private string v;
        private int start;

        public ParseException()
        {
        }

        public ParseException(string message) : base(message)
        {
        }

        public ParseException(string v, int start)
        {
            this.v = v;
            this.start = start;
        }

        public ParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}