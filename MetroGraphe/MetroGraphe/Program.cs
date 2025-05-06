using ExcelDataReader;
using MySql.Data.MySqlClient;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace livinparis_dufourmantelle_veyrie
{
    public class Program
    {
        /// <summary>
        /// Crée un graphe représentant le réseau de métro à partir de deux fichiers Excel :
        /// - MetroParis.xlsx pour les coordonnées GPS
        /// - Liens_corrige.xlsx pour les stations et connexions
        /// </summary>
        /// <returns>Un objet Graphe<int> représentant le métro</returns>
        public static Graphe<int> creationgraphe()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var noeuds = new List<Noeud<int>>();
            var liens = new List<Lien<int>>();
            var noeudsDict = new Dictionary<int, Noeud<int>>();

            var coordonnees = new Dictionary<int, (double lat, double lon)>();
            using (var stream = File.Open("MetroParis.xlsx", FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read(); 
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader.GetValue(0));
                    double lon = Convert.ToDouble(reader.GetValue(3), CultureInfo.InvariantCulture);
                    double lat = Convert.ToDouble(reader.GetValue(4), CultureInfo.InvariantCulture);
                    coordonnees[id] = (lat, lon);
                }
            }

            using (var stream = File.Open("Liens_corrige.xlsx", FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read(); 

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader.GetDouble(0));
                    string nom = reader.GetString(1);
                    string precedentStr = reader.GetValue(2)?.ToString();
                    string suivantStr = reader.GetValue(3)?.ToString();
                    string distanceStr = reader.GetValue(4)?.ToString();
                    string ligneStr = reader.GetValue(5)?.ToString();
                    int ligne2 = Convert.ToInt32(ligneStr);

                    double latitude = coordonnees.ContainsKey(id) ? coordonnees[id].lat : 0;
                    double longitude = coordonnees.ContainsKey(id) ? coordonnees[id].lon : 0;

                    if (!noeudsDict.ContainsKey(id))
                    {
                        var noeud = new Noeud<int>(id, nom, latitude, longitude);
                        if (int.TryParse(ligneStr, out int l))
                            noeud.Lignes.Add(l);
                        noeudsDict[id] = noeud;
                        noeuds.Add(noeud);
                    }
                    else
                    {
                        if (int.TryParse(ligneStr, out int l) && !noeudsDict[id].Lignes.Contains(l))
                            noeudsDict[id].Lignes.Add(l);
                    }

                    double distance = 0;
                    if (!string.IsNullOrEmpty(distanceStr) &&
                        double.TryParse(distanceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                        distance = d;

                    if (int.TryParse(suivantStr, out int nextId))
                    {
                        if (!noeudsDict.ContainsKey(nextId))
                        {
                            var nextNode = new Noeud<int>(nextId, nom, latitude, longitude);
                            noeudsDict[nextId] = nextNode;
                            noeuds.Add(nextNode);
                        }
                        liens.Add(new Lien<int>(noeudsDict[id], noeudsDict[nextId], distance));
                        liens.Add(new Lien<int>(noeudsDict[nextId], noeudsDict[id], distance));
                    }

                    if (int.TryParse(precedentStr, out int prevId))
                    {
                        if (!noeudsDict.ContainsKey(prevId))
                        {
                            var prevNode = new Noeud<int>(prevId, nom, latitude, longitude);
                            noeudsDict[prevId] = prevNode;
                            noeuds.Add(prevNode);
                        }
                        liens.Add(new Lien<int>(noeudsDict[prevId], noeudsDict[id], distance));
                        liens.Add(new Lien<int>(noeudsDict[id], noeudsDict[prevId], distance));
                    }
                }
            }

            return new Graphe<int>(noeuds, liens);
        }

        /// <summary>
        /// Point d’entrée principal du programme.
        /// Gère la connexion à la base de données et l’affichage initial.
        /// </summary>
        static void Main()
        {
            #region Connexion MySQL
            MySqlConnection connexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=baselivinparis;" +
                                         "user=root;PASSWORD=Sabrelaser00;AllowBatch=true;";

                connexion = new MySqlConnection(connexionString);
                connexion.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur de connexion : " + e.ToString());
                return;
            }
            #endregion

            Console.Clear();
            
            var graphe = creationgraphe();
            Console.WriteLine($"Graphe initialisé : {graphe.Noeuds.Count} stations, {graphe.Liens.Count} liens");
            
            interfaceuser interface1 = new interfaceuser(connexion);


         //   affichechemincuisinier("Concorde", "Nation", graphe);
            // Ici, d'autres appels peuvent être faits si besoin
        }

        /// <summary>
        /// Calcule et affiche le chemin le plus court entre deux stations données par leur nom
        /// </summary>
        public static void affichechemincuisinier(string departstring, string arriveestring, Graphe<int> graphe)
        {
            var depart = graphe.Noeuds.FirstOrDefault(n => n.NOM == departstring);
            var arrivee = graphe.Noeuds.FirstOrDefault(n => n.NOM == arriveestring);

            if (depart == null || arrivee == null)
            {
                Console.WriteLine("Station introuvable");
                return;
            }


            var chemin = graphe.BellmanFord(depart, arrivee);

            Console.WriteLine("Chemin trouvé :");
            foreach (var station in chemin)
            {
                Console.WriteLine($"{station.ID} - {station.NOM}");
            }

            var coloration = graphe.ColorationWelshPowell();

            int nbCouleurs = coloration.Values.Max() + 1;
            bool estBiparti = nbCouleurs <= 2;
            bool estPlanaire = nbCouleurs <= 4;

            var visu = new Visualisation<int>(graphe, chemin, coloration);
            visu.Dessiner("metro_coloration.png");
            Console.WriteLine($"Nombre minimal de couleurs : {nbCouleurs}");
            Console.WriteLine($"Biparti : {estBiparti}");
            Console.WriteLine($"Planaire (test par 4-couleurs) : {estPlanaire}");
        }
    }
}
