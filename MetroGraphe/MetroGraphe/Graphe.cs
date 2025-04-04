using MetroGraphe;

public class Graphe<T>
{
    public List<Noeud<T>> Noeuds { get; set; }
    public List<Lien<T>> Liens { get; set; }
    public List<Lien<T>>[] ListeAdjacente;

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

    public void AjouterLien(Lien<T> lien)
    {
        Liens.Add(lien);
        int index = Convert.ToInt32(lien.Source.ID);
        ListeAdjacente[index].Add(lien);
    }



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
            /// Trouver le sommet non visité avec la plus petite distance
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

            if (u == -1) break; /// Aucun sommet accessible

            visite[u] = true;

            /// Mise à jour des distances pour les voisins de u
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

        /// Construction du chemin
        List<Noeud<T>> chemin = new List<Noeud<T>>();
        int courant = Convert.ToInt32(arrivee.ID);

        while (courant != -1)
        {
            var noeud = Noeuds.FirstOrDefault(n => Convert.ToInt32(n.ID).Equals(courant));
            if (noeud != null)
                chemin.Insert(0, noeud);

            courant = predecesseur[courant];
        }

        return chemin;
    }


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
        

        // Relaxation des arêtes n - 1 fois
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
