using System;
using System.Collections.Generic;
using System.IO;

public class ModelInput
{
    public string OptimizationType { get; set; } = string.Empty;
    public List<double> ObjectiveCoefficients { get; set; } = new List<double>();
    public List<CustomConstraint> Constraints { get; set; } = new List<CustomConstraint>();
    public List<string> SignRestrictions { get; set; } = new List<string>();

    public ModelInput(string filePath)
    {
        ParseInputFile(filePath);
    }

    private void ParseInputFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var firstLine = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        OptimizationType = firstLine[0];
        for (int i = 1; i < firstLine.Length; i += 2)
        {
            string sign = firstLine[i];
            string coefficientStr = firstLine[i + 1];
            double coefficient = double.Parse(coefficientStr);
            if (sign == "-")
            {
                coefficient = -coefficient;
            }
            ObjectiveCoefficients.Add(coefficient);
        }

        for (int i = 1; i < lines.Length - 1; i++)
        {
            var constraintParts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var constraint = new CustomConstraint
            {
                Coefficients = new List<double>(),
                Relation = constraintParts[constraintParts.Length - 2],
                RHS = double.Parse(constraintParts[constraintParts.Length - 1])
            };

            for (int j = 0; j < constraintParts.Length - 2; j += 2)
            {
                string sign = constraintParts[j];
                string coefficientStr = constraintParts[j + 1];
                double coefficient = double.Parse(coefficientStr);
                if (sign == "-")
                {
                    coefficient = -coefficient;
                }
                constraint.Coefficients.Add(coefficient);
            }

            Constraints.Add(constraint);
        }

        SignRestrictions.AddRange(lines[lines.Length - 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
    }
}

public class CustomConstraint
{
    public List<double> Coefficients { get; set; } = new List<double>();
    public string Relation { get; set; } = string.Empty;
    public double RHS { get; set; }
}