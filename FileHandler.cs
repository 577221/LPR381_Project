using System;
using System.IO;
using System.Linq;

namespace LPR381_Project
{
    internal class FileHandler
    {
        private string filepath;
        private Model model;

        public FileHandler(string filepath, Model model)
        {
            this.filepath = filepath;
            this.model = model;
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
            string fileContent = ReadFile() ?? throw new InvalidOperationException("Error reading file.");
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
                model.ProblemType = firstLineParts[0];
                model.ObjFunction = firstLineParts.Skip(1).Select(int.Parse).ToArray();
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

            model.ConstraintsCoefficients = new int[numConstraints, model.ObjFunction.Length];
            model.OperatorsConstraints = new string[numConstraints];
            model.RhsConstraints = new int[numConstraints];

            for (int i = 0; i < numConstraints; i++)
            {
                var constraintParts = constraintLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (constraintParts.Length != model.ObjFunction.Length + 1)
                {
                    throw new InvalidOperationException("Mismatch between number of constraints and objective function elements.");
                }

                for (int j = 0; j < model.ObjFunction.Length; j++)
                {
                    if (!int.TryParse(constraintParts[j], out model.ConstraintsCoefficients[i, j]))
                    {
                        throw new InvalidOperationException($"Invalid constraint coefficient at line {constraintStartLine + i + 1}, position {j + 1}.");
                    }
                }

                // Extract the operator part
                string operatorPart = new string(constraintParts[constraintParts.Length - 1].Where(c => c == '<' || c == '=' || c == '>').ToArray());
                model.OperatorsConstraints[i] = operatorPart;

                // Trim the operator characters from the RHS value and try to parse it
                if (!int.TryParse(constraintParts[constraintParts.Length - 1].Trim(new char[] { '<', '=', '>' }), out model.RhsConstraints[i]))
                {
                    throw new InvalidOperationException($"Invalid RHS constraint at line {constraintStartLine + i + 1}, position {constraintParts.Length}.");
                }
            }
        }

        public void ParseSignRestrictions(string fileContent)
        {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var signRestrictionsLine = lines[lines.Length - 1];
            model.SignRestrictions = signRestrictionsLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}