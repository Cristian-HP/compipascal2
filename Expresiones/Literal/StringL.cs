using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Literal
{
    class StringL : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Types type;
        private string valor;

        public StringL(Types type, string valor, int linea, int columna)
        {
            this.type = type;
            this.valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            string temp = generator.newTemporal();
            generator.addExpresion(temp, "HP","");
            for (int i = 0; i < this.valor.ToString().Length; i++)
            {
                generator.addSetHeap("HP", (int)this.valor.ToString()[i]);
                generator.nextHeap();
            }
            generator.addSetHeap("HP", "-1");
            generator.nextHeap();
            return new Retorno(temp, true, new Utils.Type(this.type, "String"));
        }
    }
}
