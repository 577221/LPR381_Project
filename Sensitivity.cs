using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Sensitivity
    {
        private Model model;

        public Sensitivity(Model model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        // Non-Basic Variables
        public void ChangeNBV(int variableIndex, double newCoefficient)
        {
            // Change the objective coefficient of a non-basic variable
            double originalCoefficient = model.ObjFunction[variableIndex];
            model.ObjFunction[variableIndex] = (int)newCoefficient;

            Console.WriteLine($"Non-Basic Variable x{variableIndex + 1} Coefficient changed from {originalCoefficient} to {newCoefficient}");
        }

        public void DisplayNBVRange(int variableIndex)
        {
            // Display the allowable increase and decrease of the objective coefficient of a non-basic variable
            double originalCoefficient = model.ObjFunction[variableIndex];

            Console.WriteLine($"Non-Basic Variable x{variableIndex + 1}:");
            Console.WriteLine($"Original Coefficient: {originalCoefficient}");

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
            double originalRHS = model.RhsConstraints[constraintIndex];
            model.RhsConstraints[constraintIndex] = (int)newRHS;

            Console.WriteLine($"RHS of Constraint {constraintIndex + 1} changed from {originalRHS} to {newRHS}");
        }

        public void DisplayRHSRange(int constraintIndex)
        {
            // Display the allowable increase and decrease of a constraint's RHS
            double originalRHS = model.RhsConstraints[constraintIndex];

            Console.WriteLine($"RHS of Constraint {constraintIndex + 1}:");
            Console.WriteLine($"Original RHS: {originalRHS}");

            double allowableIncrease = double.PositiveInfinity;
            double allowableDecrease = originalRHS;

            Console.WriteLine($"Allowable Increase: {allowableIncrease}");
            Console.WriteLine($"Allowable Decrease: {allowableDecrease}");
        }

        // Adding New Activity
        public void AddNewActivity(double newCoefficient)
        {
            model.ObjFunction.Add(newCoefficient);
            foreach (var constraint in model.Constraints)
            {
                Console.Write($"Enter coefficient for the new activity in constraint with RHS {constraint.RHS}: ");
                double newConstraintCoefficient = double.Parse(Console.ReadLine());
                constraint.Coefficients.Add(newConstraintCoefficient);
            }
        }

        // Adding New Constraint
        public void AddNewConstraint(List<double> coefficients, string relation, double rhs)
        {
            // Add a new constraint to the model
            CustomConstraint newConstraint = new CustomConstraint
            {
                Coefficients = coefficients,
                Relation = relation,
                RHS = rhs
            };
            model.Constraints.Add(newConstraint);
        }
    }
}
