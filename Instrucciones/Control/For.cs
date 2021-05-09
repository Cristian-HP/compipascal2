using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class For : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }

        private Instruccion inicio;
        private Expresion condicion;
        private Expresion condicionupdate;
        private Instruccion update;
        private LinkedList<Instruccion> instrucciones;

        public For(Instruccion inicio,Expresion condicion,Instruccion update,Expresion condicionupdate, LinkedList<Instruccion> instrucciones, int linea, int columna)
        {
            this.inicio = inicio;
            this.condicion = condicion;
            this.update = update;
            this.condicionupdate = condicionupdate;
            this.instrucciones = instrucciones;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            Generator generator = Generator.getInstance();
            try
            {
                string labelwhile = generator.newLabel();
                this.inicio.generar(ent,errorps);
                generator.addComment("Inicia For");
                generator.addLabel(labelwhile);
                Retorno condicion = this.condicion.resolver(ent);
                if (condicion.type.type == Types.BOOLEAN)
                {
                    ent.break1.Push(condicion.Labelfalse);
                    ent.continue1.Push(labelwhile);
                    generator.addLabel(condicion.Labeltrue);
                    foreach (Instruccion inst in instrucciones)
                    {
                        inst.generar(ent,errorps);
                    }
                    this.condicionupdate.labeltrue = condicion.Labelfalse;
                    Retorno auxret = this.condicionupdate.resolver(ent);
                    generator.addLabel(auxret.Labelfalse);
                    this.update.generar(ent,errorps);
                    generator.addGoto(labelwhile);
                    generator.addLabel(condicion.Labelfalse);
                    generator.addComment("Finaliza For");
                    ent.break1.Pop();
                    ent.continue1.Pop();
                    return null;
                }
                throw new Errorp(Linea, Columna, "Semantico", "La condicion del For no es de tipo booleana sino de ->" + condicion.type.type, ent.nombre);
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
