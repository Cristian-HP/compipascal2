using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.AbstracOP
{
    abstract class ExpresionOP
    {
        public int linea { get; set; }
        public int columna { get; set; }

        //objeto optimizacionResultado
        public abstract string OptimizarCodigo();

        public abstract string generarA();
    }
}
