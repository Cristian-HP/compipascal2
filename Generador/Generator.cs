using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace compipascal2.Generador
{
    class Generator
    {
        private static Generator generator;
        private int temporal = 5;
        private int label;
        private LinkedList<string> code;
        private HashSet<string> tempStorage;
        string isFunc = "";
        private Generator()
        {
            this.temporal = 5;
            this.label = 0;
            this.code = new LinkedList<string>();
            this.tempStorage = new HashSet<string>();
        }

        public static Generator getInstance()
        {
            if (generator == null)
            {
                return generator = new Generator();
            }
            return generator;
        }

        public HashSet<string> getTempStorage()
        {
            return this.tempStorage;
        }
        public void clearTempStorage()
        {
            this.tempStorage.Clear();
        }
        public void setTempStorage(HashSet<string> tempStorage)
        {
            this.tempStorage = tempStorage;
        }
        public void clearCode()
        {
            this.temporal = 5;
            this.label = 0;
            this.code = new LinkedList<string>();
            this.tempStorage = new HashSet<string>();
        }
        public void addCode(string code)
        {
            this.code.AddLast(this.isFunc + code);
        }
        public string getCode()
        {
            StringBuilder concatenatedString = new StringBuilder();
            foreach (String myString in this.code)
            {
                concatenatedString.Append(myString + "\n");
            }
            return concatenatedString.ToString();
        }
        public string newTemporal()
        {
            string temp = "T" + this.temporal++;
            this.tempStorage.Add(temp);
            return temp;
        }
        public string newLabel()
        {
            return "L" + this.label++;
        }
        public void addLabel(string label)
        {
            this.code.AddLast(this.isFunc + label + ":");
        }
        public void addExpresion(string target, object izq, object der, string operador = "")
        {
            this.code.AddLast(this.isFunc + target + " = " + izq.ToString() + " " + operador + " " + der.ToString() + " ;");
        }
        public void addGoto(string label)
        {
            this.code.AddLast(this.isFunc + "goto " + label + ";");
        }
        public void addIF(object izq, object der, string operador, string label)
        {
            this.code.AddLast(this.isFunc + "if (" + izq.ToString() + " " + operador + " " + der.ToString() + " ) goto " + label + ";");
        }
        public void addSetHeap(object index,object valor)
        {
            this.code.AddLast(this.isFunc + "Heap[" + index.ToString() + "] = " + valor+";");
        }
        public void nextHeap()
        {
            this.code.AddLast(this.isFunc + "HP = HP + 1;");
        }
        public void freeTemp(string temp)
        {
            if (this.tempStorage.Contains(temp))
            {
                this.tempStorage.Remove(temp);
            }
        }
        public void addSetStack(object index,object valor)
        {
            this.code.AddLast(this.isFunc+"Stack["+index.ToString()+"] = " + valor.ToString()+";");
        }
        public void addGetStack(object target,object index)
        {
            this.code.AddLast(this.isFunc + target + " = Stack[" + index + "];");
        }
        public void addNextEnv(int size)
        {
            this.code.AddLast(this.isFunc+"SP = SP + "+size+";");
        }
        public void addAntEnv(int size)
        {
            this.code.AddLast(this.isFunc+"SP = SP - "+size+";");
        }
        public void addCall(string id)
        {
            this.code.AddLast(this.isFunc+id+";" );
        }
        public void addPrint(string expre,object valor)
        {
            this.code.AddLast(this.isFunc+"printf(\"%"+expre+"\","+valor.ToString()+");");
        }
        public string getEncabezado()
        {
            string head = "#include <stdio.h>\n";
            head += "float Heap[100000];\n";
            head += "float Stack[100000];\n";
            head += "int SP;\n";
            head += "int HP;\n";
            return head;
        }
        public string getTempstr()
        {
            string tempo = "float ";
            for(int i = 0; i < temporal; i++)
            {
                tempo += "T" + i+",";
            }
            tempo += "T" + temporal + ";\n";
            return tempo;
        }

        public void addPrintTrue()
        {
            this.addPrint("c", (int)'t' );
            this.addPrint("c", (int)'r' );
            this.addPrint("c", (int)'u');
            this.addPrint("c", (int)'e');
        }
        public void addPrintFalse()
        {
            this.addPrint("c", (int)'f');
            this.addPrint("c", (int)'a');
            this.addPrint("c", (int)'l');
            this.addPrint("c", (int)'s');
            this.addPrint("c", (int)'e');
        }

        public void addComment(string comment)
        {
            this.code.AddLast("/*** "+comment+"  **/");
        }
        public string nativas()
        {
            string nativa_print_str = "void native_print_str(){\n";
            nativa_print_str += "L0:\n\tT1=Heap[(int)T0];\n";
            nativa_print_str += "\tif(T1 == -1) goto L1;\n";
            nativa_print_str += "\tprintf(\"%c\",(int)T1);\n";
            nativa_print_str += "\tT0 = T0 + 1 ;\n\tgoto L0; \nL1: \n\treturn;\n }\n";

            string native_concat_str = "void native_concat_str(){\n";
            native_concat_str += "T3 = HP ;\nL0:\n";
            native_concat_str += "\tT4 = Heap[(int)T1];\n\tif(T4 == -1) goto L1;\n";
            native_concat_str += "\tHeap[HP] = T4;\n\tT1= T1 + 1;\n\tHP = HP +1;\n\tgoto L0;\n\n";
            native_concat_str += "L1:\n\tT4=Heap[(int)T2];\n\tif( T4 == -1) goto L2;\n";
            native_concat_str += "\tHeap[HP] = T4;\n\tT2 = T2 +1 ; \n\tHP = HP +1;\n\tgoto L1;\n";
            native_concat_str += "L2:\n\tHeap[HP]=-1;\n\tHP = HP + 1;\n\treturn;\n}\n";

            string native_less_str = "void native_less_str(){\n";
            native_less_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_less_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_less_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_less_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_less_str += "\tL3:\n\t\tif(T2 >= T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";

            string native_equal_str = "void native_equal_str(){\n";
            native_equal_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_equal_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_equal_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_equal_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_equal_str += "\tL3:\n\t\tif(T2 != T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";

            string native_less_equal_str = "void native_less_equal_str(){\n";
            native_less_equal_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_less_equal_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_less_equal_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_less_equal_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_less_equal_str += "\tL3:\n\t\tif(T2 > T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";


            string native_greater_str = "void native_greater_str(){\n";
            native_greater_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_greater_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_greater_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_greater_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_greater_str += "\tL3:\n\t\tif(T2 <= T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";

            string native_greater_equal_str = "void native_greater_equal_str(){\n";
            native_greater_equal_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_greater_equal_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_greater_equal_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_greater_equal_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_greater_equal_str += "\tL3:\n\t\tif(T2 < T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";

            string native_not_equal_str = "void native_not_equal_str(){\n";
            native_not_equal_str += "\tT4 = 0;\n\tT2 = Heap[(int)T0];\n\tT3 = Heap[(int)T1];\n";
            native_not_equal_str += "\tL0:\n\t\tif(T2==-1) goto L3;\n\t\tif(T3==-1) goto L3;\n";
            native_not_equal_str += "\tL1:\n\t\tif(T2==T3) goto L2;\n\t\tgoto L3;\n";
            native_not_equal_str += "\tL2:\n\t\tT0 = T0 + 1;\n\t\tT1 = T1 + 1;\n\t\tT2 = Heap[(int)T0];\n\t\tT3 = Heap[(int)T1];\n\t\tgoto L0;\n";
            native_not_equal_str += "\tL3:\n\t\tif(T2 == T3 ) goto L4;\n\t\tT4 = 1; // cumplio va ser true\n\tL4:\n\t\treturn;\n}\n";

            return nativa_print_str+native_concat_str+native_less_str+native_equal_str+native_less_equal_str+native_greater_str+native_greater_equal_str+native_not_equal_str;
        }
    }
}
