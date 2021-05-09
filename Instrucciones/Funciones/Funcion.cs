using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Funciones
{
    class Funcion : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        public string id { get; set; }
        public Utils.Type tipo { get; set; }
        private LinkedList<Instruccion> instrucciones;
        public LinkedList<Param> parametros { get; set; }
        private bool preCompile;

        public Funcion(string id, Utils.Type tipo, LinkedList<Instruccion> instrucciones, LinkedList<Param> parametros, int linea, int columna)
        {
            this.id = id;
            this.tipo = tipo;
            this.instrucciones = instrucciones;
            this.parametros = parametros;
            this.preCompile = true;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            try
            {
                if (this.preCompile)
                {
                    this.preCompile = false;
                    this.validateParams(ent);
                    this.validteType(ent);
                    string uniqueid = this.uniqueId(ent);
                    if (!ent.addFunc(this, uniqueid,Linea,Columna))
                        throw new Errorp(Linea, Columna, "Semantico", "Ya Existe una funcion con el id: " + this.id, ent.nombre);
                    return null;
                }
                SimboloFuncion symbolfunc = ent.getFunc(this.id);
                if (symbolfunc != null)
                {
                    Generator generator = Generator.getInstance();
                    Entorno nuevo = new Entorno(ent, this.id);
                    string returnlabel = generator.newLabel();
                    HashSet<string> tempstorage = generator.getTempStorage();
                    nuevo.setEntornoFuncion(this.id, symbolfunc, returnlabel);
                    foreach (Param parmet in this.parametros)
                    {
                        nuevo.declararvariable(parmet.id, parmet.type, false, false,this.Linea,this.Columna);
                    }
                    generator.clearTempStorage();
                    generator.isFunc = "\t";
                    generator.addBegin(symbolfunc.uniqueId);
                    foreach (Instruccion inst in this.instrucciones)
                    {
                        inst.generar(nuevo,errorps);
                    }
                    generator.addLabel(returnlabel);
                    generator.addEnd();
                    generator.isFunc = "";
                    generator.setTempStorage(tempstorage);
                }
            }
            catch(Exception ex)
            {
                errorps.AddLast((Errorp)ex);
                Form1.salida.AppendText(ex.ToString());
            }
            
            return null;
        }
        private void validateParams(Entorno ent)
        {
            HashSet<string> set1 = new HashSet<string>();
            foreach(Param parmet in this.parametros)
            {
                if (set1.Contains(parmet.id.ToLower()))
                    throw new Errorp(Linea,Columna,"Semantico","Ya Existe un parametro con el id "+parmet.id,ent.nombre);
                if(parmet.type.type == Types.OBJECT)
                {

                }
                set1.Add(parmet.id.ToLower());
            }
        }
        private void validteType(Entorno ent)
        {
            if(this.tipo.type == Types.OBJECT)
            {
                //const struct = enviorement.getStruct(this.type.typeId);
                //if (!structt)
                  //  throw new Errorp(Linea,Columna,"Semantico","No existe el struct",ent.nombre);
            }
        }
        public string uniqueId(Entorno ent)
        {
            string id = ent.prop + "_" + this.id;
            if (this.parametros.Count == 0) return id + "_empty";
            foreach(Param parament in this.parametros)
            {
                id += "_" + parament.getUnicType();
            }
            return id;
        }
    }
}
