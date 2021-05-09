using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ArbolOP;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.ExpresionesOP
{
    class OperacionOP : ExpresionOP
    {
        public enum TIPO_OPERACION
        {
            SUMA = 1,
            RESTA = 2,
            MULTI = 3,
            DIV = 4,
            MENOS_UNARIO = 5,
            MAYOR_QUE = 6,
            MENOR_QUE = 7,
            MAYOR_IGUAL = 8,
            MENOR_IGUAL = 9,
            IGUAL_IGUAL = 10,
            DIFERENTE = 11,
            PRIMITIVO = 12,
            ID=13,
            ACCESO = 14,
            MODULO = 15
        }

        public TIPO_OPERACION tipo;
        public OperacionOP izq;
        public OperacionOP der;
        public object valor;

        public OperacionOP(TIPO_OPERACION tipo, OperacionOP izq, OperacionOP der,int linea,int columna)
        {
            this.tipo = tipo;
            this.izq = izq;
            this.der = der;
            this.linea = linea;
            this.columna = columna;
        }

        public OperacionOP()
        {
        }

        public override string OptimizarCodigo()
        {
            string antes = this.generarA();
            return antes;
        }

        public void Primitivo(object valor)
        {
            this.tipo = TIPO_OPERACION.PRIMITIVO;
            this.valor = valor;
        }

        public void Identificador(object valor,int linea,int columna)
        {
            this.tipo = TIPO_OPERACION.ID;
            this.valor = valor;
        }

        public void Operacion(OperacionOP izq,OperacionOP der,TIPO_OPERACION operacion ,int linea,int columna)
        {
            this.tipo = operacion;
            this.izq = izq;
            this.der = der;
            this.linea = linea;
            this.columna = columna;
        }

        public void OperacionUnaria(OperacionOP expre ,int linea,int columna)
        {
            this.tipo = TIPO_OPERACION.MENOS_UNARIO;
            this.izq = expre;
            this.linea = linea;
            this.columna = columna;
        }

        public override string generarA()
        {
            if(this.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                Primitivo value = (Primitivo)this.valor;
                return value.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.ID)
            {
                SimboloOP simbolop = new SimboloOP(this.valor.ToString(),this.linea,this.columna);
                return simbolop.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.SUMA)
            {
                return this.izq.generarA() + " + " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.RESTA)
            {
                return this.izq.generarA() + " - " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MULTI)
            {
                return this.izq.generarA() + " * " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.DIV)
            {
                return this.izq.generarA() + " / " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MODULO)
            {
                return this.izq.generarA() + " % " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MENOS_UNARIO)
            {
                return "-" + this.izq.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MAYOR_QUE)
            {
                return this.izq.generarA() + " > " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MAYOR_IGUAL)
            {
                return this.izq.generarA() + " >= " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MENOR_QUE)
            {
                return this.izq.generarA() + " < " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.MENOR_IGUAL)
            {
                return this.izq.generarA() + " <= " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.IGUAL_IGUAL)
            {
                return this.izq.generarA() + " == " + this.der.generarA();
            }
            else if(this.tipo == TIPO_OPERACION.DIFERENTE)
            {
                return this.izq.generarA() + " != " + this.der.generarA();
            }

            return "";
        }
        
        public string InvertirCondicion()
        {
            if(this.tipo == TIPO_OPERACION.IGUAL_IGUAL)
            {
                return this.izq.generarA() + "!=" + this.der.generarA();
            }else if(this.tipo == TIPO_OPERACION.DIFERENTE)
            {
                return this.izq.generarA() + " == " + this.der.generarA();
            }
            else
            {
                return this.generarA();
            }
        }

        //regla 4 del aux
        public bool validarRegla3()
        {
            if(this.izq.tipo == TIPO_OPERACION.PRIMITIVO && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.izq.generarA();
                string value2 = this.der.generarA();
                if (value.Equals(value2)) return true;
            }else if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.ID)
            {
                string value = this.izq.generarA();
                string value2 = this.der.generarA();
                if (value.Equals(value2)) return true;
            }
            return false;
        }

        //regla 5 del aux
        public bool validarRegla4()
        {
            if(this.izq.tipo == TIPO_OPERACION.PRIMITIVO && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.izq.generarA();
                string value2 = this.der.generarA();
                if (!value.Equals(value2)) return true;
            }
            return false;
        }

        //regla 1 del aux
        public bool validarRegla5(string varactual,string varAsigna,string varPrevia,string varAsignaprevia)
        {
            if (varAsignaprevia.Equals(varactual) && varPrevia.Equals(varAsigna))
                return true;
            return false;
        }
        //regla 8 del aux
        public bool validarRegla6(string id)
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.izq.valor.ToString().Equals(id))
                {
                    string value = this.der.generarA();
                    if (value.Equals("0")) return true;
                }
            }else if(this.der.tipo == TIPO_OPERACION.ID && this.izq.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.der.valor.ToString().Equals(id))
                {
                    string value = this.izq.generarA();
                    if (value.Equals("0")) return true;
                }
            }
            return false;
        }

        //regla 9 del aux
        public bool validarRegla7(string id)
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.izq.valor.ToString().Equals(id))
                {
                    string value = this.der.generarA();
                    if (value.Equals("0")) return true;
                }
            }
            return false;
        }

        //regla 10 del auc
        public bool validarRegla8(string id)
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.izq.valor.ToString().Equals(id))
                {
                    string value = this.der.generarA();
                    if (value.Equals("1")) return true;
                }
            }
            else if(this.der.tipo == TIPO_OPERACION.ID && this.izq.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.der.valor.ToString().Equals(id))
                {
                    string value = this.izq.generarA();
                    if (value.Equals("1")) return true;
                }
            }
            return false;
        }
        //regla 11 del aux
        public bool validarRegla9(string id)
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                if (this.izq.valor.ToString().Equals(id))
                {
                    string value = this.der.generarA();
                    if (value.Equals("1")) return true;
                }
            }
            return false;
        }
        //regla 12 del aux
        public string validarRegla10()
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("0")) return this.izq.valor.ToString();
            }else if(this.der.tipo == TIPO_OPERACION.ID && this.izq.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.izq.generarA();
                if (value.Equals("0")) return this.der.valor.ToString();
            }
            return "";
        }

        //regla 13 del aux
        public string validarRegla11()
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("0")) return this.izq.valor.ToString();
            }
            return "";
        }

        //regla 14 del aux
        public string validarRegla12()
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("1")) return this.izq.valor.ToString() ;
            }
            else if(this.der.tipo == TIPO_OPERACION.ID && this.izq.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.izq.generarA();
                if (value.Equals("1")) return this.der.valor.ToString();
            }
            return "";
        }

        //regla 15 del aux
        public string validarRegla13()
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("1")) return this.izq.valor.ToString();
            }
            return "";
        }
        //regla 16 del aux
        public string validarRegla14()
        {
            if(this.izq.tipo == TIPO_OPERACION.ID && this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("2")) return this.izq.valor + " + " + this.izq.valor;
            }
            return "";
        }
        //regla 17 del aux
        public string ValidarRegla15()
        {
            if(this.der.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.der.generarA();
                if (value.Equals("0")) return "0";
            }
            else if(this.izq.tipo == TIPO_OPERACION.PRIMITIVO)
            {
                string value = this.izq.generarA();
                if (value.Equals("0")) return "0";
            }
            return "";
        }

        public string validarRelga16()
        {
            if(this.izq.tipo == TIPO_OPERACION.PRIMITIVO && this.der.tipo == TIPO_OPERACION.ID)
            {
                string value = this.izq.generarA();
                if (value.Equals("0")) return "0";
            }
            return "";
        }
    }
}
