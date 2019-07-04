using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoBool : GoProperty
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
            return rand.Next(0,1) == 1;
        }
    }
}
