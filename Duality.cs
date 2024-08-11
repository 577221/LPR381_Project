using System;
using System.Linq;
using System.Text;

namespace LPR381_Project
{
    internal class Duality
    {
        private Model model;

        public Duality(Model model)
        {
            // Ensure the passed model is used
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public string PrimalForm()
        {
            if (model == null || model.ConstraintsCoefficients == null)
                throw new InvalidOperationException("Model or ConstraintsCoefficients are not initialized.");

            StringBuilder constraintsStr = new StringBuilder();
            for (int i = 0; i < model.ConstraintsCoefficients.GetLength(0); i++)
            {
                // Modify the constraints string to include variable indices (starting from 1)
                string constraintStr = string.Join(" + ", Enumerable.Range(0, model.ConstraintsCoefficients.GetLength(1))
                    .Select(j => $"{model.ConstraintsCoefficients[i, j]}x{j + 1}"));

                constraintsStr.Append($"{constraintStr} {model.OperatorsConstraints[i]} {model.RhsConstraints[i]}\n");
            }

            // Modify the objective function string to include variable indices (starting from 1)
            string objectiveFunctionStr = string.Join(" + ", model.ObjFunction
                .Select((coef, index) => $"{coef}x{index + 1}"));

            return $"Primal Form:\n" +
                   $"------------\n" +
                   $"{model.ProblemType} z = {objectiveFunctionStr}\n" +
                   $"s.t. {constraintsStr}" +
                   $"     {string.Join(" ", model.SignRestrictions)}\n";
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
                ProblemType = (model.ProblemType == "max") ? "min" : "max",
                ObjFunction = new int[model.ConstraintsCoefficients.GetLength(1)],
                RhsConstraints = new int[model.ObjFunction.Length]
            };

            // Copy RHS constraints to Objective Function
            for (int i = 0; i < model.RhsConstraints.Length; i++)
            {
                dualModel.ObjFunction[i] = model.RhsConstraints[i];
            }

            // Copy Objective Function to RHS constraints
            for (int i = 0; i < model.ObjFunction.Length; i++)
            {
                dualModel.RhsConstraints[i] = model.ObjFunction[i];
            }

            int numberOfConstraints = model.ConstraintsCoefficients.GetLength(0);
            int numberOfVariables = model.ConstraintsCoefficients.GetLength(1);

            dualModel.ConstraintsCoefficients = new int[numberOfVariables, numberOfConstraints];
            dualModel.OperatorsConstraints = new string[numberOfVariables];
            dualModel.SignRestrictions = new string[numberOfVariables];

            // Populate dual constraints by transposing original constraints
            for (int i = 0; i < numberOfVariables; i++)
            {
                for (int j = 0; j < numberOfConstraints; j++)
                {
                    dualModel.ConstraintsCoefficients[i, j] = model.ConstraintsCoefficients[j, i];
                }
            }

            // Convert primal constraint operators to dual sign restrictions
            for (int j = 0; j < numberOfConstraints; j++)
            {
                string constraintOperator = model.OperatorsConstraints[j];
                if (constraintOperator == ">=")
                {
                    dualModel.OperatorsConstraints[j] = "<=";
                }
                else if (constraintOperator == "<=")
                {
                    dualModel.OperatorsConstraints[j] = ">=";
                }
                else
                {
                    throw new InvalidOperationException("Unknown constraint operator.");
                }
            }

            dualModel.SignRestrictions = model.SignRestrictions;

            return dualModel;
        }

        public double SolveDuality()
        {
            // Solve with Primal
            return 0.00;
        }

    }
}


