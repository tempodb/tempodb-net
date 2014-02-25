using System;


namespace TempoDB.Exceptions
{
    public class TempoDBException : Exception
    {
        public TempoDBException(string message) : base(message) {}
        public TempoDBException() : base() {}
    }
}
