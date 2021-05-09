using compipascal2.Abstract;
using compipascal2.Expresiones.Literal;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Instrucciones.Funciones
{
    class StructFst : Instruccion
    {
        public int Linea { get;set; }
        public int Columna { get;set; }
        private string id;
        private LinkedList<Param> atributos;

        public StructFst(string id, LinkedList<Param> atributos, int linea, int columna)
        {
            this.id = id;
            this.atributos = atributos;
            Linea = linea;
            Columna = columna;
        }

        public object generar(Entorno ent, LinkedList<Errorp> errorps)
        {
            if (!ent.addStruct(this.id, this.atributos.Count, this.atributos))
                throw new Errorp(Linea,Columna,"Semantico","Ya existe un struct con el id "+this.id,ent.nombre);
            this.validateParams(ent);
            return null;
        }

        private void validateParams(Entorno ent)
        {
            HashSet<String> seting = new HashSet<string>();
            foreach(Param auxparam in this.atributos)
            {
                if (seting.Contains(auxparam.id.ToLower()))
                    throw new Errorp(Linea,Columna,"Semantico","Ya existe un atributo con el id "+auxparam.id,ent.nombre);
                if(auxparam.type.type == Types.OBJECT)
                {
                    SimboloStruct stru = ent.getStruct(auxparam.type.idtype);
                    if (stru == null)
                        throw new Errorp(Linea,Columna,"Semantico","No existe el object "+auxparam.type.idtype,ent.nombre);
                }
                seting.Add(auxparam.id.ToLower());
            }
        }
    }
}
