using System;
using System.Collections.Generic;
using System.IO;

namespace PROJET_étudiant
{
    class graphe
    {
        private int NombreSommets;

        private List<int>[] ListeAdjacence;
        private int[,] MatriceAdjacence;

        public int Nombresommets
        {
            get { return NombreSommets; }
            set { NombreSommets = value; }
        }

        public List<int>[] Listeadjacence
        {
            get { return ListeAdjacence; }
            set { ListeAdjacence = value; }
        }

        public graphe(int n)
        {
            NombreSommets = n;
            ListeAdjacence = new List<int>[n + 1];
            MatriceAdjacence = new int[n + 1, n + 1];


            for (int i = 1; i <= n; i++)
            {
                ListeAdjacence[i] = new List<int>();
            }
        }

        public void AjouterLien(int u, int v)
        {
            if (!ListeAdjacence[u].Contains(v))
            {
                ListeAdjacence[u].Add(v);
                ListeAdjacence[v].Add(u);
                MatriceAdjacence[u, v] = 1;
                MatriceAdjacence[v, u] = 1;
            }
        }
        public void ParcoursLargeur(int depart)
        {
            bool[] visite = new bool[NombreSommets + 1];
            Queue<int> file = new Queue<int>();
            file.Enqueue(depart);
            visite[depart] = true;

            Console.WriteLine("Parcours en largeur");

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
            Console.WriteLine();
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
            ParcoursProfondeur(1, visite); /// On démarre à partir du sommet 1

            foreach (bool estVisite in visite[1..]) // On vérifie à partir du sommet à l'index 1 si chaque sommets est bien visité
            {
                if (!estVisite)
                {
                    Console.WriteLine("Le graphe n'est pas connexe.");
                    return false;
                }
            }
            Console.WriteLine("Le graphe est connexe.");
            return true;
        }

        public void RendreConnexe()
        {
            if (EstConnexe()) return; /// Si le graphe est déjà connexe on ne touche à rien

            bool[] visite = new bool[NombreSommets + 1];
            List<int> representants = new List<int>();

            for (int i = 1; i <= NombreSommets; i++)
            {
                if (!visite[i])
                {
                    List<int> composante = new List<int>();
                    ParcoursprofondeurCollect(i, visite, composante);
                    representants.Add(i);
                }
            }

            for (int i = 1; i < representants.Count; i++)
            {
                AjouterLien(representants[i - 1], representants[i]);
            }

            Console.WriteLine("Graphe rendu connexe.");
        }

        private void ParcoursprofondeurCollect(int sommet, bool[] visite, List<int> composante)
        {
            Stack<int> pile = new Stack<int>();
            pile.Push(sommet);

            while (pile.Count > 0)
            {
                int current = pile.Pop();
                if (!visite[current])
                {
                    visite[current] = true;
                    composante.Add(current);

                    foreach (int voisin in ListeAdjacence[current])
                    {
                        if (!visite[voisin])
                            pile.Push(voisin);
                    }
                }
            }
        }
        public bool ContientCycle()
        {
            bool[] visite = new bool[NombreSommets + 1];
            return ContientCycleParcourslongueur(1, -1, visite);
        }
        private bool ContientCycleParcourslongueur(int n, int parent, bool[] visite)
        {
            visite[n] = true;

            foreach (var voisin in ListeAdjacence[n])
            {
                if (!visite[voisin])
                {
                    if (ContientCycleParcourslongueur(voisin, n, visite))
                        Console.WriteLine("le parcours contient des cycles ");
                        return true;
                }
                else if (voisin != parent)
                {
                  
                    return true;
                }

            }
            return false;
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
            RendreConnexe(); // On s'assure que le graphe est connexe après le chargement
            ContientCycle();
        }
    }
}

