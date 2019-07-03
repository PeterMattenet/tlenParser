using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.MierdaDeGo
{
    public class GoArray : GoProperty
    {
        public int Dimensions { get; set; }

        public GoProperty ArrayProperty { get; set; }

        public GoArray()
        {
            Dimensions = 1;
        }
    }
}
