using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Access
{
    class AccessId : Expresion
    {
        public string labeltrue { get; set; }
        public string labelfalse { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }

        private string id;
        private Expresion anterior;

        public AccessId(string id, Expresion anterior, int linea, int columna)
        {
            this.id = id;
            this.anterior = anterior;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            Generator generator = Generator.getInstance();
            if (this.anterior == null)
            {
                Simbolo symbol = ent.getvariable(this.id);
                if (symbol == null)
                {
                    throw new Errorp(Linea, Columna, "Semantico", "No existe la variable: " + this.id, ent.nombre);
                }
                string temp = generator.newTemporal();
                if (symbol.isGlobal)
                {
                    generator.addGetStack(temp, symbol.position);
                    if (symbol.type.type != Types.BOOLEAN) return new Retorno(temp, true, symbol.type, symbol);

                    Retorno retorno = new Retorno("", false, symbol.type, symbol);
                    this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                    this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                    generator.addIF(temp, "1", "==", this.labeltrue);
                    generator.addGoto(this.labelfalse);
                    retorno.Labeltrue = this.labeltrue;
                    retorno.Labelfalse = this.labelfalse;
                    return retorno;
                }
                else
                {
                    string tempaux = generator.newTemporal();
                    generator.freeTemp(tempaux);
                    generator.addExpresion(tempaux, "SP", symbol.position, "+");
                    generator.addGetStack(temp, tempaux);
                    if (symbol.type.type != Types.BOOLEAN) return new Retorno(temp, true, symbol.type, symbol);
                    Retorno retorno = new Retorno("", false, symbol.type);
                    this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                    this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                    generator.addExpresion(temp, "1", "==", this.labeltrue);
                    generator.addGoto(this.labelfalse);
                    retorno.Labelfalse = this.labelfalse;
                    retorno.Labeltrue = this.labeltrue;
                    return retorno;
                }
            }
            else
            {
                Retorno anterior = this.anterior.resolver(ent);
                SimboloStruct symstruct = ent.getStruct(anterior.type.idtype);
                if (anterior.type.type != Types.OBJECT || symstruct == null)
                    throw new Errorp(Linea,Columna,"Semantico","Acceso NO valido para el tipo "+anterior.type.type,ent.nombre);
                var atribute = symstruct.getAttribute(this.id);

                if (atribute.valor == null)
                    throw new Errorp(Linea,Columna,"Semantico","El struct "+symstruct.id+" no tiene el atributo "+this.id,ent.nombre);
                string tempaux = generator.newTemporal();
                generator.freeTemp(tempaux);
                string temp = generator.newTemporal();

                generator.addExpresion(tempaux,anterior.getValor(),atribute.index,"+");
                generator.addGetHeap(temp, tempaux);

                if(atribute.valor.type.type != Types.BOOLEAN) return new Retorno(temp, true, atribute.valor.type);
                Retorno retorno = new Retorno("", false, atribute.valor.type);
                this.labeltrue = this.labeltrue == "" || this.labeltrue == null ? generator.newLabel() : this.labeltrue;
                this.labelfalse = this.labelfalse == "" || this.labelfalse == null ? generator.newLabel() : this.labelfalse;
                generator.addIF(temp, "1", "==", this.labeltrue);
                generator.addGoto(this.labelfalse);
                retorno.Labeltrue = this.labeltrue;
                retorno.Labelfalse = this.labelfalse;
                return retorno;

            }
        }

    }
}
