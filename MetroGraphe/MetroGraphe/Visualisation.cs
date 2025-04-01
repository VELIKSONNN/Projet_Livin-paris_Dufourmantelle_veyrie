using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Visualisation<T>
{
    private readonly Graphe<T> _graphe;
    private readonly Random _rand = new();

    public Visualisation(Graphe<T> graphe)
    {
        _graphe = graphe;
    }

    public void Dessiner(string filePath)
    {
        const int largeur = 4000;
        const int hauteur = 4000;

        using var bitmap = new SKBitmap(largeur, hauteur);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var paintNoeud = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        var paintTexte = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 16,
            IsAntialias = true
        };

        var paintLien = new SKPaint
        {
            Color = SKColors.Gray,
            StrokeWidth = 2,
            IsAntialias = true
        };

        var positions = new Dictionary<T, SKPoint>();

        // Couleurs par ligne
        var couleursLignes = new Dictionary<int, SKColor>();
        var palette = new[]
        {
            SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Orange,
            SKColors.Purple, SKColors.Brown, SKColors.Pink, SKColors.Teal,
            SKColors.Gold, SKColors.CadetBlue, SKColors.DarkCyan, SKColors.Indigo,
            SKColors.DarkOliveGreen, SKColors.Sienna, SKColors.MediumVioletRed
        };
        int paletteIndex = 0;
        foreach (var ligne in _graphe.Noeuds.Select(n => n.numeroStation).Distinct())
        {
            couleursLignes[ligne] = palette[paletteIndex % palette.Length];
            paletteIndex++;
        }

        // Position aléatoire sans superposition
        foreach (var noeud in _graphe.Noeuds)
        {
            SKPoint point;
            do
            {
                float x = _rand.Next(100, largeur - 200);
                float y = _rand.Next(100, hauteur - 200);
                point = new SKPoint(x, y);
            } while (positions.Values.Any(p => Distance(p, point) < 100));

            positions[noeud.ID] = point;
        }

        // Liaisons
        foreach (var lien in _graphe.Liens)
        {
            var p1 = positions[lien.Source.ID];
            var p2 = positions[lien.Destination.ID];
            canvas.DrawLine(p1, p2, paintLien);

            var milieu = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            canvas.DrawText($"{lien.Distancesuivant}", milieu.X, milieu.Y, paintTexte);
        }

        // Noeuds
        foreach (var noeud in _graphe.Noeuds)
        {
            var pos = positions[noeud.ID];
            paintNoeud.Color = couleursLignes[noeud.numeroStation];
            canvas.DrawCircle(pos, 12, paintNoeud);
            canvas.DrawText($"{noeud.NOM}", pos.X + 15, pos.Y + 5, paintTexte);
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
            Console.WriteLine($"Erreur à l'ouverture de l'image : {e.Message}");
        }
    }

    private float Distance(SKPoint p1, SKPoint p2)
    {
        return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
