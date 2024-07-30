using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPR381_Project
{
    public partial class Main : Form
    {
        // Created global variable to use the FileHandler Class
        private FileHandler fileHandler;

        public Main()
        {
            InitializeComponent();
            // Initializing the FileHandler Class as an object
            fileHandler = new FileHandler("input.txt");
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Read and display the file content using FileHandler
            string fileContent = fileHandler.ReadFile();
            richTextBox1.Text = fileContent;
        }
    }
}
