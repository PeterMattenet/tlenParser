using System.Collections.Generic;
using TelengParser.GoData;

namespace TelengParser
{
    public class GoTypeKey : GoProperty
    {
        public string KeyValue;

        public int LineDefinition;

        public override void FailIfUndefinedInParser(GoParser parser)
        {
            parser.IsPropertyDefined(this);
        }

        public override void AddToTypeNavigationQueue(Queue<GoObject> queue, GoParser parser)
        {
            parser.AddGoPropertyToNavigationQueue(this, queue);
        }

        public override void NavigateDependenciesTo(Stack<GoObject> pathTaken, GoParser parser)
        {
            parser.NavigateDependenciesInto(this, pathTaken);
        }

        public override dynamic GenerateTrashValue(GoInstance goInstance)
        {
            return goInstance.GetRandomInstanceObject(this.KeyValue);
        }

        public GoTypeKey(string key, int lineDefinition)
        {
            this.KeyValue = key;
            this.LineDefinition = lineDefinition;
        }
    }
}