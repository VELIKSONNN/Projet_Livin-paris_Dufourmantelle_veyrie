


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using PROJET_étudiant;

class VisualisationGraphe
{
    private graphe _graphe;
    private graphe g;
    public VisualisationGraphe(graphe g)
    {
   

        this.g = g;
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

        // Générer des positions aléatoires pour chaque sommet
        for (int i = 1; i <= this.g.Nombresommets; i++)
        {
            positions[i] = new PointF(rand.Next(50, largeur - 50), rand.Next(50, hauteur - 50));
        }

        
        Pen pen = new Pen(Color.Black, 2);
        for (int i = 1; i <= this.g.Nombresommets; i++)
        {
            foreach (int voisin in this.g.Listeadjacence[i])
            {
                g.DrawLine(pen, positions[i], positions[voisin]);
            }
        }

    
        Brush brush = Brushes.Blue;
        foreach (var kvp in positions)
        {
            g.FillEllipse(brush, kvp.Value.X - 5, kvp.Value.Y - 5, 10, 10);
            g.DrawString(kvp.Key.ToString(), new Font("Arial", 10), Brushes.Black, kvp.Value);
        }
        string texteConnexite = this.g.EstConnexe() ? "Graphe connexe: Oui" : "Graphe non connexe";
        Font font = new Font("Arial", 12, FontStyle.Bold);
        Brush brush2 = Brushes.Red;
        g.DrawString(texteConnexite, font, brush, new PointF(10, hauteur - 40));

        bitmap.Save(cheminImage);
        g.Dispose();
        bitmap.Dispose();
    }

    private void OuvrirImage(string cheminImage)
    {
        if (File.Exists(cheminImage))
        {
            Console.WriteLine($" Image générée et affichée : {cheminImage}");
            Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true });
        }
        else
        {
            Console.WriteLine(" L'image n'a pas été créée.");
        }
    }
}
