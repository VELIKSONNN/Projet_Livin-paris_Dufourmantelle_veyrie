using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using livinparis_dufourmantelle_veyrie; 
namespace livinparis_dufourmantelle_veyrie
{
    public class Visualisation<T>
    {
        private readonly Graphe<T> _graphe;
        private readonly List<Noeud<T>> _chemin;
        Dictionary<Noeud<T>, int> coloration;

        public Visualisation(Graphe<T> graphe, List<Noeud<T>> chemin = null, Dictionary<Noeud<T>, int> coloration = null)
        {
            _graphe = graphe;
            _chemin = chemin;
            this.coloration = coloration;
        }
        public void DessinerCercle(string filePath)
        {
            const int largeur = 3000;
            const int hauteur = 3000;

            // ------------------------------------------------------------
            // 1. SET‑UP  Skia
            // ------------------------------------------------------------
            using var bitmap = new SKBitmap(largeur, hauteur);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintTexte = new SKPaint { Color = SKColors.Black, TextSize = 18, IsAntialias = true };
            var paintNoeud = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill };
            var paintLien = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 2 };
            var paintChemin = new SKPaint { Color = SKColors.Red, StrokeWidth = 4 };

            var couleurs = new[]
            {
        SKColors.Blue, SKColors.Green, SKColors.Orange, SKColors.Purple,
        SKColors.Teal, SKColors.Brown, SKColors.DarkCyan, SKColors.Goldenrod,
        SKColors.Crimson, SKColors.DarkOliveGreen, SKColors.DarkMagenta
    };

            // ------------------------------------------------------------
            // 2. CALCUL DES POSITIONS SUR UN CERCLE
            // ------------------------------------------------------------
            var positions = new Dictionary<T, SKPoint>();
            int n = _graphe.Noeuds.Count;
            if (n == 0) return;

            float centreX = largeur / 2f;
            float centreY = hauteur / 2f;
            float rayon = 0.4f * MathF.Min(largeur, hauteur); // marge de 20 %

            int index = 0;
            foreach (var noeud in _graphe.Noeuds)
            {
                double angle = 2 * Math.PI * index / n;           // répartition uniforme
                float x = centreX + rayon * (float)Math.Cos(angle);
                float y = centreY + rayon * (float)Math.Sin(angle);
                positions[noeud.ID] = new SKPoint(x, y);
                index++;
            }

            // ------------------------------------------------------------
            // 3. TRAÇAGE DES LIENS
            // ------------------------------------------------------------
            foreach (var lien in _graphe.Liens)
            {
                var p1 = positions[lien.Source.ID];
                var p2 = positions[lien.Destination.ID];
                canvas.DrawLine(p1, p2, paintLien);

                // Affiche la distance (ou poids) au milieu
                var milieu = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                if (lien.Distancesuivant > 0)
                    canvas.DrawText($"{lien.Distancesuivant:F2}", milieu.X - 10, milieu.Y + 20, paintTexte);
            }

            // ------------------------------------------------------------
            // 4. (Optionnel) CHEMIN OPTIMAL  en rouge
            // ------------------------------------------------------------
            double totalKm = 0;
            if (_chemin != null && _chemin.Count > 1)
            {
                for (int i = 0; i < _chemin.Count - 1; i++)
                {
                    var src = _chemin[i];
                    var dst = _chemin[i + 1];
                    canvas.DrawLine(positions[src.ID], positions[dst.ID], paintChemin);

                    // additionne la distance
                    var lien = _graphe.Liens.FirstOrDefault(l =>
                        EqualityComparer<T>.Default.Equals(l.Source.ID, src.ID) &&
                        EqualityComparer<T>.Default.Equals(l.Destination.ID, dst.ID));
                    if (lien != null) totalKm += lien.Distancesuivant;
                }
            }

            // ------------------------------------------------------------
            // 5. TRAÇAGE DES NŒUDS
            // ------------------------------------------------------------
            foreach (var noeud in _graphe.Noeuds)
            {
                var pos = positions[noeud.ID];

                int idxCouleur = coloration.TryGetValue(noeud, out int c) ? c : 0;
                paintNoeud.Color = couleurs[idxCouleur % couleurs.Length];

                canvas.DrawCircle(pos, 10, paintNoeud);

                if (!string.IsNullOrEmpty(noeud.NOM))
                    canvas.DrawText(noeud.NOM, pos.X + 12, pos.Y - 12, paintTexte);
            }

