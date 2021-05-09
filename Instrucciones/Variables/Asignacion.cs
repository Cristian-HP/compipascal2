using compipascal2.Abstract;
using compipascal2.Expresiones.Assignment;
using compipascal2.Generador;
using compipascal2.Instrucciones.Tranferencia;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Variables
{
    class Asignacion : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }

        private Expresion valor;
        private AssignmentId target;

        public Asignacion(AssignmentId target, Expresion valor, int linea, int columna)
        {
            this.target = target;
            this.valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            
            try
            {
                if (this.target.id.Equals(ent.nombre, StringComparison.InvariantCultureIgnoreCase))
                {
                    Return aux = new Return(Linea, Columna, this.valor);
                    aux.generar(ent,errorps);
                    return null;
                }
                Retorno target = this.target.resolver(ent);

                Retorno valor = this.valor.resolver(ent);
                Generator generator = Generator.getInstance();
                Simbolo symbol = target.symbol;
                if (!this.sameType(target.type, valor.type))
                {
                    throw new Errorp(Linea, Columna, "Semantico", "No es posible asignar el tipo " + valor.type.type + " al tipo " + target.type.type, ent.nombre);
                }

                // para los array

                if (symbol.isConst)
                {
                    throw new Errorp(Linea, Columna, "Semantico", " No es posible una asignacion lla que " +target.symbol.id + " es una Constante", ent.nombre);
                }
                if(symbol == null || symbol.isHeap)
                {
                    if (target.type.type == Types.BOOLEAN)
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
                        generator.addSetHeap(target.getValor(), valor.getValor());
                    }
                }
                else
                {
                    if (target.type.type == Types.BOOLEAN)
                    {
                        string templabel = generator.newLabel();
                        generator.addLabel(valor.Labeltrue);
                        generator.addSetStack(target.getValor(), "1");
                        generator.addGoto(templabel);
                        generator.addLabel(valor.Labelfalse);
                        generator.addSetStack(target.getValor(), "0");
                        generator.addLabel(templabel);
                    }
                    else
                    {
                        generator.addSetStack(target.getValor(), valor.getValor());
                    }
                }
            }
            catch(Exception ex)
            {
                errorps.AddLast((Errorp)ex);
                Form1.salida.AppendText(ex.ToString());
            }
            
            return null;
        }


        private bool sameType(Utils.Type type1,Utils.Type type2)
        {
            if(type1.type == type2.type)
            {
                if(type1.type == Types.OBJECT)
                {
                    return type1.idtype.Equals(type2.idtype, StringComparison.InvariantCultureIgnoreCase) && type1.dimension == type2.dimension;

                }
                return type1.dimension == type2.dimension;
            }else if(type1.type == Types.OBJECT || type2.type == Types.OBJECT)
            {
                return type1.dimension > 0;
            }
            return false;
        }


    }
}
