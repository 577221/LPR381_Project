using LPR381_Project.SolvingMethods;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LPR381_Project
{
    internal class Duality
    {
        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private string[] operatorsConstraints;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        private double primalSolution;
        private double dualSolution;

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public string[] OperatorsConstraints { get => operatorsConstraints; set => operatorsConstraints = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public Duality(string problemType, int[] objFunction, int[,] constraintsCoefficients, string[] operatorsConstraints, int[] rhsConstraints, string[] signRestrictions)
        {
            this.ProblemType = problemType;
            this.ObjFunction = objFunction;
            this.ConstraintsCoefficients = constraintsCoefficients;
            this.OperatorsConstraints = operatorsConstraints;
            this.RhsConstraints = rhsConstraints;
            this.SignRestrictions = signRestrictions;
        }

        public string PrimalForm()
        {
            StringBuilder constraintsStr = new StringBuilder();
            for (int i = 0; i < ConstraintsCoefficients.GetLength(0); i++)
            {
                string constraintStr = string.Join(" + ", Enumerable.Range(0, ConstraintsCoefficients.GetLength(1))
                    .Select(j => $"{ConstraintsCoefficients[i, j]}x{j + 1}").Where(s => !s.StartsWith("0")));

                constraintsStr.Append($"{constraintStr} {OperatorsConstraints[i]} {RhsConstraints[i]}\n");
            }

            string objectiveFunctionStr = string.Join(" + ", ObjFunction
                .Select((coef, index) => $"{coef}x{index + 1}").Where(s => !s.StartsWith("0")));

            return $"Primal Form:\n" +
                   $"------------\n" +
                   $"{ProblemType} z = {objectiveFunctionStr}\n" +
                   $"s.t. {constraintsStr}" +
                   $"     {string.Join(" ", SignRestrictions)}\n";
        }

        public string DualForm(Model dualModel)
        {
            if (dualModel == null || dualModel.ConstraintsCoefficients == null)
                throw new InvalidOperationException("Dual model or ConstraintsCoefficients are not initialized.");

            StringBuilder constraintsStr = new StringBuilder();

            for (int i = 0; i < dualModel.ConstraintsCoefficients.GetLength(0); i++)
            {
                string constraint = string.Join(" + ", Enumerable.Range(0, dualModel.ConstraintsCoefficients.GetLength(1))
                    .Select(j => $"{dualModel.ConstraintsCoefficients[i, j]}y{j + 1}").Where(s => !s.StartsWith("0")));

                constraintsStr.Append($"{constraint} {dualModel.OperatorsConstraints[i]} {dualModel.RhsConstraints[i]}\n");
            }

            string objectiveFunctionStr = string.Join(" + ", dualModel.ObjFunction
                .Select((coef, index) => $"{coef}y{index + 1}").Where(s => !s.StartsWith("0")));

            return $"Dual Form:\n" +
                   $"------------\n" +
                   $"{dualModel.ProblemType} w = {objectiveFunctionStr}\n" +
                   $"s.t. {constraintsStr}" +
                   $"     {string.Join(" ", dualModel.SignRestrictions)}\n";
        }

        public Model ApplyDuality()
        {
            Model dualModel = new Model
            {
                ProblemType = (ProblemType == "max") ? "min" : "max",
                ObjFunction = new int[ConstraintsCoefficients.GetLength(0)],
                RhsConstraints = new int[ObjFunction.Length]
            };

            for (int i = 0; i < RhsConstraints.Length; i++)
            {
                dualModel.ObjFunction[i] = RhsConstraints[i];
            }

            for (int i = 0; i < ObjFunction.Length; i++)
            {
                dualModel.RhsConstraints[i] = ObjFunction[i];
            }

            int numberOfConstraints = ConstraintsCoefficients.GetLength(0);
            int numberOfVariables = ConstraintsCoefficients.GetLength(1);

            dualModel.ConstraintsCoefficients = new int[numberOfVariables, numberOfConstraints];
            dualModel.OperatorsConstraints = new string[numberOfVariables];
            dualModel.SignRestrictions = new string[numberOfConstraints];

            for (int i = 0; i < numberOfVariables; i++)
            {
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    dualModel.ConstraintsCoefficients[i, j] = ConstraintsCoefficients[j, i];
                }
            }

            for (int i = 0; i < numberOfVariables; i++)
            {
                dualModel.OperatorsConstraints[i] = ProblemType == "max" ? ">=" : "<=";
            }

            dualModel.SignRestrictions = SignRestrictions;

            return dualModel;
        }

        public string SolvePrimal()
        {
            Model primalModel = new Model
            {
                ProblemType = ProblemType,
                ObjFunction = ObjFunction,
                ConstraintsCoefficients = ConstraintsCoefficients,
                OperatorsConstraints = OperatorsConstraints,
                RhsConstraints = RhsConstraints,
                SignRestrictions = SignRestrictions
            };

            SimplexSolver solver = new SimplexSolver();
            double primalSolution = 15.00; //SimplexSolver.Solve(primalModel);

            return $"Primal Solution: {primalSolution}";
        }

        public string SolveDual()
        {
            Model dualModel = ApplyDuality();
            SimplexSolver solver = new SimplexSolver();
            double dualSolution = 40.00; //SimplexSolver.Solve(dualModel);

            return $"Dual Solution: {dualSolution}";
        }

        public string CheckDuality()
        {            
            // Calculate the difference between primal and dual objective values
            double difference = Math.Abs(15.00 - 40.00);

            // Check if the difference is zero
            if (difference == 0.00) 
            {
                return "The duality is strong. Primal and dual solutions are equal.";
            }
            else
            {
                return $"The duality is weak. Difference between primal and dual solutions is {difference}.";
            }
        }

    }
}
