using compipascal2.Abstract;
using compipascal2.Expresiones.Access;
using compipascal2.Expresiones.Arimeticas;
using compipascal2.Expresiones.Assignment;
using compipascal2.Expresiones.Literal;
using compipascal2.Expresiones.Logicas;
using compipascal2.Expresiones.Relacional;
using compipascal2.Instrucciones;
using compipascal2.Instrucciones.Control;
using compipascal2.Instrucciones.Variables;
using compipascal2.SymbolTable;
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
                //LinkedList<Funcion> funciones = new LinkedList<Funcion>();
                //LinkedList<Metodo> metodos = new LinkedList<Metodo>();
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                foreach (Instruccion inst in temp)
                {
                    /*if (inst is Funcion)
                    {
                        funciones.AddLast((Funcion)inst);
                    }
                    else if (inst is Metodo)
                    {
                        metodos.AddLast((Metodo)inst);
                    }
                    else
                    {
                        instrucciones.AddLast(inst);
                    }*/
                    instrucciones.AddLast(inst);
                }
                //return new AST(instrucciones, funciones, metodos);
                return new AST(instrucciones);
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
            else if (equalnode(current, "LISTA_VARIABLE"))
            {
                LinkedList<Instruccion> lisdecla = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in current.ChildNodes)
                {
                    lisdecla.AddLast((Instruccion)analisisnodo(hijo));
                }
                return lisdecla;
            }
            else if (equalnode(current, "VARIABLE"))
            {
                LinkedList<string> variables;
                Utils.Type tipo;
                if (current.ChildNodes.Count == 4)
                {
                    variables = (LinkedList<string>)analisisnodo(current.ChildNodes[0]);
                    tipo = (Utils.Type)analisisnodo(current.ChildNodes[2]);
                    return new Declaracion(tipo,variables,null, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
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
                AssignmentId tar = new AssignmentId(obtenerid(current.ChildNodes[0]).ToLower(),null, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
                return new Asignacion(tar,valor, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
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
            else if (equalnode(current, "BEGIN"))
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
            else if (equaliteral(current))
            {
                return obtenerliteral(current);
                //return new Literal(obtenerliteral(current), current.Token.Location.Line, current.Token.Location.Column);

            }
            else if (equalid(current))
            {
                return new AccessId(obtenerid(current).ToLower(),null, current.Token.Location.Line, current.Token.Location.Column);
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
