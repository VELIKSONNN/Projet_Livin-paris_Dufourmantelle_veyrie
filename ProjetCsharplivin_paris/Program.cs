using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProjetCsharplivin_paris
{
    public class Program
    {
        static void Main()
        {
            
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

           
            string cheminFichier = Path.Combine(projectDirectory, "");
            string cheminImage = Path.Combine(projectDirectory, "graphe.png");

            
            if (!File.Exists(cheminFichier))
            {
                Console.WriteLine($"Erreur : Le fichier {cheminFichier} n'existe pas.");
                return;
            }

            graphe g = new graphe(25);
            Console.WriteLine("Parcours en largeur :");
            g.ParcoursLargeur(25);

            Console.WriteLine(" \n Parcours en profondeur:");
            bool[] visite = new bool[cheminFichier.Length];
            g.ParcoursProfondeur(1, visite);


            g.ChargerDepuisFichier(cheminFichier);
            VisualisationGraphe visualizer = new VisualisationGraphe(g);
            visualizer.DessinerEtAfficherGraphe(cheminImage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();



        }
    }


}