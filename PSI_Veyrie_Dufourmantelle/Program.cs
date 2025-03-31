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

            string connectionString = "Server=localhost;Database=baselinvinparis;User ID=root;Password=Sabrelaser00;SslMode=none;";
           
             try
            {
                // 1. Créer une connexion
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // 2. Ouvrir la connexion
                    connection.Open();
                    Console.WriteLine("Connexion à la base MySQL réussie.");

                    // 3. Exemple de requête SELECT
                    string query = "SELECT * FROM Restaurants";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // 4. Exécuter et récupérer les données
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Supposons qu’il y a une colonne "Name" dans la table Restaurants
                                string restaurantName = reader["Name"].ToString();
                                Console.WriteLine($"Restaurant: {restaurantName}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur lors de la connexion : " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur inattendue : " + ex.Message);
            }


        }
    }


}