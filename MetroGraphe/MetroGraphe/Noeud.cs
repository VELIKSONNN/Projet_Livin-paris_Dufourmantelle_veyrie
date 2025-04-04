using livinparis_dufourmantelle_veyrie;
namespace livinparis_dufourmantelle_veyrie
{
    public class Noeud<T>
    {
        public T ID { get; set; }
        public string NOM { get; set; }
        public List<int> Lignes { get; set; } = new();
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Noeud(T id, string nom, double latitude, double longitude)
        {
            ID = id;
            NOM = nom;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}