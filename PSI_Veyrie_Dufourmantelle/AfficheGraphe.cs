using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// Assurez-vous d'ajouter SkiaSharp depuis NuGet
using SkiaSharp;

namespace PSI_Veyrie_Dufourmantelle
{
    public class AfficheGraphe
    {
        private graphe g; // Référence à la classe "graphe" que vous avez déjà dans votre projet

        public AfficheGraphe(graphe g)
        {
            this.g = g;
        }
        
        public void DessinerEtAfficherGraphe(string cheminImage)
        {
            DessinerGraphe(cheminImage);
            OuvrirImage(cheminImage);
        }

        /// <summary>
        /// Dessine le graphe et sauvegarde l'image au chemin spécifié
        /// en utilisant SkiaSharp.
        /// </summary>
        /// <param name="cheminImage">Chemin du fichier image à créer.</param>
        private void DessinerGraphe(string cheminImage)
        {
            int largeur = 600, hauteur = 600;

            // Création d'une surface de dessin SkiaSharp
            SKImageInfo info = new SKImageInfo(largeur, hauteur);
            using (SKSurface surface = SKSurface.Create(info))
            {
                SKCanvas canvas = surface.Canvas;

                // On efface tout en blanc
                canvas.Clear(SKColors.White);

                // Dictionnaire pour mémoriser la position (x,y) de chaque sommet
                Dictionary<int, SKPoint> positions = new Dictionary<int, SKPoint>();

                // Calcul du rayon et du centre (pour placement circulaire)
                int rayon = Math.Min(largeur, hauteur) / 3;
                SKPoint centre = new SKPoint(largeur / 2f, hauteur / 2f);

                // 1) Calculer la position de chaque sommet sur le cercle
                for (int i = 1; i <= this.g.Nombresommets; i++)
                {
                    double angle = (2 * Math.PI * (i - 1)) / this.g.Nombresommets;
                    float x = centre.X + (float)(rayon * Math.Cos(angle));
                    float y = centre.Y + (float)(rayon * Math.Sin(angle));
                    positions[i] = new SKPoint(x, y);
                }

                // 2) Dessiner les arêtes (lignes) entre les sommets connectés
                using (var linePaint = new SKPaint())
                {
                    linePaint.Color = SKColors.Black;
                    linePaint.IsStroke = true;
                    linePaint.StrokeWidth = 2f;

                    for (int i = 1; i <= this.g.Nombresommets; i++)
                    {
                        foreach (int voisin in this.g.Listeadjacence[i])
                        {
                            SKPoint start = positions[i];
                            SKPoint end = positions[voisin];
                            canvas.DrawLine(start, end, linePaint);
                        }
                    }
                }

                // 3) Dessiner chaque sommet (cercle + étiquette)
                using (var circlePaint = new SKPaint())
                {
                    circlePaint.Color = SKColors.Blue;
                    circlePaint.IsStroke = false;

                    foreach (var kvp in positions)
                    {
                        // Dessine un petit cercle
                        canvas.DrawCircle(kvp.Value, 5, circlePaint);
                    }
                }

                // 4) Dessiner les étiquettes (numéro du sommet)
                using (var textPaint = new SKPaint())
                {
                    textPaint.Color = SKColors.Black;
                    textPaint.TextSize = 12;
                    textPaint.IsAntialias = true;

                    foreach (var kvp in positions)
                    {
                        // Décaler légèrement le texte pour qu'il soit lisible
                        float textOffsetX = 5;
                        float textOffsetY = -5;
                        canvas.DrawText(kvp.Key.ToString(),
                                        kvp.Value.X + textOffsetX,
                                        kvp.Value.Y + textOffsetY,
                                        textPaint);
                    }
                }

                // 5) Afficher les informations de connexité et de cycle
                using (var infoPaint = new SKPaint())
                {
                    infoPaint.TextSize = 14;
                    infoPaint.IsAntialias = true;

                    // Connexité (en rouge)
                    infoPaint.Color = SKColors.Red;
                    string texteConnexite = g.EstConnexe()
                        ? "Graphe connexe: Oui"
                        : "Graphe non connexe";
                    canvas.DrawText(texteConnexite, 10, hauteur - 40, infoPaint);

                    // Cycle (en vert)
                    infoPaint.Color = SKColors.Green;
                    string texteCycle = g.ContientCycle()
                        ? "Le graphe contient des circuits: Oui"
                        : "Le graphe contient des circuits: Non";
                    canvas.DrawText(texteCycle, 10, hauteur - 20, infoPaint);
                }

                // 6) Sauvegarder l'image en PNG dans le chemin spécifié
                using (SKImage image = surface.Snapshot())
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (FileStream stream = File.OpenWrite(cheminImage))
                    {
                        data.SaveTo(stream);
                    }
                }
            }
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
                Process.Start(new ProcessStartInfo(cheminImage)
                {
                    UseShellExecute = true
                });
            }
            else
            {
                Console.WriteLine("L'image n'a pas été créée.");
            }
        }
    }

    // Exemples de définitions minimales pour "graphe" / "EstConnexe" / "ContientCycle"
    // à adapter selon votre propre logique.
    // ---------------------------------------
   
}
