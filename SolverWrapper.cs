using System;
using System.Collections.Generic;
using System.IO;
using Google.OrTools.LinearSolver;

public class SolverWrapper
{
    private Solver solver;
    private List<Variable> decisionVariables;
    private List<Variable> slackVariables;

    public SolverWrapper()
    {
        solver = Solver.CreateSolver("GLOP");
        decisionVariables = new List<Variable>();
        slackVariables = new List<Variable>();
    }

    public bool Solve(ModelInput input)
    {
        if (solver == null) throw new Exception("Could not create solver.");

        decisionVariables.Clear();
        slackVariables.Clear();

        // Add decision variables (binary)
        for (int i = 0; i < input.ObjectiveCoefficients.Count; i++)
        {
            var variable = solver.MakeIntVar(0.0, 1.0, $"x{i + 1}");
            decisionVariables.Add(variable);
        }

        // Set up the objective function
        var objective = solver.Objective();
        for (int i = 0; i < decisionVariables.Count; i++)
        {
            objective.SetCoefficient(decisionVariables[i], input.ObjectiveCoefficients[i]);
        }
        objective.SetMaximization();

        // Add constraints with slack/excess variables
        foreach (var constraint in input.Constraints)
        {
            var relation = constraint.Relation;
            double lowerBound = double.NegativeInfinity;
            double upperBound = double.PositiveInfinity;

            if (relation == "<=")
            {
                upperBound = constraint.RHS;
            }
            else if (relation == ">=")
            {
                lowerBound = constraint.RHS;
            }
            else
            {
                throw new Exception("Unsupported relation in constraints");
            }

            var ct = solver.MakeConstraint(lowerBound, upperBound);
            for (int i = 0; i < constraint.Coefficients.Count; i++)
            {
                ct.SetCoefficient(decisionVariables[i], constraint.Coefficients[i]);
            }

            if (relation == "<=")
            {
                // Add slack variable
                var slack = solver.MakeNumVar(0.0, double.PositiveInfinity, $"s{slackVariables.Count + 1}");
                slackVariables.Add(slack);
                ct.SetCoefficient(slack, 1.0);
            }
            else if (relation == ">=")
            {
                // Add excess variable
                var excess = solver.MakeNumVar(0.0, double.PositiveInfinity, $"e{slackVariables.Count + 1}");
                slackVariables.Add(excess);
                ct.SetCoefficient(excess, -1.0);
            }
        }

        var resultStatus = solver.Solve();
        if (resultStatus != Solver.ResultStatus.OPTIMAL && resultStatus != Solver.ResultStatus.FEASIBLE)
        {
            Console.WriteLine("Model is infeasible.");
            return false;  // Infeasible
        }

        return true;  // Feasible
    }

    public double GetObjectiveValue()
    {
        return solver.Objective().Value();
    }

    public double GetVariableValue(int index)
    {
        if (index < decisionVariables.Count)
        {
            return decisionVariables[index].SolutionValue();
        }
        else
        {
            // Handle slack/excess variables
            int slackIndex = index - decisionVariables.Count;
            if (slackIndex >= 0 && slackIndex < slackVariables.Count)
            {
                return slackVariables[slackIndex].SolutionValue();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range.");
            }
        }
    }

    public int DecisionVariableCount => decisionVariables.Count;
    public int SlackVariableCount => slackVariables.Count;
    public int TotalVariableCount => decisionVariables.Count + slackVariables.Count;
}