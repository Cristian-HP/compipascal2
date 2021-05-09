using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    public class TablaReport
    {
        public int Linea { get; set; }
        public int Columna { get; set; }
        public string Ambito { get; set; }

        public string tipo { get; set; }
        public string nombre { get; set; }
        public string Ambiente { get; set; }
        public string Numero { get; set; }
        public TablaReport(string nombre, int linea, int columna, string ambito, string tipo, string ambiente, int numero)
        {
            Linea = linea + 1;
            Columna = columna;
            Ambito = ambito;
            this.tipo = tipo;
            this.nombre = nombre;
            this.Ambiente = ambiente;
            this.Numero = numero + "";
        }
    }
}
