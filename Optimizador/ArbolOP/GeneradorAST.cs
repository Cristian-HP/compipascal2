using compipascal2.Optimizador.AbstracOP;
using compipascal2.Optimizador.ExpresionesOP;
using compipascal2.Optimizador.InstruccionesOP;
using compipascal2.Optimizador.InstruccionesOP.OptiPrimitivas;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using static compipascal2.Optimizador.ExpresionesOP.OperacionOP;

namespace compipascal2.Optimizador.ArbolOP
{
    class GeneradorAST
    {
        private ParseTree treeirony;

        public ASTOP mytree { get; set; }
        public LinkedList<FuncionesOP> mifuncion { get; set; }
        public string Encabezado { get; set; }

        public GeneradorAST(ParseTree tree)
        {
            treeirony = tree;
            creador(treeirony.Root);
        }
        private void creador(ParseTreeNode root)
        {
            mifuncion = (LinkedList<FuncionesOP>)analisisnodo(root);
            //mytree = (ASTOP)analisisnodo(root);
        }

        private object analisisnodo(ParseTreeNode current)
        {
            if (equalnode(current, "INICIO"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current,"PROGRAM")){
                analisisnodo(current.ChildNodes[0]);
                LinkedList<FuncionesOP> funciones = (LinkedList<FuncionesOP>)analisisnodo(current.ChildNodes[1]);
                return funciones;
            }
            else if(equalnode(current, "ENCABEZADO"))
            {
                int numtemp = current.ChildNodes[5].ChildNodes[1].ChildNodes.Count;
                Encabezado = "#include <stdio.h>\n";
                Encabezado += "float Heap[100000];\n";
                Encabezado += "float Stack[100000];\n";
                Encabezado += "int SP;\n";
                Encabezado += "int HP;\n";
                Encabezado += "float ";
                for (int i = 0; i < numtemp-1; i++)
                {
                    Encabezado += "T" + i + ",";
                }
                Encabezado += "T" + (numtemp-1) + ";\n";
                
            }
            else if(equalnode(current, "L_FUN"))
            {
                LinkedList<FuncionesOP> L_funciones = new LinkedList<FuncionesOP>();
                foreach(ParseTreeNode hijo in current.ChildNodes)
                {
                    L_funciones.AddLast((FuncionesOP)analisisnodo(hijo));
                }
                return L_funciones;
            }
            else if (equalnode(current, "FUNCIONES"))
            {
                string idfun = obtenerid(current.ChildNodes[1]);
                if (current.ChildNodes.Count == 8)
                {
                    LinkedList<InstruccionOP> L_intruc = (LinkedList<InstruccionOP>)analisisnodo(current.ChildNodes[5]);
                    Etiqueta primera = new Etiqueta("//init",L_intruc,1,1);
                    LinkedList<Etiqueta> L_etiqueta = (LinkedList<Etiqueta>)analisisnodo(current.ChildNodes[6]);
                    L_etiqueta.AddFirst(primera);
                    return new FuncionesOP(idfun, L_etiqueta);
                }
                else
                {
                    try
                    {
                        LinkedList<Etiqueta> L_etiqueta = (LinkedList<Etiqueta>)analisisnodo(current.ChildNodes[5]);
                        return new FuncionesOP(idfun, L_etiqueta);
                    }
                    catch
                    {
                        LinkedList<InstruccionOP> L_instruc = (LinkedList<InstruccionOP>)analisisnodo(current.ChildNodes[5]);
                        Etiqueta primera = new Etiqueta("//init", L_instruc, 1, 1);
                        LinkedList<Etiqueta> L_eti = new LinkedList<Etiqueta>();
                        L_eti.AddLast(primera);
                        return new FuncionesOP(idfun,L_eti);
                    }
                    
                }
               
                
            }
            else if (equalnode(current, "L_INST"))
            {
                LinkedList<InstruccionOP> L_inst = new LinkedList<InstruccionOP>();
                foreach(ParseTreeNode hijo in current.ChildNodes)
                {
                    L_inst.AddLast((InstruccionOP)analisisnodo(hijo));
                }
                return L_inst;
            }
            else if(equalnode(current, "INST"))
            {
                return analisisnodo(current.ChildNodes[0]);
            }
            else if (equalnode(current, "L_ETIQ"))
            {
                LinkedList<Etiqueta> L_etique = new LinkedList<Etiqueta>();
                foreach(ParseTreeNode hijo in current.ChildNodes)
                {
                    L_etique.AddLast((Etiqueta)analisisnodo(hijo));
                }
                return L_etique;
            }
            else if(equalnode(current, "ETIQ"))
            {
                string idlabel = current.ChildNodes[0].Token.Text;
                LinkedList<InstruccionOP> L_inst;
                if (current.ChildNodes.Count == 3)
                {
                   L_inst = (LinkedList<InstruccionOP>)analisisnodo(current.ChildNodes[2]);
                }
                else
                {
                    L_inst = new LinkedList<InstruccionOP>();
                }
                
                return new Etiqueta(idlabel,L_inst, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
            }
            else if(equalnode(current, "ASIG"))
            {
                string id = analisisnodo(current.ChildNodes[0]).ToString();
                OperacionOP valor = (OperacionOP)analisisnodo(current.ChildNodes[2]);
                return new AsignacionOP(id,valor,current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
            }
            else if(equalnode(current, "ACCESO"))
            {
                string id = current.ChildNodes[0].Token.Text + "[";
                if (current.ChildNodes.Count == 5)
                {
                    id += "(" + analisisnodo(current.ChildNodes[2]).ToString() + ")";
                    id += analisisnodo(current.ChildNodes[3]).ToString() + "]";
                }
                else
                {
                   id += analisisnodo(current.ChildNodes[2]).ToString() + "]";
                }
                return id;
            }
            else if(equalnode(current, "CASTEO"))
            {
                return current.ChildNodes[1].Token.Text;
            }
            else if(equalnode(current, "INDICE"))
            {
                if (equaltemp(current.ChildNodes[0]))
                {
                    return obtenerTemp(current.ChildNodes[0]);
                }else if (current.ChildNodes[0].ToString().EndsWith("(NUMBER)"))
                {
                    return obtenerTemp(current.ChildNodes[0]);
                }
                else
                {
                    return current.ChildNodes[0].ChildNodes[0].Token.Text;
                }
            }
            else if(equalnode(current, "LLAMA"))
            {
                string id = analisisnodo(current.ChildNodes[0]).ToString();
                return new LLAMA(id,current.ChildNodes[1].Token.Location.Line,current.ChildNodes[1].Token.Location.Column);
            }
            else if (equalnode(current, "RET"))
            {
                return new RETURN();
            }
            else if (equalnode(current, "PRINT"))
            {
                string formato = current.ChildNodes[2].Token.Text.Replace("'", "");
                string cadena = "(int)";
                if (current.ChildNodes.Count == 8)
                    cadena += analisisnodo(current.ChildNodes[5]).ToString();
                else
                    cadena = analisisnodo(current.ChildNodes[4]).ToString();
                return new IMPRIMIR(cadena,formato,current.ChildNodes[0].Token.Location.Line,current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "IF"))
            {
                OperacionOP condi = (OperacionOP)analisisnodo(current.ChildNodes[2]);
                string labe1 = current.ChildNodes[5].Token.Text;
                return new IFop(condi,labe1,current.ChildNodes[0].Token.Location.Line,current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "GOTO"))
            {
                string labe1 = current.ChildNodes[1].Token.Text;
                return new GoTo(labe1, current.ChildNodes[0].Token.Location.Line, current.ChildNodes[0].Token.Location.Column);
            }
            else if (equalnode(current, "E"))
            {
                if (current.ChildNodes.Count == 3)
                {
                    return new OperacionOP(opera(current.ChildNodes[1].Token.Text),(OperacionOP)analisisnodo(current.ChildNodes[0]),(OperacionOP)analisisnodo(current.ChildNodes[2]),current.ChildNodes[1].Token.Location.Line,current.ChildNodes[1].Token.Location.Column);
                }
                else
                {
                    return analisisnodo(current.ChildNodes[0]);
                }
            }
            else if (equalnode(current, "PRIMITIVO"))
            {
                object valor;
                if (current.ChildNodes.Count == 2)
                {
                    try
                    {
                        int temp=int.Parse(current.ChildNodes[1].Token.Text);
                        temp = temp * -1;
                        valor = new Primitivo(temp, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                    }
                    catch
                    {
                        double temp2 = double.Parse(current.ChildNodes[1].Token.Text);
                        temp2 = temp2 * -1;
                        valor = new Primitivo(temp2, current.ChildNodes[1].Token.Location.Line, current.ChildNodes[1].Token.Location.Column);
                    }
                    
                }  
                else
                    valor=analisisnodo(current.ChildNodes[0]);

                OperacionOP ope = new OperacionOP();
                ope.Primitivo(valor);
                return ope;
            }
            else if (equalnode(current, "IDENTI"))
            {
                string iden=analisisnodo(current.ChildNodes[0]).ToString();
                OperacionOP ope2 = new OperacionOP();
                ope2.Identificador(iden,1,1);
                return ope2;
            }
            else if (equalnode(current, "PUNT"))
            {
                return current.ChildNodes[0].Token.Text;
            }
            else if (equaltemp(current))
            {
                return obtenerTemp(current);
            }
            else if (equaliteral(current))
            {
                return obtenerliteral(current);
            }
            else if (equalid(current))
            {
                return obtenerid(current);
            }
            return null;
        }

        private bool equalnode(ParseTreeNode node, string name)
        {
            return node.Term.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase);
        }
       

        private bool equaliteral(ParseTreeNode node)
        {
            if (node.ToString().EndsWith("(CADENA)") || node.ToString().EndsWith("(NUMBER)"))
                return true;
            return false;
        }

        private ExpresionOP obtenerliteral(ParseTreeNode node)
        {
            // case de opciones 
            if (node.ToString().EndsWith("(CADENA)"))
            {
                return new Primitivo(node.Token.Text.Replace("'", ""), node.Token.Location.Line, node.Token.Location.Column);
                //return node.Token.Text.Replace("'", "");
            }
            else if (node.ToString().EndsWith("(NUMBER)"))
            {
                try
                {
                    return new Primitivo(int.Parse(node.Token.Text), node.Token.Location.Line, node.Token.Location.Column);
                    //return int.Parse(node.Token.Text);
                }
                catch
                {
                    return new Primitivo(double.Parse(node.Token.Text), node.Token.Location.Line, node.Token.Location.Column);
                    //return double.Parse(node.Token.Text);
                }
            }
            return null;
        }


        private string obtenerid(ParseTreeNode node)
        {
            return node.Token.Text.ToString();
        }

        private bool equalid(ParseTreeNode node)
        {
            return node.ToString().EndsWith("(ID)");
        }
        private bool equaltemp(ParseTreeNode node)
        {
            return node.ToString().EndsWith("(temporal)");
        }
        private string obtenerTemp(ParseTreeNode node)
        {
            return node.Token.Text.ToString();
        }

        private TIPO_OPERACION opera(string op)
        {
            switch (op)
            {
                case "<":
                    return TIPO_OPERACION.MENOR_QUE;
                case "<=":
                    return TIPO_OPERACION.MENOR_IGUAL;
                case ">":
                    return TIPO_OPERACION.MAYOR_QUE;
                case ">=":
                    return TIPO_OPERACION.MAYOR_QUE;
                case "==":
                    return TIPO_OPERACION.IGUAL_IGUAL;
                case "!=":
                    return TIPO_OPERACION.DIFERENTE;
                case "+":
                    return TIPO_OPERACION.SUMA;
                case "-":
                    return TIPO_OPERACION.RESTA;
                case "*":
                    return TIPO_OPERACION.MULTI;
                case "/":
                    return TIPO_OPERACION.DIV;
                case "%":
                    return TIPO_OPERACION.MODULO;

            }
            return TIPO_OPERACION.ID;
        }
    }
}
