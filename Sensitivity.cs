using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Sensitivity
    {
        private ModelInput modelInput;

        public Sensitivity(ModelInput modelInput)
        {
            this.modelInput = modelInput;
        }

        // Non-Basic Variables
        public void ChangeNBV(int variableIndex, double newCoefficient)
        {
            // Change the objective coefficient of a non-basic variable
            double originalCoefficient = modelInput.ObjectiveCoefficients[variableIndex];
            modelInput.ObjectiveCoefficients[variableIndex] = newCoefficient;

            Console.WriteLine($"Non-Basic Variable x{variableIndex + 1} Coefficient changed from {originalCoefficient} to {newCoefficient}");
        }


        public void DisplayNBVRange(int variableIndex)
        {
            // Display the allowable increase and decrease of the objective coefficient of a non-basic variable
            double originalCoefficient = modelInput.ObjectiveCoefficients[variableIndex];

            Console.WriteLine($"Non-Basic Variable x{variableIndex + 1}:");
            Console.WriteLine($"Original Coefficient: {originalCoefficient}");

            // For simplicity, these would be computed based on solver outputs or assumptions
            double allowableIncrease = double.PositiveInfinity;  
            double allowableDecrease = originalCoefficient;     

            Console.WriteLine($"Allowable Increase: {allowableIncrease}");
            Console.WriteLine($"Allowable Decrease: {allowableDecrease}");
        }


        // Basic Variables
        public static void ChangeBV()
        {

        }

        public static void DisplayBVRange()
        {

        }

        // RHS Values
        public void ChangeRHS(int constraintIndex, double newRHS)
        {
            // Change the RHS of a constraint
            double originalRHS = modelInput.Constraints[constraintIndex].RHS;
            modelInput.Constraints[constraintIndex].RHS = newRHS;

            Console.WriteLine($"RHS of Constraint {constraintIndex + 1} changed from {originalRHS} to {newRHS}");
        }

        public void DisplayRHSRange(int constraintIndex)
        {
            // Display the allowable increase and decrease of a constraint's RHS
            double originalRHS = modelInput.Constraints[constraintIndex].RHS;

            Console.WriteLine($"RHS of Constraint {constraintIndex + 1}:");
            Console.WriteLine($"Original RHS: {originalRHS}");

            // For simplicity, these would be computed based on solver outputs or assumptions
            double allowableIncrease = double.PositiveInfinity;  // Example, replace with actual computation
            double allowableDecrease = originalRHS;              // Example, replace with actual computation

            Console.WriteLine($"Allowable Increase: {allowableIncrease}");
            Console.WriteLine($"Allowable Decrease: {allowableDecrease}");
        }

        // Adding New Activity
        public void AddNewActivity(double newCoefficient)
        {
            modelInput.ObjectiveCoefficients.Add(newCoefficient);
            foreach (var constraint in modelInput.Constraints)
            {
                Console.Write($"Enter coefficient for the new activity in constraint with RHS {constraint.RHS}: ");
                double newConstraintCoefficient = double.Parse(Console.ReadLine());
                constraint.Coefficients.Add(newConstraintCoefficient);
            }
        }

        // Adding New Constraint
        public void AddNewConstraint(List<double> coefficients, string relation, double rhs)
        {
            CustomConstraint newConstraint = new CustomConstraint
            {
                Coefficients = coefficients,
                Relation = relation,
                RHS = rhs
            };
            modelInput.Constraints.Add(newConstraint);
        }
    }
}
