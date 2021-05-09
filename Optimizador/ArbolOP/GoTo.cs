using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 

namespace compipascal2.Optimizador.ArbolOP
{
    class GoTo : InstruccionOP
    {
        public string id { get; set; }
        public ASTOP ast { get; set; }

        public GoTo(string id, int linea,int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

        public override string generarA(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string codigoA = "goto " + this.id + ";\n";
           
            return codigoA;

        }

        public override string OptimizarCodigo(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string antes = this.generarA(ast,report,aplicaBlock);
            return antes;
        }
    }
}
