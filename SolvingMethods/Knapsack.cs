using System;
using System.Collections.Generic;

namespace LPR381_Project
{
    internal class Knapsack
    {
        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public Knapsack(string problemType, int[] objFunction, int[,] constraintsCoefficients, int[] rhsConstraints, string[] signRestrictions)
        {
            this.problemType = problemType;
            this.objFunction = objFunction;
            this.constraintsCoefficients = constraintsCoefficients;
            this.rhsConstraints = rhsConstraints;
            this.signRestrictions = signRestrictions;
        }

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] Constraints { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        // Method to solve the knapsack problem
        public string Solve()
        {
            int n = objFunction.Length;
            int W = rhsConstraints[0]; // Knapsack capacity
            int[] weights = new int[n];
            int[] values = new int[n];

            for (int i = 0; i < n; i++)
            {
                weights[i] = constraintsCoefficients[0, i];
                values[i] = objFunction[i];
            }

            int[,] K = new int[n + 1, W + 1];
            string iterations = "";

            // Build table K[][] in bottom-up manner and capture iterations
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

                    // Capture the current iteration
                    iterations += CaptureCurrentIteration(K, i, w);
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

            // Capture the final best answer
            string bestAnswer = CaptureBestAnswer(selectedItems);

            return iterations + bestAnswer;
        }

        // Method to capture the current iteration of the ranking table
        private string CaptureCurrentIteration(int[,] K, int i, int w)
        {
            string iteration = $"Iteration [{i},{w}]:\n";
            for (int x = 0; x <= K.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= K.GetLength(1) - 1; y++)
                {
                    iteration += K[x, y] + "\t";
                }
                iteration += "\n";
            }
            iteration += "\n";
            return iteration;
        }

        // Method to capture the final best answer
        private string CaptureBestAnswer(List<int> selectedItems)
        {
            string bestAnswer = "Best Answer:\n";
            bestAnswer += "Selected items:\n";
            foreach (var item in selectedItems)
            {
                bestAnswer += $"Item {item + 1} with value {objFunction[item]} and weight {constraintsCoefficients[0, item]}\n";
            }
            return bestAnswer;
        }
    }
}
