using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.Analisis
{
    class GramaticaOP : Grammar
    {
        public GramaticaOP() : base(caseSensitive: false)
        {
            #region ER
            NumberLiteral number = new NumberLiteral("NUMBER");
            StringLiteral cadena = new StringLiteral("CADENA", "\"");
            RegexBasedTerminal tempo = new RegexBasedTerminal("temporal", "T[0-9]+");
            tempo.Priority = TerminalPriority.High;
            RegexBasedTerminal label = new RegexBasedTerminal("label", "L[0-9]+");
            label.Priority = TerminalPriority.High;
            IdentifierTerminal id = new IdentifierTerminal("ID");
            id.Priority = TerminalPriority.Low;
            //Comentarios
            CommentTerminal comentariolinea = new CommentTerminal("COMENTARIOLINEA", "//", "\n", "\r\n");
            CommentTerminal comentariomulti1 = new CommentTerminal("COMENTARIOMULTI1", "/*", "*/");
            #endregion

            #region TERMINALES
            //signos
            var coma = ToTerm(",");
            var parabre = ToTerm("(");
            var parcierra = ToTerm(")");
            var corabre = ToTerm("[");
            var corcierra = ToTerm("]");
            var llaveabre = ToTerm("{");
            var llavecierra = ToTerm("}");
            var ptocoma = ToTerm(";");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var menorigual = ToTerm("<=");
            var mayorigual = ToTerm(">=");
            var igual = ToTerm("=");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var multi = ToTerm("*");
            var div = ToTerm("/");
            var mod = ToTerm("%");
            var desigual = ToTerm("!=");
            var dospto = ToTerm(":");
            var dobleigual = ToTerm("==");
            var hastag = ToTerm("#");

            //palabras reservadas
            var rinclude = ToTerm("include");
            var rlibreria = ToTerm("stdio.h");
            var rfloat = ToTerm("float");
            var rint = ToTerm("int");
            var rvoid = ToTerm("void");
            var rheap = ToTerm("Heap");
            var rstack = ToTerm("Stack");
            var rsp = ToTerm("SP");
            var rhp = ToTerm("HP");
            var rif = ToTerm("if");
            var rgoto = ToTerm("goto");
            var rprint = ToTerm("printf");
            var rreturn = ToTerm("return");

            #endregion

            #region NOTERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal PROGRA = new NonTerminal("PROGRAM");
            NonTerminal ENCABEZADO = new NonTerminal("ENCABEZADO");
            NonTerminal FUNCIONES = new NonTerminal("FUNCIONES");
            NonTerminal L_INST = new NonTerminal("L_INST");
            NonTerminal INST = new NonTerminal("INST");
            NonTerminal INCLU = new NonTerminal("INCLU");
            NonTerminal ESTRUCTURA = new NonTerminal("ESTRUCT");
            NonTerminal PUNTERO = new NonTerminal("PUNTERO");
            NonTerminal PUNT = new NonTerminal("PUNT");
            NonTerminal DECLATEMP = new NonTerminal("DECLATEMP");
            NonTerminal L_DECTEMP = new NonTerminal("L_DECTEMP");
            NonTerminal ASIG = new NonTerminal("ASIG");
            NonTerminal GOTO = new NonTerminal("GOTO");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal ACCESO = new NonTerminal("ACCESO");
            NonTerminal CASTEO = new NonTerminal("CASTEO");
            NonTerminal LLAMA = new NonTerminal("LLAMA");
            NonTerminal E = new NonTerminal("E");
            NonTerminal RET = new NonTerminal("RET");
            NonTerminal PRINT = new NonTerminal("PRINT");
            NonTerminal L_FUN = new NonTerminal("L_FUN");
            NonTerminal L_ETIQ = new NonTerminal("L_ETIQ");
            NonTerminal ETIQ = new NonTerminal("ETIQ");
            NonTerminal INDICE = new NonTerminal("INDICE");
            NonTerminal PRIMITIVO = new NonTerminal("PRIMITIVO");
            NonTerminal IDENTI = new NonTerminal("IDENTI");

            #endregion

            #region GRAMATICA
            INICIO.Rule = PROGRA;
            PROGRA.Rule = ENCABEZADO+L_FUN;
            ENCABEZADO.Rule = INCLU + ESTRUCTURA + ESTRUCTURA + PUNTERO + PUNTERO + DECLATEMP;
            INCLU.Rule = hastag + rinclude + menor + rlibreria + mayor;
            ESTRUCTURA.Rule = rfloat + rheap + corabre + number + corcierra + ptocoma
                            | rfloat + rstack + corabre + number + corcierra + ptocoma
                            ;
            PUNTERO.Rule = rint + PUNT + ptocoma;
            PUNT.Rule = rsp
                      | rhp
                      ;
            DECLATEMP.Rule = rfloat + L_DECTEMP + ptocoma;
            L_DECTEMP.Rule = MakePlusRule(L_DECTEMP, coma, tempo);

            L_FUN.Rule = MakePlusRule(L_FUN, FUNCIONES);

            FUNCIONES.Rule = rvoid + id + parabre + parcierra + llaveabre + L_INST + L_ETIQ + llavecierra
                           | rvoid + id + parabre + parcierra + llaveabre + L_ETIQ + llavecierra
                           | rvoid + id + parabre + parcierra + llaveabre + L_INST + llavecierra
                           ;

            L_INST.Rule = MakePlusRule(L_INST, INST);

            L_ETIQ.Rule = MakePlusRule(L_ETIQ, ETIQ);

            ETIQ.Rule = label + dospto + L_INST
                      | label + dospto
                      ;

            INST.Rule = ASIG
                      | IF
                      | GOTO
                      | RET
                      | LLAMA
                      | PRINT
                      ;
            ASIG.Rule = tempo + igual + E + ptocoma
                      | ACCESO + igual + E + ptocoma
                      | PUNT + igual + E + ptocoma
                      ;
            //LABEL.Rule = label + dospto;

            IF.Rule = rif + parabre + E + parcierra + rgoto + label + ptocoma;
            GOTO.Rule = rgoto + label + ptocoma;
            RET.Rule = rreturn + ptocoma;
            LLAMA.Rule = id + parabre + parcierra + ptocoma;

            ACCESO.Rule = rheap + corabre + CASTEO + INDICE + corcierra
                        | rheap + corabre + INDICE + corcierra
                        | rstack + corabre + CASTEO + INDICE + corcierra
                        | rstack + corabre + INDICE + corcierra
                        ;
            CASTEO.Rule = parabre + rint + parcierra;

            PRINT.Rule = rprint + parabre + cadena + coma + CASTEO + INDICE + parcierra + ptocoma
                       | rprint + parabre + cadena + coma + INDICE + parcierra + ptocoma
                       ;

            INDICE.Rule = tempo
                        | number
                        | PUNT
                        ;

            PRIMITIVO.Rule = number
                           | menos + number
                           ;
            IDENTI.Rule = ACCESO
                        | tempo
                        | PUNT
                        ;

            E.Rule = E + mas + E
                   | E + menos + E
                   | E + div + E
                   | E + multi + E
                   | E + mod + E
                   | E + menor + E
                   | E + menorigual + E
                   | E + mayor + E
                   | E + mayorigual + E
                   | E + dobleigual + E //operacion
                   | E + desigual + E
                   | PRIMITIVO
                   | IDENTI
                   ;
            #endregion

            #region PREFECENCIAS
            NonGrammarTerminals.Add(comentariolinea);
            NonGrammarTerminals.Add(comentariomulti1);

            this.Root = INICIO;

            RegisterOperators(1, dobleigual, desigual, mayor, menor, mayorigual, menorigual);
            RegisterOperators(2, mas, menos);
            RegisterOperators(3, multi, div, mod);
            #endregion
        }
    }
}
