
using MySql.Data.MySqlClient;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using livinparis_dufourmantelle_veyrie;
{
    public class UnitTest1
    {
        [Fact]
        public void Dijkstra_Returns_Correct_Path()
        {
            // Arrange : Cr�ation d'un graphe simple
            // On cr�e 4 n�uds : A (0), B (1), C (2) et D (3)
            var nodeA = new Noeud<int>(0, "A", 0, 0);
            var nodeB = new Noeud<int>(1, "B", 0, 0);
            var nodeC = new Noeud<int>(2, "C", 0, 0);
            var nodeD = new Noeud<int>(3, "D", 0, 0);

            // Cr�ation des liens (ar�tes)
            // A -> B : 1, A -> C : 4, B -> C : 2, B -> D : 5, C -> D : 1
            var liens = new List<Lien<int>> {
                new Lien<int>(nodeA, nodeB, 1),
                new Lien<int>(nodeA, nodeC, 4),
                new Lien<int>(nodeB, nodeC, 2),
                new Lien<int>(nodeB, nodeD, 5),
                new Lien<int>(nodeC, nodeD, 1)
            };

            var noeuds = new List<Noeud<int>> { nodeA, nodeB, nodeC, nodeD };

            // Cr�ation du graphe
            var graphe = new Graphe<int>(noeuds, liens);

            // Act : Calculer le chemin le plus court de A � D avec Dijkstra
            var chemin = graphe.Dijkstra(nodeA, nodeD);

            // Assert : On attend le chemin A -> B -> C -> D
            Assert.NotNull(chemin);
            Assert.Equal(4, chemin.Count);
            Assert.Equal(0, chemin[0].ID);
            Assert.Equal(1, chemin[1].ID);
            Assert.Equal(2, chemin[2].ID);
            Assert.Equal(3, chemin[3].ID);
        }

        [Fact]
        public void BellmanFord_Returns_Correct_Path()
        {
            // Arrange : Utilisons le m�me graphe que dans le test pr�c�dent
            var nodeA = new Noeud<int>(0, "A", 0, 0);
            var nodeB = new Noeud<int>(1, "B", 0, 0);
            var nodeC = new Noeud<int>(2, "C", 0, 0);
            var nodeD = new Noeud<int>(3, "D", 0, 0);

            var liens = new List<Lien<int>> {
                new Lien<int>(nodeA, nodeB, 1),
                new Lien<int>(nodeA, nodeC, 4),
                new Lien<int>(nodeB, nodeC, 2),
                new Lien<int>(nodeB, nodeD, 5),
                new Lien<int>(nodeC, nodeD, 1)
            };

            var noeuds = new List<Noeud<int>> { nodeA, nodeB, nodeC, nodeD };
            var graphe = new Graphe<int>(noeuds, liens);

            // Act : Calculer le chemin le plus court de A � D avec Bellman-Ford
            var chemin = graphe.BellmanFord(nodeA, nodeD);

            // Assert : On attend le chemin A -> B -> C -> D
            Assert.NotNull(chemin);
            Assert.Equal(4, chemin.Count);
            Assert.Equal(0, chemin[0].ID);
            Assert.Equal(1, chemin[1].ID);
            Assert.Equal(2, chemin[2].ID);
            Assert.Equal(3, chemin[3].ID);
        }

        [Fact]
        public void AjouterLien_Updates_ListeAdjacente()
        {
            // Arrange : Cr�ation d'un graphe avec 2 n�uds
            var nodeA = new Noeud<int>(0, "A", 0, 0);
            var nodeB = new Noeud<int>(1, "B", 0, 0);
            var noeuds = new List<Noeud<int>> { nodeA, nodeB };
            var liens = new List<Lien<int>>();
            var graphe = new Graphe<int>(noeuds, liens);

            // Act : Ajouter un lien de A vers B
            var lien = new Lien<int>(nodeA, nodeB, 10);
            graphe.AjouterLien(lien);

            // Assert : V�rifier que dans la liste d'adjacence du n�ud A, le lien a bien �t� ajout�
            int indexA = Convert.ToInt32(nodeA.ID);
            Assert.Contains(lien, graphe.ListeAdjacente[indexA]);
        }
    }
}
