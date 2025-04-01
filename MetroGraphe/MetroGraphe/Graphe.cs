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
}
