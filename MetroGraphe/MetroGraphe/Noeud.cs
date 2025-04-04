using livinparis_dufourmantelle_veyrie;

namespace livinparis_dufourmantelle_veyrie
{
    /// <summary>
    /// Représente un nœud dans le graphe, correspondant à une station de métro.
    /// </summary>
    public class Noeud<T>
    {
        /// <summary>
        /// Identifiant unique du nœud (ex: ID de la station).
        /// </summary>
        public T ID { get; set; }

        /// <summary>
        /// Nom de la station (ex: "Châtelet", "Alésia").
        /// </summary>
        public string NOM { get; set; }

        /// <summary>
        /// Liste des lignes de métro desservant cette station (ex: 1, 4, 7...).
        /// </summary>
        public List<int> Lignes { get; set; } = new();

        /// <summary>
        /// Latitude GPS de la station.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude GPS de la station.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Constructeur du nœud.
        /// </summary>
        /// <param name="id">Identifiant du nœud</param>
        /// <param name="nom">Nom de la station</param>
        /// <param name="latitude">Latitude GPS</param>
        /// <param name="longitude">Longitude GPS</param>
        public Noeud(T id, string nom, double latitude, double longitude)
        {
            ID = id;
            NOM = nom;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
