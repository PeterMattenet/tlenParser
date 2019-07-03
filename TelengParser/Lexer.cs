using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser
{
    public class Lexer
    {
        private List<TokenDefinition> _tokenDefinitions;

        public Lexer()
        {
            //El lexer va a tokenizar cuando un match esta contenido en otro, el que arranca primero, por eso se resuelven problemas de ambigueadad como "int" y "sINTetizar", de manera automatica
            _tokenDefinitions = new List<TokenDefinition>();
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Int, "int[;\r\t\n} ]", 1));
            //Int se tokeniza si lo que le sigue a "int" es un espacio, tab, salto de linea, cosa de que las claves con prefijo "int", como "inteligencia" no sean tokenizadas incorrectamente
            _tokenDefinitions.Add(new TokenDefinition(TokenType.String, "string[;\r\t\n} ]", 1));
            //Idem string
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Float64, "float64[;\r\t\n} ]", 1));
            //Idem Float
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Bool, "bool[;\r\t\n} ]", 1));
            //Idem bool
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Array, "[\\[][\\]]", 1)); 
            //Este tiene mayor precedencia que el token ID, por lo que si un match comienza con [], este sera el token que se priorize
            _tokenDefinitions.Add(new TokenDefinition(TokenType.StructLeftKey, "{", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.StructRightKey, "}", 1));
            //_tokenDefinitions.Add(new TokenDefinition(TokenType.EOF, "=", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Struct, "struct(\r|\t|\n| |{)", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Type, "type(\r|\t|\n| )", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Id, "[a-zA-Z_][a-zA-Z_0-9]*", 2));
            //Fuerzo a que los token Id no puedan comenzar con [] para evitar que matchee con []asdfasf en vez de asdfasdf
        }

        public IEnumerable<DslToken> Tokenize(string inputMessage)
        {
            var tokenMatches = FindTokenMatches(inputMessage);

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                .OrderBy(x => x.Key)
                .ToList();


            TokenMatch lastMatch = null;
            for (int i = 0; i < groupedByIndex.Count; i++)
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;

                yield return new DslToken(bestMatch.TokenType, bestMatch.Value);

                lastMatch = bestMatch;
            }

            yield return new DslToken(TokenType.EOF);
        }

        private List<TokenMatch> FindTokenMatches(string inputMessage)
        {
            var tokenMatches = new List<TokenMatch>();

            foreach (var tokenDefinition in _tokenDefinitions)
                tokenMatches.AddRange(tokenDefinition.FindMatches(inputMessage).ToList());

            return tokenMatches;
        }

    }
}
