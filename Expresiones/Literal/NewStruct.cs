using compipascal2.Abstract;
using compipascal2.Generador;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Expresiones.Literal
{
    class NewStruct : Expresion
    {
        public string labeltrue { get;set; }
        public string labelfalse { get;set; }
        public int Linea { get;set; }
        public int Columna { get;set; }
        private string id;

        public NewStruct(string id, int linea, int columna)
        {
            this.id = id;
            Linea = linea;
            Columna = columna;
        }

        public Retorno resolver(Entorno ent)
        {
            SimboloStruct symstruct = ent.getStruct(id.ToLower());
            Generator generator = Generator.getInstance();
            if (symstruct == null)
                throw new Errorp(Linea,Columna,"Semantico","No existe el struct "+this.id +" en el ambito",ent.nombre);
            string temp = generator.newTemporal();
            generator.addExpresion(temp, "HP", "", "");
            foreach(Param atri in symstruct.atributos)
            {
                switch (atri.type.type)
                {
                    case Types.INTEGER:
                    case Types.REAL:
                    case Types.BOOLEAN:
                        generator.addSetHeap("HP", "0");
                        generator.nextHeap();
                        break;
                    case Types.STRING:
                    case Types.ARRAY:
                        generator.addSetHeap("HP", "-1");
                        generator.nextHeap();
                        break;
                    case Types.OBJECT:
                        string tem2 = generator.newTemporal();
                        generator.freeTemp(tem2);
                        generator.addExpresion(tem2,"HP","1","+");
                        generator.addSetHeap("HP",tem2);
                        generator.nextHeap();
                        NewStruct aux22 = new NewStruct(atri.type.idtype,Linea,Columna);
                        Retorno pru =aux22.resolver(ent);
                        break;
                }
                
            }
            return new Retorno(temp, true, new Utils.Type(Types.OBJECT, symstruct.id));
        }
    }
}
