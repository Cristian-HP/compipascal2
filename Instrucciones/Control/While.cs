using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class While : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private Expresion condicion;
        private LinkedList<Instruccion> instruciones;

        public While(Expresion condicion, LinkedList<Instruccion> instruciones, int linea, int columna)
        {
            this.condicion = condicion;
            this.instruciones = instruciones;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            string labelwhile = generator.newLabel();
            generator.addComment("Inicia While");
            generator.addLabel(labelwhile);
            Retorno condicion = this.condicion.resolver(ent);
            if(condicion.type.type == Types.BOOLEAN)
            {
                ent.break1 = condicion.Labelfalse;
                ent.continue1 = labelwhile;
                generator.addLabel(condicion.Labeltrue);
                foreach(Instruccion inst in instruciones)
                {
                    inst.generar(ent);
                }
                generator.addGoto(labelwhile);
                generator.addLabel(condicion.Labelfalse);
                generator.addComment("Finaliza While");
                return null;
            }
            throw new Errorp(Linea, Columna, "Semantico", "La condicion del While no es de tipo booleana sino de ->"+condicion.type.type, ent.nombre);
        }
    }
}
