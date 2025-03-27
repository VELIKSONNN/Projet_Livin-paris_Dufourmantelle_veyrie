using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Veyrie_Dufourmantelle
{
    public class Noeud
    {
        public int Id { get; set; } // Identifiant du noeud
        public string Nom { get; set; } // Nom ou label du noeud (optionnel)

        public Noeud(int id, string nom = "")
        {
            Id = id;
            Nom = nom;
        }
    }
}

