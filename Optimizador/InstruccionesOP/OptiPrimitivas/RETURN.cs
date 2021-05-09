using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.InstruccionesOP.OptiPrimitivas
{
    class RETURN : InstruccionOP
    {
        public RETURN()
        {
        }

        public override string generarA(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string codigoA = "return ;\n";
            return codigoA;
        }

        public override string OptimizarCodigo(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string antes = this.generarA(ast,report,aplicaBlock);
            return antes;
        }
    }
}
