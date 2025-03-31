using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroGraphe
{
  public class Lien<T>
    {


        public Noeud<T> Source { get; set; }
        public Noeud<T> Destination { get; set; }
     
        public double Distance { get; set; }

        public Lien(Noeud<T> source, Noeud<T> destination, double distance)
        {
            Source = source;
            Destination = destination;
           Distance = distance;
        }
        public static List<Lien<T>> ChargerLiensDepuisFichier(string fichierLiens, Dictionary<T, Noeud<T>> noeuds)
        {
            var liens = new List<Lien<T>>();

            foreach (var ligne in File.ReadAllLines(fichierLiens).Skip(1)) // Skip en-tête
            {
                var valeurs = ligne.Split(';');

                if (valeurs.Length < 3) continue;

                try
                {
                    T sourceId = (T)Convert.ChangeType(valeurs[0], typeof(T));
                    T destId = (T)Convert.ChangeType(valeurs[1], typeof(T));

                    if (!noeuds.ContainsKey(sourceId) || !noeuds.ContainsKey(destId))
                    {
                        Console.WriteLine($"Lien ignoré : nœud inexistant ({valeurs[0]} -> {valeurs[1]})");
                        continue;
                    }

                    if (!double.TryParse(valeurs[2], out double distance))
                    {
                        Console.WriteLine($"Erreur: Distance invalide ({valeurs[2]})");
                        continue;
                    }

                    liens.Add(new Lien<T>(noeuds[sourceId], noeuds[destId], distance));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la lecture du lien: {ex.Message}");
                }
            }
            return liens;
        }
    }
}

