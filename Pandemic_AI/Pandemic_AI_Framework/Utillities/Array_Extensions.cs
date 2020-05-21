using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class Array_Extensions
    {
        public static bool ArrayEqual(this double[] first, double[] second)
        {
            if (first == second)
                return true;
            if (first == null || second == null)
                return false;
            if (first.Length != second.Length)
                return false;
            for (var i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the number of rows of a 2d array of any type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array2D"></param>
        /// <returns></returns>
        public static int NumRows<T>(this T[,] array2D)
        {
            return array2D.GetLength(0);
        }

        public static int NumColumns<T>(this T[,] array2D)
        {
            return array2D.GetLength(1);
        }

        public static int Height<T>(this T[,] array2D)
        {
            return NumRows(array2D);
        }

        public static int Width<T>(this T[,] array2D)
        {
            return NumColumns(array2D);
        }

        public static double[,] Squared(this double[,] originalArray)
        {
            double[,] newArray = originalArray.CustomDeepCopy();
            for (int i = 0; i < originalArray.Height(); i++)
            {
                for (int j = 0; j < originalArray.Width(); j++)
                {
                    newArray[i, j] = originalArray[i, j] * originalArray[i, j];
                }
            }
            return newArray;
        }

        public static int[,] Squared(this int[,] originalArray)
        {
            int[,] newArray = originalArray.CustomDeepCopy();
            for (int i = 0; i < originalArray.Height(); i++)
            {
                for (int j = 0; j < originalArray.Width(); j++)
                {
                    newArray[i, j] = originalArray[i, j] * originalArray[i, j];
                }
            }
            return newArray;
        }

        /// <summary>
        /// Returns a specific row of a 2d array, as a new 1d array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array2D"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static T[] Row<T>(this T[,] array2D, int rowIndex)
        {
            int height = array2D.Height();
            int width = array2D.Width();
            if (rowIndex < 0 || rowIndex > height)
            {
                throw new System.Exception("invalid row index");
            }
            T[] rowToReturn = new T[width];
            for (int x = 0; x < width; x++)
            {
                rowToReturn[x] = array2D[rowIndex, x];
            }
            return rowToReturn;
        }

        public static T[] Column<T>(this T[,] array2D, int columnIndex)
        {
            int height = array2D.Height();
            int width = array2D.Width();
            if (columnIndex < 0 || columnIndex > width)
            {
                throw new System.Exception("invalid row index");
            }
            T[] columnToReturn = new T[height];
            for (int y = 0; y < height; y++)
            {
                columnToReturn[y] = array2D[y, columnIndex];
            }
            return columnToReturn;
        }

        public static double[] DivideBy(this double[] array, double divisor)
        {
            if (divisor == 0.0)
            {
                throw new System.Exception("cannot divide by zero!");
            }
            double[] dividedArray = array;
            for (int i = 0; i < array.Length; i++)
            {
                dividedArray[i] /= divisor;
            }
            return dividedArray;
        }

        public static double[] MultiplyBy(this double[] array, double multiplier)
        {
            double[] dividedArray = array;
            for (int i = 0; i < array.Length; i++)
            {
                dividedArray[i] /= multiplier;
            }
            return dividedArray;
        }

        public static double Max2D(this double[,] array2D)
        {
            double maxValue = -Double.MaxValue;
            int numRows = array2D.GetLength(0);
            int numColumns = array2D.GetLength(1);
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < numColumns; colIndex++)
                {
                    if (array2D[rowIndex, colIndex] > maxValue)
                    {
                        maxValue = array2D[rowIndex, colIndex];
                    }
                }
            }
            return maxValue;
        }

        public static double RowMax(this double[,] array2D, int rowIndex)
        {
            return array2D.Row(rowIndex).Max();
        }

        public static double ColumnMax(this double[,] array2D, int columnIndex)
        {
            return array2D.Column(columnIndex).Max();
        }

        public static double[] RowMax_PerRow(this double[,] array2D)
        {
            int numRows = array2D.NumRows();
            double[] rowMax_PerRow = new double[numRows];
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                rowMax_PerRow[rowIndex] = array2D.Row(rowIndex).Max();
            }
            return rowMax_PerRow;
        }

        public static double[] ColumnMax_PerColumn(this double[,] array2D)
        {
            int numColumns = array2D.NumColumns();
            double[] columnMax_PerColumn = new double[numColumns];
            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                columnMax_PerColumn[columnIndex] = array2D.Column(columnIndex).Max();
            }
            return columnMax_PerColumn;
        }

        public static double RowSum(this double[,] array2D, int rowIndex)
        {
            return array2D.Row(rowIndex).Sum();
        }

        public static double ColumnSum(this double[,] array2D, int columnIndex)
        {
            return array2D.Column(columnIndex).Sum();
        }

        public static double[] RowSum_PerRow(this double[,] array2D)
        {
            int numRows = array2D.NumRows();
            double[] rowSum_PerRow = new double[numRows];
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                rowSum_PerRow[rowIndex] = array2D.Row(rowIndex).Sum();
            }
            return rowSum_PerRow;
        }

        public static double[] ColumnSum_PerColumn(this double[,] array2D)
        {
            int numColumns = array2D.NumColumns();
            double[] columnSum_PerColumn = new double[numColumns];
            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                columnSum_PerColumn[columnIndex] = array2D.Column(columnIndex).Sum();
            }
            return columnSum_PerColumn;
        }

        public static double RowAverage(this double[,] table2D, int rowIndex)
        {
            return table2D.Row(rowIndex).Average();
        }

        public static double ColumnAverage(this double[,] table2D, int columnIndex)
        {
            return table2D.Column(columnIndex).Average();
        }


        public static int[] CustomDeepCopy(this int[] originalArray)
        {
            int[] newArray = new int[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
            {
                newArray[i] = originalArray[i];
            }
            return newArray;
        }

        public static double[] CustomDeepCopy(this double[] originalArray)
        {
            double[] newArray = new double[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
            {
                newArray[i] = originalArray[i];
            }
            return newArray;
        }

        public static int[,] CustomDeepCopy(this int[,] originalArray)
        {
            int[,] newArray = new int[originalArray.NumRows(), originalArray.NumColumns()];
            for (int rowIndex = 0; rowIndex < originalArray.NumRows(); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < originalArray.NumColumns(); columnIndex++)
                {
                    newArray[rowIndex, columnIndex] = originalArray[rowIndex, columnIndex];
                }
            }
            return newArray;
        }

        public static double[,] CustomDeepCopy(this double[,] originalArray)
        {
            double[,] newArray = new double[originalArray.NumRows(), originalArray.NumColumns()];
            for (int rowIndex = 0; rowIndex < originalArray.NumRows(); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < originalArray.NumColumns(); columnIndex++)
                {
                    newArray[rowIndex, columnIndex] = originalArray[rowIndex, columnIndex];
                }
            }
            return newArray;
        }

        public static double[,] Multiplied_By(this double[,] originalArray, double multiplier)
        {
            double[,] newArray = new double[originalArray.NumRows(), originalArray.NumColumns()];
            for (int rowIndex = 0; rowIndex < originalArray.NumRows(); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < originalArray.NumColumns(); columnIndex++)
                {
                    newArray[rowIndex, columnIndex] = originalArray[rowIndex, columnIndex] * multiplier;
                }
            }
            return newArray;
        }

        public static double[,] Divided_By(this double[,] originalArray, double divisor)
        {
            if (divisor == 0)
            {
                throw new System.Exception("division by zero!");
            }
            double[,] newArray = new double[originalArray.NumRows(), originalArray.NumColumns()];
            for (int rowIndex = 0; rowIndex < originalArray.NumRows(); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < originalArray.NumColumns(); columnIndex++)
                {
                    newArray[rowIndex, columnIndex] = originalArray[rowIndex, columnIndex] / divisor;
                }
            }
            return newArray;
        }

        public static double[] Multiplied_By(this double[] originalArray, double multiplier)
        {
            double[] newArray = new double[originalArray.Length];

            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = originalArray[i] * multiplier;
            }

            return newArray;
        }

        #region integer array extensions
        public static int[] RowMax_PerRow(this int[,] array2D)
        {
            int numRows = array2D.NumRows();
            int[] rowMax_PerRow = new int[numRows];
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                rowMax_PerRow[rowIndex] = array2D.Row(rowIndex).Max();
            }
            return rowMax_PerRow;
        }

        public static int[] ColumnMax_PerColumn(this int[,] array2D)
        {
            int numColumns = array2D.NumColumns();
            int[] columnMax_PerColumn = new int[numColumns];
            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                columnMax_PerColumn[columnIndex] = array2D.Column(columnIndex).Max();
            }
            return columnMax_PerColumn;
        }

        public static int[] RowSum_PerRow(this int[,] array2D)
        {
            int numRows = array2D.NumRows();
            int[] rowSum_PerRow = new int[numRows];
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                rowSum_PerRow[rowIndex] = array2D.Row(rowIndex).Sum();
            }
            return rowSum_PerRow;
        }

        public static int[] ColumnSum_PerColumn(this int[,] array2D)
        {
            int numColumns = array2D.NumColumns();
            int[] columnSum_PerColumn = new int[numColumns];
            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                columnSum_PerColumn[columnIndex] = array2D.Column(columnIndex).Sum();
            }
            return columnSum_PerColumn;
        }
        #endregion
    }
}
