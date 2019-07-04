using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoFloat : GoProperty
    {
        public override void FailIfUndefinedInParser(GoParser parser)
        {
        }

        public override void NavigateDependenciesTo(Stack<GoObject> pathTaken, GoParser parser)
        {
        }

        public override void AddToTypeNavigationQueue(Queue<GoObject> queue, GoParser parser)
        {
        }

        public override dynamic GenerateTrashValue(GoInstance goInstance)
        {
            int i1 = rand.Next(int.MinValue, int.MaxValue);
            int i2 = rand.Next(int.MinValue, int.MaxValue);

            return ((float) i1)/((float) i2);
        }
    }
}
