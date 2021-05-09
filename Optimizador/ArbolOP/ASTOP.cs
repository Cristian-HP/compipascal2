using compipascal2.Optimizador.AbstracOP;
using System;
using System.Collections.Generic;
using System.Text;

namespace compipascal2.Optimizador.ArbolOP
{
    class ASTOP
    {
        public LinkedList<Etiqueta> etiquetas { get; set; }
        public LinkedList<string> Betadas { get; set; }

        public ASTOP(LinkedList<Etiqueta> intrucciones)
        {
            this.etiquetas = intrucciones;
            this.Betadas = new LinkedList<string>();
        }

        public void AddEtiqueta(Etiqueta nueva)
        {
            this.etiquetas.AddLast(nueva);
        }

        public bool ExiteEtiqueta(string id)
        {
            foreach (Etiqueta aux in this.etiquetas)
            {
                if (aux.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public Etiqueta getEtiqueta(string id)
        {
            foreach(Etiqueta aux in this.etiquetas)
            {
                if (aux.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    return aux;
            }
            return null;
        }

        public Etiqueta NextEtiqueta(string id)
        {
            bool sigue = false;
            foreach (Etiqueta aux in this.etiquetas)
            {
                if (sigue) 
                    return aux;
                if (aux.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    sigue = true;
            }
            return null;
        }
    }
}
