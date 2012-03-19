using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Altarboy
{
    partial class AltarboyConfig : Form
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
            this.components = new System.ComponentModel.Container();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.mastHead = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.altarboySettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Advanced = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.chkMindSpikeWithFiend = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtCooldownDuration = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpellClipping = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnAddMobToList = new System.Windows.Forms.Button();
            this.mobGrid = new System.Windows.Forms.DataGridView();
            this.MobID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MobName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MobFocus = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IgnoreMob = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MindSpikeMob = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MindSearMob = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chkShackle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CooldownsTab = new System.Windows.Forms.TabPage();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.cmbPotionNames = new System.Windows.Forms.ComboBox();
            this.chkUsePotionsOnLust = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.chkTinkerGlovesWithArchangel = new System.Windows.Forms.CheckBox();
            this.chkUseGloves = new System.Windows.Forms.CheckBox();
            this.chkUseBelt = new System.Windows.Forms.CheckBox();
            this.Trinket2Label = new System.Windows.Forms.GroupBox();
            this.radTrinket2NeverUse = new System.Windows.Forms.RadioButton();
            this.radTrinket2UseWithArchangel = new System.Windows.Forms.RadioButton();
            this.radTrinket2UseOnCD = new System.Windows.Forms.RadioButton();
            this.Trinket1Label = new System.Windows.Forms.GroupBox();
            this.radTrinket1NeverUse = new System.Windows.Forms.RadioButton();
            this.radTrinket1UseWithArchangel = new System.Windows.Forms.RadioButton();
            this.radTrinket1UseOnCD = new System.Windows.Forms.RadioButton();
            this.CombatTab = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.chkClipMindFlay = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAllowMindSear = new System.Windows.Forms.CheckBox();
            this.numMindSearCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkUseDispersionForManaRegen = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numDispersionManaPercent = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkSWDWhileMoving = new System.Windows.Forms.CheckBox();
            this.chkShadowWordDeathCheckDebuffs = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShadowWordDeathExecute = new System.Windows.Forms.CheckBox();
            this.radShadowWordDeathNever = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.numShadowWordDeathManaPercent = new System.Windows.Forms.NumericUpDown();
            this.radShadowWordDeathManaRegen = new System.Windows.Forms.RadioButton();
            this.radShadowWordDeathOnCD = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUsefiendOnArchAngel = new System.Windows.Forms.CheckBox();
            this.chkForceFiendOnBloodLust = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numShadowFiendManaPercent = new System.Windows.Forms.NumericUpDown();
            this.radUseShadowFiendForManaRegen = new System.Windows.Forms.RadioButton();
            this.radUseShadowFiendOnCD = new System.Windows.Forms.RadioButton();
            this.BuffTab = new System.Windows.Forms.TabPage();
            this.chkAutoShield = new System.Windows.Forms.CheckBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.chkEatAndDrinkWhileResting = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.chkLevitateWhileFalling = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.chkBuffShadowProtection = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.chkFadeOnAggro = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkVampiricEmbrace = new System.Windows.Forms.CheckBox();
            this.chkForceShadowFormInCombat = new System.Windows.Forms.CheckBox();
            this.chkUseShadowform = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkUseFortitude = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkInnerWillWhileResting = new System.Windows.Forms.CheckBox();
            this.radInnerFire = new System.Windows.Forms.RadioButton();
            this.radInnerWill = new System.Windows.Forms.RadioButton();
            this.TargetingTab = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkRafLeadertTargeting = new System.Windows.Forms.CheckBox();
            this.chkEnableMultiDotTargeting = new System.Windows.Forms.CheckBox();
            this.chkEnableMindSearTargeting = new System.Windows.Forms.CheckBox();
            this.chkEnableTargeting = new System.Windows.Forms.CheckBox();
            this.SettingsTabs = new System.Windows.Forms.TabControl();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.chkUseMindBlastOnCD = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.mastHead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altarboySettingsBindingSource)).BeginInit();
            this.Advanced.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mobGrid)).BeginInit();
            this.CooldownsTab.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.Trinket2Label.SuspendLayout();
            this.Trinket1Label.SuspendLayout();
            this.CombatTab.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMindSearCount)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDispersionManaPercent)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShadowWordDeathManaPercent)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShadowFiendManaPercent)).BeginInit();
            this.BuffTab.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TargetingTab.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SettingsTabs.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(405, 429);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Location = new System.Drawing.Point(477, 429);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(97, 23);
            this.btnSaveAndClose.TabIndex = 5;
            this.btnSaveAndClose.Text = "Save and Close";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // mastHead
            // 
            this.mastHead.Location = new System.Drawing.Point(1, 0);
            this.mastHead.Name = "mastHead";
            this.mastHead.Size = new System.Drawing.Size(577, 90);
            this.mastHead.TabIndex = 6;
            this.mastHead.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(85, 74);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(328, 13);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version 1.0 - Created and maintained by Spriestdev and Altarboydev";
            // 
            // Advanced
            // 
            this.Advanced.Controls.Add(this.groupBox17);
            this.Advanced.Controls.Add(this.groupBox7);
            this.Advanced.Location = new System.Drawing.Point(4, 22);
            this.Advanced.Name = "Advanced";
            this.Advanced.Padding = new System.Windows.Forms.Padding(3);
            this.Advanced.Size = new System.Drawing.Size(573, 306);
            this.Advanced.TabIndex = 2;
            this.Advanced.Text = "Advanced";
            this.Advanced.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.chkMindSpikeWithFiend);
            this.groupBox17.Location = new System.Drawing.Point(14, 132);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(549, 103);
            this.groupBox17.TabIndex = 10;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Tier Considerations";
            // 
            // chkMindSpikeWithFiend
            // 
            this.chkMindSpikeWithFiend.AutoSize = true;
            this.chkMindSpikeWithFiend.Location = new System.Drawing.Point(6, 19);
            this.chkMindSpikeWithFiend.Name = "chkMindSpikeWithFiend";
            this.chkMindSpikeWithFiend.Size = new System.Drawing.Size(278, 17);
            this.chkMindSpikeWithFiend.TabIndex = 10;
            this.chkMindSpikeWithFiend.Text = "Use Mindspike Rotation with ShadowFiend (T13 4pc)";
            this.chkMindSpikeWithFiend.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtCooldownDuration);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.txtSpellClipping);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Location = new System.Drawing.Point(14, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(549, 103);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Spell Clipping";
            // 
            // txtCooldownDuration
            // 
            this.txtCooldownDuration.Location = new System.Drawing.Point(6, 48);
            this.txtCooldownDuration.Name = "txtCooldownDuration";
            this.txtCooldownDuration.Size = new System.Drawing.Size(53, 20);
            this.txtCooldownDuration.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Cooldown Duration (Decimal)";
            // 
            // txtSpellClipping
            // 
            this.txtSpellClipping.Location = new System.Drawing.Point(6, 22);
            this.txtSpellClipping.Name = "txtSpellClipping";
            this.txtSpellClipping.Size = new System.Drawing.Size(53, 20);
            this.txtSpellClipping.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Spell Clip Duration (Decimal)";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRemoveSelected);
            this.tabPage1.Controls.Add(this.btnAddMobToList);
            this.tabPage1.Controls.Add(this.mobGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(573, 306);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Special Targets";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(325, 277);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(119, 23);
            this.btnRemoveSelected.TabIndex = 4;
            this.btnRemoveSelected.Text = "Delete Selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnAddMobToList
            // 
            this.btnAddMobToList.Location = new System.Drawing.Point(450, 277);
            this.btnAddMobToList.Name = "btnAddMobToList";
            this.btnAddMobToList.Size = new System.Drawing.Size(119, 23);
            this.btnAddMobToList.TabIndex = 3;
            this.btnAddMobToList.Text = "Add Current Target ";
            this.btnAddMobToList.UseVisualStyleBackColor = true;
            this.btnAddMobToList.Click += new System.EventHandler(this.btnAddMobToList_Click);
            // 
            // mobGrid
            // 
            this.mobGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mobGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MobID,
            this.MobName,
            this.MobFocus,
            this.IgnoreMob,
            this.MindSpikeMob,
            this.MindSearMob,
            this.chkShackle});
            this.mobGrid.GridColor = System.Drawing.SystemColors.Window;
            this.mobGrid.Location = new System.Drawing.Point(0, 6);
            this.mobGrid.MultiSelect = false;
            this.mobGrid.Name = "mobGrid";
            this.mobGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mobGrid.Size = new System.Drawing.Size(569, 265);
            this.mobGrid.TabIndex = 2;
            this.mobGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mobGrid_CellValueChanged);
            this.mobGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.mobGrid_CurrentCellDirtyStateChanged);
            // 
            // MobID
            // 
            this.MobID.DataPropertyName = "TargetId";
            this.MobID.HeaderText = "Mob ID";
            this.MobID.Name = "MobID";
            this.MobID.Width = 70;
            // 
            // MobName
            // 
            this.MobName.DataPropertyName = "TargetName";
            this.MobName.HeaderText = "Mob Name";
            this.MobName.Name = "MobName";
            this.MobName.Width = 160;
            // 
            // MobFocus
            // 
            this.MobFocus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MobFocus.DataPropertyName = "Focus";
            this.MobFocus.HeaderText = "Focus";
            this.MobFocus.Name = "MobFocus";
            this.MobFocus.Width = 42;
            // 
            // IgnoreMob
            // 
            this.IgnoreMob.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.IgnoreMob.DataPropertyName = "Ignore";
            this.IgnoreMob.HeaderText = "Ignore";
            this.IgnoreMob.Name = "IgnoreMob";
            this.IgnoreMob.Width = 43;
            // 
            // MindSpikeMob
            // 
            this.MindSpikeMob.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MindSpikeMob.DataPropertyName = "MindSpike";
            this.MindSpikeMob.HeaderText = "Mind Spike";
            this.MindSpikeMob.Name = "MindSpikeMob";
            this.MindSpikeMob.Width = 66;
            // 
            // MindSearMob
            // 
            this.MindSearMob.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MindSearMob.DataPropertyName = "MindSear";
            this.MindSearMob.HeaderText = "Mind Sear";
            this.MindSearMob.Name = "MindSearMob";
            this.MindSearMob.Width = 61;
            // 
            // chkShackle
            // 
            this.chkShackle.DataPropertyName = "Shackle";
            this.chkShackle.HeaderText = "Shackle";
            this.chkShackle.Name = "chkShackle";
            this.chkShackle.Width = 50;
            // 
            // CooldownsTab
            // 
            this.CooldownsTab.Controls.Add(this.groupBox16);
            this.CooldownsTab.Controls.Add(this.groupBox8);
            this.CooldownsTab.Controls.Add(this.Trinket2Label);
            this.CooldownsTab.Controls.Add(this.Trinket1Label);
            this.CooldownsTab.Location = new System.Drawing.Point(4, 22);
            this.CooldownsTab.Name = "CooldownsTab";
            this.CooldownsTab.Padding = new System.Windows.Forms.Padding(3);
            this.CooldownsTab.Size = new System.Drawing.Size(573, 306);
            this.CooldownsTab.TabIndex = 4;
            this.CooldownsTab.Text = "Cooldowns";
            this.CooldownsTab.UseVisualStyleBackColor = true;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.cmbPotionNames);
            this.groupBox16.Controls.Add(this.chkUsePotionsOnLust);
            this.groupBox16.Location = new System.Drawing.Point(287, 101);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(272, 84);
            this.groupBox16.TabIndex = 13;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Potions";
            // 
            // cmbPotionNames
            // 
            this.cmbPotionNames.FormattingEnabled = true;
            this.cmbPotionNames.Location = new System.Drawing.Point(7, 45);
            this.cmbPotionNames.Name = "cmbPotionNames";
            this.cmbPotionNames.Size = new System.Drawing.Size(257, 21);
            this.cmbPotionNames.TabIndex = 13;
            // 
            // chkUsePotionsOnLust
            // 
            this.chkUsePotionsOnLust.AutoSize = true;
            this.chkUsePotionsOnLust.Location = new System.Drawing.Point(7, 24);
            this.chkUsePotionsOnLust.Name = "chkUsePotionsOnLust";
            this.chkUsePotionsOnLust.Size = new System.Drawing.Size(207, 17);
            this.chkUsePotionsOnLust.TabIndex = 12;
            this.chkUsePotionsOnLust.Text = "Use Potions during Bloodlust/Heroism.";
            this.chkUsePotionsOnLust.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkTinkerGlovesWithArchangel);
            this.groupBox8.Controls.Add(this.chkUseGloves);
            this.groupBox8.Controls.Add(this.chkUseBelt);
            this.groupBox8.Location = new System.Drawing.Point(287, 12);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(272, 79);
            this.groupBox8.TabIndex = 11;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Engineering";
            // 
            // chkTinkerGlovesWithArchangel
            // 
            this.chkTinkerGlovesWithArchangel.AutoSize = true;
            this.chkTinkerGlovesWithArchangel.Location = new System.Drawing.Point(146, 45);
            this.chkTinkerGlovesWithArchangel.Name = "chkTinkerGlovesWithArchangel";
            this.chkTinkerGlovesWithArchangel.Size = new System.Drawing.Size(118, 17);
            this.chkTinkerGlovesWithArchangel.TabIndex = 11;
            this.chkTinkerGlovesWithArchangel.Text = "Use with Archangel";
            this.chkTinkerGlovesWithArchangel.UseVisualStyleBackColor = true;
            // 
            // chkUseGloves
            // 
            this.chkUseGloves.AutoSize = true;
            this.chkUseGloves.Location = new System.Drawing.Point(10, 45);
            this.chkUseGloves.Name = "chkUseGloves";
            this.chkUseGloves.Size = new System.Drawing.Size(129, 17);
            this.chkUseGloves.TabIndex = 10;
            this.chkUseGloves.Text = "Use Tinker on Gloves";
            this.chkUseGloves.UseVisualStyleBackColor = true;
            // 
            // chkUseBelt
            // 
            this.chkUseBelt.AutoSize = true;
            this.chkUseBelt.Location = new System.Drawing.Point(10, 22);
            this.chkUseBelt.Name = "chkUseBelt";
            this.chkUseBelt.Size = new System.Drawing.Size(114, 17);
            this.chkUseBelt.TabIndex = 9;
            this.chkUseBelt.Text = "Use Tinker on Belt";
            this.chkUseBelt.UseVisualStyleBackColor = true;
            // 
            // Trinket2Label
            // 
            this.Trinket2Label.Controls.Add(this.radTrinket2NeverUse);
            this.Trinket2Label.Controls.Add(this.radTrinket2UseWithArchangel);
            this.Trinket2Label.Controls.Add(this.radTrinket2UseOnCD);
            this.Trinket2Label.Location = new System.Drawing.Point(13, 118);
            this.Trinket2Label.Name = "Trinket2Label";
            this.Trinket2Label.Size = new System.Drawing.Size(258, 97);
            this.Trinket2Label.TabIndex = 12;
            this.Trinket2Label.TabStop = false;
            this.Trinket2Label.Text = "Trinket2";
            // 
            // radTrinket2NeverUse
            // 
            this.radTrinket2NeverUse.AutoSize = true;
            this.radTrinket2NeverUse.Location = new System.Drawing.Point(15, 23);
            this.radTrinket2NeverUse.Name = "radTrinket2NeverUse";
            this.radTrinket2NeverUse.Size = new System.Drawing.Size(76, 17);
            this.radTrinket2NeverUse.TabIndex = 8;
            this.radTrinket2NeverUse.TabStop = true;
            this.radTrinket2NeverUse.Text = "Never Use";
            this.radTrinket2NeverUse.UseVisualStyleBackColor = true;
            // 
            // radTrinket2UseWithArchangel
            // 
            this.radTrinket2UseWithArchangel.AutoSize = true;
            this.radTrinket2UseWithArchangel.Location = new System.Drawing.Point(15, 67);
            this.radTrinket2UseWithArchangel.Name = "radTrinket2UseWithArchangel";
            this.radTrinket2UseWithArchangel.Size = new System.Drawing.Size(117, 17);
            this.radTrinket2UseWithArchangel.TabIndex = 7;
            this.radTrinket2UseWithArchangel.TabStop = true;
            this.radTrinket2UseWithArchangel.Text = "Use with Archangel";
            this.radTrinket2UseWithArchangel.UseVisualStyleBackColor = true;
            // 
            // radTrinket2UseOnCD
            // 
            this.radTrinket2UseOnCD.AutoSize = true;
            this.radTrinket2UseOnCD.Location = new System.Drawing.Point(15, 45);
            this.radTrinket2UseOnCD.Name = "radTrinket2UseOnCD";
            this.radTrinket2UseOnCD.Size = new System.Drawing.Size(77, 17);
            this.radTrinket2UseOnCD.TabIndex = 6;
            this.radTrinket2UseOnCD.TabStop = true;
            this.radTrinket2UseOnCD.Text = "Use on CD";
            this.radTrinket2UseOnCD.UseVisualStyleBackColor = true;
            // 
            // Trinket1Label
            // 
            this.Trinket1Label.Controls.Add(this.radTrinket1NeverUse);
            this.Trinket1Label.Controls.Add(this.radTrinket1UseWithArchangel);
            this.Trinket1Label.Controls.Add(this.radTrinket1UseOnCD);
            this.Trinket1Label.Location = new System.Drawing.Point(12, 12);
            this.Trinket1Label.Name = "Trinket1Label";
            this.Trinket1Label.Size = new System.Drawing.Size(258, 97);
            this.Trinket1Label.TabIndex = 10;
            this.Trinket1Label.TabStop = false;
            this.Trinket1Label.Text = "Trinket 1";
            // 
            // radTrinket1NeverUse
            // 
            this.radTrinket1NeverUse.AutoSize = true;
            this.radTrinket1NeverUse.Location = new System.Drawing.Point(15, 23);
            this.radTrinket1NeverUse.Name = "radTrinket1NeverUse";
            this.radTrinket1NeverUse.Size = new System.Drawing.Size(76, 17);
            this.radTrinket1NeverUse.TabIndex = 8;
            this.radTrinket1NeverUse.TabStop = true;
            this.radTrinket1NeverUse.Text = "Never Use";
            this.radTrinket1NeverUse.UseVisualStyleBackColor = true;
            // 
            // radTrinket1UseWithArchangel
            // 
            this.radTrinket1UseWithArchangel.AutoSize = true;
            this.radTrinket1UseWithArchangel.Location = new System.Drawing.Point(15, 67);
            this.radTrinket1UseWithArchangel.Name = "radTrinket1UseWithArchangel";
            this.radTrinket1UseWithArchangel.Size = new System.Drawing.Size(117, 17);
            this.radTrinket1UseWithArchangel.TabIndex = 7;
            this.radTrinket1UseWithArchangel.TabStop = true;
            this.radTrinket1UseWithArchangel.Text = "Use with Archangel";
            this.radTrinket1UseWithArchangel.UseVisualStyleBackColor = true;
            // 
            // radTrinket1UseOnCD
            // 
            this.radTrinket1UseOnCD.AutoSize = true;
            this.radTrinket1UseOnCD.Location = new System.Drawing.Point(15, 45);
            this.radTrinket1UseOnCD.Name = "radTrinket1UseOnCD";
            this.radTrinket1UseOnCD.Size = new System.Drawing.Size(77, 17);
            this.radTrinket1UseOnCD.TabIndex = 6;
            this.radTrinket1UseOnCD.TabStop = true;
            this.radTrinket1UseOnCD.Text = "Use on CD";
            this.radTrinket1UseOnCD.UseVisualStyleBackColor = true;
            // 
            // CombatTab
            // 
            this.CombatTab.Controls.Add(this.groupBox18);
            this.CombatTab.Controls.Add(this.groupBox15);
            this.CombatTab.Controls.Add(this.groupBox9);
            this.CombatTab.Controls.Add(this.groupBox6);
            this.CombatTab.Controls.Add(this.groupBox5);
            this.CombatTab.Controls.Add(this.groupBox4);
            this.CombatTab.Location = new System.Drawing.Point(4, 22);
            this.CombatTab.Name = "CombatTab";
            this.CombatTab.Padding = new System.Windows.Forms.Padding(3);
            this.CombatTab.Size = new System.Drawing.Size(573, 306);
            this.CombatTab.TabIndex = 1;
            this.CombatTab.Text = "Combat Setings";
            this.CombatTab.UseVisualStyleBackColor = true;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.chkClipMindFlay);
            this.groupBox15.Location = new System.Drawing.Point(13, 251);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(258, 49);
            this.groupBox15.TabIndex = 10;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Mind Flay";
            // 
            // chkClipMindFlay
            // 
            this.chkClipMindFlay.AutoSize = true;
            this.chkClipMindFlay.Location = new System.Drawing.Point(11, 21);
            this.chkClipMindFlay.Name = "chkClipMindFlay";
            this.chkClipMindFlay.Size = new System.Drawing.Size(91, 17);
            this.chkClipMindFlay.TabIndex = 8;
            this.chkClipMindFlay.Text = "Clip Mind Flay";
            this.chkClipMindFlay.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label6);
            this.groupBox9.Controls.Add(this.chkAllowMindSear);
            this.groupBox9.Controls.Add(this.numMindSearCount);
            this.groupBox9.Location = new System.Drawing.Point(13, 147);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(258, 85);
            this.groupBox9.TabIndex = 9;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Mind Sear";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Mob count to use Mind Sear";
            // 
            // chkAllowMindSear
            // 
            this.chkAllowMindSear.AutoSize = true;
            this.chkAllowMindSear.Location = new System.Drawing.Point(11, 21);
            this.chkAllowMindSear.Name = "chkAllowMindSear";
            this.chkAllowMindSear.Size = new System.Drawing.Size(96, 17);
            this.chkAllowMindSear.TabIndex = 8;
            this.chkAllowMindSear.Text = "Use Mind Sear";
            this.chkAllowMindSear.UseVisualStyleBackColor = true;
            // 
            // numMindSearCount
            // 
            this.numMindSearCount.Location = new System.Drawing.Point(11, 44);
            this.numMindSearCount.Name = "numMindSearCount";
            this.numMindSearCount.Size = new System.Drawing.Size(40, 20);
            this.numMindSearCount.TabIndex = 2;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkUseDispersionForManaRegen);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.numDispersionManaPercent);
            this.groupBox6.Location = new System.Drawing.Point(289, 193);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(258, 58);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Dispersion";
            // 
            // chkUseDispersionForManaRegen
            // 
            this.chkUseDispersionForManaRegen.AutoSize = true;
            this.chkUseDispersionForManaRegen.Location = new System.Drawing.Point(11, 21);
            this.chkUseDispersionForManaRegen.Name = "chkUseDispersionForManaRegen";
            this.chkUseDispersionForManaRegen.Size = new System.Drawing.Size(171, 17);
            this.chkUseDispersionForManaRegen.TabIndex = 8;
            this.chkUseDispersionForManaRegen.Text = "Use Dispersion for mana regen";
            this.chkUseDispersionForManaRegen.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "%";
            // 
            // numDispersionManaPercent
            // 
            this.numDispersionManaPercent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numDispersionManaPercent.Location = new System.Drawing.Point(189, 19);
            this.numDispersionManaPercent.Name = "numDispersionManaPercent";
            this.numDispersionManaPercent.Size = new System.Drawing.Size(40, 20);
            this.numDispersionManaPercent.TabIndex = 2;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkSWDWhileMoving);
            this.groupBox5.Controls.Add(this.chkShadowWordDeathCheckDebuffs);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.chkShadowWordDeathExecute);
            this.groupBox5.Controls.Add(this.radShadowWordDeathNever);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.numShadowWordDeathManaPercent);
            this.groupBox5.Controls.Add(this.radShadowWordDeathManaRegen);
            this.groupBox5.Controls.Add(this.radShadowWordDeathOnCD);
            this.groupBox5.Location = new System.Drawing.Point(289, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(258, 174);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Shadow Word: Death Settings";
            // 
            // chkSWDWhileMoving
            // 
            this.chkSWDWhileMoving.AutoSize = true;
            this.chkSWDWhileMoving.Location = new System.Drawing.Point(10, 148);
            this.chkSWDWhileMoving.Name = "chkSWDWhileMoving";
            this.chkSWDWhileMoving.Size = new System.Drawing.Size(176, 17);
            this.chkSWDWhileMoving.TabIndex = 8;
            this.chkSWDWhileMoving.Text = "Cast While Moving (DPS Boost)";
            this.chkSWDWhileMoving.UseVisualStyleBackColor = true;
            // 
            // chkShadowWordDeathCheckDebuffs
            // 
            this.chkShadowWordDeathCheckDebuffs.AutoSize = true;
            this.chkShadowWordDeathCheckDebuffs.Location = new System.Drawing.Point(10, 123);
            this.chkShadowWordDeathCheckDebuffs.Name = "chkShadowWordDeathCheckDebuffs";
            this.chkShadowWordDeathCheckDebuffs.Size = new System.Drawing.Size(131, 17);
            this.chkShadowWordDeathCheckDebuffs.TabIndex = 7;
            this.chkShadowWordDeathCheckDebuffs.Text = "Check for Debuffs first";
            this.chkShadowWordDeathCheckDebuffs.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(27, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "(Overrides other settings)";
            // 
            // chkShadowWordDeathExecute
            // 
            this.chkShadowWordDeathExecute.AutoSize = true;
            this.chkShadowWordDeathExecute.Location = new System.Drawing.Point(10, 88);
            this.chkShadowWordDeathExecute.Name = "chkShadowWordDeathExecute";
            this.chkShadowWordDeathExecute.Size = new System.Drawing.Size(158, 17);
            this.chkShadowWordDeathExecute.TabIndex = 5;
            this.chkShadowWordDeathExecute.Text = "Use when target below 25%";
            this.chkShadowWordDeathExecute.UseVisualStyleBackColor = true;
            // 
            // radShadowWordDeathNever
            // 
            this.radShadowWordDeathNever.AutoSize = true;
            this.radShadowWordDeathNever.Location = new System.Drawing.Point(10, 19);
            this.radShadowWordDeathNever.Name = "radShadowWordDeathNever";
            this.radShadowWordDeathNever.Size = new System.Drawing.Size(76, 17);
            this.radShadowWordDeathNever.TabIndex = 4;
            this.radShadowWordDeathNever.TabStop = true;
            this.radShadowWordDeathNever.Text = "Never Use";
            this.radShadowWordDeathNever.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "%";
            // 
            // numShadowWordDeathManaPercent
            // 
            this.numShadowWordDeathManaPercent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numShadowWordDeathManaPercent.Location = new System.Drawing.Point(189, 63);
            this.numShadowWordDeathManaPercent.Name = "numShadowWordDeathManaPercent";
            this.numShadowWordDeathManaPercent.Size = new System.Drawing.Size(40, 20);
            this.numShadowWordDeathManaPercent.TabIndex = 2;
            // 
            // radShadowWordDeathManaRegen
            // 
            this.radShadowWordDeathManaRegen.AutoSize = true;
            this.radShadowWordDeathManaRegen.Location = new System.Drawing.Point(10, 63);
            this.radShadowWordDeathManaRegen.Name = "radShadowWordDeathManaRegen";
            this.radShadowWordDeathManaRegen.Size = new System.Drawing.Size(172, 17);
            this.radShadowWordDeathManaRegen.TabIndex = 1;
            this.radShadowWordDeathManaRegen.TabStop = true;
            this.radShadowWordDeathManaRegen.Text = "Use for mana regeneration only";
            this.radShadowWordDeathManaRegen.UseVisualStyleBackColor = true;
            // 
            // radShadowWordDeathOnCD
            // 
            this.radShadowWordDeathOnCD.AutoSize = true;
            this.radShadowWordDeathOnCD.Location = new System.Drawing.Point(10, 41);
            this.radShadowWordDeathOnCD.Name = "radShadowWordDeathOnCD";
            this.radShadowWordDeathOnCD.Size = new System.Drawing.Size(77, 17);
            this.radShadowWordDeathOnCD.TabIndex = 0;
            this.radShadowWordDeathOnCD.TabStop = true;
            this.radShadowWordDeathOnCD.Text = "Use on CD";
            this.radShadowWordDeathOnCD.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkUsefiendOnArchAngel);
            this.groupBox4.Controls.Add(this.chkForceFiendOnBloodLust);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.numShadowFiendManaPercent);
            this.groupBox4.Controls.Add(this.radUseShadowFiendForManaRegen);
            this.groupBox4.Controls.Add(this.radUseShadowFiendOnCD);
            this.groupBox4.Location = new System.Drawing.Point(13, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(258, 126);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Shadow Fiend Settings";
            // 
            // chkUsefiendOnArchAngel
            // 
            this.chkUsefiendOnArchAngel.AutoSize = true;
            this.chkUsefiendOnArchAngel.Location = new System.Drawing.Point(10, 92);
            this.chkUsefiendOnArchAngel.Name = "chkUsefiendOnArchAngel";
            this.chkUsefiendOnArchAngel.Size = new System.Drawing.Size(145, 17);
            this.chkUsefiendOnArchAngel.TabIndex = 10;
            this.chkUsefiendOnArchAngel.Text = "Force fiend on Archangel";
            this.chkUsefiendOnArchAngel.UseVisualStyleBackColor = true;
            // 
            // chkForceFiendOnBloodLust
            // 
            this.chkForceFiendOnBloodLust.AutoSize = true;
            this.chkForceFiendOnBloodLust.Location = new System.Drawing.Point(10, 69);
            this.chkForceFiendOnBloodLust.Name = "chkForceFiendOnBloodLust";
            this.chkForceFiendOnBloodLust.Size = new System.Drawing.Size(140, 17);
            this.chkForceFiendOnBloodLust.TabIndex = 9;
            this.chkForceFiendOnBloodLust.Text = "Force fiend on Bloodlust";
            this.chkForceFiendOnBloodLust.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "%";
            // 
            // numShadowFiendManaPercent
            // 
            this.numShadowFiendManaPercent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numShadowFiendManaPercent.Location = new System.Drawing.Point(189, 43);
            this.numShadowFiendManaPercent.Name = "numShadowFiendManaPercent";
            this.numShadowFiendManaPercent.Size = new System.Drawing.Size(40, 20);
            this.numShadowFiendManaPercent.TabIndex = 2;
            // 
            // radUseShadowFiendForManaRegen
            // 
            this.radUseShadowFiendForManaRegen.AutoSize = true;
            this.radUseShadowFiendForManaRegen.Location = new System.Drawing.Point(10, 43);
            this.radUseShadowFiendForManaRegen.Name = "radUseShadowFiendForManaRegen";
            this.radUseShadowFiendForManaRegen.Size = new System.Drawing.Size(172, 17);
            this.radUseShadowFiendForManaRegen.TabIndex = 1;
            this.radUseShadowFiendForManaRegen.TabStop = true;
            this.radUseShadowFiendForManaRegen.Text = "Use for mana regeneration only";
            this.radUseShadowFiendForManaRegen.UseVisualStyleBackColor = true;
            // 
            // radUseShadowFiendOnCD
            // 
            this.radUseShadowFiendOnCD.AutoSize = true;
            this.radUseShadowFiendOnCD.Location = new System.Drawing.Point(10, 20);
            this.radUseShadowFiendOnCD.Name = "radUseShadowFiendOnCD";
            this.radUseShadowFiendOnCD.Size = new System.Drawing.Size(77, 17);
            this.radUseShadowFiendOnCD.TabIndex = 0;
            this.radUseShadowFiendOnCD.TabStop = true;
            this.radUseShadowFiendOnCD.Text = "Use on CD";
            this.radUseShadowFiendOnCD.UseVisualStyleBackColor = true;
            // 
            // BuffTab
            // 
            this.BuffTab.Controls.Add(this.chkAutoShield);
            this.BuffTab.Controls.Add(this.groupBox14);
            this.BuffTab.Controls.Add(this.groupBox13);
            this.BuffTab.Controls.Add(this.groupBox12);
            this.BuffTab.Controls.Add(this.groupBox11);
            this.BuffTab.Controls.Add(this.groupBox3);
            this.BuffTab.Controls.Add(this.groupBox2);
            this.BuffTab.Controls.Add(this.groupBox1);
            this.BuffTab.Location = new System.Drawing.Point(4, 22);
            this.BuffTab.Name = "BuffTab";
            this.BuffTab.Padding = new System.Windows.Forms.Padding(3);
            this.BuffTab.Size = new System.Drawing.Size(573, 306);
            this.BuffTab.TabIndex = 0;
            this.BuffTab.Text = "Buff Settings";
            this.BuffTab.UseVisualStyleBackColor = true;
            // 
            // chkAutoShield
            // 
            this.chkAutoShield.AutoSize = true;
            this.chkAutoShield.Location = new System.Drawing.Point(303, 250);
            this.chkAutoShield.Name = "chkAutoShield";
            this.chkAutoShield.Size = new System.Drawing.Size(96, 17);
            this.chkAutoShield.TabIndex = 9;
            this.chkAutoShield.Text = "Autoshield Self";
            this.chkAutoShield.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.chkEatAndDrinkWhileResting);
            this.groupBox14.Location = new System.Drawing.Point(289, 194);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(258, 49);
            this.groupBox14.TabIndex = 8;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Resting";
            // 
            // chkEatAndDrinkWhileResting
            // 
            this.chkEatAndDrinkWhileResting.AutoSize = true;
            this.chkEatAndDrinkWhileResting.Location = new System.Drawing.Point(14, 22);
            this.chkEatAndDrinkWhileResting.Name = "chkEatAndDrinkWhileResting";
            this.chkEatAndDrinkWhileResting.Size = new System.Drawing.Size(155, 17);
            this.chkEatAndDrinkWhileResting.TabIndex = 0;
            this.chkEatAndDrinkWhileResting.Text = "Eat and Drink while resting.";
            this.chkEatAndDrinkWhileResting.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.chkLevitateWhileFalling);
            this.groupBox13.Location = new System.Drawing.Point(11, 218);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(258, 49);
            this.groupBox13.TabIndex = 8;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Levitate";
            // 
            // chkLevitateWhileFalling
            // 
            this.chkLevitateWhileFalling.AutoSize = true;
            this.chkLevitateWhileFalling.Location = new System.Drawing.Point(14, 22);
            this.chkLevitateWhileFalling.Name = "chkLevitateWhileFalling";
            this.chkLevitateWhileFalling.Size = new System.Drawing.Size(192, 17);
            this.chkLevitateWhileFalling.TabIndex = 0;
            this.chkLevitateWhileFalling.Text = "Levitate While Falling (Expirimental)";
            this.chkLevitateWhileFalling.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.chkBuffShadowProtection);
            this.groupBox12.Location = new System.Drawing.Point(289, 70);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(258, 53);
            this.groupBox12.TabIndex = 7;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Shadow Protection";
            // 
            // chkBuffShadowProtection
            // 
            this.chkBuffShadowProtection.AutoSize = true;
            this.chkBuffShadowProtection.Location = new System.Drawing.Point(14, 22);
            this.chkBuffShadowProtection.Name = "chkBuffShadowProtection";
            this.chkBuffShadowProtection.Size = new System.Drawing.Size(138, 17);
            this.chkBuffShadowProtection.TabIndex = 0;
            this.chkBuffShadowProtection.Text = "Buff Shadow Protection";
            this.chkBuffShadowProtection.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.chkFadeOnAggro);
            this.groupBox11.Location = new System.Drawing.Point(289, 136);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(258, 49);
            this.groupBox11.TabIndex = 7;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Fade";
            // 
            // chkFadeOnAggro
            // 
            this.chkFadeOnAggro.AutoSize = true;
            this.chkFadeOnAggro.Location = new System.Drawing.Point(14, 22);
            this.chkFadeOnAggro.Name = "chkFadeOnAggro";
            this.chkFadeOnAggro.Size = new System.Drawing.Size(132, 17);
            this.chkFadeOnAggro.TabIndex = 0;
            this.chkFadeOnAggro.Text = "Enable Fade on Aggro";
            this.chkFadeOnAggro.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkVampiricEmbrace);
            this.groupBox3.Controls.Add(this.chkForceShadowFormInCombat);
            this.groupBox3.Controls.Add(this.chkUseShadowform);
            this.groupBox3.Location = new System.Drawing.Point(11, 117);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 95);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shadowform Settings";
            // 
            // chkVampiricEmbrace
            // 
            this.chkVampiricEmbrace.AutoSize = true;
            this.chkVampiricEmbrace.Location = new System.Drawing.Point(12, 66);
            this.chkVampiricEmbrace.Name = "chkVampiricEmbrace";
            this.chkVampiricEmbrace.Size = new System.Drawing.Size(133, 17);
            this.chkVampiricEmbrace.TabIndex = 5;
            this.chkVampiricEmbrace.Text = "Use Vampiric Embrace";
            this.chkVampiricEmbrace.UseVisualStyleBackColor = true;
            // 
            // chkForceShadowFormInCombat
            // 
            this.chkForceShadowFormInCombat.AutoSize = true;
            this.chkForceShadowFormInCombat.Location = new System.Drawing.Point(12, 43);
            this.chkForceShadowFormInCombat.Name = "chkForceShadowFormInCombat";
            this.chkForceShadowFormInCombat.Size = new System.Drawing.Size(171, 17);
            this.chkForceShadowFormInCombat.TabIndex = 4;
            this.chkForceShadowFormInCombat.Text = "Force Shadow Form in Combat";
            this.chkForceShadowFormInCombat.UseVisualStyleBackColor = true;
            // 
            // chkUseShadowform
            // 
            this.chkUseShadowform.AutoSize = true;
            this.chkUseShadowform.Location = new System.Drawing.Point(12, 22);
            this.chkUseShadowform.Name = "chkUseShadowform";
            this.chkUseShadowform.Size = new System.Drawing.Size(107, 17);
            this.chkUseShadowform.TabIndex = 3;
            this.chkUseShadowform.Text = "Use Shadowform";
            this.chkUseShadowform.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUseFortitude);
            this.groupBox2.Location = new System.Drawing.Point(289, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Foritude";
            // 
            // chkUseFortitude
            // 
            this.chkUseFortitude.AutoSize = true;
            this.chkUseFortitude.Location = new System.Drawing.Point(14, 22);
            this.chkUseFortitude.Name = "chkUseFortitude";
            this.chkUseFortitude.Size = new System.Drawing.Size(89, 17);
            this.chkUseFortitude.TabIndex = 0;
            this.chkUseFortitude.Text = "Buff Fortitude";
            this.chkUseFortitude.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkInnerWillWhileResting);
            this.groupBox1.Controls.Add(this.radInnerFire);
            this.groupBox1.Controls.Add(this.radInnerWill);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 100);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inner Fire/Will Settings";
            // 
            // chkInnerWillWhileResting
            // 
            this.chkInnerWillWhileResting.AutoSize = true;
            this.chkInnerWillWhileResting.Location = new System.Drawing.Point(12, 68);
            this.chkInnerWillWhileResting.Name = "chkInnerWillWhileResting";
            this.chkInnerWillWhileResting.Size = new System.Drawing.Size(218, 17);
            this.chkInnerWillWhileResting.TabIndex = 3;
            this.chkInnerWillWhileResting.Text = " Inner Will while resting (10% Faster Run)";
            this.chkInnerWillWhileResting.UseVisualStyleBackColor = true;
            // 
            // radInnerFire
            // 
            this.radInnerFire.AutoSize = true;
            this.radInnerFire.Checked = true;
            this.radInnerFire.Location = new System.Drawing.Point(12, 22);
            this.radInnerFire.Name = "radInnerFire";
            this.radInnerFire.Size = new System.Drawing.Size(154, 17);
            this.radInnerFire.TabIndex = 0;
            this.radInnerFire.TabStop = true;
            this.radInnerFire.Text = "Use Inner Fire (For Combat)";
            this.radInnerFire.UseVisualStyleBackColor = true;
            // 
            // radInnerWill
            // 
            this.radInnerWill.AutoSize = true;
            this.radInnerWill.Checked = true;
            this.radInnerWill.Location = new System.Drawing.Point(12, 45);
            this.radInnerWill.Name = "radInnerWill";
            this.radInnerWill.Size = new System.Drawing.Size(154, 17);
            this.radInnerWill.TabIndex = 1;
            this.radInnerWill.TabStop = true;
            this.radInnerWill.Text = "Use Inner Will (For Combat)";
            this.radInnerWill.UseVisualStyleBackColor = true;
            // 
            // TargetingTab
            // 
            this.TargetingTab.Controls.Add(this.groupBox10);
            this.TargetingTab.Location = new System.Drawing.Point(4, 22);
            this.TargetingTab.Name = "TargetingTab";
            this.TargetingTab.Size = new System.Drawing.Size(573, 306);
            this.TargetingTab.TabIndex = 3;
            this.TargetingTab.Text = "Targeting";
            this.TargetingTab.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chkRafLeadertTargeting);
            this.groupBox10.Controls.Add(this.chkEnableMultiDotTargeting);
            this.groupBox10.Controls.Add(this.chkEnableMindSearTargeting);
            this.groupBox10.Controls.Add(this.chkEnableTargeting);
            this.groupBox10.Location = new System.Drawing.Point(11, 9);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(541, 134);
            this.groupBox10.TabIndex = 12;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Targeting";
            // 
            // chkRafLeadertTargeting
            // 
            this.chkRafLeadertTargeting.AutoSize = true;
            this.chkRafLeadertTargeting.Location = new System.Drawing.Point(12, 49);
            this.chkRafLeadertTargeting.Name = "chkRafLeadertTargeting";
            this.chkRafLeadertTargeting.Size = new System.Drawing.Size(163, 17);
            this.chkRafLeadertTargeting.TabIndex = 11;
            this.chkRafLeadertTargeting.Text = "Enable Raf Leader Targeting";
            this.chkRafLeadertTargeting.UseVisualStyleBackColor = true;
            // 
            // chkEnableMultiDotTargeting
            // 
            this.chkEnableMultiDotTargeting.AutoSize = true;
            this.chkEnableMultiDotTargeting.Location = new System.Drawing.Point(187, 50);
            this.chkEnableMultiDotTargeting.Name = "chkEnableMultiDotTargeting";
            this.chkEnableMultiDotTargeting.Size = new System.Drawing.Size(152, 17);
            this.chkEnableMultiDotTargeting.TabIndex = 10;
            this.chkEnableMultiDotTargeting.Text = "Enable Multi Dot Targeting";
            this.chkEnableMultiDotTargeting.UseVisualStyleBackColor = true;
            // 
            // chkEnableMindSearTargeting
            // 
            this.chkEnableMindSearTargeting.AutoSize = true;
            this.chkEnableMindSearTargeting.Location = new System.Drawing.Point(187, 26);
            this.chkEnableMindSearTargeting.Name = "chkEnableMindSearTargeting";
            this.chkEnableMindSearTargeting.Size = new System.Drawing.Size(158, 17);
            this.chkEnableMindSearTargeting.TabIndex = 9;
            this.chkEnableMindSearTargeting.Text = "Enable Mind Sear Targeting";
            this.chkEnableMindSearTargeting.UseVisualStyleBackColor = true;
            // 
            // chkEnableTargeting
            // 
            this.chkEnableTargeting.AutoSize = true;
            this.chkEnableTargeting.Location = new System.Drawing.Point(12, 26);
            this.chkEnableTargeting.Name = "chkEnableTargeting";
            this.chkEnableTargeting.Size = new System.Drawing.Size(153, 17);
            this.chkEnableTargeting.TabIndex = 8;
            this.chkEnableTargeting.Text = "Allow CC to choose targets";
            this.chkEnableTargeting.UseVisualStyleBackColor = true;
            // 
            // SettingsTabs
            // 
            this.SettingsTabs.Controls.Add(this.TargetingTab);
            this.SettingsTabs.Controls.Add(this.BuffTab);
            this.SettingsTabs.Controls.Add(this.CombatTab);
            this.SettingsTabs.Controls.Add(this.CooldownsTab);
            this.SettingsTabs.Controls.Add(this.tabPage1);
            this.SettingsTabs.Controls.Add(this.Advanced);
            this.SettingsTabs.Location = new System.Drawing.Point(1, 96);
            this.SettingsTabs.Name = "SettingsTabs";
            this.SettingsTabs.SelectedIndex = 0;
            this.SettingsTabs.Size = new System.Drawing.Size(581, 332);
            this.SettingsTabs.TabIndex = 4;
            // 
            // Mind Blast Settings
            // 
            this.groupBox18.Controls.Add(this.chkUseMindBlastOnCD);
            this.groupBox18.Location = new System.Drawing.Point(289, 251);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(258, 49);
            this.groupBox18.TabIndex = 11;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Mind Blast";
            // 
            // chkUseMindBlastOnCD
            // 
            this.chkUseMindBlastOnCD.AutoSize = true;
            this.chkUseMindBlastOnCD.Location = new System.Drawing.Point(11, 21);
            this.chkUseMindBlastOnCD.Name = "chkUseMindBlastOnCD";
            this.chkUseMindBlastOnCD.Size = new System.Drawing.Size(91, 17);
            this.chkUseMindBlastOnCD.TabIndex = 8;
            this.chkUseMindBlastOnCD.Text = "Use Mind Blast on Cooldown";
            this.chkUseMindBlastOnCD.UseVisualStyleBackColor = true;
            // 
            // AltarboyConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 457);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.mastHead);
            this.Controls.Add(this.btnSaveAndClose);
            this.Controls.Add(this.SettingsTabs);
            this.Controls.Add(this.btnSave);
            this.Name = "AltarboyConfig";
            this.Text = "Altarboy : The Priest Companion";
            this.Load += new System.EventHandler(this.AltarboyConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mastHead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altarboySettingsBindingSource)).EndInit();
            this.Advanced.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mobGrid)).EndInit();
            this.CooldownsTab.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.Trinket2Label.ResumeLayout(false);
            this.Trinket2Label.PerformLayout();
            this.Trinket1Label.ResumeLayout(false);
            this.Trinket1Label.PerformLayout();
            this.CombatTab.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMindSearCount)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDispersionManaPercent)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShadowWordDeathManaPercent)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShadowFiendManaPercent)).EndInit();
            this.BuffTab.ResumeLayout(false);
            this.BuffTab.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TargetingTab.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.SettingsTabs.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnSave;
        private Button btnSaveAndClose;
        private PictureBox mastHead;
        private Label lblVersion;
        private BindingSource altarboySettingsBindingSource;
        private TabPage Advanced;
        private GroupBox groupBox7;
        private TextBox txtCooldownDuration;
        private Label label7;
        private TextBox txtSpellClipping;
        private Label label5;
        private TabPage tabPage1;
        private Button btnRemoveSelected;
        private Button btnAddMobToList;
        private DataGridView mobGrid;
        private DataGridViewTextBoxColumn MobID;
        private DataGridViewTextBoxColumn MobName;
        private DataGridViewCheckBoxColumn MobFocus;
        private DataGridViewCheckBoxColumn IgnoreMob;
        private DataGridViewCheckBoxColumn MindSpikeMob;
        private DataGridViewCheckBoxColumn MindSearMob;
        private DataGridViewCheckBoxColumn chkShackle;
        private TabPage CooldownsTab;
        private GroupBox groupBox16;
        private ComboBox cmbPotionNames;
        private CheckBox chkUsePotionsOnLust;
        private GroupBox groupBox8;
        private CheckBox chkTinkerGlovesWithArchangel;
        private CheckBox chkUseGloves;
        private CheckBox chkUseBelt;
        private GroupBox Trinket2Label;
        private RadioButton radTrinket2NeverUse;
        private RadioButton radTrinket2UseWithArchangel;
        private RadioButton radTrinket2UseOnCD;
        private GroupBox Trinket1Label;
        private RadioButton radTrinket1NeverUse;
        private RadioButton radTrinket1UseWithArchangel;
        private RadioButton radTrinket1UseOnCD;
        private TabPage CombatTab;
        private GroupBox groupBox15;
        private CheckBox chkClipMindFlay;
        private GroupBox groupBox9;
        private Label label6;
        private CheckBox chkAllowMindSear;
        private NumericUpDown numMindSearCount;
        private GroupBox groupBox6;
        private CheckBox chkUseDispersionForManaRegen;
        private Label label4;
        private NumericUpDown numDispersionManaPercent;
        private GroupBox groupBox5;
        private CheckBox chkSWDWhileMoving;
        private CheckBox chkShadowWordDeathCheckDebuffs;
        private Label label3;
        private CheckBox chkShadowWordDeathExecute;
        private RadioButton radShadowWordDeathNever;
        private Label label2;
        private NumericUpDown numShadowWordDeathManaPercent;
        private RadioButton radShadowWordDeathManaRegen;
        private RadioButton radShadowWordDeathOnCD;
        private GroupBox groupBox4;
        private CheckBox chkForceFiendOnBloodLust;
        private CheckBox chkUseMindBlastOnCD;
        private Label label1;
        private NumericUpDown numShadowFiendManaPercent;
        private RadioButton radUseShadowFiendForManaRegen;
        private RadioButton radUseShadowFiendOnCD;
        private TabPage BuffTab;
        private GroupBox groupBox14;
        private CheckBox chkEatAndDrinkWhileResting;
        private GroupBox groupBox13;
        private CheckBox chkLevitateWhileFalling;
        private GroupBox groupBox12;
        private CheckBox chkBuffShadowProtection;
        private GroupBox groupBox11;
        private CheckBox chkFadeOnAggro;
        private GroupBox groupBox3;
        private CheckBox chkVampiricEmbrace;
        private CheckBox chkForceShadowFormInCombat;
        private CheckBox chkUseShadowform;
        private GroupBox groupBox2;
        private CheckBox chkUseFortitude;
        private GroupBox groupBox1;
        private CheckBox chkInnerWillWhileResting;
        private RadioButton radInnerFire;
        private RadioButton radInnerWill;
        private TabPage TargetingTab;
        private GroupBox groupBox10;
        private CheckBox chkRafLeadertTargeting;
        private CheckBox chkEnableMultiDotTargeting;
        private CheckBox chkEnableMindSearTargeting;
        private CheckBox chkEnableTargeting;
        private TabControl SettingsTabs;
        private CheckBox chkAutoShield;
        private GroupBox groupBox17;
        private CheckBox chkMindSpikeWithFiend;
        private CheckBox chkUsefiendOnArchAngel;
        private GroupBox groupBox18;
        private CheckBox checkBox1;
    }
}