using System;
using System.Collections.Generic;
using Xunit;
using livinparis_dufourmantelle_veyrie;

namespace livinparis_dufourmantelle_veyrie.Tests
{
    public class GrapheTests
    {
     
        private static Noeud<int> Node(int id)
        {
            // Replace with the correct constructor / factory if needed
            return (Noeud<int>)Activator.CreateInstance(typeof(Noeud<int>), id);
        }

        private static Graphe<int> BuildSimpleGraph()
        {
           
            var n0 = Node(0);
            var n1 = Node(1);
            var n2 = Node(2);

            var nodes = new List<Noeud<int>> { n0, n1, n2 };
            var links = new List<Lien<int>>
            {
                new Lien<int>(n0, n1, 1),
                new Lien<int>(n1, n2, 1),
                new Lien<int>(n0, n2, 3)
            };

            return new Graphe<int>(nodes, links);
        }

    
        [Fact]
        public void AjouterLien_ShouldAddLienAndUpdateAdjacency()
        {
            // Arrange
            var graph = BuildSimpleGraph();
            var n3 = Node(3);
            graph.Noeuds.Add(n3);
            var lien = new Lien<int>(graph.Noeuds[2], n3, 2); // 2 -> 3

            // Act
            graph.AjouterLien(lien);

            // Assert
            Assert.Contains(lien, graph.Liens);
            Assert.Contains(lien, graph.ListeAdjacente[graph.Noeuds[2].ID]);
        }

      

        [Fact]
        public void Dijkstra_ReturnsExpectedShortestPath()
        {
        
            var graph = BuildSimpleGraph();
            var start = graph.Noeuds[0];
            var end = graph.Noeuds[2];

           
            var path = graph.Dijkstra(start, end);

           
            Assert.Equal(3, path.Count);
            Assert.Collection(path,
                n => Assert.Equal(0, n.ID),
                n => Assert.Equal(1, n.ID),
                n => Assert.Equal(2, n.ID));
        }
-----------------------------------------------------

        [Fact]
        public void BellmanFord_ReturnsSameResultAsDijkstra_WhenNoNegativeWeights()
        {
            
            var graph = BuildSimpleGraph();
            var start = graph.Noeuds[0];
            var end = graph.Noeuds[2];

         
            var pathBF = graph.BellmanFord(start, end);
            var pathDI = graph.Dijkstra(start, end);

            Assert.Equal(pathDI.Count, pathBF.Count);
            for (int i = 0; i < pathDI.Count; i++)
            {
                Assert.Equal(pathDI[i].ID, pathBF[i].ID);
            }
        }


        [Fact]
        public void BellmanFord_ShouldThrow_WhenNegativeCyclePresent()
        {
            
            var n0 = Node(0);
            var n1 = Node(1);
            var n2 = Node(2);
            var nodes = new List<Noeud<int>> { n0, n1, n2 };
            var links = new List<Lien<int>>
            {
                new Lien<int>(n0, n1, 1),
                new Lien<int>(n1, n2, 1),
                new Lien<int>(n2, n0, -4)
            };
            var graph = new Graphe<int>(nodes, links);

           
            Assert.Throws<InvalidOperationException>(() => graph.BellmanFord(n0, n2));
        }


        [Fact]
        public void ColorationWelshPowell_AssignsDifferentColorsToAdjacentNodes()
        {
            var graph = BuildSimpleGraph();

            var colors = graph.ColorationWelshPowell();

            foreach (var lien in graph.Liens)
            {
                Assert.NotEqual(colors[lien.Source], colors[lien.Destination]);
            }
        }
    }
}
