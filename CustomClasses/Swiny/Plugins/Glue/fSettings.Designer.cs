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
            this.cbxMovementEnabled = new System.Windows.Forms.CheckBox();
            this.cbxPlayersOnly = new System.Windows.Forms.CheckBox();
            this.lblHonorbuddyTPS = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbHonorbuddyTPS = new System.Windows.Forms.TrackBar();
            this.lblMovementDistance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMovementDistance = new System.Windows.Forms.TrackBar();
            this.lblMovementTPS = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMovementTPS = new System.Windows.Forms.TrackBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHonorbuddyTPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMovementDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMovementTPS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxMovementEnabled);
            this.groupBox1.Controls.Add(this.cbxPlayersOnly);
            this.groupBox1.Controls.Add(this.lblHonorbuddyTPS);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbHonorbuddyTPS);
            this.groupBox1.Controls.Add(this.lblMovementDistance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbMovementDistance);
            this.groupBox1.Controls.Add(this.lblMovementTPS);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbMovementTPS);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 226);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
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
            this.lblHonorbuddyTPS.Location = new System.Drawing.Point(232, 173);
            this.lblHonorbuddyTPS.Name = "lblHonorbuddyTPS";
            this.lblHonorbuddyTPS.Size = new System.Drawing.Size(19, 15);
            this.lblHonorbuddyTPS.TabIndex = 8;
            this.lblHonorbuddyTPS.Text = "0";
            this.lblHonorbuddyTPS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Honorbuddy [ Ticks Per Second ]";
            // 
            // tbHonorbuddyTPS
            // 
            this.tbHonorbuddyTPS.Location = new System.Drawing.Point(4, 189);
            this.tbHonorbuddyTPS.Maximum = 30;
            this.tbHonorbuddyTPS.Minimum = 15;
            this.tbHonorbuddyTPS.Name = "tbHonorbuddyTPS";
            this.tbHonorbuddyTPS.Size = new System.Drawing.Size(256, 45);
            this.tbHonorbuddyTPS.TabIndex = 6;
            this.tbHonorbuddyTPS.Value = 15;
            this.tbHonorbuddyTPS.Scroll += new System.EventHandler(this.tbHonorbuddyTPS_Scroll);
            // 
            // lblMovementDistance
            // 
            this.lblMovementDistance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMovementDistance.Location = new System.Drawing.Point(232, 122);
            this.lblMovementDistance.Name = "lblMovementDistance";
            this.lblMovementDistance.Size = new System.Drawing.Size(19, 15);
            this.lblMovementDistance.TabIndex = 5;
            this.lblMovementDistance.Text = "0";
            this.lblMovementDistance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Movement Distance";
            // 
            // tbMovementDistance
            // 
            this.tbMovementDistance.Location = new System.Drawing.Point(4, 138);
            this.tbMovementDistance.Maximum = 15;
            this.tbMovementDistance.Minimum = 5;
            this.tbMovementDistance.Name = "tbMovementDistance";
            this.tbMovementDistance.Size = new System.Drawing.Size(256, 45);
            this.tbMovementDistance.TabIndex = 3;
            this.tbMovementDistance.Value = 10;
            this.tbMovementDistance.Scroll += new System.EventHandler(this.tbMovementDistance_Scroll);
            // 
            // lblMovementTPS
            // 
            this.lblMovementTPS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMovementTPS.Location = new System.Drawing.Point(232, 70);
            this.lblMovementTPS.Name = "lblMovementTPS";
            this.lblMovementTPS.Size = new System.Drawing.Size(19, 15);
            this.lblMovementTPS.TabIndex = 2;
            this.lblMovementTPS.Text = "0";
            this.lblMovementTPS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Movement [ Ticks Per Second ]";
            // 
            // tbMovementTPS
            // 
            this.tbMovementTPS.Location = new System.Drawing.Point(4, 87);
            this.tbMovementTPS.Maximum = 60;
            this.tbMovementTPS.Minimum = 20;
            this.tbMovementTPS.Name = "tbMovementTPS";
            this.tbMovementTPS.Size = new System.Drawing.Size(256, 45);
            this.tbMovementTPS.TabIndex = 0;
            this.tbMovementTPS.Value = 45;
            this.tbMovementTPS.Scroll += new System.EventHandler(this.tbMovementTPS_Scroll);
            // 
            // fSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 226);
            this.Controls.Add(this.groupBox1);
            this.Name = "fSettings";
            this.Text = "Glue Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fSettings_FormClosing);
            this.Load += new System.EventHandler(this.fSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHonorbuddyTPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMovementDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMovementTPS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblMovementTPS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbMovementTPS;
        private System.Windows.Forms.Label lblMovementDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tbMovementDistance;
        private System.Windows.Forms.Label lblHonorbuddyTPS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tbHonorbuddyTPS;
        private System.Windows.Forms.CheckBox cbxPlayersOnly;
        private System.Windows.Forms.CheckBox cbxMovementEnabled;
    }
}