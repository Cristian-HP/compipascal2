using compipascal2.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Arbol
{
    class Group
    {
        public LinkedList<Instruccion> instrucciones { get; set; }

        public Group(LinkedList<Instruccion> instrucciones)
        {
            this.instrucciones = instrucciones;
        }
    }
}
