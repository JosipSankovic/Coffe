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
                cnn.Close();
                return allDrinks;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            cnn.Close();
            return null;
        }
        
        public void SpremiRacun(Table stol)
        {
            string sql1 = $"INSERT INTO Racun_Cijena(Cijena) VALUES ({stol.tableCost})";
            string getId = $"SELECT MAX(Id_Racun) FROM Racun_Cijena";
            int Id_Racun = 0;
            string sql2="";
            int item_count=stol.billItems.Count();

           

            cnn.Open();
            sqlCommand = new SqlCommand(sql1, cnn);
            sqlCommand.ExecuteNonQuery();
      
            sqlCommand.CommandText = getId;
            dataReader=sqlCommand.ExecuteReader();
            while(dataReader.Read())
            Id_Racun =(int) dataReader.GetValue(0);
            dataReader.Close();
            foreach (var x in stol.billItems)
            {
                sql2 += $" INSERT INTO Racun_Pica(Id_Racun,Kolicina,Id_Pica) VALUES({Id_Racun},{x.Kolicina},{x.Id_Pica}) ";
            }
            
            sqlCommand.CommandText = sql2;
            sqlCommand.ExecuteNonQuery();

            sqlCommand.Dispose();
            cnn.Close();
        }
    }
}
