using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class Repeat : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private Expresion condicion;
        private LinkedList<Instruccion> instrucciones;

        public Repeat(Expresion condicion, LinkedList<Instruccion> instrucciones, int linea, int columna)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            Generator generator = Generator.getInstance();
            try
            {
                generator.addComment("Inicia Repeat");
                this.condicion.labeltrue = generator.newLabel();
                ent.continue1.Push(this.condicion.labeltrue);
                this.condicion.labelfalse = generator.newLabel();
                ent.break1.Push(this.condicion.labelfalse);
                generator.addLabel(this.condicion.labelfalse);
                foreach (Instruccion inst in instrucciones)
                {
                    inst.generar(ent,errorps);
                }
                Retorno condicion = this.condicion.resolver(ent);
                if (condicion.type.type == Types.BOOLEAN)
                {
                    generator.addLabel(condicion.Labeltrue);
                    generator.addComment("Finaliza Repeat");
                    ent.break1.Pop();
                    ent.continue1.Pop();
                    return null;
                }
                throw new Errorp(Linea, Columna, "Semantico", "La condicion del While no es de tipo booleana sino de ->" + condicion.type.type, ent.nombre);
            }
            catch(Exception ex)
            {
                errorps.AddLast((Errorp)ex);
                Form1.salida.AppendText(ex.ToString());
                return null;
            }
            
        }
    }
}
