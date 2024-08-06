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

            Console.WriteLine("Reading input.txt...");
            Thread.Sleep(3000);
            Console.WriteLine("IP / Model Received!\n");
            Thread.Sleep(3000);
            Console.WriteLine(fileHandler.ToString());

            bool Continue = true;

            while (Continue)
            {
                int choice = int.Parse(Console.ReadLine());

                switch ((Menu)choice)
                {
                    case Menu.PrimalSimplex:

                        break;

                    case Menu.RevisedSimplex:

                        break;

                    case Menu.BranchAndBound:

                        break;

                    case Menu.CuttingPlane:

                        break;

                    case Menu.Knapsack:

                        break;
                }
            }

        }
    }
}
