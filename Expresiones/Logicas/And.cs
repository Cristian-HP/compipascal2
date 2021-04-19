using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Logicas
{
    class And : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get;set; }
        public int Columna { get;set; }
        private Expresion izq;
        private Expresion der;

        public And(Expresion izq, Expresion der, int linea, int columna)
        {
            this.izq = izq;
            this.der = der;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
            this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;

            this.izq.labeltrue = generator.newLabel();

            this.der.labeltrue = this.labeltrue;
            this.izq.labelfalse = this.der.labelfalse = this.labelfalse;
            generator.addComment("Inicia And");
            Retorno izq = this.izq.resolver(ent);
            generator.addLabel(this.izq.labeltrue);
            Retorno der = this.der.resolver(ent);
            generator.addComment("Finaliza And");
            if(izq.type.type == Types.BOOLEAN && der.type.type == Types.BOOLEAN)
            {
                Retorno resul = new Retorno("", false, izq.type);
                resul.Labeltrue = this.labeltrue;
                resul.Labelfalse = this.der.labelfalse;
                return resul;
            }
            throw new Errorp(Linea, Columna, "Semantico", "No se puede realizar un operacion And con los tipos -> "+izq.type.type+" && " +der.type.type, ent.nombre);
        }
    }
}
