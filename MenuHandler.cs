using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class MenuHandler
    {
        public FileHandler fileHandler;

        public MenuHandler(FileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
        }

        private enum Menu
        {
            PrimalSimplex = 1,
            RevisedSimplex,
            BranchAndBound,
            CuttingPlane,
            Knapsack
        }

        public void ShowMenu()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine("Please choose an algorithm to solve the IP/LP Model:");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("1. Primal Simplex");
                Console.WriteLine("2. Revised Primal Simplex");
                Console.WriteLine("3. Branch & Bound Simplex");
                Console.WriteLine("4. Cutting Plane Simplex");
                Console.WriteLine("5. Branch & Bound Knapsack");
                Console.WriteLine();
                Console.WriteLine("Please enter 1, 2, 3, 4 or 5:");

                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!validChoice || choice < 1 || choice > 5)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    continue;
                }

                ExecuteChoice((Menu)choice);
                Console.WriteLine();
                Console.WriteLine("Press 'Q' to quit or any other key to continue...");
                var key = Console.ReadKey(true);
                continueLoop = key.KeyChar != 'Q' && key.KeyChar != 'q';
                Console.Clear();
            }
        }

        private void ExecuteChoice(Menu choice)
        {
            switch (choice)
            {
                case Menu.PrimalSimplex:
                    Console.WriteLine("Primal Simplex:");
                    Console.WriteLine("---------------");

                    break;

                case Menu.RevisedSimplex:
                    Console.WriteLine("Revised Primal Simplex:");
                    Console.WriteLine("-----------------------");

                    break;

                case Menu.BranchAndBound:
                    Console.WriteLine("Branch & Bound Simplex:");
                    Console.WriteLine("-----------------------");

                    break;

                case Menu.CuttingPlane:
                    Console.WriteLine("Cutting Plane Simplex:");
                    Console.WriteLine("----------------------");
                    /*try
                    {
                        Console.Write("Enter file path: ");
                        var filePath = Console.ReadLine();
                        var input = new ModelInput(filePath);
                        Console.WriteLine("Model loaded successfully.");

                        var solver = new SolverWrapper();
                        solver.Solve(input);
                        Console.WriteLine("Model solved. Results written to output.txt.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }*/
                    break;


                case Menu.Knapsack:
                    Console.WriteLine("Branch & Bound Knapsack:");
                    Console.WriteLine("------------------------");
                    try
                    {
                        Knapsack knapsack = new Knapsack(
                            fileHandler.ProblemType,
                            fileHandler.ObjFunction,
                            fileHandler.ConstraintsCoefficients,
                            fileHandler.RhsConstraints,
                            fileHandler.SignRestrictions);

                        // Execute the Ranking Table and Evaluation
                        string rankingTable = knapsack.RankingTable();
                        string rankingEvaluation = knapsack.RankingEvaluation();
                        string iterations = knapsack.Iterations();

                        // Display the results
                        Console.WriteLine(rankingTable);
                        Console.WriteLine();
                        Console.WriteLine(rankingEvaluation);
                        Console.WriteLine();
                        Console.WriteLine(iterations);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
            }
        }
    }
}
   
