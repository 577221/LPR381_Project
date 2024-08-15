using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Google.OrTools.LinearSolver;
using System.Text.RegularExpressions;
using LPR381_Project.SolvingMethods;

namespace LPR381_Project
{

    public class CuttingPlane
    {
        private ModelInput input;
        private const int MaxIterations = 100;

        public void LoadModel(string filePath)
        {
            try
            {
                input = new ModelInput(filePath);
                Console.WriteLine("Model loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading model: " + ex.Message);
            }
        }

        public void ApplyCuttingPlane()
        {
            if (input == null)
            {
                Console.WriteLine("Please load a model first.");
                return;
            }

            SolverWrapper solverWrapper = new SolverWrapper();
            int iteration = 0;
            bool feasible = true;

            while (iteration < MaxIterations && feasible)
            {
                iteration++;
                Console.WriteLine($"\nIteration {iteration}: Solving the LP relaxation...");

                feasible = solverWrapper.Solve(input);
                DisplayCurrentSolution(solverWrapper, iteration, feasible);
                WriteSolutionToFile(solverWrapper, iteration, feasible);

                if (!feasible)
                {
                    Console.WriteLine("Model became infeasible. Exiting.");
                    break;
                }

                // Identify the first fractional decision variable
                int fractionalVarIndex = -1;
                double fractionalValue = 0.0;

                for (int i = 0; i < solverWrapper.DecisionVariableCount; i++)
                {
                    double value = solverWrapper.GetVariableValue(i);

                    if (Math.Abs(value - Math.Round(value)) > 1e-6) // Check for fractional part
                    {
                        fractionalVarIndex = i;
                        fractionalValue = value;
                        break;
                    }
                }

                if (fractionalVarIndex == -1)
                {
                    Console.WriteLine("Integer solution found. Stopping the algorithm.");
                    break;
                }

                Console.WriteLine("Generating Gomory cut...");
                CustomConstraint cut = GenerateGomoryCut(solverWrapper, fractionalVarIndex, fractionalValue);
                if (cut == null)
                {
                    Console.WriteLine("Failed to generate a useful Gomory cut. Stopping the algorithm.");
                    break;
                }

                // Add the cut to the model
                input.Constraints.Add(cut);
                Console.WriteLine("Gomory cut added.");
            }

            if (iteration == MaxIterations)
            {
                Console.WriteLine("Reached maximum iterations without finding an integer solution.");
            }

            Console.WriteLine("Final solution written to output.txt.");
        }

        private CustomConstraint GenerateGomoryCut(SolverWrapper solverWrapper, int fractionalVarIndex, double fractionalValue)
        {
            var cut = new CustomConstraint
            {
                Coefficients = new List<double>(),
                Relation = "<=",
                RHS = Math.Floor(fractionalValue)  // Floor the RHS to enforce the cut
            };

            for (int i = 0; i < solverWrapper.DecisionVariableCount; i++)
            {
                double coeff = solverWrapper.GetVariableValue(i) - Math.Floor(solverWrapper.GetVariableValue(i));
                cut.Coefficients.Add(coeff);
            }

            // Ensure the coefficient corresponding to the fractional variable is correctly reduced
            cut.Coefficients[fractionalVarIndex] -= fractionalValue;

            return cut;
        }

        private void DisplayCurrentSolution(SolverWrapper solverWrapper, int iteration, bool feasible)
        {
            Console.WriteLine($"Iteration {iteration}:");

            if (!feasible)
            {
                Console.WriteLine("Model became infeasible, but here is the last solution:");
            }

            Console.WriteLine($"Objective Value: {solverWrapper.GetObjectiveValue()}");

            for (int i = 0; i < solverWrapper.DecisionVariableCount; i++)
            {
                Console.WriteLine($"x{i + 1} = {solverWrapper.GetVariableValue(i)}");
            }

            for (int i = 0; i < solverWrapper.SlackVariableCount; i++)
            {
                Console.WriteLine($"s{i + 1} = {solverWrapper.GetVariableValue(solverWrapper.DecisionVariableCount + i)}");
            }

            Console.WriteLine();
        }

        private void WriteSolutionToFile(SolverWrapper solverWrapper, int iteration, bool feasible)
        {
            using (var writer = new StreamWriter("output.txt", true))
            {
                writer.WriteLine($"Iteration {iteration}:");

                if (!feasible)
                {
                    writer.WriteLine("Model became infeasible, but here is the last solution:");
                }

                writer.WriteLine($"Objective Value: {solverWrapper.GetObjectiveValue()}");

                for (int i = 0; i < solverWrapper.DecisionVariableCount; i++)
                {
                    writer.WriteLine($"x{i + 1} = {solverWrapper.GetVariableValue(i)}");
                }

                for (int i = 0; i < solverWrapper.SlackVariableCount; i++)
                {
                    writer.WriteLine($"s{i + 1} = {solverWrapper.GetVariableValue(solverWrapper.DecisionVariableCount + i)}");
                }

                writer.WriteLine();
            }
        }
    }
}
