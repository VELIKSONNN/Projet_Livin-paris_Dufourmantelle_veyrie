using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace livinparis_dufourmantelle_veyrie
{
    /// <summary>
    /// Représente un lien (ou une arête) entre deux nœuds dans un graphe.
    /// Chaque lien contient une source, une destination et un poids (distance ou temps).
    /// </summary>
    /// <typeparam name="T">Type utilisé pour identifier les nœuds (souvent un int).</typeparam>
    public class Lien<T>
    {
        /// <summary>
        /// Nœud de départ du lien (source).
        /// </summary>
        public Noeud<T> Source { get; set; }

        /// <summary>
        /// Nœud d’arrivée du lien (destination).
        /// </summary>
        public Noeud<T> Destination { get; set; }

        /// <summary>
        /// Poids du lien, généralement utilisé comme distance ou temps entre les deux nœuds.
        /// </summary>
        public double Distancesuivant { get; set; }

        /// <summary>
        /// Constructeur d’un lien entre deux nœuds avec un poids.
        /// </summary>
        /// <param name="source">Nœud de départ</param>
        /// <param name="destination">Nœud d’arrivée</param>
        /// <param name="distance">Poids du lien</param>
        public Lien(Noeud<T> source, Noeud<T> destination, double distance)
        {
            Source = source;
            Destination = destination;
            Distancesuivant = distance;
        }
    }
}
