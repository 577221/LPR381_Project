using LPR381_Project.SolvingMethods;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Sensitivity
    {
        private ModelInput modelInput;
        private Model model;
        private Primal primal;

        public Sensitivity(ModelInput modelInput)
        {
            this.modelInput = modelInput;
        }

        // Calculations
        // Identify BV & NBV & save them in cBV & cNBV and B & N arrays
        // cBV = Basic Variables coefficients & cNBV = Non-Basic Variables coefficients (e.g. 1)
        // B = Basic Variables Column values & N = Non-Basic Variables Column Values
        public void IdentifyBVNBV()
        {
            // Lists to store the indices of Basic Variables (BV) and Non-Basic Variables (NBV)
            List<int> BV = new List<int>();
            List<int> NBV = new List<int>();

            // Identify the basic variables by checking if the column in the constraints matrix
            // has exactly one '1' and the rest '0's
            for (int i = 0; i < modelInput.Constraints[0].Coefficients.Count; i++)
            {
                int count = 0;
                int index = -1;

                for (int j = 0; j < modelInput.Constraints.Count; j++)
                {
                    if (modelInput.Constraints[j].Coefficients[i] == 1)
                    {
                        count++;
                        index = j;
                    }
                    else if (modelInput.Constraints[j].Coefficients[i] != 0)
                    {
                        count = -1; // Invalidate if there's a non-zero value other than '1'
                        break;
                    }
                }

                if (count == 1)
                {
                    BV.Add(i);
                }
                else
                {
                    NBV.Add(i);
                }
            }

            // Initialize cBV, cNBV, B, and N arrays based on the number of variables
            double[] cBV = new double[BV.Count];
            double[] cNBV = new double[NBV.Count];

            double[,] B = new double[modelInput.Constraints.Count, BV.Count];
            double[,] N = new double[modelInput.Constraints.Count, NBV.Count];

            // Populate cBV, cNBV, B, and N arrays
            for (int i = 0; i < BV.Count; i++)
            {
                int varIndex = BV[i];
                cBV[i] = modelInput.ObjectiveCoefficients[varIndex];

                for (int j = 0; j < modelInput.Constraints.Count; j++)
                {
                    B[j, i] = modelInput.Constraints[j].Coefficients[varIndex];
                }
            }

            for (int i = 0; i < NBV.Count; i++)
            {
                int varIndex = NBV[i];
                cNBV[i] = modelInput.ObjectiveCoefficients[varIndex];

                for (int j = 0; j < modelInput.Constraints.Count; j++)
                {
                    N[j, i] = modelInput.Constraints[j].Coefficients[varIndex];
                }
            }

            // Output the results
            Console.WriteLine("Basic Variables (xBV): " + string.Join(", ", BV.Select(i => $"x{i + 1}")));
            Console.WriteLine("Non-Basic Variables (xNBV): " + string.Join(", ", NBV.Select(i => $"x{i + 1}")));
            Console.WriteLine("Basic Variable Coefficients (cBV): " + string.Join(", ", cBV));
            Console.WriteLine("Non-Basic Variable Coefficients (cNBV): " + string.Join(", ", cNBV));

            Console.WriteLine("B Matrix:");
            for (int i = 0; i < modelInput.Constraints.Count; i++)
            {
                Console.WriteLine("[" + string.Join(", ", Enumerable.Range(0, BV.Count).Select(j => B[i, j])) + "]");
            }

            Console.WriteLine("N Matrix:");
            for (int i = 0; i < modelInput.Constraints.Count; i++)
            {
                Console.WriteLine("[" + string.Join(", ", Enumerable.Range(0, NBV.Count).Select(j => N[i, j])) + "]");
            }

            // Calculate the Inverse of the B matrix
            double[,] BInverse = InverseMatrix(B);

            Console.WriteLine("B^-1 Matrix (Inverse of B):");
            for (int i = 0; i < BInverse.GetLength(0); i++)
            {
                Console.WriteLine("[" + string.Join(", ", Enumerable.Range(0, BInverse.GetLength(1)).Select(j => BInverse[i, j])) + "]");
            }

            // Calculate the cBV * B^-1 (Inverse of B) matrix
            double[,] cBVBInverse = MultiplyMatrices(BInverse, cBV);

            Console.WriteLine("cBV * B^-1:");
            Console.WriteLine("[" + string.Join(", ", cBVBInverse) + "]");
        }

        // Function to calculate the inverse of a matrix
        public double[,] InverseMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result = new double[n, n];
            double[,] temp = new double[n, n];

            // Initialize the result matrix as an identity matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        result[i, j] = 1;
                    else
                        result[i, j] = 0;
                }
            }

            // Copy the original matrix to temp
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    temp[i, j] = matrix[i, j];
                }
            }

            // Perform Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                double diagElement = temp[i, i];
                for (int j = 0; j < n; j++)
                {
                    temp[i, j] /= diagElement;
                    result[i, j] /= diagElement;
                }
                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = temp[k, i];
                        for (int j = 0; j < n; j++)
                        {
                            temp[k, j] -= factor * temp[i, j];
                            result[k, j] -= factor * result[i, j];
                        }
                    }
                }
            }

            return result;
        }

        // Function to multiply two matrices
        public double[,] MultiplyMatrices(double[,] matrixA, double[] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int rowsB = matrixB.Length;

            if (colsA != rowsB)
                throw new InvalidOperationException("Matrix dimensions do not match for multiplication.");

            double[,] result = new double[rowsA, 1];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k];
                    }
                }
            }

            return result;
        }            


        // Non-Basic Variables
        /*public string ChangeNBV()
        {
            //Receive variable index from user
            //Receive new coefficient from user
        }

        public string DisplayNBVRange()
        {

        }*/

        // Basic Variables
        public static void ChangeBV()
        {
            //Receive variable index from user
            //Receive new coefficient from user
        }

        public static void DisplayBVRange()
        {

        }

        // RHS Values
        public void ChangeRHS(int constraintIndex, double newRHS)
        {
            Console.WriteLine("What constraint's RHS value would you like to change?");
            string rhsConstraint = Console.ReadLine();
            Console.WriteLine("Please enter the new value: ");
            int rhsValue = int.Parse(Console.ReadLine());
        }

        public void DisplayRHSRange(int constraintIndex)
        {

        }

        // Adding New Activity
        public void AddNewActivity(double newCoefficient)
        {
            modelInput.ObjectiveCoefficients.Add(newCoefficient);
            foreach (var constraint in modelInput.Constraints)
            {
                Console.Write($"Enter coefficient for the new activity in constraint with RHS {constraint.RHS}: ");
                double newConstraintCoefficient = double.Parse(Console.ReadLine());
                constraint.Coefficients.Add(newConstraintCoefficient);
            }
        }

        // Adding New Constraint
        public void AddNewConstraint(List<double> coefficients, string relation, double rhs)
        {
            CustomConstraint newConstraint = new CustomConstraint
            {
                Coefficients = coefficients,
                Relation = relation,
                RHS = rhs
            };
            modelInput.Constraints.Add(newConstraint);
        }
    }
}
