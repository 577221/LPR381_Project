using System;
using System.Collections.Generic;

namespace LPR381_Project
{
    internal class Knapsack
    {
        private string problemType;
        private int[] objFunction;
        private int[] constraints;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[] Constraints { get => constraints; set => constraints = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public Knapsack(string problemType, int[] objFunction, int[] constraints, int[] rhsConstraints, string[] signRestrictions)
        {
            this.ProblemType = problemType;
            this.ObjFunction = objFunction;
            this.Constraints = constraints;
            this.RhsConstraints = rhsConstraints;
            this.SignRestrictions = signRestrictions;
        }

        // Method to solve the knapsack problem
        public void Solve()
        {
            int n = objFunction.Length;
            int W = RhsConstraints[0]; // Knapsack capacity
            int[] weights = new int[n];
            int[] values = new int[n];

            for (int i = 0; i < n; i++)
            {
                weights[i] = constraints[0][i];
                values[i] = objFunction[i];
            }

            int[,] K = new int[n + 1, W + 1];

            // Build table K[][] in bottom-up manner and display iterations
            for (int i = 0; i <= n; i++)
            {
                for (int w = 0; w <= W; w++)
                {
                    if (i == 0 || w == 0)
                        K[i, w] = 0;
                    else if (weights[i - 1] <= w)
                        K[i, w] = Math.Max(values[i - 1] + K[i - 1, w - weights[i - 1]], K[i - 1, w]);
                    else
                        K[i, w] = K[i - 1, w];

                    // Display the current iteration
                    DisplayCurrentIteration(K, n, W);
                }
            }

            // Find the selected items
            List<int> selectedItems = new List<int>();
            int res = K[n, W];
            int remainingWeight = W;

            for (int i = n; i > 0 && res > 0; i--)
            {
                if (res != K[i - 1, remainingWeight])
                {
                    selectedItems.Add(i - 1);
                    res -= values[i - 1];
                    remainingWeight -= weights[i - 1];
                }
            }

            // Display the final best answer
            DisplayBestAnswer(selectedItems);
        }

        // Method to display the current iteration of the ranking table
        private void DisplayCurrentIteration(int[,] K, int n, int W)
        {
            Console.WriteLine("Current Iteration:");
            for (int i = 0; i <= n; i++)
            {
                for (int w = 0; w <= W; w++)
                {
                    Console.Write(K[i, w] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Method to display the final best answer
        private void DisplayBestAnswer(List<int> selectedItems)
        {
            Console.WriteLine("Best Answer:");
            Console.WriteLine("Selected items:");
            foreach (var item in selectedItems)
            {
                Console.WriteLine("Item " + item + " with value " + objFunction[item] + " and weight " + constraints[0][item]);
            }
        }
    }
}
