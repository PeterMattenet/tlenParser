using System.Collections.Generic;
using TelengParser.MierdaDeGo;

namespace TelengParser
{
    public class GoParser
    {
        private IEnumerator<DslToken> _iterator;

        private Dictionary<string, GoObject> _contentInstance = new Dictionary<string, GoObject>();

        public GoParser()
        {

        }

        private DslToken MatchToken(TokenType tokenExpected)
        {
            if (_iterator.Current.TokenType != tokenExpected)
            {
                throw new System.Exception("Invalid input");
            }

            var dslToken = _iterator.Current;

            _iterator.MoveNext();

            return dslToken;
        }

        public Dictionary<string,GoObject> SetAndValidateInput(IEnumerable<DslToken> tokenList)
        {
            _iterator = tokenList.GetEnumerator();

            _iterator.MoveNext();

            CallRoutineS(out Dictionary<string,GoObject> _contentInstance);

            MatchToken(TokenType.EOF);

            return _contentInstance;
        }

        private void CallRoutineS(out Dictionary<string, GoObject> entityDictionary)
        {
            DslToken matchToken;

            if (_iterator.Current.TokenType == TokenType.Type)
            {
                MatchToken(TokenType.Type);
                matchToken = MatchToken(TokenType.Id);

                var currentGoObjectKey = matchToken.Value;

                MatchToken(TokenType.Struct);
                MatchToken(TokenType.StructLeftKey);

                CallRoutineST(out GoObject currentGoObject);

                MatchToken(TokenType.StructRightKey);

                CallRoutineS(out entityDictionary);

                currentGoObject.ObjectType = currentGoObjectKey;
                entityDictionary.Add(currentGoObjectKey, currentGoObject);
            }
            else
            {
                entityDictionary = new Dictionary<string, GoObject>();
            }
            
        }

        private void CallRoutineST(out GoObject currentGoObject)
        {

            if (_iterator.Current.TokenType == TokenType.Id)
            {
                var dslMatch = MatchToken(TokenType.Id);
                var currentGoPropertyKey = dslMatch.Value;

                CallRoutineTY(out GoProperty currentGoProperty);

                //Traeme un goObject de mi hijo
                CallRoutineST(out currentGoObject);

                currentGoObject._properties.Add(currentGoPropertyKey, currentGoProperty);
            }
            else
            {
                currentGoObject = new GoObject();
            }
               
            //CallRoutineSTR(ref currentGoObject);
        }

        private void CallRoutineSTR(out GoStruct currentGoStruct)
        {
            if (_iterator.Current.TokenType == TokenType.Id)
            {
                var dslMatch = MatchToken(TokenType.Id);
                var currentGoPropertyKey = dslMatch.Value;

                CallRoutineTY(out GoProperty currentGoStructProperty);

                //Traeme un goObject de mi hijo
                CallRoutineSTR(out currentGoStruct);

                currentGoStruct._properties.Add(currentGoPropertyKey, currentGoStructProperty);
            }
            else
            {
                currentGoStruct = new GoStruct();
            }
        }

        private void CallRoutineTYA(out GoArray currentGoArray)
        {

            switch (_iterator.Current.TokenType)
            {
                case TokenType.Array:
                    MatchToken(TokenType.Array);
                    CallRoutineTYA(out currentGoArray);
                    currentGoArray.Dimensions++;
                    break;

                case TokenType.Int:
                    MatchToken(TokenType.Int);
                    currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = new GoInt(); 
                    break;

                case TokenType.String:
                    MatchToken(TokenType.String);
                    currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = new GoString();
                    break;

                case TokenType.Float64:
                    MatchToken(TokenType.Float64);
                    currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = new GoFloat();
                    break;

                case TokenType.Bool:
                    MatchToken(TokenType.Bool);
                    currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = new GoBool();
                    break;

                case TokenType.Id:
                    MatchToken(TokenType.Id);
                    currentGoArray = new GoArray();
                    var dslMatch = MatchToken(TokenType.Id);
                    var currentGoPropertyKey = dslMatch.Value;
                    currentGoArray.ArrayProperty = new GoTypeKey(currentGoPropertyKey);
                    break;

                case TokenType.Struct:
                    MatchToken(TokenType.Struct);
                    MatchToken(TokenType.StructLeftKey);
                    CallRoutineSTR(out GoStruct arrayStructProperty);
                    currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = arrayStructProperty;
                    MatchToken(TokenType.StructRightKey);
                    break;
                default:
                    throw new System.Exception("Invalid token type for array");
            }

        }

        //private void CallRoutineSTA()

        private void CallRoutineTY(out GoProperty currentGoProperty)
        {

            switch (_iterator.Current.TokenType)
            {
                case TokenType.Array:
                    MatchToken(TokenType.Array);
                    CallRoutineTYA(out GoArray currentGoArray);
                    currentGoProperty = currentGoArray;
                    break;

                case TokenType.Int:
                    MatchToken(TokenType.Int);
                    currentGoProperty = new GoInt();
                    break;

                case TokenType.String:
                    MatchToken(TokenType.String);
                    currentGoProperty = new GoString();
                    break;

                case TokenType.Float64:
                    MatchToken(TokenType.Float64);
                    currentGoProperty = new GoFloat();
                    break;

                case TokenType.Bool:
                    MatchToken(TokenType.Bool);
                    currentGoProperty = new GoBool();
                    break;
                case TokenType.Id:
                    var dslMatch = MatchToken(TokenType.Id);
                    var currentGoPropertyKey = dslMatch.Value;
                    currentGoProperty = new GoTypeKey(currentGoPropertyKey);
                    break;
                case TokenType.Struct:
                    MatchToken(TokenType.Struct);
                    MatchToken(TokenType.StructLeftKey);
                    CallRoutineSTR(out GoStruct goStructProperty);
                    currentGoProperty = goStructProperty;
                    MatchToken(TokenType.StructRightKey);
                    break;
                default:
                    throw new System.Exception("Invalid property token");
            }
        }
    }
}