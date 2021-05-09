using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.InstruccionesOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace compipascal2.Optimizador.ArbolOP
{
    class Etiqueta : InstruccionOP
    {
        public string id { get; set; }
        public string codigoOptimizado { get; set; }
        public bool ImprimirEtiqueta { get; set; }
        public LinkedList<InstruccionOP> intrucciones { get; set; }

        public Etiqueta(string id, LinkedList<InstruccionOP> intrucciones,int linea,int columna)
        {
            this.id = id;
            this.intrucciones = intrucciones;
            this.linea = linea;
            this.columna = columna;
            this.codigoOptimizado = "";
            this.ImprimirEtiqueta = true;
        }

        public string traducirCodigo(ASTOP ast, LinkedList<InstruccionOP> instrucciones, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            int contador = 0;
            string codigoOptimizado = "";
            InstruccionOP instAnterior = null;
            AsignacionOP asigPrevia = null;
            string codigoAnterior = "";

            foreach(InstruccionOP inst in instrucciones)
            {
                if(inst is AsignacionOP)
                {
                    ((AsignacionOP)inst).Previa = asigPrevia;
                    asigPrevia = (AsignacionOP)inst;
                }
                else if(inst is GoTo)
                {
                    ((GoTo)inst).ast = ast;

                }
                else if(inst is IFop)
                {
                    for(int i = contador + 1; i< instrucciones.Count; i++)
                    {
                        ((IFop)inst).Instrucciones.AddLast(intrucciones.ElementAt(i));
                    }
                }

                string optimizado = "";

                if(inst is IFop)
                {
                    optimizado = inst.OptimizarCodigo(ast, report, aplicaBlock);
                    if(report.Lista_optimizacion.Count > 0)
                    {

                    }
                }
                else
                {
                    if(instAnterior is IFop && inst is GoTo)
                    {
                        if (!((IFop)instAnterior).isRegla3)
                            optimizado = inst.OptimizarCodigo(ast,report,aplicaBlock);
                    }
                    else
                    {
                        optimizado = inst.OptimizarCodigo(ast, report, aplicaBlock);
                    }
                }

                if(inst is GoTo)
                {
                    if (codigoAnterior.StartsWith("goto"))
                    {
                        if(instAnterior is IFop)
                        {
                            codigoAnterior = "";
                            continue;
                        }
                        
                    }else if (ast.ExiteEtiqueta(((GoTo)inst).id))
                    {
                        if (!optimizado.Equals(""))
                        {
                            codigoOptimizado += "\t" + optimizado;
                            codigoAnterior = optimizado;
                        }
                        if (contador + 1 == intrucciones.Count) continue;

                        Optimizacion optimizacion = new Optimizacion();
                        optimizacion.linea = this.linea + "";
                        string codigoOptimizar = "<div>";

                        for(int i = contador + 1; i < instrucciones.Count; i++)
                        {
                            InstruccionOP ii = instrucciones.ElementAt(i);
                            if (ii is GoTo)
                                ((GoTo)ii).ast = ast;
                            else if (ii is IFop)
                                continue;

                            codigoOptimizar += "<p>" + ii.OptimizarCodigo(ast,report,aplicaBlock) + "</p>";
                        }

                        codigoOptimizar += "</div>";
                        optimizacion.antes = codigoOptimizar;
                        optimizacion.despues = ((GoTo)inst).id + ":";
                        optimizacion.regla = "Regla 1";
                        optimizacion.tipo = "Mirilla - Eliminación de Código Inalcanzable";
                        report.Lista_optimizacion.AddLast(optimizacion);
                        codigoAnterior = "";
                        break;

                    }
                    else
                    {
                        if (!optimizado.Equals(""))
                        {
                            codigoOptimizado += "\t" + optimizado;
                            codigoAnterior = optimizado;
                        }
                    }
                }
                else
                {
                    if (!optimizado.Equals(""))
                    {
                        codigoOptimizado += "\t" + optimizado;
                        codigoAnterior = optimizado;
                    }
                }


                instAnterior = inst;
                contador++;
            }

            return codigoOptimizado;
        }
        public override string OptimizarCodigo(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            this.codigoOptimizado = "";
            if (this.ImprimirEtiqueta)
                this.codigoOptimizado += this.id + ":\n";

            string resul = this.traducirCodigo(ast,this.intrucciones,report,aplicaBlock);
            this.codigoOptimizado += resul;
            return this.codigoOptimizado;
        }

        public override string generarA(ASTOP ast,ReporteOptimizacion report, bool aplicaBlock = false)
        {
            return "";
        }
    }
}
