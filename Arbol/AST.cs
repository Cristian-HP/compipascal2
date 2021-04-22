using compipascal2.Abstract;
using compipascal2.Instrucciones.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Arbol
{
    class AST
    {
        public LinkedList<Instruccion> instrucciones;
        public LinkedList<Funcion> funciones;
        public AST(LinkedList<Instruccion> instrucciones,LinkedList<Funcion> funciones)
        {
            this.instrucciones = instrucciones;
            this.funciones = funciones;
        }

    }
}
