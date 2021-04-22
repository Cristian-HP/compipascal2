using compipascal2.Abstract;
using compipascal2.Expresiones.Literal;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Variables
{
    class Declaracion : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private Utils.Type type;
        private LinkedList<string> variables;
        private Expresion valor;
        private bool isConst;

        public Declaracion(Utils.Type type, LinkedList<string> variables, Expresion valor, int linea, int columna, bool isConst = false)
        {
            this.type = type;
            this.variables = variables;
            this.valor = valor;
            this.isConst = isConst;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            try
            {
                if (this.variables.Count > 1 && this.valor != null)
                {
                    throw new Errorp(Linea, Columna, "Semantico", "No es posible declarar una lista de variables con valor explicito", ent.nombre);
                }
                else
                {
                    Retorno valor1;
                    if (this.variables.Count == 1 && this.valor != null)
                    {
                        valor1 = this.valor.resolver(ent);
                    }
                    else
                    {
                        valor1 = valor_defecto(this.type);
                    }

                    if (this.type == null || valor1.type.type == this.type.type || valor1.type.type == Types.INTEGER)
                    {
                        foreach (string temp1 in this.variables)
                        {
                            if(this.type != null &&valor1.type.type != this.type.type)
                            {
                                valor1.type = this.type;
                            }
                            Simbolo newVar = ent.declararvariable(temp1, valor1.type, isConst, false);
                            if (newVar == null) throw new Errorp(Linea, Columna, "Semantico", "La Variable: " + temp1 + " ya existe en este ambito", ent.nombre);
                            if (newVar.isGlobal)
                            {
                                if (valor1.type.type == Types.BOOLEAN)
                                {
                                    string templabel = generator.newLabel();
                                    generator.addLabel(valor1.Labeltrue);
                                    generator.addSetStack(newVar.position, "1");
                                    generator.addGoto(templabel);
                                    generator.addLabel(valor1.Labelfalse);
                                    generator.addSetStack(newVar.position, "0");
                                    generator.addLabel(templabel);
                                }
                                else
                                {
                                    generator.addSetStack(newVar.position, valor1.getValor());
                                }
                            }
                            else
                            {
                                string temp = generator.newTemporal();
                                generator.freeTemp(temp);
                                generator.addExpresion(temp, "SP", newVar.position, "+");
                                if (valor1.type.type == Types.BOOLEAN)
                                {
                                    string templabel = generator.newLabel();
                                    generator.addLabel(valor1.Labeltrue);
                                    generator.addSetStack(temp, "1");
                                    generator.addGoto(templabel);
                                    generator.addLabel(valor1.Labelfalse);
                                    generator.addSetStack(temp, "0");
                                    generator.addLabel(templabel);
                                }
                                else
                                {
                                    generator.addSetStack(temp, valor1.getValor());
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Errorp(Linea, Columna, "Semantico", "Tipos de datos diferentes " + this.type.type + " " + valor1.type.type, ent.nombre);
                    }
                }
            }
            catch(Exception ex)
            {
                Form1.salida.AppendText(ex.ToString());
            }
            
            return null;

        }

        private Retorno valor_defecto(Utils.Type tipo)
        {
            Generator generator = Generator.getInstance();
            switch (tipo.type)
            {
                case Types.INTEGER:
                case Types.REAL:
                    return new Retorno("0",false,tipo,null);
                case Types.STRING:
                    
                    string temp = generator.newTemporal();
                    generator.addExpresion(temp, "HP", "");
                    generator.addSetHeap("HP",0);
                    generator.nextHeap();
                    generator.addSetHeap("HP", "-1");
                    generator.nextHeap();
                    return new Retorno(temp, true, tipo);
                case Types.BOOLEAN:
                    string value = "";
                    string labeltrue = generator.newLabel();
                    string labelfalse =  generator.newLabel();
                    value = "0";
                    generator.addGoto(labelfalse);
                    Retorno retorno = new Retorno(value, false, tipo);
                    retorno.Labeltrue = labeltrue;
                    retorno.Labelfalse = labelfalse;
                    return retorno;
                default:
                    return null;
            }
        }
    }
}
