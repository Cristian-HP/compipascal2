using compipascal2.Abstract;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class Else : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get; set; }
        private LinkedList<Instruccion> instrucciones;

        public Else(LinkedList<Instruccion> instrucciones, int linea, int columna)
        {
            this.instrucciones = instrucciones;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            foreach(Instruccion inst in instrucciones)
            {
                inst.generar(ent,errorps);
            }
            return null;
        }
    }
}
