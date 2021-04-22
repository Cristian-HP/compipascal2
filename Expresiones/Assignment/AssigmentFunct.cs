using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace compipascal2.Expresiones.Assignment
{
    class AssigmentFunct : Expresion,Instruccion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get;set; }
        public int Linea { get; set; }
        public int Columna { get; set; }
        private string id;
        private LinkedList<Expresion> parametros;

        public AssigmentFunct(string id, LinkedList<Expresion> parametros, int linea, int columna)
        {
            this.id = id;
            this.parametros = parametros;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent)
        {
            try
            {
                Retorno value = this.resolver(ent);
                value.getValor();
            }catch(Exception ex)
            {
                Form1.salida.AppendText(ex.ToString());
            }
            
            return null;
        }

        public Retorno resolver(Entorno ent)
        {

            SimboloFuncion symfunc = ent.searchFunc(this.id);
            if (symfunc == null) throw new Errorp(Linea,Columna,"Semantico","No se encontro la funcion: "+this.id,ent.nombre);
            LinkedList<Retorno> valueparams = new LinkedList<Retorno>();
            Generator generator = Generator.getInstance();
            int size = generator.saveTemps(ent);
            foreach(Expresion parmet in this.parametros)
            {
                valueparams.AddLast(parmet.resolver(ent));
            }
            //comprobar parametros
            if(symfunc.parametros.Count == valueparams.Count)
            {
                for(int i =0; i< symfunc.parametros.Count;i++)
                {
                    if (!(valueparams.ElementAt(i).type.type == symfunc.parametros.ElementAt(i).type.type))
                        throw new Errorp(Linea,Columna,"Semantico","Argumento de Tipo: "+valueparams.ElementAt(i).type.type+" No Coinciden con el parametro Requerido Tipo"+symfunc.parametros.ElementAt(i).type.type,ent.nombre);
                    
                }
            }
            else
            {
                throw new Errorp(Linea,Columna,"Semantico","La Funcion no acepta "+valueparams.Count+" Argumentos",ent.nombre);
            }
            string temp = generator.newTemporal();
            generator.freeTemp(temp);
            if(valueparams.Count != 0)
            {
                generator.addExpresion(temp, "SP", ent.size + 1, "+");
                int index = -1;
                foreach(Retorno value in valueparams)
                {
                    index++;
                    generator.addSetStack(temp,value.getValor());
                    if (index != valueparams.Count - 1) generator.addExpresion(temp,temp,"1","+");
                }
            }

            generator.addNextEnv(ent.size);
            generator.addCall(symfunc.uniqueId+"()");
            generator.addGetStack(temp, "SP");
            generator.addAntEnv(ent.size);

            generator.recoverTemps(ent, size);
            generator.addTEmp(temp);

            if (symfunc.type.type != Types.BOOLEAN) return new Retorno(temp,true,symfunc.type);

            Retorno resul = new Retorno("", false, symfunc.type);
            this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
            this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
            generator.addIF(temp, "1", "==", this.labeltrue);
            generator.addGoto(this.labelfalse);
            resul.Labelfalse = this.labelfalse;
            resul.Labeltrue = this.labeltrue;
            return resul;
        }
    }
}
