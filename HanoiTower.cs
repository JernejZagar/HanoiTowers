using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HanoiTower
{
    public enum Tower
    {
        Hanoi_K4,
        Hanoi_K13e,
        Hanoi_K13,
        Hanoi_K4Ne,
        Hanoi_C4,
        Hanoi_P13
    }

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

        abstract public HashSet<(int, int)> Povezave { get; }

        /// <summary>
        /// Metoda poišče možne stolpe v katere lahko premaknemo iz i-ti disk.
        /// Pri tem ne upošteva povezave, ampak le to, da manjši disk postavimo le na večjega.
        /// </summary>
        /// <param name="pozicijaByteArray"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private HashSet<byte> MozniStolpi(byte[] pozicijaByteArray, byte i)
        {
            // fiskirano za 4 stolpe, če želimo v splošnem, potem je potrebno zapisati množico v splošnem
            HashSet<byte> mozniStolpi = new HashSet<byte> { 0, 1, 2, 3 };
            for (int j = 0; j <= i; j++)
            {
                mozniStolpi.Remove(pozicijaByteArray[j]);
            }

            return mozniStolpi;
        }


        /// <summary>
        /// Metoda poišče vse zgornje diske.
        /// </summary>
        /// <param name="pozicija">Trenutna pozicija</param>
        /// <returns>Metoda vrne množico zgornjih diskov; HashSet<byte></returns>
        private HashSet<byte> ZgornjiDiski(byte[] pozicija)
        {
            HashSet<byte> zgornjiDiski = new HashSet<byte> { };
            HashSet<byte> stolpiZacasno = new HashSet<byte> { };

            for (byte i = 0; i < pozicija.Length; i++)
            {
                if (!stolpiZacasno.Contains(pozicija[i]))
                {
                    zgornjiDiski.Add(i);
                    stolpiZacasno.Add(pozicija[i]);
                }
            }
            return zgornjiDiski;
        }


        public virtual void DolzinaNajkrajsePoti(byte[] state)
        {
            // Inicailizacija stanj pozicij, ki jih potrebujemo za rešitev problema.
            HashSet<int> seznamPredhodnihPozicij = new HashSet<int>();
            HashSet<int> seznamTrenutnihPozicij = new HashSet<int>() { ConvertNumbers.TetraToDecimal(state) };
            Queue<int> seznamNovihPozicij = new Queue<int>();


            // Inicailizacija končnega stanja, ki ga uporabimo za preverjanje ali smo že prišli do rešitve.
            byte[] final = ConvertNumbers.SetPosition(this.Final, this.Discs);
            int koncnaPozicija = ConvertNumbers.TetraToDecimal(final);

            int steviloPonovitev = 0;
            long mem = 0;
            bool status = true;
            while (status)
            {
                Parallel.ForEach(seznamTrenutnihPozicij, (pozicija, state) => 
                //foreach (int pozicija in seznamTrenutnihPozicij)
                {
                    byte[] pozicijaByteArray = ConvertNumbers.DecimalToTetra(pozicija, this.Discs);
                    HashSet<byte> zgornjiDiski = this.ZgornjiDiski(pozicijaByteArray);

                    HashSet<byte> zePremaknjeniStolpi = new HashSet<byte>();
                    for (byte i = 0; i < pozicijaByteArray.Length; i++)
                    {
                        // Preverjamo ali smo iz stolpa na katerem je i-ti disk že naredili premik.
                        // Če še nismo, ga naredimo.
                        // Preverimo še, če je disk zgornji.
                        if (!zePremaknjeniStolpi.Contains(pozicijaByteArray[i]) & zgornjiDiski.Contains(i))
                        {
                            HashSet<byte> mozniStolpi = MozniStolpi(pozicijaByteArray, i);

                            foreach (byte stolpX in mozniStolpi)
                            {
                                // Zaradi referenčnega tipa uporabim prvotno pozicijo iz tipa int.
                                // Zato, da se ob kreiranju nove pozicije ne spremeni prvotna.
                                byte[] novaPozicija = (byte[])pozicijaByteArray.Clone();
                                
                                byte stolpY = novaPozicija[i];

                                // premikamo iz stolpaY v stolpY, zato preverimo ali obstaja povezava
                                var preveriPovezavo = (stolpX, stolpY);

                                novaPozicija[i] = stolpX;
                                int novaPozicijaInt = ConvertNumbers.TetraToDecimal(novaPozicija);

                                if (!seznamPredhodnihPozicij.Contains(novaPozicijaInt) & this.Povezave.Contains(preveriPovezavo))
                                {
                                    if (novaPozicijaInt == koncnaPozicija)
                                    {
                                        state.Break();
                                        status = false;

                                        //goto test
                                        // ko najdemo končno pozicijo skočimo iz zanke in izpišemo število ponovitev
                                    }

                                    else
                                    {
                                        //Console.WriteLine($"Nova pozicija: {String.Join(",", novaPozicija)}");
                                        lock (seznamNovihPozicij)
                                        {
                                            seznamNovihPozicij.Enqueue(novaPozicijaInt);
                                        }

                                        //zePremaknjeniStolpi.Add(pozicijaByteArray[i]);

                                        lock (zePremaknjeniStolpi)
                                        {
                                            zePremaknjeniStolpi.Add(pozicijaByteArray[i]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                );
                seznamPredhodnihPozicij = new HashSet<int>(seznamTrenutnihPozicij);
                seznamTrenutnihPozicij = new HashSet<int>(seznamNovihPozicij);

                // Garbage collector, ki vrne porabljen ram samo za ta program, ne gleda porabe rama od ostalih aplikacij.
                long tmpMem = GC.GetTotalMemory(false);
                if (tmpMem > mem)
                {
                    mem = tmpMem;
                }
                
                seznamNovihPozicij.Clear();
                steviloPonovitev += 1;
                //Console.WriteLine($"Stevilo ponovitev: {steviloPonovitev}");
            }
            Console.WriteLine($"Stevilo ponovitev: {steviloPonovitev}");
            Console.WriteLine($"Max memory = {mem / 1000000} MB");
        }
    }



    public class Hanoi_K4 : HanoiTower
    {
        public Hanoi_K4(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (0, 1), (1, 0), (0, 2), (2, 0), (3, 0), (0, 3), (1, 2), (2, 1), (2, 3), (3, 2), (1, 3), (3, 1) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }




    public class Hanoi_K13e : HanoiTower
    {
        public Hanoi_K13e(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (0, 1), (1, 0), (0, 2), (2, 0), (3, 0), (0, 3), (2, 3), (3, 2) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }




    public class Hanoi_K13 : HanoiTower
    {
        public Hanoi_K13(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (0, 1), (1, 0), (0, 2), (2, 0), (3, 0), (0, 3) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }





    public class Hanoi_K4Ne : HanoiTower
    {
        public Hanoi_K4Ne(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (0, 1), (1, 0), (0, 2), (2, 0), (3, 0), (0, 3), (1, 2), (2, 1), (1, 3), (3, 1) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }



    public class Hanoi_C4 : HanoiTower
    {
        public Hanoi_C4(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (0, 2), (2, 0), (3, 0), (0, 3), (1, 2), (2, 1), (1, 3), (3, 1) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }


    public class Hanoi_P13 : HanoiTower
    {
        public Hanoi_P13(int discs, int pegs, byte start, byte final) : base(discs, pegs, start, final)
        {

        }

        // Povozimo povezave iz nadrejenega razreda. Podamo ustrezne povezave.
        // Set spustimo, da ne moremo spreminjati povezav, saj so fiksne glede na ime razreda.
        public override HashSet<(int, int)> Povezave
        {
            get
            {
                return new HashSet<(int, int)>() { (3, 0), (0, 3), (1, 2), (2, 1), (2, 3), (3, 2) };
            }

        }

        // Povozimo osrednjo metodo iz glavnega razreda, ki za posamezen tip grafa vrne dolžino najkrajše poti za vse variante grafa.
        public override void DolzinaNajkrajsePoti(byte[] state)
        {
            base.DolzinaNajkrajsePoti(state);
        }
    }


    /// <summary>
    /// Nacrtovalski vzorec Factory, ki nam ustvari instanco izbranega stolpa.
    /// </summary>
    static class HanoiTowerFactory
    {
        public static HanoiTower GetHanoiTower(string izbira, int discs, byte start, byte final)
        {
            int pegs = 4;
            HanoiTower hanoiTower = null;

            // Ustvarimo instanco glede na izbrani tip stolpa
            switch (izbira)
            {
                case "5":
                    hanoiTower = new Hanoi_C4(discs, pegs, start, final);
                    break;
                case "3":
                    hanoiTower = new Hanoi_K13(discs, pegs, start, final);
                    break;
                case "2":
                    hanoiTower = new Hanoi_K13e(discs, pegs, start, final);
                    break;
                case "1":
                    hanoiTower = new Hanoi_K4(discs, pegs, start, final);
                    break;
                case "4":
                    hanoiTower = new Hanoi_K4Ne(discs, pegs, start, final);
                    break;
                case "6":
                    hanoiTower = new Hanoi_P13(discs, pegs, start, final);
                    break;
            }

            return hanoiTower;
        }
    }
}
