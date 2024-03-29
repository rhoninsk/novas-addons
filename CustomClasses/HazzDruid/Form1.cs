﻿using System;
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

namespace HazzDruid
{
    public partial class HazzDruidConfig : Form
    {
        public HazzDruidConfig()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.Load();
            checkBox1.Checked = HazzDruidSettings.Instance.UseTree;
            checkBox2.Checked = HazzDruidSettings.Instance.UseRebirth;
            checkBox3.Checked = HazzDruidSettings.Instance.UseRevive;
            checkBox4.Checked = HazzDruidSettings.Instance.UseRemoveCurse;
            checkBox5.Checked = HazzDruidSettings.Instance.UseTravel;
            checkBox6.Checked = HazzDruidSettings.Instance.UseLifebloom;
            checkBox7.Checked = HazzDruidSettings.Instance.UseCombat;
            checkBox8.Checked = HazzDruidSettings.Instance.UseTranquility;
            checkBox9.Checked = HazzDruidSettings.Instance.SpecRestoration;
            checkBox10.Checked = HazzDruidSettings.Instance.SpecFeral;
            checkBox11.Checked = HazzDruidSettings.Instance.SpecBalance;
            checkBox12.Checked = HazzDruidSettings.Instance.UseBarkskin;
            checkBox13.Checked = HazzDruidSettings.Instance.UseThorns;
            checkBox14.Checked = HazzDruidSettings.Instance.UseMoTW;
            checkBox15.Checked = HazzDruidSettings.Instance.UseStarsurge;
            checkBox16.Checked = HazzDruidSettings.Instance.UseSwarm;
            checkBox17.Checked = HazzDruidSettings.Instance.UseMoonkin;
            checkBox18.Checked = HazzDruidSettings.Instance.UseMoonfire;
            checkBox19.Checked = HazzDruidSettings.Instance.UseStarfall;
            checkBox20.Checked = HazzDruidSettings.Instance.UseMushroom;
            checkBox21.Checked = HazzDruidSettings.Instance.UseFoN;
            checkBox22.Checked = HazzDruidSettings.Instance.UseSolarBeam;
            checkBox23.Checked = HazzDruidSettings.Instance.UseMoonkinHeal;
            checkBox24.Checked = HazzDruidSettings.Instance.UseBSkullBash;
            checkBox25.Checked = HazzDruidSettings.Instance.UseGrasp;
            checkBox26.Checked = HazzDruidSettings.Instance.UseTyphoon;
            checkBox27.Checked = HazzDruidSettings.Instance.UseMangle;
            checkBox28.Checked = HazzDruidSettings.Instance.UseTigersFury;
            checkBox29.Checked = HazzDruidSettings.Instance.UseSkullBash;
            checkBox30.Checked = HazzDruidSettings.Instance.UseFaerieFire;
            checkBox31.Checked = HazzDruidSettings.Instance.UseBerserk;
            checkBox32.Checked = HazzDruidSettings.Instance.UseShred;
            checkBox33.Checked = HazzDruidSettings.Instance.UseRake;
            checkBox34.Checked = HazzDruidSettings.Instance.UseSavageRoar;
            checkBox35.Checked = HazzDruidSettings.Instance.UseCat;
            checkBox36.Checked = HazzDruidSettings.Instance.UseBear;
            checkBox37.Checked = HazzDruidSettings.Instance.UseSurvivalInstincts;
            checkBox38.Checked = HazzDruidSettings.Instance.UseEnrage;
            checkBox39.Checked = HazzDruidSettings.Instance.UseBMangle;
            checkBox40.Checked = HazzDruidSettings.Instance.UseBBeserk;
            checkBox41.Checked = HazzDruidSettings.Instance.UseMaul;
            checkBox43.Checked = HazzDruidSettings.Instance.UseDemoralizingRoar;
            checkBox44.Checked = HazzDruidSettings.Instance.UseSwipe;
            checkBox42.Checked = HazzDruidSettings.Instance.UseThrash;
            checkBox45.Checked = HazzDruidSettings.Instance.UsePulverize;
            checkBox46.Checked = HazzDruidSettings.Instance.UseBFaerieFire;
            trackBar1.Value = HazzDruidSettings.Instance.SwiftmendPercent;
            trackBar2.Value = HazzDruidSettings.Instance.InnervatePercent;
            trackBar3.Value = HazzDruidSettings.Instance.RejuvenationPercent;
            trackBar4.Value = HazzDruidSettings.Instance.RegrowthPercent;
            trackBar5.Value = HazzDruidSettings.Instance.HealingTouchPercent;
            trackBar6.Value = HazzDruidSettings.Instance.WildGrowthPercent;
            trackBar7.Value = HazzDruidSettings.Instance.NaturesPercent;
            trackBar8.Value = HazzDruidSettings.Instance.HealthPercent;
            trackBar9.Value = HazzDruidSettings.Instance.ManaPercent;
            trackBar10.Value = HazzDruidSettings.Instance.RipCount;
            trackBar11.Value = HazzDruidSettings.Instance.MushroomPercent;
            trackBar12.Value = HazzDruidSettings.Instance.BiteCount;
            trackBar13.Value = HazzDruidSettings.Instance.LacerateCount;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseTree = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseRebirth = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseRevive = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseRemoveCurse = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseTravel = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseLifebloom = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseCombat = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseTranquility = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.SpecRestoration = checkBox9.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.SpecFeral = checkBox10.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.SpecBalance = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBarkskin = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseThorns = checkBox13.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMoTW = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseStarsurge = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSwarm = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMoonkin = checkBox17.Checked;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMoonfire = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseStarfall = checkBox19.Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMushroom = checkBox20.Checked;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseFoN = checkBox21.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSolarBeam = checkBox22.Checked;
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMoonkinHeal = checkBox23.Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBSkullBash = checkBox24.Checked;
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseGrasp = checkBox25.Checked;
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseTyphoon = checkBox26.Checked;
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMangle = checkBox27.Checked;
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseTigersFury = checkBox28.Checked;
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSkullBash = checkBox29.Checked;
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseFaerieFire = checkBox30.Checked;
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBerserk = checkBox31.Checked;
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseShred = checkBox32.Checked;
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseRake = checkBox33.Checked;
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSavageRoar = checkBox34.Checked;
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseCat = checkBox35.Checked;
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBear = checkBox36.Checked;
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSurvivalInstincts = checkBox37.Checked;
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseEnrage = checkBox38.Checked;
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBMangle = checkBox39.Checked;
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBBeserk = checkBox40.Checked;
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseMaul = checkBox41.Checked;
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseThrash = checkBox42.Checked;
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseDemoralizingRoar = checkBox43.Checked;
        }

        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseSwipe = checkBox44.Checked;
        }

        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UsePulverize = checkBox45.Checked;
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.UseBFaerieFire = checkBox46.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.SwiftmendPercent = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.InnervatePercent = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.RejuvenationPercent = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.RegrowthPercent = trackBar4.Value;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.HealingTouchPercent = trackBar5.Value;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.WildGrowthPercent = trackBar6.Value;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.NaturesPercent = trackBar7.Value;
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.HealthPercent = trackBar8.Value;
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.ManaPercent = trackBar9.Value;
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.RipCount = trackBar10.Value;
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.MushroomPercent = trackBar11.Value;
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.BiteCount = trackBar12.Value;
        }

        private void trackBar13_Scroll(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.LacerateCount = trackBar13.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HazzDruidSettings.Instance.Save();
            Close();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }
    }
}
