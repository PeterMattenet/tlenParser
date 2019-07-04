using System;
using System.Runtime.Serialization;

namespace TelengParser
{
    internal class ParserUndefinedObjectTypeReferenceException : GoParserException
    {
        private GoTypeKey goObjKey;

        public ParserUndefinedObjectTypeReferenceException(GoTypeKey goObjKey) : base(string.Empty)
        {
            this.goObjKey = goObjKey;

            this.ParserExceptionMessage = $"Referencia en la propiedad en la linea {goObjKey.LineDefinition} de nombre \"{goObjKey.KeyValue}\". Este tipo se encuentra indefinido.";
        }
        
    }
}