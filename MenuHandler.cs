using LPR381_Project.SolvingMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class MenuHandler
    {
        public Model model;

        public MenuHandler(Model model)
        {
            this.model = model;
        }

        private enum Menu
        {
            PrimalSimplex = 1,
            RevisedSimplex,
            BranchAndBound,
            CuttingPlane,
            Knapsack,
            Other,
            Exit
        }

        private enum SubMenu
        {
            Sensitivity = 1,
            Duality
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
                Console.WriteLine("6. Other");
                Console.WriteLine("7. Exit");
                Console.WriteLine();
                Console.WriteLine("Please enter 1 - 7:");

                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!validChoice || choice < 1 || choice > 6)
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
                    Console.WriteLine();
                    Console.WriteLine("Primal Simplex:");
                    Console.WriteLine("---------------");

                    try
                    {
                        Primal primal = new Primal(model);
                        primal.Solve();
                        Console.WriteLine(primal.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                case Menu.RevisedSimplex:
                    Console.WriteLine();
                    Console.WriteLine("Revised Primal Simplex:");
                    Console.WriteLine("-----------------------");

                    break;

                case Menu.BranchAndBound:
                    Console.WriteLine();
                    Console.WriteLine("Branch & Bound Simplex:");
                    Console.WriteLine("-----------------------");

                    break;

                case Menu.CuttingPlane:
                    Console.WriteLine();
                    Console.WriteLine("Cutting Plane Simplex:");
                    Console.WriteLine("----------------------");
                    try
                    {
                        CuttingPlane cuttingPlane = new CuttingPlane();
                        cuttingPlane.ApplyCuttingPlane();  // Apply the Gomory Cutting Plane algorithm
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;


                case Menu.Knapsack:
                    Console.WriteLine();
                    Console.WriteLine("Branch & Bound Knapsack:");
                    Console.WriteLine("------------------------");
                    try
                    {
                        Knapsack knapsack = new Knapsack(
                            model.ProblemType,
                            model.ObjFunction,
                            model.ConstraintsCoefficients,
                            model.RhsConstraints,
                            model.SignRestrictions);

                        // Execute the Ranking Table and Evaluation
                        string rankingTable = knapsack.RankingTable();
                        string rankingEvaluation = knapsack.RankingEvaluation();
                        string generateSubProblems = knapsack.GenerateSubProblems();
                        string bestSolution = knapsack.FindBestSolution();

                        // Display the results
                        Console.WriteLine(rankingTable);
                        Console.WriteLine();
                        Console.WriteLine(rankingEvaluation);
                        Console.WriteLine();
                        Console.WriteLine(generateSubProblems);
                        Console.WriteLine();
                        Console.WriteLine(bestSolution);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                case Menu.Other:
                    ShowOtherMenu();
                    break;

                case Menu.Exit:
                    break;
            }
        }

        private void ShowOtherMenu()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine();
                Console.WriteLine("Please choose what you would like to perform/apply on the IP/LP Model:");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("1. Sensitivity Analysis");
                Console.WriteLine("2. Duality");
                Console.WriteLine();
                Console.WriteLine("Please enter 1 or 2:");

                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!validChoice || choice < 1 || choice > 2)
                {
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    continue;
                }

                ExecuteOtherMenuChoice((SubMenu)choice);
                Console.WriteLine();
                Console.WriteLine("Press 'Q' to return to the main menu or any other key to continue...");
                var key = Console.ReadKey(true);
                continueLoop = key.KeyChar != 'Q' && key.KeyChar != 'q';
                Console.Clear();
            }
        }

        private void ExecuteOtherMenuChoice(SubMenu choice)
        {
            switch (choice)
            {
                case SubMenu.Sensitivity:
                    Console.WriteLine();
                    Console.WriteLine("Sensitivity Analysis");
                    Console.WriteLine("--------------------");

                    break;

                case SubMenu.Duality:
                    Console.WriteLine();
                    Console.WriteLine("Duality:");
                    Console.WriteLine("--------");

                    try
                    {
                        Model model = new Model();  // You need to adapt this to your model's structure
                        Duality duality = new Duality(model);
                        Console.WriteLine(duality.PrimalForm());
                        Model dualModel = duality.ApplyDuality();
                        Console.WriteLine(duality.DualForm(dualModel));
                        Console.WriteLine(duality.SolveDuality());
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