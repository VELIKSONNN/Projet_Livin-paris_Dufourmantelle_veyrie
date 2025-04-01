using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExcelDataReader;
using MetroGraphe;

class Program
{
    static void Main()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var noeuds = new List<Noeud<int>>();
        var liens = new List<Lien<int>>();

        var noeudsDict = new Dictionary<int, Noeud<int>>();

        using (var stream = File.Open("Liens.xlsx", FileMode.Open, FileAccess.Read))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            reader.Read(); // Sauter l'en-tête

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader.GetDouble(0));
                string nom = reader.GetString(1);
                int ligne = Convert.ToInt32(reader.GetDouble(5)) ;


                string suivantStr = reader.GetValue(2)?.ToString();
                string precedentStr = reader.GetValue(3)?.ToString();
                string distanceStr = reader.GetValue(4)?.ToString()?.Replace("km", "").Trim();

                if (!noeudsDict.ContainsKey(id))
                {
                    var noeud = new Noeud<int>(id, nom, ligne);
                    noeudsDict[id] = noeud;
                    noeuds.Add(noeud);
                }

                double distance = 0;
                if (!string.IsNullOrEmpty(distanceStr) && double.TryParse(distanceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    distance = d;

                // Lien vers suivant
                if (int.TryParse(suivantStr, out int nextId))
                {
                    if (!noeudsDict.ContainsKey(nextId))
                    {
                        string nextNom = $"Station {nextId}";
                        var nextNode = new Noeud<int>(nextId, nextNom, ligne);
                        noeudsDict[nextId] = nextNode;
                        noeuds.Add(nextNode);
                    }
                    liens.Add(new Lien<int>(noeudsDict[id], noeudsDict[nextId], distance));
                }

                // Lien vers précédent
                if (int.TryParse(precedentStr, out int prevId))
                {
                    if (!noeudsDict.ContainsKey(prevId))
                    {
                        string prevNom = $"Station {prevId}";
                        var prevNode = new Noeud<int>(prevId, prevNom, ligne);
                        noeudsDict[prevId] = prevNode;
                        noeuds.Add(prevNode);
                    }
                    liens.Add(new Lien<int>(noeudsDict[prevId], noeudsDict[id], distance));
                }
            }
        }

        var graphe = new Graphe<int>(noeuds, liens);

        Console.WriteLine($"✅ Graphe initialisé : {graphe.Noeuds.Count} stations, {graphe.Liens.Count} liens");

        var visu = new Visualisation<int>(graphe);
        visu.Dessiner("reseau_metro.png");
    }
}
