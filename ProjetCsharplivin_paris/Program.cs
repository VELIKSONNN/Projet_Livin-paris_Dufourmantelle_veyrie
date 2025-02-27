using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string cheminFichier = "C:\\Users\\joach\\Documents\\ESILV\\GITHUB\\Projet_Livin-paris\\ProjetCsharplivin_paris\\soc-karate.mtx";
        string cheminImage = "C:\\Users\\joach\\Documents\\ESILV\\GITHUB\\Projet_Livin-paris\\graphe.png";

      
        List<Tuple<int, int>> liens = ChargerLiens(cheminFichier);
        if (liens.Count == 0)
        {
            Console.WriteLine("❌ Aucun lien trouvé, arrêt du programme.");
            return;
        }

        
        GrapheVisualizer visualizer = new GrapheVisualizer(liens);
        visualizer.DessinerEtAfficherGraphe(cheminImage);
    }

    static List<Tuple<int, int>> ChargerLiens(string cheminFichier)
    {
        List<Tuple<int, int>> liens = new List<Tuple<int, int>>();

        if (!File.Exists(cheminFichier))
        {
            Console.WriteLine("❌ ERREUR : fichier introuvable !");
            return liens;
        }

        foreach (string ligne in File.ReadLines(cheminFichier))
        {
            if (ligne.StartsWith("%")) continue; 

            string[] valeurs = ligne.Split();
            if (valeurs.Length < 2) continue;

            int a = int.Parse(valeurs[0]);
            int b = int.Parse(valeurs[1]);

            liens.Add(Tuple.Create(a, b));
        }

        return liens;
    }
}


