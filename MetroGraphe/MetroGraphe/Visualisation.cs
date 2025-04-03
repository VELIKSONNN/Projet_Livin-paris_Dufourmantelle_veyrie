using MetroGraphe;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Visualisation<T>
{
    private readonly Graphe<T> _graphe;
    private readonly List<Noeud<T>> _chemin;

    public Visualisation(Graphe<T> graphe, List<Noeud<T>> chemin = null)
    {
        _graphe = graphe;
        _chemin = chemin;
    }

    public void Dessiner(string filePath)
    {
        const int largeur = 3000;
        const int hauteur = 3000;
        const int rayon = 10;

        using var bitmap = new SKBitmap(largeur, hauteur);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var paintTexte = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 18,
            IsAntialias = true
        };

        var paintNoeud = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        var paintLien = new SKPaint
        {
            Color = SKColors.LightGray,
            StrokeWidth = 2
        };

        var paintChemin = new SKPaint
        {
            Color = SKColors.Red,
            StrokeWidth = 4
        };

        var couleurs = new SKColor[]
        {
            SKColors.Blue, SKColors.Green, SKColors.Orange, SKColors.Purple,
            SKColors.Teal, SKColors.Brown, SKColors.DarkCyan, SKColors.Goldenrod,
            SKColors.Crimson, SKColors.DarkOliveGreen, SKColors.DarkMagenta
        };

        var ligneCouleurs = new Dictionary<int, SKColor>();
        int ligneIndex = 0;

        // Positionner les nœuds en grille
        var positions = new Dictionary<T, SKPoint>();
        int cols = (int)Math.Sqrt(_graphe.Noeuds.Count);
        int spacing = 150;
        int index = 0;

        foreach (var noeud in _graphe.Noeuds)
        {
            int row = index / cols;
            int col = index % cols;
            float x = col * spacing + 100;
            float y = row * spacing + 100;
            positions[noeud.ID] = new SKPoint(x, y);
            index++;
        }

        // Tracer les liens
        foreach (var lien in _graphe.Liens)
        {
            var p1 = positions[lien.Source.ID];
            var p2 = positions[lien.Destination.ID];
            canvas.DrawLine(p1, p2, paintLien);

            // Distance au milieu
            var milieu = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            if (lien.Distancesuivant > 0)
            {
                canvas.DrawText($"{lien.Distancesuivant} km", milieu.X - 15, milieu.Y + 20, paintTexte);
            }
        }

        // Tracer le chemin s'il existe
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

        // Tracer les nœuds
        foreach (var noeud in _graphe.Noeuds)
        {
            var pos = positions[noeud.ID];

            SKColor couleur = SKColors.Black;
            if (noeud.Lignes.Count > 0)
            {
                int ligne = noeud.Lignes.First();
                if (!ligneCouleurs.ContainsKey(ligne))
                {
                    ligneCouleurs[ligne] = couleurs[ligneIndex % couleurs.Length];
                    ligneIndex++;
                }
                couleur = ligneCouleurs[ligne];
            }

            paintNoeud.Color = couleur;
            canvas.DrawCircle(pos, rayon, paintNoeud);

            if (!string.IsNullOrWhiteSpace(noeud.NOM))
                canvas.DrawText(noeud.NOM, pos.X + 12, pos.Y + 5, paintTexte);
        }

        if (_chemin != null && _chemin.Count > 1)
        {
            canvas.DrawText($"Distance totale du chemin : {totalKm} km", 100, hauteur - 40, paintTexte);
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
            Console.WriteLine($"Erreur lors de l'ouverture de l'image : {e.Message}");
        }
    }
}
