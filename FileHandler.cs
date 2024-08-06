using System;
using System.IO;
using System.Linq;

namespace LPR381_Project
{
    internal class FileHandler
    {
        private string filepath;

        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public FileHandler(string filepath)
        {
            this.filepath = filepath;
        }

        // Function that reads the content of the text file
        public string ReadFile()
        {
            try
            {
                return File.ReadAllText(filepath);
            }
            catch (Exception ex)
            {
                return $"Error reading file: {ex.Message}";
            }
        }

        // Function that parses and stores data from the text file
        public void StoreFileData()
        {
            string fileContent = ReadFile();
            if (fileContent == null)
            {
                throw new InvalidOperationException("Error reading file.");
            }
            ParseObjFunction(fileContent);
            ParseConstraints(fileContent);
            ParseSignRestrictions(fileContent);
        }

        public void ParseObjFunction(string fileContent)
        {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            if (lines.Length > 0)
            {
                var firstLineParts = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ProblemType = firstLineParts[0];
                ObjFunction = firstLineParts.Skip(1).Select(int.Parse).ToArray();
            }
            else
            {
                throw new InvalidOperationException("The file does not contain any data.");
            }
        }

        public void ParseConstraints(string fileContent)
        {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int constraintStartLine = 1; // constraints start after the first non-empty line
            int constraintEndLine = lines.Length - 1; // sign restrictions are on the last non-empty line

            var constraintLines = lines.Skip(constraintStartLine).Take(constraintEndLine - constraintStartLine).ToArray();
            int numConstraints = constraintLines.Length;

            // Check if there are any constraints to parse
            if (numConstraints == 0)
            {
                throw new InvalidOperationException("No constraints found in the file.");
            }

            constraintsCoefficients = new int[numConstraints, ObjFunction.Length];
            rhsConstraints = new int[numConstraints];

            for (int i = 0; i < numConstraints; i++)
            {
                var constraintParts = constraintLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (constraintParts.Length != ObjFunction.Length + 1)
                {
                    throw new InvalidOperationException("Mismatch between number of constraints and objective function elements.");
                }

                for (int j = 0; j < ObjFunction.Length; j++)
                {
                    if (!int.TryParse(constraintParts[j], out constraintsCoefficients[i, j]))
                    {
                        throw new InvalidOperationException($"Invalid constraint coefficient at line {constraintStartLine + i + 1}, position {j + 1}.");
                    }
                }

                if (!int.TryParse(constraintParts[constraintParts.Length - 1].Trim(new char[] { '<', '=', '>' }), out rhsConstraints[i]))
                {
                    throw new InvalidOperationException($"Invalid RHS constraint at line {constraintStartLine + i + 1}, position {constraintParts.Length}.");
                }
            }
        }

        public void ParseSignRestrictions(string fileContent)
        {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var signRestrictionsLine = lines[lines.Length - 1];
            signRestrictions = signRestrictionsLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            string constraintsStr = "";
            for (int i = 0; i < constraintsCoefficients.GetLength(0); i++)
            {
                for (int j = 0; j < constraintsCoefficients.GetLength(1); j++)
                {
                    constraintsStr += constraintsCoefficients[i, j] + " ";
                }
                constraintsStr += "<= " + rhsConstraints[i] + "\n";
            }

            return $"IP Model Values:\n" +
                   $"- - - - - - - - - - - - - - -\n" +
                   $"Problem Type: {ProblemType}\n" +
                   $"Objective Function: {string.Join(" ", ObjFunction)}\n" +
                   $"Constraints:\n{constraintsStr}\n" +
                   $"Sign Restrictions: {string.Join(" ", SignRestrictions)}\n";
        }
    }
}