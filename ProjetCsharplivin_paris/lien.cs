using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCsharplivin_paris
{
    public class Lien
    {
        public int Source { get; set; } ///représente le premier sommet de l'arête
        public int Destination { get; set; } /// représente le deuxième sommet de l'arête

        public Lien(int source, int destination)
        {
            Source = source;
            Destination = destination;
        }
    }

}