using compipascal2.Abstract;
using compipascal2.Arbol;
using compipascal2.Generador;
using compipascal2.Instrucciones.Funciones;
using compipascal2.Instrucciones.Variables;
using compipascal2.SymbolTable;
using compipascal2.Utils;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Analisis
{
    class Analizador
    {
        ParseTreeNode raiz;
        public LinkedList<Errorp> miserrores;
        public string analizar(string cadena)
        {
            miserrores = new LinkedList<Errorp>();
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            raiz = arbol.Root;

            bool verifica = verificarerrores(arbol, raiz);
            if (verifica)
            {
               return ejecucion(arbol, miserrores);
            }
            return "";
        }

        private bool verificarerrores(ParseTree arbol, ParseTreeNode raiz)
        {
            if (raiz == null)
            {
                Errorp mi = new Errorp(0, 0, "Analizador", "ERROR Fatal No se Pudo Recuperar El analizador", "Gramatica");
                miserrores.AddLast(mi);
                string tipoerro;
                for (int i = 0; i < arbol.ParserMessages.Count; i++)
                {
                    if (arbol.ParserMessages[i].Message.Contains("Syntax"))
                        tipoerro = "Sintactico";
                    else
                        tipoerro = "Lexico";
                    Errorp mier = new Errorp(arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Column, tipoerro, arbol.ParserMessages[i].Message, "Gramatica");
                    miserrores.AddLast(mier);
                    Form1.salida.AppendText(mier.ToString() + "\n");
                }
                Form1.salida.AppendText(mi.ToString() + "\n");
                return false;
            }
            else if (arbol.ParserMessages.Count > 0)
            {
                string tipoerro;
                for (int i = 0; i < arbol.ParserMessages.Count; i++)
                {
                    if (arbol.ParserMessages[i].Message.Contains("Syntax"))
                        tipoerro = "Sintactico";
                    else
                        tipoerro = "Lexico";
                    Errorp mier = new Errorp(arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Column, tipoerro, arbol.ParserMessages[i].Message,  "Gramatica");
                    miserrores.AddLast(mier);
                    Form1.salida.AppendText(mier.ToString() + "\n");
                }
                return false;
            }
            else if (raiz != null)
            {
                return true;
            }
            return false;
        }

        public static string ejecucion(ParseTree tree,LinkedList<Errorp> errorps)
        {
            Creadorast arbolgenerado = new Creadorast(tree);
            AST ast = arbolgenerado.mytree;
            Entorno ent = new Entorno(null,"GLOBAL");
            string funciones = "";
            string declara = "";
            if (ast != null)
            {
                Generator.getInstance().clearCode();
                foreach(Instruccion func in ast.instrucciones)
                {
                    if(func is Funcion || func is StructFst)
                    {
                        func.generar(ent,errorps);
                    }
                }
                foreach (Instruccion func in ast.instrucciones)
                {
                    if (func is Declaracion)
                    {
                        func.generar(ent,errorps);
                    }
                }
                declara = Generator.getInstance().getCode();
                Generator.getInstance().NewCode();
                foreach (Instruccion inst in ast.instrucciones)
                {
                    if(inst is Funcion)
                    {
                        inst.generar(ent,errorps);
                    }
                }
                funciones = Generator.getInstance().getCode();
                Generator.getInstance().NewCode();
                foreach (Instruccion inst in ast.instrucciones)
                {
                    if(!(inst is Funcion || inst is Declaracion || inst is StructFst))
                    {
                        inst.generar(ent,errorps);
                    }
                }
            }
            string encabezado = Generator.getInstance().getEncabezado();
            string nativas = Generator.getInstance().nativas();
            string tempo = Generator.getInstance().getTempstr();
            string C3D = Generator.getInstance().getCode();
            string strmain = "void main()\n{\n";
            string totalD3D = "";
            totalD3D += encabezado + tempo + nativas + funciones + strmain + declara + C3D + "return;  \n}";
            return totalD3D;
           /* Form1.salida.AppendText(encabezado);
            Form1.salida.AppendText(tempo);
            Form1.salida.AppendText(nativas);
            Form1.salida.AppendText(funciones);
            Form1.salida.AppendText(strmain);
            Form1.salida.AppendText(declara);
            Form1.salida.AppendText(C3D);
            Form1.salida.AppendText("return; \n }");*/
        }
    }
}
