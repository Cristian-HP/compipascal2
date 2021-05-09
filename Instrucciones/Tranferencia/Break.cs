using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Tranferencia
{
    class Break : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }

        public Break(int linea, int columna)
        {
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            if(ent.break1.Count == 0)
            {
                throw new Errorp(Linea,Columna,"Semantico", "Break no esta dentro de un ciclo", ent.nombre);
            }
            Generator.getInstance().addComment(" Break aqui ");
            Generator.getInstance().addGoto(ent.break1.Peek());
            Generator.getInstance().addComment(" Break termina ");
            return null;
        }
    }
}
