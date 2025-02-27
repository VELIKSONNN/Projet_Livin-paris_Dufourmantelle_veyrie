
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;

class GrapheVisualizer
{
    private List<Tuple<int, int>> liens;

    public GrapheVisualizer(List<Tuple<int, int>> liens)
    {
        this.liens = liens;
    }

    public void DessinerEtAfficherGraphe(string cheminImage)
    {
        DessinerGraphe(cheminImage);
        OuvrirImage(cheminImage);
    }

    private void DessinerGraphe(string cheminImage)
    {
        int largeur = 600, hauteur = 600;
        Bitmap bitmap = new Bitmap(largeur, hauteur);
        Graphics g = Graphics.FromImage(bitmap);
        g.Clear(Color.White);

        Dictionary<int, PointF> positions = new Dictionary<int, PointF>();
        Random rand = new Random();

        // Placer les sommets aléatoirement
        foreach (var lien in liens)
        {
            if (!positions.ContainsKey(lien.Item1))
                positions[lien.Item1] = new PointF(rand.Next(50, largeur - 50), rand.Next(50, hauteur - 50));

            if (!positions.ContainsKey(lien.Item2))
                positions[lien.Item2] = new PointF(rand.Next(50, largeur - 50), rand.Next(50, hauteur - 50));
        }

        // Dessiner les liens (traits entre sommets)
        Pen pen = new Pen(Color.Black, 2);
        foreach (var lien in liens)
        {
            g.DrawLine(pen, positions[lien.Item1], positions[lien.Item2]);
        }

        // Dessiner les sommets (cercles bleus)
        foreach (var kvp in positions)
        {
            g.FillEllipse(Brushes.Blue, kvp.Value.X - 5, kvp.Value.Y - 5, 10, 10);
            g.DrawString(kvp.Key.ToString(), new Font("Arial", 10), Brushes.Black, kvp.Value);
        }

        // Sauvegarder l'image
        bitmap.Save(cheminImage);
        g.Dispose();
        bitmap.Dispose();
    }

    private void OuvrirImage(string cheminImage)
    {
        if (File.Exists(cheminImage))
        {
            Console.WriteLine($"✅ Image générée et affichée : {cheminImage}");
            Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true });
        }
        else
        {
            Console.WriteLine("❌ ERREUR : L'image n'a pas été créée.");
        }
    }
}




