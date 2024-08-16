using Google.OrTools.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project.SolvingMethods
{
    internal class RevisedPrimal
    {
        private Model model;
        double[,] B; //column values for constraints s1, s2, s3
        double[,] BInv; //Inverse of B

        double[,] CBV; //S1, S2, S3 etc values for objective function
        double[,] CBVBInv; //Matrix multiplication of CBV and Binv 

        double[,] NBV; //Non basic variable in obj row  x1, x2, x3, x4
        double[,] C; //Co-efecients for deciding the entering variable

        double[,] RHS; //RHS values for the constraints



        public RevisedPrimal(Model model)
        {
            this.model = model;
        }

        public void Solve()
        {
            int iteration = 0;
            bool solved = false;

            Console.WriteLine("Given:");
            Console.WriteLine(model.ToString());
            Prepare();
            

            do
            {
                Console.WriteLine($"Itteration {iteration}");
                //This determines the enteing variable
                int width = C.GetLength(1);
                double[,] entering = new double[1, width];
                int enteringVar = 0; //Is the index of the column that is chosen as the entering variable
                Console.WriteLine("Entering Values");
                entering = Multiply(CBVBInv, C);
                
                for (int i = 0; i < width; i++)
                {
                    entering[0, i] = entering[0, i] - NBV[0, i];
                    Console.Write($"{entering[0, i]} ");
                    if (entering[0, i] < entering[0, enteringVar])
                    {
                        enteringVar = i;
                    }
                }
                
                /*
                bool pass = false;
                int enteringIndex = 0; //Is the index of the column that is chosen as the entering variable
                //Selects the first index value that is valid for the test
                int tempWidth = NBV.GetLength(1);
                for (int i = 0; i < width; i++)
                {
                    entering[0, i] = entering[0, i] - NBV[0, i];
                    Console.Write($"{entering[0, i]} ");
                    Console.WriteLine(entering[0, i]);
                    if (entering[0, i] > 0)
                    {
                        pass = true;
                        enteringIndex = i;
                        break;
                    }
                    else

                }

                if (!pass)
                {
                    solved = true;
                    Console.WriteLine("Finished");
                    break;
                }
                */



                Console.WriteLine();
                Console.WriteLine($"Entering var is: {entering[0, enteringVar]} with index: {enteringVar}");
                //Calculates the values for the constraint column that contains the leaving variable
                int hight = C.GetLength(0);
                double[,] a = new double[hight, 1];
                for (int i = 0; i < hight; i++)
                {
                    a[i, 0] = C[i, enteringVar];
                    //Console.WriteLine($"{a[i, 0]}");
                }
                double[,] myCol = new double[hight, 1];
                myCol = Multiply(B, a);
                Console.WriteLine("Column variables:");
                for (int i = 0; i < myCol.GetLength(0); i++)
                {
                    Console.WriteLine(myCol[i, 0]);
                }

                //Calculates the RHS variables
                RHS = Multiply(B, RHS);
                Console.WriteLine("RHS variables:");
                for (int i = 0; i < RHS.GetLength(0); i++)
                {

                    Console.WriteLine(RHS[i, 0]);
                }

                //Calculates the ratios to be used in the ratio test
                double[,] ratio = new double[hight, 1];
                Console.WriteLine("Ratio Variables:");
                for (int i = 0; i < hight; i++)
                {
                    ratio[i, 0] = RHS[i, 0] / a[i, 0];
                    Console.WriteLine(ratio[i, 0]);

                }

                //Performs the ratio test to determine the row that contains the leaving variable 
                bool pass = false;
                int ratioIndex = 0; //Is the index of the ratio that is chosen as the entering variable
                //Selects the first ratio value that is valid for the test
                //Console.WriteLine("Selection of first ratioIndex:")
                for (int i = 0; i < hight; i++)
                {
                    if (ratio[ratioIndex, 0] > 0)
                    {
                        pass = true;
                    }
                    else
                    {
                        ratioIndex++;
                    }
                }
                if (pass)
                {

                }


                for (int i = 0; i < hight; i++)
                {
                    if ((ratio[i, 0] < ratio[ratioIndex, 0]) && (ratio[i, 0] > 0))
                    {
                        ratioIndex = i;
                    }
                }

                Console.WriteLine($"Chosen ratio is: {ratio[ratioIndex, 0]} with index: {ratioIndex}");

                double holder = NBV[0, enteringVar];
                NBV[0, enteringVar] = CBV[0, ratioIndex];
                CBV[0, ratioIndex] = holder;

                //Prints the new values of NBV to console
                Console.WriteLine("New NBV: ");
                for (int i = 0; i < NBV.GetLength(0); i++)
                {
                    for (int j = 0; j < NBV.GetLength(1); j++)
                    {
                        Console.Write($"{NBV[i, j]} ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("New CBV: ");
                for (int i = 0; i < CBV.GetLength(0); i++)
                {
                    for (int j = 0; j < CBV.GetLength(1); j++)
                    {
                        Console.Write($"{CBV[i, j]} ");
                    }
                    Console.WriteLine();
                }
                



                //Saves the value of the leaving variable
                double myRatio = C[ratioIndex, enteringVar];
                //Updates B and then Inserts new values into the column that contains the leaving variable
                for (int i = 0; i < hight; i++)
                {
                    B[i, ratioIndex] = (C[i, enteringVar] /myRatio) * -1; 
                    C[i, enteringVar] = 0;
                }
                C[ratioIndex, enteringVar] = 1;

                Console.WriteLine("New C column for the leaving variable:");
                for (int i = 0; i < hight; i++)
                {
                    Console.WriteLine(C[i, enteringVar]);
                }

                Console.WriteLine("New B:");
                for (int i = 0; i < B.GetLength(1); i++)
                {
                    for (int j = 0; j < B.GetLength(0); j++)
                    {
                        Console.Write($"{B[i, j]} ");
                    }
                    Console.WriteLine("");
                }

                BInv = Invert(B);

                Console.WriteLine("New BInv:");
                for (int i = 0; i < BInv.GetLength(1); i++)
                {
                    for (int j = 0; j < BInv.GetLength(0); j++)
                    {
                        Console.Write($"{BInv[i, j]} ");
                    }
                    Console.WriteLine("");
                }

                iteration++;
                Console.WriteLine("----------------------\n");
            } while (!solved);
            
        }

        private void Prepare()
        {
            //INserts the constraint values for s1, S2, S3 etc into BV table
            int size = model.OperatorsConstraints.Length;
            B = new double[size, size];
            CBV = new double[1, size];
            NBV = new double[1, model.ObjFunction.Length];
            for (int i = 0; i < size; i++)
            {
                //Makes all the values in B = 0
                for (int j = 0; j < size; j++)
                {
                    B[i, j] = 0;
                }
                //Makes the values of B 1 where necesary
                B[i, i] = 1;

                //Fills in CBV values with 0;
                CBV[0, i] = 0;  
            }

            for (int i = 0; i < model.ObjFunction.Length; i++)
            {
                NBV[0, i] = model.ObjFunction[i];
            }

            //Prints the values of B to console
            Console.WriteLine("B: ");
            for (int i = 0; i < B.GetLength(0); i++)
            {
                for (int j = 0;  j< B.GetLength(1); j++)
                {
                    Console.Write($"{B[i, j]} ");
                }
                Console.WriteLine();
            }

            //Prints the values of NBV to console
            Console.WriteLine("NBV: ");
            for (int i = 0; i < NBV.GetLength(0); i++)
            {
                for (int j = 0; j < NBV.GetLength(1); j++)
                {
                    Console.Write($"{NBV[i, j]} ");
                }
                Console.WriteLine();
            }
            //Prints the values of CBV to console
            Console.WriteLine("CBV: ");
            for (int i = 0; i < CBV.GetLength(0); i++)
            {
                for (int j = 0; j < CBV.GetLength(1); j++)
                {
                    Console.Write($"{CBV[i, j]} ");
                }
                Console.WriteLine();
            }

            //Inverts B and assigns it to  BInv
            BInv = Invert(B);
            Console.WriteLine("BInv:");
            for(int i = 0; i < BInv.GetLength(1); i++)
            {
                for(int j = 0;j < BInv.GetLength(0); j++)
                {
                    Console.Write($"{BInv[i, j]} ");
                }
                Console.WriteLine("");
            }


            //Multiplies CBV and Binv
            CBVBInv = Multiply(CBV, BInv);
            Console.WriteLine("CBVBInv");
            for (int i = 0; i < CBVBInv.GetLength(0); i++)
            {
                for(int j = 0;j < CBVBInv.GetLength(1); j++)
                {
                    Console.Write($"{CBVBInv[i, j]} ");
                }
                Console.WriteLine();
            }

            //Gets C
            int height = model.ConstraintsCoefficients.GetLength(0);
            int lent = model.ConstraintsCoefficients.GetLength(1);
            C = new double[height, lent];
            Console.WriteLine("C: ");
            for (int i = 0;i < height; i++)
            {
                for (int j = 0; j < lent; j++)
                {
                    C[i, j] = model.ConstraintsCoefficients[i, j];
                    Console.Write($"{C[i, j]} ");
                }
                Console.WriteLine() ;
            }

            //Gets the initial values for thr RHS values
            RHS = new double[height, 1];
            for (int i = 0; i < height; i++)
            {
                RHS[i, 0] = model.RhsConstraints[i];
                //Console.WriteLine($"{a[i, 0]}");
            }
            Console.WriteLine("RHS:");
            for (int i = 0; i < RHS.GetLength(0); i++)
            {
                Console.WriteLine(RHS[i, 0]);
            }

        }

        private double[,] Invert(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result = new double[n, n];
            double[,] augmented = new double[n, 2 * n];

            // Create the augmented matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                }
                augmented[i, n + i] = 1;
            }

            // Perform Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                // Make the diagonal contain all 1's
                double diagElement = augmented[i, i];
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= diagElement;
                }

                // Make the other rows contain 0's in the current column
                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = augmented[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            augmented[k, j] -= factor * augmented[i, j];
                        }
                    }
                }
            }

            // Extract the inverse matrix from the augmented matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, n + j];
                }
            }

            return result;
        }

        private double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            var matrix1Rows = matrix1.GetLength(0);
            var matrix1Cols = matrix1.GetLength(1);
            var matrix2Rows = matrix2.GetLength(0);
            var matrix2Cols = matrix2.GetLength(1);

            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            double[,] newMatrix = new double[matrix1Rows, matrix2Cols];

            // looping through matrix 1 rows  
            //Console.WriteLine("Matrix mult response:");
            for (int matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                // for each matrix 1 row, loop through matrix 2 columns  
                for (int matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                {
                    // loop through matrix 1 columns to calculate the dot product  
                    for (int matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                    {
                        newMatrix[matrix1_row, matrix2_col] += matrix1[matrix1_row, matrix1_col] * matrix2[matrix1_col, matrix2_col];
                        //Console.Write($"{newMatrix[matrix1_row, matrix2_col]} ");
                    }
                    //Console.WriteLine();
                }
            }

            return newMatrix;
        }

        public override string ToString()
        {
            string myString = string.Empty;
            return base.ToString();
        }
    }

    

}
