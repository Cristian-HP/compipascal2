using compipascal2.Analisis;
using compipascal2.Optimizador.Analisis;
using compipascal2.Optimizador.Reporteria;
using compipascal2.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compipascal2
{
    public partial class Form1 : Form
    {
        public static RichTextBox salida;
        public static LinkedList<TablaReport> Tablasim;
        Analizador analizador = new Analizador();
        AnalizadorOP analisisOP = new AnalizadorOP();
        ReporteOptimizacion reporte;
        private int caracter2;
        private int caracter;
        string resultado;
        public Form1()
        {
            InitializeComponent();
            salida = consola1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salida.Text = "";
            richTextBox2.Text = "";
            Tablasim = new LinkedList<TablaReport>();
            string txt = richTextBox1.Text;
            richTextBox2.Text= analizador.analizar(txt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = richTextBox2.Text;
            (resultado,reporte)= analisisOP.analizar(txt);
            richTextBox2.Text = resultado;
            //richTextBox2.Text = resultado;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            caracter = 0;
            int altura = richTextBox2.GetPositionFromCharIndex(0).Y;
            if (richTextBox2.Lines.Length > 0)
            {
                for (int i = 0; i < richTextBox2.Lines.Length; i++)
                {
                    e.Graphics.DrawString((i + 1).ToString(), richTextBox2.Font, Brushes.Blue, pictureBox1.Width - (e.Graphics.MeasureString((i + 1).ToString(), richTextBox2.Font).Width + 10), altura);
                    caracter += richTextBox2.Lines[i].Length + 1;
                    altura = richTextBox2.GetPositionFromCharIndex(caracter).Y;
                }
            }
            else
            {
                e.Graphics.DrawString((1).ToString(), richTextBox2.Font, Brushes.Blue, pictureBox2.Width - (e.Graphics.MeasureString((1).ToString(), richTextBox2.Font).Width + 10), altura);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //timer1.Interval = 10;
            //timer1.Start();
            timer2.Interval = 10;
            timer2.Start();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            caracter2 = 0;
            int altura = richTextBox1.GetPositionFromCharIndex(0).Y;
            if (richTextBox1.Lines.Length > 0)
            {
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    e.Graphics.DrawString((i + 1).ToString(), richTextBox1.Font, Brushes.Blue, pictureBox1.Width - (e.Graphics.MeasureString((i + 1).ToString(), richTextBox1.Font).Width + 10), altura);
                    caracter2 += richTextBox1.Lines[i].Length + 1;
                    altura = richTextBox1.GetPositionFromCharIndex(caracter2).Y;
                }
            }
            else
            {
                e.Graphics.DrawString((1).ToString(), richTextBox1.Font, Brushes.Blue, pictureBox1.Width - (e.Graphics.MeasureString((1).ToString(), richTextBox1.Font).Width + 10), altura);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
        }


        private void graficarTabla()
        {
            string dottex = "digraph { \n  tbl [ \n    shape = plaintext \n    label =< \n ";
            dottex += "<table border ='0' cellborder ='1' color ='RED' cellspacing ='1' >";
            dottex += "<tr><td> Nombre </td><td> Tipo </td><td> Ambito </td><td> Ambiente </td><td> Numero Parametros </td><td> Fila </td><td> Columna </td></tr> ";
            for (int i = 0; i < Tablasim.Count; i++)
            {
                dottex += "<tr> \n ";
                dottex += "<td color ='blue' >" + Tablasim.ElementAt(i).nombre + "</td><td color ='blue'>" + Tablasim.ElementAt(i).tipo + "</td><td color ='blue'>" + Tablasim.ElementAt(i).Ambito;
                dottex += "</td><td color ='blue'>" + Tablasim.ElementAt(i).Ambiente + "</td><td color ='blue'>" + Tablasim.ElementAt(i).Numero + "</td><td color ='blue'>" + Tablasim.ElementAt(i).Linea + "</td><td color ='blue'>" + Tablasim.ElementAt(i).Columna + "</td>\n</tr>\n";
            }


            dottex += " </table> \n>]; \n}";
            string path = "C:\\compiladores2\\ReporteTablaSim.dot";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(dottex);
                    fs.Write(info, 0, info.Length);
                }
                Thread.Sleep(2000);
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "dot.exe",
                    Arguments = "-Tpng C:\\compiladores2\\ReporteTablaSim.dot -o C:\\compiladores2\\ReporteTablaSim.png",
                    UseShellExecute = false
                };
                Process.Start(startInfo);

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LinkedList<Errorp> loserrores = analizador.miserrores;
            if (Tablasim != null)
                graficarTabla();
            if(reporte!= null)
                reporte.Reporteht();
            if (loserrores != null)
                graficar(loserrores);
            
        }

        private void graficar(LinkedList<Errorp> errores)
        {
            string errorestext = "<html>\n <body> <h2>Errores proyecto 1</h2> <table style=\"width:100%\" border=\"1\"> <tr> <th>Tipo</th><th>Ambito</th> <th>Descripcion del error</th><th>Linea</th> <th>Columna</th></tr> \n";
            foreach (Errorp elerror in errores)
            {
                errorestext += "<tr>" +
                   "<td>" + elerror.tipoe+
                   "</td>" +
                   "<td>" + elerror.ambito +
                   "</td>" +
                   "<td>" + elerror.descripcion +
                   "</td>" +
                   "<td>" + elerror.Linea +
                   "</td>" +
                   "<td>" + elerror.Columna +
                   "</td>" +
                   "</tr>";
            }
            errorestext += "</table> </body> </html>";
            using (StreamWriter outputFile = new StreamWriter("C:\\compiladores2\\reporteErrores.html"))
            {
                outputFile.WriteLine(errorestext);
            }
        }
    }
}
