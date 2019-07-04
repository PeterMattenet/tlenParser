namespace TelengParser
{
    internal class ParserDependencyCycleException : GoParserException
    {
        public ParserDependencyCycleException(GoObject firstObject, GoTypeKey firstObjectProperty, GoObject secondObject ) : base(string.Empty)
        {
            ParserExceptionMessage = $"Hubo un error de dependencias en el código ingresado. El objeto de tipo \"{firstObject.ObjectType}\" definido en la linea {firstObject.LineDefinition}, " +
                $"tiene una propiedad en la linea {firstObjectProperty.LineDefinition}, de tipo \"{secondObject.ObjectType}\", del cual ya previamente dependía.";
        }

    }
}