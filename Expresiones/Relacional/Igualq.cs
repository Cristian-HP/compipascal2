using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Relacional
{
    class Igualq : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }

        private Expresion izq;
        private Expresion der;

        public Igualq(Expresion izq, Expresion der, int linea, int columna)
        {
            this.izq = izq;
            this.der = der;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Retorno izqR = this.izq.resolver(ent);
            Retorno derR;
            Generator generator = Generator.getInstance();
            switch (izqR.type.type)
            {
                case Types.INTEGER:
                case Types.REAL:
                    derR = this.der.resolver(ent);
                    switch (derR.type.type)
                    {
                        case Types.INTEGER:
                        case Types.REAL:
                            this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                            this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                            generator.addIF(izqR.getValor(), derR.getValor(), "==", this.labeltrue);
                            generator.addGoto(this.labelfalse);
                            Retorno resul = new Retorno("", false, new Utils.Type(Types.BOOLEAN));
                            resul.Labelfalse = this.labelfalse;
                            resul.Labeltrue = this.labeltrue;
                            return resul;
                        default:
                            break;
                    }
                    break;
                case Types.BOOLEAN:
                    string truelabel = this.labeltrue == null || this.labeltrue == "" ? generator.newLabel() : this.labeltrue;
                    string falselabel = this.labelfalse == null || this.labelfalse == "" ? generator.newLabel() : this.labelfalse;
                    generator.addLabel(izqR.Labeltrue);
                    this.der.labeltrue = truelabel;
                    this.der.labelfalse = falselabel;
                    derR = this.der.resolver(ent);
                    generator.addLabel(izqR.Labelfalse);
                    this.der.labeltrue = falselabel;
                    this.der.labelfalse = truelabel;
                    derR = this.der.resolver(ent);
                    if (derR.type.type == Types.BOOLEAN)
                    {
                        Retorno resul = new Retorno("", false, izqR.type);
                        resul.Labeltrue = truelabel;
                        resul.Labelfalse = falselabel;
                        return resul;
                    }
                    break;
                case Types.STRING:
                    derR = this.der.resolver(ent);
                    if (derR.type.type == Types.STRING)
                    {
                        this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                        this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                        string temp = generator.newTemporal();
                        generator.addExpresion("T0", izqR.getValor(), "");
                        generator.addExpresion("T1", derR.getValor(), "");
                        generator.addCall("native_equal_str()");
                        generator.addExpresion(temp, "T4", "");
                        generator.addIF(temp, "1", "==", this.labeltrue);
                        generator.addGoto(this.labelfalse);
                        Retorno resul = new Retorno(temp, false, new Utils.Type(Types.BOOLEAN));
                        resul.Labeltrue = this.labeltrue;
                        resul.Labelfalse = this.labelfalse;
                        return resul;
                    }
                    break;
                default:
                    derR = null;
                    break;
            }
            throw new Errorp(this.Linea, this.Columna, "Semantico", "No se Puede Realizar " + izqR.type.type.ToString() + " == " +  derR == null? "Indefinido": derR.type.type.ToString(), ent.nombre);
        }
    }
}
