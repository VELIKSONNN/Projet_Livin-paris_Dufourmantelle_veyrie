using PROJET_étudiant;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        /// Récupérer le chemin du projet (dossier parent du dossier bin/Debug/netX.X)
        string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// Construire les chemins relatifs
        string cheminFichier = Path.Combine(projectDirectory, "Data", "soc-karate.mtx");
        string cheminImage = Path.Combine(projectDirectory, "graphe.png");

        /// Vérifier si les fichiers existent
        if (!File.Exists(cheminFichier))
        {
            Console.WriteLine($"Erreur : Le fichier {cheminFichier} n'existe pas.");
            return;
        }

        graphe g = new graphe(34);
        Console.WriteLine("Parcours en largeur :");
        g.ParcoursLargeur(25);
        
        Console.WriteLine(" \n Parcours en profondeur:");
        bool[] visite = new bool[cheminFichier.Length];
        g.ParcoursProfondeur(1, visite);







        g. ChargerDepuisFichier(cheminFichier);
            VisualisationGraphe visualizer = new VisualisationGraphe(g);
            visualizer.DessinerEtAfficherGraphe(cheminImage);
        
        
        
       
       


        Console.WriteLine("Press any key to exit");
        Console.ReadKey();



    }
}


