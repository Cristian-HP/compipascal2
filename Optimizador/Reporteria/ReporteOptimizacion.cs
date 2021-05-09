using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace compipascal2.Optimizador.Reporteria
{
    class ReporteOptimizacion
    {
        private char[] dottex;

        public LinkedList<Optimizacion> Lista_optimizacion { get; set; }

        public ReporteOptimizacion()
        {
            Lista_optimizacion = new LinkedList<Optimizacion>();
        }

        public void Reporteht()
        {
            int cont = 1;
            string contenido = "<html>" + '\n' + "<head>" + '\n' + "<title>Reporte de Optimización</title>" + '\n' + "</head>" + '\n';
            contenido = contenido + "<body bgcolor=\"white\">" + '\n' + "<center><Font size=22 color=darkblue>" + "Reporte de Optimización CompiPascal" + "</Font></center>" + '\n';
            contenido = contenido + "<hr >" + '\n' + "<font color=white>" + '\n' + "<center>" + '\n';
            contenido = contenido + "<table border=1 align=center style=\"width:100%;\" >" + '\n';
            contenido = contenido + "<TR bgcolor=red>" + "\n";
            contenido = contenido + "<TH  style=\"font-size: 18px; width:10%; color:blue\" align=center>No.</TH>" + '\n';
            contenido = contenido + "<TH  style=\"font-size: 18px; width:25%; color:blue\" align=center>Tipo de Regla</TH>" + '\n';
            contenido = contenido + "<TH  style=\"font-size: 18px; width:15%; color:blue\" align=center>Regla</TH>" + '\n';
            contenido = contenido + "<TH  style=\"font-size: 18px; width:20%; color:blue\" align=center>Antes</TH>" + '\n';
            contenido = contenido + "<TH  style=\"font-size: 18px; width:20%; color:blue\" align=center>Despues</TH>" + '\n';
            contenido = contenido + "<TH  style=\"font-size: 18px; width:10%; color:blue\" align=center>Linea</TH>" + '\n';
            contenido = contenido + "</TR>" + '\n';
            foreach(Optimizacion opti in Lista_optimizacion)
            {
                contenido = contenido + "<TD style=\"font-size: 15px; color:red;\" align=center>" + cont + "</TD>" + '\n';
                contenido = contenido + "<TD style=\"font-size: 15px; color:forestgreen;\" color:white align=center>" + opti.tipo + "</TD>" + '\n';
                contenido = contenido + "<TD style=\"font-size: 15px; color:forestgreen;\" color:white align=center>" + opti.regla + "</TD>" + '\n';
                contenido = contenido + "<TD style=\"font-size: 15px; color:forestgreen;\" color:white align=center>" + opti.antes + "</TD>" + '\n';
                contenido = contenido + "<TD style=\"font-size: 15px; color:forestgreen;\" color:white align=center>" + opti.despues + "</TD>" + '\n';
                contenido = contenido + "<TD style=\"font-size: 15px; color:forestgreen;\" color:white align=center>" + opti.linea + "</TD>" + '\n';
                contenido = contenido + "</TR>" + '\n';
                cont++;
            }
            contenido = contenido + '\n' + "</center>" + '\n' + "</table>" + "</body>" + '\n' + "</html>";

            string path = "C:\\compiladores2\\ReporteOptimizacion.html";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(contenido);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
