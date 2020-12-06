using System;
namespace HanoiTower
{
    public class ConvertNumbers
    {
        public ConvertNumbers()
        {
        }


        public static int Potenca(int x, int y)
        {
            int potenca = 1;
            while (y != 0)
            {
                potenca = potenca * x;
                y--;
            }
            return potenca;

        }

        
        public static int TetraToDecimal(byte[] positionByteList)
        {
            int decimalNumber = 0;
            int len = positionByteList.Length;
            for (int i = 0; i<len; i++)
            {
                decimalNumber += positionByteList[i] * Potenca(4, len - i -1);

            }
            return decimalNumber;
        }

        public static byte[] DecimalToTetra(int x, int discs)
        {
            byte[] stanje = new byte[discs];
            int i = 0;
            while (x != 0)
            {
                int ostanek = x % 4;
                x = (int)x / 4;
                stanje[i] = Convert.ToByte(ostanek);
                i++;

            }
            Array.Reverse(stanje);
            return stanje;
        }

        public static byte[] StartEndPosition(byte start, int discs)
        {
            byte[] startPosition = new byte[discs];
            int i = 0;
            while (discs > 0)
            {
                startPosition[i] = start;
                discs--;
                i++;
            }
            return startPosition;
        }

    }
}
