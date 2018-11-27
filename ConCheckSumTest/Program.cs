using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCheckSumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string tString = "30h00h00h30h";

            string[] s = tString.Split('h');

            for (int i = 0; i < s.Length - 1 ; i++)
            {


                Console.WriteLine(s[i] + "/" + String.Format("{0:X}", Convert.ToInt32(s[i], 16)));
            }

            

            


        }
    }
}
