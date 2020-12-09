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

        HashSet<string> povezave = new HashSet<string>() {"01", "10", "02", "20", "03", "30", "12", "21", "23", "32", "13", "31"};

        //HashSet<string> povezave = new HashSet<string>() { "01", "10", "02", "20", "03", "30", "12", "21", "23", "32", "13", "31" };

        public override void MakeMove(byte[] state)
        {
            List<int> seznamPredhodnihPozicij = new List<int>();
            List<int> seznamTrenutnihPozicij = new List<int>() {ConvertNumbers.TetraToDecimal(state)};
            List<int> seznamNovihPozicij = new List<int>();

            byte[] startPosition = ConvertNumbers.SetPosition(this.Start, this.Discs);
            byte[] final = ConvertNumbers.SetPosition(this.Final, this.Discs);
            string endPosition = string.Join(",", final);

            int steviloPonovitev = 0;
            bool status = true;
            while (status)
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
                            HashSet<byte> mozniStolpi = new HashSet<byte> { 0, 1, 2, 3};
                            for (int j = 0; j <= i; j++)
                            {
                                mozniStolpi.Remove(pozicijaByteArray[j]);
                            }

                            foreach (byte x in mozniStolpi)
                            {
                                // Zaradi referenčnega tipa uporabim prvotno pozicijo iz tipa int.
                                // Zato, da se ob kreiranju nove pozicije ne spremeni prvotna.
                                byte[] novaPozicija = ConvertNumbers.DecimalToTetra(pozicija, this.Discs);

                                // s tem podatkov v if-u spodaj preverimo ali obstaja povezava med stolpoma.
                                byte y = novaPozicija[i];
                                string preveriPovezavo = x.ToString() + y.ToString();
                                //Console.WriteLine($"Preveri povezavo: {preveriPovezavo}");

                                novaPozicija[i] = x;
                                int novaPozicijaInt = ConvertNumbers.TetraToDecimal(novaPozicija);
                                string possibleFinal = string.Join(",", novaPozicija);

                                if (!seznamPredhodnihPozicij.Contains(novaPozicijaInt) & !seznamNovihPozicij.Contains(novaPozicijaInt) &
                                    !seznamTrenutnihPozicij.Contains(novaPozicijaInt) & povezave.Contains(preveriPovezavo) )
                                {
                                    if (possibleFinal == endPosition)
                                    {
                                        status = false;
                                        Console.WriteLine($"Našli smo rešitev: {String.Join(",", novaPozicija)}");
                                        break;

                                    }
                                    else
                                    {
                                        //Console.WriteLine($"Nova pozicija: {String.Join(",", novaPozicija)}");
                                        seznamNovihPozicij.Add(novaPozicijaInt);
                                        zePremaknjeniStolpi.Add(pozicijaByteArray[i]);
                                    }
                                }
                            }
                        }
                    }
                    //Console.WriteLine("------");
                }
                seznamPredhodnihPozicij = new List<int>(seznamTrenutnihPozicij);
                seznamTrenutnihPozicij = new List<int>(seznamNovihPozicij);
                seznamNovihPozicij.Clear();
                steviloPonovitev += 1;
                Console.WriteLine($"Stevilo ponovitev: {steviloPonovitev}");
            }
        }
    }
}
