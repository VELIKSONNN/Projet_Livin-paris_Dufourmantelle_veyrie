using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCsharplivin_paris
{
    public class VisualisationGraphe
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

     
        /// <summary>
        /// Dessine le graphe et sauvegarde l'image au chemin spécifié. Pour une raison inconnu le graph n'est pas en cercle
        /// </summary>
        /// <param name="cheminImage">Chemin du fichier image à créer.</param>
        private void DessinerGraphe(string cheminImage)
        {
            int largeur = 600, hauteur = 600; // Dimensions de l'image.

            // Création de l'image.
            Bitmap bitmap = new Bitmap(largeur, hauteur);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White); // Met un fond blanc pour l'image.

            // Dictionnaire stockant les positions des sommets.
            Dictionary<int, PointF> positions = new Dictionary<int, PointF>();

            // Rayon du cercle pour placer les sommets.
            int rayon = Math.Min(largeur, hauteur) / 3;
            PointF centre = new PointF(largeur / 2, hauteur / 2);

            // Positionnement des sommets en cercle.
            for (int i = 1; i <= this.g.Nombresommets; i++)
            {
                double angle = (2 * Math.PI * (i - 1)) / this.g.Nombresommets; // Calcul de l'angle
                float x = centre.X + (float)(rayon * Math.Cos(angle)); // Coordonnée X
                float y = centre.Y + (float)(rayon * Math.Sin(angle)); // Coordonnée Y
                positions[i] = new PointF(x, y); // Affectation de la position
            }

            // Crayon pour dessiner les arêtes.
            Pen pen = new Pen(Color.Black, 2);

            // Dessin des arêtes du graphe.
            for (int i = 1; i <= this.g.Nombresommets; i++)
            {
                foreach (int voisin in this.g.Listeadjacence[i])
                {
                    g.DrawLine(pen, positions[i], positions[voisin]); // Ligne entre les sommets connectés.
                }
            }

            // Dessin des sommets sous forme de cercles.
            Brush brush = Brushes.Blue;
            foreach (var kvp in positions)
            {
                g.FillEllipse(brush, kvp.Value.X - 5, kvp.Value.Y - 5, 10, 10); // Dessin du sommet
                g.DrawString(kvp.Key.ToString(), new Font("Arial", 10), Brushes.Black, kvp.Value); // Numéro du sommet
            }

            // Ajout du texte indiquant si le graphe est connexe.
            string texteConnexite = this.g.EstConnexe() ? "Graphe connexe: Oui" : "Graphe non connexe";
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush brush2 = Brushes.Red;
            g.DrawString(texteConnexite, font, brush2, new PointF(10, hauteur - 40));

            // Ajout du texte indiquant si le graphe contient des cycles.
            string texteCycle = this.g.ContientCycle() ? "Le graphe contient des circuits: Oui" : "Le graphe contient des circuits: Non";
            Font font2 = new Font("Arial", 12, FontStyle.Bold);
            Brush brush3 = Brushes.Green;
            g.DrawString(texteCycle, font2, brush3, new PointF(10, hauteur - 20));

            // Sauvegarde de l'image.
            bitmap.Save(cheminImage);

            // Libération des ressources graphiques.
            g.Dispose();
            bitmap.Dispose();
        }

        /// <summary>
        /// Ouvre l'image générée à l'aide de la visionneuse par défaut.
        /// </summary>
        /// <param name="cheminImage">Chemin de l'image à ouvrir.</param>
        private void OuvrirImage(string cheminImage)
        {
            if (File.Exists(cheminImage))
            {
                Console.WriteLine($"Image générée et affichée : {cheminImage}");
                Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true }); // Ouvre l'image avec la visionneuse par défaut.
            }
            else
            {
                Console.WriteLine("L'image n'a pas été créée.");
            }
        }
    }
}