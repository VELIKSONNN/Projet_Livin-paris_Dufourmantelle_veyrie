using PROJET_étudiant;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Récupérer le chemin du projet (dossier parent du dossier bin/Debug/netX.X)
        string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Construire les chemins relatifs
        string cheminFichier = Path.Combine(projectDirectory, "Data", "soc-karate.mtx");
        string cheminImage = Path.Combine(projectDirectory, "graphe.png");

        // Vérifier si les fichiers existent (optionnel mais recommandé)
        if (!File.Exists(cheminFichier))
        {
            Console.WriteLine($"Erreur : Le fichier {cheminFichier} n'existe pas.");
            return;
        }

        graphe g = new graphe(34);
        g.ChargerDepuisFichier(cheminFichier);
        g.EstConnexe();
        
        VisualisationGraphe visualizer = new VisualisationGraphe(g);
        visualizer.DessinerEtAfficherGraphe(cheminImage);






    }
}


