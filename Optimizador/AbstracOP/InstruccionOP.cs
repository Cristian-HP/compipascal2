using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.AbstracOP
{
    abstract class InstruccionOP
    {
        public int linea { get; set; }
        public int columna { get; set; }

        //objeto optimizacionResultado
        public abstract string OptimizarCodigo(ASTOP ast,ReporteOptimizacion report ,bool aplicaBlock=false);

        public abstract string generarA(ASTOP ast,ReporteOptimizacion report,bool aplicaBlock = false);
    }
}
