using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_étudiant
{
    internal class Lien
    {
        public int Source { get; set; }
        public int Destination { get; set; }

        public Lien(int source, int destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
