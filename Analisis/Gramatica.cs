using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Analisis
{
    class Gramatica : Grammar
    {
        public Gramatica() :base(caseSensitive: false)
        {
            #region ER
            //primitivos
            NumberLiteral number = new NumberLiteral("NUMBER");
            StringLiteral cadena = new StringLiteral("CADENA", "'");
            RegexBasedTerminal bolean = new RegexBasedTerminal("BOOLEAN", "true|false");
            IdentifierTerminal id = new IdentifierTerminal("ID");
            //Comentarios
            CommentTerminal comentariolinea = new CommentTerminal("COMENTARIOLINEA", "//", "\n", "\r\n");
            CommentTerminal comentariomulti1 = new CommentTerminal("COMENTARIOMULTI1", "(*", "*)");
            CommentTerminal comentariomulti2 = new CommentTerminal("COMENTARIOMULTI2", "{", "}");
            #endregion

            #region TERMINALES
            //signos
            var coma = ToTerm(",");
            var parabre = ToTerm("(");
            var parcierra = ToTerm(")");
            var corabre = ToTerm("[");
            var corcierra = ToTerm("]");
            var ptocoma = ToTerm(";");
            var punto = ToTerm(".");
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
            var desigual = ToTerm("<>");
            var dospto = ToTerm(":");
            var asig = ToTerm(":=");

            //palabras reservadas
            var rdownto = ToTerm("downto");
            var rprogram = ToTerm("program");
            var rbreak = ToTerm("break");
            var rcotinue = ToTerm("continue");
            var rreal = ToTerm("real");
            var rwrite = ToTerm("write");
            var rstring = ToTerm("string");
            var rboolean = ToTerm("boolean");
            var writeln = ToTerm("writeln");
            var rand = ToTerm("and");
            var ror = ToTerm("or");
            var rnot = ToTerm("not");
            var rtype = ToTerm("type");
            var robject = ToTerm("object");
            var rarray = ToTerm("array");
            var rinteger = ToTerm("integer");
            var rif = ToTerm("if");
            var rthen = ToTerm("then");
            var relse = ToTerm("else");
            var rcase = ToTerm("case");
            var rof = ToTerm("of");
            var rvar = ToTerm("var");
            var rbegin = ToTerm("begin");
            var rend = ToTerm("end");
            var rwhile = ToTerm("while");
            var rrepeat = ToTerm("repeat");
            var runtil = ToTerm("until");
            var rfor = ToTerm("for");
            var rdo = ToTerm("do");
            var rto = ToTerm("to");
            var rprocedure = ToTerm("procedure");
            var rfuntion = ToTerm("function");
            var rexit = ToTerm("exit");
            var rconst = ToTerm("const");
            var rgraficar = ToTerm("graficar_ts");
            #endregion

            #region NOTERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal PROGRA = new NonTerminal("PROGRAM");
            NonTerminal MAIN = new NonTerminal("MAIN");
            NonTerminal LISTA_PARTES = new NonTerminal("LISTA_PARTES");
            NonTerminal PARTE = new NonTerminal("PARTE");
            NonTerminal CONSTATE_GLO = new NonTerminal("CONSTANTE_GLO");
            NonTerminal TYPES_GLO = new NonTerminal("TYPES_GLO");
            NonTerminal DECLARACIONES = new NonTerminal("DECLARACIONES");
            NonTerminal LISTA_FUNPRO = new NonTerminal("LISTA_FUNPRO");
            NonTerminal FUNPRO = new NonTerminal("FUNPRO");
            NonTerminal LISTA_CONSTANTE = new NonTerminal("LISTA_CONSTANTE");
            NonTerminal CONSTANTE = new NonTerminal("CONSTANTE");
            NonTerminal LISTA_TYPES = new NonTerminal("LISTA_TYPES");
            NonTerminal TYPES = new NonTerminal("TYPES");
            NonTerminal LISTA_VARIABLE = new NonTerminal("LISTA_VARIABLE");
            NonTerminal VARIABLE = new NonTerminal("VARIABLE");
            NonTerminal TIPOPRI = new NonTerminal("TIPO_PRIMITIVO");
            NonTerminal TIPOID = new NonTerminal("TIPO_CONID");
            NonTerminal TIPONOPRI = new NonTerminal("TIPO_NOPRI");
            NonTerminal OBJ = new NonTerminal("OBJETO");
            NonTerminal ARR = new NonTerminal("ARRAY");
            NonTerminal L_DEF = new NonTerminal("L_DEF");
            NonTerminal DEF = new NonTerminal("DEF");
            NonTerminal IT = new NonTerminal("IT");
            NonTerminal PRO = new NonTerminal("PROCEDIMIENTO");
            NonTerminal FUN = new NonTerminal("FUNCIONES");
            NonTerminal BEG = new NonTerminal("BEGIN");
            NonTerminal LISTA_INTS = new NonTerminal("LISTA_INTS");
            NonTerminal INST = new NonTerminal("INST");
            NonTerminal LISTA_ID = new NonTerminal("LISTA_ID");
            NonTerminal ELOG = new NonTerminal("ELOG");
            NonTerminal EARI = new NonTerminal("EARI");
            NonTerminal ERELA = new NonTerminal("ERELA");
            NonTerminal ASIGN = new NonTerminal("ASIGN");
            NonTerminal L_PARAM = new NonTerminal("L_PARAM");
            NonTerminal PARAM = new NonTerminal("PARAM");
            NonTerminal BODY = new NonTerminal("BODY");
            NonTerminal L_EXP = new NonTerminal("L_EXP");
            NonTerminal LLAMA = new NonTerminal("LLAMA");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal CASE = new NonTerminal("CASE");
            NonTerminal REP = new NonTerminal("REP");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal BRK = new NonTerminal("BKR");
            NonTerminal COT = new NonTerminal("COT");
            NonTerminal WRT = new NonTerminal("WRT");
            NonTerminal EXT = new NonTerminal("EXT");
            NonTerminal GTS = new NonTerminal("GTS");
            NonTerminal WL = new NonTerminal("WL");
            NonTerminal ELSE = new NonTerminal("ELSE");
            NonTerminal FORDI = new NonTerminal("FORD");
            NonTerminal L_OPC = new NonTerminal("L_OPC");
            NonTerminal OPC = new NonTerminal("OPC");
            NonTerminal EBC = new NonTerminal("EBC");
            NonTerminal L_EBC = new NonTerminal("L_EBC");
            #endregion

            #region GRAMATICA
            INICIO.Rule = PROGRA;

            PROGRA.Rule = rprogram + id + ptocoma + LISTA_PARTES + MAIN
                        | rprogram + id + ptocoma + MAIN
                        ;

            LISTA_PARTES.Rule = MakePlusRule(LISTA_PARTES, PARTE);

            PARTE.Rule = CONSTATE_GLO
                        | TYPES_GLO
                        | DECLARACIONES
                        | LISTA_FUNPRO
                        ;
            PARTE.ErrorRule = SyntaxError + ptocoma;

            LISTA_CONSTANTE.Rule = MakePlusRule(LISTA_CONSTANTE, CONSTANTE);

            LISTA_FUNPRO.Rule = MakePlusRule(LISTA_FUNPRO, FUNPRO);

            MAIN.Rule = rbegin + LISTA_INTS + rend + punto;

            CONSTATE_GLO.Rule = rconst + LISTA_CONSTANTE;

            TYPES_GLO.Rule = rtype + LISTA_TYPES;

            DECLARACIONES.Rule = rvar + LISTA_VARIABLE;

            LISTA_TYPES.Rule = MakePlusRule(LISTA_TYPES, TYPES);

            LISTA_VARIABLE.Rule = MakePlusRule(LISTA_VARIABLE, VARIABLE);

            CONSTANTE.Rule = id + igual + ELOG + ptocoma;

            TYPES.Rule = id + igual + TIPONOPRI + ptocoma;

            VARIABLE.Rule = LISTA_ID + dospto + TIPOID + igual + ELOG + ptocoma
                           | LISTA_ID + dospto + TIPOID + ptocoma
                           ;

            LISTA_ID.Rule = MakePlusRule(LISTA_ID, coma, id);

            TIPONOPRI.Rule = OBJ
                            | ARR
                            ;

            OBJ.Rule = robject + DECLARACIONES + rend;

            ARR.Rule = rarray + corabre + IT + corcierra + rof + TIPOID;

            L_DEF.Rule = MakePlusRule(L_DEF, DEF);

            ASIGN.Rule = id + asig + ELOG + ptocoma
                        | id + asig + ELOG
                        ;

            TIPOPRI.Rule = rinteger
                          | rstring
                          | rboolean
                          | rreal
                          ;
            TIPOID.Rule = TIPOPRI
                        | id
                        ;
            IT.Rule = rinteger
                    | rboolean
                    ;

            FUNPRO.Rule = PRO
                        | FUN
                        ;

            DEF.Rule = LISTA_FUNPRO
                    | DECLARACIONES
                    ;

            PRO.Rule = rprocedure + id + parabre + L_PARAM + parcierra + ptocoma + BODY
                    | rprocedure + id + parabre + parcierra + ptocoma + BODY
                    | rprocedure + id + ptocoma + BODY
                    ;

            BEG.Rule = rbegin + LISTA_INTS + rend + ptocoma;

            L_PARAM.Rule = MakePlusRule(L_PARAM, ptocoma, PARAM);

            PARAM.Rule = rvar + LISTA_ID + dospto + TIPOID
                        | LISTA_ID + dospto + TIPOID
                        ;

            FUN.Rule = rfuntion + id + parabre + L_PARAM + parcierra + dospto + TIPOID + ptocoma + BODY
                     | rfuntion + id + parabre + parcierra + dospto + TIPOID + ptocoma + BODY
                     | rfuntion + id + dospto + TIPOID + ptocoma + BODY
                     ;

            BODY.Rule = L_DEF + BEG
                      | BEG
                      ;

            L_EXP.Rule = MakePlusRule(L_EXP, coma, ELOG);

            LLAMA.Rule = id + parabre + L_EXP + parcierra + ptocoma
                        | id + parabre + parcierra + ptocoma
                        | id + parabre + L_EXP + parcierra
                        | id + parabre + parcierra
                        ;

            ELOG.Rule = ELOG + rand + ELOG
                      | ELOG + ror + ELOG
                      | rnot + ELOG
                      | parabre + ELOG + parcierra
                      | ERELA
                      ;
            ERELA.Rule = ERELA + mayor + ERELA
                        | ERELA + menor + ERELA
                        | ERELA + mayorigual + ERELA
                        | ERELA + menorigual + ERELA
                        | ERELA + igual + ERELA
                        | ERELA + desigual + ERELA
                        | parabre + ERELA + parcierra
                        | bolean
                        | EARI
                        ;
            EARI.Rule = EARI + mas + EARI
                      | EARI + menos + EARI
                      | EARI + multi + EARI
                      | EARI + div + EARI
                      | EARI + mod + EARI
                      | parabre + EARI + parcierra
                      | menos + EARI
                      | number
                      | cadena
                      | id
                      | LLAMA
                      ;

            INST.Rule = ASIGN
                      | IF
                      | CASE
                      | WL
                      | REP
                      | FOR
                      | BRK
                      | COT
                      | WRT
                      | EXT
                      | GTS
                      | LLAMA
                      ;
            INST.ErrorRule = SyntaxError + rend;

            COT.Rule = rcotinue + ptocoma;

            BRK.Rule = rbreak + ptocoma;

            IF.Rule = rif + ELOG + rthen + INST
                    | rif + ELOG + rthen + INST + ELSE
                    | rif + ELOG + rthen + BEG
                    | rif + ELOG + rthen + rbegin + LISTA_INTS + rend + ELSE
                    ;

            LISTA_INTS.Rule = MakePlusRule(LISTA_INTS, INST);

            ELSE.Rule = relse + INST
                      | relse + BEG
                      ;


            FORDI.Rule = rto
                       | rdownto
                       ;

            CASE.Rule = rcase + id + rof + L_OPC + rend + ptocoma
                      | rcase + id + rof + L_OPC + ELSE + rend + ptocoma
                      ;

            L_OPC.Rule = MakePlusRule(L_OPC, OPC);

            L_EBC.Rule = MakePlusRule(L_EBC, coma, EBC);

            OPC.Rule = L_EBC + dospto + BEG
                     | L_EBC + dospto + INST
                     ;

            EBC.Rule = number
                    | bolean
                    | cadena
                    | menos + number;
            ;

            REP.Rule = rrepeat + LISTA_INTS + runtil + ELOG + ptocoma
                    | rrepeat + BEG + runtil + ELOG + ptocoma
                    ;
            WL.Rule = rwhile + ELOG + rdo + INST
                    | rwhile + ELOG + rdo + BEG
                    ;


            FOR.Rule = rfor + id + asig + EARI + FORDI + EARI + rdo + INST
                     | rfor + id + asig + EARI + FORDI + EARI + rdo + BEG
                     ;

            WRT.Rule = writeln + parabre + L_EXP + parcierra + ptocoma
                     | rwrite + parabre + L_EXP + parcierra + ptocoma
                     | writeln + parabre + L_EXP + parcierra
                     | rwrite + parabre + L_EXP + parcierra
                     ;

            EXT.Rule = rexit + parabre + ELOG + parcierra + ptocoma
                     | rexit + parabre + ELOG + parcierra
                     ;

            GTS.Rule = rgraficar + parabre + parcierra + ptocoma
                     | rgraficar + parabre + parcierra
                     ;

            #endregion

            #region PREFECENCIAS
            NonGrammarTerminals.Add(comentariolinea);
            NonGrammarTerminals.Add(comentariomulti1);
            NonGrammarTerminals.Add(comentariomulti2);

            this.Root = INICIO;

            RegisterOperators(1, igual, desigual, mayor, menor, mayorigual, menorigual);
            RegisterOperators(2, mas, menos, ror);
            RegisterOperators(3, multi, div, mod, rand);
            RegisterOperators(4, Associativity.Right, rnot);

            #endregion

        }
    }
}
