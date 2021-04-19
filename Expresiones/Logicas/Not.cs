using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Logicas
{
    class Not : Expresion
    {
        public string labeltrue { get;set; }
        public string labelfalse { get;set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Expresion valor;

        public Not(Expresion valor, int linea, int columna)
        {
            this.valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
            this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;

            this.valor.labeltrue = this.labelfalse;
            this.valor.labelfalse = this.labeltrue;

            Retorno valor = this.valor.resolver(ent);
            if(valor.type.type == Types.BOOLEAN)
            {
                Retorno resul = new Retorno("", false, valor.type);
                resul.Labelfalse = this.labelfalse;
                resul.Labeltrue = this.labeltrue;
                return resul;
            }
            throw new Errorp(Linea, Columna, "Semantico", "No se puede realizar un operacion NOt del tipo -> " + valor.type.type, ent.nombre);
        }
    }
}
