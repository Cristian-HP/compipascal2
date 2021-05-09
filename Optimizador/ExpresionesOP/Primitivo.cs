using compipascal2.Optimizador.AbstracOP;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.ExpresionesOP
{
    class Primitivo : ExpresionOP
    {
        public object valor { get; set; }

        public Primitivo(object valor,int linea,int columna)
        {
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public override string generarA()
        {
            return this.valor.ToString();
        }

        public override string OptimizarCodigo()
        {
            string antes = this.generarA();
            return antes;
        }
    }
}
