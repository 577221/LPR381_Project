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
        string outputFilePath = "output.txt";

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
            Duality,
            ReturnToMenu
        }

        private enum SensitivityMenu
        {
            ObjectiveCoefficientChange = 1,
            ConstraintCoefficientChange,
            RHSChange,
            NewActivity,
            NewConstraint,
            ReturnToSubMenu
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
                Console.WriteLine("Please enter a number between 1 and 7:");

                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!validChoice || choice < 1 || choice > 7)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
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
                        //Console.WriteLine(primal.ToString());
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
                    RevisedPrimal revised = new RevisedPrimal(model);
                    revised.Solve();
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
                        //CuttingPlane cuttingPlane = new CuttingPlane();
                        //cuttingPlane.ApplyCuttingPlane();  // Apply the Gomory Cutting Plane algorithm
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
                        try
                        {
                            knapsack.SaveOutputToFile(outputFilePath);
                            Console.WriteLine("Content of Knapsack Solution successfully stored in output.txt!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
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
                    Environment.Exit(0);
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
                Console.WriteLine("3. Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine("Please enter a number between 1 and 3:");

                int choice;
                bool validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!validChoice || choice < 1 || choice > 3)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                    continue;
                }

                if (choice == (int)SubMenu.ReturnToMenu)
                {
                    return; // Return to the previous menu
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
                    ShowSensitivityMenu();

                    break;

                case SubMenu.Duality:
                    Console.WriteLine();
                    Console.WriteLine("Duality:");
                    Console.WriteLine("--------");

                    try
                    {
                        Duality duality = new Duality(
                            model.ProblemType,
                            model.ObjFunction,
                            model.ConstraintsCoefficients,
                            model.OperatorsConstraints,
                            model.RhsConstraints,
                            model.SignRestrictions);

                        // Print the primal form
                        Console.WriteLine(duality.PrimalForm());

                        // Apply duality to get the dual model
                        Model dualModel = duality.ApplyDuality();

                        // Print the dual form
                        Console.WriteLine(duality.DualForm(dualModel));

                        Console.WriteLine(duality.SolvePrimal());
                        Console.WriteLine(duality.SolveDual());
                        Console.WriteLine(duality.CheckDuality());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
            }
        }

        private void ShowSensitivityMenu()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine();
                Console.WriteLine("Sensitivity Analysis Options:");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("1. Change Objective Coefficient");
                Console.WriteLine("2. Change Constraint Coefficient");
                Console.WriteLine("3. Change RHS Value");
                Console.WriteLine("4. Add a new activity");
                Console.WriteLine("5. Add a new constraint");
                Console.WriteLine("6. Return to Previous Menu");
                Console.WriteLine();
                Console.WriteLine("Please enter a number between 1 and 6:");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 6)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                    continue;
                }

                if (choice == (int)SensitivityMenu.ReturnToSubMenu)
                {
                    return; // Return to the previous menu
                }

                ExecuteSensitivityMenuChoice((SensitivityMenu)choice);
                Console.WriteLine();
                Console.WriteLine("Press 'Q' to return to the sensitivity menu or any other key to continue...");
                var key = Console.ReadKey(true);
                continueLoop = key.KeyChar != 'Q' && key.KeyChar != 'q';
                Console.Clear();
            }
        }

        private void ExecuteSensitivityMenuChoice(SensitivityMenu choice)
        {
            switch (choice)
            {
                case SensitivityMenu.ObjectiveCoefficientChange:
                    Console.WriteLine("What variable's objective function value would you like to change? (1 for x1, 2 for x2 etc.)");
                    string objVariable = Console.ReadLine();
                    Console.WriteLine("Please enter the new value: ");
                    int objValue = int.Parse(Console.ReadLine());

                    break;

                case SensitivityMenu.ConstraintCoefficientChange:
                    Console.WriteLine("What variable's constraint value would you like to change? (1 for x1, 2 for x2 etc.)");
                    string constraintVariable = Console.ReadLine();
                    Console.WriteLine("Please enter the new value: ");
                    int constraintValue = int.Parse(Console.ReadLine());

                    break;

                case SensitivityMenu.RHSChange:
                    Console.WriteLine("What rhs value would you like to change? (1 for C1, 2 for C2 etc.)");
                    string rhsConstraint = Console.ReadLine();
                    Console.WriteLine("Please enter the new value: ");
                    int rhsValue = int.Parse(Console.ReadLine());

                    break;

                case SensitivityMenu.NewActivity:
                    break;

                case SensitivityMenu.NewConstraint:
                    Console.WriteLine("Please enter the new constraint (form: x1 +/-x2 +/-x3 <=/>= rhs): ");
                    break;
            }
        }
    }
}