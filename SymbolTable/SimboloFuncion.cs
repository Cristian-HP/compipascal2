using compipascal2.Instrucciones.Funciones;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.SymbolTable
{
    class SimboloFuncion
    {
        public Utils.Type type;
        public string id;
        public string uniqueId;
        public int size;
        public LinkedList<Param> parametros;

        public SimboloFuncion(Funcion func,string uniqueId)
        {
            this.type = func.tipo;
            this.id = func.id;
            this.size = func.parametros.Count;
            this.uniqueId = uniqueId;
            this.parametros = func.parametros;
        }
    }
}
