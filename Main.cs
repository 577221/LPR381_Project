using System;
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
            string filepath = "input.txt"; 
            fileHandler = new FileHandler(filepath);

            try
            {
                fileHandler.StoreFileData();
                rtxDisplay.Clear();
                rtxDisplay.AppendText(fileHandler.ToString());
            }
            catch (Exception ex)
            {
                rtxDisplay.AppendText($"Error: {ex.Message}");
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (fileHandler != null)
            {
                Knapsack knapsack = new Knapsack(
                    fileHandler.ProblemType,
                    fileHandler.ObjFunction,
                    fileHandler.ConstraintsCoefficients,
                    fileHandler.RhsConstraints,
                    fileHandler.SignRestrictions
                );

                string result = knapsack.Solve();
                rtxDisplay.AppendText(result);
            }
            else
            {
                rtxDisplay.AppendText("Please load a file first.\n");
            }
        }

        private void btnBranchAndBound_Click(object sender, EventArgs e)
        {

        }

        private void pnlSensitivity_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
