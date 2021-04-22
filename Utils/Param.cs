using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    class Param
    {
        public string id { get; set; }
        public Utils.Type type { get; set; }
        

        public Param(string id, Type type)
        {
            this.id = id;
            this.type = type;
        }
        public string getUnicType()
        {
            if(this.type.type == Types.OBJECT)
            {
                return this.type.idtype;
            }
            return this.type.type.ToString();
        }
        public string toString()
        {
            return "{id: "+this.id+", type: "+this.type.ToString() + "}";
        }
    }
}
