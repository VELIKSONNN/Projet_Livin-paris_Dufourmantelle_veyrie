using System.Collections.Generic;

namespace MetroGraphe
{
    public class Noeud<T>
    {
        public T ID { get; set; }
        public string NOM { get; set; }
        public List<int> Lignes { get; private set; } = new List<int>();

        public Noeud(T id, string nom, int ligne)
        {
            ID = id;
            NOM = nom;
            AjouterLigne(ligne);
        }

        public void AjouterLigne(int ligne)
        {
            if (!Lignes.Contains(ligne))
            {
                Lignes.Add(ligne);
            }
        }
    }
}