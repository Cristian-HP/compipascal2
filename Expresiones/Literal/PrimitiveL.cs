using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Text;

namespace compipascal2.Expresiones.Literal
{
    class PrimitiveL : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Types type;
        private object valor;

        public PrimitiveL(Types type, object valor, int linea, int columna)
        {
            this.type = type;
            this.valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            switch (this.type)
            {
                case Types.INTEGER:
                case Types.REAL:
                    return new Retorno(this.valor.ToString(), false, new Utils.Type(this.type));
                case Types.BOOLEAN:
                    Generator generator = Generator.getInstance();
                    string value = "";
                    this.labeltrue = this.labeltrue == "" || this.labeltrue==null? generator.newLabel() : this.labeltrue;
                    this.labelfalse = this.labelfalse == "" || this.labelfalse==null ? generator.newLabel() : this.labelfalse;
                    if (bool.Parse(this.valor.ToString()))
                    {
                        value = "1";
                        generator.addGoto(this.labeltrue);
                    }
                    else
                    {
                        value = "0";
                        generator.addGoto(this.labelfalse);
                    }
                    Retorno retorno = new Retorno(value, false, new Utils.Type(this.type));
                    retorno.Labeltrue = this.labeltrue;
                    retorno.Labelfalse = this.labelfalse;
                    return retorno;
                default:
                    throw new Errorp(Linea, Columna, "Semantico", "Tipo de dato no Reconocido",ent.nombre);
            }
        }
    }
}
