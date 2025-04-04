using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livinparis_dufourmantelle_veyrie;
namespace livinparis_dufourmantelle_veyrie
{

    public class Lien<T>
        {
            public Noeud<T> Source { get; set; }
            public Noeud<T> Destination { get; set; }
            public double Distancesuivant { get; set; }
       


            public Lien(Noeud<T> source, Noeud<T> destination, double distance)
            {
                Source = source;
                Destination = destination;
                Distancesuivant = distance;
            }
        }


    
}

