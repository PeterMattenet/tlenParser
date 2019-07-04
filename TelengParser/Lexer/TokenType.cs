using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser
{
    public enum TokenType
    {
        NotDefined,
        Int,
        String,
        Float64,
        Bool,
        Id,
        Type,
        Struct,
        Array,
        StructLeftKey,
        StructRightKey,
        EOF
    }
}
