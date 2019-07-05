using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoString : GoProperty
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
            StringBuilder builder = new StringBuilder();
            rand = new Random();
            int size = rand.Next(0, 128);

            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }

            /*Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < size; i++)
                builder.Append(chars[random.Next(chars.Length)]);
            */
            return builder.ToString().ToLower();
            
        }
    }
}
