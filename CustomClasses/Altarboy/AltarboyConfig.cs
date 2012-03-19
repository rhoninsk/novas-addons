using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
using CommonBehaviors.Actions;


namespace Altarboy
{
    public partial class AltarboyConfig : Form
    {
        public AltarboyConfig()
        {
            InitializeComponent();
           if (File.Exists(Logging.ApplicationPath + @"\CustomClasses\Altarboy\utils\altarboy.png"))
            {
                mastHead.ImageLocation = Logging.ApplicationPath + @"\CustomClasses\Altarboy\utils\altarboy.png";
            } 
          
            this.Icon =
                new System.Drawing.Icon(Logging.ApplicationPath + @"\CustomClasses\Altarboy\utils\priest_icon.ico");
            

            lblVersion.Text = Priest.Version + " - " + Priest.Credits;

            LoadMobs();
            
           cmbPotionNames.DataSource = new BindingSource(AltarboyHashes.Potions, null);
           cmbPotionNames.DisplayMember = "Value";
           cmbPotionNames.ValueMember = "Key"; 
            if(AltarboySettings.Instance.FavouritePotionID > 0)
            {
                cmbPotionNames.SelectedValue = AltarboySettings.Instance.FavouritePotionID;

            }
          
        }

        private void AltarboyConfig_Load(object sender, EventArgs e)
        {
            AltarboySettings.Instance.Load();

            chkEnableTargeting.Checked = AltarboySettings.Instance.EnableTargeting;
            chkEnableMindSearTargeting.Checked = AltarboySettings.Instance.EnableMindSearTargeting;
            chkEnableMultiDotTargeting.Checked = AltarboySettings.Instance.EnableMultiDotTargeting;
            chkRafLeadertTargeting.Checked = AltarboySettings.Instance.RaFLeaderTargeting;


            chkAutoShield.Checked = AltarboySettings.Instance.AutoShield;
            chkMindSpikeWithFiend.Checked = AltarboySettings.Instance.UseMindSpikeRotationWithShadowFiend;
            chkUsefiendOnArchAngel.Checked = AltarboySettings.Instance.ForceShadowFiendOnArchangel;


            chkFadeOnAggro.Checked = AltarboySettings.Instance.EnableFadeOnAggro;
            chkLevitateWhileFalling.Checked = AltarboySettings.Instance.LevitateWhileFalling;

            radInnerWill.Checked = AltarboySettings.Instance.UseInnerWill;
            radInnerFire.Checked = AltarboySettings.Instance.UseInnerFire;
            chkInnerWillWhileResting.Checked = AltarboySettings.Instance.ChangeInnerSpellWhileResting; 
            chkEatAndDrinkWhileResting.Checked = AltarboySettings.Instance.AllowFoodAndDrinkWhileResting; 

            chkUseFortitude.Checked = AltarboySettings.Instance.BuffFortitude;
            chkBuffShadowProtection.Checked = AltarboySettings.Instance.BuffShadowProtection;

            chkUseShadowform.Checked = AltarboySettings.Instance.UseShadowForm;
            chkForceShadowFormInCombat.Checked = AltarboySettings.Instance.ForceShadowFormInCombat;
            chkVampiricEmbrace.Checked = AltarboySettings.Instance.UseVampiricEmbrace;

            numShadowFiendManaPercent.Value = (int)AltarboySettings.Instance.UseShadowFiendManaPercent;
            radUseShadowFiendOnCD.Checked =  AltarboySettings.Instance.UseShadowFiendOnCD ;
            radUseShadowFiendForManaRegen.Checked = AltarboySettings.Instance.UseShadowFiendForManaRegen;
            chkForceFiendOnBloodLust.Checked =  AltarboySettings.Instance.ForceFiendOnBloodlust ;

            chkUseMindBlastOnCD.Checked = AltarboySettings.Instance.UseMindBlastOnCD;

            radShadowWordDeathManaRegen.Checked = AltarboySettings.Instance.ShadowWordDeathForManaOnly;
            radShadowWordDeathNever.Checked = AltarboySettings.Instance.ShadowWordDeathNever;
            radShadowWordDeathOnCD.Checked = AltarboySettings.Instance.ShadowWordDeathOnCD;
            numShadowWordDeathManaPercent.Value = (int)AltarboySettings.Instance.ShadowWordDeathManaPercent;
            chkShadowWordDeathExecute.Checked = AltarboySettings.Instance.ShadowWordDeathForExecute;
            chkShadowWordDeathCheckDebuffs.Checked = AltarboySettings.Instance.ShadowWordDeathCheckForDebuffs;
            chkSWDWhileMoving.Checked = AltarboySettings.Instance.ShadowWordDeathWhileMoving;
        
      
            chkUseDispersionForManaRegen.Checked = AltarboySettings.Instance.UseDispersionForManaRegen;
            numDispersionManaPercent.Value = (int)AltarboySettings.Instance.DispersionManaPercent;

            chkAllowMindSear.Checked = AltarboySettings.Instance.UseMindSear;
            numMindSearCount.Value = AltarboySettings.Instance.MindSearHostileCount;

            txtSpellClipping.Text = AltarboySettings.Instance.ClippingDuration.ToString();
            txtCooldownDuration.Text = AltarboySettings.Instance.CoolDownDuration.ToString();
            chkClipMindFlay.Checked = AltarboySettings.Instance.ClipMindFlay;

            if (StyxWoW.Me.Inventory.GetItemBySlot(12) != null)
            {
                Trinket1Label.Text = StyxWoW.Me.Inventory.GetItemBySlot(12).Name;
                radTrinket1NeverUse.Checked = AltarboySettings.Instance.Trinket1NeverUse;
                radTrinket1UseOnCD.Checked = AltarboySettings.Instance.Trinket1UseOnCD;
                radTrinket1UseWithArchangel.Checked = AltarboySettings.Instance.Trinket1UseWithArchangel;
            }
            else
            {
                radTrinket1NeverUse.Checked = false;
                radTrinket1UseOnCD.Checked = false;
                radTrinket1UseWithArchangel.Checked = false;
                radTrinket1NeverUse.Enabled  = false;
                radTrinket1UseOnCD.Enabled = false;
                radTrinket1UseWithArchangel.Enabled = false;
            }

            if (StyxWoW.Me.Inventory.GetItemBySlot(13) != null)
            {
                Trinket2Label.Text = StyxWoW.Me.Inventory.GetItemBySlot(13).Name;
                radTrinket2NeverUse.Checked = AltarboySettings.Instance.Trinket2NeverUse;
                radTrinket2UseOnCD.Checked = AltarboySettings.Instance.Trinket2UseOnCD;
                radTrinket2UseWithArchangel.Checked = AltarboySettings.Instance.Trinket2UseWithArchangel;
            }
            else
            {
                radTrinket2NeverUse.Checked = false;
                radTrinket2UseOnCD.Checked = false;
                radTrinket2UseWithArchangel.Checked = false;
                radTrinket2NeverUse.Enabled = false;
                radTrinket2UseOnCD.Enabled = false;
                radTrinket2UseWithArchangel.Enabled = false;
            }

            //Potions
            chkUsePotionsOnLust.Checked = AltarboySettings.Instance.UsePotionsOnLust;
            
            //Enginnering
            chkUseGloves.Checked = AltarboySettings.Instance.EngUseGloves;
            chkUseBelt.Checked = AltarboySettings.Instance.EngUseBelt;
            chkTinkerGlovesWithArchangel.Checked = AltarboySettings.Instance.EngUseGlovesWithArchangel;

      

        }

