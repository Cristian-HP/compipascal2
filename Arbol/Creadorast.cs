using compipascal2.Abstract;
using compipascal2.Expresiones.Access;
using compipascal2.Expresiones.Arimeticas;
using compipascal2.Expresiones.Assignment;
using compipascal2.Expresiones.Literal;
using compipascal2.Expresiones.Logicas;
using compipascal2.Expresiones.Relacional;
using compipascal2.Instrucciones;
using compipascal2.Instrucciones.Control;
using compipascal2.Instrucciones.Funciones;
using compipascal2.Instrucciones.Tranferencia;
using compipascal2.Instrucciones.Variables;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Arbol
{
    class Creadorast
    {
        private ParseTree treeirony;

        public AST mytree { get; set; }

        public Creadorast(ParseTree tree)
        {
            treeirony = tree;
            creador(treeirony.Root);
        }
        private void creador(ParseTreeNode root)
        {
            mytree = (AST)analisisnodo(root);
        }
        private object analisisnodo(ParseTreeNode current)
        {
            if (equalnode(current, "INICIO"))
            {
                LinkedList<Instruccion> temp = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[0]);
                LinkedList<Funcion> funciones = new LinkedList<Funcion>();
                //LinkedList<Metodo> metodos = new LinkedList<Metodo>();
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                foreach (Instruccion inst in temp)
                {
                    /*if (inst is Funcion)
                    {
                        funciones.AddLast((Funcion)inst);
                    }
                    else
                    {
                        instrucciones.AddLast(inst);
                    }*/
                    instrucciones.AddLast(inst);
                }
                //return new AST(instrucciones, funciones, metodos);
                return new AST(instrucciones,funciones);
            }
            else if (equalnode(current, "PROGRAM"))
            {
                LinkedList<Instruccion> instrucc = new LinkedList<Instruccion>();

                if (current.ChildNodes.Count == 4)
                {
                    Group bloq = (Group)analisisnodo(current.ChildNodes[3]);
                    iterarbloque(instrucc, bloq);
                }
                else if (current.ChildNodes.Count == 5)
                {
                    LinkedList<Group> bloques = (LinkedList<Group>)analisisnodo(current.ChildNodes[3]);

                    foreach (Group bloque in bloques)
                    {
                        iterarbloque(instrucc, bloque);
                    }
                    iterarbloque(instrucc, (Group)analisisnodo(current.ChildNodes[4]));
                }

                return instrucc;

            }
            else if (equalnode(current, "LISTA_PARTES"))
            {
                LinkedList<Group> partes = new LinkedList<Group>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    partes.AddLast((Group)analisisnodo(hijo));
                }
                return partes;
            }
            else if (equalnode(current, "PARTE"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "DECLARACIONES"))
            {
                return new Group((LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]));
            }
            else if (equalnode(current, "CONSTANTE_GLO"))
            {
                return new Group((LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]));
            }
            else if (equalnode(current, "TYPES_GLO"))
            {
                return new Group((LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]));
            }
            else if (equalnode(current, "LISTA_VARIABLE"))
            {
                LinkedList<Instruccion> lisdecla = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    lisdecla.AddLast((Instruccion)analisisnodo(hijo));
                }
                return lisdecla;
            }
            else if (equalnode(current, "LISTA_FUNPRO"))
            {
                LinkedList<Instruccion> funopro = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    funopro.AddLast((Instruccion)analisisnodo(hijo));
                }
                return new Group(funopro);
            }
            else if (equalnode(current, "FUNPRO"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "PROCEDIMIENTO"))
            {
                string idfun = current.ChildNodes[1].Token.Text.ToLower();
                LinkedList<Instruccion> instrucciones;
                int linea = current.ChildNodes[0].Token.Location.Line;
                int columna = current.ChildNodes[0].Token.Location.Column;
                Utils.Type tipomet = new Utils.Type(Utils.Types.VOID, "");
                //termino de la declaracion
                if (current.ChildNodes.Count == 7)
                {
                    LinkedList<Param> parametros = (LinkedList<Param>)analisisnodo(current.ChildNodes[3]);
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[6]);
                    return new Funcion(idfun, tipomet,instrucciones,parametros,linea,columna);
                }
                else if (current.ChildNodes.Count == 6)
                {
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[5]);
                    return new Funcion(idfun,tipomet,instrucciones,new LinkedList<Param>(),linea,columna);
                }
                else
                {
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[3]);
                    return new Funcion(idfun,tipomet,instrucciones,new LinkedList<Param>(),linea,columna);
                }
            }
            else if (equalnode(current, "FUNCIONES"))
            {
                string idfun = current.ChildNodes[1].Token.Text.ToLower();
                Utils.Type tipofun;
                LinkedList<Instruccion> instrucciones;
                int linea = current.ChildNodes[0].Token.Location.Line;
                int columna = current.ChildNodes[0].Token.Location.Column;
                if (current.ChildNodes.Count == 9)
                {
                    tipofun = (Utils.Type)analisisnodo(current.ChildNodes[6]);
                    LinkedList<Param> parametros = (LinkedList<Param>)analisisnodo(current.ChildNodes[3]);
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[8]);
                    return new Funcion(idfun,tipofun,instrucciones,parametros,linea,columna);
                }
                else if (current.ChildNodes.Count == 8)
                {
                    tipofun = (Utils.Type)analisisnodo(current.ChildNodes[5]);
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[7]);
                    return new Funcion(idfun,tipofun,instrucciones,new LinkedList<Param>(),linea,columna);
                }
                else
                {
                    tipofun = (Utils.Type)analisisnodo(current.ChildNodes[3]);
                    instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[5]);
                    return new Funcion(idfun,tipofun,instrucciones,new LinkedList<Param>(),linea,columna);

                }
            }
            else if (equalnode(current, "L_PARAM"))
            {
                LinkedList<Param> listaparam = new LinkedList<Param>();
                LinkedList<Param> param;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    param = (LinkedList<Param>)analisisnodo(hijo);
                    foreach (Param simbol in param)
                    {
                        listaparam.AddLast(simbol);
                    }
                }
                return listaparam;
            }
            else if (equalnode(current, "PARAM"))
            {
                if (current.ChildNodes.Count == 4)
                {
                    //por referencia
                    return null;
                }
                else
                {
                    LinkedList<Param> parametros = new LinkedList<Param>();
                    LinkedList<string> varia = (LinkedList<string>)analisisnodo(current.ChildNodes[0]);
                    Utils.Type tipo = (Utils.Type)analisisnodo(current.ChildNodes[2]);
                    foreach (String auxtipo in varia)
                    {
                        parametros.AddLast(new Param(auxtipo.ToLower(),tipo));
                    }
                    return parametros;
                }

            }
            else if (equalnode(current, "LLAMA"))
            {
                int linea = current.ChildNodes[0].Token.Location.Line;
                int columna = current.ChildNodes[0].Token.Location.Column;
                string idfun = current.ChildNodes[0].Token.Text.ToLower();
                if (current.ChildNodes.Count == 5)
                {
                    LinkedList<Expresion> expresio = (LinkedList<Expresion>)analisisnodo(current.ChildNodes[2]);
                    return new AssigmentFunct(idfun, expresio, linea, columna);
                }
                else if (current.ChildNodes.Count == 4)
                {
                    if (current.ChildNodes[3].Token.Text.Equals(")"))
                    {
                        LinkedList<Expresion> expresio = (LinkedList<Expresion>)analisisnodo(current.ChildNodes[2]);
                        return new AssigmentFunct(idfun, expresio, linea, columna);
                    }
                    else
                        return new AssigmentFunct(idfun, new LinkedList<Expresion>(), linea, columna);
                }
                else
                {
                    return new AssigmentFunct(idfun, new LinkedList<Expresion>(), linea, columna);
                }
            }
            else if (equalnode(current, "BODY"))
            {
                if (current.ChildNodes.Count == 2)
                {
                    LinkedList<Instruccion> dec = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[0]);
                    LinkedList<Instruccion> insts = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]);
                    foreach (Instruccion inst in insts)
                    {
                        dec.AddLast(inst);
                    }
                    return dec;

                }
                else
                {
                    return analisisnodo(current.ChildNodes[0]);
                }
            }
            else if (equalnode(current, "L_DEF"))
            {
                LinkedList<Instruccion> todas = new LinkedList<Instruccion>();
                Group temlis;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    temlis = (Group)analisisnodo(hijo);
                    iterarbloque(todas, temlis);
                }
                return todas;
            }
            else if (equalnode(current, "DEF"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "VARIABLE"))
            {
                LinkedList<string> variables;
                Utils.Type tipo;
                if (current.ChildNodes.Count == 4)
                {
                    variables = (LinkedList<string>)analisisnodo(current.ChildNodes[0]);
                    tipo = (Utils.Type)analisisnodo(current.ChildNodes[2]);
                    Expresion value = null;
                    if(tipo.type == Types.OBJECT)
                    {
                        value = new NewStruct(tipo.idtype, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                    }
                    return new Declaracion(tipo,variables,value, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                }
                else if (current.ChildNodes.Count == 6)
                {
                    variables = (LinkedList<string>)analisisnodo(current.ChildNodes[0]);
                    tipo = (Utils.Type)analisisnodo(current.ChildNodes[2]);
                    Expresion valor = (Expresion)analisisnodo(current.ChildNodes[4]);
                    return new Declaracion(tipo,variables, valor, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                }
            }
            else if (equalnode(current, "LISTA_ID"))
            {
                LinkedList<string> variables = new LinkedList<string>();
                string aux1;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    aux1 = obtenerid(hijo).ToLower();
                    variables.AddLast(aux1);
                }
                return variables;
            }
            else if (equalnode(current, "TIPO_CONID"))
            {
                if (equalid(current.ChildNodes[0]))
                    return new Utils.Type(Utils.Types.OBJECT, obtenerid(current.ChildNodes[0]).ToLower());
                else
                {
                    ParseTreeNode aux2 = current.ChildNodes[0].ChildNodes[0];
                    if (aux2.Token.Text.Equals("integer", StringComparison.InvariantCultureIgnoreCase))
                        return new Utils.Type(Utils.Types.INTEGER, "");
                    else if (aux2.Token.Text.Equals("string", StringComparison.InvariantCultureIgnoreCase))
                        return new Utils.Type(Utils.Types.STRING, "");
                    else if (aux2.Token.Text.Equals("real", StringComparison.InvariantCultureIgnoreCase))
                        return new Utils.Type(Utils.Types.REAL, "");
                    else if (aux2.Token.Text.Equals("boolean", StringComparison.InvariantCultureIgnoreCase))
                        return new Utils.Type(Utils.Types.BOOLEAN, "");
                }
            }
            else if (equalnode(current, "ASIGN"))
            {
                Expresion valor = (Expresion)analisisnodo(current.ChildNodes[2]);
                AssignmentId tar = (AssignmentId)analisisnodo(current.ChildNodes[0]);
                return new Asignacion(tar,valor, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
            }
            else if(equalnode(current, "ASIGMENT"))
            {
                if(current.ChildNodes.Count == 1)
                {
                    return new AssignmentId(obtenerid(current.ChildNodes[0]).ToLower(),null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }else if(current.ChildNodes.Count == 3)
                {
                    AssignmentId temp = (AssignmentId)analisisnodo(current.ChildNodes[0]);
                    return new AssignmentId(obtenerid(current.ChildNodes[2]).ToLower(),temp,current.ChildNodes[2].Token.Location.Line, current.ChildNodes[2].Token.Location.Column);
                }
                else
                {
                    // esto para el array
                }
            }
            else if (equalnode(current, "MAIN"))
            {
                return new Group((LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]));
            }
            else if (equalnode(current, "LISTA_INTS"))
            {
                LinkedList<Instruccion> intruc = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    intruc.AddLast((Instruccion)analisisnodo(hijo));
                }
                return intruc;
            }
            else if (equalnode(current, "INST"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "WRT"))
            {
                bool jump = false;
                if (current.ChildNodes[0].Token.Text.Equals("writeln", StringComparison.InvariantCultureIgnoreCase))
                    jump = true;
                return new Writeln((LinkedList<Expresion>)analisisnodo(current.ChildNodes[2]), jump, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "IF"))
            {
                if (current.ChildNodes.Count == 4)
                {
                    if (current.ChildNodes[3].Term.Name.Equals("INST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                        Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                        Instruccion instrucc = (Instruccion)analisisnodo(current.ChildNodes[3]);
                        instrucciones.AddLast(instrucc);
                        return new If(valor, instrucciones, null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                    }
                    else
                    {
                        LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[3]);
                        Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                        return new If(valor, instrucciones, null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                    }
                }
                else if (current.ChildNodes.Count == 5)
                {
                    LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                    Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                    Instruccion instrucc = (Instruccion)analisisnodo(current.ChildNodes[3]);
                    instrucciones.AddLast(instrucc);
                    Instruccion inselse = (Instruccion)analisisnodo(current.ChildNodes[4]);
                    return new If(valor, instrucciones, inselse, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[4]);
                    Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                    Instruccion inselse = (Instruccion)analisisnodo(current.ChildNodes[6]);
                    return new If(valor, instrucciones, inselse, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
            }
            else if (equalnode(current, "ELSE"))
            {
                if (current.ChildNodes[1].Term.Name.Equals("INST", StringComparison.InvariantCulture))
                {
                    LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                    Instruccion instrucc = (Instruccion)analisisnodo(current.ChildNodes[1]);
                    instrucciones.AddLast(instrucc);
                    return new Else(instrucciones, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]);
                    return new Else(instrucciones, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
            }
            else if (equalnode(current, "WL"))
            {
                if (current.ChildNodes[3].Term.Name.Equals("INST", StringComparison.InvariantCulture))
                {
                    LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                    Instruccion instrucc = (Instruccion)analisisnodo(current.ChildNodes[3]);
                    Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                    instrucciones.AddLast(instrucc);
                    return new While(valor,instrucciones, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[3]);
                    Expresion valor = (Expresion)analisisnodo(current.ChildNodes[1]);
                    return new While(valor, instrucciones,  current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
            }
            else if (equalnode(current, "REP"))
            {
                Expresion condi = (Expresion)analisisnodo(current.ChildNodes[3]);
                LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[1]);
                return new Repeat(condi, instrucciones, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "FOR"))
            {
                int linea = current.ChildNodes[0].Token.Location.Line;
                int columna = current.ChildNodes[0].Token.Location.Column;
                //Id idini = new Id(obtenerid(current.ChildNodes[1]), linea, current.ChildNodes[1].Token.Location.Column);
                string nombre = obtenerid(current.ChildNodes[1]).ToLower();
                AssignmentId idini = new AssignmentId(nombre,null,linea,current.ChildNodes[1].Token.Location.Column);
                Expresion inicio = (Expresion)analisisnodo(current.ChildNodes[3]);
                Asignacion asigini = new Asignacion(idini, inicio, linea, columna);
                Expresion idinicio = new AccessId(nombre,null,linea,columna);
                Expresion tope = (Expresion)analisisnodo(current.ChildNodes[5]);
                string direccion = current.ChildNodes[4].ChildNodes[0].Token.Text;
                Expresion condicion;
                Expresion condicionupdat = new Igualq(idinicio, tope, linea, columna);
                Asignacion update;
                if (direccion.ToLower() == "to")
                {
                    condicion = new Menorq(idinicio, tope, true, linea, columna);
                    update = new Asignacion(idini,new Suma(idinicio,new PrimitiveL(Utils.Types.INTEGER,"1",linea,columna),linea,columna),linea,columna);
                }
                else
                {
                    condicion = new Mayorq(idinicio, tope, true, linea, columna);
                    update = new Asignacion(idini, new Resta(idinicio, new PrimitiveL(Utils.Types.INTEGER, "1", linea, columna), linea, columna), linea, columna);
                }
                if (current.ChildNodes[7].Term.Name.Equals("INST", StringComparison.InvariantCultureIgnoreCase))
                {
                    LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                    Instruccion instrucc = (Instruccion)analisisnodo(current.ChildNodes[7]);
                    instrucciones.AddLast(instrucc);
                    return new For(asigini,condicion,update,condicionupdat, instrucciones, linea, columna);
                }
                else
                {
                    LinkedList<Instruccion> instrucciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[7]);
                    return new For(asigini,condicion,update,condicionupdat, instrucciones, linea, columna);
                }
            }
            else if (equalnode(current, "CASE"))
            {
                AccessId temid = (AccessId)analisisnodo(current.ChildNodes[1]);
                LinkedList<Case> opciones = (LinkedList<Case>)analisisnodo(current.ChildNodes[3]);
                if (current.ChildNodes.Count == 7)
                {
                    Else temelse = (Else)analisisnodo(current.ChildNodes[4]);
                    return new Switch(temid, opciones, temelse, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return new Switch(temid, opciones, null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }

            }
            else if (equalnode(current, "L_OPC"))
            {
                LinkedList<Case> opciones = new LinkedList<Case>();
                Case temcase;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    temcase = (Case)analisisnodo(hijo);
                    opciones.AddLast(temcase);
                }
                return opciones;
            }
            else if (equalnode(current, "OPC"))
            {
                LinkedList<Expresion> etiq = (LinkedList<Expresion>)analisisnodo(current.ChildNodes[0]);
                LinkedList<Instruccion> instruciones;
                if (current.ChildNodes[2].Term.Name.Equals("INST", StringComparison.InvariantCultureIgnoreCase))
                {
                    instruciones = new LinkedList<Instruccion>();
                    Instruccion temp = (Instruccion)analisisnodo(current.ChildNodes[2]);
                }
                instruciones = (LinkedList<Instruccion>)analisisnodo(current.ChildNodes[2]);
                return new Case(etiq, instruciones, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);

            }
            else if (equalnode(current, "L_EBC"))
            {
                LinkedList<Expresion> listaeti = new LinkedList<Expresion>();
                Expresion temexp;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    temexp = (Expresion)analisisnodo(hijo);
                    listaeti.AddLast(temexp);
                }
                return listaeti;
            }
            else if (equalnode(current, "EBC"))
            {
                if (current.ChildNodes.Count == 2)
                {
                    int temp = int.Parse(current.ChildNodes[1].Token.Text);
                    temp = temp * -1;
                    return new PrimitiveL(Utils.Types.INTEGER ,temp, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "BKR"))
            {
                return new Break(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "COT"))
            {
                return new Continue(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "CONSTANTE"))
            {
                LinkedList<string> variables = new LinkedList<string>();
                string name = obtenerid(current.ChildNodes[0]);
                variables.AddLast(name);
                Expresion valor = (Expresion)analisisnodo(current.ChildNodes[2]);
                return new Declaracion(null,variables,valor, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column,true);
            }
            else if (equalnode(current, "LISTA_CONSTANTE"))
            {
                LinkedList<Instruccion> constantes = new LinkedList<Instruccion>();
                Declaracion auxconst;
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    auxconst = (Declaracion)analisisnodo(hijo);
                    constantes.AddLast(auxconst);
                }
                return constantes;
            }
            else if (equalnode(current, "BEGIN"))
            {
                return analisisnodo(current.ChildNodes[1]);
            }
            else if (equalnode(current, "EXT"))
            {
                if(current.ChildNodes.Count == 5)
                {
                    return new Return(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column, (Expresion)analisisnodo(current.ChildNodes[2]));
                }else if(current.ChildNodes.Count == 4)
                {
                    if(current.ChildNodes[2].Token.Text == ")")
                    {
                        return new Return(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column, null);
                    }
                    else
                    {
                        return new Return(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column, (Expresion)analisisnodo(current.ChildNodes[2]));
                    }
                }
                else
                {
                    return new Return(current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column,null);
                }
                
            }
            else if (equalnode(current, "LISTA_TYPES"))
            {
                LinkedList<Instruccion> lisdecla = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    lisdecla.AddLast((Instruccion)analisisnodo(hijo));
                }
                return lisdecla;
            }
            else if (equalnode(current, "TYPES"))
            {
                string id = current.ChildNodes[0].Token.Text.ToString().ToLower();
                if (current.ChildNodes[2].Term.Name.Equals("OBJETO"))
                {
                    LinkedList<Param> lista_elements = (LinkedList<Param>)analisisnodo(current.ChildNodes[2]);
                    return new StructFst(id,lista_elements,current.ChildNodes[0].Token.Location.Line,current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    //array
                }
            }
            else if (equalnode(current, "OBJETO"))
            {
                return analisisnodo(current.ChildNodes[1]);
            }
            else if(equalnode(current, "L_ELEM"))
            {
                LinkedList<Param> lista_element = new LinkedList<Param>();
                foreach(ParseTreeNode hijo in current.ChildNodes)
                {
                    LinkedList<Param> elements = (LinkedList<Param>)analisisnodo(hijo);
                    foreach(Param element in elements)
                    {
                        lista_element.AddLast(element);
                    }
                }
                return lista_element;
            }
            else if (equalnode(current, "ELEM"))
            {
                LinkedList<string> listid = (LinkedList<string>)analisisnodo(current.ChildNodes[0]);
                Utils.Type tipo = (Utils.Type)analisisnodo(current.ChildNodes[2]);
                LinkedList<Param> elemntos = new LinkedList<Param>();
                foreach(string aux in listid)
                {
                    elemntos.AddLast(new Param(aux,tipo));
                }
                return elemntos;
            }
            else if (equalnode(current, "L_VARELE"))
            {
                LinkedList<Param> lista_tot = new LinkedList<Param>();
                foreach(ParseTreeNode hijo in current.ChildNodes)
                {
                    LinkedList<Param> aux = (LinkedList<Param>)analisisnodo(hijo);
                    foreach(Param temp in aux)
                    {
                        lista_tot.AddLast(temp);
                    }
                }
                return lista_tot;
            }
            else if (equalnode(current, "VARELM"))
            {
                return analisisnodo(current.ChildNodes[1]);
            }
            else if (equalnode(current, "L_EXP"))
            {
                LinkedList<Expresion> expresioness = new LinkedList<Expresion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    expresioness.AddLast((Expresion)analisisnodo(hijo));
                }
                return expresioness;
            }
            else if (equalnode(current, "ELOG"))
            {
                if (current.ChildNodes.Count == 3)
                {
                    if (current.ChildNodes[0].Term.Name.Equals("("))
                        return analisisnodo(current.ChildNodes[1]);
                    else
                    {
                        Expresion izq = (Expresion)analisisnodo(current.ChildNodes[0]);
                        string tip = current.ChildNodes[1].Token.Text;
                        Expresion der = (Expresion)analisisnodo(current.ChildNodes[2]);
                        if(tip.ToLower() == "and")
                        {
                            return new And(izq, der, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                        }
                        else
                        {
                            return new Or(izq, der, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                        }
                        //return new Logica(izq, der, tip, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                    }
                }
                else if (current.ChildNodes.Count == 2)
                {
                    Expresion izq = (Expresion)analisisnodo(current.ChildNodes[1]);
                    return new Not(izq, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                    //return new Logica(izq, "n", current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return analisisnodo(current.ChildNodes[0]);
                }
            }
            else if (equalnode(current, "ERELA"))
            {
                if (current.ChildNodes.Count == 3)
                {
                    if (current.ChildNodes[0].Term.Name.Equals("("))
                        return analisisnodo(current.ChildNodes[1]);
                    else
                    {
                        Expresion izq = (Expresion)analisisnodo(current.ChildNodes[0]);
                        string tip = current.ChildNodes[1].Token.Text;
                        Expresion der = (Expresion)analisisnodo(current.ChildNodes[2]);
                        return Relacionales(izq,der,tip, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                        //return new Relacional(izq, der, tip, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                    }
                }
                else
                {
                    return analisisnodo(current.ChildNodes[0]);
                }
            }
            else if (equalnode(current, "EARI"))
            {
                if (current.ChildNodes.Count == 3)
                {
                    if (current.ChildNodes[0].Term.Name.Equals("("))
                        return analisisnodo(current.ChildNodes[1]);
                    else
                    {
                        Expresion izq = (Expresion)analisisnodo(current.ChildNodes[0]);
                        string tip = current.ChildNodes[1].Token.Text;
                        Expresion der = (Expresion)analisisnodo(current.ChildNodes[2]);
                        return Arimeticas(izq,der,tip, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                        
                    }
                }
                else if (current.ChildNodes.Count == 2)
                {
                    Expresion izq = (Expresion)analisisnodo(current.ChildNodes[1]);
                    //string tip = current.ChildNodes[1].Token.Text;
                    return Arimeticas(izq,null,"-", current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                    //return new Arimetica(izq, "-", current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return analisisnodo(current.ChildNodes[0]);
                }
            }
            else if (equalnode(current, "ACCESSID"))
            {
                if(current.ChildNodes.Count == 1)
                {
                    return new AccessId(obtenerid(current.ChildNodes[0]).ToLower(), null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                }else if(current.ChildNodes.Count == 3)
                {
                    Expresion temp = (Expresion)analisisnodo(current.ChildNodes[0]);
                    return new AccessId(obtenerid(current.ChildNodes[2]).ToLower(),temp,current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                }
                else
                {
                    //array
                }
            }
            else if (equaliteral(current))
            {
                return obtenerliteral(current);
                //return new Literal(obtenerliteral(current), current.Token.Location.Line, current.Token.Location.Column);

            }
            else if (equalid(current))
            {
                return new AccessId(obtenerid(current).ToLower(), null, current.Token.Location.Line, current.Token.Location.Column);
            }
            return null;
        }
        
        
        
        //metodos Generales para analisis
        private bool equalnode(ParseTreeNode node, string name)
        {
            return node.Term.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase);
        }
        private void iterarbloque(LinkedList<Instruccion> temp, Group bloque)
        {
            LinkedList<Instruccion> instrucciones = bloque.instrucciones;
            foreach (Instruccion inst in instrucciones)
            {
                if (inst is Group)
                    iterarbloque(temp, (Group)inst);
                else
                    temp.AddLast(inst);
            }
        }

        private bool equaliteral(ParseTreeNode node)
        {
            if (node.ToString().EndsWith("(CADENA)") || node.ToString().EndsWith("(NUMBER)") || node.ToString().EndsWith("(BOOLEAN)"))
                return true;
            return false;
        }

        private Expresion obtenerliteral(ParseTreeNode node)
        {
            // case de opciones 
            if (node.ToString().EndsWith("(CADENA)"))
            {
                return new StringL(Utils.Types.STRING, node.Token.Text.Replace("'", ""), node.Token.Location.Line, node.Token.Location.Column);
                //return node.Token.Text.Replace("'", "");
            }
            else if (node.ToString().EndsWith("(NUMBER)"))
            {
                try
                {
                    return new PrimitiveL(Utils.Types.INTEGER, int.Parse(node.Token.Text), node.Token.Location.Line, node.Token.Location.Column);
                    //return int.Parse(node.Token.Text);
                }
                catch
                {
                    return new PrimitiveL(Utils.Types.REAL, double.Parse(node.Token.Text), node.Token.Location.Line, node.Token.Location.Column);
                    //return double.Parse(node.Token.Text);
                }
            }
            else
            {
                if (node.Token.Text.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new PrimitiveL(Utils.Types.BOOLEAN, true, node.Token.Location.Line, node.Token.Location.Column);
                    //return true;
                }
                else
                {
                    return new PrimitiveL(Utils.Types.BOOLEAN, false, node.Token.Location.Line, node.Token.Location.Column);
                    //return false;
                }
            }
        }


        private string obtenerid(ParseTreeNode node)
        {
            return node.Token.Text.ToString();
        }

        private bool equalid(ParseTreeNode node)
        {
            return node.ToString().EndsWith("(ID)");
        }
        private Expresion Arimeticas(Expresion izq,Expresion der,string tip,int linea,int columna)
        {
            switch (tip)
            {
                case "+":
                    return new Suma(izq,der,linea,columna);
                case "-":
                    if(der != null)
                    {
                        return new Resta(izq, der, linea, columna);
                    }
                    else
                    {
                        return new Resta(new PrimitiveL(Utils.Types.INTEGER,0,1,1), izq, linea, columna);
                    }
                case "*":
                    return new Multi(izq, der, linea, columna);
                case "/":
                    return new Div(izq, der, linea, columna);
                case "%":
                    return new Mod(izq, der, linea, columna);
            }
            return null;
        }
        private Expresion Relacionales(Expresion izq, Expresion der, string tip, int linea, int columna)
        {
            switch (tip)
            {
                case ">=":
                    return new Mayorq(izq,der,true,linea,columna);
                case ">":
                    return new Mayorq(izq, der, false, linea, columna);
                case "<=":
                    return new Menorq(izq, der, true, linea, columna);
                case "<":
                    return new Menorq(izq, der, false, linea, columna);
                case "=":
                    return new Igualq(izq, der, linea, columna);
                case "<>":
                    return new Diferente(izq, der, linea, columna);
            }
            return null;
        }
    }
}
