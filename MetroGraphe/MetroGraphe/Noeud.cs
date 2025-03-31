using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroGraphe
{
   public class Noeud<T>
    {
        private T Id;
        private string Nom;

        private int NumeroStation;

      
        public T ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string NOM
        {
            get { return Nom; }
            set { Nom = value; }
        }
        public int numeroStation
        {
            get { return NumeroStation;  }
            set { NumeroStation = value; }
        }

        public Noeud(T id, string nom)
        {
            Id = id;
            Nom = nom;
          
        }
        public static Dictionary<T, Noeud<T>> ChargerNoeudsDepuisFichier(string fichierNoeuds)
        {
            var noeuds = new Dictionary<T, Noeud<T>>();

            foreach (var ligne in File.ReadAllLines(fichierNoeuds).Skip(1)) // Skip en-tête
            {
                var valeurs = ligne.Split(';');

                if (valeurs.Length < 4) continue; // Vérification pour éviter erreurs de format


                try
                {
                    T id = (T)Convert.ChangeType(valeurs[0], typeof(T));
                    string nom = valeurs[1];

                    noeuds[id] = new Noeud<T>(id, nom);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la conversion de l'ID '{valeurs[0]}': {ex.Message}");
                }
            }
            return noeuds;
        }
    }
}
