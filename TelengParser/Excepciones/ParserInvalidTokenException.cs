using System;
using System.Runtime.Serialization;

namespace TelengParser
{
    internal class ParserInvalidTokenException : GoParserException
    {
        private DslToken matchToken;

        public ParserInvalidTokenException(DslToken matchToken) : base(string.Empty)
        {
            this.matchToken = matchToken;

            this.ParserExceptionMessage = $"Hubo un error de parseo en la linea {matchToken.LineNumber} del código ingresado. Revisar el valor: {matchToken.Value}";
        }
        
    }
}