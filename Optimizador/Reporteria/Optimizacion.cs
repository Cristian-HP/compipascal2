using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.Reporteria
{
    class Optimizacion
    {
        public string tipo { get; set; }
        public string regla { get; set; }
        public string antes { get; set; }
        public string despues { get; set; }
        public string linea { get; set; }

        public Optimizacion()
        {
            this.tipo = "";
            this.regla = "";
            this.antes = "";
            this.despues = "";
            this.linea = "";
        }


    }
}
