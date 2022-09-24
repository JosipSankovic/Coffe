using System;
using System.Collections.Generic;
using System.Linq;
using Ordering.Orders;
using Ordering.Database;
namespace Ordering
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int TableNumber = 0;

            List<Table> stolovi = new List<Table>();
            List<Pica> ListOfDrinks;
            Table stol;
            DbConnection connection = new DbConnection();


            while (true)
            {
                int IndexPica = 1;
                Console.WriteLine("Odaberi Stol");

                try
                {
                    TableNumber = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                int TableIndex = stolovi.FindIndex(x => x.tableNumber.Equals(TableNumber));
                // Ako nema tog stola dodaj novi inace dobijemo Index tog stola
                if (TableIndex == -1)
                {
                    stol = new Table();
                    stol.tableNumber = TableNumber;
                    stol.tableCost = 0;
                    stol.billItems = new List<Pica>();
                    stolovi.Add(stol);
                    TableIndex = stolovi.Count() - 1;


                }
                Console.Clear();

                Table table = stolovi[TableIndex];
                //GetDrinksFromSqlDatavase
                ListOfDrinks = connection.GetPica();

                Console.WriteLine("Lista pica");
                ShowAllDrinks(ListOfDrinks);

                while (IndexPica != 0)
                {
                    Console.Clear();
                    Console.WriteLine("Dodaj sto zelite u racunu pritisni 0 za odabir stola i 'Y' za spremanje racuna u bazu podataka");
                    
                    Console.WriteLine("Racun::");
                    ShowAllRecipeDriks(stolovi[TableIndex]);
                    string odabir = Console.ReadLine();
                    try
                    {
                        IndexPica = Convert.ToInt32(odabir);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        
                        IndexPica = 0;
                        if (odabir == "Y")
                        {
                            SpremiUBazu(connection, table);
                            break;
                        }
                    }

                    //Dodavanje pica
                    if (IndexPica > 0 && IndexPica <5)
                    {
                        
                        if(table.billItems.FindIndex(x => x.Id_Pica.Equals(IndexPica))<0)
                        table.billItems.Add(ListOfDrinks.Find(x => x.Id_Pica.Equals(IndexPica)));
                        table.tableCost += ListOfDrinks.Find(x => x.Id_Pica.Equals(IndexPica)).Cijena;
                        var pice = table.billItems.Find(x => x.Id_Pica.Equals(IndexPica));
                        pice.Kolicina++;
                        
                        table.billItems[table.billItems.FindIndex(x => x.Id_Pica.Equals(IndexPica))] = pice;
                        stolovi[TableIndex] = table;
                    }

                   


                }

            }
        }

        public static void SpremiUBazu(DbConnection cnn,Table stol)
        {
            Console.WriteLine("Spremi");
            cnn.SpremiRacun(stol);
            
        }

        //Prikaz racuna
        public static void ShowAllRecipeDriks(Table stol)
        {
            foreach (var x in stol.billItems)
            {
                Console.WriteLine($"{x.Naziv}, Kolicina:{x.Kolicina} ");

            }
            Console.WriteLine(stol.tableCost);
        }
        //Prikaz pica
        public static void ShowAllDrinks(List<Pica> ListOfDrinks)
        {
            foreach (var x in ListOfDrinks)
            {
                Console.WriteLine($"Id:{x.Id_Pica}, Naziv: {x.Naziv}, Cijena:{x.Cijena} kn");
            }
        }
    }
}

