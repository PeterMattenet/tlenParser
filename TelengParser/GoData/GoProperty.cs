using System;
using System.Collections.Generic;
using TelengParser.GoData;

namespace TelengParser
{
    public abstract class GoProperty
    {
        protected Random rand;

        public GoProperty()
        {
            rand = new Random();
        }

        public abstract void FailIfUndefinedInParser(GoParser parser);

        public abstract void AddToTypeNavigationQueue(Queue<GoObject> queue, GoParser parser);

        public abstract void NavigateDependenciesTo(Stack<GoObject> pathTaken, GoParser parser);

        public abstract dynamic GenerateTrashValue(GoInstance goInstance);
    }
}