using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project.SolvingMethods
{
    internal class BranchAndBound
    {
    static void Main(string[] args)
    {
        string filePath = "input.txt"; // Path to your input file
        string outputFilePath = "output.txt"; // Path to the output file

        // Read the problem from a file
        LinearProgram lp = ReadProblemFromFile(filePath);

        // Open the output file for writing
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            // Start Branch and Bound
            BranchAndBound(lp, writer);

            // Ensure all data is written to the file
            writer.Flush();
        }

        Console.ReadLine();
    }

    static void BranchAndBound(LinearProgram lp, StreamWriter writer)
    {
        Queue<Node> nodes = new Queue<Node>();
        Node rootNode = new Node(lp.LowerBounds, lp.UpperBounds);
        nodes.Enqueue(rootNode);

        double bestObjective = double.NegativeInfinity;
        double[] bestSolution = null;
        int iteration = 0;

        while (nodes.Count > 0)
        {
            Node currentNode = nodes.Dequeue();
            iteration++;

            // Solve the relaxed linear programming problem at the current node
            double[] solution = SimplexSolver.Solve(lp.Objective, lp.Constraints, lp.Bounds, currentNode.LowerBounds, currentNode.UpperBounds);
            double objectiveValue = CalculateObjective(lp.Objective, solution);

            string iterationOutput = $"Iteration {iteration}: Objective = {objectiveValue}, Solution = [{string.Join(", ", solution)}]";
            Console.WriteLine(iterationOutput);
            writer.WriteLine(iterationOutput);

            // If the solution is better than the current best and feasible (integer), update the best solution
            if (objectiveValue > bestObjective && IsFeasibleIntegerSolution(solution))
            {
                bestObjective = objectiveValue;
                bestSolution = solution;
                string bestSolutionOutput = $"New best solution found: Objective = {bestObjective}, Solution = [{string.Join(", ", bestSolution)}]";
                Console.WriteLine(bestSolutionOutput);
                writer.WriteLine(bestSolutionOutput);
            }

            // Branch if the solution is not integer
            int branchIndex = FindBranchingIndex(solution);
            if (branchIndex != -1)
            {
                // Branching on the fractional variable
                double[] lowerBounds1 = (double[])currentNode.LowerBounds.Clone();
                double[] upperBounds1 = (double[])currentNode.UpperBounds.Clone();
                upperBounds1[branchIndex] = Math.Floor(solution[branchIndex]);

                double[] lowerBounds2 = (double[])currentNode.LowerBounds.Clone();
                double[] upperBounds2 = (double[])currentNode.UpperBounds.Clone();
                lowerBounds2[branchIndex] = Math.Ceiling(solution[branchIndex]);

                nodes.Enqueue(new Node(lowerBounds1, upperBounds1));
                nodes.Enqueue(new Node(lowerBounds2, upperBounds2));
            }
        }

        Console.WriteLine("Branch and Bound complete.");
        writer.WriteLine("Branch and Bound complete.");
        
        if (bestSolution != null)
        {
            string finalOutput = $"Best solution: Objective = {bestObjective}, Solution = [{string.Join(", ", bestSolution)}]";
            Console.WriteLine(finalOutput);
            writer.WriteLine(finalOutput);
        }
        else
        {
            string finalOutput = "No feasible integer solution found.";
            Console.WriteLine(finalOutput);
            writer.WriteLine(finalOutput);
        }
    }

    static double[] SolveLP(double[] objective, double[,] constraints, double[] bounds, double[] lowerBounds, double[] upperBounds)
    {
        // Use Simplex solver to check feasibility
        return SimplexSolver.Solve(objective, constraints, bounds, lowerBounds, upperBounds);
    }

    static double CalculateObjective(double[] objective, double[] solution)
    {
        return objective.Select((coeff, i) => coeff * solution[i]).Sum();
    }

    static bool IsFeasibleIntegerSolution(double[] solution)
    {
        return solution.All(x => Math.Abs(x - Math.Round(x)) < 1e-6);
    }

    static int FindBranchingIndex(double[] solution)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            if (Math.Abs(solution[i] - Math.Round(solution[i])) > 1e-6)
            {
                return i;
            }
        }
        return -1;
    }

    static LinearProgram ReadProblemFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        double[] objective = null;
        List<double[]> constraints = new List<double[]>();
        List<double> bounds = new List<double>();
        bool maximize = true;
        List<double> lowerBounds = new List<double>();
        List<double> upperBounds = new List<double>();

        int lineIndex = 0;

        // Read the objective function
        var objectiveParts = lines[lineIndex++].Split(' ');
        maximize = objectiveParts[0] == "max";
        objective = objectiveParts.Skip(1).Select(double.Parse).ToArray();

        // Read the constraints
        while (lineIndex < lines.Length && !lines[lineIndex].Trim().StartsWith("bin") && !lines[lineIndex].Trim().StartsWith("int") && !lines[lineIndex].Trim().StartsWith("urs"))
        {
            var constraintParts = lines[lineIndex++].Split(' ');
            double[] constraint = constraintParts.Take(constraintParts.Length - 2).Select(double.Parse).ToArray();
            constraints.Add(constraint);
            bounds.Add(double.Parse(constraintParts.Last()));
        }

        // Read the variable types and bounds
        var variableTypes = lines[lineIndex++].Split(' ');
        for (int i = 0; i < variableTypes.Length; i++)
        {
            switch (variableTypes[i])
            {
                case "bin":
                    lowerBounds.Add(0);
                    upperBounds.Add(1);
                    break;
                case "int":
                    lowerBounds.Add(0);
                    upperBounds.Add(double.PositiveInfinity);
                    break;
                case "urs":
                    lowerBounds.Add(0);
                    upperBounds.Add(double.PositiveInfinity);
                    break;
                case "+":
                    lowerBounds.Add(0);
                    upperBounds.Add(double.PositiveInfinity);
                    break;
                case "-":
                    lowerBounds.Add(double.NegativeInfinity);
                    upperBounds.Add(double.PositiveInfinity);
                    break;
                default:
                    throw new Exception("Invalid variable type");
            }
        }

        return new LinearProgram
        {
            Objective = objective.ToArray(),
            Constraints = To2DArray(constraints),
            Bounds = bounds.ToArray(),
            LowerBounds = lowerBounds.ToArray(),
            UpperBounds = upperBounds.ToArray(),
            Maximize = maximize
        };
    }

    static double[,] To2DArray(List<double[]> list)
    {
        int rows = list.Count;
        int cols = list[0].Length;
        double[,] array = new double[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = list[i][j];
            }
        }
        return array;
    }
}

