using System;
using System.Threading;

namespace LPR381_Project
{
    internal static class Program
    {

        static void Main(string[] args)
        {
            string filepath = "input.txt";
            Model model = new Model();
            FileHandler fileHandler = new FileHandler(filepath, model);

            try
            {
                fileHandler.StoreFileData();
                Console.Clear();
                Console.WriteLine(model.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Clear();
            Console.WriteLine("Reading input.txt...");
            Thread.Sleep(3000);
            Console.WriteLine("IP/LP Model Received!\n");
            Thread.Sleep(3000);
            Console.WriteLine(model.ToString());
            Console.WriteLine();

            MenuHandler menuHandler = new MenuHandler(model);
            menuHandler.ShowMenu();
        }
    }
}