using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Plugins.PluginClass;

using IU = InterrupterUltimate.InterrupterUltimate;

namespace InterrupterUltimate
{
    public partial class GUI : Form
    {
        private static GUI _instance;

        public static GUI Instance
        {
            get
            {
                if (_instance == default(GUI))
                    _instance = new GUI();

                return _instance;
            }
        }

        public GUI()
        {
            InitializeComponent();
            this.Load += new EventHandler(GUI_Load);
        }

        public void GUI_Load(object sender, EventArgs e)
        {
            LoadSettings();
            int i;

            //for (i = 0; i < dgvSpellsDB.Rows.Count; i++)
            //    dgvSpellsDB.Rows.RemoveAt(i);
            dgvSpellsDB.Rows.Clear();

            dgvSpellsDB.Columns[0].ValueType = typeof(string);
            dgvSpellsDB.Columns[1].ValueType = typeof(bool);
            dgvSpellsDB.Columns[2].ValueType = typeof(string);
            dgvSpellsDB.Columns[3].ValueType = typeof(int);
            dgvSpellsDB.Columns[4].ValueType = typeof(bool);
            dgvSpellsDB.Columns[5].ValueType = typeof(string);

            foreach (KeyValuePair<int, IU.InterrupterSpell> spell in IU.InterrupterContainer.Spells)
            {
                dgvSpellsDB.Rows.Add();
                i = dgvSpellsDB.Rows.Count - 1;
                dgvSpellsDB[0, i].Value = "Verified";
                dgvSpellsDB[1, i].Value = false;
                dgvSpellsDB[2, i].Value = spell.Value.Name;
                dgvSpellsDB[3, i].Value = spell.Key;
                dgvSpellsDB[4, i].Value = spell.Value.Include;
                dgvSpellsDB[5, i].Value = spell.Value.TagGroupIDs.ToRealString();
            }

            dgvSpellsDB.CellClick += new DataGridViewCellEventHandler(dgvSpells_CellClick);
            dgvSpellsDB.CellValueChanged += new DataGridViewCellEventHandler(dgvSpells_CellValueChanged);

            dgvTagGroupsDB.Rows.Clear();

            dgvTagGroupsDB.Columns[0].ValueType = typeof(string);
            dgvTagGroupsDB.Columns[1].ValueType = typeof(bool);
            dgvTagGroupsDB.Columns[2].ValueType = typeof(string);
            dgvTagGroupsDB.Columns[3].ValueType = typeof(bool);
            dgvTagGroupsDB.Columns[4].ValueType = typeof(bool);
            dgvTagGroupsDB.Columns[5].ValueType = typeof(string);

            foreach (KeyValuePair<string, IU.SpellTagGroup> tag in IU.InterrupterContainer.Tags)
            {
                dgvTagGroupsDB.Rows.Add();
                i = dgvTagGroupsDB.Rows.Count - 1;
                dgvTagGroupsDB[0, i].Value = "Verified";
                dgvTagGroupsDB[1, i].Value = false;
                dgvTagGroupsDB[2, i].Value = tag.Key;
                dgvTagGroupsDB[3, i].Value = tag.Value.Include;
                dgvTagGroupsDB[4, i].Value = tag.Value.EntirelyExclude;
                dgvTagGroupsDB[5, i].Value = tag.Value.Aliases.ToRealString();
            }

            dgvTagGroupsDB.CellClick += new DataGridViewCellEventHandler(dgvTagGroups_CellClick);
            dgvTagGroupsDB.CellValueChanged += new DataGridViewCellEventHandler(dgvTagGroups_CellValueChanged);

            dgvTargets.Rows.Clear();

            dgvTargets.Columns[0].ValueType = typeof(string);
            dgvTargets.Columns[1].ValueType = typeof(bool);
            dgvTargets.Columns[2].ValueType = typeof(string);
            dgvTargets.Columns[3].ValueType = typeof(bool);

            foreach (KeyValuePair<string, bool> unit in IU.InterrupterContainer.Units)
            {
                dgvTargets.Rows.Add();
                i = dgvTargets.Rows.Count - 1;
                dgvTargets[0, i].Value = "Verified";
                dgvTargets[1, i].Value = false;
                dgvTargets[2, i].Value = unit.Key;
                dgvTargets[3, i].Value = unit.Value;
            }

            dgvTargets.CellClick += new DataGridViewCellEventHandler(dgvTargets_CellClick);
            dgvTargets.CellValueChanged += new DataGridViewCellEventHandler(dgvTargets_CellValueChanged);
        }

        #region dgvSpells

