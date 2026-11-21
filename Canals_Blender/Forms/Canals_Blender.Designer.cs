namespace Canals_Blender
{
    partial class Canals_Blender
    {
        /// <summary>
        /// Conteneur des composants du formulaire.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Libère les ressources utilisées par le formulaire.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.R_Picture = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.G_Picture = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.P_B = new System.Windows.Forms.Panel();
            this.B_Picture = new System.Windows.Forms.PictureBox();
            this.P_RGB = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.T_Package = new System.Windows.Forms.PictureBox();
            this.Btn_Delete_G = new System.Windows.Forms.Button();
            this.Btn_Download = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Btn_Delete_R = new System.Windows.Forms.Button();
            this.Btn_Delete_B = new System.Windows.Forms.Button();
            this.Btn_Delete_RGB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.R_Picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.G_Picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B_Picture)).BeginInit();
            this.P_RGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.T_Package)).BeginInit();
            this.SuspendLayout();
            // 
            // R_Picture
            // 
            this.R_Picture.AllowDrop = true;
            this.R_Picture.BackColor = System.Drawing.Color.Black;
            this.R_Picture.Cursor = System.Windows.Forms.Cursors.Default;
            this.R_Picture.Location = new System.Drawing.Point(13, 18);
            this.R_Picture.Name = "R_Picture";
            this.R_Picture.Size = new System.Drawing.Size(173, 173);
            this.R_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.R_Picture.TabIndex = 1;
            this.R_Picture.TabStop = false;
            this.R_Picture.DragDrop += new System.Windows.Forms.DragEventHandler(this.R_Picture_DragDrop);
            this.R_Picture.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxs_DragEnter);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Green;
            this.panel2.Location = new System.Drawing.Point(192, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(175, 175);
            this.panel2.TabIndex = 13;
            // 
            // G_Picture
            // 
            this.G_Picture.AllowDrop = true;
            this.G_Picture.BackColor = System.Drawing.Color.Black;
            this.G_Picture.Location = new System.Drawing.Point(193, 18);
            this.G_Picture.Name = "G_Picture";
            this.G_Picture.Size = new System.Drawing.Size(173, 173);
            this.G_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.G_Picture.TabIndex = 0;
            this.G_Picture.TabStop = false;
            this.G_Picture.DragDrop += new System.Windows.Forms.DragEventHandler(this.G_Picture_DragDrop);
            this.G_Picture.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxs_DragEnter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(12, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 175);
            this.panel1.TabIndex = 12;
            // 
            // P_B
            // 
            this.P_B.BackColor = System.Drawing.Color.DodgerBlue;
            this.P_B.Enabled = false;
            this.P_B.Location = new System.Drawing.Point(373, 17);
            this.P_B.Name = "P_B";
            this.P_B.Size = new System.Drawing.Size(175, 175);
            this.P_B.TabIndex = 0;
            // 
            // B_Picture
            // 
            this.B_Picture.AllowDrop = true;
            this.B_Picture.BackColor = System.Drawing.Color.Black;
            this.B_Picture.Location = new System.Drawing.Point(374, 18);
            this.B_Picture.Name = "B_Picture";
            this.B_Picture.Size = new System.Drawing.Size(173, 173);
            this.B_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.B_Picture.TabIndex = 0;
            this.B_Picture.TabStop = false;
            this.B_Picture.DragDrop += new System.Windows.Forms.DragEventHandler(this.B_Picture_DragDrop);
            this.B_Picture.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxs_DragEnter);
            // 
            // P_RGB
            // 
            this.P_RGB.BackColor = System.Drawing.Color.Green;
            this.P_RGB.Controls.Add(this.panel4);
            this.P_RGB.Controls.Add(this.panel6);
            this.P_RGB.Enabled = false;
            this.P_RGB.Location = new System.Drawing.Point(192, 239);
            this.P_RGB.Name = "P_RGB";
            this.P_RGB.Size = new System.Drawing.Size(175, 175);
            this.P_RGB.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Red;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(60, 175);
            this.panel4.TabIndex = 8;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(115, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(60, 175);
            this.panel6.TabIndex = 7;
            // 
            // T_Package
            // 
            this.T_Package.BackColor = System.Drawing.Color.Black;
            this.T_Package.Location = new System.Drawing.Point(193, 240);
            this.T_Package.Name = "T_Package";
            this.T_Package.Size = new System.Drawing.Size(173, 173);
            this.T_Package.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.T_Package.TabIndex = 0;
            this.T_Package.TabStop = false;
            // 
            // Btn_Delete_G
            // 
            this.Btn_Delete_G.AutoSize = true;
            this.Btn_Delete_G.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Delete_G.ForeColor = System.Drawing.Color.White;
            this.Btn_Delete_G.Location = new System.Drawing.Point(192, 199);
            this.Btn_Delete_G.Name = "Btn_Delete_G";
            this.Btn_Delete_G.Size = new System.Drawing.Size(174, 25);
            this.Btn_Delete_G.TabIndex = 17;
            this.Btn_Delete_G.Text = "Delete image";
            this.Btn_Delete_G.UseVisualStyleBackColor = true;
            this.Btn_Delete_G.Click += new System.EventHandler(this.Btn_Delete_G_Click);
            // 
            // Btn_Download
            // 
            this.Btn_Download.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Download.ForeColor = System.Drawing.Color.White;
            this.Btn_Download.Location = new System.Drawing.Point(192, 420);
            this.Btn_Download.Name = "Btn_Download";
            this.Btn_Download.Size = new System.Drawing.Size(175, 25);
            this.Btn_Download.TabIndex = 16;
            this.Btn_Download.Text = "Download";
            this.Btn_Download.UseVisualStyleBackColor = true;
            this.Btn_Download.Click += new System.EventHandler(this.Btn_Download_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(515, 466);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "v1.2.0";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Français",
            "English",
            "Español"});
            this.comboBox1.Location = new System.Drawing.Point(14, 240);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(173, 21);
            this.comboBox1.TabIndex = 19;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // Btn_Delete_R
            // 
            this.Btn_Delete_R.AutoSize = true;
            this.Btn_Delete_R.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Delete_R.ForeColor = System.Drawing.Color.White;
            this.Btn_Delete_R.Location = new System.Drawing.Point(13, 199);
            this.Btn_Delete_R.Name = "Btn_Delete_R";
            this.Btn_Delete_R.Size = new System.Drawing.Size(174, 25);
            this.Btn_Delete_R.TabIndex = 20;
            this.Btn_Delete_R.Text = "Delete image";
            this.Btn_Delete_R.UseVisualStyleBackColor = true;
            this.Btn_Delete_R.Click += new System.EventHandler(this.Btn_Delete_R_Click);
            // 
            // Btn_Delete_B
            // 
            this.Btn_Delete_B.AutoSize = true;
            this.Btn_Delete_B.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Delete_B.ForeColor = System.Drawing.Color.White;
            this.Btn_Delete_B.Location = new System.Drawing.Point(374, 199);
            this.Btn_Delete_B.Name = "Btn_Delete_B";
            this.Btn_Delete_B.Size = new System.Drawing.Size(174, 25);
            this.Btn_Delete_B.TabIndex = 21;
            this.Btn_Delete_B.Text = "Delete image";
            this.Btn_Delete_B.UseVisualStyleBackColor = true;
            this.Btn_Delete_B.Click += new System.EventHandler(this.Btn_Delete_B_Click);
            // 
            // Btn_Delete_RGB
            // 
            this.Btn_Delete_RGB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Btn_Delete_RGB.ForeColor = System.Drawing.Color.White;
            this.Btn_Delete_RGB.Location = new System.Drawing.Point(192, 451);
            this.Btn_Delete_RGB.Name = "Btn_Delete_RGB";
            this.Btn_Delete_RGB.Size = new System.Drawing.Size(175, 25);
            this.Btn_Delete_RGB.TabIndex = 22;
            this.Btn_Delete_RGB.Text = "Delete image";
            this.Btn_Delete_RGB.UseVisualStyleBackColor = true;
            this.Btn_Delete_RGB.Click += new System.EventHandler(this.Btn_Delete_Click);
            // 
            // Canals_Blender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(564, 488);
            this.Controls.Add(this.Btn_Delete_RGB);
            this.Controls.Add(this.Btn_Delete_B);
            this.Controls.Add(this.Btn_Delete_R);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.T_Package);
            this.Controls.Add(this.B_Picture);
            this.Controls.Add(this.G_Picture);
            this.Controls.Add(this.R_Picture);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.P_B);
            this.Controls.Add(this.P_RGB);
            this.Controls.Add(this.Btn_Delete_G);
            this.Controls.Add(this.Btn_Download);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Canals_Blender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Canals Blender";
            ((System.ComponentModel.ISupportInitialize)(this.R_Picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.G_Picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B_Picture)).EndInit();
            this.P_RGB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.T_Package)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox R_Picture;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox G_Picture;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel P_B;
        private System.Windows.Forms.PictureBox B_Picture;
        private System.Windows.Forms.Panel P_RGB;
        private System.Windows.Forms.PictureBox T_Package;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button Btn_Delete_G;
        private System.Windows.Forms.Button Btn_Download;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Btn_Delete_R;
        private System.Windows.Forms.Button Btn_Delete_B;
        private System.Windows.Forms.Button Btn_Delete_RGB;
    }
}