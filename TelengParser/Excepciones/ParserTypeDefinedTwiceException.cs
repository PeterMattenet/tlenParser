using System;
using System.Runtime.Serialization;

namespace TelengParser
{
    internal class ParserTypeDefinedTwiceException : GoParserException
    {
        private DslToken matchToken;
        
        public ParserTypeDefinedTwiceException(DslToken matchToken, string message) : base(message)
        {
            this.matchToken = matchToken;

            this.ParserExceptionMessage = $"Hubo un error en el intento de parseo del archivo ingresado, dado que el objeto de tipo {matchToken.Value} fue definido dos veces en el mismo código." +
                $" Revisar la linea {matchToken.LineNumber} del código.";
        }

    }
}