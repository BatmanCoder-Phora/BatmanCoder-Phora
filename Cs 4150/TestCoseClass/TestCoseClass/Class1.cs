using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCoseClass
{
    public  class Class1
    {

      public static void Main(string[] args)
        {
            string[] vacation = { "in", "R", "in", "R", "R", "in", "R" };
            testvaction(vacation);
            Console.ReadLine();
        }

        // Problem set one: Q3//
        public static void testvaction(string[] vaction)
        {
            int n = vaction.Length;
            int ni = n - 2;
            int subi = 0;
            string replace = "";
            for (int i = 0; i < ni; i++)
            {
                subi = n - i - 2;
                for (int j = 0; j < subi; j++)
                {
                    if (vaction[j] == "R" && vaction[j + 1] == "in")
                    {
                        replace = vaction[j + 1];
                        vaction[j + 1] = vaction[j];
                        vaction[j] = replace;
                    }

                }
            }

        }

    }

}
