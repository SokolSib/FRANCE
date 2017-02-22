using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algoritmLuna
{
    class Program
    {
        public static int GetLuhnSecureDigital(string Num)
        {
            //Num[0..N-1] - card number
            //N - card number len
            //Num[N-1] - check digit 

            int p = 0;
            int sum = 0;
            int N = Num.Length;

            for (int i = 1; i < N; i++)
            {
                p = Num[(N - 1) - i] - '0';
                if (i % 2 != 0)
                {
                    p = 2 * p;
                    if (p > 9) p = p - 9;
                    
                }
                sum = sum + p;
            }
            sum = (10 - (sum % 10));
            return sum;
        }
        static void Main(string[] args)
        {
          //  Console.WriteLine("eter kode");
            
            decimal d = 091522556000;

            int i = 0;

            while (d < 9000000000000 )
            {
                d += 1;

                

                string s = d.ToString();

                int lastNum = int.Parse(s.Substring(s.Length - 1, 1)) + int.Parse(s.Substring(0, 1));

                if (lastNum == GetLuhnSecureDigital(s))
                {
                    Console.WriteLine(d);
                    i++;
                }
                if (i == 2500) break;
            
            }
            Console.ReadLine();
       }
    }
}
