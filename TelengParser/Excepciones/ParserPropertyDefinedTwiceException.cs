using System;
using System.Runtime.Serialization;

namespace TelengParser
{
    internal class ParserPropertyDefinedTwiceException : GoParserException
    {
        private DslToken matchToken;

        public ParserPropertyDefinedTwiceException(DslToken matchToken, string goObjectKey, string message) : base(message)
        {
            this.matchToken = matchToken;

            this.ParserExceptionMessage = $"Hubo un error en el intento de parseo del archivo ingresado, dado que la propiedad {matchToken.Value} fue definida dos veces en el mismo objeto {goObjectKey}." +
                  $" Revisar la linea {matchToken.LineNumber} del código.";
        }

        public ParserPropertyDefinedTwiceException(DslToken matchToken, string message) : base(message)
        {
            this.matchToken = matchToken;

            this.ParserExceptionMessage = $"Hubo un error en el intento de parseo del archivo ingresado, dado que la propiedad {matchToken.Value} fue definida dos veces en un mismo struct de tipo anónimo." +
                  $" Revisar la linea {matchToken.LineNumber} del código.";
        }
    }
    
}