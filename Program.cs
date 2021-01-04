using System;

namespace HanoiTower
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Problem hanojskih stolpov - Jernej Žagar");


            Console.WriteLine();
            byte i = 1;
            foreach (string stolp in Enum.GetNames(typeof(Tower)))
            {
                Console.WriteLine("     " + i + "   " + stolp);
                i++;
            }

            Console.WriteLine();
            Console.WriteLine("Izberi številko problema in pritisni Enter.");
            string izbira = Console.ReadLine();

            Console.WriteLine("Izberi število diskov in pritisni Enter.");
            string stDiskov = Console.ReadLine();
            int discs = Convert.ToInt32(stDiskov);

            Console.WriteLine("Izberi začetni stolp in pritisni Enter.");
            string startPeg = Console.ReadLine();
            byte start = Convert.ToByte(startPeg);

            Console.WriteLine("Izberi končni stolp in pritisni Enter.");
            string endPeg = Console.ReadLine();
            byte final = Convert.ToByte(endPeg);

            HanoiTower vrstaStolpa = HanoiTowerFactory.GetHanoiTower(izbira, discs, start, final);
            byte[] zacetnoStanje = ConvertNumbers.SetPosition(vrstaStolpa.Start, vrstaStolpa.Discs);


            Console.WriteLine("Začetek");
            DateTime dtStart = DateTime.Now;

            vrstaStolpa.DolzinaNajkrajsePoti(zacetnoStanje);

            DateTime dtEnd = DateTime.Now;
            Console.WriteLine("Konec");
            Console.WriteLine($"Čas izvajanja= {(dtEnd - dtStart).TotalSeconds}");
         
        }
    }
}
