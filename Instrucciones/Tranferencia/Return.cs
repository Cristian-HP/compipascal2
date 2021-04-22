using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Tranferencia
{
    class Return : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private Expresion valor;

        public Return(int linea, int columna, Expresion valor)
        {
            Linea = linea;
            Columna = columna;
            this.valor = valor;
        }

        public object generar(Entorno ent)
        {
            Retorno value = this.valor != null ? this.valor.resolver(ent) : new Retorno("0",false,new Utils.Type(Utils.Types.VOID));
            SimboloFuncion symfunc = ent.actualFunc;
            Generator generator = Generator.getInstance();
            if (symfunc == null)
                throw new Errorp(Linea,Columna,"Semantico","Exit fuera de una funcion",ent.nombre);
            if (!this.sameType(symfunc.type, value.type))
                throw new Errorp(Linea,Columna,"Semantico","Se esperaba el Tipo"+symfunc.type.type+" y se Obtuvo"+value.type.type,ent.nombre);

            if (symfunc.type.type == Types.BOOLEAN)
            {
                string templabel = generator.newLabel();
                generator.addLabel(value.Labeltrue);
                generator.addSetStack("SP", "1");
                generator.addGoto(templabel);
                generator.addLabel(value.Labelfalse);
                generator.addSetStack("SP", "0");
                generator.addLabel(templabel);
            }
            else if (symfunc.type.type != Types.VOID)
                generator.addSetStack("SP",value.getValor());

            generator.addGoto(ent.return1);
            return null;

        }

        private bool sameType(Utils.Type type1, Utils.Type type2)
        {
            if (type1.type == type2.type)
            {
                if (type1.type == Types.OBJECT)
                {
                    return type1.idtype.Equals(type2.idtype, StringComparison.InvariantCultureIgnoreCase) && type1.dimension == type2.dimension;

                }
                return type1.dimension == type2.dimension;
            }
            else if (type1.type == Types.OBJECT || type2.type == Types.OBJECT)
            {
                return type1.dimension > 0;
            }
            return false;
        }
    }
}