            // ------------------------------------------------------------
            // 6. INFO DISTANCE TOTALE
            // ------------------------------------------------------------
            if (_chemin != null && _chemin.Count > 1)
                canvas.DrawText($"Distance totale : {totalKm:F2} km", 100, hauteur - 40, paintTexte);

            // ------------------------------------------------------------
            // 7. ENREGISTREMENT & OUVERTURE
            // ------------------------------------------------------------
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var file = File.OpenWrite(filePath);
            data.SaveTo(file);

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur ouverture : {e.Message}");
            }
        }
        public void Dessiner(string filePath)
        {
            const int largeur = 3000;
            const int hauteur = 3000;

            using var bitmap = new SKBitmap(largeur, hauteur);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintTexte = new SKPaint { Color = SKColors.Black, TextSize = 18, IsAntialias = true };
            var paintNoeud = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill };
            var paintLien = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 2 };
            var paintChemin = new SKPaint { Color = SKColors.Red, StrokeWidth = 4 };

            var couleurs = new SKColor[]
            {
            SKColors.Blue, SKColors.Green, SKColors.Orange, SKColors.Purple,
            SKColors.Teal, SKColors.Brown, SKColors.DarkCyan, SKColors.Goldenrod,
            SKColors.Crimson, SKColors.DarkOliveGreen, SKColors.DarkMagenta
            };

            var ligneCouleurs = new Dictionary<int, SKColor>();
            int ligneIndex = 0;

            float minLat = (float)_graphe.Noeuds.Min(n => n.Latitude);
            float maxLat = (float)_graphe.Noeuds.Max(n => n.Latitude);
            float minLon = (float)_graphe.Noeuds.Min(n => n.Longitude);
            float maxLon = (float)_graphe.Noeuds.Max(n => n.Longitude);

            var positions = new Dictionary<T, SKPoint>();
            foreach (var noeud in _graphe.Noeuds)
            {
                float x = (float)((noeud.Longitude - minLon) / (maxLon - minLon) * (largeur - 200) + 100);
                float y = (float)((1 - (noeud.Latitude - minLat) / (maxLat - minLat)) * (hauteur - 200) + 100);
                positions[noeud.ID] = new SKPoint(x, y);
            }

            foreach (var lien in _graphe.Liens)
            {
                var p1 = positions[lien.Source.ID];
                var p2 = positions[lien.Destination.ID];
                canvas.DrawLine(p1, p2, paintLien);

                var milieu = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                if (lien.Distancesuivant > 0)
                    canvas.DrawText($"{lien.Distancesuivant:F2} km", milieu.X - 10, milieu.Y + 20, paintTexte);
            }

            double totalKm = 0;
            if (_chemin != null && _chemin.Count > 1)
            {
                for (int i = 0; i < _chemin.Count - 1; i++)
                {
                    var src = _chemin[i];
                    var dst = _chemin[i + 1];
                    var pos1 = positions[src.ID];
                    var pos2 = positions[dst.ID];
                    canvas.DrawLine(pos1, pos2, paintChemin);

                    var lien = _graphe.Liens.FirstOrDefault(l =>
                        EqualityComparer<T>.Default.Equals(l.Source.ID, src.ID) &&
                        EqualityComparer<T>.Default.Equals(l.Destination.ID, dst.ID));
                    if (lien != null)
                        totalKm += lien.Distancesuivant;
                }
            }

            
            foreach (var noeud in _graphe.Noeuds)
            {
                var pos = positions[noeud.ID];

                int idxCouleur = 0;
                if (coloration.TryGetValue(noeud, out int c))
                    idxCouleur = c;

                paintNoeud.Color = couleurs[idxCouleur % couleurs.Length];

                canvas.DrawCircle(pos, 10, paintNoeud);

                if (!string.IsNullOrEmpty(noeud.NOM))
                    canvas.DrawText(noeud.NOM, pos.X + 12, pos.Y - 12, paintTexte);
            }


            if (_chemin != null && _chemin.Count > 1)
            {
                canvas.DrawText($"Distance totale du chemin : {totalKm:F2} km", 100, hauteur - 40, paintTexte);
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(filePath);
            data.SaveTo(stream);

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur ouverture : {e.Message}");
            }
        }
    }
}