namespace Glue
{
    partial class fSettings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.cbxMovementEnabled = new System.Windows.Forms.CheckBox();
            this.cbxPlayersOnly = new System.Windows.Forms.CheckBox();
            this.lblHonorbuddyTPS = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbHonorbuddyTPS = new System.Windows.Forms.TrackBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHonorbuddyTPS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInfo);
            this.groupBox1.Controls.Add(this.cbxMovementEnabled);
            this.groupBox1.Controls.Add(this.cbxPlayersOnly);
            this.groupBox1.Controls.Add(this.lblHonorbuddyTPS);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbHonorbuddyTPS);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(175, 13);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(75, 23);
            this.btnInfo.TabIndex = 11;
            this.btnInfo.Text = "Show Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // cbxMovementEnabled
            // 
            this.cbxMovementEnabled.AutoSize = true;
            this.cbxMovementEnabled.Checked = true;
            this.cbxMovementEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxMovementEnabled.Location = new System.Drawing.Point(6, 42);
            this.cbxMovementEnabled.Name = "cbxMovementEnabled";
            this.cbxMovementEnabled.Size = new System.Drawing.Size(118, 17);
            this.cbxMovementEnabled.TabIndex = 10;
            this.cbxMovementEnabled.Text = "Movement Enabled";
            this.cbxMovementEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxPlayersOnly
            // 
            this.cbxPlayersOnly.AutoSize = true;
            this.cbxPlayersOnly.Checked = true;
            this.cbxPlayersOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxPlayersOnly.Location = new System.Drawing.Point(6, 19);
            this.cbxPlayersOnly.Name = "cbxPlayersOnly";
            this.cbxPlayersOnly.Size = new System.Drawing.Size(84, 17);
            this.cbxPlayersOnly.TabIndex = 9;
            this.cbxPlayersOnly.Text = "Players Only";
            this.cbxPlayersOnly.UseVisualStyleBackColor = true;
            // 
            // lblHonorbuddyTPS
            // 
            this.lblHonorbuddyTPS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHonorbuddyTPS.Location = new System.Drawing.Point(232, 69);
            this.lblHonorbuddyTPS.Name = "lblHonorbuddyTPS";
            this.lblHonorbuddyTPS.Size = new System.Drawing.Size(19, 15);
            this.lblHonorbuddyTPS.TabIndex = 8;
            this.lblHonorbuddyTPS.Text = "0";
            this.lblHonorbuddyTPS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Honorbuddy [ Ticks Per Second ]";
            // 
            // tbHonorbuddyTPS
            // 
            this.tbHonorbuddyTPS.Location = new System.Drawing.Point(4, 85);
            this.tbHonorbuddyTPS.Maximum = 30;
            this.tbHonorbuddyTPS.Minimum = 15;
            this.tbHonorbuddyTPS.Name = "tbHonorbuddyTPS";
            this.tbHonorbuddyTPS.Size = new System.Drawing.Size(256, 45);
            this.tbHonorbuddyTPS.TabIndex = 6;
            this.tbHonorbuddyTPS.Value = 15;
            this.tbHonorbuddyTPS.Scroll += new System.EventHandler(this.tbHonorbuddyTPS_Scroll);
            // 
            // fSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 118);
            this.Controls.Add(this.groupBox1);
            this.Name = "fSettings";
            this.Text = "Glue Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fSettings_FormClosing);
            this.Load += new System.EventHandler(this.fSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHonorbuddyTPS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblHonorbuddyTPS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tbHonorbuddyTPS;
        private System.Windows.Forms.CheckBox cbxPlayersOnly;
        private System.Windows.Forms.CheckBox cbxMovementEnabled;
        private System.Windows.Forms.Button btnInfo;
    }
}