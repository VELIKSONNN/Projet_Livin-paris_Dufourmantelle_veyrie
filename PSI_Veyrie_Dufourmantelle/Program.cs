using PSI_Veyrie_Dufourmantelle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;

using System.Threading.Tasks;
namespace PSI_Veyrie_Dufourmantelle
{
    public class Program
    {
        static void Main()
        {
            
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string choix = Console.ReadLine();
            string cheminFichier = Path.Combine(projectDirectory, "soc-karate.mtx");
            string cheminImage = Path.Combine(projectDirectory, "graphe.png");


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


            g.ChargerDepuisFichier(cheminFichier);
            AfficheGraphe visualizer = new AfficheGraphe(g);
            visualizer.DessinerEtAfficherGraphe(cheminImage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            


        }
      
    }


}