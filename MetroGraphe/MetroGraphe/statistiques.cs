using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livinparis_dufourmantelle_veyrie;
using MySql.Data.MySqlClient;
namespace livinparis_dufourmantelle_veyrie
{
    public class statistiques
    {
        public static void choisirstats(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("Que voulez vous faire ? -afficher une commande par période '1'\n-afficher la moyenne des prix des commandes '2' \n-afficher la moyenne de prix par client (compte dans le sujet) '3'" +
                "\n-afficher les commande par période et nationalité '4'\n-afficher le nombre de livraisons par cuisinier '5'\n-Afficher le montant des achats cumulé par clients '6' " +
                "\n -exporter les statistiques utilisateurs en JSON"+ " " );
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
            ExporterStatsUtilisateursJson(conn);
            
            
        }

        /// <summary>
        ///  query d'affiche des commandes selon une période
        /// </summary>
        /// <param name="connection"></param>
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
            interfaceuser.adminInterface();
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
            interfaceuser.adminInterface();

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
            interfaceuser.adminInterface();

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
            interfaceuser.adminInterface();

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
            interfaceuser.adminInterface();

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
            interfaceuser.adminInterface();

        }

        public static void ChoisirStatsClient(MySqlConnection conn, int idClient)
        {
            Console.Clear();
            Console.WriteLine(
                $"Statistiques pour le client #{idClient}\n" +
                "1 : Afficher ses commandes sur une période\n" +
                "2 : Afficher la moyenne du prix de SES commandes\n" +
                "3 : Afficher le montant total qu’il a dépensé\n" +
                "4 : Afficher ses commandes par nationalité sur une période\n" +
                "0 : Retour"
            );
            char choix = Convert.ToChar(Console.ReadLine());
            switch (choix)
            {
                case '1': CommandesParPeriodeClient(conn, idClient); break;
                case '2': MoyenneCommandeClient(conn, idClient); break;
                case '3': TotalDepenseClient(conn, idClient); break;
                case '4': CommandesParNationaliteEtPeriode(conn, idClient); break;
                default: return;
            }
        }

        /* ---------- 1. Commandes du client entre deux dates ---------- */
        private static void CommandesParPeriodeClient(MySqlConnection c, int idClient)
        {
            Console.Write("Date début (yyyy-MM-dd) : ");
            DateTime d1 = DateTime.Parse(Console.ReadLine() + " 00:00:00");
            Console.Write("Date fin   (yyyy-MM-dd) : ");
            DateTime d2 = DateTime.Parse(Console.ReadLine() + " 23:59:59");

            const string sql = @"
            SELECT co.commande,
                   co.date_heure_commande,
                   SUM(p.prix) AS total_commande
            FROM   commande            co
            JOIN   ligne_de_commande_  ldc  ON ldc.commande = co.commande
            JOIN   inclue             i    ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN   plat               p    ON p.id = i.id
            WHERE  co.id_client = @cli
              AND  co.date_heure_commande BETWEEN @d1 AND @d2
            GROUP  BY co.commande , co.date_heure_commande
            ORDER  BY co.date_heure_commande;";

            using var cmd = new MySqlCommand(sql, c);
            cmd.Parameters.AddWithValue("@cli", idClient);
            cmd.Parameters.AddWithValue("@d1", d1);
            cmd.Parameters.AddWithValue("@d2", d2);

            using var r = cmd.ExecuteReader();
            if (!r.HasRows) { Console.WriteLine("Aucune commande."); return; }

            Console.WriteLine("\n# |     Date et heure     | Total (€)");
            while (r.Read())
                Console.WriteLine($"{r.GetInt32(0),2} | {r.GetDateTime(1):yyyy-MM-dd HH:mm} | {r.GetDecimal(2):F2}");
        }

        /* ---------- 2. Moyenne du prix de SES commandes ---------- */
        private static void MoyenneCommandeClient(MySqlConnection c, int idClient)
        {
            const string sql = @"
            SELECT AVG(montant) FROM (
              SELECT co.commande,
                     SUM(p.prix) AS montant
              FROM   commande co
              JOIN   ligne_de_commande_ ldc ON ldc.commande = co.commande
              JOIN   inclue i              ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
              JOIN   plat   p              ON p.id = i.id
              WHERE  co.id_client = @cli
              GROUP  BY co.commande
            ) t;";

            using var cmd = new MySqlCommand(sql, c);
            cmd.Parameters.AddWithValue("@cli", idClient);
            var res = cmd.ExecuteScalar();
            Console.WriteLine(res == DBNull.Value
                ? "Aucune commande."
                : $"Moyenne de ses commandes : {Convert.ToDecimal(res):F2} €");
        }

