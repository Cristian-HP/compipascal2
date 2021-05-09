using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Control
{
    class Switch : Instruccion
    {
        public int Linea { get; set; }
        public int Columna { get;set; }

        private Expresion condicion;
        private LinkedList<Case> lista_case;
        private Instruccion defaultcase;

        public Switch(Expresion condicion, LinkedList<Case> lista_case, Instruccion defaultcase, int linea, int columna)
        {
            this.condicion = condicion;
            this.lista_case = lista_case;
            this.defaultcase = defaultcase;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            Generator generator = Generator.getInstance();
            try
            {
                string templabel = generator.newLabel();
                generator.addComment(" Inicia Case ");
                Retorno condi = this.condicion.resolver(ent);
                condi.Labeltrue = templabel;
                foreach (Case opcion in lista_case)
                {
                    opcion.initcondicion = condi;
                    opcion.generar(ent,errorps);
                }
                if (defaultcase != null)
                {
                    this.defaultcase.generar(ent,errorps);
                }
                generator.addLabel(condi.Labeltrue);
                generator.addComment(" Finaliza Case ");
            }
            catch(Exception ex)
            {
                errorps.AddLast((Errorp)ex);
                Form1.salida.AppendText(ex.ToString());
            }
            
            return null;
        }
    }
}
