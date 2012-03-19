namespace InterrupterUltimate
{
    partial class GUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.bReloadSettings = new System.Windows.Forms.Button();
            this.bClose_Settings = new System.Windows.Forms.Button();
            this.bSaveSettings = new System.Windows.Forms.Button();
            this.gbMiscSettings = new System.Windows.Forms.GroupBox();
            this.cbShouldForceCast = new System.Windows.Forms.CheckBox();
            this.cbInterruptAll = new System.Windows.Forms.CheckBox();
            this.gbInterruptWhen = new System.Windows.Forms.GroupBox();
            this.nudChannelMillisecondsElapsed = new System.Windows.Forms.NumericUpDown();
            this.lChannelMillisecondsElapsed = new System.Windows.Forms.Label();
            this.lCastMillisecondsLeft = new System.Windows.Forms.Label();
            this.nudCastMilliSecondsLeft = new System.Windows.Forms.NumericUpDown();
            this.tpTagGroupsDB = new System.Windows.Forms.TabPage();
            this.bClose_TagGroups = new System.Windows.Forms.Button();
            this.dgvTagGroupsAddButton = new System.Windows.Forms.Button();
            this.dgvTagGroupsVerifyFirstButton = new System.Windows.Forms.Button();
            this.dgvTagGroupsRemoveAllButton = new System.Windows.Forms.Button();
            this.dgvTagGroupsRemoveOneButton = new System.Windows.Forms.Button();
            this.dgvTagGroupsVerifyAllButton = new System.Windows.Forms.Button();
            this.dgvTagGroupsDB = new System.Windows.Forms.DataGridView();
            this.dgvTagGroupsStatusButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvTagGroupsRemoveButton = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvTagGroupsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTagGroupsInterrupt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvTagGroupsExclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvTagGroupsAliases = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpSpellsDB = new System.Windows.Forms.TabPage();
            this.bClose_Spells = new System.Windows.Forms.Button();
            this.dgvSpellsAddButton = new System.Windows.Forms.Button();
            this.dgvSpellsVerifyFirstButton = new System.Windows.Forms.Button();
            this.dgvSpellsRemoveAllButton = new System.Windows.Forms.Button();
            this.dgvSpellsDB = new System.Windows.Forms.DataGridView();
            this.dgvSpellsVerifyButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvSpellsRemoveCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvSpellsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSpellsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSpellsInterrupt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvSpellsTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSpellsRemoveOneButton = new System.Windows.Forms.Button();
            this.dgvSpellsVerifyAllButton = new System.Windows.Forms.Button();
            this.tpTargets = new System.Windows.Forms.TabPage();
            this.bClose_Targets = new System.Windows.Forms.Button();
            this.dgvTargetsAddButton = new System.Windows.Forms.Button();
            this.dgvTargetsVerifyFirstButton = new System.Windows.Forms.Button();
            this.dgvTargetsRemoveAllButton = new System.Windows.Forms.Button();
            this.dgvTargets = new System.Windows.Forms.DataGridView();
            this.dgvTargetsVerifyButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvTargetsRemoveCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvTargetsUnitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTargetsIncludedCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvTargetsRemoveOneButton = new System.Windows.Forms.Button();
            this.dgvTargetsVerifyAllButton = new System.Windows.Forms.Button();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.gbLogging = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.gbMiscSettings.SuspendLayout();
            this.gbInterruptWhen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannelMillisecondsElapsed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCastMilliSecondsLeft)).BeginInit();
            this.tpTagGroupsDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagGroupsDB)).BeginInit();
            this.tpSpellsDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpellsDB)).BeginInit();
            this.tpTargets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTargets)).BeginInit();
            this.gbLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSettings);
            this.tabControl1.Controls.Add(this.tpTagGroupsDB);
            this.tabControl1.Controls.Add(this.tpSpellsDB);
            this.tabControl1.Controls.Add(this.tpTargets);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(634, 451);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.gbLogging);
            this.tpSettings.Controls.Add(this.bReloadSettings);
            this.tpSettings.Controls.Add(this.bClose_Settings);
            this.tpSettings.Controls.Add(this.bSaveSettings);
            this.tpSettings.Controls.Add(this.gbMiscSettings);
            this.tpSettings.Controls.Add(this.gbInterruptWhen);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tpSettings.Size = new System.Drawing.Size(626, 425);
            this.tpSettings.TabIndex = 2;
            this.tpSettings.Text = "Settings";
            this.tpSettings.ToolTipText = "Settings...";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // bReloadSettings
            // 
            this.bReloadSettings.Location = new System.Drawing.Point(25, 245);
            this.bReloadSettings.Name = "bReloadSettings";
            this.bReloadSettings.Size = new System.Drawing.Size(79, 21);
            this.bReloadSettings.TabIndex = 16;
            this.bReloadSettings.Text = "Reload";
            this.bReloadSettings.UseVisualStyleBackColor = true;
            this.bReloadSettings.Click += new System.EventHandler(this.bReloadSettings_Click);
            // 
            // bClose_Settings
            // 
            this.bClose_Settings.Location = new System.Drawing.Point(537, 399);
            this.bClose_Settings.Name = "bClose_Settings";
            this.bClose_Settings.Size = new System.Drawing.Size(83, 21);
            this.bClose_Settings.TabIndex = 15;
            this.bClose_Settings.Text = "Close";
            this.bClose_Settings.UseVisualStyleBackColor = true;
            this.bClose_Settings.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bSaveSettings
            // 
            this.bSaveSettings.Location = new System.Drawing.Point(110, 245);
            this.bSaveSettings.Name = "bSaveSettings";
            this.bSaveSettings.Size = new System.Drawing.Size(79, 21);
            this.bSaveSettings.TabIndex = 14;
            this.bSaveSettings.Text = "Save";
            this.bSaveSettings.UseVisualStyleBackColor = true;
            this.bSaveSettings.Click += new System.EventHandler(this.bSaveSettings_Click);
            // 
            // gbMiscSettings
            // 
            this.gbMiscSettings.Controls.Add(this.cbShouldForceCast);
            this.gbMiscSettings.Controls.Add(this.cbInterruptAll);
            this.gbMiscSettings.Location = new System.Drawing.Point(8, 15);
            this.gbMiscSettings.Name = "gbMiscSettings";
            this.gbMiscSettings.Size = new System.Drawing.Size(198, 70);
            this.gbMiscSettings.TabIndex = 13;
            this.gbMiscSettings.TabStop = false;
            this.gbMiscSettings.Text = "Misc";
            // 
            // cbShouldForceCast
            // 
            this.cbShouldForceCast.AutoSize = true;
            this.cbShouldForceCast.Location = new System.Drawing.Point(17, 19);
            this.cbShouldForceCast.Name = "cbShouldForceCast";
            this.cbShouldForceCast.Size = new System.Drawing.Size(158, 17);
            this.cbShouldForceCast.TabIndex = 1;
            this.cbShouldForceCast.Text = "stop current cast to interrupt";
            this.cbShouldForceCast.UseVisualStyleBackColor = true;
            // 
            // cbInterruptAll
            // 
            this.cbInterruptAll.AutoSize = true;
            this.cbInterruptAll.Location = new System.Drawing.Point(17, 42);
            this.cbInterruptAll.Name = "cbInterruptAll";
            this.cbInterruptAll.Size = new System.Drawing.Size(137, 17);
            this.cbInterruptAll.TabIndex = 5;
            this.cbInterruptAll.Text = "interrupt every spellcast";
            this.cbInterruptAll.UseVisualStyleBackColor = true;
            // 
            // gbInterruptWhen
            // 
            this.gbInterruptWhen.Controls.Add(this.nudChannelMillisecondsElapsed);
            this.gbInterruptWhen.Controls.Add(this.lChannelMillisecondsElapsed);
            this.gbInterruptWhen.Controls.Add(this.lCastMillisecondsLeft);
            this.gbInterruptWhen.Controls.Add(this.nudCastMilliSecondsLeft);
            this.gbInterruptWhen.Location = new System.Drawing.Point(8, 91);
            this.gbInterruptWhen.Name = "gbInterruptWhen";
            this.gbInterruptWhen.Size = new System.Drawing.Size(198, 95);
            this.gbInterruptWhen.TabIndex = 11;
            this.gbInterruptWhen.TabStop = false;
            this.gbInterruptWhen.Text = "Interrupt when:";
            // 
            // nudChannelMillisecondsElapsed
            // 
            this.nudChannelMillisecondsElapsed.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudChannelMillisecondsElapsed.Location = new System.Drawing.Point(17, 54);
            this.nudChannelMillisecondsElapsed.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudChannelMillisecondsElapsed.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudChannelMillisecondsElapsed.Name = "nudChannelMillisecondsElapsed";
            this.nudChannelMillisecondsElapsed.Size = new System.Drawing.Size(42, 20);
            this.nudChannelMillisecondsElapsed.TabIndex = 10;
            this.nudChannelMillisecondsElapsed.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // lChannelMillisecondsElapsed
            // 
            this.lChannelMillisecondsElapsed.AutoSize = true;
            this.lChannelMillisecondsElapsed.Location = new System.Drawing.Point(65, 56);
            this.lChannelMillisecondsElapsed.Name = "lChannelMillisecondsElapsed";
            this.lChannelMillisecondsElapsed.Size = new System.Drawing.Size(130, 13);
            this.lChannelMillisecondsElapsed.TabIndex = 9;
            this.lChannelMillisecondsElapsed.Text = "ms elapsed on channeling";
            // 
            // lCastMillisecondsLeft
            // 
            this.lCastMillisecondsLeft.AutoSize = true;
            this.lCastMillisecondsLeft.Location = new System.Drawing.Point(65, 30);
            this.lCastMillisecondsLeft.Name = "lCastMillisecondsLeft";
            this.lCastMillisecondsLeft.Size = new System.Drawing.Size(113, 13);
            this.lCastMillisecondsLeft.TabIndex = 8;
            this.lCastMillisecondsLeft.Text = "ms left to finish casting";
            // 
            // nudCastMilliSecondsLeft
            // 
            this.nudCastMilliSecondsLeft.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCastMilliSecondsLeft.Location = new System.Drawing.Point(17, 28);
            this.nudCastMilliSecondsLeft.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudCastMilliSecondsLeft.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudCastMilliSecondsLeft.Name = "nudCastMilliSecondsLeft";
            this.nudCastMilliSecondsLeft.Size = new System.Drawing.Size(42, 20);
            this.nudCastMilliSecondsLeft.TabIndex = 6;
            this.nudCastMilliSecondsLeft.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // tpTagGroupsDB
            // 
            this.tpTagGroupsDB.Controls.Add(this.bClose_TagGroups);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsAddButton);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsVerifyFirstButton);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsRemoveAllButton);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsRemoveOneButton);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsVerifyAllButton);
            this.tpTagGroupsDB.Controls.Add(this.dgvTagGroupsDB);
            this.tpTagGroupsDB.Location = new System.Drawing.Point(4, 22);
            this.tpTagGroupsDB.Name = "tpTagGroupsDB";
            this.tpTagGroupsDB.Padding = new System.Windows.Forms.Padding(3);
            this.tpTagGroupsDB.Size = new System.Drawing.Size(626, 425);
            this.tpTagGroupsDB.TabIndex = 1;
            this.tpTagGroupsDB.Text = "Database - Tag Groups";
            this.tpTagGroupsDB.ToolTipText = "Tag groups database";
            this.tpTagGroupsDB.UseVisualStyleBackColor = true;
            // 
            // bClose_TagGroups
            // 
            this.bClose_TagGroups.Location = new System.Drawing.Point(537, 399);
            this.bClose_TagGroups.Name = "bClose_TagGroups";
            this.bClose_TagGroups.Size = new System.Drawing.Size(83, 21);
            this.bClose_TagGroups.TabIndex = 11;
            this.bClose_TagGroups.Text = "Close";
            this.bClose_TagGroups.UseVisualStyleBackColor = true;
            this.bClose_TagGroups.Click += new System.EventHandler(this.bClose_Click);
            // 
            // dgvTagGroupsAddButton
            // 
            this.dgvTagGroupsAddButton.Location = new System.Drawing.Point(6, 399);
            this.dgvTagGroupsAddButton.Name = "dgvTagGroupsAddButton";
            this.dgvTagGroupsAddButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTagGroupsAddButton.TabIndex = 10;
            this.dgvTagGroupsAddButton.Text = "Add";
            this.dgvTagGroupsAddButton.UseVisualStyleBackColor = true;
            this.dgvTagGroupsAddButton.Click += new System.EventHandler(this.dgvTagGroupsAddButton_Click);
            // 
            // dgvTagGroupsVerifyFirstButton
            // 
            this.dgvTagGroupsVerifyFirstButton.Location = new System.Drawing.Point(95, 399);
            this.dgvTagGroupsVerifyFirstButton.Name = "dgvTagGroupsVerifyFirstButton";
            this.dgvTagGroupsVerifyFirstButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTagGroupsVerifyFirstButton.TabIndex = 9;
            this.dgvTagGroupsVerifyFirstButton.Text = "Verify First";
            this.dgvTagGroupsVerifyFirstButton.UseVisualStyleBackColor = true;
            this.dgvTagGroupsVerifyFirstButton.Click += new System.EventHandler(this.dgvTagGroupsVerifyFirstButton_Click);
            // 
            // dgvTagGroupsRemoveAllButton
            // 
            this.dgvTagGroupsRemoveAllButton.Location = new System.Drawing.Point(362, 399);
            this.dgvTagGroupsRemoveAllButton.Name = "dgvTagGroupsRemoveAllButton";
            this.dgvTagGroupsRemoveAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTagGroupsRemoveAllButton.TabIndex = 8;
            this.dgvTagGroupsRemoveAllButton.Text = "Remove All";
            this.dgvTagGroupsRemoveAllButton.UseVisualStyleBackColor = true;
            this.dgvTagGroupsRemoveAllButton.Click += new System.EventHandler(this.dgvTagGroupsRemoveAllButton_Click);
            // 
            // dgvTagGroupsRemoveOneButton
            // 
            this.dgvTagGroupsRemoveOneButton.Location = new System.Drawing.Point(273, 399);
            this.dgvTagGroupsRemoveOneButton.Name = "dgvTagGroupsRemoveOneButton";
            this.dgvTagGroupsRemoveOneButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTagGroupsRemoveOneButton.TabIndex = 7;
            this.dgvTagGroupsRemoveOneButton.Text = "Remove One";
            this.dgvTagGroupsRemoveOneButton.UseVisualStyleBackColor = true;
            this.dgvTagGroupsRemoveOneButton.Click += new System.EventHandler(this.dgvTagGroupsRemoveOneButton_Click);
            // 
            // dgvTagGroupsVerifyAllButton
            // 
            this.dgvTagGroupsVerifyAllButton.Location = new System.Drawing.Point(184, 399);
            this.dgvTagGroupsVerifyAllButton.Name = "dgvTagGroupsVerifyAllButton";
            this.dgvTagGroupsVerifyAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTagGroupsVerifyAllButton.TabIndex = 6;
            this.dgvTagGroupsVerifyAllButton.Text = "Verify All";
            this.dgvTagGroupsVerifyAllButton.UseVisualStyleBackColor = true;
            this.dgvTagGroupsVerifyAllButton.Click += new System.EventHandler(this.dgvTagGroupsVerifyAllButton_Click);
            // 
            // dgvTagGroupsDB
            // 
            this.dgvTagGroupsDB.AllowUserToAddRows = false;
            this.dgvTagGroupsDB.AllowUserToDeleteRows = false;
            this.dgvTagGroupsDB.AllowUserToOrderColumns = true;
            this.dgvTagGroupsDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTagGroupsDB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTagGroupsStatusButton,
            this.dgvTagGroupsRemoveButton,
            this.dgvTagGroupsID,
            this.dgvTagGroupsInterrupt,
            this.dgvTagGroupsExclude,
            this.dgvTagGroupsAliases});
            this.dgvTagGroupsDB.Location = new System.Drawing.Point(3, 3);
            this.dgvTagGroupsDB.Name = "dgvTagGroupsDB";
            this.dgvTagGroupsDB.Size = new System.Drawing.Size(623, 390);
            this.dgvTagGroupsDB.TabIndex = 0;
            // 
            // dgvTagGroupsStatusButton
            // 
            this.dgvTagGroupsStatusButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsStatusButton.Frozen = true;
            this.dgvTagGroupsStatusButton.HeaderText = "Status";
            this.dgvTagGroupsStatusButton.Name = "dgvTagGroupsStatusButton";
            this.dgvTagGroupsStatusButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvTagGroupsStatusButton.Text = "Verify";
            this.dgvTagGroupsStatusButton.ToolTipText = "Verification with Database status.";
            this.dgvTagGroupsStatusButton.Width = 62;
            // 
            // dgvTagGroupsRemoveButton
            // 
            this.dgvTagGroupsRemoveButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsRemoveButton.Frozen = true;
            this.dgvTagGroupsRemoveButton.HeaderText = "Del";
            this.dgvTagGroupsRemoveButton.Name = "dgvTagGroupsRemoveButton";
            this.dgvTagGroupsRemoveButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTagGroupsRemoveButton.ToolTipText = "Remove tag group entirely?";
            this.dgvTagGroupsRemoveButton.Width = 29;
            // 
            // dgvTagGroupsID
            // 
            this.dgvTagGroupsID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsID.HeaderText = "Tag Group ID";
            this.dgvTagGroupsID.Name = "dgvTagGroupsID";
            this.dgvTagGroupsID.ToolTipText = "Identificator of the tag group";
            this.dgvTagGroupsID.Width = 97;
            // 
            // dgvTagGroupsInterrupt
            // 
            this.dgvTagGroupsInterrupt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsInterrupt.HeaderText = "Include";
            this.dgvTagGroupsInterrupt.IndeterminateValue = "True";
            this.dgvTagGroupsInterrupt.Name = "dgvTagGroupsInterrupt";
            this.dgvTagGroupsInterrupt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTagGroupsInterrupt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvTagGroupsInterrupt.ToolTipText = "Should we include this group?";
            this.dgvTagGroupsInterrupt.Width = 67;
            // 
            // dgvTagGroupsExclude
            // 
            this.dgvTagGroupsExclude.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsExclude.HeaderText = "Exclude";
            this.dgvTagGroupsExclude.IndeterminateValue = "False";
            this.dgvTagGroupsExclude.Name = "dgvTagGroupsExclude";
            this.dgvTagGroupsExclude.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTagGroupsExclude.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvTagGroupsExclude.ToolTipText = "Should we exclude this group? Overrides any of include settings.";
            this.dgvTagGroupsExclude.Width = 70;
            // 
            // dgvTagGroupsAliases
            // 
            this.dgvTagGroupsAliases.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTagGroupsAliases.HeaderText = "Aliases";
            this.dgvTagGroupsAliases.Name = "dgvTagGroupsAliases";
            this.dgvTagGroupsAliases.ToolTipText = "Tag aliases, can be used instead of group identificators.";
            this.dgvTagGroupsAliases.Width = 65;
            // 
            // tpSpellsDB
            // 
            this.tpSpellsDB.Controls.Add(this.bClose_Spells);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsAddButton);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsVerifyFirstButton);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsRemoveAllButton);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsDB);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsRemoveOneButton);
            this.tpSpellsDB.Controls.Add(this.dgvSpellsVerifyAllButton);
            this.tpSpellsDB.Location = new System.Drawing.Point(4, 22);
            this.tpSpellsDB.Name = "tpSpellsDB";
            this.tpSpellsDB.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpellsDB.Size = new System.Drawing.Size(626, 425);
            this.tpSpellsDB.TabIndex = 0;
            this.tpSpellsDB.Text = "Database - Spells";
            this.tpSpellsDB.ToolTipText = "Spellcasts database";
            this.tpSpellsDB.UseVisualStyleBackColor = true;
            // 
            // bClose_Spells
            // 
            this.bClose_Spells.Location = new System.Drawing.Point(537, 399);
            this.bClose_Spells.Name = "bClose_Spells";
            this.bClose_Spells.Size = new System.Drawing.Size(83, 21);
            this.bClose_Spells.TabIndex = 6;
            this.bClose_Spells.Text = "Close";
            this.bClose_Spells.UseVisualStyleBackColor = true;
            this.bClose_Spells.Click += new System.EventHandler(this.bClose_Click);
            // 
            // dgvSpellsAddButton
            // 
            this.dgvSpellsAddButton.Location = new System.Drawing.Point(6, 399);
            this.dgvSpellsAddButton.Name = "dgvSpellsAddButton";
            this.dgvSpellsAddButton.Size = new System.Drawing.Size(83, 21);
            this.dgvSpellsAddButton.TabIndex = 5;
            this.dgvSpellsAddButton.Text = "Add";
            this.dgvSpellsAddButton.UseVisualStyleBackColor = true;
            this.dgvSpellsAddButton.Click += new System.EventHandler(this.dgvSpellsAddButton_Click);
            // 
            // dgvSpellsVerifyFirstButton
            // 
            this.dgvSpellsVerifyFirstButton.Location = new System.Drawing.Point(95, 399);
            this.dgvSpellsVerifyFirstButton.Name = "dgvSpellsVerifyFirstButton";
            this.dgvSpellsVerifyFirstButton.Size = new System.Drawing.Size(83, 21);
            this.dgvSpellsVerifyFirstButton.TabIndex = 4;
            this.dgvSpellsVerifyFirstButton.Text = "Verify First";
            this.dgvSpellsVerifyFirstButton.UseVisualStyleBackColor = true;
            this.dgvSpellsVerifyFirstButton.Click += new System.EventHandler(this.dgvSpellsVerifyFirstButton_Click);
            // 
            // dgvSpellsRemoveAllButton
            // 
            this.dgvSpellsRemoveAllButton.Location = new System.Drawing.Point(362, 399);
            this.dgvSpellsRemoveAllButton.Name = "dgvSpellsRemoveAllButton";
            this.dgvSpellsRemoveAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvSpellsRemoveAllButton.TabIndex = 3;
            this.dgvSpellsRemoveAllButton.Text = "Remove All";
            this.dgvSpellsRemoveAllButton.UseVisualStyleBackColor = true;
            this.dgvSpellsRemoveAllButton.Click += new System.EventHandler(this.dgvSpellsRemoveAllButton_Click);
            // 
            // dgvSpellsDB
            // 
            this.dgvSpellsDB.AllowUserToAddRows = false;
            this.dgvSpellsDB.AllowUserToDeleteRows = false;
            this.dgvSpellsDB.AllowUserToOrderColumns = true;
            this.dgvSpellsDB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvSpellsVerifyButton,
            this.dgvSpellsRemoveCB,
            this.dgvSpellsName,
            this.dgvSpellsID,
            this.dgvSpellsInterrupt,
            this.dgvSpellsTags});
            this.dgvSpellsDB.Location = new System.Drawing.Point(3, 3);
            this.dgvSpellsDB.Name = "dgvSpellsDB";
            this.dgvSpellsDB.Size = new System.Drawing.Size(623, 390);
            this.dgvSpellsDB.TabIndex = 0;
            // 
            // dgvSpellsVerifyButton
            // 
            this.dgvSpellsVerifyButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvSpellsVerifyButton.HeaderText = "Status";
            this.dgvSpellsVerifyButton.Name = "dgvSpellsVerifyButton";
            this.dgvSpellsVerifyButton.Text = "Verify";
            this.dgvSpellsVerifyButton.ToolTipText = "Verification with Database status.";
            this.dgvSpellsVerifyButton.Width = 43;
            // 
            // dgvSpellsRemoveCB
            // 
            this.dgvSpellsRemoveCB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvSpellsRemoveCB.HeaderText = "Del";
            this.dgvSpellsRemoveCB.IndeterminateValue = "False";
            this.dgvSpellsRemoveCB.Name = "dgvSpellsRemoveCB";
            this.dgvSpellsRemoveCB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSpellsRemoveCB.ToolTipText = "Remove spell entirely? Mark spell and confirm with one of the bottom buttons.";
            this.dgvSpellsRemoveCB.Width = 29;
            // 
            // dgvSpellsName
            // 
            this.dgvSpellsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvSpellsName.HeaderText = "Spell Name";
            this.dgvSpellsName.Name = "dgvSpellsName";
            this.dgvSpellsName.ReadOnly = true;
            this.dgvSpellsName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSpellsName.ToolTipText = "English name of the spell. Not editable.";
            this.dgvSpellsName.Width = 86;
            // 
            // dgvSpellsID
            // 
            this.dgvSpellsID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvSpellsID.HeaderText = "Spell ID";
            this.dgvSpellsID.Name = "dgvSpellsID";
            this.dgvSpellsID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSpellsID.ToolTipText = "Spell ID, easiest you can get it from Wowhead links, for example in http://www.wo" +
                "whead.com/spell=1449, 1449 is Spell ID";
            this.dgvSpellsID.Width = 69;
            // 
            // dgvSpellsInterrupt
            // 
            this.dgvSpellsInterrupt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvSpellsInterrupt.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvSpellsInterrupt.HeaderText = "Include";
            this.dgvSpellsInterrupt.IndeterminateValue = "";
            this.dgvSpellsInterrupt.Name = "dgvSpellsInterrupt";
            this.dgvSpellsInterrupt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSpellsInterrupt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvSpellsInterrupt.ToolTipText = "Should we include this spell?";
            this.dgvSpellsInterrupt.Width = 67;
            // 
            // dgvSpellsTags
            // 
            this.dgvSpellsTags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvSpellsTags.HeaderText = "Tag Groups attached";
            this.dgvSpellsTags.Name = "dgvSpellsTags";
            this.dgvSpellsTags.ToolTipText = "Tag Group IDs attached to this spell, can also be any of the tag aliases. Seperat" +
                "ed by single comma, no whitespaces!";
            this.dgvSpellsTags.Width = 133;
            // 
            // dgvSpellsRemoveOneButton
            // 
            this.dgvSpellsRemoveOneButton.Location = new System.Drawing.Point(273, 399);
            this.dgvSpellsRemoveOneButton.Name = "dgvSpellsRemoveOneButton";
            this.dgvSpellsRemoveOneButton.Size = new System.Drawing.Size(83, 21);
            this.dgvSpellsRemoveOneButton.TabIndex = 2;
            this.dgvSpellsRemoveOneButton.Text = "Remove One";
            this.dgvSpellsRemoveOneButton.UseVisualStyleBackColor = true;
            this.dgvSpellsRemoveOneButton.Click += new System.EventHandler(this.dgvSpellsRemoveOneButton_Click);
            // 
            // dgvSpellsVerifyAllButton
            // 
            this.dgvSpellsVerifyAllButton.Location = new System.Drawing.Point(184, 399);
            this.dgvSpellsVerifyAllButton.Name = "dgvSpellsVerifyAllButton";
            this.dgvSpellsVerifyAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvSpellsVerifyAllButton.TabIndex = 1;
            this.dgvSpellsVerifyAllButton.Text = "Verify All";
            this.dgvSpellsVerifyAllButton.UseVisualStyleBackColor = true;
            this.dgvSpellsVerifyAllButton.Click += new System.EventHandler(this.dgvSpellsVerifyAllButton_Click);
            // 
            // tpTargets
            // 
            this.tpTargets.Controls.Add(this.bClose_Targets);
            this.tpTargets.Controls.Add(this.dgvTargetsAddButton);
            this.tpTargets.Controls.Add(this.dgvTargetsVerifyFirstButton);
            this.tpTargets.Controls.Add(this.dgvTargetsRemoveAllButton);
            this.tpTargets.Controls.Add(this.dgvTargets);
            this.tpTargets.Controls.Add(this.dgvTargetsRemoveOneButton);
            this.tpTargets.Controls.Add(this.dgvTargetsVerifyAllButton);
            this.tpTargets.Location = new System.Drawing.Point(4, 22);
            this.tpTargets.Name = "tpTargets";
            this.tpTargets.Padding = new System.Windows.Forms.Padding(3);
            this.tpTargets.Size = new System.Drawing.Size(626, 425);
            this.tpTargets.TabIndex = 3;
            this.tpTargets.Text = "Targets";
            this.tpTargets.UseVisualStyleBackColor = true;
            // 
            // bClose_Targets
            // 
            this.bClose_Targets.Location = new System.Drawing.Point(537, 399);
            this.bClose_Targets.Name = "bClose_Targets";
            this.bClose_Targets.Size = new System.Drawing.Size(83, 21);
            this.bClose_Targets.TabIndex = 13;
            this.bClose_Targets.Text = "Close";
            this.bClose_Targets.UseVisualStyleBackColor = true;
            this.bClose_Targets.Click += new System.EventHandler(this.bClose_Click);
            // 
            // dgvTargetsAddButton
            // 
            this.dgvTargetsAddButton.Location = new System.Drawing.Point(6, 399);
            this.dgvTargetsAddButton.Name = "dgvTargetsAddButton";
            this.dgvTargetsAddButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTargetsAddButton.TabIndex = 12;
            this.dgvTargetsAddButton.Text = "Add";
            this.dgvTargetsAddButton.UseVisualStyleBackColor = true;
            this.dgvTargetsAddButton.Click += new System.EventHandler(this.dgvTargetsAddButton_Click);
            // 
            // dgvTargetsVerifyFirstButton
            // 
            this.dgvTargetsVerifyFirstButton.Location = new System.Drawing.Point(95, 399);
            this.dgvTargetsVerifyFirstButton.Name = "dgvTargetsVerifyFirstButton";
            this.dgvTargetsVerifyFirstButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTargetsVerifyFirstButton.TabIndex = 11;
            this.dgvTargetsVerifyFirstButton.Text = "Verify First";
            this.dgvTargetsVerifyFirstButton.UseVisualStyleBackColor = true;
            this.dgvTargetsVerifyFirstButton.Click += new System.EventHandler(this.dgvTargetsVerifyFirstButton_Click);
            // 
            // dgvTargetsRemoveAllButton
            // 
            this.dgvTargetsRemoveAllButton.Location = new System.Drawing.Point(362, 399);
            this.dgvTargetsRemoveAllButton.Name = "dgvTargetsRemoveAllButton";
            this.dgvTargetsRemoveAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTargetsRemoveAllButton.TabIndex = 10;
            this.dgvTargetsRemoveAllButton.Text = "Remove All";
            this.dgvTargetsRemoveAllButton.UseVisualStyleBackColor = true;
            this.dgvTargetsRemoveAllButton.Click += new System.EventHandler(this.dgvTargetsRemoveAllButton_Click);
            // 
            // dgvTargets
            // 
            this.dgvTargets.AllowUserToAddRows = false;
            this.dgvTargets.AllowUserToDeleteRows = false;
            this.dgvTargets.AllowUserToOrderColumns = true;
            this.dgvTargets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTargetsVerifyButton,
            this.dgvTargetsRemoveCB,
            this.dgvTargetsUnitID,
            this.dgvTargetsIncludedCB});
            this.dgvTargets.Location = new System.Drawing.Point(3, 3);
            this.dgvTargets.Name = "dgvTargets";
            this.dgvTargets.Size = new System.Drawing.Size(623, 390);
            this.dgvTargets.TabIndex = 7;
            // 
            // dgvTargetsVerifyButton
            // 
            this.dgvTargetsVerifyButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTargetsVerifyButton.HeaderText = "Status";
            this.dgvTargetsVerifyButton.Name = "dgvTargetsVerifyButton";
            this.dgvTargetsVerifyButton.Text = "Verify";
            this.dgvTargetsVerifyButton.ToolTipText = "Verification with Database status.";
            this.dgvTargetsVerifyButton.Width = 43;
            // 
            // dgvTargetsRemoveCB
            // 
            this.dgvTargetsRemoveCB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTargetsRemoveCB.HeaderText = "Del";
            this.dgvTargetsRemoveCB.IndeterminateValue = "False";
            this.dgvTargetsRemoveCB.Name = "dgvTargetsRemoveCB";
            this.dgvTargetsRemoveCB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTargetsRemoveCB.ToolTipText = "Remove spell entirely? Mark spell and confirm with one of the bottom buttons.";
            this.dgvTargetsRemoveCB.Width = 29;
            // 
            // dgvTargetsUnitID
            // 
            this.dgvTargetsUnitID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvTargetsUnitID.HeaderText = "Unit ID";
            this.dgvTargetsUnitID.Name = "dgvTargetsUnitID";
            this.dgvTargetsUnitID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTargetsUnitID.ToolTipText = "you can find description of unit IDs in http://www.wowpedia.org/UnitId";
            this.dgvTargetsUnitID.Width = 65;
            // 
            // dgvTargetsIncludedCB
            // 
            this.dgvTargetsIncludedCB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvTargetsIncludedCB.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvTargetsIncludedCB.HeaderText = "Include";
            this.dgvTargetsIncludedCB.IndeterminateValue = "";
            this.dgvTargetsIncludedCB.Name = "dgvTargetsIncludedCB";
            this.dgvTargetsIncludedCB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTargetsIncludedCB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvTargetsIncludedCB.ToolTipText = "Should we include this spell?";
            this.dgvTargetsIncludedCB.Width = 67;
            // 
            // dgvTargetsRemoveOneButton
            // 
            this.dgvTargetsRemoveOneButton.Location = new System.Drawing.Point(273, 399);
            this.dgvTargetsRemoveOneButton.Name = "dgvTargetsRemoveOneButton";
            this.dgvTargetsRemoveOneButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTargetsRemoveOneButton.TabIndex = 9;
            this.dgvTargetsRemoveOneButton.Text = "Remove One";
            this.dgvTargetsRemoveOneButton.UseVisualStyleBackColor = true;
            this.dgvTargetsRemoveOneButton.Click += new System.EventHandler(this.dgvTargetsRemoveOneButton_Click);
            // 
            // dgvTargetsVerifyAllButton
            // 
            this.dgvTargetsVerifyAllButton.Location = new System.Drawing.Point(184, 399);
            this.dgvTargetsVerifyAllButton.Name = "dgvTargetsVerifyAllButton";
            this.dgvTargetsVerifyAllButton.Size = new System.Drawing.Size(83, 21);
            this.dgvTargetsVerifyAllButton.TabIndex = 8;
            this.dgvTargetsVerifyAllButton.Text = "Verify All";
            this.dgvTargetsVerifyAllButton.UseVisualStyleBackColor = true;
            this.dgvTargetsVerifyAllButton.Click += new System.EventHandler(this.dgvTargetsVerifyAllButton_Click);
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(17, 19);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(85, 17);
            this.cbDebugMode.TabIndex = 6;
            this.cbDebugMode.Text = "debug mode";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // gbLogging
            // 
            this.gbLogging.Controls.Add(this.cbDebugMode);
            this.gbLogging.Location = new System.Drawing.Point(8, 192);
            this.gbLogging.Name = "gbLogging";
            this.gbLogging.Size = new System.Drawing.Size(198, 47);
            this.gbLogging.TabIndex = 17;
            this.gbLogging.TabStop = false;
            this.gbLogging.Text = "Logging";
            // 
            // GUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(634, 452);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GUI";
            this.Text = "Interrupter Ultimate";
            this.tabControl1.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.gbMiscSettings.ResumeLayout(false);
            this.gbMiscSettings.PerformLayout();
            this.gbInterruptWhen.ResumeLayout(false);
            this.gbInterruptWhen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannelMillisecondsElapsed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCastMilliSecondsLeft)).EndInit();
            this.tpTagGroupsDB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagGroupsDB)).EndInit();
            this.tpSpellsDB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpellsDB)).EndInit();
            this.tpTargets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTargets)).EndInit();
            this.gbLogging.ResumeLayout(false);
            this.gbLogging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSpellsDB;
        private System.Windows.Forms.TabPage tpTagGroupsDB;
        private System.Windows.Forms.TabPage tpSettings;
        public System.Windows.Forms.DataGridView dgvSpellsDB;
        public System.Windows.Forms.DataGridView dgvTagGroupsDB;
        private System.Windows.Forms.Button dgvSpellsVerifyAllButton;
        private System.Windows.Forms.Button dgvSpellsRemoveOneButton;
        private System.Windows.Forms.Button dgvSpellsRemoveAllButton;
        private System.Windows.Forms.Button dgvSpellsVerifyFirstButton;
        private System.Windows.Forms.Button dgvSpellsAddButton;
        private System.Windows.Forms.Button dgvTagGroupsAddButton;
        private System.Windows.Forms.Button dgvTagGroupsVerifyFirstButton;
        private System.Windows.Forms.Button dgvTagGroupsRemoveAllButton;
        private System.Windows.Forms.Button dgvTagGroupsRemoveOneButton;
        private System.Windows.Forms.Button dgvTagGroupsVerifyAllButton;
        private System.Windows.Forms.CheckBox cbInterruptAll;
        private System.Windows.Forms.CheckBox cbShouldForceCast;
        private System.Windows.Forms.Label lChannelMillisecondsElapsed;
        private System.Windows.Forms.Label lCastMillisecondsLeft;
        private System.Windows.Forms.NumericUpDown nudCastMilliSecondsLeft;
        private System.Windows.Forms.NumericUpDown nudChannelMillisecondsElapsed;
        private System.Windows.Forms.GroupBox gbInterruptWhen;
        private System.Windows.Forms.GroupBox gbMiscSettings;
        private System.Windows.Forms.Button bClose_Spells;
        private System.Windows.Forms.Button bSaveSettings;
        private System.Windows.Forms.Button bClose_TagGroups;
        private System.Windows.Forms.Button bClose_Settings;
        private System.Windows.Forms.Button bReloadSettings;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSpellsVerifyButton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvSpellsRemoveCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSpellsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSpellsID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvSpellsInterrupt;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSpellsTags;
        private System.Windows.Forms.DataGridViewButtonColumn dgvTagGroupsStatusButton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvTagGroupsRemoveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTagGroupsID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvTagGroupsInterrupt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvTagGroupsExclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTagGroupsAliases;
        private System.Windows.Forms.TabPage tpTargets;
        private System.Windows.Forms.Button bClose_Targets;
        private System.Windows.Forms.Button dgvTargetsAddButton;
        private System.Windows.Forms.Button dgvTargetsVerifyFirstButton;
        private System.Windows.Forms.Button dgvTargetsRemoveAllButton;
        public System.Windows.Forms.DataGridView dgvTargets;
        private System.Windows.Forms.DataGridViewButtonColumn dgvTargetsVerifyButton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvTargetsRemoveCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTargetsUnitID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvTargetsIncludedCB;
        private System.Windows.Forms.Button dgvTargetsRemoveOneButton;
        private System.Windows.Forms.Button dgvTargetsVerifyAllButton;
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.GroupBox gbLogging;
    }
}