using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PSI_Veyrie_Dufourmantelle
{
    public class graphe
    {
        private int NombreSommets;

        private List<int>[] ListeAdjacence;
        private int[,] MatriceAdjacence;
        private List<Liens> lien;
        

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
        public List<Liens> Lien
        {
            get { return lien; }
            set { lien = value; }
        }
        public graphe(int nombreSommets)
        {
            NombreSommets = nombreSommets;
            
        }

        ///


        /// <summary>
        /// constructeur de la classe liens 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <summary>
        /// Constructeur de la classe graphe qui initialise la liste et la matrice d'adjacence.
        /// </summary>
        /// <param name="n">Nombre de sommets dans le graphe.</param>


        /// <summary>
        /// Ajoute un lien entre deux sommets du graphe.
        /// </summary>
        /// <param name="u">Premier sommet.</param>
        /// <param name="v">Deuxième sommet.</param>
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

        /// <summary>
        /// Effectue un parcours en largeur (BFS) à partir d'un sommet donné.
        /// </summary>
        /// <param name="depart">Sommet de départ du parcours.</param>
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

        /// <summary>
        /// Effectue un parcours en profondeur (DFS) à partir d'un sommet donné.
        /// </summary>
        /// <param name="depart">Sommet de départ.</param>
        /// <param name="visite">Tableau des sommets visités.</param>
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

        /// <summary>
        /// Vérifie si le graphe est connexe.
        /// </summary>
        /// <returns>Retourne true si le graphe est connexe, sinon false.</returns>
        public bool EstConnexe()
        {
            bool[] visite = new bool[NombreSommets + 1];
            ParcoursProfondeur(1, visite);

            foreach (bool estVisite in visite[1..])
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

        /// <summary>
        /// Rend le graphe connexe en ajoutant des arêtes entre les composantes connexes.
        /// </summary>
        public void RendreConnexe()
        {
            if (EstConnexe()) return; // Si le graphe est déjà connexe, on ne touche à rien

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

        /// <summary>
        /// Effectue un parcours en profondeur pour collecter les sommets d'une composante connexe.
        /// </summary>
        /// <param name="sommet">Sommet de départ.</param>
        /// <param name="visite">Tableau des sommets visités.</param>
        /// <param name="composante">Liste des sommets de la composante.</param>
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

        /// <summary>
        /// Vérifie si le graphe contient un cycle.
        /// </summary>
        /// <returns>Retourne true si un cycle est détecté, sinon false.</returns>
        public bool ContientCycle()
        {
            bool[] visite = new bool[NombreSommets + 1];
            return ContientCycleParcourslongueur(1, -1, visite);
        }

        /// <summary>
        /// Vérifie récursivement si un cycle est présent en utilisant un parcours en profondeur.
        /// </summary>
        /// <param name="n">Sommet actuel.</param>
        /// <param name="parent">Sommet parent.</param>
        /// <param name="visite">Tableau des sommets visités.</param>
        /// <returns>Retourne true si un cycle est détecté.</returns>
        private bool ContientCycleParcourslongueur(int n, int parent, bool[] visite)
        {
            visite[n] = true;

            foreach (var voisin in ListeAdjacence[n])
            {
                if (!visite[voisin])
                {
                    if (ContientCycleParcourslongueur(voisin, n, visite))
                        Console.WriteLine("Le parcours contient des cycles ");
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
        public List<int> Dijkstra(int depart, int arrivee)
        {
            // On initialise les distances à l'infini 
            int[] distance = new int[NombreSommets + 1];
            int[] precedent = new int[NombreSommets + 1];
            bool[] visite = new bool[NombreSommets + 1];
            for (int i = 0; i <= NombreSommets; i++)
            {
                distance[i] = int.MaxValue;
                precedent[i] = -1;


            }
            distance[depart] = 0;
            while (true)
            {
                int Noeudactuel = -1;
                int DistanceMin = int.MaxValue;


                for (int i = 0; i <= NombreSommets; i++)
                {
                    if (!visite[i] && distance[i] < DistanceMin)
                    {
                        Noeudactuel = i;
                        DistanceMin = distance[i];
                    }
                }
                // Si tous les sommets sont visités ou plus de nœuds accessibles
                if (Noeudactuel == -1) break;

                visite[Noeudactuel] = true;

                foreach (var voisin in ListeAdjacence[Noeudactuel])
                {
                    // Trouver le poids du lien entre NoeudActuel et voisin
                    int poidsLien = 1; // Valeur par défaut si le lien n'est pas trouvé

                    // Cherche le poids du lien dans la liste Liens

                    foreach (var lien in Liens)
                    {
                        if ((lien.Source == Noeudactuel && lien.Destination == voisin) ||
                    (lien.Source == voisin && lien.Destination == Noeudactuel))
                        {
                            poidsLien = lien.Poids;
                            break; // Si trouvé, on arrête la recherche
                        }
                    }
                    

                    // Vérification si le chemin est plus court
                    int nouvelleDistance = distance[Noeudactuel] + poidsLien;
                    if (nouvelleDistance < distance[voisin])
                    {
                        distance[voisin] = nouvelleDistance;
                        precedent[voisin] = Noeudactuel;
                    }
                }
            }

            // Reconstruction du chemin le plus court
            List<int> chemin = new List<int>();
            for (int at = arrivee; at != -1; at = precedent[at])
            {
                chemin.Add(at);
            }

            chemin.Reverse(); // On inverse le chemin pour avoir le bon ordre de départ à arrivée

            return chemin.Count > 1 ? chemin : new List<int>(); // Retourne un chemin valide, ou une liste vide si aucun chemin
        }
    
     }
    
}
