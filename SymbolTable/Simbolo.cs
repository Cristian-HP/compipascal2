using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.SymbolTable
{
    class Simbolo
    {
        public Utils.Type type { get; set; }
        public string id { get; set; }
        public int position { get; set; }
        public bool isConst { get; set; }
        public bool isGlobal { get; set; }
        public bool isHeap { get; set; }

        public Simbolo(Utils.Type type, string id, int position, bool isConst, bool isGlobal,bool isHeap=false)
        {
            this.type = type;
            this.id = id;
            this.position = position;
            this.isConst = isConst;
            this.isGlobal = isGlobal;
            this.isHeap = isHeap;
        }
    }
}
