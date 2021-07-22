using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Klausur_Tobias_Brehauer
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser paser = new Parser();
            Calculate calculator = new Calculate();
            Console.WriteLine("Type the complete File Path into Console:");
            string name = Console.ReadLine();
            paser.readFile(name);
            Console.WriteLine("\n");
            double[,] array = paser.intoArray();
            calculator.matrixSwitch(array);
            double[,] matrix = calculator.addSlack();
            double[,] negate = calculator.negateOF(matrix);
            Console.WriteLine("Enter for calculation");
            Console.ReadLine();
            double [,] calcArray = calculator.calculateMax(negate);
            while (calculator.finish==false)
            {
                calculator.calculateMax(calcArray);
            }

            Console.ReadLine();
        }
    }
}
