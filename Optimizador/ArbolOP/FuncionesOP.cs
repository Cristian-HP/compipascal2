using compipascal2.Optimizador.AbstracOP;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.ArbolOP
{
    class FuncionesOP
    {
        public string id { get; set; }
        public LinkedList<Etiqueta> etiquetasf { get; set; }

        public FuncionesOP(string id, LinkedList<Etiqueta> etiquetasf)
        {
            this.id = id;
            this.etiquetasf = etiquetasf;
        }
    }
}
