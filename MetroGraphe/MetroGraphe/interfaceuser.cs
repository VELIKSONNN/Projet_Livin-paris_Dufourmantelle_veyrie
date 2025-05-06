using MySql.Data.MySqlClient;
using Mysqlx.Prepare;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livinparis_dufourmantelle_veyrie;
using System.ComponentModel.Design;
using System.Data;
using System.Reflection;

namespace livinparis_dufourmantelle_veyrie
{
    /// <summary>
    /// Représente l'interface utilisateur permettant d'interagir avec la base de données.
    /// </summary>
    public class interfaceuser
    {
        static public MySqlConnection connexion;
        public MySqlConnection Connexion { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="interfaceuser"/> et lance l'interface principale.
        /// </summary>
        /// <param name="_connexion">La connexion MySQL à utiliser.</param>
        public interfaceuser(MySqlConnection _connexion)
        {
            connexion = _connexion;
            MainInterface();
        }

        /// <summary>
        /// Affiche l'interface principale invitant l'utilisateur à saisir ses identifiants.
        /// </summary>
        static public void MainInterface()
        {
            Console.WriteLine("Bienvenue !\n--Se connecter en tant qu'utilisateur'1'--\n--Se connecter en tant qu'admin '2--\n--S'enregistrer '3'--\n");
            char choixactio = Convert.ToChar(Console.ReadLine());
            switch (choixactio)
            {
                case '1':
                    Console.WriteLine(" Veuillez entrer votre mot de passe et nom de famille pour vous connecter");
                    Console.Write("nom :");
                    string idutil = Console.ReadLine();
                    Console.Write("mot de passe :");
                    string mdp = Console.ReadLine();

                    string query = @"
                                            SELECT COUNT(*) 
                                            FROM utilisateur 
                                            WHERE Nom = @nom 
                                              AND mdp = @mdp;
                                        ";

                    using (var command = new MySqlCommand(query, connexion))
                    {
                        command.Parameters.AddWithValue("@nom", idutil);
                        command.Parameters.AddWithValue("@mdp", mdp);


                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("Authentification réussie !");
                            interface_utilisateur(idutil, mdp);

                        }
                        else
                        {
                            Console.WriteLine("nom ou mot de passe incorrecte :/");
                            MainInterface();
                        }


                    }

                    break;

                case '2':
                    Console.WriteLine("--Veuillez entrer votre mot de passe et identifiants admin pour vous connecter-- (appuyer simplement sur entrée lors du test)");
                    Console.Write("identifiant: ");
                    string identifiant = Console.ReadLine();
                    Console.Write("Mot de passe: ");
                    string motdepasse = Console.ReadLine();


                    if (identifiant == "" && motdepasse == "")
                    {
                        Console.Clear();
                        Console.WriteLine("Ouverture de l'interface admin");

                        adminInterface();

                    }

                    break;
                case '3':



                    AjoutUtilisateur();
                    break;
                

            }



        }

        /// <summary>
        /// Permet à l'utilisateur de choisir l'aciton qu'il souhaite réaliser
        /// </summary>
        /// <param name="idutil"></param>
        /// <param name="mdp"></param>
        static public void interface_utilisateur(string idutil,string mdp)
        {
            Console.Clear ();
            Console.WriteLine("Que souhaitez vous faire ? \n--Afficher le plus court trajet pour l'une de vos commandes '1'--\n--Faire une nouvelle commandes '2'--\n --Voir les statistiques de votre compte ?'3'--\n -- Afficher les statistiques d'un utilisateur en JSON '4--\n----Exporter vos statistiques en XML '5'--");
            char rep= Convert.ToChar(Console.ReadLine());
            switch (rep)
            {
                case '1':
                    Console.WriteLine("--Voici toute vos commandes, chaque ligne correspond à une portion du plat livré--");
                    affiche_commandes_util(idutil, mdp);
                    Console.WriteLine("Quelle est l'id de la commande à afficher ? ");
                    int idcommande = int.Parse(Console.ReadLine());
                    recupdépartarrivé(idcommande);
                    break;
                case '2':
                    Console.Clear () ;
                    Console.WriteLine("quel est l'id du cuisinier à qui vous voulez faire une commande");
                    AfficherTable("cuisinier");
                    int idcuisinier= int.Parse(Console.ReadLine());
                    int idClient;
                    using (var cmd = new MySqlCommand(@"
    SELECT cust.id_client
      FROM custommer AS cust
      JOIN utilisateur AS u ON cust.id = u.id
     WHERE u.nom = @nom
       AND u.mdp = @mdp", connexion))
                    {
                        cmd.Parameters.AddWithValue("@nom", idutil);
                        cmd.Parameters.AddWithValue("@mdp", mdp);

                        object res = cmd.ExecuteScalar();
                        if (res == null || res == DBNull.Value)
                        {
                            Console.WriteLine("Vous n’êtes pas enregistré comme client.");
                            return;          
                        }
                        idClient = Convert.ToInt32(res);
                    }

                    Console.WriteLine($"ID client = {idClient}");
                    ajoutcommande(idClient, idcuisinier);

                    break;
                case '3':
                   
            using (var cmd = new MySqlCommand(@"SELECT cust.id_client
                                              FROM custommer AS cust
                                              JOIN utilisateur AS u ON cust.id = u.id
                                             WHERE u.nom = @nom
                                               AND u.mdp = @mdp", connexion))
                    {
                        cmd.Parameters.AddWithValue("@nom", idutil);
                        cmd.Parameters.AddWithValue("@mdp", mdp);

                        object res = cmd.ExecuteScalar();
                        if (res == null || res == DBNull.Value)
                        {
                            Console.WriteLine("Vous n’êtes pas enregistré comme client.");
                            return;          
                        }
                        idClient = Convert.ToInt32(res);
                    }
                    statistiques.ChoisirStatsClient(connexion, idClient);

                    break;
                case '4':
                    statistiques.ExporterStatistiquesJson(connexion, idutil, mdp);
                    break;
                case '5':
                    statistiques.ExporterStatistiquesXml(connexion, idutil, mdp);
                    break;

            }

        }

        /// <summary>
        /// Cette commande affiche toute les commande d'un utilisateur
        /// 
        /// </summary>
        /// <param name="idutil"></param>
        /// <param name="mdp"></param>
        public static void affiche_commandes_util(string idutil,string mdp)
        {
             string query = @"SELECT
                              cmd.`commande`               AS idCommande,
                              p.`nom`                      AS platLigne,
                              l.`date_heure_livraison`     AS dateLivraison,
                              CONCAT(cu.`Prenom`, ' ', cu.`Nom`) AS cuisinier,
                              CONCAT(cl.`Prenom`, ' ', cl.`Nom`) AS client
                            FROM `utilisateur` AS cl
                              JOIN `custommer` AS csm ON csm.`id`= cl.`id`
                              JOIN `commande` AS cmd ON cmd.`id_client`= csm.`id_client`
                              JOIN `cuisinier` AS cs  ON cs.`id_cuisinier`= cmd.`id_cuisinier`
                              JOIN `utilisateur` AS cu  ON cu.`id`= cs.`id`
                              JOIN `ligne_de_commande_` AS ldc ON ldc.`commande` = cmd.`commande`
                              JOIN `livraison`      AS l   ON l.`id_livraison`= ldc.`id_livraison`
                              JOIN `inclue` AS inc ON inc.`id_ligne_de_commande` = ldc.`id_ligne_de_commande`
                              JOIN `plat` AS p   ON p.`id`                = inc.`id`
                            WHERE
                              cl.`Nom` = @idutil
                              AND cl.`mdp` = @mdp
                            ORDER BY
                              l.`date_heure_livraison` DESC;
                            ";

            
            using (var cmd = new MySqlCommand(query, connexion))
            {
                cmd.Parameters.AddWithValue("@idutil",idutil);
                cmd.Parameters.AddWithValue("@mdp", mdp);

           
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Aucune commande trouvée pour cet utilisateur.");
                        return;
                    }

                    Console.WriteLine($"Commandes pour l'utilisateur {idutil} :\n");
                    while (reader.Read())
                    {
                        DateTime dateLiv = reader.GetDateTime("dateLivraison");
                        string plat = reader.GetString("platLigne");
                        string cuisinier = reader.GetString("cuisinier");
                        string client = reader.GetString("client");
                        int idcommande = reader.GetInt32("idCommande");
                        Console.WriteLine(
                            $"Id de la commande : {idcommande} | "+
                            $"Date livraison : {dateLiv:yyyy-MM-dd HH:mm} | " +
                            $"Cuisinier : {cuisinier} | " +
                            $"Client : {client} | " +
                            $"Plat : {plat}"
                        ) ;
                    }
                }
            }
        }

        /// <summary>
        /// Ouvre l'interface administrateur et propose différentes actions.
        /// </summary>
        static public void adminInterface()
        {


            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande)\nSupprimer un tuple '2'\nExectuer une query(stats) '3'\nSimplement afficher une table '4' \nAfficher directement un chemin le plus court pour une livraison? '5'\nSortir du programme '6' \nExporter les statistiques d'un admin en JSON '7'--Exporter les statistiques d'un utilisateur en XML '8'");


            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande)\nSupprimer un tuple '2'\nExectuer une query(stats) '3'\nSimplement afficher une table '4' \nAfficher directement un chemin le plus court pour une livraison? '5'\nSortir du programme '6' \nExporter les statistiques d'un admin en JSON '7'");
            Console.WriteLine("Afficher le graphe colorée des liens entre les utilisateur '8' ");

            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande)\nSupprimer un tuple '2'\nExectuer une query(stats) '3'\nSimplement afficher une table '4' \nAfficher directement un chemin le plus court pour une livraison? '5'\nSortir du programme '6' \nExporter les statistiques d'un admin en JSON '7'");
            Console.WriteLine("Afficher le graphe colorée des liens entre les utilisateur '8' ");


