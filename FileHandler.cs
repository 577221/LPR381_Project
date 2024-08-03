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
        private int[] constraints;
        private int[] rhsConstraints;
        private string[] signRestrictions;

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[] Constraints { get => constraints; set => constraints = value; }
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
        public string StoreFileData()
        {
            string fileContent = ReadFile();

            if (fileContent == null)
            {
                return $"Error reading file";
            }
            else
            {
               
            }

            return fileContent;
        }

        public void ParseObjFunction(string fileContent)
        {
            // Split the content into lines, preserving empty lines
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

            // Check if there is at least one line for the objective function
            if (lines.Length > 0)
            {
                var firstLineParts = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Extract problem type and objective function coefficients
                ProblemType = firstLineParts[0];
                ObjFunction = firstLineParts.Skip(1).Select(int.Parse).ToArray();
            }
            else
            {
                throw new InvalidOperationException("The file does not contain any data.");
            }
        }

        public int[] ParseConstraints()
        {
            return Constraints;
        }

        public string[] ParseSignRestrictions()
        {
            return SignRestrictions;
        }

        public override string ToString()
        {
            return $"IP Model Values:\n" +
                   $"- - - - - - - - - -\n" +
                   $"Problem Type: {ProblemType}\n" +
                   $"Objective Function: {string.Join(" ", ObjFunction)}\n";
            // You can add more details like Constraints and Sign Restrictions if needed
        }

    }
}
