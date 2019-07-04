using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoInt : GoProperty
    {
        public override void AddToTypeNavigationQueue(Queue<GoObject> queue, GoParser parser)
        {
        }

        public override void NavigateDependenciesTo(Stack<GoObject> pathTaken, GoParser parser)
        {
        }

        public override void FailIfUndefinedInParser(GoParser parser)
        {
        }

        public override dynamic GenerateTrashValue(GoInstance goInstance)
        {
            return rand.Next(int.MinValue, int.MaxValue);
        }
    }
}
