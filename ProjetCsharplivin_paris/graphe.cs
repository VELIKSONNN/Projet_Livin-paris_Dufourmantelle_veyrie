using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace PROJET_étudiant
{
     class graphe
    {
        private int NombreSommets; // Nombre de sommets du graphe
        private List<int>[] ListeAdjacence; // On utilise une liste d'adjacence pour stocker les voisins de chaque sommet 
        private int[,] MatriceAdjacence; // Ici la matrice d'adjacence signifie qu'il existera un lien entre i et j
        private List<Lien> Liens; //
        public int Nombresommets // On passe Le Nombresommets en publique afin qu'elle puisse être utilisée dans les autres classes
        {
            get { return NombreSommets; }
            set {  NombreSommets = value; }
        } 
        public List<int>[] Listeadjacence // On passe la liste d'adjacence en publique afin qu'elle puisse être utilisée dans les autres classes
        {
            get { return ListeAdjacence; }
            
        }

        public graphe(int n)
        {
            NombreSommets = n;
            ListeAdjacence = new List<int>[n + 1]; // On initialise la liste d'adjacence 
            MatriceAdjacence = new int[n + 1, n + 1]; // On initialise la matrice d'adjacence
            for (int i = 1; i <= n; i++) { ListeAdjacence[i] = new List<int>(); } // Chaque sommets va recevoir une liste vide 

            
        }

        public void AjouterLien(int u, int v) // On va ajouter un lien au graphe et pour cela la liste d'adjacence et la matrice d'adjacence vont être mis à jour par l'intermédiaire de u et v
        {
            ListeAdjacence[u].Add(v);
            ListeAdjacence[v].Add(u);
            MatriceAdjacence[u, v] = 1;
            MatriceAdjacence[v, u] = 1;
        }

        public void ParcoursLargeur(int depart) // On explore tout les voisins d'un sommet avant de passer aux suivants, cela permet de trouver le plus court chemin non pondéré
        {
            bool[] visite = new bool[NombreSommets + 1]; // On y marque les sommets visités
            Queue<int> file = new Queue<int>(); // On utilise une file pour explorer les sommets
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
