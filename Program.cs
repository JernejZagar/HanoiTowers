using System;

namespace HanoiTower
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Clear();
            Console.WriteLine("Hello World!");

            Hanoi_K4 hanoiK4 = new Hanoi_K4(5, 4, 1, 4);


            //Metode za konvertiranje tipov števil.
            //Console.WriteLine(ConvertNumbers.Potenca(-2, 2));
            //byte[] stanje = new byte[] {3, 1, 3, 0, 1 };
            //Console.WriteLine(ConvertNumbers.TetraToDecimal(stanje));
            //byte[] stanje = ConvertNumbers.DecimalToTetra(881,5);
            //Console.WriteLine(String.Join(",", stanje));
            //byte[] stanje2 = ConvertNumbers.StartEndPosition(1, 10);
            //Console.WriteLine(String.Join(",", stanje2));
            Console.WriteLine("Začetek");
            byte[] stanje = new byte[] { 1, 0, 0, 0, 0 };
            hanoiK4.MakeMove(stanje);
            Console.WriteLine("Konec");
        }
    }
}
