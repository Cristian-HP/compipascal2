using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class If : Instruccion
    {
        public int Linea { get; set; }
        public int Columna { get; set; }
        private Expresion condicion;
        private LinkedList<Instruccion> instrucciones;
        private Instruccion _else;

        public If(Expresion condicion, LinkedList<Instruccion> instrucciones, Instruccion @else, int linea, int columna)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
            _else = @else;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            try
            {
                generator.addComment("Inicia IF");
                Retorno condicion = this.condicion.resolver(ent);
                if (condicion.type.type == Types.BOOLEAN)
                {
                    generator.addLabel(condicion.Labeltrue);
                    foreach (Instruccion ints in instrucciones)
                    {
                        ints.generar(ent);
                    }
                    if (this._else != null)
                    {
                        string temlabel = generator.newLabel();
                        generator.addGoto(temlabel);
                        generator.addLabel(condicion.Labelfalse);
                        this._else.generar(ent);
                        generator.addLabel(temlabel);
                    }
                    else
                    {
                        generator.addLabel(condicion.Labelfalse);
                    }
                    return null;
                }
                throw new Errorp(Linea, Columna, "Semantico", "La Condicion no es de tipo booleana sino de tipo" + condicion.type.type, ent.nombre);
            }
            catch(Exception ex)
            {
                Form1.salida.AppendText(ex.ToString());
                return null;
            }
            
        }
    }
}
