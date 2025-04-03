using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Veyrie_Dufourmantelle { 
 public class Liens
{
    public Noeud Source { get; set; } // Noeud de départ
    public Noeud Destination { get; set; } // Noeud d'arrivée
    public int Poids { get; set; } // Poids de l'arête

    public Liens(Noeud source, Noeud destination, int poids = 1)
    {
        Source = source;
        Destination = destination;
        Poids = poids;
    }
}
}


