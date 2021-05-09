using compipascal2.Optimizador.AbstracOP;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.ArbolOP
{
    class SimboloOP : ExpresionOP
    {
        public enum TIPO_DATO
        {
            ENTERO = 1,
            REAL = 2,
            STRING = 3,
            BOOLEAN = 4
        }

        public string id { get; set; }

        public SimboloOP(string id,int linea,int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

        public override string OptimizarCodigo()
        {
            string antes = this.generarA();
            return antes;
        }

        public override string generarA()
        {
            return this.id;
        }
    }
}
