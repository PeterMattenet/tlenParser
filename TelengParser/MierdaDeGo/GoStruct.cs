using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.MierdaDeGo
{
    public class GoStruct : GoProperty
    {
        public Dictionary<string, GoProperty> _properties;
        
        public GoStruct()
        {
            _properties = new Dictionary<string, GoProperty>();
        }
    }
}
