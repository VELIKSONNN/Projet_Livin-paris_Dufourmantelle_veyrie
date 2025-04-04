using livinparis_dufourmantelle_veyrie;
using ExcelDataReader;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
namespace livinparis_dufourmantelle_veyrie
{
    
    class Program
    {
           public  static Graphe<int> creationgraphe()
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    var noeuds = new List<Noeud<int>>();
                    var liens = new List<Lien<int>>();

                    var noeudsDict = new Dictionary<int, Noeud<int>>();

                    // Lecture des coordonnées GPS depuis MetroParis.xlsx
                    var coordonnees = new Dictionary<int, (double lat, double lon)>();
                    using (var stream = File.Open("MetroParis.xlsx", FileMode.Open, FileAccess.Read))
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read(); // Skip header
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader.GetValue(0));
                            double lon = Convert.ToDouble(reader.GetValue(3), CultureInfo.InvariantCulture);
                            double lat = Convert.ToDouble(reader.GetValue(4), CultureInfo.InvariantCulture);
                            coordonnees[id] = (lat, lon);
                        }
                    }

                    // Lecture du fichier Liens.xlsx contenant les informations principales
                    using (var stream = File.Open("Liens_corrige.xlsx", FileMode.Open, FileAccess.Read))
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read(); // Sauter l'en-tête

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
                                if (int.TryParse(ligneStr, out int ligne))
                                    noeud.Lignes.Add(ligne);
                                noeudsDict[id] = noeud;
                                noeuds.Add(noeud);
                            }
                            else
                            {
                                if (int.TryParse(ligneStr, out int ligne) && !noeudsDict[id].Lignes.Contains(ligne))
                                    noeudsDict[id].Lignes.Add(ligne);
                            }

                            double distance = 0;
                            if (!string.IsNullOrEmpty(distanceStr) && double.TryParse(distanceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
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
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    var idToName = new Dictionary<int, string>();

                    using (var stream = File.Open("Liens_corrige.xlsx", FileMode.Open, FileAccess.Read))
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read(); // skip header
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader.GetDouble(0));
                            string nom = reader.GetString(1);

                            if (!idToName.ContainsKey(id))
                                idToName[id] = nom;
                        }
                    }





                    var graphe = new Graphe<int>(noeuds, liens);
                    return graphe;
                }
            static void Main()
            {
                #region connexion
                MySqlConnection connexion = null;
                try
                {
                    string connexionString = "SERVER=localhost;PORT=3306;" +
                                             "DATABASE=baselivinparis;" +
                                             "user=root;PASSWORD=Sabrelaser00";

                    connexion = new MySqlConnection(connexionString);
                    connexion.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(" ErreurConnexion : " + e.ToString());
                    return;
                }
                #endregion


                Console.Clear();
                interfaceuser interface1 = new interfaceuser(connexion);




            

           
            }
            public static void affichechemincuisinier(string departstring, string arriveestring,Graphe<int> graphe)
            {
                 Console.WriteLine($"✅ Graphe initialisé : {graphe.Noeuds.Count} stations, {graphe.Liens.Count} liens");

                            

                            var depart = graphe.Noeuds.FirstOrDefault(n => n.NOM == departstring);
                            var arrivee = graphe.Noeuds.FirstOrDefault(n => n.NOM == arriveestring);

<<<<<<< Updated upstream
            Console.WriteLine($"✅ Graphe initialisé : {graphe.Noeuds.Count} stations, {graphe.Liens.Count} liens");
        Console.WriteLine("quelle est la station de départ ?");
            string nomDepart = Console.ReadLine();
        Console.WriteLine("quelle est la station d'arrivée ?");
        string nomArrivee = Console.ReadLine() ;
        if (nomDepart == null || nomArrivee == null) {
            Console.WriteLine("Station de départ ou d'arrivée  introuvable");
                }
        string nomDepart2 = "Jaurès";
        string nomArrivee2 = "Duroc";
        
            var depart = graphe.Noeuds.FirstOrDefault(n => n.NOM == nomDepart);
            var arrivee = graphe.Noeuds.FirstOrDefault(n => n.NOM == nomArrivee);
        var depart2 = graphe.Noeuds.FirstOrDefault(n => n.NOM == nomDepart2);
        var arrivee2=graphe.Noeuds.FirstOrDefault(n=>n.NOM==nomDepart2);

            var chemin = graphe.BellmanFord(depart, arrivee);
        var chemin2=graphe.Dijkstra(depart2, arrivee2);
            foreach (var station in chemin)
                Console.WriteLine($"{station.ID} - {station.NOM}");
     
            var visu = new Visualisation<int>(graphe, chemin);
            visu.Dessiner("reseau_metro.png");
        }
=======
                            var chemin = graphe.BellmanFord(depart, arrivee);
                            foreach (var station in chemin)
                                Console.WriteLine($"{station.ID} - {station.NOM}");

                            var visu = new Visualisation<int>(graphe, chemin);
                            visu.Dessiner("reseau_metro.png");

            }

>>>>>>> Stashed changes
    }

}