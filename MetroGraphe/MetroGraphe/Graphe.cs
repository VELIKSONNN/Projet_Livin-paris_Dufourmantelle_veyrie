
using System;
using System.Collections.Generic;
using System.Linq;

namespace livinparis_dufourmantelle_veyrie
{
    public class Graphe<T>
    {
        public List<Noeud<T>> Noeuds { get; set; }
        public List<Lien<T>> Liens { get; set; }
        public List<Lien<T>>[] ListeAdjacente;

        public Graphe(List<Noeud<T>> noeuds, List<Lien<T>> liens)
        {
            Noeuds = noeuds;
            Liens = liens;

            // Calcul de la taille maximale pour la liste d’adjacence
            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

            // Initialisation de chaque sous-liste
            for (int i = 0; i < ListeAdjacente.Length; i++)
            {
                ListeAdjacente[i] = new List<Lien<T>>();
            }

            // Peuplement de la structure d’adjacence
            foreach (var lien in Liens)
            {
                int index = Convert.ToInt32(lien.Source.ID);
                ListeAdjacente[index].Add(lien);
            }
        }

        public void AjouterLien(Lien<T> lien)
        {
            Liens.Add(lien);
            int index = Convert.ToInt32(lien.Source.ID);
            ListeAdjacente[index].Add(lien);
        }

        /// <summary>
        /// Algorithme de Dijkstra (version array-based)
        /// </summary>
        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
            int n = ListeAdjacente.Length;
            double[] distance = new double[n];
            int[] predecesseur = new int[n];
            bool[] visite = new bool[n];

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
                predecesseur[i] = -1;
                visite[i] = false;
            }

            int departId = Convert.ToInt32(depart.ID);
            distance[departId] = 0;

            // Boucle principale
            for (int count = 0; count < n - 1; count++)
            {
                double min = double.MaxValue;
                int u = -1;

                // Trouver le sommet non visité avec la plus petite distance
                for (int i = 0; i < n; i++)
                {
                    if (!visite[i] && distance[i] < min)
                    {
                        min = distance[i];
                        u = i;
                    }
                }

                if (u == -1) break; // plus de sommet accessible
                visite[u] = true;

                // Mise à jour des distances pour les voisins de u
                foreach (var lien in ListeAdjacente[u])
                {
                    int v = Convert.ToInt32(lien.Destination.ID);
                    double poids = lien.Distancesuivant;

                    if (!visite[v] && distance[u] + poids < distance[v])
                    {
                        distance[v] = distance[u] + poids;
                        predecesseur[v] = u;
                    }
                }
            }

            // Reconstruction du chemin
            var chemin = new List<Noeud<T>>();
            int actuel = Convert.ToInt32(arrivee.ID);

            while (actuel != -1)
            {
                var noeud = Noeuds.FirstOrDefault(nd => Convert.ToInt32(nd.ID) == actuel);
                if (noeud != null)
                {
                    chemin.Insert(0, noeud);
                }
                actuel = predecesseur[actuel];
            }

            return chemin;
        }

        /// <summary>
        /// Algorithme de Bellman-Ford
        /// </summary>
        public List<Noeud<T>> BellmanFord(Noeud<T> depart, Noeud<T> arrivee)
        {
            int n = ListeAdjacente.Length;
            double[] distance = new double[n];
            int[] predecesseur = new int[n];

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
                predecesseur[i] = -1;
            }

            distance[Convert.ToInt32(depart.ID)] = 0;

            // Relaxation des arêtes n-1 fois
            for (int k = 0; k < n - 1; k++)
            {
                for (int u = 0; u < n; u++)
                {
                    foreach (var lien in ListeAdjacente[u])
                    {
                        int v = Convert.ToInt32(lien.Destination.ID);
                        double poids = lien.Distancesuivant;

                        if (distance[u] != double.MaxValue && distance[u] + poids < distance[v])
                        {
                            distance[v] = distance[u] + poids;
                            predecesseur[v] = u;
                        }
                    }
                }
            }

            // Détection de cycle de poids négatif
            for (int u = 0; u < n; u++)
            {
                foreach (var lien in ListeAdjacente[u])
                {
                    int v = Convert.ToInt32(lien.Destination.ID);
                    double poids = lien.Distancesuivant;

                    if (distance[u] != double.MaxValue && distance[u] + poids < distance[v])
                    {
                        throw new InvalidOperationException("Le graphe contient un cycle de poids négatif.");
                    }
                }
            }

            // Construction du chemin
            var resultat = new List<Noeud<T>>();
            int current = Convert.ToInt32(arrivee.ID);
            while (current != -1)
            {
                var noeud = Noeuds.FirstOrDefault(n => Convert.ToInt32(n.ID) == current);
                if (noeud != null)
                {
                    resultat.Insert(0, noeud);
                }
                current = predecesseur[current];
            }

            return resultat;
        }
    }
}
