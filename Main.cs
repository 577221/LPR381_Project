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
        // Created global variable to use the FileHandler and Knapsack Class
        private FileHandler fileHandler;
        private Knapsack knapsack;

        public Main()
        {
            InitializeComponent();
            // Initializing the FileHandler and Knapsack Class as an object
            fileHandler = new FileHandler("input.txt");
            knapsack = new Knapsack();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtxDisplay.Clear();
        }

        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            string fileContent = fileHandler.ReadFile();
            rtxDisplay.Text = fileContent;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            rtxDisplay.Text = fileHandler.ToString();
        }
    }
}