        private void SaveSettings() {

            AltarboySettings.Instance.AutoShield  = chkAutoShield.Checked;
            AltarboySettings.Instance.UseMindSpikeRotationWithShadowFiend = chkMindSpikeWithFiend.Checked;
            AltarboySettings.Instance.ForceShadowFiendOnArchangel = chkUsefiendOnArchAngel.Checked;


          AltarboySettings.Instance.EnableTargeting = chkEnableTargeting.Checked;
          AltarboySettings.Instance.EnableMindSearTargeting = chkEnableMindSearTargeting.Checked ;
          AltarboySettings.Instance.EnableMultiDotTargeting = chkEnableMultiDotTargeting.Checked;
        //    AltarboySettings.Instance.PrioritizeRaidMarkerTargeting = chkPrioritizeRaidTargets.Checked;
            AltarboySettings.Instance.RaFLeaderTargeting  = chkRafLeadertTargeting.Checked;

          //Fade Settings
          AltarboySettings.Instance.EnableFadeOnAggro = chkFadeOnAggro.Checked ;
         
        //Levitate
        AltarboySettings.Instance.LevitateWhileFalling = chkLevitateWhileFalling.Checked;

          //Inner Settings
          AltarboySettings.Instance.UseInnerWill  = radInnerWill.Checked ;
          AltarboySettings.Instance.UseInnerFire  = radInnerFire.Checked ;
          AltarboySettings.Instance.ChangeInnerSpellWhileResting = chkInnerWillWhileResting.Checked;
          AltarboySettings.Instance.AllowFoodAndDrinkWhileResting = chkEatAndDrinkWhileResting.Checked;

          //Fortitude
          AltarboySettings.Instance.BuffFortitude = chkUseFortitude.Checked;
       
          //Shadow Protection
          AltarboySettings.Instance.BuffShadowProtection = chkBuffShadowProtection.Checked ;
       

          // Shadowfrom / Vampiric Embrace
          AltarboySettings.Instance.UseShadowForm = chkUseShadowform.Checked;
          AltarboySettings.Instance.ForceShadowFormInCombat = chkForceShadowFormInCombat.Checked;
          AltarboySettings.Instance.UseVampiricEmbrace = chkVampiricEmbrace.Checked;

          //Shadowfiend settings
          AltarboySettings.Instance.UseShadowFiendOnCD = radUseShadowFiendOnCD.Checked;
          AltarboySettings.Instance.UseShadowFiendForManaRegen = radUseShadowFiendForManaRegen.Checked;
          AltarboySettings.Instance.UseShadowFiendManaPercent = (int)numShadowFiendManaPercent.Value;
          AltarboySettings.Instance.ForceFiendOnBloodlust = chkForceFiendOnBloodLust.Checked;
          
          //MindBlast settings
          AltarboySettings.Instance.UseMindBlastOnCD = chkUseMindBlastOnCD.Checked;



         //Shadow Word: Death Settings
          AltarboySettings.Instance.ShadowWordDeathForManaOnly = radShadowWordDeathManaRegen.Checked;
          AltarboySettings.Instance.ShadowWordDeathNever = radShadowWordDeathNever.Checked;
          AltarboySettings.Instance.ShadowWordDeathOnCD =radShadowWordDeathOnCD.Checked ;
          AltarboySettings.Instance.ShadowWordDeathManaPercent = (int)numShadowWordDeathManaPercent.Value;
          AltarboySettings.Instance.ShadowWordDeathForExecute = chkShadowWordDeathExecute.Checked ;
          AltarboySettings.Instance.ShadowWordDeathCheckForDebuffs = chkShadowWordDeathCheckDebuffs.Checked;
          AltarboySettings.Instance.ShadowWordDeathWhileMoving = chkSWDWhileMoving.Checked; 

            //Mind Sear

          AltarboySettings.Instance.UseMindSear = chkAllowMindSear.Checked;
          AltarboySettings.Instance.MindSearHostileCount = (int)numMindSearCount.Value;

          //Dispersion Settings
           AltarboySettings.Instance.UseDispersionForManaRegen = chkUseDispersionForManaRegen.Checked;
           AltarboySettings.Instance.DispersionManaPercent = (int)numDispersionManaPercent.Value;


           AltarboySettings.Instance.ClippingDuration = System.Convert.ToDouble(txtSpellClipping.Text);
           AltarboySettings.Instance.CoolDownDuration = System.Convert.ToDouble(txtCooldownDuration.Text);
            AltarboySettings.Instance.ClipMindFlay = chkClipMindFlay.Checked;

            //Trinkets
           AltarboySettings.Instance.Trinket1NeverUse = radTrinket1NeverUse.Checked;
           AltarboySettings.Instance.Trinket1UseOnCD = radTrinket1UseOnCD.Checked;
           AltarboySettings.Instance.Trinket1UseWithArchangel = radTrinket1UseWithArchangel.Checked;

           AltarboySettings.Instance.Trinket2NeverUse = radTrinket2NeverUse.Checked;
           AltarboySettings.Instance.Trinket2UseOnCD = radTrinket2UseOnCD.Checked;
           AltarboySettings.Instance.Trinket2UseWithArchangel = radTrinket2UseWithArchangel.Checked;

            //Engineering
           AltarboySettings.Instance.EngUseGloves = chkUseGloves.Checked;
           AltarboySettings.Instance.EngUseBelt = chkUseBelt.Checked;
           AltarboySettings.Instance.EngUseGlovesWithArchangel = chkTinkerGlovesWithArchangel.Checked ;

           //Potions
          AltarboySettings.Instance.UsePotionsOnLust  = chkUsePotionsOnLust.Checked ;
          AltarboySettings.Instance.FavouritePotionID = Convert.ToInt32(cmbPotionNames.SelectedValue);

          AltarboySettings.Instance.Save();
          Altarboy.Logger.Log("Altarboy settings saved.");
        }

