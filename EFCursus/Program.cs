using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;

namespace EFCursus
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.Write("Artikel nr.: ");
                var artikelNr = int.Parse(Console.ReadLine());
                Console.Write("Magazijn nr.:");
                var magazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Aantal stuks toevoegen:");
                var aantalStuks = int.Parse(Console.ReadLine());
                VoorraadBijvulling(artikelNr, magazijnNr, aantalStuks);

            }
            catch(FormatException)
            {
                Console.WriteLine("Tik een getal");
            }
           


        }



        static void VoorraadBijvulling(int artikelNr, int magazijnNr, int aantalStuks)
        {
            using (var entities = new OpleidingenEntities())
            {
                var voorraad = entities.Voorraden.Find(magazijnNr, artikelNr);
                if(voorraad !=null)
                {
                    voorraad.AantalStuks += aantalStuks;
                    Console.WriteLine("Pas nu de voorraad aan met server explorer, druk daatna op enter");
                    Console.ReadLine();
                    try
                    {
                        entities.SaveChanges();
                    }
                    catch(DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("Voorraad werd door een andere applicatie aangepast.");
                    }
                }
                else
                {
                    Console.WriteLine("Voorraad niet gevonden.");
                }
            }
        }
        static void VoorraadTransfer(int artikelNr, int vanMagazijnNr, int naarMagazijnNr, int aantalStuks)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead
            };
            using (var transactionscope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var vanVoorraad = entities.Voorraden.Find(vanMagazijnNr, artikelNr);
                    if (vanVoorraad != null)
                    {
                        if (vanVoorraad.AantalStuks >= aantalStuks)
                        {
                            vanVoorraad.AantalStuks -= aantalStuks;
                            var naarvoorraad = entities.Voorraden.Find(naarMagazijnNr, artikelNr);
                            if (naarvoorraad != null)
                            {
                                naarvoorraad.AantalStuks += aantalStuks;
                            }
                            else
                            {
                                naarvoorraad = new Voorraad { ArtikelNr = artikelNr, MagazijnNr = naarMagazijnNr, AantalStuks = aantalStuks };
                                entities.Voorraden.Add(naarvoorraad);
                            }
                            entities.SaveChanges();
                            transactionscope.Complete();
                        }
                        else
                        {
                            Console.WriteLine("Te weinig voorraad voor transfer");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Artiel niet gevonden in magazijn {0}", vanMagazijnNr);
                    }

                }
            }
        }


    }
}
