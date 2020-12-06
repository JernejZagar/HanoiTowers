using System;
using System.Collections.Generic;

namespace HanoiTower
{
    public abstract class HanoiTower
    {
        public HanoiTower(int discs, int pegs, byte start, byte final)
        {
            this.Discs = discs;
            this.Pegs = pegs;
            this.Start = start;
            this.Final = final;
        }


        public int Discs { get; set; }

        public int Pegs { get; set; }

        public byte Start { get; set; }

        public byte Final { get; set; }

        public abstract void MakeMove(byte[] state);

        
        

    }

    public class Hanoi_K4 : HanoiTower
    {
        public Hanoi_K4(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        public override void MakeMove(byte[] state)
        {
            List<int> seznamPredhodnihPozicij = new List<int>();
            List<int> seznamTrenutnihPozicij = new List<int>() {ConvertNumbers.TetraToDecimal(state)};
            List<int> seznamNovihPozicij = new List<int>();

            while (true)
            {
                foreach (int pozicija in seznamTrenutnihPozicij)
                {
                    byte[] pozicijaByteArray = ConvertNumbers.DecimalToTetra(pozicija, this.Discs);

                    HashSet<byte> zePremaknjeniStolpi = new HashSet<byte>();
                    for (int i = 0; i < pozicijaByteArray.Length; i++)
                    {
                        // Preverjamo ali smo iz tega stolpa že naredili premik. Če še nismo, ga naredimo.
                        // Ker gremo od leve proti desni bomo vedno premaknili zgornji disk, kar je pravilno.
                        if (!zePremaknjeniStolpi.Contains(pozicijaByteArray[i]))
                        {
                            HashSet<byte> mozniStolpi = new HashSet<byte> { 0, 1, 2, 3 };
                            for (int j = 0; j <= i; j++)
                            {
                                mozniStolpi.Remove(pozicijaByteArray[j]);
                            }

                            foreach (byte x in mozniStolpi)
                            {
                                // Zaradi referenčnega tipa uporabim prvotno pozicijo iz tipa int.
                                // Zato, da se ob kreiranju nove pozicije ne spremeni prvotna.
                                byte[] novaPozicija = ConvertNumbers.DecimalToTetra(pozicija, this.Discs);
                                novaPozicija[i] = x;
                                int novaPozicijaInt = ConvertNumbers.TetraToDecimal(novaPozicija);
                                if (!seznamPredhodnihPozicij.Contains(novaPozicijaInt))
                                {
                                    Console.WriteLine($"Nova pozicija: {String.Join(",", novaPozicija)}");
                                    seznamNovihPozicij.Add(novaPozicijaInt);
                                    zePremaknjeniStolpi.Add(pozicijaByteArray[i]);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Predhodne pozicije: {String.Join(",", seznamPredhodnihPozicij)}");
                Console.WriteLine($"Trenutne pozicije: {String.Join(",", seznamTrenutnihPozicij)}");
                Console.WriteLine($"Nove pozicije: {String.Join(",", seznamNovihPozicij)}");
                seznamPredhodnihPozicij = seznamTrenutnihPozicij;
                seznamTrenutnihPozicij = seznamNovihPozicij;
                seznamNovihPozicij.Clear();
            }




            











            /*
            byte first = state[0];
            
            byte[] mozniStolpi = new byte[] { 0, 1, 2, 3 };
            foreach(byte x in mozniStolpi)
            {
                //Console.WriteLine($"Prvotni: {String.Join(",", state)}");
                //Console.WriteLine($"Prvotni integer: {first}");
                if (first != x)
                {
                    state[0] = x;
                    seznamNovihPozicij.Add(state);
                    //Console.WriteLine($"Dodani: {String.Join(",", stateAppended)}");
                }
                
            }

            //throw new NotImplementedException();
            */
        }
    }
}
