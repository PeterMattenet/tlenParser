using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoArray : GoProperty
    {

        public GoProperty ArrayProperty { get; set; }
        

        public override void AddToTypeNavigationQueue(Queue<GoObject> queue, GoParser parser)
        {
            parser.AddGoPropertyToNavigationQueue(this, queue);
        }

        public override void NavigateDependenciesTo(Stack<GoObject> pathTaken, GoParser parser)
        {
            parser.NavigateDependenciesInto(this, pathTaken);
        }

        public override void FailIfUndefinedInParser(GoParser parser)
        {
            parser.IsPropertyDefined(this);
        }

        public override dynamic GenerateTrashValue(GoInstance goInstance)
        {
            int length = rand.Next(1, 5);

            dynamic[] array = new dynamic[length];

            for (int j = 0; j < length; j++)
            {
                array[j] = ArrayProperty.GenerateTrashValue(goInstance);
            }

            return array;
        }
    }
}
