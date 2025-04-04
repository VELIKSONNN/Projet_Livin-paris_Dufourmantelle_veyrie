using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using interfacelivin;
namespace interfacelivin

{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region connexion
            MySqlConnection connexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=baselivinparis;" +
                                         "user=root;PASSWORD=Sabrelaser00";

                connexion = new MySqlConnection(connexionString);
                connexion.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                return;
            }
            #endregion

            
            Console.Clear();
            interfaceuser interface1 = new interfaceuser(connexion);
            }

                
        }
        
}
 
