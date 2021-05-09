using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.InstruccionesOP.OptiPrimitivas
{
    class IMPRIMIR : InstruccionOP
    {
        public string cadena { get; set; }
        public string formato { get; set; }

        public IMPRIMIR(string cadena, string formato,int linea,int columna)
        {
            this.cadena = cadena;
            this.formato = formato;
            this.linea = linea;
            this.columna = columna;
        }

        public override string generarA(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string codigoA = "printf(" + this.formato + "," + this.cadena + ");\n";
            return codigoA;
        }

        public override string OptimizarCodigo(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string antes = this.generarA(ast,report,aplicaBlock);
            return antes;
        }
    }
}
