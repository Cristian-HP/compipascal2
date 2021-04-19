using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Relacional
{
    class Menorq : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Expresion izq;
        private Expresion der;
        private bool equal;

        public Menorq(Expresion izq, Expresion der, bool equal, int linea, int columna)
        {
            this.izq = izq;
            this.der = der;
            this.equal = equal;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Retorno izqR = this.izq.resolver(ent);
            Retorno derR = this.der.resolver(ent);
            var izqtipe = izqR.type.type;
            var dertipe = derR.type.type;
            Generator generator = Generator.getInstance();
            if ((izqtipe == Types.REAL || izqtipe == Types.INTEGER) && (dertipe == Types.REAL || dertipe == Types.INTEGER))
            {
                this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                if (equal)
                {
                    generator.addIF(izqR.getValor(), derR.getValor(), "<=", this.labeltrue);
                }
                else
                {
                    generator.addIF(izqR.getValor(), derR.getValor(), "<", this.labeltrue);
                }
                generator.addGoto(this.labelfalse);
                Retorno resul = new Retorno("", false, new Utils.Type(Types.BOOLEAN));
                resul.Labeltrue = this.labeltrue;
                resul.Labelfalse = this.labelfalse;
                return resul;
            }else if(izqtipe == Types.STRING && dertipe == Types.STRING)
            {
                string namefunc = "native_less_str()";
                this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                string temp = generator.newTemporal();
                generator.addExpresion("T0", izqR.getValor(), "");
                generator.addExpresion("T1", derR.getValor(), "");
                if (equal)
                {
                    namefunc = "native_less_equal_str()";
                }
                generator.addCall(namefunc);
                generator.addExpresion(temp, "T4","");
                generator.addIF(temp,"1","==",this.labeltrue);
                generator.addGoto(this.labelfalse);
                Retorno resul = new Retorno(temp, false, new Utils.Type(Types.BOOLEAN));
                resul.Labeltrue = this.labeltrue;
                resul.Labelfalse = this.labelfalse;
                return resul;

            }
            throw new Errorp(Linea, Columna, "Semantico", "No se puede realizar " + izqtipe.ToString() + " < " + dertipe.ToString(), ent.nombre);
        }
    }
}
