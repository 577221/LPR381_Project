using System;
using System.Text;

namespace LPR381_Project
{
    internal class Duality
    {
        private Model dualModel;

        public Duality(Model dualModel)
        {
            dualModel = new Model();
        }

        public string PrimalTable()
        {
            StringBuilder sb = new StringBuilder();
            int numRows = dualModel.ConstraintsCoefficients.GetLength(0);
            int numCols = dualModel.ConstraintsCoefficients.GetLength(1);

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
                    sb.Append($"{dualModel.ConstraintsCoefficients[i, j]}\t");

                // Add slack/surplus variables
                if (dualModel.OperatorsConstraints[i] == "<=")
                    sb.Append($"s{i + 1} = {dualModel.RhsConstraints[i]}\n");
                else if (dualModel.OperatorsConstraints[i] == ">=")
                    sb.Append($"- e{i + 1} = {dualModel.RhsConstraints[i]}\n");
                else
                    sb.Append($"= {dualModel.RhsConstraints[i]}\n");
            }

            // Objective function
            sb.AppendLine(new string('-', 60));
            sb.Append("Objective Function\t");
            for (int i = 0; i < numCols; i++)
                sb.Append($"{dualModel.ObjFunction[i]}\t");
            sb.AppendLine();

            return sb.ToString();
        }

        public static void ApplyDuality()
        {
            

        }
    }
}
