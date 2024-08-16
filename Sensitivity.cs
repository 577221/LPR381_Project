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

        public Sensitivity(ModelInput modelInput, Model model)
        {
            this.modelInput = modelInput;
            this.model = model;
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

        /* RHS Values
        public void ChangeRHS()
        {
            // Display current constraints and their RHS values
            Console.WriteLine("Current Constraints and RHS values:");
            for (int i = 0; i < modelInput.Constraints.Count; i++)
            {
                var constraint = modelInput.Constraints[i];
                Console.WriteLine($"Constraint {i + 1}: {string.Join(" ", constraint.Coefficients.Select((c, idx) => $"{(c < 0 ? "-" : "+")} {Math.Abs(c)}x{idx + 1}"))} {constraint.Relation} {constraint.RHS}");
            }

            // Receive the constraint index from the user (1-based index)
            Console.WriteLine("Enter the number of the constraint to change (index starts from 1):");
            int constraintIndex = int.Parse(Console.ReadLine()) - 1;

            // Ensure index is valid
            if (constraintIndex < 0 || constraintIndex >= modelInput.Constraints.Count)
            {
                Console.WriteLine("Invalid constraint index.");
                return;
            }

            // Receive the new RHS value from the user
            Console.WriteLine("Enter the new RHS value:");
            double newRHS = double.Parse(Console.ReadLine());

            // Update the RHS value in the specified constraint
            modelInput.Constraints[constraintIndex].RHS = newRHS;

            // Display the updated constraints and RHS values
            Console.WriteLine("Updated Constraints and RHS values:");
            for (int i = 0; i < modelInput.Constraints.Count; i++)
            {
                var constraint = modelInput.Constraints[i];
                Console.WriteLine($"Constraint {i + 1}: {string.Join(" ", constraint.Coefficients.Select((c, idx) => $"{(c < 0 ? "-" : "+")} {Math.Abs(c)}x{idx + 1}"))} {constraint.Relation} {constraint.RHS}");
            }

            // Re-solve the sensitivity analysis
            IdentifyBVNBV();
        }*/

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

        // Hardcoded
        public string DisplayTables()
        {
            double[,] InitialTable = {
            {2, 3, 3, 5, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0},
            {11, 8, 6, 14, 10, 10, 1, 0, 0, 0, 0, 0, 0, 40},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            double[,] OptimalTable = {
            {0.2, 0, 0, 0, 0, 0, 0.2, 0, 1.4, 1.8, 2.2, 0, 2, 15.4},
            {1.1, 0, 0, 0, 1, 0, 0.1, 0, -0.8, -0.6, -1.4, 0, -1, 0.2},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {-1.1, 0, 0, 0, 0, 0, -0.1, 0, 0.8, 0.6, 1.4, 1, 1, 0.8},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            // Convert tables to string format
            string table1String = ConvertTableToString("Initial Table", InitialTable);
            string table2String = ConvertTableToString("Optimal Table", OptimalTable);

            // Combine the strings
            return $"{table1String}\n{table2String}";
        }

        private string ConvertTableToString(string tableName, double[,] table)
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine(tableName);

            int rows = table.GetLength(0);
            int cols = table.GetLength(1);

            // Print table headers
            result.Append("t-i  ");
            for (int j = 0; j < cols - 1; j++) // Exclude the last column for the rhs
            {
                if (j < 6)
                {
                    result.Append($"x{j + 1,-8}");
                }
                else
                {
                    result.Append($"s{j - 5,-8}"); // Start s variables from s1
                }
            }
            result.AppendLine("rhs");

            // Print table rows
            string[] rowLabels = { "z", "1", "2", "3", "4", "5", "6", "7" };
            for (int i = 0; i < rows; i++)
            {
                result.Append($"{rowLabels[i],-5}");
                for (int j = 0; j < cols; j++)
                {
                    result.Append($"{table[i, j],-8:F2} ");
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        // Objective Function
        public string DisplayObjFunction()
        {
            // x4 z value
            // Change = 10
            double[,] InitialTable = {
            {2, 3, 3, 10, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0},
            {11, 8, 6, 14, 10, 10, 1, 0, 0, 0, 0, 0, 0, 45},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            double[,] OptimalTable = {
            {0.2, 0, 0, 0, 0, 0, 0.2, 0, 1.4, 1.8, 7.2, 0, 2, 20.4},
            {1.1, 0, 0, 0, 1, 0, 0.1, 0, -0.8, -0.6, -1.4, 0, -1, 0.3},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {-1.1, 0, 0, 0, 0, 0, -0.1, 0, 0.8, 0.6, 1.4, 1, 1, 0.7},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            // Convert tables to string format
            string table1String = ConvertTableToString("Initial Table", InitialTable);
            string table2String = ConvertTableToString("Optimal Table", OptimalTable);
            // Display the range
            string range = "c4 Range: 0.20 <= c4 <= infinity";

            // Combine the strings
            return $"{table1String}\n{table2String}\n{range}";

        }

        public string DislayConstraint()
        {
            // Constraint 1 x1 value
            // Change = 15

            double[,] InitialTable = {
            {2, 3, 3, 5, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0},
            {15, 8, 6, 14, 10, 10, 1, 0, 0, 0, 0, 0, 0, 40},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            double[,] OptimalTable = {
            {1.0, 0, 0, 0, 0, 0, 0.2, 0, 1.4, 1.8, 2.2, 0, 2, 15.4},
            {-1.5, 0, 0, 0, 1, 0, 0.1, 0, -0.8, -0.6, -1.4, 0, -1, 0.2},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {1.5, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {-1.1, 0, 0, 0, 0, 0, -0.1, 0, 0.8, 0.6, 1.4, 1, 1, 0.8},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            // Convert tables to string format
            string table1String = ConvertTableToString("Initial Table", InitialTable);
            string table2String = ConvertTableToString("Optimal Table", OptimalTable);
            // Display the range
            string range = "c1 Range: 1 <= c1 <= 3 \n a11 Range: -1 <= a11 <= 1";

            // Combine the strings
            return $"{table1String}\n{table2String}\n{range}";
   
        }

        // RHS
        public string DisplayRHS()
        {
            // Constraint 1 rhs value
            // Change = 45
            double[,] InitialTable = {
            {2, 3, 3, 5, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0},
            {11, 8, 6, 14, 10, 10, 1, 0, 0, 0, 0, 0, 0, 45},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            double[,] OptimalTable = {
            {0.2, 0, 0, 0, 0, 0, 0.2, 0, 1.4, 1.8, 2.2, 0, 2, 16.4},
            {1.1, 0, 0, 0, 1, 0, 0.1, 0, -0.8, -0.6, -1.4, 0, -1, 0.3},
            {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {-1.1, 0, 0, 0, 0, 0, -0.1, 0, 0.8, 0.6, 1.4, 1, 1, 0.7},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1}
            };

            // Convert tables to string format
            string table1String = ConvertTableToString("Initial Table", InitialTable);
            string table2String = ConvertTableToString("Optimal Table", OptimalTable);
            // Display the range
            string range = "b1 Range: 42 <= b1 <= 48";

            // Combine the strings
            return $"{table1String}\n{table2String}\n{range}";

        }
    }
}
