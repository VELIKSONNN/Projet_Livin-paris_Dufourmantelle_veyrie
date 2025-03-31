using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;


namespace MetroGraphe
{
    internal class Program
    {

        static void ConvertirExcelEnTexte(string fichierExcel, string fichierTexte)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(fichierExcel, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            using (var writer = new StreamWriter(fichierTexte))
            {
                bool firstRow = true;
                while (reader.Read())
                {
                    if (firstRow) // Ignore la première ligne (en-tête)
                    {
                        firstRow = false;
                        continue;
                    }

                    string id = reader.GetValue(0)?.ToString();
                    string nom = reader.GetValue(1)?.ToString();
                    string latitude = reader.GetValue(2)?.ToString();
                    string longitude = reader.GetValue(3)?.ToString();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(nom) &&
                        !string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
                    {
                        writer.WriteLine($"{id};{nom};{latitude};{longitude}");
                    }
                }
            }
           }
            static void Main(string[] args)
            {
                string fichierExcelStations = "metrostations.xlsx";
                string fichierExcelLiens = "Liens.xlsx";
                string fichierTxtStations = "stations.txt";
                string fichierTxtLiens = "liens.txt";

                // Convertir Excel en fichiers texte
                ConvertirExcelEnTexte(fichierExcelStations, fichierTxtStations);
                ConvertirExcelEnTexte(fichierExcelLiens, fichierTxtLiens);

                // Charger les fichiers texte dans le graphe
                Graphe<int> metroGraphe = new Graphe<int>();
                metroGraphe.ChargerDepuisFichiers(fichierTxtLiens);

                metroGraphe.AfficherGraphe ();
                
                Console.ReadKey();
            }
        }
    }


