using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using interfacelivin;
using MySql.Data.MySqlClient;
namespace interfacelivin
{
    public class statistiques
    {

        static void AfficherCommandesParPeriode(MySqlConnection connection)
        {

            Console.WriteLine("Quelle est la date de début ? année,n°mois,n°jour");
            DateTime dateDebut = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Quelle est la date de fin ?");
             DateTime dateFin = Convert.ToDateTime(Console.ReadLine());
            string query = @"
        SELECT commande, date_heure_commande, id_client, id_cuisinier
        FROM commande
        WHERE date_heure_commande BETWEEN @debut AND @fin
        ORDER BY date_heure_commande
    ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@debut", dateDebut);
                cmd.Parameters.AddWithValue("@fin", dateFin);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int noCommande = reader.GetInt32("commande");
                        DateTime dateHeure = reader.GetDateTime("date_heure_commande");
                        int idClient = reader.GetInt32("id_client");
                        int idCuisinier = reader.GetInt32("id_cuisinier");

                        Console.WriteLine($"Commande {noCommande} du {dateHeure} (Client {idClient}, Cuisinier {idCuisinier})");
                    }
                }
            }
        }

        static void AfficherMoyennePrixDesCommandes(MySqlConnection connection)
        {
            string query = @"
        SELECT AVG(total_par_commande) AS moyennePrix
        FROM (
            SELECT co.commande,
                   SUM(p.prix) AS total_par_commande
            FROM commande co
            JOIN ligne_de_commande_ ldc ON co.commande = ldc.commande
            JOIN inclue i ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN plat p ON p.id = i.id
            GROUP BY co.commande
        ) AS sous_requete
    ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    double moyenne = Convert.ToDouble(result);
                    Console.WriteLine($"Moyenne des prix des commandes : {moyenne:F2}");
                }
                else
                {
                    Console.WriteLine("Aucune commande trouvée.");
                }
            }
        }

        static void AfficherMoyenneComptesClients(MySqlConnection connection)
        {
            string query = @"
        SELECT AVG(total_client) AS moyenneComptesClients
        FROM (
            SELECT c.id_client,
                   SUM(p.prix) AS total_client
            FROM commande co
            JOIN ligne_de_commande_ ldc ON co.commande = ldc.commande
            JOIN inclue i ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN plat p ON p.id = i.id
            JOIN custommer c ON co.id_client = c.id_client
            GROUP BY c.id_client
        ) sous_requete
    ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    double moyenne = Convert.ToDouble(result);
                    Console.WriteLine($"Moyenne des comptes clients (montant dépensé) : {moyenne:F2}");
                }
                else
                {
                    Console.WriteLine("Aucune commande trouvée.");
                }
            }
        }
        static void AfficherCommandesClientParNationaliteEtPeriode(MySqlConnection connection,int idClient)
        {
            Console.WriteLine("Quelle est la nationalité du plat en question");
            string nationalitePlat=Console.ReadLine();

            Console.WriteLine("Quelle est la date de début ? année,n°mois,n°jour");
            DateTime debut = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Quelle est la date de fin ?");
            DateTime fin = Convert.ToDateTime(Console.ReadLine());

            string query = @"
        SELECT co.commande,
               co.date_heure_commande,
               p.nom AS nomPlat,
               pa.nom AS nationalitePlat
        FROM commande co
        JOIN ligne_de_commande_ ldc ON co.commande = ldc.commande
        JOIN inclue i ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
        JOIN plat p ON p.id = i.id
        JOIN pays pa ON pa.idpays = p.idpays
        WHERE co.id_client = @idClient
          AND pa.nom = @paysNom
          AND co.date_heure_commande BETWEEN @debut AND @fin
        ORDER BY co.date_heure_commande
    ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@idClient", idClient);
                cmd.Parameters.AddWithValue("@paysNom", nationalitePlat);
                cmd.Parameters.AddWithValue("@debut", debut);
                cmd.Parameters.AddWithValue("@fin", fin);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int commandeId = reader.GetInt32("commande");
                        DateTime dateCde = reader.GetDateTime("date_heure_commande");
                        string platNom = reader.GetString("nomPlat");
                        string pays = reader.GetString("nationalitePlat");

                        Console.WriteLine($"Commande {commandeId}, le {dateCde}, Plat: {platNom} (Pays: {pays})");
                    }
                }
            }
        }

        

    public static class StatsManager
        {
        public static void AfficherNombreLivraisonsParCuisinier(MySqlConnection connection)
            {
            // Requête pour compter le nombre de lignes de commande (livraisons) par cuisinier
            string query = @"
            SELECT c.id_cuisinier,
                   COUNT(DISTINCT ldc.id_ligne_de_commande) AS nbLivraisons
            FROM cuisinier c
            JOIN commande co ON co.id_cuisinier = c.id_cuisinier
            JOIN ligne_de_commande_ ldc ON ldc.commande = co.commande
            GROUP BY c.id_cuisinier
        ";

            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("------ Nombre de livraisons par cuisinier ------");
                while (reader.Read())
                {
                    int idCuisinier = reader.GetInt32("id_cuisinier");
                    int nbLivraisons = reader.GetInt32("nbLivraisons");

                    Console.WriteLine($"Cuisinier #{idCuisinier} => {nbLivraisons} livraison(s).");
                }
            }
        }
    }

    }
}
