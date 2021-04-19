using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Variables
{
    class Asignacion : Instruccion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get;set; }
        public int Linea { get;set; }
        public int Columna { get;set; }

        private Expresion valor;
        private Expresion target;

        public Asignacion(Expresion target, Expresion valor, int linea, int columna)
        {
            this.target = target;
            this.valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            Retorno target = this.target.resolver(ent);
            Retorno valor = this.valor.resolver(ent);
            Generator generator = Generator.getInstance();
            Simbolo symbol = target.symbol;
            if(target.type == valor.type)
            {
                throw new Errorp(Linea, Columna, "Semantico", "No es posible asignar el tipo "+valor.type.type +" al tipo "+target.type.type, ent.nombre);
            }
            // para los array

            if(symbol == null || symbol.isHeap)
            {
                if(target.type.type == Types.BOOLEAN)
                {
                    string templabel = generator.newLabel();
                    generator.addLabel(valor.Labeltrue);
                    generator.addSetHeap(target.getValor(), "1");
                    generator.addGoto(templabel);
                    generator.addLabel(valor.Labelfalse);
                    generator.addSetHeap(target.getValor(), "0");
                    generator.addLabel(templabel);
                }
                else
                {
                    generator.addSetHeap(target.getValor(),valor.getValor());
                }
            }
            else
            {
                if(target.type.type == Types.BOOLEAN)
                {
                    string templabel = generator.newLabel();
                    generator.addLabel(valor.Labeltrue);
                    generator.addSetStack(target.getValor(),"1");
                    generator.addGoto(templabel);
                    generator.addLabel(valor.Labelfalse);
                    generator.addSetStack(target.getValor(), "0");
                    generator.addLabel(templabel);
                }
                else
                {
                    generator.addSetStack(target.getValor(),valor.getValor());
                }
            }

            return null;
        }
    }
}
