using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    public enum Types
    {
        INTEGER = 0,
        BOOLEAN = 3,
        REAL = 1,
        STRING = 2,
        OBJECT = 4,
        VOID = 5,
        ERROR = 9
    }
    class Type
    {
        public Types type;
        public string idtype;

        public Type(Types type, string idtype="")
        {
            this.type = type;
            this.idtype = idtype;
        }
    }
}
