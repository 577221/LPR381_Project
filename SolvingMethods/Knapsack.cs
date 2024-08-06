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
            List<(double Fraction, double Value, int Rank)> rankings = new List<(double Fraction, double Value, int Rank)>();
            int numConstraints = constraintsCoefficients.GetLength(0);
            int numVariables = constraintsCoefficients.GetLength(1);

            // Calculate fractions and values
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    double fraction = (double)constraintsCoefficients[i, j] / objFunction[j];
                    double value = fraction * rhsConstraints[i];
                    rankings.Add((fraction, value, 0));
                }
            }

            // Sort by value and assign ranks
            rankings.Sort((x, y) => y.Value.CompareTo(x.Value));
            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i] = (rankings[i].Fraction, rankings[i].Value, i + 1);
            }

            // Create string representation of the ranking table
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Ranking Table:");
            sb.AppendLine("Fraction\tValue\tRank");
            foreach (var item in rankings)
            {
                sb.AppendLine($"{item.Fraction}\t{item.Value}\t{item.Rank}");
            }

            return sb.ToString();
        }
    }
}
