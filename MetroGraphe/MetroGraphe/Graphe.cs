<<<<<<< Updated upstream
/*<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿
using System;
using System.Collections.Generic;
using System.Linq;

=======
﻿using livinparis_dufourmantelle_veyrie;
>>>>>>> Stashed changes
=======
﻿using livinparis_dufourmantelle_veyrie;
>>>>>>> Stashed changes
=======
﻿using livinparis_dufourmantelle_veyrie;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // Calcul de la taille maximale pour la liste d’adjacence
            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

            // Initialisation de chaque sous-liste
=======
            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

>>>>>>> Stashed changes
=======
            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

>>>>>>> Stashed changes
=======
            int maxId = Convert.ToInt32(Noeuds.Max(n => Convert.ToInt32(n.ID))) + 1;
            ListeAdjacente = new List<Lien<T>>[maxId];

>>>>>>> Stashed changes
            for (int i = 0; i < ListeAdjacente.Length; i++)
            {
                ListeAdjacente[i] = new List<Lien<T>>();
            }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // Peuplement de la structure d’adjacence
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======


        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
=======


        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
>>>>>>> Stashed changes
=======


        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
>>>>>>> Stashed changes
            var distances = new Dictionary<T, double>();
            var precedent = new Dictionary<T, Noeud<T>>();
            var nonVisites = new List<Noeud<T>>(Noeuds);

            foreach (var noeud in Noeuds)
                distances[noeud.ID] = double.MaxValue;

            distances[depart.ID] = 0;

            while (nonVisites.Count > 0)
            {
                // Sélection du nœud avec la plus petite distance
                var noeudActuel = nonVisites.OrderBy(n => distances[n.ID]).First();
                nonVisites.Remove(noeudActuel);
                while (arrivee != null)
                {
                    if (noeudActuel.ID.Equals(arrivee.ID))
                        break;
                }

                foreach (var lien in ListeAdjacente[Convert.ToInt32(noeudActuel.ID)])
                {
                    var voisin = lien.Destination;
                    double tentative = distances[noeudActuel.ID] + lien.Distancesuivant;

                    if (tentative < distances[voisin.ID])
                    {
                        distances[voisin.ID] = tentative;
                        precedent[voisin.ID] = noeudActuel;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
                    }
                }
            }

            // Reconstruction du chemin
            var chemin = new List<Noeud<T>>();
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
            var courant = arrivee;

            while (courant != null && precedent.ContainsKey(courant.ID))
            {
                chemin.Insert(0, courant);
                courant = precedent[courant.ID];
            }

            if (courant != null && courant.ID.Equals(depart.ID))
                chemin.Insert(0, depart);

            return chemin;
        }

>>>>>>> Stashed changes
=======
            var courant = arrivee;

            while (courant != null && precedent.ContainsKey(courant.ID))
            {
                chemin.Insert(0, courant);
                courant = precedent[courant.ID];
            }

            if (courant != null && courant.ID.Equals(depart.ID))
                chemin.Insert(0, depart);

            return chemin;
        }

>>>>>>> Stashed changes
=======
            var courant = arrivee;

            while (courant != null && precedent.ContainsKey(courant.ID))
            {
                chemin.Insert(0, courant);
                courant = precedent[courant.ID];
            }

            if (courant != null && courant.ID.Equals(depart.ID))
                chemin.Insert(0, depart);

            return chemin;
        }

>>>>>>> Stashed changes
        public List<Noeud<T>> BellmanFord(Noeud<T> depart, Noeud<T> arrivee)
        {
            int n = ListeAdjacente.Length;
            double[] distance = new double[n];
            int[] predecesseur = new int[n];

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // Initialisation
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
                predecesseur[i] = -1;
            }

            distance[Convert.ToInt32(depart.ID)] = 0;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // Relaxation des arêtes n-1 fois
=======
            // Relaxation des arêtes n - 1 fois
>>>>>>> Stashed changes
=======
            // Relaxation des arêtes n - 1 fois
>>>>>>> Stashed changes
=======
            // Relaxation des arêtes n - 1 fois
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                    }
                }
            }

            // Construction du chemin
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
            List<Noeud<T>> chemin = new List<Noeud<T>>();
            int current = Convert.ToInt32(arrivee.ID);
            while (current != -1)
            {
                chemin.Insert(0, Noeuds.First(n => Convert.ToInt32(n.ID).Equals(current)));
                current = predecesseur[current];
            }

            return chemin;
        }

    }
}
>>>>>>> Stashed changes
=======
            List<Noeud<T>> chemin = new List<Noeud<T>>();
            int current = Convert.ToInt32(arrivee.ID);
            while (current != -1)
            {
                chemin.Insert(0, Noeuds.First(n => Convert.ToInt32(n.ID).Equals(current)));
                current = predecesseur[current];
            }

            return chemin;
        }

    }
}
>>>>>>> Stashed changes
            */

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
=======
                    }
                }
            }

            // Construction du chemin
            List<Noeud<T>> chemin = new List<Noeud<T>>();
            int current = Convert.ToInt32(arrivee.ID);
            while (current != -1)
            {
                chemin.Insert(0, Noeuds.First(n => Convert.ToInt32(n.ID).Equals(current)));
                current = predecesseur[current];
            }

            return chemin;
        }

    }
}
>>>>>>> Stashed changes