            char actionprimaire = Convert.ToChar(Console.ReadLine());

            switch (actionprimaire)
            {
                case '1':
                    ajouttuple();
                    adminInterface();
                    break;
                case '2':
                    supprselection();
                    adminInterface();
                    break;
                case '3':
                    statistiques.choisirstats(connexion);
                    adminInterface();
                    break;
                case '4':
                    Console.WriteLine("Quelle est la table à afficher");
                    string table = Console.ReadLine();
                    AfficherTable(table);
                    adminInterface();
                    break;
                case '5':
                    Console.WriteLine("quelle est l'id de la commande dont vous voulez calcule le plus court chemins ? ");
                    int idcommande = int.Parse(Console.ReadLine());
                    recupdépartarrivé(idcommande);
                    adminInterface();
                    break;
                case '6':
                    Console.Clear();
                    Console.WriteLine("Merci d'avoir utilisé livinParis et à très bientôt !!");
                    adminInterface();
                    break;
                case '7':
                    Console.WriteLine("Nom de l'utilisateur : ");
                    string nom = Console.ReadLine();
                    Console.WriteLine("Mot de passe : ");
                    string mdp = Console.ReadLine();
                    statistiques.ExporterStatistiquesJson(connexion, nom, mdp);
                    adminInterface();
                    break;
                case '8':

                    Console.Write("Nom de l'utilisateur : ");
                    string nomXml = Console.ReadLine();
                    Console.Write("Mot de passe : ");
                    string mdpXml = Console.ReadLine();
                    statistiques.ExporterStatistiquesXml(connexion, nomXml, mdpXml);
                    break;


                    afficheGrapheCommandes(connexion);
                    adminInterface();
                    break;
                    
                    

            }
        }

