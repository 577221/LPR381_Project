using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project.SolvingMethods
{
    using System;

class SimplexSolver
{
    public static double[] Solve(double[] objective, double[,] constraints, double[] bounds, double[] lowerBounds, double[] upperBounds)
    {
        int numVariables = objective.Length;
        int numConstraints = bounds.Length;

        // Initializing of tableau
        double[,] tableau = InitializeTableau(objective, constraints, bounds, numVariables, numConstraints);

        // Perform Simplex algorithm
        while (true)
        {
            // Identify the pivot column (most negative coefficient in the objective row)
            int pivotColumn = -1;
            double mostNegative = 0;
            for (int j = 0; j < numVariables; j++)
            {
                if (tableau[numConstraints, j] < mostNegative)
                {
                    mostNegative = tableau[numConstraints, j];
                    pivotColumn = j;
                }
            }

            // If there's no negative coefficient, the optimal solution has been found
            if (pivotColumn == -1)
            {
                break;
            }

            // Identify the pivot row (minimum ratio test)
            int pivotRow = -1;
            double minRatio = double.PositiveInfinity;
            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, numVariables + numConstraints] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivot = i;
                    }
                }
            }

            // If no valid pivot row is found, the problem is unbounded
            if (pivot == -1)
            {
                throw new Exception("The problem is unbounded.");
            }

            // Pivot operation
            Pivot(tableau, pivotColumn, pivot, numVariables, numConstraints);
        }

        // Extract the solution
        double[] solution = new double[numVariables];
        for (int i = 0; i < numConstraints; i++)
        {
            if (tableau[i, numVariables + i] == 1)
            {
                solution[i] = tableau[i, numVariables + numConstraints];
            }
        }

        return solution;
    }

    private static double[,] InitializeTableau(double[] objective, double[,] constraints, double[] bounds, int numVariables, int numConstraints)
    {
        double[,] tableau = new double[numConstraints + 1, numVariables + numConstraints + 1];

        // Fill the tableau with constraints coefficients and bounds
        for (int i = 0; i < numConstraints; i++)
        {
            for (int j = 0; j < numVariables; j++)
            {
                tableau[i, j] = constraints[i, j];
            }
            tableau[i, numVariables + i] = 1; // Add slack variables
            tableau[i, numVariables + numConstraints] = bounds[i];
        }

        // Fill the objective function row
        for (int j = 0; j < numVariables; j++)
        {
            tableau[numConstraints, j] = -objective[j];
        }

        return tableau;
    }

    private static void Pivot(double[,] tableau, int pivotColumn, int pivot, int numVariables, int numConstraints)
    {
        double pivotElement = tableau[pivot, pivotColumn];

        // Normalize the pivot row
        for (int j = 0; j <= numVariables + numConstraints; j++)
        {
            tableau[pivot, j] /= pivotElement;
        }

        // Eliminate the pivot column from other rows
        for (int i = 0; i <= numConstraints; i++)
        {
            if (i != pivot)
            {
                double factor = tableau[i, pivotColumn];
                for (int j = 0; j <= numVariables + numConstraints; j++)
                {
                    tableau[i, j] -= factor * tableau[pivot, j];
                }
            }
        }
    }
}

    internal class BranchAndBound
    {
    }
}
