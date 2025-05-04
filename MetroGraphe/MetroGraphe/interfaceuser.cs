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
            char choixactio =Convert.ToChar(Console.ReadLine());
            switch (choixactio)
            {
                case '1':
                Console.WriteLine(" Veuillez entrer votre mot de passe et identifiants pour vous connecter");
                    Console.Write("identifiant :");
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
                            // Remplacez identifiants et motDePasse par vos variables réelles
                            command.Parameters.AddWithValue("@nom", idutil);
                            command.Parameters.AddWithValue("@mdp", mdp);


                        int count = Convert.ToInt32(command.ExecuteScalar()); // récupère le nombre de lignes

                        if (count ==1)
                            {
                                Console.Clear();
                                Console.WriteLine("Authentification réussie !");
                                interface_utilisateur(idutil,mdp);

                            }
                            else
                            {
                                Console.WriteLine("Identifiant ou mot de passe incorrecte :/");
                                MainInterface();
                            }

                        
                    }
                    
                    break;

                case '2': Console.WriteLine("--Veuillez entrer votre mot de passe et identifiants admin pour vous connecter--");
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

                  

                    ajoututilisateur();
                    break;

            }
            
            
            
        }
        static public void interface_utilisateur(string idutil,string mdp)
        {
            Console.Clear ();
            Console.WriteLine("Que souhaitez vous faire ? \n--Afficher le plus court trajet pour l'une de vos commandes '1'--\n--Faire une nouvelle commandes '2'--\n --Voir les statistiques de votre compte ?'3'--");
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
                    string query = @"SELECT id_client as int_client 
                                    FROM custommer cust 
                                    JOIN utilisateur u on cust.id_client=u.id

                                    HERE u.nom=@nom AND u.mdp=@mdp";


                    using (var cmd = new MySqlCommand(query, connexion))
                    {
                        cmd.Parameters.AddWithValue("@nom", idutil);
                        cmd.Parameters.AddWithValue("@mdp", mdp);


                        using (var reader = cmd.ExecuteReader())
                        {
                           

                        
                            
                                
                                int int_client = reader.GetInt32("int_client");

                               ajoutcommande(int_client, idcuisinier);
                            
                            
                        }

                    }
                    break;
                case '3':
                    break;


            }      

        }
        public static void affiche_commandes_util(string idutil,string mdp)
        {
             string query = @"SELECT
                              cmd.`commande`               AS idCommande,
                              p.`nom`                      AS platLigne,
                              l.`date_heure_livraison`     AS dateLivraison,
                              CONCAT(cu.`Prenom`, ' ', cu.`Nom`) AS cuisinier,
                              CONCAT(cl.`Prenom`, ' ', cl.`Nom`) AS client
                            FROM `utilisateur` AS cl
                              JOIN `custommer`      AS csm ON csm.`id`               = cl.`id`
                              JOIN `commande`       AS cmd ON cmd.`id_client`        = csm.`id_client`
                              JOIN `cuisinier`      AS cs  ON cs.`id_cuisinier`      = cmd.`id_cuisinier`
                              JOIN `utilisateur`    AS cu  ON cu.`id`                = cs.`id`
                              JOIN `ligne_de_commande_` AS ldc ON ldc.`commande`     = cmd.`commande`
                              JOIN `livraison`      AS l   ON l.`id_livraison`       = ldc.`id_livraison`
                              JOIN `inclue`         AS inc ON inc.`id_ligne_de_commande` = ldc.`id_ligne_de_commande`
                              JOIN `plat`           AS p   ON p.`id`                = inc.`id`
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

            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande)\nSupprimer un tuple '2'\nExectuer une query(stats) '3'\nSimplement afficher une table '4' \nAfficher directement un chemin le plus court pour une livraison? '5'\nSortir du programme '6'");


            char actionprimaire = Convert.ToChar(Console.ReadLine());

            switch (actionprimaire)
            {
                case '1':
                    ajouttuple();
                    break;
                case '2':
                    supprselection();
                    break;
                case '3':
                    statistiques.choisirstats(connexion);
                    break;
                case '4':
                    Console.WriteLine("Quelle est la table à afficher");
                    string table = Console.ReadLine();
                    AfficherTable(table);
                    break;
                case '5':
                    Console.WriteLine("quelle est l'id de la commande dont vous voulez calcule le plus court chemins ? ");
                    int idcommande = int.Parse(Console.ReadLine());
                    recupdépartarrivé(idcommande);
                    break;
                case '6':
                    Console.Clear();
                    Console.WriteLine("Merci d'avoir utilisé livinParis et à très bientôt !!");
                    break;
            }
        }

        /// <summary>
        /// Récupère les adresses du cuisinier et du client pour une commande donnée et affiche le chemin le plus court.
        /// </summary>
        /// <param name="idcommande">L'identifiant de la commande.</param>
        static void recupdépartarrivé(int idcommande)
        {
            string query = @"
        SELECT 
            ucl.adresse AS adresseCuisinier,
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

                        // Appel de la méthode pour calculer et afficher le chemin le plus court
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
        /// Gère l'ajout d'un tuple dans la base de données selon le type d'entité choisi par l'utilisateur.
        /// </summary>
        /// 
        static private void ajoututilisateur() 
        {
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
            Console.WriteLine("nouvelle utilisateur ajouté");

            MainInterface();
        }
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

        static void ajoutcommande(int idclient, int idcuisinier)
        {
           

            

            // Générer un nouvel ID de commande (supposant que la PK de commande s'appelle "commande")
            int commande = maxindice("commande", "commande") + 1;

            using (var transaction = connexion.BeginTransaction())
            {
                // 1) Insertion dans la table commande
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

                // 2) Demander le nombre de lignes de commande pour cette commande
                Console.Write("Combien de lignes de commande pour cette commande ? ");
                int nbLignes = int.Parse(Console.ReadLine());

                for (int i = 0; i < nbLignes; i++)
                {
                    Console.WriteLine($"\n=== Création de la ligne de commande n°{i + 1} ===");

                    // Calculer un nouvel id_ligne_de_commande
                    int idmaxligne = maxindice("ligne_de_commande_", "id_ligne_de_commande") + 1;

                    // Demander la date de livraison pour cette ligne
                    Console.Write("Entrez la date de livraison (yyyy-MM-dd HH:mm:ss) : ");
                    string dateLivraisonStr = Console.ReadLine();
                    DateTime dateLivraison;
                    if (!DateTime.TryParse(dateLivraisonStr, out dateLivraison))
                    {
                        Console.WriteLine("Date invalide, utilisation de DateTime.Now.");
                        dateLivraison = DateTime.Now;
                    }
                    int idmaxlivraison = maxindice("livraison", "id_livraison") + 1;
                    // Insertion dans la table livraison
                    string insertLivraisonQuery = @"
                INSERT INTO livraison (id_livraison, date_heure_livraison)
                VALUES (@id_livraison, @dateLivraison)";
                    using (MySqlCommand cmdLivraison = new MySqlCommand(insertLivraisonQuery, connexion, transaction))
                    {
                        cmdLivraison.Parameters.AddWithValue("@id_livraison", idmaxlivraison);
                        cmdLivraison.Parameters.AddWithValue("@dateLivraison", dateLivraison);
                        cmdLivraison.ExecuteNonQuery();
                    }

                    // Insertion dans la table ligne_de_commande_
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

                    // 3) Demander le nombre de plats pour cette ligne
                    Console.Write("Combien de plats pour cette ligne ? ");
                    int nbPlats = int.Parse(Console.ReadLine());
                    for (int j = 0; j < nbPlats; j++)
                    {
                        Console.Write($"ID du plat n°{j + 1} : ");
                        int idPlat = int.Parse(Console.ReadLine());

                        // Insertion dans la table inclue (association plat - ligne_de_commande_)
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

                // 4) Calculer et afficher le prix total de la commande
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
                    //string attribut = rechercheattribut();
                    string table = recherchetable();
                    AfficherTable(table);
                    supprtuple(table);
                    break;

                case '2':
                    Console.WriteLine();
                    // attribut = rechercheattribut();
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
                    // Jointure pour récupérer Nom, Prénom...
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
                    // Jointure pour récupérer Nom, Prénom...
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
                    // Cas par défaut : demande d'un attribut à afficher
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
            adminInterface();
        }

        /// <summary>
        /// Supprime un tuple d'une table spécifiée en se basant sur son identifiant.
        /// </summary>
        /// <param name="table">Le nom de la table depuis laquelle supprimer le tuple.</param>
        static void supprtuple(string table)
        {
            Console.WriteLine();
            Console.WriteLine("Quel est l'id du tuple à supprimer ?");

            // S'assurer que les contraintes de clé étrangère sont présentes pour le DELETE en cascade
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
            // Requête pour vérifier si la contrainte existe déjà
            string checkQuery = @"SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = @databaseName 
            AND TABLE_NAME = @tableName AND CONSTRAINT_NAME = @constraintName;";

            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, connexion))
            {
                checkCmd.Parameters.AddWithValue("@databaseName", databaseName);
                checkCmd.Parameters.AddWithValue("@tableName", tableName);
                checkCmd.Parameters.AddWithValue("@constraintName", constraintName);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                // Si la contrainte existe, la supprimer
                if (count > 0)
                {
                    string dropQuery = $"ALTER TABLE {tableName} DROP FOREIGN KEY {constraintName};";
                    using (MySqlCommand dropCmd = new MySqlCommand(dropQuery, connexion))
                    {
                        dropCmd.ExecuteNonQuery();
                    }
                }
            }

            // Ajouter la contrainte avec ON DELETE CASCADE
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
    }
}
