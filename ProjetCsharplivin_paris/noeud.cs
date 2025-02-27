using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_étudiant
{
    internal class noeud
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public noeud(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

    }
}
