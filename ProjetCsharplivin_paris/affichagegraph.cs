using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using PROJET_étudiant;

class VisualisationGraphe
{
    private graphe g; // Instance du graphe à visualiser

    // Constructeur prenant un objet graphe
    public VisualisationGraphe(graphe g)
    {
        this.g = g;
    }

    /// Méthode principale qui dessine et affiche le graphe
    public void DessinerEtAfficherGraphe(string cheminImage)
    {
        DessinerGraphe(cheminImage); ///Permet de  dessiner le graphe et enregistre l'image
        OuvrirImage(cheminImage); /// Ouvre l'image générée 
    }

   /// Méthode qui permet de dessiner le graphe et sauvegarde l'image
    private void DessinerGraphe(string cheminImage)
    {
        int largeur = 600, hauteur = 600; ///  Ce sont Dimensions de l'image
        Bitmap bitmap = new Bitmap(largeur, hauteur); /// Création de l'image
        Graphics g = Graphics.FromImage(bitmap);
        g.Clear(Color.White); /// On met un fond blanc pour l'image

        Dictionary<int, PointF> positions = new Dictionary<int, PointF>(); /// Stocke la position des sommets
        Random rand = new Random(); /// Générateur de positions aléatoires

        /// Génération de positions aléatoires pour chaque sommet du graphe
        for (int i = 1; i <= this.g.Nombresommets; i++)
        {
            positions[i] = new PointF(rand.Next(50, largeur - 50), rand.Next(50, hauteur - 50));
        }

        Pen pen = new Pen(Color.Black, 2); /// Crayon pour dessiner les liens (arêtes)

        /// Dessin des arêtes du graphe
        for (int i = 1; i <= this.g.Nombresommets; i++)
        {
            foreach (int voisin in this.g.Listeadjacence[i])
            {
                g.DrawLine(pen, positions[i], positions[voisin]); /// Dessine une ligne entre deux sommets connectés
            }
        }

        Brush brush = Brushes.Blue; /// Couleur des sommets
        foreach (var kvp in positions)
        {
            g.FillEllipse(brush, kvp.Value.X - 5, kvp.Value.Y - 5, 10, 10); /// Dessine un cercle pour chaque sommet
            g.DrawString(kvp.Key.ToString(), new Font("Arial", 10), Brushes.Black, kvp.Value); /// Affiche l'identifiant du sommet
        }

        /// Ajout du texte pour indiquer si le graphe est connexe
        string texteConnexite = this.g.EstConnexe() ? "Graphe connexe: Oui" : "Graphe non connexe";
        Font font = new Font("Arial", 12, FontStyle.Bold);
        Brush brush2 = Brushes.Red;
        g.DrawString(texteConnexite, font, brush2, new PointF(10, hauteur - 40));

        // Ajout du texte pour indiquer si le graphe contient des cycles
        string texteCycle = this.g.ContientCycle() ? "Le graphe contient des circuits: Oui" : "Le graphe contient des circuits: Non";
        Font font2 = new Font("Arial", 12, FontStyle.Bold);
        Brush brush3 = Brushes.Green;
        g.DrawString(texteCycle, font2, brush3, new PointF(10, hauteur - 20));

        // Sauvegarde de l'image
        bitmap.Save(cheminImage);

        // Libération des ressources graphiques
        g.Dispose();
        bitmap.Dispose();
    }

    // Méthode pour ouvrir l'image générée
    private void OuvrirImage(string cheminImage)
    {
        if (File.Exists(cheminImage))
        {
            Console.WriteLine($"Image générée et affichée : {cheminImage}");
            Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true }); // Ouvre l'image avec la visionneuse par défaut
        }
        else
        {
            Console.WriteLine("L'image n'a pas été créée.");
        }
    }
}
