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

    public List<Noeud<T>> Djikstra(Noeud<T> Depart, Noeud<T> Arrivee)
    {
        var Distance = new double[ListeAdjacente.Length];  /// Distance minimale depuis le point de départ
        var precedents = new int[ListeAdjacente.Length];  ///  noeuds précédents avant l'arrivée afin de reconstruire le chemin
        var nonvisite = new List<int>();    ///station pas encore visitée
        for (int i = 0; i < ListeAdjacente.Length; i++)
        {
            Distance[i] = double.MaxValue;
            precedents[i] = -1;
            nonvisite.Add(i);
        }
        Distance[Convert.ToInt32(Depart.ID)] = 0;
        while (nonvisite.Count > 0)
        {
            int u = nonvisite.OrderBy(id => Distance[id]).First();  /// Choisir le sommet avec la plus petite distance
            nonvisite.Remove(u);                                      /// Marquer comme visité

            foreach (var lien in ListeAdjacente[u])
            {
                int voisin= Convert.ToInt32(lien.Destination.ID); /// identifiant du  sommet voisin
                double a = Distance[u] + lien.Distancesuivant;
                if (a < Distance[voisin])
                {
                    Distance[voisin] = a;
                    precedents[voisin] = u;
                }
                


            }


        }
        var chemin = new List<Noeud<T>>();
        int c = Convert.ToInt32(Arrivee.ID);
        while (c != -1)
        {
            var noeud= Noeuds.FirstOrDefault(n=>Convert.ToInt32(n.ID) == c);
            if (noeud != null)
            {
                chemin.Insert(0, noeud);
                c= precedents[c];
            }
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
