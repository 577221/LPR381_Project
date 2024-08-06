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
        

    }
}
