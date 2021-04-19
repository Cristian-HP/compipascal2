using compipascal2.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Arbol
{
    class AST
    {
        public LinkedList<Instruccion> instrucciones;
        public AST(LinkedList<Instruccion> instrucciones)
        {
            this.instrucciones = instrucciones;
        }

    }
}
