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
namespace livinparis_dufourmantelle_veyrie
{
    public class interfaceuser
    {
        static public MySqlConnection connexion;
        public MySqlConnection Connexion { get; set; }

        public interfaceuser(MySqlConnection _connexion)
        {
            connexion = _connexion;

            MainInterface();
        }

        static public void MainInterface()
        {
            Console.WriteLine("Bienvenue !\n Veuillez entrer votre mot de passe et identifiants pour vous connecter ");
            Console.Write("identifiant: ");
            string identifiant = Console.ReadLine();
            Console.Write("Mot de passe: ");
            string motdepasse = Console.ReadLine();

            if (identifiant == "" && motdepasse == "")
            {
                adminInterface();
            }
        }

        static private void adminInterface()
        {
            Console.Clear();
            Console.WriteLine("Ouverture de l'interface admin");
<<<<<<< Updated upstream
            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande), supprimer un tuple '2', ou exectuer une query(stats) '3', simplement afficher une table '4' ou afficher directement un chemin le plus court pour une livraison? '5' ");
=======
            Console.WriteLine("Que voulez vous faire ?\n Ajouter un tuple '1' (commande), supprimer un tuple '2', ou exectuer une query(stats) '3', simplement afficher une table '4' ou afficher directement un chemin le plus court pour une livraison?");
>>>>>>> Stashed changes
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
                    string table=Console.ReadLine();
                    AfficherTable(table);
                    break;
                case '5':
                    Console.WriteLine("quelle est l'id de la commande dont vous voulez calcule le plus court chemins ? ");
                    int idcommande=int.Parse(Console.ReadLine());
                    recupdépartarrivé( idcommande);
                break;
            }
        }

        
       static void recupdépartarrivé(int idcommande)
        {
            string query = @"
        SELECT 
            uC.adresse AS adresseCuisinier,
            uCl.adresse AS adresseClient
        FROM commande co
        JOIN cuisinier c ON co.id_cuisinier = c.id_cuisinier
        JOIN utilisateur uC ON c.id = uC.id
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

                        // Appel de ta méthode pour calculer le chemin le plus court
                        Program.affichechemincuisinier(adresseCuisinier, adresseClient, Program.creationgraphe());
                    }
                    else
                    {
                        Console.WriteLine("Aucune commande trouvée avec cet ID.");
                    }
                }
            }
        }
        static int maxindice(string table,string primarykey)
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


        static private void ajouttuple()
        {
            Console.Clear();
            Console.WriteLine("Voulez-vous ajouter un utilisateur '1', un cuisinier '2' ,un client '3' ou une commande '4' ?");
            char ajoutelement = Convert.ToChar(Console.ReadLine());
            switch (ajoutelement)
            {
                case '1':
                    Console.WriteLine("Veuillez fournir dans l'odre:Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise,le nom,et le mdp");
                    string Prenom = Convert.ToString(Console.ReadLine());

                    string email = Convert.ToString(Console.ReadLine());
                    int id = maxindice("utilisateur","id") + 1;
                    string tel = Convert.ToString(Console.ReadLine());
                    string adresse = Convert.ToString(Console.ReadLine());
                    string entreprise = Convert.ToString(Console.ReadLine());
                    string Nom = Convert.ToString(Console.ReadLine());
                    string mdp = Convert.ToString(Console.ReadLine());

                    using (var transaction = connexion.BeginTransaction())
                    {
                        string insertQuery = @"
                            INSERT INTO utilisateur (id, Prenom, email, tel, adresse, entreprise, Nom, mdp)
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

                        transaction.Commit(); // Valide toutes les requêtes de la transaction
                    }


                    AfficherTable("utilisateur");

                    break;



                case '2':
                    Console.WriteLine("Veuillez fournir dans l'odre:Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise,le nom,et le mdp");
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
                        // Exécution de INSERT, UPDATE, DELETE...
                        // ...
                                string insertQuery = @"
                                INSERT INTO utilisateur (id, Prenom, email, tel, adresse, entreprise, Nom, mdp)
                                VALUES (@id, @Prenom, @Email, @Tel, @Adresse, @Entreprise, @Nom, @Mdp)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connexion))
                    {
                        // Associer les paramètres C# aux placeholders @... dans la requête
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
                        string insertQuery2 = @"INSERT INTO cuisinier(id_client, id) VALUES (@id,@id)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery2, connexion))
                    {
                        command.Parameters.AddWithValue("@id_client", id);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                    AfficherTable("custommer");

                  int id_cuisinier = maxindice("cuisinier", "id_cuisinier");
                    insertQuery = $"INSERT INTO cuisinier(id_client, id) VALUES (@id,@id))";
                        transaction.Commit(); // Sans ceci, les changements restent en suspens.
                    }
                   
                    AfficherTable("cuisinier");
                    break;
                case '3':
                    
                    Console.WriteLine("Veuillez fournir dans l'odre:Le prenom, l'email, le numéro de tel, l'adresse, l'entreprise,le nom,et le mdp");
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
                     insertQuery = @"INSERT INTO custommer(id_client, id) VALUES (@id,@id)";
                    using(MySqlCommand  command= new MySqlCommand(insertQuery , connexion))
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
                            int idmaxlivraison = maxindice("livraison", "id_livraison")+1;
                            // Insertion dans la table livraison et récupération de l'ID généré
                            string insertLivraisonQuery = @"
                INSERT INTO livraison (id_livraison,date_heure_livraison)
                VALUES (@id_livraison,@dateLivraison)";
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

                        transaction.Commit();
                        recupdépartarrivé(commande);
                        Console.WriteLine("Transaction commitée.");
                    }
                    
                    break;


            }


        }
        static private void supprselection()
        {
            Console.Clear();
            Console.WriteLine("Pour supprimer un tuple vous pouvez effectuer une recherche par par attributs pour connaitre l'id du tuple à supprimer ou donner directement son id");
            Console.WriteLine("faire une recherche '1' ou supprimer directement avec l'id '2'");
            char actionsuppr = Convert.ToChar(Console.ReadLine());
            switch (actionsuppr)
            {
                case '1':

                    //string attribut = rechercheattribut();
                    string table = recherchetable();
                    AfficherTable(table);
/*
                    string query = $"SELECT id,{attribut} FROM {table}";
                    using (MySqlCommand command1 = new MySqlCommand(query, connexion)) 
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id"]} {attribut}: {reader[attribut]}");
                        }
                    }
*/
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
                    // Exécuter la requête et afficher
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
                    // Exécuter la requête et afficher
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
                    // Table qui a déjà ses champs
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

                // etc. pour d'autres tables
                default:
                    // Cas par défaut, si la table a bien un champ "id" + autre attribut
                    Console.Write("Quel attribut veux-tu voir ? ");
                    string attribut = Console.ReadLine();
                    query = $"SELECT id, {attribut} FROM {table}";
                     // Exécuter la requête et afficher
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

        static void supprtuple(string table)
        {
            
            Console.WriteLine();
            Console.WriteLine("quel est l'id du tuple à supprimer ?");
            /*string checkConstraintQuery = @" SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS  WHERE CONSTRAINT_SCHEMA = 'baselivinparis'
                                            AND TABLE_NAME = 'cuisinier' AND CONSTRAINT_NAME = 'cuisinier_ibfk_1';";

            using (MySqlCommand checkCmd = new MySqlCommand(checkConstraintQuery, connexion))
            {
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    // La contrainte existe, donc on la supprime.
                    string dropQuery = "ALTER TABLE cuisinier DROP FOREIGN KEY cuisinier_ibfk_1;";
                    using (MySqlCommand dropCmd = new MySqlCommand(dropQuery, connexion))
                    {
                        dropCmd.ExecuteNonQuery();

                    }
                }


                string addQuery = @"ALTER TABLE cuisinier ADD CONSTRAINT cuisinier_ibfk_1 FOREIGN KEY (id) REFERENCES utilisateur(id) ON DELETE CASCADE;";
                using (MySqlCommand addCmd = new MySqlCommand(addQuery, connexion))
                {
                    addCmd.ExecuteNonQuery();
                    Console.WriteLine("Contrainte ajoutée avec ON DELETE CASCADE.");
                }
            }
            */
            // Pour cuisinier -> utilisateur
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "cuisinier", "cuisinier_ibfk_1", "id", "utilisateur", "id");

            // Pour custommer -> utilisateur
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "custommer", "custommer_ibfk_1", "id", "utilisateur", "id");

            // Pour commande -> custommer
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "commande", "commande_ibfk_1", "id_client", "custommer", "id_client");

            // Pour commande -> cuisinier
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "commande", "commande_ibfk_2", "id_cuisinier", "cuisinier", "id_cuisinier");

            // Pour avis -> commande
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "avis", "avis_ibfk_1", "commande", "commande", "commande");

            // Pour contient -> plat
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "contient", "contient_ibfk_1", "id", "plat", "id");

            // Pour contient -> ingrédients
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "contient", "contient_ibfk_2", "id_ingr", "ingrédients", "id_ingr");

            // Pour inclue -> plat
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "inclue", "inclue_ibfk_1", "id", "plat", "id");

            // Pour inclue -> ligne_de_commande_
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "inclue", "inclue_ibfk_2", "id_ligne_de_commande", "ligne_de_commande_", "id_ligne_de_commande");

            // Pour ingrédients -> pays
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ingrédients", "ingrédients_ibfk_1", "idpays", "pays", "idpays");

            // Pour ligne_de_commande_ -> livraison
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_1", "id_livraison", "livraison", "id_livraison");

            // Pour ligne_de_commande_ -> custommer
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_2", "id_client", "custommer", "id_client");

            // Pour ligne_de_commande_ -> commande
            EnsureForeignKeyConstraint(connexion, "baselivinparis", "ligne_de_commande_", "ligne_de_commande__ibfk_3", "commande", "commande", "commande");

            // Pour plat -> pays
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
            Console.WriteLine(" élément supprimer !");
            
        }
        public static void EnsureForeignKeyConstraint(MySqlConnection connexion,string databaseName,string tableName,string constraintName,string foreignKeyColumn,string referencedTable,string referencedColumn)
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

                // Si la contrainte existe, on la supprime
                if (count > 0)
                {
                    string dropQuery = $"ALTER TABLE {tableName} DROP FOREIGN KEY {constraintName};";
                    using (MySqlCommand dropCmd = new MySqlCommand(dropQuery, connexion))
                    {
                        dropCmd.ExecuteNonQuery();
                        //Console.WriteLine($"Contrainte {constraintName} supprimée de la table {tableName}.");
                    }
                }
            }

            // On ajoute ensuite la contrainte avec ON DELETE CASCADE
                        string addQuery = $@"
                    ALTER TABLE {tableName} 
                    ADD CONSTRAINT {constraintName} 
                    FOREIGN KEY ({foreignKeyColumn}) 
                    REFERENCES {referencedTable}({referencedColumn}) 
                    ON DELETE CASCADE; ";

            using (MySqlCommand addCmd = new MySqlCommand(addQuery, connexion))
            {
                addCmd.ExecuteNonQuery();
               // Console.WriteLine($"Contrainte {constraintName} ajoutée à la table {tableName} avec ON DELETE CASCADE.");
            }
        }

        
        static string rechercheattribut()
        {
                Console.WriteLine("Par quels atribut souhaiter vous faire une recherche");
                string attributderecherche = Console.ReadLine();
               
              
                
                return attributderecherche;
        }
        static string recherchetable()
        {
            Console.WriteLine("dans quelle table souhaiter vous faire une recherche");
            string tablederecherche = Console.ReadLine();
            
            return tablederecherche;
        }


    }
 }
