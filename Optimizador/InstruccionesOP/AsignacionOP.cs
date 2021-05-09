using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ArbolOP;
using compipascal2.Optimizador.ExpresionesOP;
using compipascal2.Optimizador.Reporteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.InstruccionesOP
{
    class AsignacionOP : InstruccionOP
    {
        public string id { get; set; }
        public OperacionOP valor { get; set; }
        public InstruccionOP Previa { get; set; }

        public AsignacionOP(string id, OperacionOP valor,int linea,int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
            this.Previa = null;
        }

        public override string generarA(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string codigoA = this.id + " = " + this.valor.generarA() + ";\n";
            Optimizacion optima = new Optimizacion();
            optima.linea = this.linea + "";
            optima.antes = codigoA;
            optima.tipo = "Mirrilla - Simplificacion algebraica y por fuerza";

            if(this.valor.tipo == OperacionOP.TIPO_OPERACION.SUMA)
            {
                if (this.valor.validarRegla6(this.id))
                {
                    optima.regla = "Regla 6";
                    optima.despues = "";
                    report.Lista_optimizacion.AddLast(optima);
                    return "";
                }else if (!this.valor.validarRegla10().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRegla10() + ";\n";
                    optima.regla = "Regla 10";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
            }
            else if (this.valor.tipo == OperacionOP.TIPO_OPERACION.RESTA)
            {
                if (this.valor.validarRegla7(this.id))
                {
                    optima.regla = "Regla 7";
                    optima.despues = "";
                    report.Lista_optimizacion.AddLast(optima);
                    return "";
                }
                else if (!this.valor.validarRegla11().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRegla11() + ";\n";
                    optima.regla = "Regla 11";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
            }
            else if(this.valor.tipo == OperacionOP.TIPO_OPERACION.MULTI)
            {
                if (this.valor.validarRegla8(this.id))
                {
                    optima.regla = "Regla 8";
                    optima.despues = "";
                    report.Lista_optimizacion.AddLast(optima);
                    return "";
                }
                else if (!this.valor.validarRegla12().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRegla12() + ";\n";
                    optima.regla = "Regla 12";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
                else if (!this.valor.validarRegla14().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRegla14() + ";\n";
                    optima.regla = "Regla 14";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
                else if (!this.valor.ValidarRegla15().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.ValidarRegla15() + ";\n";
                    optima.regla = "Regla 15";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
            }
            else if (this.valor.tipo == OperacionOP.TIPO_OPERACION.DIV)
            {
                if (this.valor.validarRegla9(this.id))
                {
                    optima.regla = "Regla 9";
                    optima.despues = "";
                    report.Lista_optimizacion.AddLast(optima);
                    return "";
                }
                else if (!this.valor.validarRegla13().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRegla13() + ";\n";
                    optima.regla = "Regla 13";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }else if (!this.valor.validarRelga16().Equals(""))
                {
                    codigoA = this.id + " = " + this.valor.validarRelga16() + ";\n";
                    optima.regla = "Regla 16";
                    optima.despues = codigoA;
                    report.Lista_optimizacion.AddLast(optima);
                }
            }
            else if (this.valor.tipo == OperacionOP.TIPO_OPERACION.ID)
            {
                codigoA = this.id + " = " + this.valor.generarA() + ";\n";
                if(this.Previa != null)
                {
                    try
                    {
                        if(((AsignacionOP)this.Previa).valor.tipo == OperacionOP.TIPO_OPERACION.ID)
                        {
                            if(this.valor.validarRegla5(this.id,this.valor.valor.ToString(), ((AsignacionOP)this.Previa).id, ((AsignacionOP)this.Previa).valor.valor.ToString()))
                            {
                                optima.tipo = "Mirrilla - Eliminacion de Instrucciones Redundantes y de Almacenamiento";
                                optima.regla = "Regla 5";
                                optima.despues = "";
                                report.Lista_optimizacion.AddLast(optima);
                                return "";
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                codigoA = this.id + " = " + this.valor.generarA() + ";\n" ;
            }

            return codigoA;
        }

        public override string OptimizarCodigo(ASTOP ast, ReporteOptimizacion report, bool aplicaBlock = false)
        {
            string antes = generarA(ast,report,aplicaBlock);
            return antes;
        }
    }
}
