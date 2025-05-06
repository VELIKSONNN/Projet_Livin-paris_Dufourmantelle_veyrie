using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace livinparis_dufourmantelle_veyrie
{
    public static class GrapheFactory
    {
       

        /// <summary>
        /// Construit le graphe « clients – cuisiniers » à partir de toutes les commandes.
        /// Chaque commande engendre une arête non orientée (donc deux Lien opposés).
        /// </summary>
        public static Graphe<int> CreationGrapheCommandes(MySqlConnection connexion)
        {
            // ---------- collections temporaires ----------
            var noeudsDict = new Dictionary<int, Noeud<int>>();   // id → nœud
            var liens = new List<Lien<int>>();

            const string sql = @"SELECT id_client, id_cuisinier FROM commande";
            using var cmd = new MySqlCommand(sql, connexion);
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int idClient = rdr.GetInt32(0);
                int idCuisinier = rdr.GetInt32(1);

                // -- nœuds : on ne garde QUE les utilisateurs impliqués dans au moins une commande
                if (!noeudsDict.TryGetValue(idClient, out var nClient))
                {
                    nClient = new Noeud<int>(idClient, $"Client_{idClient}", 0, 0);
                    noeudsDict[idClient] = nClient;
                }
                if (!noeudsDict.TryGetValue(idCuisinier, out var nCuisinier))
                {
                    nCuisinier = new Noeud<int>(idCuisinier, $"Cuisinier_{idCuisinier}", 0, 0);
                    noeudsDict[idCuisinier] = nCuisinier;
                }

                // -- une arête par commande  (poids = 1)  –> graphe non orienté ⇔ deux liens opposés
                liens.Add(new Lien<int>(nClient, nCuisinier, 1));
                liens.Add(new Lien<int>(nCuisinier, nClient, 1));
            }

            
            var graphe = new Graphe<int>(noeudsDict.Values.ToList(), liens); 

           
            var coloration = graphe.ColorationWelshPowell();   
            var visu = new Visualisation<int>(graphe, null, coloration);
            visu.DessinerCercle("graph_commandes.png");               
            Console.WriteLine("graphe des commandes créé");
            return graphe;
        }
    }
}