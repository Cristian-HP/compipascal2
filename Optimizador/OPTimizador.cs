using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.Reporteria;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador
{
    class OPTimizador
    {
        public string codigoOptimizado { get; set; }
        public string codigoAnterior { get; set; }
        public LinkedList<Etiqueta> instrucciones { get; set; }
        public ReporteOptimizacion report { get; set; }

        public OPTimizador()
        {
            this.codigoOptimizado = "";
            this.codigoAnterior = "";
            this.instrucciones = new LinkedList<Etiqueta>();
            report = new ReporteOptimizacion();
        }

        public string optimizar(string txt,ParseTree tree,bool aplicaBlock=false)
        {
            this.codigoAnterior = txt;
            this.codigoOptimizado = "";
            GeneradorAST migenerador = new GeneradorAST(tree);
            LinkedList<FuncionesOP> funciones = migenerador.mifuncion;
            string encabezado = migenerador.Encabezado;
            string totalcode = encabezado;
            foreach (FuncionesOP funcion in funciones)
            {
                LinkedList<Etiqueta> instruc1 = funcion.etiquetasf;
                this.instrucciones = instruc1;

                ASTOP ast = new ASTOP(this.instrucciones);
                

                if(instruc1 != null)
                {
                    foreach(Etiqueta func in instruc1)
                    {
                        if (ast.Betadas.Contains(func.id)) continue;
                        this.codigoOptimizado += func.OptimizarCodigo(ast,report,aplicaBlock);
                    }
                }
                totalcode += "void " + funcion.id + " (){\n";
                totalcode += codigoOptimizado;
                totalcode += "\n}\n";
                this.codigoOptimizado = "";
            }

            return totalcode;
        }


    }
}
