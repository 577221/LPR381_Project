using LPR381_Project.SolvingMethods;
using System;
using System.Linq;
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
                // Modify the constraints string to include variable indices (starting from 1)
                string constraintStr = string.Join(" + ", Enumerable.Range(0, ConstraintsCoefficients.GetLength(1))
                    .Select(j => $"{ConstraintsCoefficients[i, j]}x{j + 1}"));

                constraintsStr.Append($"{constraintStr} {OperatorsConstraints[i]} {RhsConstraints[i]}\n");
            }

            // Modify the objective function string to include variable indices (starting from 1)
            string objectiveFunctionStr = string.Join(" + ", ObjFunction
                .Select((coef, index) => $"{coef}x{index + 1}"));

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

            // Build the constraints string with the proper formatting
            for (int i = 0; i < dualModel.ConstraintsCoefficients.GetLength(0); i++)
            {
                string constraint = string.Join(" + ", Enumerable.Range(0, dualModel.ConstraintsCoefficients.GetLength(1))
                    .Select(j => dualModel.ConstraintsCoefficients[i, j] == 0 ? "" : $"{dualModel.ConstraintsCoefficients[i, j]}y{j + 1}")
                    .Where(val => !string.IsNullOrEmpty(val))); // Remove empty zeros

                constraintsStr.Append($"{constraint} {dualModel.OperatorsConstraints[i]} {dualModel.RhsConstraints[i]}\n");
            }

            // Build the objective function string with the proper formatting
            string objectiveFunctionStr = string.Join(" + ", dualModel.ObjFunction
                .Select((coef, index) => coef == 0 ? "" : $"{coef}y{index + 1}")
                .Where(val => !string.IsNullOrEmpty(val))); // Remove empty zeros

            return $"Dual Form:\n" +
                   $"------------\n" +
                   $"{dualModel.ProblemType} w = {objectiveFunctionStr}\n" +
                   $"s.t. {constraintsStr}" +
                   $"     {string.Join(" ", dualModel.SignRestrictions)}\n";
        }

        public Model ApplyDuality()
        {
            // Create a new dual model
            Model dualModel = new Model
            {
                ProblemType = (ProblemType == "max") ? "min" : "max",
                ObjFunction = new int[ConstraintsCoefficients.GetLength(1)],
                RhsConstraints = new int[ObjFunction.Length]
            };

            // Copy RHS constraints to Objective Function
            for (int i = 0; i < RhsConstraints.Length; i++)
            {
                dualModel.ObjFunction[i] = RhsConstraints[i];
            }

            // Copy Objective Function to RHS constraints
            for (int i = 0; i < ObjFunction.Length; i++)
            {
                dualModel.RhsConstraints[i] = ObjFunction[i];
            }

            int numberOfConstraints = ConstraintsCoefficients.GetLength(0);
            int numberOfVariables = ConstraintsCoefficients.GetLength(1);

            dualModel.ConstraintsCoefficients = new int[numberOfVariables, numberOfConstraints];
            dualModel.OperatorsConstraints = new string[numberOfVariables];
            dualModel.SignRestrictions = new string[numberOfVariables];

            // Populate dual constraints by transposing original constraints
            for (int i = 0; i < numberOfVariables; i++)
            {
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    dualModel.ConstraintsCoefficients[i, j] = ConstraintsCoefficients[j, i];
                }
            }

            // Assign operators to the new dual constraints based on primal sign restrictions
            for (int i = 0; i < numberOfVariables; i++)
            {
                // If primal problem has a maximization objective, dual constraints will have "≥"
                // If primal problem has a minimization objective, dual constraints will have "≤"
                dualModel.OperatorsConstraints[i] = ProblemType == "max" ? ">=" : "<=";
            }

            dualModel.SignRestrictions = SignRestrictions;

            return dualModel;
        }

        /*public double[] SolvePrimalForm()
        {
            // Solve the primal form using the SimplexSolver
            return SimplexSolver.Solve(model);
        }

        public double[] SolveDualForm()
        {
            // Apply duality to get the dual model
            Model dualModel = ApplyDuality();

            // Solve the dual form using the SimplexSolver
            return SimplexSolver.Solve(dualModel);
        }

        public (double[] primalSolution, double[] dualSolution) SolveDuality()
        {
            // Solve the primal form
            double[] primalSolution = SolvePrimalForm();

            // Solve the dual form
            double[] dualSolution = SolveDualForm();

            return (primalSolution, dualSolution);
        }*/
    }
}
