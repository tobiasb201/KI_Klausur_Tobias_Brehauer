using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Klausur_Tobias_Brehauer
{
    class Calculate
    {

        private double[,] result;
        public Boolean finish = false;

        public double[,] matrixSwitch(double[,] constrainArray)// array[,] = Reihe,Spalte
        {
            double[,] result = new double[constrainArray.GetLength(1), constrainArray.GetLength(0)]; //Transponierte Matrix
            String tmp = "";
            Console.WriteLine("Matrix:");
            for (int i = 0; i < constrainArray.GetLength(0); i++) //Reihe
            {
                for (int j = 0; j < constrainArray.GetLength(1); j++) //Spalte
                {
                    result[j, i] = constrainArray[i, j];
                    tmp = tmp + constrainArray[i, j] + " ";
                }
                tmp = tmp + "\n";
            }
            Console.WriteLine(tmp);
            this.result = result;
            matrixToString(result);
            return result;
        }
        public void matrixToString(double[,] matrix)
        {
            int row = matrix.GetLength(0);
            int colum = matrix.GetLength(1);

            String tmp = "";
            Console.WriteLine("Transponierte Matrix:");
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < colum; x++)
                    tmp = tmp + matrix[y, x] + " ";

                tmp = tmp + "\n";
            }
            Console.WriteLine(tmp);
        }

        public double[,] addSlack()
        {
            double[,] newSlackMatrix = new double[result.GetLength(0), (result.GetLength(0) + result.GetLength(1))-1];

            int counter = 0;
            int columCounter = 0;
            double[] save = new double[result.GetLength(0)];//Speichert die Ergebnise der Constrains

            for (int i = 0; i < result.GetLength(0); i++) //Reihe
            {
                for (int y = 0; y < result.GetLength(1); y++) //Spalte
                {
                    newSlackMatrix[i, y] = result[i, y]; //Schreibt das resultArray in das neue Array für die Slack Var.
                    if (y == result.GetLength(1) - 1)
                    {
                        save[counter] = newSlackMatrix[i, y]; //Speichert das constrain Ergebnis
                        counter++;
                    }
                }
            }
            for (int z = 0; z < result.GetLength(0); z++) //Geht durch alle Reihen
            {
                newSlackMatrix[z, result.GetLength(1) - 1] = 0;
                if (z != result.GetLength(0) - 1)
                {
                    newSlackMatrix[z, result.GetLength(1) - 1 + columCounter] = 1; //Speichert die 1 Diagonal an die constraints
                    newSlackMatrix[z, newSlackMatrix.GetLength(1) - 1] = save[columCounter];
                    columCounter++;
                }
            }
            matrixToString(newSlackMatrix);
            return newSlackMatrix;
        }

        public double[,] negateOF(double[,] matrixArray)
        {
            for(int i =0;i< matrixArray.GetLength(1)-1; i++)
            {
                double save =matrixArray[matrixArray.GetLength(0) - 1, i] * -1;
                matrixArray[matrixArray.GetLength(0) - 1, i] = save;
            }

            matrixToString(matrixArray);
            return matrixArray;
        }

        public double[,] calculateMax(double[,] matrixArray)
        {
            double minimum = 0;
            int pivotColum = 0;
            int pivotRow = 0;
            double multi = 0;
            double[,] temp = new double[matrixArray.GetLength(0), matrixArray.GetLength(1)];

            for (int i = 0; i < matrixArray.GetLength(0); i++)
            {
                if (i == matrixArray.GetLength(0) - 1)
                {
                    minimum = matrixArray[i, pivotColum];
                    for (int y = 0; y < result.GetLength(1) - 1; y++)
                        if (minimum > matrixArray[i, y])
                        {
                            minimum = matrixArray[i, y];
                            pivotColum = y;
                        }
                }
            }
            //Erechne das Privotelement
            double pivotElement = matrixArray[0, pivotColum] / matrixArray[0, matrixArray.GetLength(1) - 1];
            for (int i = 0; i < matrixArray.GetLength(0) - 1; i++) //Letzte Reihe ist die OF, deshalb -1
            {
                double pivotingElement = matrixArray[i, matrixArray.GetLength(1) - 1] / matrixArray[i, pivotColum];
                if (pivotElement > pivotingElement)
                {
                    pivotElement = pivotingElement;
                    pivotRow = i;
                }
            }
            //Setze die Pivotspalte auf 1
            //Errechne den multiplier aller anderen Zeilen
            Console.WriteLine(matrixArray[pivotRow, pivotColum]);
            for (int o = 0; o < matrixArray.GetLength(0); o++)
            {
                for (int j = 0; j < matrixArray.GetLength(1); j++)
                    if (o == pivotRow)
                    {
                        temp[o, j] = matrixArray[pivotRow, j] / matrixArray[pivotRow, pivotColum];
                    }
                    else
                    {
                        multi = matrixArray[o, pivotColum] / matrixArray[pivotRow, pivotColum];
                        temp[o, j] = Math.Round(matrixArray[o, j] - (multi * matrixArray[pivotRow, j]), 4);
                    }

            }
            matrixArray = temp;
            matrixToString(matrixArray);
            finish = finished(matrixArray);
            return matrixArray;
            
        }
        public Boolean finished(double[,] matrixArray)
        {
            for (int repeat = 0; repeat < matrixArray.GetLength(1) - 1; repeat++)
            {
                if (matrixArray[matrixArray.GetLength(0) - 1, repeat] < 0) //Checkt ob OF variable kleiner 0 ist
                {
                    repeat = 0;
                    return false;
                }
            }
            Console.WriteLine("Ergebnis gefunden!");
            int index = 0;
            for(int i = result.GetLength(1)-1; i < matrixArray.GetLength(1); i++)
            {
                if (i == matrixArray.GetLength(1) - 1)
                {
                    Console.WriteLine("max:" + matrixArray[matrixArray.GetLength(0) - 1, i]);
                }
                else{
                    Console.WriteLine("x"+index+":"+matrixArray[matrixArray.GetLength(0) - 1, i]);
                    index++;
                }
            }
            return true;
        }
    }       
}