        private void LoadPotions()
        {
            cmbPotionNames.DataSource  = AltarboyHashes.Potions;
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();           
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void LoadMobs()
        {
            mobGrid.AutoGenerateColumns = false;
            mobGrid.DataSource = TargetManager.AltarboySpecialTargets;
        }

        private void forumLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/classes/priest/34368-altarboy-v1-0-priest-cc-shadow.html");
        }

        private void btnAddMobToList_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me.CurrentTarget != null || StyxWoW.Me.CurrentTarget.IsPlayer )
            {
           
                var newTarget = new SpecialTarget { 
                    TargetName = StyxWoW.Me.CurrentTarget.Name ,
                    TargetId = StyxWoW.Me.CurrentTarget.Entry, 
                };

                //Don't create a new record if there is one.
                if (!TargetManager.AltarboySpecialTargets.Any(tar => tar.TargetId == newTarget.TargetId))
                {
                    TargetManager.AltarboySpecialTargets.Add(newTarget);
                    TargetManager.SaveSpecialTargets();
                    Logger.Log("Current target entry - (" + newTarget.TargetId + ") " + newTarget.TargetName);
                } 

            }
        }

        //private void mobGrid_SelectionChanged(object sender, EventArgs e)
        //{
        //   if(mobGrid.SelectedRows.Count > 0)
        //   {
        //        int index = mobGrid.SelectedRows[0].Index; 
        //        uint targetID = (uint)mobGrid["MobID", index].Value;
        //        lblTargetName.Text = TargetManager.AltarboySpecialTargets.First(tar => tar.TargetId == targetID).TargetName;
        //   }
        //}

        private void mobGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            TargetManager.SaveSpecialTargets();
            //SaveSettings();  
        }

        private void mobGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (mobGrid.IsCurrentCellDirty)
            {
                mobGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (mobGrid.SelectedRows.Count > 0)
            {
                int index = mobGrid.SelectedRows[0].Index;
                var tarID = (uint)mobGrid["MobID", index].Value;
                if (tarID > 0)
                {
                    var Target = TargetManager.AltarboySpecialTargets.First(tar => tar.TargetId == tarID);
                    if (Target != null)
                    {
                         TargetManager.AltarboySpecialTargets.Remove(Target);
                        TargetManager.SaveSpecialTargets();
                    }
                   
                }

            }
        }

      
        // Credits FPSWARE!
        public int MouPosValue(double mousePosition, double progressBarWidth)
        {
            if (mousePosition < 0) mousePosition = 0;
            if (mousePosition > progressBarWidth) mousePosition = progressBarWidth;

            double ratio = mousePosition / progressBarWidth;
            double value = ratio * 100;

            if (value > 100) value = 100;
            if (value < 0) value = 0;

            return (int)Math.Ceiling(value);
        }      
        
    }
}
