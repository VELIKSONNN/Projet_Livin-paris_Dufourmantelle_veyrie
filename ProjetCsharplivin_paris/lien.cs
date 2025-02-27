using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_étudiant
{
    internal class lien
    {
        public noeud Noeud1 { get; }
        public noeud Noeud2 { get; }
        public lien(noeud n1, noeud n2)
        {
            Noeud1 = n1;
            Noeud2 = n2;
        }
    }
}
