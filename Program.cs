using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPR381_Project
{
    internal static class Program
    {
        enum Menu
        {
            PrimalSimplex = 1,
            RevisedSimplex,
            BranchAndBound,
            CuttingPlane,
            Knapsack
        }

        static void Main(string[] args)
        {
            string filepath = "input.txt";
            FileHandler fileHandler = new FileHandler(filepath);

            try
            {
                fileHandler.StoreFileData();
                Console.Clear();
                Console.WriteLine(fileHandler.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            bool Continue = true;

            Console.Clear();
            Console.WriteLine("Reading input.txt...");
            Thread.Sleep(3000);
            Console.WriteLine("IP/LP Model Received!\n");
            Thread.Sleep(3000);
            Console.WriteLine(fileHandler.ToString());
            Console.WriteLine();

            while (Continue)
            {
                
                Console.WriteLine("Please choose a algorithm to solve the IP/LP Model:");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("1. Primal Simplex");
                Console.WriteLine("2. Revised Primal Simplex");
                Console.WriteLine("3. Branch & Bound Simplex");
                Console.WriteLine("4. Cutting Plane Simplex");
                Console.WriteLine("5. Branch & Bound Knapsack");
                Console.WriteLine();
                Console.WriteLine("Please enter 1, 2, 3, 4 or 5:");

                int choice = int.Parse(Console.ReadLine());

                switch ((Menu)choice)
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

                            string rankingTable = knapsack.RankingTable();
                            Console.WriteLine(rankingTable);
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
}
