using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.SymbolTable
{
    class Entorno
    {
        Dictionary<string, Simbolo> variables;
        Entorno? padre;
        public string nombre { get; set; }
        public int size { get; set; }
        public string? break1 { get; set; }
        public string? continue1 { get; set; }
        public string? return1 { get; set; }
        public string prop { get; set; }

        public Entorno(Entorno padre,string nombre)
        {
            this.padre = padre;
            this.nombre = nombre;
            this.variables = new Dictionary<string, Simbolo>();
            this.size = padre !=null ? padre.size : 0;
            this.break1 = padre != null ? padre.break1 : null;
            this.return1 = padre != null ? padre.return1 : null;
            this.continue1 = padre != null ? padre.continue1 : null;
            this.prop = "main";

        }

        public Simbolo declararvariable(string id,Utils.Type type, bool isconst,bool isRef)
        {
            id = id.ToLower();
            if (this.variables.ContainsKey(id)) return null;
            Simbolo newVar = new Simbolo(type, id, this.size++,isconst,this.padre==null,isRef);
            this.variables.Add(id, newVar);
            return newVar;
        }

        public Simbolo getvariable(string id)
        {
            id = id.ToLower();
            Entorno actual = this;
            while(actual != null)
            {
                if (actual.variables.ContainsKey(id)) return actual.variables[id];
                actual = actual.padre;
            }
            return null;
        }

    }
}
