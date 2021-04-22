using compipascal2.Instrucciones.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.SymbolTable
{
    class Entorno
    {
        Dictionary<string, Simbolo> variables;
        Dictionary<string, SimboloFuncion> funciones;
        Entorno? padre;
        public string nombre { get; set; }
        public int size { get; set; }
        public Stack<string> break1 { get; set; }
        public Stack<string> continue1 { get; set; }
        public string? return1 { get; set; }
        public string prop { get; set; }
        public SimboloFuncion actualFunc { get; set; }
        public Entorno(Entorno padre,string nombre)
        {
            this.padre = padre;
            this.nombre = nombre;
            this.variables = new Dictionary<string, Simbolo>();
            this.funciones = new Dictionary<string, SimboloFuncion>();
            this.size = padre !=null ? padre.size : 0;
            this.break1 = padre != null ? padre.break1 : new Stack<string>();
            this.return1 = padre != null ? padre.return1 : null;
            this.continue1 = padre != null ? padre.continue1 : new Stack<string>();
            this.prop = "main";
            this.actualFunc = padre == null ? null : padre.actualFunc;

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


        public void setEntornoFuncion(string prop,SimboloFuncion actualfun,string ret)
        {
            this.size = 1;
            this.prop = prop;
            this.return1 = ret;
            this.actualFunc = actualfun;
        }

        public bool addFunc(Funcion func,string uniqueId)
        {
            if (this.funciones.ContainsKey(func.id.ToLower())) return false;
            this.funciones.Add(func.id.ToLower(), new SimboloFuncion(func, uniqueId));
            return true;
        }
        public SimboloFuncion getFunc(string id)
        {
            if(this.funciones.ContainsKey(id.ToLower())) return this.funciones[id.ToLower()];
            return null;
        }

        public SimboloFuncion searchFunc(string id)
        {
            id = id.ToLower();
            Entorno actual = this;
            while(actual != null)
            {
                if (actual.funciones.ContainsKey(id)) return actual.funciones[id];
                actual = actual.padre;
            }
            return null;
        }

    }
}
