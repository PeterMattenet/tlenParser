using System;
using System.Collections.Generic;
using TelengParser.GoData;
using System.Linq;

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
            var dslToken = _iterator.Current;

            if (dslToken.TokenType != tokenExpected)
            {
                throw new ParserInvalidTokenException(dslToken);
            }

            _iterator.MoveNext();

            return dslToken;
        }

        public GoInstance SetAndValidateInput(IEnumerable<DslToken> tokenList)
        {
            _iterator = tokenList.GetEnumerator();

            _iterator.MoveNext();

            var mainObject = string.Empty;

            try
            {
                CallRoutineS(out Dictionary<string, GoObject> _contentInstance, out mainObject);

                this._contentInstance = _contentInstance;

                CheckNoMissingDependencies();

                CheckNoCircularDependencies();

                MatchToken(TokenType.EOF);
            }
            catch (GoParserException parserEx)
            {
                //Outputtear esto despues por consola cuando fallo por parseo
                var err = parserEx.ParserExceptionMessage;
            }
            catch (Exception)
            {
                //RIP la vida
                throw;
            }

            return new GoInstance(_contentInstance, mainObject);
        }

        private void CheckNoMissingDependencies()
        {
            foreach (GoObject obj in _contentInstance.Values)
            {
                foreach (GoProperty prop in obj._properties.Values)
                {
                    prop.FailIfUndefinedInParser(this);
                }
            }
        }

        private void CheckNoCircularDependencies()
        {
            var types = _contentInstance.Values;

            if (types.Count == 0)
                return;

            var iterator = types.GetEnumerator();

            //Itero en todos porque a) si el grafo tiene componentes inconexas entonces hay que probar el algoritmo arrancando por todo tipo, b) podriamos guardarnos los recorridos pero pinto paja
            bool hasNext = iterator.MoveNext();

            while (hasNext)
            {
                Stack<GoObject> pathTaken = new Stack<GoObject>();

                pathTaken.Push(iterator.Current);

                TraverseDependencies(pathTaken);

                hasNext = iterator.MoveNext();
            }


        }

        private void TraverseDependencies(Stack<GoObject> pathTaken)
        {
            var currentObject = pathTaken.Peek();

            foreach (GoProperty goProp in currentObject._properties.Values)
            {
                goProp.NavigateDependenciesTo(pathTaken, this);
            }
        }


        #region Parser Routine Calls
        private void CallRoutineS(out Dictionary<string, GoObject> entityDictionary, out string mainObject)
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

                CallRoutineS(out entityDictionary, out mainObject);

                currentGoObject.LineDefinition = matchToken.LineNumber;
                currentGoObject.ObjectType = currentGoObjectKey;

                try
                {
                    entityDictionary.Add(currentGoObjectKey, currentGoObject);
                }
                catch (ArgumentException ex)
                {
                    throw new ParserTypeDefinedTwiceException(matchToken, ex.Message);
                }
                
                mainObject = currentGoObjectKey;
            }
            else
            {
                mainObject = string.Empty;
                entityDictionary = new Dictionary<string, GoObject>();
            }

        }

        private void CallRoutineST(out GoObject currentGoObject)
        {

            if (_iterator.Current.TokenType == TokenType.Id)
            {
                var matchToken = MatchToken(TokenType.Id);
                var currentGoPropertyKey = matchToken.Value;

                CallRoutineTY(out GoProperty currentGoProperty);
                
                CallRoutineST(out currentGoObject);

                try
                {
                    currentGoObject._properties.Add(currentGoPropertyKey, currentGoProperty);
                }
                catch (ArgumentException ex)
                {
                    throw new ParserPropertyDefinedTwiceException(matchToken, currentGoObject.ObjectType, ex.Message);
                }
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
                var matchToken = MatchToken(TokenType.Id);
                var currentGoPropertyKey = matchToken.Value;

                CallRoutineTY(out GoProperty currentGoStructProperty);

                CallRoutineSTR(out currentGoStruct);

                try
                {
                    currentGoStruct._properties.Add(currentGoPropertyKey, currentGoStructProperty);
                }
                catch (ArgumentException ex)
                {
                    throw new ParserPropertyDefinedTwiceException(matchToken, ex.Message);
                }
            }
            else
            {
                currentGoStruct = new GoStruct();
            }
        }

        //private void CallRoutineTYA(out GoArray currentGoArray)
        //{

        //    switch (_iterator.Current.TokenType)
        //    {
        //        case TokenType.Array:
        //            MatchToken(TokenType.Array);
        //            CallRoutineTYA(out GoProperty childGoArray);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = childGoArray
        //            currentGoArray.Dimensions++;
        //            break;

        //        case TokenType.Int:
        //            MatchToken(TokenType.Int);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = new GoInt();
        //            break;

        //        case TokenType.String:
        //            MatchToken(TokenType.String);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = new GoString();
        //            break;

        //        case TokenType.Float64:
        //            MatchToken(TokenType.Float64);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = new GoFloat();
        //            break;

        //        case TokenType.Bool:
        //            MatchToken(TokenType.Bool);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = new GoBool();
        //            break;

        //        case TokenType.Id:
        //            MatchToken(TokenType.Id);
        //            currentGoArray = new GoArray();
        //            var dslMatch = MatchToken(TokenType.Id);
        //            var currentGoPropertyKey = dslMatch.Value;
        //            currentGoArray.ArrayProperty = new GoTypeKey(currentGoPropertyKey, dslMatch.LineNumber);
        //            break;

        //        case TokenType.Struct:
        //            MatchToken(TokenType.Struct);
        //            MatchToken(TokenType.StructLeftKey);
        //            CallRoutineSTR(out GoStruct arrayStructProperty);
        //            currentGoArray = new GoArray();
        //            currentGoArray.ArrayProperty = arrayStructProperty;
        //            MatchToken(TokenType.StructRightKey);
        //            break;
        //        default:
        //            throw new System.Exception("Invalid token type for array");
        //    }

        //}
        
        private void CallRoutineTY(out GoProperty currentGoProperty)
        {

            switch (_iterator.Current.TokenType)
            {
                case TokenType.Array:
                    MatchToken(TokenType.Array);
                    CallRoutineTY(out GoProperty childGoProperty);
                    GoArray currentGoArray = new GoArray();
                    currentGoArray.ArrayProperty = childGoProperty;
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
                    currentGoProperty = new GoTypeKey(currentGoPropertyKey, dslMatch.LineNumber);
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
        #endregion

        #region Double Dispatch Methods

        public void NavigateDependenciesInto(GoTypeKey goTypeKey, Stack<GoObject> pathTaken)
        {
            if (pathTaken.Any(obj => obj.ObjectType == goTypeKey.KeyValue))
                throw new ParserDependencyCycleException(pathTaken.Peek(), goTypeKey, _contentInstance[goTypeKey.KeyValue]);

            pathTaken.Push(_contentInstance[goTypeKey.KeyValue]);
            TraverseDependencies(pathTaken);
            pathTaken.Pop();
        }

        public void NavigateDependenciesInto(GoStruct goStruct, Stack<GoObject> pathTaken)
        {
            foreach (GoProperty goProp in goStruct._properties.Values)
            {
                goProp.NavigateDependenciesTo(pathTaken, this);
            }
        }

        public void NavigateDependenciesInto(GoArray goArray, Stack<GoObject> pathTaken)
        {
            goArray.ArrayProperty.NavigateDependenciesTo(pathTaken, this);
        }

        public void AddGoPropertyToNavigationQueue(GoArray goArray, Queue<GoObject> navigationQueue)
        {
            goArray.ArrayProperty.AddToTypeNavigationQueue(navigationQueue, this);
        }

        public void AddGoPropertyToNavigationQueue(GoTypeKey goTypeKey, Queue<GoObject> navigationQueue)
        {
            navigationQueue.Enqueue(_contentInstance[goTypeKey.KeyValue]);
        }

        public void AddGoPropertyToNavigationQueue(GoStruct goStruct, Queue<GoObject> navigationQueue)
        {
            foreach (GoProperty goProp in goStruct._properties.Values)
            {
                goProp.AddToTypeNavigationQueue(navigationQueue, this);
            }
        }

        public void IsPropertyDefined(GoArray goArray)
        {
            goArray.ArrayProperty.FailIfUndefinedInParser(this);
        }

        public void IsPropertyDefined(GoStruct goStruct)
        {
            foreach (GoProperty prop in goStruct._properties.Values)
                prop.FailIfUndefinedInParser(this);
        }

        public void IsPropertyDefined(GoTypeKey goObjKey)
        {
            if (!_contentInstance.ContainsKey(goObjKey.KeyValue))
                throw new ParserUndefinedObjectTypeReferenceException(goObjKey);
        }
        #endregion
    }
}