using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Abstract
{
    interface Instruccion
    {
        public int Linea { get; set; }
        public int Columna { get; set; }

        public abstract object generar(Entorno ent,LinkedList<Errorp> mierror);
    }
}
