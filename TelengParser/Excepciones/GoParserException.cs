using System;

namespace TelengParser
{
    public class GoParserException : Exception
    {
        public string ParserExceptionMessage { get; protected set; }

        public GoParserException(string message) : base(message)
        {
        }

        public GoParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
    }
}