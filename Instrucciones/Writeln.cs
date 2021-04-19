using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones
{
    class Writeln : Instruccion
    {
        public int Linea { get; set; }
        public int Columna { get; set; }
        public LinkedList<Expresion> expresion { get; set; }

        private readonly bool jump;

        public Writeln(LinkedList<Expresion> expresion, bool jump, int linea, int columna)
        {
            this.expresion = expresion;
            this.jump = jump;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            foreach (Expresion expre in expresion)
            {
                try
                {
                    Retorno valor = expre.resolver(ent);
                    switch (valor.type.type)
                    {
                        case Types.INTEGER:
                            generator.addPrint("i", "(int)" + valor.getValor());
                            break;
                        case Types.REAL:
                            generator.addPrint("f", valor.getValor());
                            break;
                        case Types.STRING:
                            generator.addExpresion("T0", valor.getValor(),"");
                            generator.addCall("native_print_str()");
                            break;
                        case Types.BOOLEAN:
                            string templabel = generator.newLabel();
                            generator.addLabel(valor.Labeltrue);
                            generator.addPrintTrue();
                            generator.addGoto(templabel);
                            generator.addLabel(valor.Labelfalse);
                            generator.addPrintFalse();
                            generator.addLabel(templabel);
                            break;
                        default:
                            throw new Errorp(Linea, Columna, "Sementico", "No se puede Imprimir el tipo de dato " + valor.type.type, ent.nombre);
                    }
                }catch(Exception ex)
                {
                    Form1.salida.AppendText(ex.ToString());
                    Console.WriteLine(ex.ToString());
                    return null;
                }
                
            }
            if (jump)
            {
                generator.addPrint("c", 10);
            }
            return null;
        }
    }
}