        /// <summary>
        /// Récupère les adresses du cuisinier et du client pour une commande donnée et appelle la fonction affichechemincuisinier qui affiche le chemin le plus court.
        /// </summary>
        /// <param name="idcommande">L'identifiant de la commande.</param>
        static void recupdépartarrivé(int idcommande)
        {
            string query = @"
        SELECT 
            u.adresse AS adresseCuisinier,
            uCl.adresse AS adresseClient
        FROM commande co
        JOIN cuisinier c ON co.id_cuisinier = c.id_cuisinier
        JOIN utilisateur u ON c.id = u.id
        JOIN custommer cl ON co.id_client = cl.id_client
        JOIN utilisateur uCl ON cl.id = uCl.id
        WHERE co.commande = @commandeId;
    ";

            using (MySqlCommand cmd = new MySqlCommand(query, connexion))
            {
                cmd.Parameters.AddWithValue("@commandeId", idcommande);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string adresseCuisinier = reader.GetString("adresseCuisinier");
                        string adresseClient = reader.GetString("adresseClient");

                        Program.affichechemincuisinier(adresseCuisinier, adresseClient, Program.creationgraphe());
                    }
                    else
                    {
                        Console.WriteLine("Aucune commande trouvée avec cet ID.");
                    }
                }
            }
        }

        /// <summary>
        /// Récupère l'indice maximum d'une colonne primaire dans une table donnée.
        /// </summary>
        /// <param name="table">Le nom de la table.</param>
        /// <param name="primarykey">Le nom de la clé primaire.</param>
        /// <returns>La valeur maximale ou 0 si aucun enregistrement n'existe.</returns>
        static int maxindice(string table, string primarykey)
        {
            string query = $"SELECT MAX({primarykey}) FROM {table}";
            using (MySqlCommand command1 = new MySqlCommand(query, connexion))
            {
                object result = command1.ExecuteScalar();

                if (result == DBNull.Value)
                {
                    return 0;
                }
                return Convert.ToInt32(result);
            }
        }

        
        /// <summary>
        /// Ajoute un compte en demandant à l'utilisateur de renseigner ses coordonnée, puis ajoute un tuple dans les table cuisinier et custommer en fonctions des besoin de l'utilisateur
        /// </summary>
        static private void AjoutUtilisateur()
        {
            Console.WriteLine(
                "Veuillez renseigner l’un après l’autre en appyant sur 'entrée' à chaque fois: prénom, nom, email, téléphone (10 chiffres), adresse, entreprise (ou vide), mot de passe.");
            string prenom = Console.ReadLine();
            string nom = Console.ReadLine();
            string email = Console.ReadLine();

            string tel = Console.ReadLine();
            while (tel.Length != 10)
            {
                Console.WriteLine("Format de numéro invalide. Réessayez :");
                tel = Console.ReadLine();
            }
          
            Console.WriteLine();
            affichestations();
            Console.WriteLine("Stations de metro la plus proche de chez vous ?");
            string adresse = Console.ReadLine();
            string entreprise = Console.ReadLine();
            string mdp = Console.ReadLine();
           
            using (var transaction = connexion.BeginTransaction())
            { 
                int iduser = maxindice("utilisateur", "id")+1;
                string sqlInsertUser = @"
            INSERT INTO utilisateur
                (id,Prenom, Nom, email, tel, adresse, entreprise, mdp)
            VALUES
                (@id, @Prenom, @Nom, @Email, @Tel, @Adresse, @Entreprise, @Mdp)";
                
                using (var cmd = new MySqlCommand(sqlInsertUser, connexion, transaction))
                {

                    cmd.Parameters.AddWithValue("@id", iduser);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Tel", tel);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue(
                        "@Entreprise",
                        string.IsNullOrWhiteSpace(entreprise) ? (object)DBNull.Value : entreprise
                    );
                    cmd.Parameters.AddWithValue("@Mdp", mdp);

                    cmd.ExecuteNonQuery();
                   
                }

                Console.WriteLine(
                    "Voulez-vous vous enregistrer comme :\n" +
                    " 1 = Client   |   2 = Cuisinier   |   3 = Les deux   ");
                string choix = Console.ReadLine()?.Trim();

               

                if (choix == "2" || choix == "3")
                {
                    string sqlCuisinier = @"
                INSERT INTO cuisinier (id_cuisinier,id)
                VALUES (@IdCuisinier,@id)";
                    using (var cmd2 = new MySqlCommand(sqlCuisinier, connexion, transaction))
                    {
                        int id_cuisinier = maxindice("cuisinier", "id_cuisinier")+1;
                        cmd2.Parameters.AddWithValue("@IdCuisinier", id_cuisinier);
                        cmd2.Parameters.AddWithValue("@id", iduser);
                        cmd2.ExecuteNonQuery();
                    }
                } 
                
                if (choix == "1" || choix == "3")
                {
                    string sqlClient = @"
                INSERT INTO custommer (id_client,id)
                VALUES (@IdClient,@id)";
                    using (var cmd3 = new MySqlCommand(sqlClient, connexion, transaction))
                    {
                    int    id_client = maxindice("custommer", "id_client")+1;
                        cmd3.Parameters.AddWithValue("@IdClient", id_client);
                        cmd3.Parameters.AddWithValue("@id", iduser);

                        cmd3.ExecuteNonQuery();
                    }
                }

                transaction.Commit();

                Console.WriteLine($"Utilisateur #{iduser} créé avec succès.");
            }

            MainInterface();
        }


        /// <summary>
        /// permet à l'admin d'ajouter un tuple
        /// </summary>
        static private void ajouttuple()
        {
            Console.Clear();
            Console.WriteLine("Voulez-vous ajouter un utilisateur '1', un cuisinier '2' ,un client '3' ou une commande '4' ?");
            char ajoutelement = Convert.ToChar(Console.ReadLine());
            switch (ajoutelement)
            {
                case '1':
                    Console.WriteLine("Veuillez fournir dans l'ordre et en appuyant sur entrée a chaque fois: Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise, le nom, et le mdp");
                    string Prenom = Convert.ToString(Console.ReadLine());
                    string email = Convert.ToString(Console.ReadLine());
                    int id = maxindice("utilisateur", "id") + 1;
                    string tel = Convert.ToString(Console.ReadLine());
                    string adresse = Convert.ToString(Console.ReadLine());
                    string entreprise = Convert.ToString(Console.ReadLine());
                    string Nom = Convert.ToString(Console.ReadLine());
                    string mdp = Convert.ToString(Console.ReadLine());

                    using (var transaction = connexion.BeginTransaction())
                    {
                        string insertQuery = @$"
                            INSERT INTO utilisateur ( Prenom, email, tel, adresse, entreprise, Nom, mdp)
                            VALUES (@id, @Prenom, @Email, @Tel, @Adresse, @Entreprise, @Nom, @Mdp)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connexion, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@Prenom", Prenom);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Tel", tel);
                            command.Parameters.AddWithValue("@Adresse", adresse);
                            command.Parameters.AddWithValue("@Entreprise", entreprise);
                            command.Parameters.AddWithValue("@Nom", Nom);
                            command.Parameters.AddWithValue("@Mdp", mdp);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit(); // Valide la transaction
                    }

                    AfficherTable("utilisateur");
                    break;

                case '2':
                    Console.WriteLine("Veuillez fournir dans l'ordre et en appuyant sur entrée a chaque fois: Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise, le nom, et le mdp");
                    Prenom = Convert.ToString(Console.ReadLine());
                    email = Convert.ToString(Console.ReadLine());
                    id = maxindice("utilisateur", "id") + 1;
                    tel = Convert.ToString(Console.ReadLine());
                    adresse = Convert.ToString(Console.ReadLine());
                    entreprise = Convert.ToString(Console.ReadLine());
                    Nom = Convert.ToString(Console.ReadLine());
                    mdp = Convert.ToString(Console.ReadLine());

                    using (var transaction = connexion.BeginTransaction())
                    {
                        // Insertion dans la table utilisateur
                        string insertQuery = @"
                                INSERT INTO utilisateur (id, Prenom, email, tel, adresse, entreprise, Nom, mdp)
                                VALUES (@id, @Prenom, @Email, @Tel, @Adresse, @Entreprise, @Nom, @Mdp)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connexion))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@Prenom", Prenom);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Tel", tel);
                            command.Parameters.AddWithValue("@Adresse", adresse);
                            command.Parameters.AddWithValue("@Entreprise", entreprise);
                            command.Parameters.AddWithValue("@Nom", Nom);
                            command.Parameters.AddWithValue("@Mdp", mdp);
                            command.ExecuteNonQuery();
                        }
                        // Insertion dans la table cuisinier
                        string insertQuery2 = @"INSERT INTO cuisinier(id_cuisinier, id) VALUES (@id, @id)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery2, connexion))
                        {
                            command.Parameters.AddWithValue("@id_cuisinier", id);
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }
                        AfficherTable("custommer");

                        int id_cuisinier = maxindice("cuisinier", "id_cuisinier");
                        insertQuery = $"INSERT INTO cuisinier(id_client, id) VALUES (@id,@id))";
                        transaction.Commit(); // Valide la transaction
                    }

                    AfficherTable("cuisinier");
                    break;

                case '3':
                    Console.WriteLine("Veuillez fournir dans l'ordre et en appuyant sur entrée a chaque fois: Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise, le nom, et le mdp");
                    Prenom = Convert.ToString(Console.ReadLine());
                    email = Convert.ToString(Console.ReadLine());
                    id = maxindice("utilisateur", "id") + 1;
                    tel = Convert.ToString(Console.ReadLine());
                    adresse = Convert.ToString(Console.ReadLine());
                    entreprise = Convert.ToString(Console.ReadLine());
                    Nom = Convert.ToString(Console.ReadLine());
                    mdp = Convert.ToString(Console.ReadLine());

                    using (var transaction = connexion.BeginTransaction())
                    {
                        string insertQuery = @"
                        INSERT INTO utilisateur (id, Prenom, email, tel, adresse, entreprise, Nom, mdp)
                        VALUES (@id, @Prenom, @Email, @Tel, @Adresse, @Entreprise, @Nom, @Mdp)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connexion))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@Prenom", Prenom);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Tel", tel);
                            command.Parameters.AddWithValue("@Adresse", adresse);
                            command.Parameters.AddWithValue("@Entreprise", entreprise);
                            command.Parameters.AddWithValue("@Nom", Nom);
                            command.Parameters.AddWithValue("@Mdp", mdp);
                            command.ExecuteNonQuery();
                        }
                        insertQuery = @"INSERT INTO custommer(id_client, id) VALUES (@id, @id)";
                        using (MySqlCommand command = new MySqlCommand(insertQuery, connexion))
                        {
                            command.Parameters.AddWithValue("@id_client", id);
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }

                    AfficherTable("custommer");
                    break;

                case '4':
                    Console.WriteLine("Connaissez-vous l'id du client et du cuisinier ? Tapez 1 pour oui et 0 pour non ");
                    int repconnaitid = int.Parse(Console.ReadLine());

                    if (repconnaitid == 0)
                    {
                        AfficherTable("custommer");
                        AfficherTable("cuisinier");
                    }

                    Console.WriteLine("Veuillez fournir dans l'ordre : l'id du client, l'id du cuisinier");
                    int idclient = int.Parse(Console.ReadLine());
                    int idcuisinier = int.Parse(Console.ReadLine());

                    ajoutcommande(idclient, idcuisinier);
                    break;
            }
            adminInterface();
        }

        /// <summary>
        /// permet la création d'une nouvelle commande par un admin ou un utilisateur
        /// </summary>
        /// <param name="idclient"></param>
        /// <param name="idcuisinier"></param>
        static void ajoutcommande(int idclient, int idcuisinier)
        {
           

            

            int commande = maxindice("commande", "commande") + 1;

            using (var transaction = connexion.BeginTransaction())
            {
                string insertQuery = @"
            INSERT INTO commande (commande, date_heure_commande, id_client, id_cuisinier)
            VALUES (@id, @date, @id_custommer, @id_cuisinier)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connexion, transaction))
                {
                    command.Parameters.AddWithValue("@id", commande);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@id_custommer", idclient);
                    command.Parameters.AddWithValue("@id_cuisinier", idcuisinier);
                    command.ExecuteNonQuery();
                }

                Console.Write("Combien de lignes de commande pour cette commande ? ");
                int nbLignes = int.Parse(Console.ReadLine());

                for (int i = 0; i < nbLignes; i++)
                {
                    Console.WriteLine($"\n=== Création de la ligne de commande n°{i + 1} ===");

                    int idmaxligne = maxindice("ligne_de_commande_", "id_ligne_de_commande") + 1;

                    Console.Write("Entrez la date de livraison (yyyy-MM-dd HH:mm:ss) : ");
                    string dateLivraisonStr = Console.ReadLine();
                    DateTime dateLivraison;
                    if (!DateTime.TryParse(dateLivraisonStr, out dateLivraison))
                    {
                        Console.WriteLine("Date invalide, utilisation de DateTime.Now.");
                        dateLivraison = DateTime.Now;
                    }
                    int idmaxlivraison = maxindice("livraison", "id_livraison") + 1;
                    string insertLivraisonQuery = @"
                INSERT INTO livraison (id_livraison, date_heure_livraison)
                VALUES (@id_livraison, @dateLivraison)";
                    using (MySqlCommand cmdLivraison = new MySqlCommand(insertLivraisonQuery, connexion, transaction))
                    {
                        cmdLivraison.Parameters.AddWithValue("@id_livraison", idmaxlivraison);
                        cmdLivraison.Parameters.AddWithValue("@dateLivraison", dateLivraison);
                        cmdLivraison.ExecuteNonQuery();
                    }

                    string insertLigneQuery = @"
                INSERT INTO ligne_de_commande_ (id_ligne_de_commande, id_livraison, id_client, commande)
                VALUES (@idLigne, @idLivraison, @idClient, @commande)";
                    using (MySqlCommand cmdLigne = new MySqlCommand(insertLigneQuery, connexion, transaction))
                    {
                        cmdLigne.Parameters.AddWithValue("@idLigne", idmaxligne);
                        cmdLigne.Parameters.AddWithValue("@idLivraison", idmaxlivraison);
                        cmdLigne.Parameters.AddWithValue("@idClient", idclient);
                        cmdLigne.Parameters.AddWithValue("@commande", commande);
                        cmdLigne.ExecuteNonQuery();
                    }
                    Console.WriteLine($"Ligne de commande {idmaxligne} insérée.");

                    Console.Write("Combien de plats pour cette ligne ? ");
                    int nbPlats = int.Parse(Console.ReadLine());
                    for (int j = 0; j < nbPlats; j++)
                    {
                        Console.Write($"ID du plat n°{j + 1} : ");
                        int idPlat = int.Parse(Console.ReadLine());

                        string insertInclueQuery = @"
                    INSERT INTO inclue (id, id_ligne_de_commande)
                    VALUES (@idPlat, @idLigneCommande)";
                        using (MySqlCommand cmdInclue = new MySqlCommand(insertInclueQuery, connexion, transaction))
                        {
                            cmdInclue.Parameters.AddWithValue("@idPlat", idPlat);
                            cmdInclue.Parameters.AddWithValue("@idLigneCommande", idmaxligne);
                            cmdInclue.ExecuteNonQuery();
                        }
                        Console.WriteLine($"Plat #{idPlat} associé à la ligne de commande #{idmaxligne}.");
                    }
                }

                string queryprixcommande = @"SELECT SUM(p.prix) AS prixtotal
                                                    FROM commande co
                                                    JOIN ligne_de_commande_ ldc ON ldc.commande = co.commande
                                                    JOIN inclue i ON i.id_ligne_de_commande = ldc.id_ligne_de_commande
                                                    JOIN plat p ON p.id = i.id
                                                    WHERE co.commande = @commande;";
                using (MySqlCommand commandeprix = new MySqlCommand(queryprixcommande, connexion, transaction))
                {
                    commandeprix.Parameters.AddWithValue("@commande", commande);
                    object result = commandeprix.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        decimal prixtotal = Convert.ToDecimal(result);
                        Console.WriteLine($"Prix total de la commande #{commande} : {prixtotal}");
                    }
                    else
                    {
                        Console.WriteLine("Aucun plat trouvé pour cette commande.");
                    }
                }


                recupdépartarrivé(commande);
                Console.WriteLine("Transaction commitée.");
                transaction.Commit();
            }
        }

        /// <summary>
        /// Lance la procédure de suppression d'un tuple, soit par recherche par attributs, soit directement via l'ID.
        /// </summary>
        static private void supprselection()
        {
            Console.Clear();
            Console.WriteLine("Pour supprimer un tuple vous pouvez effectuer une recherche par attributs pour connaitre l'id du tuple à supprimer ou donner directement son id");
            Console.WriteLine("Faire une recherche '1' ou supprimer directement avec l'id '2'");
            char actionsuppr = Convert.ToChar(Console.ReadLine());
            switch (actionsuppr)
            {
                case '1':
                    string table = recherchetable();
                    AfficherTable(table);
                    supprtuple(table);
                    break;

                case '2':
                    Console.WriteLine();
                    table = recherchetable();
                    supprtuple(table);
                    break;
            }
        }


        /// <summary>
        /// Affiche le contenu d'une table spécifiée.
        /// </summary>
        /// <param name="table">Le nom de la table à afficher.</param>
        static void AfficherTable(string table)
        {
            Console.WriteLine("La nouvelle table " + table + " est la suivante");
            string query;

            switch (table.ToLower())
            {
                case "cuisinier":
                    query = @"
                SELECT c.id_cuisinier, u.Nom, u.Prenom
                FROM cuisinier c
                JOIN utilisateur u ON c.id = u.id
            ";
                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id_cuisinier"]} nom {reader["Nom"]}");
                        }
                    }
                    break;

                case "custommer":
                    query = @"
                SELECT cust.id_client, u.Nom, u.Prenom
                FROM custommer cust
                JOIN utilisateur u ON cust.id = u.id
            ";
                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id_client"]} Nom: {reader["Nom"]}");
                        }
                    }
                    break;

                case "ingrédients":
                    query = "SELECT id_ingr, nom_ingr FROM ingrédients";
                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id_ingr"]} nom ingrédient: {reader["nom_ingr"]}");
                        }
                    }
                    break;

                default:
                    Console.Write("Quel attribut veux-tu voir ? ");
                    string attribut = Console.ReadLine();
                    query = $"SELECT id, {attribut} FROM {table}";
                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id"]} {attribut}: {reader[attribut]}");
                        }
                    }
                    break;
            }
            
        }

        /// <summary>
        /// Supprime un tuple d'une table spécifiée en se basant sur son identifiant.
        /// </summary>
        /// <param name="table">Le nom de la table depuis laquelle supprimer le tuple.</param>
        static void supprtuple(string table)
        {
            Console.WriteLine();
            Console.WriteLine("Quel est l'id du tuple à supprimer ?");

            EnsureForeignKeyConstraint(connexion, "baselivinparis", "cuisinier", "cuisinier_ibfk_1", "id", "utilisateur", "id");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "custommer", "custommer_ibfk_1", "id", "utilisateur", "id");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "commande", "commande_ibfk_1", "id_client", "custommer", "id_client");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "commande", "commande_ibfk_2", "id_cuisinier", "cuisinier", "id_cuisinier");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "avis", "avis_ibfk_1", "commande", "commande", "commande");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "contient", "contient_ibfk_1", "id", "plat", "id");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "contient", "contient_ibfk_2", "id_ingr", "ingrédients", "id_ingr");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "inclue", "inclue_ibfk_1", "id", "plat", "id");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "inclue", "inclue_ibfk_2", "id_ligne_de_commande", "ligne_de_commande_", "id_ligne_de_commande");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ingrédients", "ingrédients_ibfk_1", "idpays", "pays", "idpays");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_1", "id_livraison", "livraison", "id_livraison");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_2", "id_client", "custommer", "id_client");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_3", "commande", "commande", "commande");
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "plat", "plat_ibfk_1", "idpays", "pays", "idpays");

            int idTodelete = int.Parse(Console.ReadLine());
            using (var transaction = connexion.BeginTransaction())
            {
                string querysuppr = $"DELETE FROM {table} WHERE id={idTodelete}";
                using (MySqlCommand command2 = new MySqlCommand(querysuppr, connexion))
                {
                    int rowsAffected = command2.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} ligne(s) supprimée(s).");
                }
                transaction.Commit();
            }

            AfficherTable(table);
            Console.WriteLine("Élément supprimé !");
            adminInterface();
        }

        /// <summary>
        /// Vérifie et met en place une contrainte de clé étrangère avec suppression en cascade.
        /// </summary>
        /// <param name="connexion">La connexion MySQL.</param>
        /// <param name="databaseName">Le nom de la base de données.</param>
        /// <param name="tableName">Le nom de la table sur laquelle appliquer la contrainte.</param>
        /// <param name="constraintName">Le nom de la contrainte de clé étrangère.</param>
        /// <param name="foreignKeyColumn">La colonne de la clé étrangère dans la table.</param>
        /// <param name="referencedTable">Le nom de la table référencée.</param>
        /// <param name="referencedColumn">La colonne référencée dans la table référencée.</param>
        public static void EnsureForeignKeyConstraint(MySqlConnection connexion, string databaseName, string tableName, string constraintName, string foreignKeyColumn, string referencedTable, string referencedColumn)
        {
            string checkQuery = @"SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = @databaseName 
            AND TABLE_NAME = @tableName AND CONSTRAINT_NAME = @constraintName;";

            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, connexion))
            {
                checkCmd.Parameters.AddWithValue("@databaseName", databaseName);
                checkCmd.Parameters.AddWithValue("@tableName", tableName);
                checkCmd.Parameters.AddWithValue("@constraintName", constraintName);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    string dropQuery = $"ALTER TABLE {tableName} DROP FOREIGN KEY {constraintName};";
                    using (MySqlCommand dropCmd = new MySqlCommand(dropQuery, connexion))
                    {
                        dropCmd.ExecuteNonQuery();
                    }
                }
            }

            string addQuery = $@"
                    ALTER TABLE {tableName} 
                    ADD CONSTRAINT {constraintName} 
                    FOREIGN KEY ({foreignKeyColumn}) 
                    REFERENCES {referencedTable}({referencedColumn}) 
                    ON DELETE CASCADE; ";

            using (MySqlCommand addCmd = new MySqlCommand(addQuery, connexion))
            {
                addCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Demande à l'utilisateur de saisir un attribut pour effectuer une recherche.
        /// </summary>
        /// <returns>L'attribut saisi par l'utilisateur.</returns>
        static string rechercheattribut()
        {
            Console.WriteLine("Par quel attribut souhaitez-vous faire une recherche");
            string attributderecherche = Console.ReadLine();
            return attributderecherche;
        }

        /// <summary>
        /// Demande à l'utilisateur de saisir le nom de la table dans laquelle effectuer une recherche.
        /// </summary>
        /// <returns>Le nom de la table saisi par l'utilisateur.</returns>
        static string recherchetable()
        {
            Console.WriteLine("Dans quelle table souhaitez-vous faire une recherche");
            string tablederecherche = Console.ReadLine();
            return tablederecherche;
        }

        /// <summary>
        /// affiche toutes les stations parisienne à l'aide d'une liste
        /// </summary>
        static void affichestations()
        {
            string[] stations = {
    /* 001 */ "Abbesses",                 /* 002 */ "Alésia",                       /* 003 */ "Alexandre‑Dumas",
    /* 004 */ "Alma‑Marceau",             /* 005 */ "Anatole‑France",               /* 006 */ "Anvers",
    /* 007 */ "Arcueil‑Cachan",           /* 008 */ "Argentine",                    /* 009 */ "Arts‑et‑Métiers",
    /* 010 */ "Assemblée‑Nationale",      /* 011 */ "Aubervilliers‑Hôtel‑de‑Ville", /* 012 */ "Aubervilliers‑Pantin 4 Chemins",
    /* 013 */ "Avron",                    /* 014 */ "Balard",                       /* 015 */ "Barbès‑Rochechouart",
    /* 016 */ "Bastille",                 /* 017 */ "Bérault",                      /* 018 */ "Belleville",
    /* 019 */ "Bel‑Air",                  /* 020 */ "Bel‑Est",                      /* 021 */ "Belgrand",
    /* 022 */ "Berault‑Fontenay‑s‑Bois",  /* 023 */ "Bercy",                        /* 024 */ "Bibliothèque‑F.‑Mitterrand",
    /* 025 */ "Bir‑Hakeim‑Tour‑Eiffel",   /* 026 */ "Blanche",                      /* 027 */ "Bobigny‑Pablo‑Picasso",
    /* 028 */ "Bobigny‑Raymond‑Queneau",  /* 029 */ "Boissière",                    /* 030 */ "Bolivar",
    /* 031 */ "Bonne‑Nouvelle",           /* 032 */ "Botzaris",                     /* 033 */ "Boulogne‑Jean‑Jaurès",
    /* 034 */ "Boulogne‑Pont‑de‑Saint‑Cloud",      /* 035 */ "Boulogne‑Pont‑de‑Sèvres",    /* 036 */ "Boucicaut",
    /* 037 */ "Bourse",                   /* 038 */ "Brochant",                     /* 039 */ "Bréguet‑Sabin",
    /* 040 */ "Buttes‑Chaumont",          /* 041 */ "Cadet",                        /* 042 */ "Cambronne",
    /* 043 */ "Campo‑Formio",             /* 044 */ "Cardinal‑Lemoine",             /* 045 */ "Carreau‑du‑Temple",
    /* 046 */ "Censier‑Daubenton",        /* 047 */ "Champs‑Élysées‑Clemenceau",    /* 048 */ "Champ‑de‑Mars‑Tour‑Eiffel",
    /* 049 */ "Chardon‑Lagache",          /* 050 */ "Charenton‑Écoles",             /* 051 */ "Charles‑de‑Gaulle‑Étoile",
    /* 052 */ "Château‑d'Eau",            /* 053 */ "Château‑Landon",               /* 054 */ "Château‑Rouge",
    /* 055 */ "Château‑de‑Vincennes",     /* 056 */ "Châtelet",                     /* 057 */ "Chaussée‑d'Antin‑La Fayette",
    /* 058 */ "Chemin‑Vert",              /* 059 */ "Chevaleret",                   /* 060 */ "Chilly‑Mazarin",
    /* 061 */ "Corentin‑Celton",          /* 062 */ "Colonel‑Fabien",               /* 063 */ "Commerce",
    /* 064 */ "Concorde",                 /* 065 */ "Convention",                   /* 066 */ "Corvisart",
    /* 067 */ "Couronnes",                /* 068 */ "Cour Saint‑Émilion",           /* 069 */ "Courcelles",
    /* 070 */ "Créteil‑L'Échat",          /* 071 */ "Créteil‑Préfecture",           /* 072 */ "Créteil‑Université",
    /* 073 */ "Crimée",                   /* 074 */ "Croix‑de‑Chavaux",             /* 075 */ "Danube",
    /* 076 */ "Daumesnil",                /* 077 */ "Denfert‑Rochereau",            /* 078 */ "Dugommier",
    /* 079 */ "Dupleix",                  /* 080 */ "Duroc",                        /* 081 */ "École Militaire",
    /* 082 */ "Église d'Auteuil",         /* 083 */ "Église de Pantin",             /* 084 */ "Église de Pantin‑H4",
    /* 085 */ "Église de Pantin‑H5",      /* 086 */ "Etienne‑Marcel",               /* 087 */ "Exelmans",
    /* 088 */ "Faidherbe‑Chaligny",       /* 089 */ "Falguière",                    /* 090 */ "Felix‑Faure",
    /* 091 */ "Filles du‑Calvaire",       /* 092 */ "Fort d'Aubervilliers",         /* 093 */ "Franklin‑D.-Roosevelt",
    /* 094 */ "Gambetta",                 /* 095 */ "Gare‑de‑l'Est",               /* 096 */ "Gare‑de‑Lyon",
    /* 097 */ "Gare‑du‑Nord",             /* 098 */ "Garches‑Marnes‑la‑Coquette",   /* 099 */ "Goncourt",
    /* 100 */ "George V",                 /* 101 */ "Glacière",                    /* 102 */ "Goncourt‑Hôpital St‑Louis",
    /* 103 */ "Grands‑Boulevards",        /* 104 */ "Gratuit‑Musée‑d'Orsay",        /* 105 */ "Guy‑Môquet",
    /* 106 */ "Havre‑Caumartin",          /* 107 */ "Hôtel‑de‑Ville",              /* 108 */ "Invalides",
    /* 109 */ "Jasmin",                   /* 110 */ "Jaurès",                       /* 111 */ "Javel‑André‑Citroën",
    /* 112 */ "Jean‑Jaurès",              /* 113 */ "Jourdain",                     /* 114 */ "Kléber",
    /* 115 */ "La Chapelle",              /* 116 */ "La Courneuve‑8 Mai 1945",      /* 117 */ "La Défense‑Grande‑Arche",
    /* 118 */ "La Fourche",               /* 119 */ "La Motte‑Picquet‑Grenelle",     /* 120 */ "La Muette",
    /* 121 */ "Laumière",                /* 122 */ "Le Kremlin‑Bicêtre",           /* 123 */ "Le Peletier",
    /* 124 */ "Les Gobelins",             /* 125 */ "Les Halles",                  /* 126 */ "Liège",
    /* 127 */ "Louis‑Blanc",              /* 128 */ "Louvre‑Rivoli",               /* 129 */ "Mabillon",
    /* 130 */ "Madeleine",               /* 131 */ "Malesherbes",                  /* 132 */ "Mairie‑d'Ivry",
    /* 133 */ "Mairie‑de‑Clichy",        /* 134 */ "Mairie‑de‑Montreuil",          /* 135 */ "Mairie‑de‑Saint‑Ouen",
    /* 136 */ "Mairie‑de‑Saint‑Denis",   /* 137 */ "Mairie‑d'Issy",                /* 138 */ "Mairie‑des‑Lilas",
    /* 139 */ "Mairie‑du‑15ᵉ",           /* 140 */ "Maison Blanche",               /* 141 */ "Maisons‑Alfort‑Alfortville",
    /* 142 */ "Maisons‑Alfort‑Stade",    /* 143 */ "Maisons‑Alfort‑Les Juilliottes",/* 144 */ "Malakoff‑Plateau‑de‑Vanves",
    /* 145 */ "Malakoff‑Rue‑Étienne‑Dolet",/* 146 */ "Maraîchers",                 /* 147 */ "Marcadet‑Poissonniers",
    /* 148 */ "Marcel‑Sembat",            /* 149 */ "Marx‑Dormoy",                 /* 150 */ "Masséna",
    /* 151 */ "Maubert‑Mutualité",        /* 152 */ "Michel‑Ange‑Auteuil",          /* 153 */ "Michel‑Ange‑Molitor",
    /* 154 */ "Michel‑Bizot",             /* 155 */ "Miromesnil",                   /* 156 */ "Monceau",
    /* 157 */ "Monge",                    /* 158 */ "Montgallet",                   /* 159 */ "Montparnasse‑Bienvenue",
    /* 160 */ "Nation",                   /* 161 */ "Notre‑Dame‑des‑Champs",        /* 162 */ "Notre‑Dame‑de‑Lorette",
    /* 163 */ "Oberkampf",                /* 164 */ "Odéon",                        /* 165 */ "Opéra",
    /* 166 */ "Ourcq",                    /* 167 */ "Palais‑Royal‑Musée‑du‑Louvre", /* 168 */ "Passy",
    /* 169 */ "Pasteur",                  /* 170 */ "Pelleport",                    /* 171 */ "Père‑Lachaise",
    /* 172 */ "Philippe‑Auguste",         /* 173 */ "Picpus",                       /* 174 */ "Pigalle",
    /* 175 */ "Place‑d'Italie",           /* 176 */ "Place‑de‑Clichy",              /* 177 */ "Place‑des‑Fêtes",
    /* 178 */ "Place‑Monge",              /* 179 */ "Plaisance",                    /* 180 */ "Pointe‑du‑Lac",
    /* 181 */ "Poissonnière",             /* 182 */ "Pont‑de‑Neuilly",              /* 183 */ "Pont‑de‑Sèvres",
    /* 184 */ "Pont‑Marie",               /* 185 */ "Pont‑Neuf",                    /* 186 */ "Porte‑de‑Bagnolet",
    /* 187 */ "Porte‑de‑Champ‑rette",     /* 188 */ "Porte‑de‑Charenton",           /* 189 */ "Porte‑de‑Choisy",
    /* 190 */ "Porte‑de‑Clignancourt",    /* 191 */ "Porte‑de‑Clichy",              /* 192 */ "Porte‑de‑la Chapelle",
    /* 193 */ "Porte‑de‑la Villette",     /* 194 */ "Porte‑de‑Lil as",              /* 195 */ "Porte‑de‑Pantin",
    /* 196 */ "Porte‑de‑Saint‑Cloud",     /* 197 */ "Porte‑de‑Saint‑Ouen",          /* 198 */ "Porte‑de‑Vanves",
    /* 199 */ "Porte‑Dauphine",           /* 200 */ "Porte‑d'Ivry",                 /* 201 */ "Porte‑Dorée",
    /* 202 */ "Porte‑d'Orléans",          /* 203 */ "Porte‑Maillot",                /* 204 */ "Poterne‑des‑Peupliers",
    /* 205 */ "Pré‑Saint‑Gervais",        /* 206 */ "Pretto‑Porte‑d'Italie",        /* 207 */ "Pyramides",
    /* 208 */ "Quai‑de‑la Gare",         /* 209 */ "Quai‑de‑la Rapée",             /* 210 */ "Quatre‑Septembre",
    /* 211 */ "Rambuteau",                /* 212 */ "Ranelagh",                     /* 213 */ "Raspail",
    /* 214 */ "Réaumur‑Sébastopol",       /* 215 */ "Rennes",                       /* 216 */ "Reuilly‑Diderot",
    /* 217 */ "Ricardo‑Rezai",            /* 218 */ "Richard‑Lenoir",               /* 219 */ "Richelieu‑Drouot",
    /* 220 */ "Riquet",                   /* 221 */ "Robespierre",                  /* 222 */ "Rome",
    /* 223 */ "Rue‑des‑Boulets",          /* 224 */ "Rue‑du‑Bac",                   /* 225 */ "Rue‑Saint‑Maur",
    /* 226 */ "Saint‑Ambroise",           /* 227 */ "Saint‑Augustin",               /* 228 */ "Saint‑Denis‑Pleyel",
    /* 229 */ "Saint‑Fargeau",            /* 230 */ "Saint‑François‑Xavier",        /* 231 */ "Saint‑Germain‑des‑Prés",
    /* 232 */ "Saint‑Jacques",            /* 233 */ "Saint‑Lazare",                 /* 234 */ "Saint‑Mandé",
    /* 235 */ "Saint‑Marcel",             /* 236 */ "Saint‑Michel",                 /* 237 */ "Saint‑Paul",
    /* 238 */ "Saint‑Philippe‑du‑Roule",  /* 239 */ "Saint‑Placide",               /* 240 */ "Saint‑Sébastien‑Froissart",
    /* 241 */ "Saint‑Sulpice",            /* 242 */ "Saint‑Surpier",               /* 243 */ "Sèvres‑Babylone",
    /* 244 */ "Sèvres‑Lecourbe",          /* 245 */ "Simplon",                      /* 246 */ "Solférino",
    /* 247 */ "Stalingrad",               /* 248 */ "Strasbourg‑Saint‑Denis",       /* 249 */ "Sully‑Morland",
    /* 250 */ "Télégraphe",               /* 251 */ "Temple",                       /* 252 */ "Ternes",
    /* 253 */ "Tolbiac",                  /* 254 */ "Trinité‑d'Estienne‑d'Orves",    /* 255 */ "Trocadéro",
    /* 256 */ "Tuileries",                /* 257 */ "Université",                   /* 258 */ "Val‑de‑Fontenay",
    /* 259 */ "Vaneau",                   /* 260 */ "Varennes",                     /* 261 */ "Vaugirard",
    /* 262 */ "Vavin",                    /* 263 */ "Victor‑Hugo",                  /* 264 */ "Villejuif‑Louis‑Aragon",
    /* 265 */ "Villejuif‑Léo‑Lagrange",   /* 266 */ "Villejuif‑Paul‑Vaillant‑Couturier", /* 267 */ "Villejuif‑Gustave‑Roussy",
    /* 268 */ "Villemomble‑Bondy",        /* 269 */ "Villeparisis‑Mitry‑Le Neuf",   /* 270 */ "Villiers",
    /* 271 */ "Volontaires",              /* 272 */ "Voltaire",                     /* 273 */ "Wagram",
    /* 274 */ "White‑House",              /* 275 */ "Zola‑III‑Orly Aéroport T4",    /* 276 */ "Orly‑Aéroport T3",
    /* 277 */ "Pont‑Carrefour‑Silic",     /* 278 */ "Pont‑Rungis‑Aéroport d'Orly",  /* 279 */ "La Bourgogne",
    /* 280 */ "Aéroport d'Orly T1‑T2",    /* 281 */ "Maison‑Blanche‑Paris‑13",      /* 282 */ "Institut‑Gustave‑Roussy",
    /* 283 */ "L'Haÿ‑les‑Roses",          /* 284 */ "Chevilly‑Trois‑Communes",      /* 285 */ "Thiais‑Orly‑Ville",
    /* 286 */ "Mines‑Pont‑de‑Sèvres",     /* 287 */ "Saint‑Maur‑Créteil‑Hôpital",   /* 288 */ "Bagneux‑Lucie‑Aubrac",
    /* 289 */ "Barbara",                  /* 290 */ "Arcueil‑Cachan‑Centre",        /* 291 */ "Pôle‑Université Lumière",
    /* 292 */ "Athis‑Mons‑Paray",         /* 293 */ "Vitry‑Centre",                 /* 294 */ "Vitry‑Les Ardoines",
    /* 295 */ "Pont‑de‑Bondy",            /* 296 */ "Drancy‑Bobigny",               /* 297 */ "Clichy‑Montfermeil",
    /* 298 */ "Hôpital‑Ballanger",        /* 299 */ "Aulnay‑Parc‑Relais",           /* 300 */ "Triangle de Gonesse",
    /* 301 */ "Le Bourget‑Aéroport",      /* 302 */ "Stains‑La‑Cerisaie",           /* 303 */ "Saint‑Denis‑Université",
    /* 304 */ "La Courneuve‑Six Routes",  /* 305 */ "Mairie‑d'Aubervilliers",       /* 306 */ "Wagram",
    /* 307 */ "Pleyel‑Hub‑Olympique",     /* 308 */ "Église de Pantin"
};

            // -----------------------------------------------------------------------------
            // Impression : 3 stations par ligne.
            for (int i = 0; i < stations.Length; i += 3)
            {
                string s1 = stations[i];
                string s2 = (i + 1 < stations.Length) ? stations[i + 1] : "";
                string s3 = (i + 2 < stations.Length) ? stations[i + 2] : "";
                Console.WriteLine($"{s1,-35}{s2,-35}{s3}");
            }
        }




        /// <summary>
        /// Construit le graphe « clients – cuisiniers » à partir de toutes les commandes.
        /// Chaque commande engendre une arête non orientée (donc deux Lien opposés).
        /// </summary>
        public static Graphe<int> afficheGrapheCommandes(MySqlConnection connexion)
        {
            // ---------- collections temporaires ----------
            var noeudsDict = new Dictionary<int, Noeud<int>>();   // id → nœud
            var liens = new List<Lien<int>>();

            /*  La requête récupère, pour chaque commande :
                - l’identifiant client  + son nom
                - l’identifiant cuisinier + son nom
               (schéma conforme aux inserts vus dans interfaceuser : tables custommer / cuisinier
                qui pointent elles‑mêmes vers utilisateur) :contentReference[oaicite:0]{index=0} */
            const string sql = @"
    SELECT  c.id_client,
            uc.Nom  AS nom_client,
            c2.id_cuisinier,
            uu.Nom  AS nom_cuisinier
    FROM commande          AS cmd
    JOIN custommer  c  ON c.id_client       = cmd.id_client
    JOIN utilisateur uc ON uc.id            = c.id
    JOIN cuisinier  c2 ON c2.id_cuisinier   = cmd.id_cuisinier
    JOIN utilisateur uu ON uu.id            = c2.id;";

            using var cmd = new MySqlCommand(sql, connexion);
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int idClient = rdr.GetInt32(0);
                string nomClient = rdr.GetString(1);
                int idCuisinier = rdr.GetInt32(2);
                string nomCuisinier = rdr.GetString(3);

                // -- nœuds : on ne garde QUE les utilisateurs impliqués dans au moins une commande
                if (!noeudsDict.TryGetValue(idClient, out var nClient))
                {
                    nClient = new Noeud<int>(idClient, nomClient, 0, 0);
                    noeudsDict[idClient] = nClient;
                }
                if (!noeudsDict.TryGetValue(idCuisinier, out var nCuisinier))
                {
                    nCuisinier = new Noeud<int>(idCuisinier, nomCuisinier, 0, 0);
                    noeudsDict[idCuisinier] = nCuisinier;
                }

                // -- une arête par commande (poids = 1) –> graphe non orienté ⇔ deux liens opposés
                liens.Add(new Lien<int>(nClient, noeudsDict[idCuisinier], 1));
                liens.Add(new Lien<int>(noeudsDict[idCuisinier], nClient, 1));
            }

            // ---------- construction du graphe ----------
            var graphe = new Graphe<int>(noeudsDict.Values.ToList(), liens);

            // ---------- coloration & export visuel ----------
            var coloration = graphe.ColorationWelshPowell();
            var visu = new Visualisation<int>(graphe, null, coloration);
            visu.DessinerCercle("graph_commandes.png");      // image dans le répertoire de l’exécutable

            Console.WriteLine("Graphe des commandes créé́.");
            return graphe;
        }





    }



}
