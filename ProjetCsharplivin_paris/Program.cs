using PROJET_étudiant;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string cheminFichier = "C:\\Users\\joach\\Documents\\ESILV\\GITHUB\\Projet_Livin-paris\\ProjetCsharplivin_paris\\soc-karate.mtx";
        string cheminImage = "C:\\Users\\joach\\Documents\\ESILV\\GITHUB\\Projet_Livin-paris\\graphe.png";
        graphe g = new graphe(34);
        g.ChargerDepuisFichier(cheminFichier);
        g.EstConnexe();
        
        VisualisationGraphe visualizer = new VisualisationGraphe(g);
        visualizer.DessinerEtAfficherGraphe(cheminImage);






    }
}


