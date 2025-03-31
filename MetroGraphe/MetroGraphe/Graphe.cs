using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroGraphe
{
    public class Graphe<T>

    {
        public Dictionary<T, Noeud<T>> Noeud { get; set; }
        public List<Lien<T>> Lien { get; set; }
        public Dictionary<T, Dictionary<T, double>> ListeAdjacence { get; private set; }
        public double[,] MatriceAdjacence { get; private set; }

        public Graphe()
        {
            Noeud = new Dictionary<T, Noeud<T>>();
            Lien = new List<Lien<T>>();
            ListeAdjacence = new Dictionary<T, Dictionary<T, double>>();
        }

        public void ChargerDepuisFichiers(string fichierNoeuds, string fichierLiens)
        {
            Noeud = Noeud<T>.ChargerNoeudsDepuisFichier(fichierNoeuds);
            Lien = Lien<T>.ChargerLiensDepuisFichier(fichierLiens, Noeud);
        }

        private void ConstruireStructuresAdjacence()
        {
            int n = Noeud.Count;
            MatriceAdjacence = new double[n, n];

            // Initialiser la matrice avec une valeur infinie (sauf pour les diagonales)
            foreach (var i in Noeud.Keys)
            {
                foreach (var j in Noeud.Keys)
                {
                    MatriceAdjacence[Convert.ToInt32(i), Convert.ToInt32(j)] =
                        i.Equals(j) ? 0 : double.PositiveInfinity;
                }
            }



            // Construire la liste d'adjacence
            foreach (var lien in Lien)
            {
                T sourceId = lien.Source.ID;
                T destId = lien.Destination.ID;
                double distance = lien.Tempsdistance;

                // Ajouter dans la matrice
                MatriceAdjacence[Convert.ToInt32(sourceId), Convert.ToInt32(destId)] = distance;
                MatriceAdjacence[Convert.ToInt32(destId), Convert.ToInt32(sourceId)] = distance;

                // Ajouter dans la liste d'adjacence
                if (!ListeAdjacence.ContainsKey(sourceId))
                    ListeAdjacence[sourceId] = new Dictionary<T, double>();

                if (!ListeAdjacence.ContainsKey(destId))
                    ListeAdjacence[destId] = new Dictionary<T, double>();

                ListeAdjacence[sourceId][destId] = distance;
                ListeAdjacence[destId][sourceId] = distance; // Graphe non orienté
            }
        }
        public void AfficherMatriceAdjacence()
        {
            Console.WriteLine("\nMatrice d'Adjacence:");
            foreach (var i in Noeud.Keys)
            {
                foreach (var j in Noeud.Keys)
                {
                    double val = MatriceAdjacence[Convert.ToInt32(i), Convert.ToInt32(j)];
                    Console.Write(val == double.PositiveInfinity ? "∞ " : $"{val} ");
                }
                Console.WriteLine();
            }
        }

        public void AfficherListeAdjacence()
        {
            Console.WriteLine("\nListe d'Adjacence:");
            foreach (var station in ListeAdjacence)
            {
                Console.Write($"{station.Key} -> ");
                foreach (var voisin in station.Value)
                {
                    Console.Write($"({voisin.Key}, {voisin.Value} km) ");
                }
                Console.WriteLine();
            }
        }

    }
}
