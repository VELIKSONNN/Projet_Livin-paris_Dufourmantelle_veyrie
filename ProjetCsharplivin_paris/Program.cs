namespace PROJET_étudiant

{
    internal class Program
    {
        static void Main(string[] args)
        {
            graphe g = new graphe(34);
            g.ChargerDepuisFichier();
            Console.WriteLine("Parcours en largeur :");
            g.ParcoursLargeur(1);

            Console.WriteLine("\nParcours en profondeur :");
            bool[] visite = new bool[35];
            g.ParcoursProfondeur(1, visite);

            Console.WriteLine("\nLe graphe est connexe ? " + g.EstConnexe());
            Console.WriteLine("Le graphe contient un cycle ? " + g.ContientCycle());

            g.DessinerGraphe("graphe.png");
            Console.WriteLine("Image du graphe générée : graphe.png");
            Console.WriteLine("Hello, World!");

        }
    }
}
