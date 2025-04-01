using MySql.Data.MySqlClient;
using interfacelivin;
namespace interfacelivin

{
    internal class Program
    {
        static void Main(string[] args)
        {
            
                string connectionString = "server=127.0.0.1;user=root;password=Sabrelaser00;database=baselivinparis";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("✅ Connexion réussie !");

                        string query = "SELECT id, Prenom, email FROM utilisateur ";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("📋 Résultats :");
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["id"]}, Prenom: {reader["Prenom"]}, Email: {reader["email"]}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Erreur : " + ex.Message);
                    }
                }

                Console.WriteLine("Fin du programme.");

            }
        }
    }
