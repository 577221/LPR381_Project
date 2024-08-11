using System;
using System.Text;

namespace LPR381_Project.SolvingMethods
{
    internal class Primal
    {
        private Model model;
        private float[,] tableau;

        public Primal(Model model)
        {
            this.model = model;
        }

        public void Solve()
        {
            Prepare();

            bool optimal = false;

            while (!optimal)
            {
                int chosen = 0;
                // Select the largest negative value in the objective row (indicating the entering variable)
                for (int i = 0; i < tableau.GetLength(1); i++)
                {
                    if (tableau[0, i] < tableau[0, chosen])
                    {
                        chosen = i;
                    }
                }

                // If there's a negative value in the objective row, continue
                if (tableau[0, chosen] < 0)
                {
                    int pos = 1;
                    // Perform the ratio test to determine the exiting variable
                    for (int i = 1; i < tableau.GetLength(0); i++)
                    {
                        if (tableau[i, chosen] > 0 &&
                            (tableau[i, tableau.GetLength(1) - 1] / tableau[i, chosen]) <
                            (tableau[pos, tableau.GetLength(1) - 1] / tableau[pos, chosen]))
                        {
                            pos = i;
                        }
                    }

                    // TODO: Pivot and update tableau here
                    // Update tableau based on pivot element at (pos, chosen)
                }
                else
                {
                    optimal = true;
                }
            }
        }

        // Prepares the tableau for the Simplex method
        private void Prepare()
        {
            int numRows = model.ConstraintsCoefficients.GetLength(0) + 1;
            int numCols = model.ConstraintsCoefficients.GetLength(1) + model.ConstraintsCoefficients.GetLength(0) + 1;

            tableau = new float[numRows, numCols];

            // Fill objective function row
            for (int i = 0; i < model.ObjFunction.Length; i++)
            {
                tableau[0, i] = model.ProblemType == "max" ? -model.ObjFunction[i] : model.ObjFunction[i];
            }

            // Fill constraint rows
            for (int i = 0; i < model.ConstraintsCoefficients.GetLength(0); i++)
            {
                for (int j = 0; j < model.ConstraintsCoefficients.GetLength(1); j++)
                {
                    tableau[i + 1, j] = model.ConstraintsCoefficients[i, j];
                }

                tableau[i + 1, numCols - 1] = model.RhsConstraints[i];
                tableau[i + 1, model.ObjFunction.Length + i] = 1; // Add slack variables
            }
        }

        // Returns a string representation of the tableau
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < tableau.GetLength(0); i++)
            {
                for (int j = 0; j < tableau.GetLength(1); j++)
                {
                    sb.Append(tableau[i, j] + "\t");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
