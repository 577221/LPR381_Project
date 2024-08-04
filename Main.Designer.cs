namespace LPR381_Project
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtxDisplay = new System.Windows.Forms.RichTextBox();
            this.btnKnapsack = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnBranchAndBound = new System.Windows.Forms.Button();
            this.btnCuttingPlane = new System.Windows.Forms.Button();
            this.btnPrimal = new System.Windows.Forms.Button();
            this.btnRevisedPrimal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxDisplay
            // 
            this.rtxDisplay.Location = new System.Drawing.Point(32, 28);
            this.rtxDisplay.Name = "rtxDisplay";
            this.rtxDisplay.Size = new System.Drawing.Size(561, 386);
            this.rtxDisplay.TabIndex = 0;
            this.rtxDisplay.Text = "";
            // 
            // btnKnapsack
            // 
            this.btnKnapsack.Location = new System.Drawing.Point(32, 445);
            this.btnKnapsack.Name = "btnKnapsack";
            this.btnKnapsack.Size = new System.Drawing.Size(165, 53);
            this.btnKnapsack.TabIndex = 2;
            this.btnKnapsack.Text = "Knapsack";
            this.btnKnapsack.UseVisualStyleBackColor = true;
            this.btnKnapsack.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(623, 103);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(165, 53);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(623, 28);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(165, 53);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click_1);
            // 
            // btnBranchAndBound
            // 
            this.btnBranchAndBound.Location = new System.Drawing.Point(235, 445);
            this.btnBranchAndBound.Name = "btnBranchAndBound";
            this.btnBranchAndBound.Size = new System.Drawing.Size(165, 53);
            this.btnBranchAndBound.TabIndex = 5;
            this.btnBranchAndBound.Text = "Branch and Bound";
            this.btnBranchAndBound.UseVisualStyleBackColor = true;
            this.btnBranchAndBound.Click += new System.EventHandler(this.btnBranchAndBound_Click);
            // 
            // btnCuttingPlane
            // 
            this.btnCuttingPlane.Location = new System.Drawing.Point(439, 445);
            this.btnCuttingPlane.Name = "btnCuttingPlane";
            this.btnCuttingPlane.Size = new System.Drawing.Size(165, 53);
            this.btnCuttingPlane.TabIndex = 6;
            this.btnCuttingPlane.Text = "Cutting Plane";
            this.btnCuttingPlane.UseVisualStyleBackColor = true;
            // 
            // btnPrimal
            // 
            this.btnPrimal.Location = new System.Drawing.Point(643, 445);
            this.btnPrimal.Name = "btnPrimal";
            this.btnPrimal.Size = new System.Drawing.Size(165, 53);
            this.btnPrimal.TabIndex = 7;
            this.btnPrimal.Text = "Primal Simplex";
            this.btnPrimal.UseVisualStyleBackColor = true;
            // 
            // btnRevisedPrimal
            // 
            this.btnRevisedPrimal.Location = new System.Drawing.Point(855, 445);
            this.btnRevisedPrimal.Name = "btnRevisedPrimal";
            this.btnRevisedPrimal.Size = new System.Drawing.Size(165, 53);
            this.btnRevisedPrimal.TabIndex = 8;
            this.btnRevisedPrimal.Text = "Revised Primal Simplex";
            this.btnRevisedPrimal.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 525);
            this.Controls.Add(this.btnRevisedPrimal);
            this.Controls.Add(this.btnPrimal);
            this.Controls.Add(this.btnCuttingPlane);
            this.Controls.Add(this.btnBranchAndBound);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnKnapsack);
            this.Controls.Add(this.rtxDisplay);
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxDisplay;
        private System.Windows.Forms.Button btnKnapsack;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnBranchAndBound;
        private System.Windows.Forms.Button btnCuttingPlane;
        private System.Windows.Forms.Button btnPrimal;
        private System.Windows.Forms.Button btnRevisedPrimal;
    }
}

