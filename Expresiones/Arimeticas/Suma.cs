using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Arimeticas
{
    class Suma : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Expresion izq;
        private Expresion der;

        public Suma(Expresion izq, Expresion der, int linea, int columna)
        {
            this.izq = izq;
            this.der = der;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Retorno izqR = this.izq.resolver(ent);
            Retorno derR = this.der.resolver(ent);
            Generator generator = Generator.getInstance();
            string temp = generator.newTemporal();
            switch (izqR.type.type)
            {
                case Types.INTEGER:
                    switch (derR.type.type)
                    {
                        case Types.INTEGER:
                        case Types.REAL:
                            generator.addExpresion(temp, izqR.getValor(), derR.getValor(), "+");
                            return new Retorno(temp,true, derR.type.type == Types.REAL ? derR.type : izqR.type);
                        default:
                            break;
                    }
                    break;
                case Types.REAL:
                    switch (derR.type.type)
                    {
                        case Types.INTEGER:
                        case Types.REAL:
                            generator.addExpresion(temp, izqR.getValor(), derR.getValor(), "+");
                            return new Retorno(temp, true, izqR.type);
                        default:
                            break;
                    }
                    break;
                case Types.STRING:
                    if (derR.type.type == Types.STRING)
                    {
                        generator.addExpresion("T1", izqR.getValor(),"");
                        generator.addExpresion("T2", derR.getValor(), "");
                        generator.addCall("native_concat_str()");
                        generator.addExpresion(temp, "T3", "");
                        return new Retorno(temp,true,izqR.type);
                    }
                    break;
                default:
                    break;
            }
            throw new Errorp(this.Linea,this.Columna,"Semantico","No se puede Sumar el tipo "+izqR.type.type+" con el Tipo "+derR.type.type,ent.nombre);
        }
    }
}
