using compipascal2.Analisis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compipascal2
{
    public partial class Form1 : Form
    {
        public static RichTextBox salida;
        Analizador analizador = new Analizador();
        public Form1()
        {
            InitializeComponent();
            salida = consola1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salida.Text = "";
            consola1.Text = "";
            string txt = richTextBox1.Text;
            analizador.analizar(txt);
        }
    }
}
