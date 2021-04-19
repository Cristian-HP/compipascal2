using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Arimeticas
{
    class Mod : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Expresion izq;
        private Expresion der;

        public Mod(Expresion izq, Expresion der, int linea, int columna)
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
            if (derR.getValor() == "0")
            {
                throw new Errorp(Linea, Columna, "Semantico", "Indefinicion No es posible el modulo entre 0 ", ent.nombre);
            }
            switch (izqR.type.type)
            {
                case Types.INTEGER:
                    switch (derR.type.type)
                    {
                        case Types.INTEGER:
                            generator.addExpresion(temp, "(int)"+izqR.getValor(), "(int)"+derR.getValor(), "%");
                            return new Retorno(temp, true,izqR.type);
                        default:
                            break;
                    }
                    break;
            }
            throw new Errorp(this.Linea, this.Columna, "Semantico", "No se Puede Obtener Modulo de un Tipo " + izqR.type.type + " Con el tipo " + derR.type.type, ent.nombre);
        
        }
    }
}
