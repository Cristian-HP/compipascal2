using compipascal2.SymbolTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Utils
{
    class Retorno
    {
        private string valor;
        public bool isTemp { get; set; }
        public Type type { get; set; }
        public string Labeltrue { get; set; }
        public string Labelfalse { get; set; }
        public Simbolo symbol { get; set; }

        public Retorno(string valor, bool isTemp, Type type, Simbolo symbol=null)
        {
            this.valor = valor;
            this.isTemp = isTemp;
            this.type = type;
            Labeltrue = "";
            Labelfalse = "";
            this.symbol = symbol;
        }

        public string getValor()
        {
            return this.valor;
        }
    }
}
