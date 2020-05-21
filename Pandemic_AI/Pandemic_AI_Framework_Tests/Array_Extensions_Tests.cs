using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace VariousUtilities_Tests
{
    [TestClass()]
    public class Array_Extensions_Tests
    {
        [TestMethod()]
        public void NumRows_Test()
        {
            double[,] array2D = new double[2, 4];
            int numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 2);

            array2D = new double[2, 3];
            numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 2);

            array2D = new double[1, 3];
            numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 1);

            array2D = new double[100, 20];
            numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 100);

            array2D = new double[,] {
                {0, 1},
                {2, 3},
                {4, 5}
            };
            numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 3);

            array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            numRows = array2D.NumRows();
            Assert.IsTrue(numRows == 3);
        }

        [TestMethod()]
        public void NumColumns_Test()
        {
            double[,] array2D = new double[2, 4];
            int numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 4);

            array2D = new double[2, 3];
            numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 3);

            array2D = new double[1, 3];
            numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 3);

            array2D = new double[100, 20];
            numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 20);

            array2D = new double[,] {
                {0, 1},
                {2, 3},
                {4, 5}
            };
            numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 2);

            array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            numColumns = array2D.NumColumns();
            Assert.IsTrue(numColumns == 4);
        }

        [TestMethod()]
        public void Height_Test()
        {
            double[,] array2D = new double[2, 4];
            int length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 2);

            array2D = new double[2, 3];
            length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 2);

            array2D = new double[1, 3];
            length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 1);

            array2D = new double[100, 20];
            length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 100);

            array2D = new double[,] {
                {0, 1},
                {2, 3},
                {4, 5}
            };
            length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 3);

            array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            length_Y = array2D.Height();
            Assert.IsTrue(length_Y == 3);
        }

        [TestMethod()]
        public void Width_Test()
        {
            double[,] array2D = new double[2, 4];
            int length_X = array2D.Width();
            Assert.IsTrue(length_X == 4);

            array2D = new double[2, 3];
            length_X = array2D.Width();
            Assert.IsTrue(length_X == 3);

            array2D = new double[1, 3];
            length_X = array2D.Width();
            Assert.IsTrue(length_X == 3);

            array2D = new double[100, 20];
            length_X = array2D.Width();
            Assert.IsTrue(length_X == 20);

            array2D = new double[,] {
                {0, 1},
                {2, 3},
                {4, 5}
            };
            length_X = array2D.Width();
            Assert.IsTrue(length_X == 2);

            array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            length_X = array2D.Width();
            Assert.IsTrue(length_X == 4);
        }

        [TestMethod()]
        public void Row_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };

            double[] row = array2D.Row(0);
            double[] testRow = new double[] { 0, 1, 2, 3 };
            Assert.IsTrue(row.SequenceEqual(testRow));

            row = array2D.Row(1);
            testRow = new double[] { 4, 5, 6, 7 };
            Assert.IsTrue(row.SequenceEqual(testRow));

            row = array2D.Row(2);
            testRow = new double[] { 8, 9, 10, 11 };
            Assert.IsTrue(row.SequenceEqual(testRow));
        }

        [TestMethod()]
        public void Column_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };

            double[] column = array2D.Column(0);
            double[] testColumn = new double[] { 0, 4, 8 };
            Assert.IsTrue(column.SequenceEqual(testColumn));

            column = array2D.Column(1);
            testColumn = new double[] { 1, 5, 9 };
            Assert.IsTrue(column.SequenceEqual(testColumn));

            column = array2D.Column(2);
            testColumn = new double[] { 2, 6, 10 };
            Assert.IsTrue(column.SequenceEqual(testColumn));

            column = array2D.Column(3);
            testColumn = new double[] { 3, 7, 11 };
            Assert.IsTrue(column.SequenceEqual(testColumn));
        }

        [TestMethod()]
        public void RowMax_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };

            double rowMax = array2D.RowMax(0);
            Assert.IsTrue(rowMax == 3);

            rowMax = array2D.RowMax(1);
            Assert.IsTrue(rowMax == 7);

            rowMax = array2D.RowMax(2);
            Assert.IsTrue(rowMax == 11);

            array2D = new double[,] {
                {0, 1, 2, 0},
                {7, 5, 6, 7},
                {8, 9, 25, 11}
            };

            rowMax = array2D.RowMax(0);
            Assert.IsTrue(rowMax == 2);

            rowMax = array2D.RowMax(1);
            Assert.IsTrue(rowMax == 7);

            rowMax = array2D.RowMax(2);
            Assert.IsTrue(rowMax == 25);
        }

        [TestMethod()]
        public void ColumnMax_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };

            double columnMax = array2D.ColumnMax(0);
            Assert.IsTrue(columnMax == 8);

            columnMax = array2D.ColumnMax(1);
            Assert.IsTrue(columnMax == 9);

            columnMax = array2D.ColumnMax(2);
            Assert.IsTrue(columnMax == 10);

            columnMax = array2D.ColumnMax(3);
            Assert.IsTrue(columnMax == 11);

            array2D = new double[,] {
                {0, 1, 2, 0},
                {7, 5, 6, 7},
                {8, 9, 25, 11}
            };

            columnMax = array2D.ColumnMax(0);
            Assert.IsTrue(columnMax == 8);

            columnMax = array2D.ColumnMax(1);
            Assert.IsTrue(columnMax == 9);

            columnMax = array2D.ColumnMax(2);
            Assert.IsTrue(columnMax == 25);

            columnMax = array2D.ColumnMax(3);
            Assert.IsTrue(columnMax == 11);
        }

        [TestMethod()]
        public void RowMax_PerRow_Double_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            double[] rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new double[] { 3, 7, 11 }));

            array2D = new double[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new double[] { 4, 8, 23 }));

            array2D = new double[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new double[] { 22, 28, 82 }));
        }

        [TestMethod()]
        public void RowMax_PerRow_Int_Test()
        {
            int[,] array2D = new int[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            int[] rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new int[] { 3, 7, 11 }));

            array2D = new int[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new int[] { 4, 8, 23 }));

            array2D = new int[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            rowMax_perRow = array2D.RowMax_PerRow();
            Assert.IsTrue(rowMax_perRow.SequenceEqual(new int[] { 22, 28, 82 }));
        }

        [TestMethod()]
        public void ColumnMax_PerColumn_Double_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            double[] columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new double[] { 8, 9, 10, 11 }));

            array2D = new double[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new double[] { 8, 23, 10, 11 }));

            array2D = new double[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new double[] { 82, 23, 28, 11 }));
        }

        [TestMethod()]
        public void ColumnMax_PerColumn_Int_Test()
        {
            int[,] array2D = new int[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            int[] columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new int[] { 8, 9, 10, 11 }));

            array2D = new int[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new int[] { 8, 23, 10, 11 }));

            array2D = new int[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            columnMax_PerColumn = array2D.ColumnMax_PerColumn();
            Assert.IsTrue(columnMax_PerColumn.SequenceEqual(new int[] { 82, 23, 28, 11 }));
        }

        [TestMethod()]
        public void RowSum_PerRow_Double_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            double[] rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new double[] { 6, 22, 38 }));

            array2D = new double[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new double[] { 9, 24, 52 }));

            array2D = new double[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new double[] { 31, 44, 126 }));
        }

        [TestMethod()]
        public void RowSum_PerRow_Int_Test()
        {
            int[,] array2D = new int[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            int[] rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new int[] { 6, 22, 38 }));

            array2D = new int[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new int[] { 9, 24, 52 }));

            array2D = new int[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            rowSum_perRow = array2D.RowSum_PerRow();
            Assert.IsTrue(rowSum_perRow.SequenceEqual(new int[] { 31, 44, 126 }));
        }

        [TestMethod()]
        public void ColumnSum_PerColumn_Double_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            double[] columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new double[] { 12, 15, 18, 21 }));

            array2D = new double[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new double[] { 12, 32, 20, 21 }));

            array2D = new double[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new double[] { 108, 32, 40, 21 }));
        }

        [TestMethod()]
        public void ColumnSum_PerColumn_Int_Test()
        {
            int[,] array2D = new int[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };
            int[] columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new int[] { 12, 15, 18, 21 }));

            array2D = new int[,] {
                {0, 4, 2, 3},
                {4, 5, 8, 7},
                {8, 23, 10, 11}
            };
            columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new int[] { 12, 32, 20, 21 }));

            array2D = new int[,] {
                {22, 4, 2, 3},
                {4, 5, 28, 7},
                {82, 23, 10, 11}
            };
            columnSum_perColumn = array2D.ColumnSum_PerColumn();
            Assert.IsTrue(columnSum_perColumn.SequenceEqual(new int[] { 108, 32, 40, 21 }));
        }

        [TestMethod()]
        public void ArrayEqual_Test()
        {
            double[] array1 = new double[] { 0, 1, 2, 3 };
            double[] array2 = new double[] { 0, 1, 2, 3 };
            Assert.IsTrue(array1.ArrayEqual(array2));

            array1 = new double[] { 0, 1, 2, 3 };
            array2 = new double[] { 0, 1, 2, 4 };
            Assert.IsTrue(array1.ArrayEqual(array2) == false);

            array1 = new double[] { 0, 1, 2, 3, 4 };
            array2 = new double[] { 0, 1, 2, 3 };
            Assert.IsTrue(array1.ArrayEqual(array2) == false);
        }

        [TestMethod()]
        public void General_Array_Test()
        {
            double[,] array2D = new double[,] {
                {0, 1, 2, 3},
                {4, 5, 6, 7},
                {8, 9, 10, 11}
            };

            double element = array2D[0, 0];
            Assert.IsTrue(element == 0);

            element = array2D[1, 0];
            Assert.IsTrue(element == 4);

            element = array2D[2, 0];
            Assert.IsTrue(element == 8);


            element = array2D[0, 1];
            Assert.IsTrue(element == 1);

            element = array2D[1, 1];
            Assert.IsTrue(element == 5);

            element = array2D[2, 1];
            Assert.IsTrue(element == 9);


            element = array2D[0, 2];
            Assert.IsTrue(element == 2);

            element = array2D[1, 2];
            Assert.IsTrue(element == 6);

            element = array2D[2, 2];
            Assert.IsTrue(element == 10);


            element = array2D[0, 3];
            Assert.IsTrue(element == 3);

            element = array2D[1, 3];
            Assert.IsTrue(element == 7);

            element = array2D[2, 3];
            Assert.IsTrue(element == 11);
        }

        [TestMethod()]
        public void CustomDeepCopy_Int_1D_Test()
        {
            int[] originalArray = new int[] { 0, 1, 2, 3 };
            int[] copyArray = originalArray.CustomDeepCopy();
            copyArray[0] = 1;
            Assert.IsTrue(originalArray[0] == 0);

            int[] refArray = originalArray;
            refArray[0] = 1;
            Assert.IsTrue(originalArray[0] == 1);
        }

        [TestMethod()]
        public void CustomDeepCopy_Double_1D_Test()
        {
            double[] originalArray = new double[] { 0, 1, 2, 3 };
            double[] copyArray = originalArray.CustomDeepCopy();
            copyArray[0] = 1;
            Assert.IsTrue(originalArray[0] == 0);

            double[] refArray = originalArray;
            refArray[0] = 1;
            Assert.IsTrue(originalArray[0] == 1);
        }

        [TestMethod()]
        public void CustomDeepCopy_Int_2D_Test()
        {
            int[,] originalArray = new int[,] { { 0, 1 }, { 2, 3 } };
            int[,] copyArray = originalArray.CustomDeepCopy();
            copyArray[0, 0] = 1;
            Assert.IsTrue(originalArray[0, 0] == 0);

            int[,] refArray = originalArray;
            refArray[0, 0] = 1;
            Assert.IsTrue(originalArray[0, 0] == 1);
        }

        [TestMethod()]
        public void CustomDeepCopy_Double_2D_Test()
        {
            double[,] originalArray = new double[,] { { 0, 1 }, { 2, 3 } };
            double[,] copyArray = originalArray.CustomDeepCopy();
            copyArray[0, 0] = 1;
            Assert.IsTrue(originalArray[0, 0] == 0);

            double[,] refArray = originalArray;
            refArray[0, 0] = 1;
            Assert.IsTrue(originalArray[0, 0] == 1);
        }
    }
}