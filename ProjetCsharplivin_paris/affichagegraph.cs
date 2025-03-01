using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCsharplivin_paris
{
    public class VisualisationGraphe
    {
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

     
        /// <summary>
        /// Dessine le graphe et sauvegarde l'image au chemin spécifié. Pour une raison inconnu le graph n'est pas en cercle
        /// </summary>
        /// <param name="cheminImage">Chemin du fichier image à créer.</param>
        private void DessinerGraphe(string cheminImage)
        {
            int largeur = 600, hauteur = 600; 

         
            Bitmap bitmap = new Bitmap(largeur, hauteur);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White); 

           
            Dictionary<int, PointF> positions = new Dictionary<int, PointF>();

            
            int rayon = Math.Min(largeur, hauteur) / 3;
            PointF centre = new PointF(largeur / 2, hauteur / 2);

            
            for (int i = 1; i <= this.g.Nombresommets; i++)
            {
                double angle = (2 * Math.PI * (i - 1)) / this.g.Nombresommets; 
                float x = centre.X + (float)(rayon * Math.Cos(angle));
                float y = centre.Y + (float)(rayon * Math.Sin(angle)); 
                positions[i] = new PointF(x, y); 
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

            
            string texteConnexite = this.g.EstConnexe() ? "Graphe connexe: Oui" : "Graphe non connexe";
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush brush2 = Brushes.Red;
            g.DrawString(texteConnexite, font, brush2, new PointF(10, hauteur - 40));

           
            string texteCycle = this.g.ContientCycle() ? "Le graphe contient des circuits: Oui" : "Le graphe contient des circuits: Non";
            Font font2 = new Font("Arial", 12, FontStyle.Bold);
            Brush brush3 = Brushes.Green;
            g.DrawString(texteCycle, font2, brush3, new PointF(10, hauteur - 20));

            
            bitmap.Save(cheminImage);

            
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