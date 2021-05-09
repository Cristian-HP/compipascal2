using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.ExpresionesOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace compipascal2.Optimizador.InstruccionesOP
{
    class IFop : InstruccionOP
    {
        public OperacionOP condicion { get; set; }
        public string etique { get; set; }
        public bool isRegla3 { get; set; }
        public LinkedList<InstruccionOP> Instrucciones { get; set; } = new LinkedList<InstruccionOP>();

        public IFop(OperacionOP condicion, string etique,int linea,int columna)
        {
            this.condicion = condicion;
            this.etique = etique;
            this.linea = linea;
            this.columna = columna;
            this.isRegla3 = false;
        }


        public override string OptimizarCodigo(ASTOP ast,ReporteOptimizacion report,bool aplicaBlock = false)
        {
            string antes = this.generarA(ast,report,aplicaBlock);
            return antes;
        }

        public override string generarA(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string codigoA = "if( " + this.condicion.generarA() + " ) goto " + this.etique + " ;\n";
            Optimizacion optimiza = new Optimizacion();
            optimiza.linea = this.linea+"";
            optimiza.antes = codigoA;
            optimiza.tipo = "Mirrilla - Eliminacion de codigo Inalcanzable";

            if(this.condicion.tipo == OperacionOP.TIPO_OPERACION.IGUAL_IGUAL)
            {
                if (this.condicion.validarRegla3())
                {
                    optimiza.regla = "Regla 3";
                    optimiza.despues = "goto " + this.etique + ";";
                    report.Lista_optimizacion.AddLast(optimiza);
                    codigoA = "goto " + this.etique + ";\n";
                }
                else if (this.condicion.validarRegla4())
                {
                    optimiza.regla = "Regla 4";
                    optimiza.despues = "";
                    report.Lista_optimizacion.AddLast(optimiza);
                    return "";
                }
            }

            //validacion de regla 2
            try
            {
                if (codigoA.StartsWith("if"))
                {
                    if(this.Instrucciones.Count > 0)
                    {
                        if(this.Instrucciones.ElementAt(0) is GoTo)
                        {
                            string condinueva = this.condicion.InvertirCondicion();
                            if (!condinueva.Equals(this.condicion.generarA()))
                            {
                                GoTo etiquetfalse = (GoTo)this.Instrucciones.ElementAt(0);
                                Etiqueta etiquetatrue = ast.getEtiqueta(this.etique);
                                string codigoOpti = "<div>";
                                codigoOpti += "<p>" + codigoA + "</p>";
                                codigoOpti += "<p>goto " + etiquetfalse.id + "</p>";
                                codigoOpti += "<P>" + etiquetatrue.id + ":</p>";
                                codigoOpti += "<p>[instrucciones]</p>";
                                codigoOpti += "<p>" + etiquetfalse.id + ":</p>";
                                codigoOpti += "</div>";

                                codigoA = "if( " + condinueva + " ) goto " + etiquetfalse.id + ";\n";
                                string codigoOptimiza = "<div>";
                                codigoOptimiza += "<p>" + codigoA + "</p>";
                                codigoOptimiza += "<p>[instrucciones]</p>";
                                codigoOptimiza += "<p>" + etiquetfalse.id + ":</p>";
                                codigoOptimiza += "</div>";

                                optimiza.antes = codigoOpti;
                                optimiza.despues = codigoOptimiza;
                                optimiza.regla = "Regla 2";
                                optimiza.tipo = "Mirrilla - Eliminacion de Codigo Inalcanzable";
                                report.Lista_optimizacion.AddLast(optimiza);
                                this.isRegla3 = true;
                                etiquetatrue.ImprimirEtiqueta = false;
                                codigoA += etiquetatrue.OptimizarCodigo(ast, report, aplicaBlock);

                                ast.Betadas.AddLast(etiquetatrue.id);
                            }
                        }
                    }
                }
            }catch(Exception)
            {

            }
            return codigoA;
        }
    }
}
