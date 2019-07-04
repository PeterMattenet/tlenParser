using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoStruct : GoProperty
    {
        public Dictionary<string, GoProperty> _properties;

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
            ExpandoObject mainObjectInstance = new ExpandoObject();

            var mainObjectInstanceDict = mainObjectInstance as IDictionary<string, dynamic>;

            var propertyIterator = _properties.Keys.GetEnumerator();

            while (propertyIterator.MoveNext())
            {
                GoProperty currProp = _properties[propertyIterator.Current];

                dynamic propertyInstance = currProp.GenerateTrashValue(goInstance);

                mainObjectInstanceDict.Add(propertyIterator.Current, propertyInstance);
            }

            return mainObjectInstanceDict;
        }

        public GoStruct()
        {
            _properties = new Dictionary<string, GoProperty>();
        }
    }
}
