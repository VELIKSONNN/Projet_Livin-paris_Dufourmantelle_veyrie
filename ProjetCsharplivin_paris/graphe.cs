using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace PROJET_étudiant
{
     class graphe
    {
        private int NombreSommets;
        private List<int>[] ListeAdjacence;
        private int[,] MatriceAdjacence;
        private List<Lien> Liens;
        public int Nombresommets
        {
            get { return NombreSommets; }
            set {  NombreSommets = value; }
        }
        public List<int>[] Listeadjacence
        {
            get { return ListeAdjacence; }
            
        }

        public graphe(int n)
        {
            NombreSommets = n;
            ListeAdjacence = new List<int>[n + 1];
            MatriceAdjacence = new int[n + 1, n + 1];
            for (int i = 1; i <= n; i++) { ListeAdjacence[i] = new List<int>(); }

            
        }

        public void AjouterLien(int u, int v)
        {
            ListeAdjacence[u].Add(v);
            ListeAdjacence[v].Add(u);
            MatriceAdjacence[u, v] = 1;
            MatriceAdjacence[v, u] = 1;
        }

        public void ParcoursLargeur(int depart)
        {
            bool[] visite = new bool[NombreSommets + 1];
            Queue<int> file = new Queue<int>();
            file.Enqueue(depart);
            visite[depart] = true;

            while (file.Count > 0)
            {
                int noeud = file.Dequeue();
                Console.Write(noeud + " ");
                foreach (var voisin in ListeAdjacence[noeud])
                {
                    if (!visite[voisin])
                    {
                        visite[voisin] = true;
                        file.Enqueue(voisin);
                    }
                }
            }
        }

        public void ParcoursProfondeur(int depart, bool[] visite)
        {
            visite[depart] = true;
            Console.Write(depart + " ");
            foreach (var voisin in ListeAdjacence[depart])
            {
                if (!visite[voisin])
                {
                    ParcoursProfondeur(voisin, visite);
                }
            }
        }
        public bool EstConnexe()
        {
            bool[] visite = new bool[NombreSommets + 1]; 
            Queue<int> file = new Queue<int>();

           
            int premierSommet = -1;
            for (int i = 1; i <= NombreSommets; i++)
            {
                if (ListeAdjacence[i].Count > 0)
                {
                    premierSommet = i;
                    break;
                }
            }

            if (premierSommet == -1) return false; 
            
            file.Enqueue(premierSommet);
            visite[premierSommet] = true;
            int nbVisites = 1;

            while (file.Count > 0)
            {
                int noeud = file.Dequeue();
                foreach (var voisin in ListeAdjacence[noeud])
                {
                    if (!visite[voisin])
                    {
                        visite[voisin] = true;
                        file.Enqueue(voisin);
                        nbVisites++;
                    }
                }
            }

           
            return nbVisites == NombreSommets;
        }



        public void ChargerDepuisFichier(string chemin)
        {
            string[] lignes = File.ReadAllLines(chemin);
            foreach (string ligne in lignes)
            {
                if (ligne.StartsWith("%")) continue;
                string[] parties = ligne.Split();
                if (parties.Length == 2)
                {
                    AjouterLien(int.Parse(parties[0]), int.Parse(parties[1]));
                }
            }
        }

       
    }
}
