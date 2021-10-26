namespace ComputerVision
{
    partial class MainForm
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
            this.panelSource = new System.Windows.Forms.Panel();
            this.panelDestination = new System.Windows.Forms.Panel();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rotatieLbl = new System.Windows.Forms.TextBox();
            this.rotatieBtn = new System.Windows.Forms.Button();
            this.egalizareBtn = new System.Windows.Forms.Button();
            this.btnNegativare = new System.Windows.Forms.Button();
            this.buttonGrayscale = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.trackBarNegativare = new System.Windows.Forms.TrackBar();
            this.trackBarContrast = new System.Windows.Forms.TrackBar();
            this.LuminozitateLbl = new System.Windows.Forms.Label();
            this.ContrastLbl = new System.Windows.Forms.Label();
            this.ftjBtn = new System.Windows.Forms.Button();
            this.ftjTxtBox = new System.Windows.Forms.TextBox();
            this.outlierBtn = new System.Windows.Forms.Button();
            this.outlierTxtBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNegativare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSource
            // 
            this.panelSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelSource.Location = new System.Drawing.Point(12, 12);
            this.panelSource.Name = "panelSource";
            this.panelSource.Size = new System.Drawing.Size(320, 240);
            this.panelSource.TabIndex = 0;
            // 
            // panelDestination
            // 
            this.panelDestination.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDestination.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelDestination.Location = new System.Drawing.Point(348, 12);
            this.panelDestination.Name = "panelDestination";
            this.panelDestination.Size = new System.Drawing.Size(320, 240);
            this.panelDestination.TabIndex = 1;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(593, 507);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.outlierTxtBox);
            this.panel1.Controls.Add(this.outlierBtn);
            this.panel1.Controls.Add(this.ftjTxtBox);
            this.panel1.Controls.Add(this.ftjBtn);
            this.panel1.Controls.Add(this.rotatieLbl);
            this.panel1.Controls.Add(this.rotatieBtn);
            this.panel1.Controls.Add(this.egalizareBtn);
            this.panel1.Controls.Add(this.btnNegativare);
            this.panel1.Controls.Add(this.buttonGrayscale);
            this.panel1.Location = new System.Drawing.Point(348, 271);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 190);
            this.panel1.TabIndex = 3;
            // 
            // rotatieLbl
            // 
            this.rotatieLbl.Location = new System.Drawing.Point(19, 127);
            this.rotatieLbl.Name = "rotatieLbl";
            this.rotatieLbl.Size = new System.Drawing.Size(186, 20);
            this.rotatieLbl.TabIndex = 17;
            // 
            // rotatieBtn
            // 
            this.rotatieBtn.Location = new System.Drawing.Point(211, 127);
            this.rotatieBtn.Name = "rotatieBtn";
            this.rotatieBtn.Size = new System.Drawing.Size(91, 23);
            this.rotatieBtn.TabIndex = 16;
            this.rotatieBtn.Text = "Rotatie";
            this.rotatieBtn.UseVisualStyleBackColor = true;
            this.rotatieBtn.Click += new System.EventHandler(this.rotatieBtn_Click);
            // 
            // egalizareBtn
            // 
            this.egalizareBtn.Location = new System.Drawing.Point(208, 155);
            this.egalizareBtn.Margin = new System.Windows.Forms.Padding(2);
            this.egalizareBtn.Name = "egalizareBtn";
            this.egalizareBtn.Size = new System.Drawing.Size(94, 23);
            this.egalizareBtn.TabIndex = 15;
            this.egalizareBtn.Text = "Egalizare";
            this.egalizareBtn.UseVisualStyleBackColor = true;
            this.egalizareBtn.Click += new System.EventHandler(this.egalizareBtn_Click);
            // 
            // btnNegativare
            // 
            this.btnNegativare.Location = new System.Drawing.Point(115, 155);
            this.btnNegativare.Margin = new System.Windows.Forms.Padding(2);
            this.btnNegativare.Name = "btnNegativare";
            this.btnNegativare.Size = new System.Drawing.Size(89, 23);
            this.btnNegativare.TabIndex = 14;
            this.btnNegativare.Text = "Negativare";
            this.btnNegativare.UseVisualStyleBackColor = true;
            this.btnNegativare.Click += new System.EventHandler(this.BtnNegativare_Click);
            // 
            // buttonGrayscale
            // 
            this.buttonGrayscale.Location = new System.Drawing.Point(19, 155);
            this.buttonGrayscale.Name = "buttonGrayscale";
            this.buttonGrayscale.Size = new System.Drawing.Size(91, 23);
            this.buttonGrayscale.TabIndex = 13;
            this.buttonGrayscale.Text = "Grayscale";
            this.buttonGrayscale.UseVisualStyleBackColor = true;
            this.buttonGrayscale.Click += new System.EventHandler(this.BtnGrayscale_Click);
            // 
            // trackBarNegativare
            // 
            this.trackBarNegativare.Location = new System.Drawing.Point(11, 498);
            this.trackBarNegativare.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarNegativare.Maximum = 255;
            this.trackBarNegativare.Minimum = -255;
            this.trackBarNegativare.Name = "trackBarNegativare";
            this.trackBarNegativare.Size = new System.Drawing.Size(131, 45);
            this.trackBarNegativare.TabIndex = 4;
            this.trackBarNegativare.Scroll += new System.EventHandler(this.TrackBarNegativare_Scroll);
            // 
            // trackBarContrast
            // 
            this.trackBarContrast.Location = new System.Drawing.Point(170, 498);
            this.trackBarContrast.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarContrast.Maximum = 120;
            this.trackBarContrast.Minimum = -120;
            this.trackBarContrast.Name = "trackBarContrast";
            this.trackBarContrast.Size = new System.Drawing.Size(115, 45);
            this.trackBarContrast.TabIndex = 5;
            this.trackBarContrast.Scroll += new System.EventHandler(this.TrackBarContrast_Scroll);
            // 
            // LuminozitateLbl
            // 
            this.LuminozitateLbl.AutoSize = true;
            this.LuminozitateLbl.Location = new System.Drawing.Point(51, 471);
            this.LuminozitateLbl.Name = "LuminozitateLbl";
            this.LuminozitateLbl.Size = new System.Drawing.Size(66, 13);
            this.LuminozitateLbl.TabIndex = 6;
            this.LuminozitateLbl.Text = "Luminozitate";
            // 
            // ContrastLbl
            // 
            this.ContrastLbl.AutoSize = true;
            this.ContrastLbl.Location = new System.Drawing.Point(206, 471);
            this.ContrastLbl.Name = "ContrastLbl";
            this.ContrastLbl.Size = new System.Drawing.Size(46, 13);
            this.ContrastLbl.TabIndex = 7;
            this.ContrastLbl.Text = "Contrast";
            // 
            // ftjBtn
            // 
            this.ftjBtn.Location = new System.Drawing.Point(211, 98);
            this.ftjBtn.Name = "ftjBtn";
            this.ftjBtn.Size = new System.Drawing.Size(91, 23);
            this.ftjBtn.TabIndex = 18;
            this.ftjBtn.Text = "FTJ";
            this.ftjBtn.UseVisualStyleBackColor = true;
            this.ftjBtn.Click += new System.EventHandler(this.ftjBtn_Click);
            // 
            // ftjTxtBox
            // 
            this.ftjTxtBox.Location = new System.Drawing.Point(19, 101);
            this.ftjTxtBox.Name = "ftjTxtBox";
            this.ftjTxtBox.Size = new System.Drawing.Size(186, 20);
            this.ftjTxtBox.TabIndex = 19;
            // 
            // outlierBtn
            // 
            this.outlierBtn.Location = new System.Drawing.Point(211, 69);
            this.outlierBtn.Name = "outlierBtn";
            this.outlierBtn.Size = new System.Drawing.Size(91, 23);
            this.outlierBtn.TabIndex = 20;
            this.outlierBtn.Text = "Outlier";
            this.outlierBtn.UseVisualStyleBackColor = true;
            this.outlierBtn.Click += new System.EventHandler(this.outlierBtn_Click);
            // 
            // outlierTxtBox
            // 
            this.outlierTxtBox.Location = new System.Drawing.Point(19, 72);
            this.outlierTxtBox.Name = "outlierTxtBox";
            this.outlierTxtBox.Size = new System.Drawing.Size(186, 20);
            this.outlierTxtBox.TabIndex = 21;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 542);
            this.Controls.Add(this.ContrastLbl);
            this.Controls.Add(this.LuminozitateLbl);
            this.Controls.Add(this.trackBarContrast);
            this.Controls.Add(this.trackBarNegativare);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.panelDestination);
            this.Controls.Add(this.panelSource);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNegativare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelSource;
        private System.Windows.Forms.Panel panelDestination;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonGrayscale;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnNegativare;
        private System.Windows.Forms.TrackBar trackBarNegativare;
        private System.Windows.Forms.TrackBar trackBarContrast;
        private System.Windows.Forms.Label LuminozitateLbl;
        private System.Windows.Forms.Label ContrastLbl;
        private System.Windows.Forms.Button egalizareBtn;
        private System.Windows.Forms.Button rotatieBtn;
        private System.Windows.Forms.TextBox rotatieLbl;
        private System.Windows.Forms.TextBox ftjTxtBox;
        private System.Windows.Forms.Button ftjBtn;
        private System.Windows.Forms.TextBox outlierTxtBox;
        private System.Windows.Forms.Button outlierBtn;
    }
}

