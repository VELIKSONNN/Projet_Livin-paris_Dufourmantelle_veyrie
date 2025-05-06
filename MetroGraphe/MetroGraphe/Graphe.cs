using System;
using System.Collections.Generic;
using System.Linq;

namespace livinparis_dufourmantelle_veyrie
{
    /// <summary>
    /// Représente un graphe orienté pondéré à l’aide d’une liste d’adjacence.
    /// </summary>
    public class Graphe<T>
    {
        /// <summary>
        /// Liste de tous les nœuds du graphe (stations).
        /// </summary>
        public List<Noeud<T>> Noeuds { get; set; }

        /// <summary>
        /// Liste de tous les liens (connexions entre stations).
        /// </summary>
        public List<Lien<T>> Liens { get; set; }

        /// <summary>
        /// Représentation du graphe en liste d’adjacence.
        /// Chaque indice correspond à un identifiant de nœud.
        /// </summary>
        public List<Lien<T>>[] ListeAdjacente;

        /// <summary>
        /// Constructeur du graphe à partir d’une liste de nœuds et de liens.
        /// </summary>
        public Graphe(List<Noeud<T>> noeuds, List<Lien<T>> liens)
        {
            Noeuds = noeuds;
            Liens = liens;

            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

            for (int i = 0; i < ListeAdjacente.Length; i++)
            {
                ListeAdjacente[i] = new List<Lien<T>>();
            }

            foreach (var lien in Liens)
            {
                int index = Convert.ToInt32(lien.Source.ID);
                ListeAdjacente[index].Add(lien);
            }
        }

        /// <summary>
        /// Ajoute un lien au graphe et met à jour la liste d’adjacence.
        /// </summary>
        
        public void AjouterLien(Lien<T> lien)
        {
            Liens.Add(lien);
            int index = Convert.ToInt32(lien.Source.ID);
            ListeAdjacente[index].Add(lien);
        }

        /// <summary>
        /// Calcule le plus court chemin entre deux nœuds avec l’algorithme de Dijkstra.
        /// </summary>
        /// <param name="depart">Station de départ</param>
        /// <param name="arrivee">Station d’arrivée</param>
        /// <returns>Liste des stations constituant le plus court chemin</returns>
        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
            int n = ListeAdjacente.Length;
            double[] distance = new double[n];
            int[] predecesseur = new int[n];
            bool[] visite = new bool[n];

            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
                predecesseur[i] = -1;
                visite[i] = false;
            }

            int departId = Convert.ToInt32(depart.ID);
            distance[departId] = 0;

            for (int count = 0; count < n - 1; count++)
            {
                double min = double.MaxValue;
                int u = -1;

                for (int i = 0; i < n; i++)
                {
                    if (!visite[i] && distance[i] < min)
                    {
                        min = distance[i];
                        u = i;
                    }
                }

                if (u == -1) break;
                visite[u] = true;

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
        /// Calcule le plus court chemin entre deux nœuds avec l’algorithme de Bellman-Ford.
        /// </summary>
        /// <param name="depart">Station de départ</param>
        /// <param name="arrivee">Station d’arrivée</param>
        /// <returns>Liste des stations constituant le plus court chemin</returns>
        /// <exception cref="InvalidOperationException">Si un cycle de poids négatif est détecté</exception>
        public List<Noeud<T>> BellmanFord(Noeud<T> depart, Noeud<T> arrivee)
        {
            int n = ListeAdjacente.Length;
            double[] distance = new double[n];
            int[] predecesseur = new int[n];

            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
                predecesseur[i] = -1;
            }

            distance[Convert.ToInt32(depart.ID)] = 0;

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

        /// <summary>
        /// Colorie les nœuds du graphe selon l'algorithme de Welsh–Powell.
        /// Retourne un dictionnaire mappant chaque nœud à un indice de couleur (0,1,2…).
        /// </summary>
        public Dictionary<Noeud<T>, int> ColorationWelshPowell()
        {
            var ordre = Noeuds
                .OrderByDescending(n => ListeAdjacente[Convert.ToInt32(n.ID)].Count)
                .ToList();

            var couleurNoeud = new Dictionary<Noeud<T>, int>();
            int couleurCourante = 0;

            while (ordre.Any())
            {
                var coloCetteCouleur = new List<Noeud<T>>();

                foreach (var noeud in ordre.ToList())
                {
                    bool conflit = ListeAdjacente[Convert.ToInt32(noeud.ID)]
                        .Select(l => l.Destination)
                        .Any(v => coloCetteCouleur.Contains(v));

                    if (!conflit)
                    {
                        couleurNoeud[noeud] = couleurCourante;
                        coloCetteCouleur.Add(noeud);
                        ordre.Remove(noeud);
                    }
                }

                couleurCourante++;
            }

            return couleurNoeud;
        }
    }
}
