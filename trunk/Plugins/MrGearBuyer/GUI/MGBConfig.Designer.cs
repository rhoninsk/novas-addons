namespace MrGearBuyer.GUI
{
    partial class MGBConfig
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
            this.AddtoBuy = new System.Windows.Forms.Button();
            this.Fetch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvVenderList = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.dgvBuyList = new System.Windows.Forms.DataGridView();
            this.RemoveBuy = new System.Windows.Forms.Button();
            this.LogoutAtCap = new System.Windows.Forms.CheckBox();
            this.RemoveJPHPWhenCapped = new System.Windows.Forms.CheckBox();
            this.BuyOppositePointToBuildUp = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVenderList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyList)).BeginInit();
            this.SuspendLayout();
            // 
            // AddtoBuy
            // 
            this.AddtoBuy.Location = new System.Drawing.Point(1, 258);
            this.AddtoBuy.Name = "AddtoBuy";
            this.AddtoBuy.Size = new System.Drawing.Size(168, 23);
            this.AddtoBuy.TabIndex = 4;
            this.AddtoBuy.Text = "Add Selected Items To Buy List";
            this.AddtoBuy.UseVisualStyleBackColor = true;
            this.AddtoBuy.Click += new System.EventHandler(this.AddtoBuyClick);
            // 
            // Fetch
            // 
            this.Fetch.Location = new System.Drawing.Point(169, 258);
            this.Fetch.Name = "Fetch";
            this.Fetch.Size = new System.Drawing.Size(110, 23);
            this.Fetch.TabIndex = 5;
            this.Fetch.Text = "Fetch Vender Items";
            this.Fetch.UseVisualStyleBackColor = true;
            this.Fetch.Click += new System.EventHandler(this.FetchClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.Fetch);
            this.groupBox1.Controls.Add(this.dgvVenderList);
            this.groupBox1.Controls.Add(this.AddtoBuy);
            this.groupBox1.Location = new System.Drawing.Point(288, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 293);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vender Controls";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // dgvVenderList
            // 
            this.dgvVenderList.AllowUserToAddRows = false;
            this.dgvVenderList.AllowUserToDeleteRows = false;
            this.dgvVenderList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVenderList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvVenderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVenderList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvVenderList.Location = new System.Drawing.Point(6, 19);
            this.dgvVenderList.Name = "dgvVenderList";
            this.dgvVenderList.ReadOnly = true;
            this.dgvVenderList.RowHeadersVisible = false;
            this.dgvVenderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVenderList.Size = new System.Drawing.Size(270, 233);
            this.dgvVenderList.TabIndex = 4;
            this.dgvVenderList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvVenderListCellMouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMoveDown);
            this.groupBox2.Controls.Add(this.btnMoveUp);
            this.groupBox2.Controls.Add(this.dgvBuyList);
            this.groupBox2.Controls.Add(this.RemoveBuy);
            this.groupBox2.Location = new System.Drawing.Point(0, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 293);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "My Buy List";
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(197, 258);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 4;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.BtnMoveDownClick);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(116, 258);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 3;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.BtnMoveUpClick);
            // 
            // dgvBuyList
            // 
            this.dgvBuyList.AllowUserToAddRows = false;
            this.dgvBuyList.AllowUserToDeleteRows = false;
            this.dgvBuyList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBuyList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBuyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuyList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBuyList.Location = new System.Drawing.Point(6, 19);
            this.dgvBuyList.Name = "dgvBuyList";
            this.dgvBuyList.RowHeadersVisible = false;
            this.dgvBuyList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuyList.Size = new System.Drawing.Size(270, 233);
            this.dgvBuyList.TabIndex = 2;
            this.dgvBuyList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvBuyListCellMouseDoubleClick);
            // 
            // RemoveBuy
            // 
            this.RemoveBuy.Location = new System.Drawing.Point(6, 258);
            this.RemoveBuy.Name = "RemoveBuy";
            this.RemoveBuy.Size = new System.Drawing.Size(104, 23);
            this.RemoveBuy.TabIndex = 1;
            this.RemoveBuy.Text = "Remove Selected Item";
            this.RemoveBuy.UseVisualStyleBackColor = true;
            this.RemoveBuy.Click += new System.EventHandler(this.RemoveBuyClick);
            // 
            // LogoutAtCap
            // 
            this.LogoutAtCap.AutoSize = true;
            this.LogoutAtCap.Location = new System.Drawing.Point(2, 300);
            this.LogoutAtCap.Name = "LogoutAtCap";
            this.LogoutAtCap.Size = new System.Drawing.Size(267, 17);
            this.LogoutAtCap.TabIndex = 10;
            this.LogoutAtCap.Text = "Logout when Honor and Justice Points are Capped";
            this.LogoutAtCap.UseVisualStyleBackColor = true;
            this.LogoutAtCap.CheckedChanged += new System.EventHandler(this.LogoutAtCapCheckedChanged);
            // 
            // RemoveJPHPWhenCapped
            // 
            this.RemoveJPHPWhenCapped.AutoSize = true;
            this.RemoveJPHPWhenCapped.Location = new System.Drawing.Point(266, 300);
            this.RemoveJPHPWhenCapped.Name = "RemoveJPHPWhenCapped";
            this.RemoveJPHPWhenCapped.Size = new System.Drawing.Size(309, 17);
            this.RemoveJPHPWhenCapped.TabIndex = 11;
            this.RemoveJPHPWhenCapped.Text = "Only Remove Justice / Honor Points from List when Capped";
            this.RemoveJPHPWhenCapped.UseVisualStyleBackColor = true;
            this.RemoveJPHPWhenCapped.CheckedChanged += new System.EventHandler(this.RemoveJphpWhenCappedCheckedChanged);
            // 
            // BuyOppositePointToBuildUp
            // 
            this.BuyOppositePointToBuildUp.AutoSize = true;
            this.BuyOppositePointToBuildUp.Location = new System.Drawing.Point(2, 321);
            this.BuyOppositePointToBuildUp.Name = "BuyOppositePointToBuildUp";
            this.BuyOppositePointToBuildUp.Size = new System.Drawing.Size(355, 17);
            this.BuyOppositePointToBuildUp.TabIndex = 12;
            this.BuyOppositePointToBuildUp.Text = "Allow Automatic Conversion of Honor and Justice Points to Buy Gear?";
            this.BuyOppositePointToBuildUp.UseVisualStyleBackColor = true;
            this.BuyOppositePointToBuildUp.CheckedChanged += new System.EventHandler(this.BuyOppositePointToBuildUp_CheckedChanged);
            // 
            // MGBConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 338);
            this.Controls.Add(this.BuyOppositePointToBuildUp);
            this.Controls.Add(this.RemoveJPHPWhenCapped);
            this.Controls.Add(this.LogoutAtCap);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MGBConfig";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mr.GearBuyer";
            this.Load += new System.EventHandler(this.Hc2ConfigLoad);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVenderList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddtoBuy;
        private System.Windows.Forms.Button Fetch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button RemoveBuy;
        private System.Windows.Forms.DataGridView dgvBuyList;
        private System.Windows.Forms.DataGridView dgvVenderList;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.CheckBox LogoutAtCap;
        private System.Windows.Forms.CheckBox RemoveJPHPWhenCapped;
        private System.Windows.Forms.CheckBox BuyOppositePointToBuildUp;
    }
}