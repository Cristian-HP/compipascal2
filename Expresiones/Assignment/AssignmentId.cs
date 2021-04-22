using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Assignment
{
    class AssignmentId : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }

        public string id { get; set; }
        private Expresion anterior;

        public AssignmentId(string id, Expresion anterior, int linea, int columna)
        {
            this.id = id;
            this.anterior = anterior;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            if(this.anterior == null)
            {
                Simbolo symbol = ent.getvariable(this.id);
                if(symbol == null)
                {
                    throw new Errorp(Linea,Columna,"Semantico","No existe la variable -> "+this.id,ent.nombre);
                }
                if (symbol.isGlobal)
                {
                    return new Retorno(symbol.position+"",false,symbol.type,symbol);
                }
                else
                {
                    string temp = generator.newTemporal();
                    generator.addExpresion(temp,"SP",symbol.position,"+");
                    return new Retorno(temp,true,symbol.type,symbol);
                }
            }
            else
            {
                //para los struct
                return null;
            }
        }
    }
}
