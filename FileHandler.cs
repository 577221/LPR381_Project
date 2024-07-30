using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class FileHandler
    {
        private string filepath;

        public FileHandler(string filepath)
        {
            this.filepath = filepath;
        }

        // Function that reads the content of the Textfile
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
    }
}
