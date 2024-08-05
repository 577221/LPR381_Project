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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCuttingPlane = new System.Windows.Forms.Button();
            this.btnPrimal = new System.Windows.Forms.Button();
            this.btnRevisedPrimal = new System.Windows.Forms.Button();
            this.pnlModel = new System.Windows.Forms.Panel();
            this.pnlSensitivity = new System.Windows.Forms.Panel();
            this.btnChanges = new System.Windows.Forms.Button();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.dgvSensitivity = new System.Windows.Forms.DataGridView();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnKnapsack = new System.Windows.Forms.Button();
            this.btnBranchAndBound = new System.Windows.Forms.Button();
            this.pnlSensitivityInfo = new System.Windows.Forms.Panel();
            this.pnlModel.SuspendLayout();
            this.pnlSensitivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSensitivity)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxDisplay
            // 
            this.rtxDisplay.Location = new System.Drawing.Point(31, 27);
            this.rtxDisplay.Name = "rtxDisplay";
            this.rtxDisplay.Size = new System.Drawing.Size(624, 366);
            this.rtxDisplay.TabIndex = 0;
            this.rtxDisplay.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(490, 418);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(165, 46);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear Textbox";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(31, 418);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(165, 46);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load Text File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click_1);
            // 
            // btnCuttingPlane
            // 
            this.btnCuttingPlane.Location = new System.Drawing.Point(270, 13);
            this.btnCuttingPlane.Name = "btnCuttingPlane";
            this.btnCuttingPlane.Size = new System.Drawing.Size(115, 53);
            this.btnCuttingPlane.TabIndex = 6;
            this.btnCuttingPlane.Text = "Cutting Plane";
            this.btnCuttingPlane.UseVisualStyleBackColor = true;
            // 
            // btnPrimal
            // 
            this.btnPrimal.Location = new System.Drawing.Point(391, 13);
            this.btnPrimal.Name = "btnPrimal";
            this.btnPrimal.Size = new System.Drawing.Size(115, 53);
            this.btnPrimal.TabIndex = 7;
            this.btnPrimal.Text = "Primal Simplex";
            this.btnPrimal.UseVisualStyleBackColor = true;
            // 
            // btnRevisedPrimal
            // 
            this.btnRevisedPrimal.Location = new System.Drawing.Point(512, 13);
            this.btnRevisedPrimal.Name = "btnRevisedPrimal";
            this.btnRevisedPrimal.Size = new System.Drawing.Size(160, 53);
            this.btnRevisedPrimal.TabIndex = 8;
            this.btnRevisedPrimal.Text = "Revised Primal Simplex";
            this.btnRevisedPrimal.UseVisualStyleBackColor = true;
            // 
            // pnlModel
            // 
            this.pnlModel.Controls.Add(this.rtxDisplay);
            this.pnlModel.Controls.Add(this.btnClear);
            this.pnlModel.Controls.Add(this.btnLoad);
            this.pnlModel.Location = new System.Drawing.Point(12, 12);
            this.pnlModel.Name = "pnlModel";
            this.pnlModel.Size = new System.Drawing.Size(685, 494);
            this.pnlModel.TabIndex = 9;
            this.pnlModel.Tag = "";
            // 
            // pnlSensitivity
            // 
            this.pnlSensitivity.Controls.Add(this.dgvSensitivity);
            this.pnlSensitivity.Controls.Add(this.btnChanges);
            this.pnlSensitivity.Controls.Add(this.btnLoadTables);
            this.pnlSensitivity.Location = new System.Drawing.Point(12, 12);
            this.pnlSensitivity.Name = "pnlSensitivity";
            this.pnlSensitivity.Size = new System.Drawing.Size(685, 494);
            this.pnlSensitivity.TabIndex = 10;
            this.pnlSensitivity.Tag = "";
            this.pnlSensitivity.Visible = false;
            this.pnlSensitivity.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSensitivity_Paint);
            // 
            // btnChanges
            // 
            this.btnChanges.Location = new System.Drawing.Point(488, 418);
            this.btnChanges.Name = "btnChanges";
            this.btnChanges.Size = new System.Drawing.Size(165, 46);
            this.btnChanges.TabIndex = 3;
            this.btnChanges.Text = "Apply Changes";
            this.btnChanges.UseVisualStyleBackColor = true;
            // 
            // btnLoadTables
            // 
            this.btnLoadTables.Location = new System.Drawing.Point(31, 418);
            this.btnLoadTables.Name = "btnLoadTables";
            this.btnLoadTables.Size = new System.Drawing.Size(165, 46);
            this.btnLoadTables.TabIndex = 4;
            this.btnLoadTables.Text = "Load Tables";
            this.btnLoadTables.UseVisualStyleBackColor = true;
            // 
            // dgvSensitivity
            // 
            this.dgvSensitivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSensitivity.Location = new System.Drawing.Point(31, 27);
            this.dgvSensitivity.Name = "dgvSensitivity";
            this.dgvSensitivity.RowHeadersWidth = 51;
            this.dgvSensitivity.RowTemplate.Height = 24;
            this.dgvSensitivity.Size = new System.Drawing.Size(622, 366);
            this.dgvSensitivity.TabIndex = 5;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnRevisedPrimal);
            this.pnlButtons.Controls.Add(this.btnPrimal);
            this.pnlButtons.Controls.Add(this.btnCuttingPlane);
            this.pnlButtons.Controls.Add(this.btnKnapsack);
            this.pnlButtons.Controls.Add(this.btnBranchAndBound);
            this.pnlButtons.Location = new System.Drawing.Point(12, 518);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(685, 81);
            this.pnlButtons.TabIndex = 12;
            // 
            // btnKnapsack
            // 
            this.btnKnapsack.Location = new System.Drawing.Point(15, 13);
            this.btnKnapsack.Name = "btnKnapsack";
            this.btnKnapsack.Size = new System.Drawing.Size(115, 53);
            this.btnKnapsack.TabIndex = 2;
            this.btnKnapsack.Text = "Knapsack";
            this.btnKnapsack.UseVisualStyleBackColor = true;
            this.btnKnapsack.Click += new System.EventHandler(this.btnKnapsack_Click);
            // 
            // btnBranchAndBound
            // 
            this.btnBranchAndBound.Location = new System.Drawing.Point(136, 13);
            this.btnBranchAndBound.Name = "btnBranchAndBound";
            this.btnBranchAndBound.Size = new System.Drawing.Size(128, 53);
            this.btnBranchAndBound.TabIndex = 5;
            this.btnBranchAndBound.Text = "Branch and Bound";
            this.btnBranchAndBound.UseVisualStyleBackColor = true;
            // 
            // pnlSensitivityInfo
            // 
            this.pnlSensitivityInfo.Location = new System.Drawing.Point(712, 12);
            this.pnlSensitivityInfo.Name = "pnlSensitivityInfo";
            this.pnlSensitivityInfo.Size = new System.Drawing.Size(596, 587);
            this.pnlSensitivityInfo.TabIndex = 13;
            this.pnlSensitivityInfo.MouseLeave += new System.EventHandler(this.pnlSensitivityInfo_MouseLeave);
            this.pnlSensitivityInfo.MouseHover += new System.EventHandler(this.pnlSensitivityInfo_MouseHover);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 611);
            this.Controls.Add(this.pnlSensitivityInfo);
            this.Controls.Add(this.pnlSensitivity);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlModel);
            this.Name = "Main";
            this.Text = "IP Model Solver";
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnlModel.ResumeLayout(false);
            this.pnlSensitivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSensitivity)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxDisplay;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCuttingPlane;
        private System.Windows.Forms.Button btnPrimal;
        private System.Windows.Forms.Button btnRevisedPrimal;
        private System.Windows.Forms.Panel pnlModel;
        private System.Windows.Forms.Panel pnlSensitivity;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnKnapsack;
        private System.Windows.Forms.Button btnBranchAndBound;
        private System.Windows.Forms.DataGridView dgvSensitivity;
        private System.Windows.Forms.Button btnChanges;
        private System.Windows.Forms.Button btnLoadTables;
        private System.Windows.Forms.Panel pnlSensitivityInfo;
    }
}

