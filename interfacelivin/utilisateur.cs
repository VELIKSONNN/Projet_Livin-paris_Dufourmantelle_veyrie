using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using interfacelivin;
using MySql.Data.MySqlClient;
namespace interfacelivin
{
    public class utilisateur
    {
        private string Prenom;
        private string Nom;
        private string email;
        private int id;
        private string adresse;
        private string tel;
        private string mdp;
        private string entreprise;
        
        
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public int Id
        {
            get;
            set;
        }
        public string Adresse
        { get;
            set;
        }
        public string Tel
        {
            get;
            set;
        }
        public string Mdp
        {
            get;
            set;
        }
        public string Entreprise
        {
            get;
            set;
        }
        public utilisateur(int id, string prenom, string email, string tel, string adresse, string entreprise, string Nom, string mdp)
        {
            this.id = id;
            this.Prenom = prenom;
            this.email = email;
            this.tel = tel;
            this.adresse = adresse;
            this.entreprise = entreprise;
            this.Nom = Nom;
            this.mdp = mdp;
        }
        

    }
}
