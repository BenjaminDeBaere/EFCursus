using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCursus
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.Write("Artikel nr.:");
                var artikelNr = int.Parse(Console.ReadLine());
                Console.Write("Van magazijn nr.:");
                var vanMagazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Naar magazijn nr:");
                var naarMagazijnNr = int.Parse(Console.ReadLine());
                Console.Write("Aantal stuks:");
                var aantalStuks = int.Parse(Console.ReadLine());
                voorraadTransfer(artikelNr, vanMagazijnNr, naarMagazijnNr, aantalStuks);
            }
            catch(FormatException)
            {
                Console.WriteLine("Tik een getal");
            }


        }
        static void VoorraadTransfer(int artikelNr, int vanMagazijnNr, int naarMagazijnNr, int aantalStuks)
        {
            using (var entities = new OpleidingenEntities())
            {
                var vanVoorraad = entities.Voorraden.Find(vanMagazijnNr, artikelNr);
                if(vanVoorraad!=null)
                {

                }

            }
        }


    }
}
