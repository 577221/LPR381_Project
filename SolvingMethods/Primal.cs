using System;
using System.Linq;

namespace LPR381_Project.SolvingMethods
{
    internal class Primal
    {
        private Model model;
        private float[,] tablua; // Tableau matrix
        private int iteration = 0; // Track the iteration number

        public Primal(Model model)
        {
            this.Model = model;
        }

        internal Model Model { get => model; set => model = value; }

        public void Solve()
        {
            Prepare(); // Prepare the tableau

            bool optimal = false;

            while (!optimal)
            {
                iteration++; // Increment iteration number

                // Display the current iteration
                Console.WriteLine($"Iteration {iteration}");
                Console.WriteLine(ToString());

                // 1. Find the entering variable (most negative value in the objective row)
                int entering = 0;
                for (int i = 1; i < tablua.GetLength(1); i++) // Start from 1 to skip the RHS column
                {
                    if (tablua[0, i] < tablua[0, entering])
                    {
                        entering = i;
                    }
                }

                // If all values are non-negative, the current solution is optimal
                if (tablua[0, entering] >= 0)
                {
                    optimal = true;
                    continue;
                }

                // 2. Find the exiting variable using the ratio test
                int exiting = -1;
                float minRatio = float.MaxValue;

                for (int i = 1; i < tablua.GetLength(0); i++) // Skip the objective row
                {
                    if (tablua[i, entering] > 0) // Avoid division by zero
                    {
                        float ratio = tablua[i, tablua.GetLength(1) - 1] / tablua[i, entering];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            exiting = i;
                        }
                    }
                }

                if (exiting == -1)
                {
                    throw new InvalidOperationException("Unbounded solution.");
                }

                // 3. Pivot around the exiting variable
                Pivot(exiting, entering);
            }
        }

        private void Prepare()
        {
            // Number of constraints and variables
            int numConstraints = model.ConstraintsCoefficients.GetLength(0);
            int numVariables = model.ConstraintsCoefficients.GetLength(1);

            // Create the tableau
            tablua = new float[numConstraints + 1, numVariables + numConstraints + 1]; // Including RHS and slack variables

            // Populate the tableau with constraints
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    tablua[i + 1, j] = model.ConstraintsCoefficients[i, j];
                }
                tablua[i + 1, numVariables + i] = 1; // Slack variables
                tablua[i + 1, tablua.GetLength(1) - 1] = model.RhsConstraints[i];
            }

            // Populate the objective function row
            for (int i = 0; i < numVariables; i++)
            {
                tablua[0, i] = -model.ObjFunction[i];
            }

            // Initialize the tableau for display
            Console.WriteLine("Initial Tableau:");
            Console.WriteLine(ToString());
        }

        private void Pivot(int exitingRow, int enteringColumn)
        {
            // Normalize the pivot row
            float pivotValue = tablua[exitingRow, enteringColumn];
            for (int j = 0; j < tablua.GetLength(1); j++)
            {
                tablua[exitingRow, j] /= pivotValue;
            }

            // Zero out the entering column in other rows
            for (int i = 0; i < tablua.GetLength(0); i++)
            {
                if (i != exitingRow)
                {
                    float factor = tablua[i, enteringColumn];
                    for (int j = 0; j < tablua.GetLength(1); j++)
                    {
                        tablua[i, j] -= factor * tablua[exitingRow, j];
                    }
                }
            }
        }

        public override string ToString()
        {
            int numConstraints = model.ConstraintsCoefficients.GetLength(0);
            int numVariables = model.ConstraintsCoefficients.GetLength(1);

            // Create headers
            string[] headers = new string[numVariables + numConstraints + 1]; // Including RHS
            for (int i = 0; i < numVariables; i++)
            {
                headers[i] = $"x{i + 1}";
            }
            for (int i = 0; i < numConstraints; i++)
            {
                headers[numVariables + i] = $"s{i + 1}"; // Slack variables
            }
            headers[headers.Length - 1] = "RHS"; // Last column for RHS

            // Create the header row with padding
            string headerRow = string.Join(" | ", headers.Select(h => h.PadLeft(10)));
            string separator = new string('-', headerRow.Length);

            // Convert tableau to string with headers and iteration
            string myString = $"{headerRow}\n{separator}\n";

            for (int i = 0; i < tablua.GetLength(0); i++)
            {
                for (int j = 0; j < tablua.GetLength(1); j++)
                {
                    myString += tablua[i, j].ToString("F2").PadLeft(10);
                }
                myString += "\n";
            }
            return myString;
        }
    }
}