        /* ---------- 3. Montant cumulé dépensé par le client ---------- */
        private static void TotalDepenseClient(MySqlConnection c, int idClient)
        {
            const string sql = @"
            SELECT SUM(p.prix)
            FROM   commande            co
            JOIN   ligne_de_commande_  ldc ON ldc.commande = co.commande
            JOIN   inclue              i   ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN   plat                p   ON p.id = i.id
            WHERE  co.id_client = @cli;";

            using var cmd = new MySqlCommand(sql, c);
            cmd.Parameters.AddWithValue("@cli", idClient);
            var res = cmd.ExecuteScalar();
            Console.WriteLine(res == DBNull.Value
                ? "Aucune dépense enregistrée."
                : $"Montant total dépensé : {Convert.ToDecimal(res):F2} €");
        }

        /* ---------- 4. Commandes par nationalité + période ---------- */
        private static void CommandesParNationaliteEtPeriode(MySqlConnection c, int idClient)
        {
            Console.Write("Nationalité de cuisine (ex. Italien) : ");
            string pays = Console.ReadLine();
            Console.Write("Date début (yyyy-MM-dd) : ");
            DateTime d1 = DateTime.Parse(Console.ReadLine() + " 00:00:00");
            Console.Write("Date fin   (yyyy-MM-dd) : ");
            DateTime d2 = DateTime.Parse(Console.ReadLine() + " 23:59:59");

            const string sql = @"
            SELECT co.commande,
                   co.date_heure_commande,
                   p.nom
            FROM   commande            co
            JOIN   ligne_de_commande_  ldc ON ldc.commande = co.commande
            JOIN   inclue              i   ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN   plat                p   ON p.id = i.id
            JOIN   pays                pa  ON pa.idpays = p.idpays
            WHERE  co.id_client   = @cli
              AND  pa.nom         = @pays
              AND  co.date_heure_commande BETWEEN @d1 AND @d2
            ORDER  BY co.date_heure_commande;";

            using var cmd = new MySqlCommand(sql, c);
            cmd.Parameters.AddWithValue("@cli", idClient);
            cmd.Parameters.AddWithValue("@pays", pays);
            cmd.Parameters.AddWithValue("@d1", d1);
            cmd.Parameters.AddWithValue("@d2", d2);

            using var r = cmd.ExecuteReader();
            if (!r.HasRows) { Console.WriteLine("Aucun résultat."); return; }

            Console.WriteLine("\n# |     Date      | Plat");
            while (r.Read())
                Console.WriteLine($"{r.GetInt32(0),2} | {r.GetDateTime(1):yyyy-MM-dd} | {r.GetString(2)}");
        }
        public static void ExporterStatsUtilisateursJson(MySqlConnection c)
        {
            string nomFichier = "stats_utilisateurs.json";

            // Étendre la longueur max pour GROUP_CONCAT
            using (var cmdInit = new MySqlCommand("SET SESSION group_concat_max_len=1000000;", c))
            {
                cmdInit.ExecuteNonQuery();
            }

            string requete = @"
        SELECT CONCAT('[', GROUP_CONCAT(
            JSON_OBJECT(
                'id_utilisateur', u.id,
                'nom_complet', CONCAT(u.Prenom, ' ', u.Nom),
                'email', u.email,
                'total_commandes', IFNULL(nb_cmd, 0),
                'montant_total', IFNULL(montant, 0),
                'moyenne_par_commande', IFNULL(montant / nb_cmd, 0)
            )
        ), ']') AS json
        FROM utilisateur u
        LEFT JOIN (
            SELECT cl.id,
                   COUNT(co.commande) AS nb_cmd,
                   SUM(p.prix) AS montant
            FROM custommer cl
            JOIN commande co ON co.id_client = cl.id_client
            JOIN ligne_de_commande_ ldc ON ldc.commande = co.commande
            JOIN inclue i ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
            JOIN plat p ON p.id = i.id
            GROUP BY cl.id
        ) stats ON stats.id = u.id;
    ";

            using (var cmd = new MySqlCommand(requete, c))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string jsonResultat = reader.GetString("json");
                    File.WriteAllText(nomFichier, jsonResultat);

                    var p = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = nomFichier,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(p);

                    Console.WriteLine($"✅ Statistiques exportées dans {nomFichier}");
                }
                else
                {
                    Console.WriteLine("❌ Aucune statistique trouvée.");
                }
            }

            interfaceuser.adminInterface();
        }


    }
}


