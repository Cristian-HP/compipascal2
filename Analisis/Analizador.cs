﻿using compipascal2.Abstract;
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
        LinkedList<Errorp> miserrores;
        public void analizar(string cadena)
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
                ejecucion(arbol, miserrores);
            }
        }

        private bool verificarerrores(ParseTree arbol, ParseTreeNode raiz)
        {
            if (raiz == null)
            {
                Errorp mi = new Errorp(0, 0, "ERROR Fatal No se Pudo Recuperar El analizador", "Analizador", "Gramatica");
                miserrores.AddLast(mi);
                //Form1.errorcon.AppendText(mi.ToString() + "\n");
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
                    Errorp mier = new Errorp(arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Column, arbol.ParserMessages[i].Message, tipoerro, "Gramatica");
                    miserrores.AddLast(mier);
                    //Form1.errorcon.AppendText(mier.ToString() + "\n");
                }
                return false;
            }
            else if (raiz != null)
            {
                return true;
            }
            return false;
        }

        public static void ejecucion(ParseTree tree,LinkedList<Errorp> elherror)
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
                    if(func is Funcion)
                    {
                        func.generar(ent);
                    }
                }
                foreach (Instruccion func in ast.instrucciones)
                {
                    if (func is Declaracion)
                    {
                        func.generar(ent);
                    }
                }
                declara = Generator.getInstance().getCode();
                Generator.getInstance().NewCode();
                foreach (Instruccion inst in ast.instrucciones)
                {
                    if(inst is Funcion)
                    {
                        inst.generar(ent);
                    }
                }
                funciones = Generator.getInstance().getCode();
                Generator.getInstance().NewCode();
                foreach (Instruccion inst in ast.instrucciones)
                {
                    if(!(inst is Funcion || inst is Declaracion))
                    {
                        inst.generar(ent);
                    }
                }
            }
            string encabezado = Generator.getInstance().getEncabezado();
            string nativas = Generator.getInstance().nativas();
            string tempo = Generator.getInstance().getTempstr();
            string C3D = Generator.getInstance().getCode();
            string strmain = "int main()\n{\n";
            Form1.salida.AppendText(encabezado);
            Form1.salida.AppendText(tempo);
            Form1.salida.AppendText(nativas);
            Form1.salida.AppendText(funciones);
            Form1.salida.AppendText(strmain);
            Form1.salida.AppendText(declara);
            Form1.salida.AppendText(C3D);
            Form1.salida.AppendText("return 0; \n }");
        }
    }
}
