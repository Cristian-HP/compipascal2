using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    class Errorp : Exception
    {
        public int Linea;
        public int Columna;
        public string tipoe;
        public string descripcion;
        public string ambito;

        public Errorp(int linea, int columna, string tipoe, string descripcion,string ambito)
        {
            Linea = linea+1;
            Columna = columna;
            this.tipoe = tipoe;
            this.descripcion = descripcion;
            this.ambito = ambito;
        }
        override
        public string ToString()
        {
            return "ERROR " + tipoe + " en la Linea:" + Linea + " y columna:" + Columna + " -> " + descripcion;
        }
    }
}
