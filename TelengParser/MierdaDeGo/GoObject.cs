using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser
{
    public class GoObject
    {
        public Dictionary<string, GoProperty> _properties;

        public string ObjectType;

        public GoObject()
        {
            _properties = new Dictionary<string, GoProperty>();
        }
    }
}
