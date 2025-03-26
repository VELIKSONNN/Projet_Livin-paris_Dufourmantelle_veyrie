using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Veyrie_Dufourmantelle
{

    public class Noeud
    {
        public int Id { get; set; }  // L'ID du sommet
        public List<Lien> Liens { get; set; }  // La liste des liens (arêtes) sortants de ce sommet

        public Noeud(int id)
        {
            Id = id;
            Liens = new List<Lien>();  // Initialisation de la liste des liens
        }

        // Ajoute un lien à ce sommet
        public void AjouterLien(Lien lien)
        {
            Liens.Add(lien);
        }
    }

}
