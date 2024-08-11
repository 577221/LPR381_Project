using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Model
    {
        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private string[] constraintSign;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public Model(string problemType, int[] objFunction, int[,] constraintsCoefficients, string[] constraintSign, int[] rhsConstraints, string[] signRestrictions)
        {
            this.ProblemType = problemType;
            this.ObjFunction = objFunction;
            this.ConstraintsCoefficients = constraintsCoefficients;
            this.ConstraintSign = constraintSign;
            this.RhsConstraints = rhsConstraints;
            this.SignRestrictions = signRestrictions;
        }

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public string[] ConstraintSign { get => constraintSign; set => constraintSign = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public override string ToString()
        {
            // Initialize an empty string for constraints representation
            string constraintsStr = "";

            if (ConstraintsCoefficients != null && RhsConstraints != null && ConstraintSign != null)
            {
                for (int i = 0; i < ConstraintsCoefficients.GetLength(0); i++)
                {
                    for (int j = 0; j < ConstraintsCoefficients.GetLength(1); j++)
                    {
                        constraintsStr += ConstraintsCoefficients[i, j] + " ";
                    }
                    constraintsStr += $"{ConstraintSign[i]} {RhsConstraints[i]}\n";
                }
            }
            else
            {
                constraintsStr = "Constraints data is not available.";
            }

            return $"IP Model Values:\n" +
                   $"----------------\n" +
                   $"Problem Type: {ProblemType ?? "N/A"}\n\n" +
                   $"Objective Function: {(ObjFunction != null ? string.Join(" ", ObjFunction) : "N/A")}\n\n" +
                   $"Constraints:\n{constraintsStr}\n" +
                   $"Sign Restrictions: {(SignRestrictions != null ? string.Join(" ", SignRestrictions) : "N/A")}\n";
        }
    }
}
