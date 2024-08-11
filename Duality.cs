using System;
using System.Text;

namespace LPR381_Project
{
    internal class Duality
    {
        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public Duality(string problemType, int[] objFunction, int[,] constraintsCoefficients, int[] rhsConstraints, string[] signRestrictions)
        {
            this.ProblemType = problemType;
            this.ObjFunction = objFunction;
            this.ConstraintsCoefficients = constraintsCoefficients;
            this.RhsConstraints = rhsConstraints;
            this.SignRestrictions = signRestrictions;
        }

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public string PrimalTable()
        {
            StringBuilder sb = new StringBuilder();
            int numRows = ConstraintsCoefficients.GetLength(0);
            int numCols = ConstraintsCoefficients.GetLength(1);

            // Header for the table
            sb.AppendLine("Primal Table:");
            sb.Append("Variables\t");
            for (int i = 0; i < numCols; i++)
                sb.Append($"x{i + 1}\t");
            sb.Append("RHS\n");

            // Constraints
            for (int i = 0; i < numRows; i++)
            {
                sb.Append($"Constraint {i + 1}\t");
                for (int j = 0; j < numCols; j++)
                    sb.Append($"{ConstraintsCoefficients[i, j]}\t");

                // Add slack/surplus variables
                if (SignRestrictions[i] == "<=")
                    sb.Append($"s{i + 1} = {RhsConstraints[i]}\n");
                else if (SignRestrictions[i] == ">=")
                    sb.Append($"- s{i + 1} = {RhsConstraints[i]}\n");
                else
                    sb.Append($"= {RhsConstraints[i]}\n");
            }

            // Objective function
            sb.AppendLine(new string('-', 60));
            sb.Append("Objective Function\t");
            for (int i = 0; i < numCols; i++)
                sb.Append($"{ObjFunction[i]}\t");
            sb.AppendLine();

            return sb.ToString();
        }

        public static void ApplyDuality()
        {
            

        }
    }
}
