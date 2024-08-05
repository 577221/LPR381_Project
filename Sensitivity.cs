using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Sensitivity
    {
        public void AddNewActivity(double newCoefficient)
    {
        ObjectiveCoefficients.Add(newCoefficient);
        foreach (var constraint in Constraints)
        {
            Console.Write($"Enter coefficient for the new activity in constraint with RHS : ");
            double newConstraintCoefficient = double.Parse(Console.ReadLine());
            constraint.Coefficients.Add(newConstraintCoefficient);
        }
    }
    }
}
