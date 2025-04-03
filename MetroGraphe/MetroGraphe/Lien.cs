using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroGraphe
{
    
        public class Lien<T>
        {
            public Noeud<T> Source { get; set; }
            public Noeud<T> Destination { get; set; }
            public double Distancesuivant { get; set; }
        public double Tempsdeplacement { get; set; }
        public double Tempsattente {  get; set; }


            public Lien(Noeud<T> source, Noeud<T> destination, double distance)
            {
                Source = source;
                Destination = destination;
                Distancesuivant = distance;
            }
        }


    
}

