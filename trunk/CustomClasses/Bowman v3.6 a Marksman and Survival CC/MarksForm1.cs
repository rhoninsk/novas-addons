using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Marksman
{
    public partial class MarksForm1 : Form
    {
        public MarksForm1()
        {
            InitializeComponent();
        }
        private void MarksForm1_Load(object sender, EventArgs e)
        {
            MarksmanSettings.Instance.Load();

            Mobs.Value = new decimal(MarksmanSettings.Instance.Mobs);
            PET.Value = new decimal(MarksmanSettings.Instance.PET);
            Aimed.Checked = MarksmanSettings.Instance.AimedROT;
            Arcane.Checked = MarksmanSettings.Instance.ArcaneROT;
            Mixed.Checked = MarksmanSettings.Instance.MixedROT;
            Explo.Checked = MarksmanSettings.Instance.ExploROT;
            Marksman.Checked = MarksmanSettings.Instance.MMSPEC;
            Survival.Checked = MarksmanSettings.Instance.SSPEC;
            Party.Checked = MarksmanSettings.Instance.Party;
            DSHC.Checked = MarksmanSettings.Instance.DSHC;
            DSLFR.Checked = MarksmanSettings.Instance.DSLFR;
            DSNOR.Checked = MarksmanSettings.Instance.DSNOR;
            CallPet.Checked = MarksmanSettings.Instance.CP;
            RevivePet.Checked = MarksmanSettings.Instance.RP;
            MendPet.Checked = MarksmanSettings.Instance.MP;
            MultiShot.Checked = MarksmanSettings.Instance.MS;
            Launcher.Checked = MarksmanSettings.Instance.TL;
            Trinket1.Checked = MarksmanSettings.Instance.T1;
            Trinket2.Checked = MarksmanSettings.Instance.T2;
            Rapid.Checked = MarksmanSettings.Instance.RF;
            LifeBlood.Checked = MarksmanSettings.Instance.LB;
            Gloves.Checked = MarksmanSettings.Instance.GE;
            Racial.Checked = MarksmanSettings.Instance.RS;
            
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/classes/all-one/42174-lazyraider-all-one-pve-ccs.html");
            linkLabel1.LinkVisited = true;
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.wowhead.com/talent#ccbcZfRG0GkMuroM");
            linkLabel3.LinkVisited = true;
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.wowhead.com/talent#ccZfMZcbhfMhrRsk");
            linkLabel4.LinkVisited = true;
        }
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/newreply.php?p=425057&noquote=1");
            linkLabel5.LinkVisited = true;
        }
        private void Mobs_ValueChanged(object sender, EventArgs e)
        {
            MarksmanSettings.Instance.Mobs = (int)Mobs.Value;
        }
        private void PET_ValueChanged(object sender, EventArgs e)
        {
            MarksmanSettings.Instance.PET = (int)PET.Value;
        }
        private void Aimed_CheckedChanged(object sender, EventArgs e)
        {
            if (Aimed.Checked == true)
            {
                MarksmanSettings.Instance.AimedROT = true;
            }
            else
            {
                MarksmanSettings.Instance.AimedROT = false;
            }
        }
        private void Arcane_CheckedChanged(object sender, EventArgs e)
        {
            if (Arcane.Checked == true)
            {
                MarksmanSettings.Instance.ArcaneROT = true;
            }
            else
            {
                MarksmanSettings.Instance.ArcaneROT = false;
            }
        }
        private void Mixed_CheckedChanged(object sender, EventArgs e)
        {
            if (Mixed.Checked == true)
            {
                MarksmanSettings.Instance.MixedROT = true;
            }
            else
            {
                MarksmanSettings.Instance.MixedROT = false;
            }
        }
        private void Explo_CheckedChanged(object sender, EventArgs e)
        {
            if (Explo.Checked == true)
            {
                MarksmanSettings.Instance.ExploROT = true;
            }
            else
            {
                MarksmanSettings.Instance.ExploROT = false;
            }
        }
        private void Marksman_CheckedChanged(object sender, EventArgs e)
        {
            if (Marksman.Checked == true)
            {
                MarksmanSettings.Instance.MMSPEC = true;
            }
            else
            {
                MarksmanSettings.Instance.MMSPEC = false;
            }
        }
        private void Survival_CheckedChanged(object sender, EventArgs e)
        {
            if (Survival.Checked == true)
            {
                MarksmanSettings.Instance.SSPEC = true;
            }
            else
            {
                MarksmanSettings.Instance.SSPEC = false;
            }
        }
        private void Party_CheckedChanged(object sender, EventArgs e)
        {
            if (Party.Checked == true)
            {
                MarksmanSettings.Instance.Party = true;
            }
            else
            {
                MarksmanSettings.Instance.Party = false;
            }
        }
        private void DSHC_CheckedChanged(object sender, EventArgs e)
        {
            if (DSHC.Checked == true)
            {
                MarksmanSettings.Instance.DSHC = true;
            }
            else
            {
                MarksmanSettings.Instance.DSHC = false;
            }
        }
        private void DSLFR_CheckedChanged(object sender, EventArgs e)
        {
            if (DSLFR.Checked == true)
            {
                MarksmanSettings.Instance.DSLFR = true;
            }
            else
            {
                MarksmanSettings.Instance.DSLFR = false;
            }
        }
        private void DSNOR_CheckedChanged(object sender, EventArgs e)
        {
            if (DSNOR.Checked == true)
            {
                MarksmanSettings.Instance.DSNOR = true;
            }
            else
            {
                MarksmanSettings.Instance.DSNOR = false;
            }
        }
        private void CallPet_CheckedChanged(object sender, EventArgs e)
        {
            if (CallPet.Checked == true)
            {
                MarksmanSettings.Instance.CP = true;
            }
            else
            {
                MarksmanSettings.Instance.CP = false;
            }
        }
        private void RevivePet_CheckedChanged(object sender, EventArgs e)
        {
            if (RevivePet.Checked == true)
            {
                MarksmanSettings.Instance.RP = true;
            }
            else
            {
                MarksmanSettings.Instance.RP = false;
            }
        }
        private void MendPet_CheckedChanged(object sender, EventArgs e)
        {
            if (MendPet.Checked == true)
            {
                MarksmanSettings.Instance.MP = true;
            }
            else
            {
                MarksmanSettings.Instance.MP = false;
            }
        }
        private void Launcher_CheckedChanged(object sender, EventArgs e)
        {
            if (Launcher.Checked == true)
            {
                MarksmanSettings.Instance.TL = true;
            }
            else
            {
                MarksmanSettings.Instance.TL = false;
            }
        }
        private void Trinket1_CheckedChanged(object sender, EventArgs e)
        {
            if (Trinket1.Checked == true)
            {
                MarksmanSettings.Instance.T1 = true;
            }
            else
            {
                MarksmanSettings.Instance.T1 = false;
            }
        }
        private void Trinket2_CheckedChanged(object sender, EventArgs e)
        {
            if (Trinket2.Checked == true)
            {
                MarksmanSettings.Instance.T2 = true;
            }
            else
            {
                MarksmanSettings.Instance.T2 = false;
            }
        }
        private void Rapid_CheckedChanged(object sender, EventArgs e)
        {
            if (Rapid.Checked == true)
            {
                MarksmanSettings.Instance.RF = true;
            }
            else
            {
                MarksmanSettings.Instance.RF = false;
            }
        }
        private void Gloves_CheckedChanged(object sender, EventArgs e)
        {
            if (Gloves.Checked == true)
            {
                MarksmanSettings.Instance.GE = true;
            }
            else
            {
                MarksmanSettings.Instance.GE = false;
            }
        }
        private void LifeBlood_CheckedChanged(object sender, EventArgs e)
        {
            if (LifeBlood.Checked == true)
            {
                MarksmanSettings.Instance.LB = true;
            }
            else
            {
                MarksmanSettings.Instance.LB = false;
            }
        }
        private void Racial_CheckedChanged(object sender, EventArgs e)
        {
            if (Racial.Checked == true)
            {
                MarksmanSettings.Instance.RS = true;
            }
            else
            {
                MarksmanSettings.Instance.RS = false;
            }
        }
        private void MultiShot_CheckedChanged(object sender, EventArgs e)
        {
            if (MultiShot.Checked == true)
            {
                MarksmanSettings.Instance.MS = true;
            }
            else
            {
                MarksmanSettings.Instance.MS = false;
            }
        }
        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            MarksmanSettings.Instance.Save();
            Logging.Write("Configuration Saved");
            Close();
        }

        private void SaveButton2_Click_1(object sender, EventArgs e)
        {
            MarksmanSettings.Instance.Save();
            Logging.Write("Configuration Saved");
            Close();
        }

    }
}
