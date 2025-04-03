using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using MetroGraphe;

class Program
{
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var noeuds = new List<Noeud<int>>();
        var noeudsDict = new Dictionary<int, Noeud<int>>();
        var liens = new List<Lien<int>>();

        // 1️⃣ Lire tous les nœuds avec leurs vrais noms et lignes
        using (var stream = File.Open("Liens_corrige.xlsx", FileMode.Open, FileAccess.Read))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            reader.Read(); // sauter l'en-tête

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader.GetDouble(0));
                string nom = reader.GetString(1)?.Trim();
                string ligneStr = reader.GetValue(5)?.ToString();
                int ligne = int.TryParse(ligneStr, out var l) ? l : 0;

                if (!noeudsDict.ContainsKey(id))
                {
                    var noeud = new Noeud<int>(id, nom, ligne);
                    noeuds.Add(noeud);
                    noeudsDict[id] = noeud;
                }
            }
        }

        // 2️⃣ Lire les liens en réutilisant les nœuds existants
        using (var stream = File.Open("Liens_corrige.xlsx", FileMode.Open, FileAccess.Read))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            reader.Read(); // sauter l'en-tête

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader.GetDouble(0));
                string precedentStr = reader.GetValue(2)?.ToString();
                string suivantStr = reader.GetValue(3)?.ToString();
                string distanceStr = reader.GetValue(4)?.ToString();

                double distance = 0;
                if (!string.IsNullOrEmpty(distanceStr) &&
                    double.TryParse(distanceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                {
                    distance = d;
                }

                if (int.TryParse(suivantStr, out int nextId))
                {
                    if (noeudsDict.ContainsKey(id) && noeudsDict.ContainsKey(nextId))
                        liens.Add(new Lien<int>(noeudsDict[id], noeudsDict[nextId], distance));
                    liens.Add(new Lien<int>(noeudsDict[nextId], noeudsDict[id], distance));

                }

                if (int.TryParse(precedentStr, out int prevId))
                {
                    if (noeudsDict.ContainsKey(prevId) && noeudsDict.ContainsKey(id))
                        liens.Add(new Lien<int>(noeudsDict[prevId], noeudsDict[id], distance));
                    liens.Add(new Lien<int>(noeudsDict[id], noeudsDict[prevId], distance));
                }
            }
        }

        // 3️⃣ Création du graphe
        var graphe = new Graphe<int>(noeuds, liens);
        Console.WriteLine($"✅ Graphe créé avec {graphe.Noeuds.Count} stations et {graphe.Liens.Count} connexions.");

        // 🔍 Test affichage
        Console.WriteLine("quelle est la station de départ du cuisinier");
        string nomDepart = Console.ReadLine();
        Console.WriteLine("quelle est la station du client ?");
        string nomArrivee = Console.ReadLine();

        var depart = graphe.Noeuds.FirstOrDefault(n => n.NOM == nomDepart);
        var arrivee = graphe.Noeuds.FirstOrDefault(n => n.NOM == nomArrivee);

        if (depart == null || arrivee == null)
        {
            Console.WriteLine("❌ Station de départ ou d’arrivée introuvable.");
            return;
        }
        Console.WriteLine(" chemin le plus court algo de Djikstra");
        var chemin = graphe.Djikstra(depart, arrivee);
        Console.WriteLine("📍 Chemin le plus court :");
        foreach (var station in chemin)
            Console.WriteLine($"{station.ID} - {station.NOM}");
        Console.WriteLine("chemin le plus court algo de Bellman-Ford");
        var cheminBellman = graphe.BellmanFord(depart, arrivee);
        foreach (var station in cheminBellman)
            Console.WriteLine($"{station.ID} - {station.NOM}");

        var visu = new Visualisation<int>(graphe,chemin);
          visu.Dessiner("reseau_metro.png");
    }
}
