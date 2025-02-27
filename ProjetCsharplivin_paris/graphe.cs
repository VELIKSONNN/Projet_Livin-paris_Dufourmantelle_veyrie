using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace PROJET_étudiant
{
    internal class graphe
    {
        private int NombreSommets;
        private List<int>[] ListeAdjacence;
        private int[,] MatriceAdjacence;
        private List<Lien> Liens;

        public graphe(int n)
        {
            NombreSommets = n;
            ListeAdjacence = new List<int>[n + 1];
            MatriceAdjacence = new int[n + 1, n + 1];
            for (int i = 1; i <= n; i++)
                ListeAdjacence[i] = new List<int>();
        }

        public void AjouterLien(int u, int v)
        {
            ListeAdjacence[u].Add(v);
            ListeAdjacence[v].Add(u);
            MatriceAdjacence[u, v] = 1;
            MatriceAdjacence[v, u] = 1;
        }

        public void ParcoursLargeur(int depart)
        {
            bool[] visite = new bool[NombreSommets + 1];
            Queue<int> file = new Queue<int>();
            file.Enqueue(depart);
            visite[depart] = true;

            while (file.Count > 0)
            {
                int noeud = file.Dequeue();
                Console.Write(noeud + " ");
                foreach (var voisin in ListeAdjacence[noeud])
                {
                    if (!visite[voisin])
                    {
                        visite[voisin] = true;
                        file.Enqueue(voisin);
                    }
                }
            }
        }

        public void ParcoursProfondeur(int depart, bool[] visite)
        {
            visite[depart] = true;
            Console.Write(depart + " ");
            foreach (var voisin in ListeAdjacence[depart])
            {
                if (!visite[voisin])
                    ParcoursProfondeur(voisin, visite);
            }
        }

        public void ChargerDepuisFichier(string chemin)
        {
            string[] lignes = File.ReadAllLines(chemin);
            foreach (string ligne in lignes)
            {
                if (ligne.StartsWith("%")) continue;
                string[] parties = ligne.Split();
                if (parties.Length == 2)
                    AjouterLien(int.Parse(parties[0]), int.Parse(parties[1]));
            }
        }
        public void DessinerGraphe(string cheminImage)
        {
            int largeur = 500;
            int hauteur = 500;
            int rayon = 200;
            int rayonNoeud = 15;
            PointF[] positions = new PointF[NombreSommets + 1];
            using (Bitmap bmp = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Pen pen = new Pen(Color.Black, 2))
            using (Brush brush = new SolidBrush(Color.Blue))
            using (Font font = new Font("Arial", 10))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                g.Clear(Color.White);
                for (int i = 1; i <= NombreSommets; i++)
                {
                    double angle = 2 * Math.PI * i / NombreSommets;
                    positions[i] = new PointF(
                        (float)(largeur / 2 + rayon * Math.Cos(angle)),
                        (float)(hauteur / 2 + rayon * Math.Sin(angle)));
                }
                while (Liens != null)
                {
                    foreach (var lien in Liens)
                        g.DrawLine(pen, positions[lien.Source], positions[lien.Destination]);
                    for (int i = 1; i <= NombreSommets; i++)
                    {
                        g.FillEllipse(brush, positions[i].X - rayonNoeud, positions[i].Y - rayonNoeud, rayonNoeud * 2, rayonNoeud * 2);
                        g.DrawString(i.ToString(), font, textBrush, positions[i].X - 6, positions[i].Y - 6);
                    }
                    bmp.Save(cheminImage);
                }
            }
        }
    }
}
