using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LPR381_Project
{
    internal class FileHandler
    {
        private string filepath;
        private Model model;

        public FileHandler(string filepath)
        {
            this.filepath = filepath;
        }

        public Model GetModel()
        {
            return model;
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
                throw new InvalidOperationException($"Error reading file: {ex.Message}");
            }
        }

        // Function that parses and stores data from the text file
        public void StoreFileData()
        {
            string fileContent = ReadFile();
            if (string.IsNullOrEmpty(fileContent))
            {
                throw new InvalidOperationException("File content is empty or not read properly.");
            }

            ParseObjFunction(fileContent);
            ParseConstraints(fileContent);
            ParseSignRestrictions(fileContent);

            // Create the Model object after parsing all the data
            model = new Model(problemType, objFunction, constraintsCoefficients, constraintSign, rhsConstraints, signRestrictions);
        }

        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private string[] constraintSign;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public void ParseObjFunction(string fileContent)
        {
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            if (lines.Length > 0)
            {
                var firstLineParts = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                problemType = firstLineParts[0];
                objFunction = firstLineParts.Skip(1).Select(int.Parse).ToArray();
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

            if (numConstraints == 0)
            {
                throw new InvalidOperationException("No constraints found in the file.");
            }

            constraintsCoefficients = new int[numConstraints, objFunction.Length];
            rhsConstraints = new int[numConstraints];
            constraintSign = new string[numConstraints];

            for (int i = 0; i < numConstraints; i++)
            {
                var constraintParts = constraintLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (constraintParts.Length != objFunction.Length + 2)
                {
                    throw new InvalidOperationException("Mismatch between number of constraints and objective function elements.");
                }

                for (int j = 0; j < objFunction.Length; j++)
                {
                    if (!int.TryParse(constraintParts[j], out constraintsCoefficients[i, j]))
                    {
                        throw new InvalidOperationException($"Invalid constraint coefficient at line {constraintStartLine + i + 1}, position {j + 1}.");
                    }
                }

                // Parse the sign before the RHS value
                constraintSign[i] = constraintParts[objFunction.Length];

                // Parse the RHS constraint value
                if (!int.TryParse(constraintParts[objFunction.Length + 1], out rhsConstraints[i]))
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
    }
}


