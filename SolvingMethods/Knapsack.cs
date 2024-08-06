using System;
using System.Collections.Generic;
using System.Linq;

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
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public string RankingTable()
        {
            // List to store fractions, values, positions, and ranks
            List<(string Fraction, double Value, int Rank, int Row, int Column)> rankings = new List<(string Fraction, double Value, int Rank, int Row, int Column)>();
            int numConstraints = constraintsCoefficients.GetLength(0);
            int numVariables = constraintsCoefficients.GetLength(1);

            // Calculate fractions and values
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    double fraction = (double)objFunction[j] / constraintsCoefficients[i, j];
                    rankings.Add(($"{constraintsCoefficients[i, j]}/{objFunction[j]}", fraction, 0, i, j));
                }
            }

            // Sort by value (fraction) and assign ranks
            rankings = rankings.OrderByDescending(x => x.Value).ToList();
            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i] = (rankings[i].Fraction, rankings[i].Value, i + 1, rankings[i].Row, rankings[i].Column);
            }

            // Create string representation of the ranking table
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Ranking Table:");
            sb.AppendLine("Variable\tFraction\tValue\tRank");
            foreach (var item in rankings)
            {
                sb.AppendLine($"x{item.Column+1}\t{item.Fraction}\t{item.Value:F2}\t{item.Rank}");
            }

            return sb.ToString();
        }

    }
}
