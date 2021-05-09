using compipascal2.Optimizador.Reporteria;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.Analisis
{
    class AnalizadorOP
    {
        ParseTreeNode raiz;
        public (string Codigo,ReporteOptimizacion Reporte) analizar(string cadena)
        {
            GramaticaOP gramatica = new GramaticaOP();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            raiz = arbol.Root;
            if (raiz != null)
            {
                if (arbol.ParserMessages.Count > 0)
                {
                    for (int i = 0; i < arbol.ParserMessages.Count; i++)
                    {   
                        Form1.salida.AppendText(arbol.ParserMessages[i].Message + "\n");
                    }
                }
                //Form1.salida.AppendText("Reconocio Cadena \n");
                OPTimizador opti = new OPTimizador();
                string codiopti = opti.optimizar(cadena,arbol,false);
                
                return (codiopti,opti.report);
                //Form1.salida.AppendText(codiopti);
            }
            else
            {
                if (arbol.ParserMessages.Count > 0)
                {
                    for (int i = 0; i < arbol.ParserMessages.Count; i++)
                    {
                        Form1.salida.AppendText(arbol.ParserMessages[i].Message+ "  Linea-> "+arbol.ParserMessages[i].Location.Line+ " Colum->"+arbol.ParserMessages[i].Location.Column + "\n");
                    }
                }
                Form1.salida.AppendText("Fallo");
            }
            return ("Fallo2",null);
        }

    }
}