class LinearProgram
{
    public double[] Objective { get; set; }
    public double[,] Constraints { get; set; }
    public double[] Bounds { get; set; }
    public double[] LowerBounds { get; set; }
    public double[] UpperBounds { get; set; }
    public bool Maximize { get; set; }
}

class Node
{
    public double[] LowerBounds { get; }
    public double[] UpperBounds { get; }

    public Node(double[] lowerBounds, double[] upperBounds)
    {
        LowerBounds = lowerBounds;
        UpperBounds = upperBounds;
    }
}

class SimplexSolver
{
    public static double[] Solve(double[] objective, double[,] constraints, double[] bounds, double[] lowerBounds, double[] upperBounds)
    {
        int numVariables = objective.Length;
        int numConstraints = bounds.Length;

        // Step 1: Initialize the tableau
        double[,] tableau = InitializeTableau(objective, constraints, bounds, numVariables, numConstraints);

        // Step 2: Perform the Simplex algorithm
        while (true)
        {
            // Step 2a: Identify the entering variable (most negative coefficient in the objective row)
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

            // Step 2b: Identify the leaving variable (minimum ratio test)
            int pivotrow = -1;
            double minRatio = double.PositiveInfinity;
            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, numVariables + numConstraints] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotrow = i;
                    }
                }
            }

            // If no valid leaving variable is found, the problem is unbounded
            if (pivotrow == -1)
            {
                throw new Exception("The problem is unbounded.");
            }

            // Step 2c: Pivot
            Pivot(tableau, pivotColumn, pivotrow, numVariables, numConstraints);
        }

        // Step 3: Extract the solution
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

    private static void Pivot(double[,] tableau, int pivotColumn, int pivotrow, int numVariables, int numConstraints)
    {
        double pivotElement = tableau[pivotrow, pivotColumn];

        // Normalize the pivot row
        for (int j = 0; j <= numVariables + numConstraints; j++)
        {
            tableau[pivotrow, j] /= pivotElement;
        }

        // Eliminate the entering column from other rows
        for (int i = 0; i <= numConstraints; i++)
        {
            if (i != pivotrow)
            {
                double factor = tableau[i, pivotColumn];
                for (int j = 0; j <= numVariables + numConstraints; j++)
                {
                    tableau[i, j] -= factor * tableau[pivotrow, j];
                }
            }
        }
    }
}
    }
