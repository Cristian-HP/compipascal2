using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace compipascal2.SymbolTable
{
    class SimboloStruct
    {
        public string id { get; set; }
        public int size { get; set; }
        public LinkedList<Param> atributos { get; set; }

        public SimboloStruct(string id, int size, LinkedList<Param> atributos)
        {
            this.id = id;
            this.size = size;
            this.atributos = atributos;
        }

        public (int index, Param valor ) getAttribute(string id)
        {
            for(int i = 0; i < atributos.Count; i++)
            {
                Param value = atributos.ElementAt(i);
                if(value.id.Equals(id,StringComparison.InvariantCultureIgnoreCase))
                {
                    return (i,value);
                }
            }
            return (-1, null);
        }
    }
}
