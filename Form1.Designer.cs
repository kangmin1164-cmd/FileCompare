namespace FileCompare
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            panel3 = new Panel();
            lvwLeftDir = new ListView();
            panel2 = new Panel();
            btnCopyFromLeft = new Button();
            txtLeftDir = new TextBox();
            panel1 = new Panel();
            btnLeftDir = new Button();
            lblAppname = new Label();
            panel6 = new Panel();
            lvwRightDir = new ListView();
            panel5 = new Panel();
            btnCopyFromRight = new Button();
            txtRightDir = new TextBox();
            panel4 = new Panel();
            btnRightDir = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel6);
            splitContainer1.Panel2.Controls.Add(panel5);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 393;
            splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(lvwLeftDir);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 184);
            panel3.Name = "panel3";
            panel3.Size = new Size(393, 266);
            panel3.TabIndex = 2;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Location = new Point(0, 0);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(390, 266);
            lvwLeftDir.TabIndex = 5;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnCopyFromLeft);
            panel2.Controls.Add(txtLeftDir);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 92);
            panel2.Name = "panel2";
            panel2.Size = new Size(393, 92);
            panel2.TabIndex = 1;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyFromLeft.Location = new Point(297, 28);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(93, 34);
            btnCopyFromLeft.TabIndex = 2;
            btnCopyFromLeft.Text = "폴더선택";
            btnCopyFromLeft.UseVisualStyleBackColor = true;
            // 
            // txtLeftDir
            // 
            txtLeftDir.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            txtLeftDir.Location = new Point(3, 31);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(295, 33);
            txtLeftDir.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLeftDir);
            panel1.Controls.Add(lblAppname);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(393, 92);
            panel1.TabIndex = 0;
            // 
            // btnLeftDir
            // 
            btnLeftDir.Location = new Point(329, 34);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(61, 34);
            btnLeftDir.TabIndex = 3;
            btnLeftDir.Text = ">>>";
            btnLeftDir.UseVisualStyleBackColor = true;
            btnLeftDir.Click += btnLeftDir_Click;
            // 
            // lblAppname
            // 
            lblAppname.AutoSize = true;
            lblAppname.Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAppname.ForeColor = Color.Blue;
            lblAppname.Location = new Point(3, 20);
            lblAppname.Name = "lblAppname";
            lblAppname.Size = new Size(283, 48);
            lblAppname.TabIndex = 0;
            lblAppname.Text = "File Compare";
            // 
            // panel6
            // 
            panel6.Controls.Add(lvwRightDir);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 184);
            panel6.Name = "panel6";
            panel6.Size = new Size(403, 266);
            panel6.TabIndex = 3;
            // 
            // lvwRightDir
            // 
            lvwRightDir.Location = new Point(3, 0);
            lvwRightDir.Name = "lvwRightDir";
            lvwRightDir.Size = new Size(400, 266);
            lvwRightDir.TabIndex = 6;
            lvwRightDir.UseCompatibleStateImageBehavior = false;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnCopyFromRight);
            panel5.Controls.Add(txtRightDir);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 92);
            panel5.Name = "panel5";
            panel5.Size = new Size(403, 92);
            panel5.TabIndex = 2;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyFromRight.Location = new Point(310, 31);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(93, 34);
            btnCopyFromRight.TabIndex = 3;
            btnCopyFromRight.Text = "폴더선택";
            btnCopyFromRight.UseVisualStyleBackColor = true;
            // 
            // txtRightDir
            // 
            txtRightDir.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            txtRightDir.Location = new Point(3, 31);
            txtRightDir.Name = "txtRightDir";
            txtRightDir.Size = new Size(307, 33);
            txtRightDir.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnRightDir);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(403, 92);
            panel4.TabIndex = 1;
            // 
            // btnRightDir
            // 
            btnRightDir.Location = new Point(3, 34);
            btnRightDir.Name = "btnRightDir";
            btnRightDir.Size = new Size(61, 34);
            btnRightDir.TabIndex = 4;
            btnRightDir.Text = "<<<";
            btnRightDir.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel3;
        private Panel panel2;
        private Panel panel1;
        private Panel panel5;
        private Panel panel4;
        private Button btnCopyFromLeft;
        private TextBox txtLeftDir;
        private Label lblAppname;
        private Panel panel6;
        private TextBox txtRightDir;
        private Button btnCopyFromRight;
        private ListView lvwLeftDir;
        private Button btnLeftDir;
        private ListView lvwRightDir;
        private Button btnRightDir;
    }
}
