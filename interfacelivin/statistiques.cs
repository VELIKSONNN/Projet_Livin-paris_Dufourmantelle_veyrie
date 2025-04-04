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
        public static void choisirstats(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("Que voulez vous faire ? -afficher une commande par période '1'\n-afficher la moyenne des prix des commandes '2' \n-afficher la moyenne de prix par client (compte dans le sujet) '3'" +
                "\n-afficher les commande par période et nationalité '4'\n-afficher le nombre de livraisons par cuisinier '5'\n-Afficher le montant des achats cumulé par clients '6' " +
                " ");
            char choixquery = Convert.ToChar(Console.ReadLine());
            switch (choixquery)
            {
                case '1':
                    AfficherCommandesParPeriode(conn);
                    break;
                case '2':
                    AfficherMoyennecommande(conn);
                    break;
                case '3':
                    AfficherMoyenneComptesClients(conn);
                    break;
                case '4':
                    AfficherCommandesClientParNationaliteEtPeriode(conn);
                    break;
                case '5':
                    AfficherNombreLivraisonsParCuisinier(conn);
                    break;
                case '6':
                    achatcumuléparclient(conn);
                    break;

            }
        }
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

        static void AfficherMoyennecommande(MySqlConnection connection)
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
        static void AfficherCommandesClientParNationaliteEtPeriode(MySqlConnection connection)
        {
            Console.WriteLine("Quel est l'id du client ?");
            int idClient = int.Parse(Console.ReadLine());
            Console.WriteLine("Quelle est la nationalité du plat en question");
            string nationalitePlat = Console.ReadLine();

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
                ORDER BY co.date_heure_commande";

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




        public static void AfficherNombreLivraisonsParCuisinier(MySqlConnection connection)
        {
            // Requête pour compter le nombre de lignes de commande (livraisons) par cuisinier
            string query = @"
            SELECT c.id_cuisinier,
                   COUNT(DISTINCT ldc.id_ligne_de_commande) AS nbLivraisons
            FROM cuisinier c
            JOIN commande co ON co.id_cuisinier = c.id_cuisinier
            JOIN ligne_de_commande_ ldc ON ldc.commande = co.commande
            GROUP BY c.id_cuisinier ";

            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine(" Nombre de livraisons par cuisinier ");
                while (reader.Read())
                {
                    int idCuisinier = reader.GetInt32("id_cuisinier");
                    int nbLivraisons = reader.GetInt32("nbLivraisons");

                    Console.WriteLine($"Cuisinier #{idCuisinier} => {nbLivraisons} livraison(s).");
                }
            }
        }

        public static void achatcumuléparclient(MySqlConnection connection)
        {
            string query = @"
                    SELECT cm.id_client, SUM(CAST(p.prix AS DECIMAL(10,2))) AS montant_total
                    FROM custommer cm
                    JOIN commande co ON cm.id_client = co.id_client
                    JOIN ligne_de_commande_ ldc ON co.commande = ldc.commande
                    JOIN inclue i ON ldc.id_ligne_de_commande = i.id_ligne_de_commande
                    JOIN plat p ON i.id = p.id
                    GROUP BY cm.id_client; ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Montant cumulé des achats par client :");
                while (reader.Read())
                {
                    int clientId = reader.GetInt32("id_client");
                    decimal montantTotal = reader.GetDecimal("montant_total");
                    Console.WriteLine($"Client {clientId} : {montantTotal:C}");
                }
            }
        }

    }
}