        private void dgvSpells_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.RowIndex < dgvSpellsDB.Rows.Count &&
                dgvSpellsDB[0, e.RowIndex].Value != "Verified")
            {
                int spellID = dgvSpellsDB[3, e.RowIndex].Value.ToString().ToInt32();

                IU.InterrupterContainer.VerifySpellToInterrupt(
                    spellID,
                    dgvSpellsDB[4, e.RowIndex].Value.ToString().ToBoolean(),
                    dgvSpellsDB[5, e.RowIndex].Value.ToString().Split(','));
                
                dgvSpellsDB[0, e.RowIndex].Value = "Verified"; 
                dgvSpellsDB[1, e.RowIndex].Value = false;
                dgvSpellsDB[2, e.RowIndex].Value = IU.InterrupterContainer.Spells[spellID].Name; 
                dgvSpellsDB[3, e.RowIndex].Value = spellID;
                dgvSpellsDB[4, e.RowIndex].Value = IU.InterrupterContainer.Spells[spellID].Include;
                dgvSpellsDB[5, e.RowIndex].Value = IU.InterrupterContainer.Spells[spellID].TagGroupIDs.ToRealString();
            }
        }

        private void dgvSpells_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                dgvSpellsDB[0, e.RowIndex].Value = "Verify";
            }
        }

        private void dgvSpellsAddButton_Click(object sender, EventArgs e)
        {
            int i = dgvSpellsDB.Rows.Add();

            dgvSpellsDB[0, i].Value = "Verify";
            dgvSpellsDB[1, i].Value = false;
            dgvSpellsDB[2, i].Value = "";
            dgvSpellsDB[3, i].Value = 0;
            dgvSpellsDB[4, i].Value = true;
            dgvSpellsDB[5, i].Value = "";
        }

        private void dgvSpellsVerifyFirstButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSpellsDB.RowCount; i++ )
            {
                if (dgvSpellsDB[0, i].Value != "Verified")
                {
                    int spellID = dgvSpellsDB[3, i].Value.ToString().ToInt32();

                    IU.InterrupterContainer.VerifySpellToInterrupt(
                        spellID,
                        dgvSpellsDB[4, i].Value.ToString().ToBoolean(),
                        dgvSpellsDB[5, i].Value.ToString().Split(','));

                    dgvSpellsDB[0, i].Value = "Verified";
                    dgvSpellsDB[1, i].Value = false;
                    dgvSpellsDB[2, i].Value = IU.InterrupterContainer.Spells[spellID].Name;
                    dgvSpellsDB[3, i].Value = spellID;
                    dgvSpellsDB[4, i].Value = IU.InterrupterContainer.Spells[spellID].Include;
                    dgvSpellsDB[5, i].Value = IU.InterrupterContainer.Spells[spellID].TagGroupIDs.ToRealString();

                    return;
                }
            }
        }

        private void dgvSpellsVerifyAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSpellsDB.RowCount; i++)
            {
                if (dgvSpellsDB[0, i].Value != "Verified")
                {
                    int spellID = dgvSpellsDB[3, i].Value.ToString().ToInt32();

                    IU.InterrupterContainer.VerifySpellToInterrupt(
                        spellID,
                        dgvSpellsDB[4, i].Value.ToString().ToBoolean(),
                        dgvSpellsDB[5, i].Value.ToString().Split(','));

                    dgvSpellsDB[0, i].Value = "Verified";
                    dgvSpellsDB[1, i].Value = false;
                    dgvSpellsDB[2, i].Value = IU.InterrupterContainer.Spells[spellID].Name;
                    dgvSpellsDB[3, i].Value = spellID;
                    dgvSpellsDB[4, i].Value = IU.InterrupterContainer.Spells[spellID].Include;
                    dgvSpellsDB[5, i].Value = IU.InterrupterContainer.Spells[spellID].TagGroupIDs.ToRealString();
              
                }
            }
        }

        private void dgvSpellsRemoveOneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSpellsDB.RowCount; i++)
            {
                if (dgvSpellsDB[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.Spells.Remove(dgvSpellsDB[3, i].Value.ToString().ToInt32());
                    dgvSpellsDB.Rows.RemoveAt(i);
                    return;
                }
            }
        }

        private void dgvSpellsRemoveAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSpellsDB.RowCount; i++)
            {
                if (dgvSpellsDB[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.Spells.Remove(dgvSpellsDB[3, i].Value.ToString().ToInt32());
                    dgvSpellsDB.Rows.RemoveAt(i);
                }
            }
        }


        #endregion

        #region dgvTagGroups

        private void dgvTagGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.RowIndex < dgvTagGroupsDB.Rows.Count &&
                dgvTagGroupsDB[0, e.RowIndex].Value != "Verified")
            {
                string tagGroupID = dgvTagGroupsDB[2, e.RowIndex].Value.ToString();

                IU.InterrupterContainer.VerifyTagGroupToInterrupt(
                tagGroupID,
                dgvTagGroupsDB[3, e.RowIndex].Value.ToString().ToBoolean(),
                dgvTagGroupsDB[4, e.RowIndex].Value.ToString().ToBoolean(),
                dgvTagGroupsDB[5, e.RowIndex].Value.ToString().Split(','));

                dgvTagGroupsDB[0, e.RowIndex].Value = "Verified";
                dgvTagGroupsDB[1, e.RowIndex].Value = false;
                dgvTagGroupsDB[2, e.RowIndex].Value = tagGroupID;
                dgvTagGroupsDB[3, e.RowIndex].Value = IU.InterrupterContainer.Tags[tagGroupID].Include;
                dgvTagGroupsDB[4, e.RowIndex].Value = IU.InterrupterContainer.Tags[tagGroupID].EntirelyExclude;
                dgvTagGroupsDB[5, e.RowIndex].Value = IU.InterrupterContainer.Tags[tagGroupID].Aliases.ToRealString();
            }
        }
        

        private void dgvTagGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                dgvTagGroupsDB[0, e.RowIndex].Value = "Verify";
            }
        }

        private void dgvTagGroupsAddButton_Click(object sender, EventArgs e)
        {
            int i;
            i = dgvTagGroupsDB.Rows.Add();


            dgvTagGroupsDB[0, i].Value = "Verify";
            dgvTagGroupsDB[1, i].Value = false;
            dgvTagGroupsDB[2, i].Value = "";
            dgvTagGroupsDB[3, i].Value = true;
            dgvTagGroupsDB[4, i].Value = false;
            dgvTagGroupsDB[5, i].Value = "";
        }

        private void dgvTagGroupsVerifyFirstButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTagGroupsDB.RowCount; i++)
            {
                if (dgvTagGroupsDB[0, i].Value != "Verified")
                {
                    string tagGroupID = dgvTagGroupsDB[2, i].Value.ToString();

                    IU.InterrupterContainer.VerifyTagGroupToInterrupt(
                        tagGroupID,
                        dgvTagGroupsDB[3, i].Value.ToString().ToBoolean(),
                        dgvTagGroupsDB[4, i].Value.ToString().ToBoolean(),
                        dgvTagGroupsDB[5, i].Value.ToString().Split(','));

                    dgvTagGroupsDB[0, i].Value = "Verified";
                    dgvTagGroupsDB[1, i].Value = false;
                    dgvTagGroupsDB[2, i].Value = tagGroupID;
                    dgvTagGroupsDB[3, i].Value = IU.InterrupterContainer.Tags[tagGroupID].Include;
                    dgvTagGroupsDB[4, i].Value = IU.InterrupterContainer.Tags[tagGroupID].EntirelyExclude;
                    dgvTagGroupsDB[5, i].Value = IU.InterrupterContainer.Tags[tagGroupID].Aliases.ToRealString();

                    return;
                }
            }
        }

        private void dgvTagGroupsVerifyAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTagGroupsDB.RowCount; i++)
            {
                if (dgvTagGroupsDB[0, i].Value != "Verified")
                {
                    string tagGroupID = dgvTagGroupsDB[2, i].Value.ToString();

                    IU.InterrupterContainer.VerifyTagGroupToInterrupt(
                        tagGroupID,
                        dgvTagGroupsDB[3, i].Value.ToString().ToBoolean(),
                        dgvTagGroupsDB[4, i].Value.ToString().ToBoolean(),
                        dgvTagGroupsDB[5, i].Value.ToString().Split(','));

                    dgvTagGroupsDB[0, i].Value = "Verified";
                    dgvTagGroupsDB[1, i].Value = false;
                    dgvTagGroupsDB[2, i].Value = tagGroupID;
                    dgvTagGroupsDB[3, i].Value = IU.InterrupterContainer.Tags[tagGroupID].Include;
                    dgvTagGroupsDB[4, i].Value = IU.InterrupterContainer.Tags[tagGroupID].EntirelyExclude;
                    dgvTagGroupsDB[5, i].Value = IU.InterrupterContainer.Tags[tagGroupID].Aliases.ToRealString();
                }
            }
        }

        private void dgvTagGroupsRemoveOneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTagGroupsDB.RowCount; i++)
            {
                if (dgvTagGroupsDB[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.Tags.Remove(dgvTagGroupsDB[3, i].Value.ToString());
                    dgvTagGroupsDB.Rows.RemoveAt(i);
                    return;
                }
            }
        }

        private void dgvTagGroupsRemoveAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTagGroupsDB.RowCount; i++)
            {
                if (dgvTagGroupsDB[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.Tags.Remove(dgvTagGroupsDB[3, i].Value.ToString());
                    dgvTagGroupsDB.Rows.RemoveAt(i);
                }
            }
        }

        #endregion

        #region dgvTargets

        private void dgvTargets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.RowIndex < dgvTargets.Rows.Count &&
                dgvTargets[0, e.RowIndex].Value != "Verified")
            {
                string unitID = dgvTargets[2, e.RowIndex].Value.ToString();
                IU.InterrupterContainer.VerifyUnit(unitID, dgvTargets[3, e.RowIndex].Value.ToString().ToBoolean());

                dgvTargets[0, e.RowIndex].Value = "Verified";
                dgvTargets[1, e.RowIndex].Value = false;
                dgvTargets[2, e.RowIndex].Value = unitID;
                dgvTargets[3, e.RowIndex].Value = IU.InterrupterContainer.Units[unitID];
            }
        }

        private void dgvTargets_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.ColumnIndex != 1)
            {
                dgvTargets[0, e.RowIndex].Value = "Verify";
            }
        }

        private void dgvTargetsAddButton_Click(object sender, EventArgs e)
        {
            int i = dgvTargets.Rows.Add();

            dgvTargets[0, i].Value = "Verify";
            dgvTargets[1, i].Value = false;
            dgvTargets[2, i].Value = "";
            dgvTargets[3, i].Value = false;
        }

        private void dgvTargetsVerifyFirstButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTargets.RowCount; i++)
            {
                if (dgvTargets[0, i].Value != "Verified")
                {
                    string unitID = dgvTargets[2, i].Value.ToString();
                    IU.InterrupterContainer.VerifyUnit(unitID, dgvTargets[3, i].Value.ToString().ToBoolean());
                    IU.Instance.SpellTracker.VerifyTargetToTrack(unitID, IU.InterrupterContainer.Units[unitID].ToString().ToBoolean());

                    dgvTargets[0, i].Value = "Verified";
                    dgvTargets[1, i].Value = false;
                    dgvTargets[2, i].Value = unitID;
                    dgvTargets[3, i].Value = IU.InterrupterContainer.Units[unitID];

                    return;
                }
            }
        }

        private void dgvTargetsVerifyAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTargets.RowCount; i++)
            {
                if (dgvTargets[0, i].Value != "Verified")
                {
                    string unitID = dgvTargets[2, i].Value.ToString();
                    IU.InterrupterContainer.VerifyUnit(unitID, dgvTargets[3, i].Value.ToString().ToBoolean());
                    IU.Instance.SpellTracker.VerifyTargetToTrack(unitID, IU.InterrupterContainer.Units[unitID].ToString().ToBoolean());

                    dgvTargets[0, i].Value = "Verified";
                    dgvTargets[1, i].Value = false;
                    dgvTargets[2, i].Value = unitID;
                    dgvTargets[3, i].Value = IU.InterrupterContainer.Units[unitID];
                }
            }
        }

        private void dgvTargetsRemoveOneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTargets.RowCount; i++)
            {
                if (dgvTargets[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.RemoveUnit(dgvTargets[2, i].Value.ToString());
                    dgvTargets.Rows.RemoveAt(i);
                    return;
                }
            }
        }

        private void dgvTargetsRemoveAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTargets.RowCount; i++)
            {
                if (dgvTargets[1, i].Value.ToString().ToBoolean())
                {
                    IU.InterrupterContainer.RemoveUnit(dgvTargets[2, i].Value.ToString());
                    dgvTargets.Rows.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Settings

        private void LoadSettings()
        {
            cbShouldForceCast.Checked = IU.Instance.Settings.ShouldForceCast;
            cbInterruptAll.Checked = IU.Instance.Settings.InterruptAll;
            cbDebugMode.Checked = IU.Instance.Settings.DebugMode;

            nudCastMilliSecondsLeft.Value = IU.Instance.Settings.CastMillisecondsLeft;
            nudChannelMillisecondsElapsed.Value = IU.Instance.Settings.ChannelMillisecondsElapsed;
        }

        private void SaveSettings()
        {
            IU.Instance.Settings.ShouldForceCast = cbShouldForceCast.Checked;
            IU.Instance.Settings.InterruptAll = cbInterruptAll.Checked;
            IU.Instance.Settings.DebugMode = cbDebugMode.Checked;

            IU.Instance.Settings.CastMillisecondsLeft = (int)nudCastMilliSecondsLeft.Value;
            IU.Instance.Settings.ChannelMillisecondsElapsed = (int)nudChannelMillisecondsElapsed.Value;
        }

        private void bSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void bReloadSettings_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        #endregion

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
