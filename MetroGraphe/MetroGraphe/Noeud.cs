using MetroGraphe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroGraphe
{
    public class Noeud<T>
    {
        public T ID { get; set; }
        public string NOM { get; set; }
        public int numeroStation { get; set; }
    

        public Noeud(T id, string nom, int numLigne)
        {
            ID = id;
            NOM = nom;
            numeroStation = numLigne;
        }

    }
}
