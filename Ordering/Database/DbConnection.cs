using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Ordering.Orders;

namespace Ordering.Database
{
    public struct Pica
    {
        public int Id_Pica;
        public string Naziv;
        public int Kolicina;
        public int Cijena;
    }
    
    public class DbConnection
    {
        SqlConnection cnn;
        SqlCommand sqlCommand;
        SqlDataReader dataReader;
        
        string connectionString= "server=LOCALHOST\\SQLEXPRESS; database=Restaurant; Trusted_Connection=true";

        public List<Pica> GetPica()
        {
            cnn = new SqlConnection(connectionString);
            string sql= $"SELECT * FROM Pica";
            
            List<Pica> allDrinks = new List<Pica>();

            try
            {
                cnn.Open();
                sqlCommand = new SqlCommand(sql, cnn);
                dataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (dataReader.Read())
                {
                    Pica drink;
                    drink.Id_Pica = (int)dataReader.GetValue(0);
                    drink.Naziv = (string)dataReader.GetValue(1);
                    drink.Cijena = (int)dataReader.GetValue(2);
                    drink.Kolicina = 0;
                    allDrinks.Add(drink);

                }
                return allDrinks;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return null;
        }
        
        public void SpremiRacun(Table stol)
        {
            
        }
    }
}
