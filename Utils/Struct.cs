using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    class Struct : Type
    {
        public Dictionary<string, object> atributos { get; set; }
        public int size { get; set; }

        public Struct(String id, Dictionary<string, object> atributos) : base(Types.OBJECT,id)
        {
            this.atributos = atributos;
            this.size = this.atributos.Count;
        }
    }
}
