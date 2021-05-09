using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class Case : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private LinkedList<Expresion> condiciones;
        private LinkedList<Instruccion> instrucciones;
        public Retorno initcondicion { get; set; }

        public Case(LinkedList<Expresion> condiciones, LinkedList<Instruccion> instrucciones, int linea, int columna)
        {
            this.condiciones = condiciones;
            this.instrucciones = instrucciones;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent,LinkedList<Errorp> errorps)
        {
            Generator generator = Generator.getInstance();
            foreach (Expresion exp in condiciones)
            {
                string auxlabel = generator.newLabel();
                Retorno auxret = exp.resolver(ent);
                if (auxret.type.type != initcondicion.type.type)
                {
                    throw new Errorp(Linea, Columna, "Semantico", "No es Posible tener case de tipo " + auxret.type.type + " en un switch de tipo " + initcondicion.type.type, ent.nombre);
                }
                generator.addIF(initcondicion.getValor(), auxret.getValor(), "!=", auxlabel);
                foreach (Instruccion inst in instrucciones)
                {
                    inst.generar(ent,errorps);
                }
                generator.addGoto(initcondicion.Labeltrue);
                generator.addLabel(auxlabel);
            }


            return null;
        }
    }
}
