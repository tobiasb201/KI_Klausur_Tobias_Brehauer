using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KI_Klausur_Tobias_Brehauer
{
    class Parser
    {
        private List<List<string>> dimList= new List<List<string>>();
        private List<int> numbers = new List<int>();
        private List<string> stringData = new List<string>();
        private double[,] constrainArray;

        public void readFile(string filePath)
        {
            stringData = File.ReadAllLines(filePath+".txt").ToList();
            parseNumbers();
        }
        public void printConstrains(string[] stringArray)
        {
            foreach (string row in stringArray)
            {
                Console.WriteLine(row);
            }

        }

        public void parseNumbers()
        {
            deleteUnnecessary();
            //OF equals to 0
            dimList[0].Add("0");
            foreach (var row in dimList)
            {
                foreach (var number in row)
                {
                    numbers.Add(int.Parse(number));
                }
            }
        }

        public List<List<String>> deleteUnnecessary()
        {
            string[] stringArray = new string[stringData.Count];
            stringData.RemoveAt(0); //Objective function
            stringData.RemoveAt(1); //contraints
            stringArray = stringData.ToArray();
            printConstrains(stringArray);
            for (int i = 0; i < stringArray.Length; i++)
            {
                char[] delete = new char[] {' ', '=', '*', ';', '+', '>', ':','m','i','n'};//Löscht die Felder
                stringArray[i] = Regex.Replace(stringArray[i],@"x...","");
                stringArray[i] = Regex.Replace(stringArray[i],@"x.*?;","");
                dimList.Add(stringArray[i].Split(delete, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
            return dimList;
        }

        public double[,] intoArray()
        {
            int zeile = dimList.Count();//Zeilenlänge
            int spalte = dimList[0].Count();//Spaltenlänge
            constrainArray = new double[zeile, spalte];
            int index = 0;
            for (int i = 0; i < zeile ; i++)
            {
                for (int y = 0; y < spalte ; y++)
                {
                    constrainArray[i, y] = numbers.ToArray()[index];
                    index++;
                }
            }
            //Swap OF from top to bottom
            swap(constrainArray);
            
            return constrainArray;
        }

        public double[,] swap(double[,] constrainArray)
        {
            double[,] temp = new double[dimList.Count(), dimList[0].Count()];
            for (int i = 0; i < constrainArray.GetLength(0); i++) //Reihe
            {
                for (int y = 0; y < constrainArray.GetLength(1); y++) //Spalte
                    temp[i, y] = constrainArray[i, y];
            }
            for (int i = 0; i < constrainArray.GetLength(0) - 1; i++)
            {
                for (int y = 0; y < constrainArray.GetLength(1); y++) //Jede Zeile +1
                    constrainArray[i, y] = temp[i + 1, y];
            }
            for (int i = 0; i < constrainArray.GetLength(1); i++)
                constrainArray[temp.GetLength(0) - 1, i] = temp[0, i];
            return constrainArray;
        }
    }
}
