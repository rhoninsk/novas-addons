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

namespace Dunatanks3
{
    public partial class ding : Form
    {
        public ding()
        {
            InitializeComponent();
        }

        private void cfg_Load(object sender, EventArgs e)
        {
            button2.Image = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\arms.png");
            button3.Image = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\tg.png");
            button4.Image = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\smf.png");
            button5.Image = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\def.png");
            tabPage1.BackgroundImage = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\warrior_1.jpg");
            tabPage1.BackgroundImageLayout = ImageLayout.Stretch;
            tabPage2.BackgroundImage = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\warrior_2.jpg");
            tabPage2.BackgroundImageLayout = ImageLayout.Stretch;
            tabPage3.BackgroundImage = new Bitmap(Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\warrior_3.jpg");
            tabPage3.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox7.ImageLocation = Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\logo.jpg";

            #region Trinkets
            if (StyxWoW.Me.Inventory.GetItemBySlot(12) != null)
            {
                TrinketOneGroupBoxArms.Text = StyxWoW.Me.Inventory.GetItemBySlot(12).Name;
                TrinketOneGroupBoxFury.Text = StyxWoW.Me.Inventory.GetItemBySlot(12).Name;
                TrinketOneGroupBoxProt.Text = StyxWoW.Me.Inventory.GetItemBySlot(12).Name;
            }
            else
            {
                UseTrinketOneArmsOnCd.Checked = false;
                UseTrinketOneArmsOnCd.Enabled = false;
                UseTrinketOneFuryOnCd.Checked = false;
                UseTrinketOneFuryOnCd.Enabled = false;
                UseTrinketOneProtOnCd.Checked = false;
                UseTrinketOneProtOnCd.Enabled = false;
                UseTrinketOneArmsHero.Checked = false;
                UseTrinketOneArmsHero.Enabled = false;
                UseTrinketOneFuryHero.Checked = false;
                UseTrinketOneFuryHero.Enabled = false;
                UseTrinketOneProtHero.Checked = false;
                UseTrinketOneProtHero.Enabled = false;
                UseTrinketOneArmsBelow20.Checked = false;
                UseTrinketOneArmsBelow20.Enabled = false;
                UseTrinketOneFuryBelow20.Checked = false;
                UseTrinketOneFuryBelow20.Enabled = false;
                UseTrinketOneProtBelow20.Checked = false;
                UseTrinketOneProtBelow20.Enabled = false;
                DoNotUseTrinketOneArms.Checked = true;
                DoNotUseTrinketOneArms.Enabled = false;
                DoNotUseTrinketOneFury.Checked = true;
                DoNotUseTrinketOneFury.Enabled = false;
                DoNotUseTrinketOneProt.Checked = true;
                DoNotUseTrinketOneProt.Enabled = false;
                UseTrinketOneArmsCC.Checked = false;
                UseTrinketOneArmsCC.Enabled = false;
                UseTrinketOneFuryCC.Checked = false;
                UseTrinketOneFuryCC.Enabled = false;
                UseTrinketOneProtCC.Checked = false;
                UseTrinketOneProtCC.Enabled = false;
            }
            if (StyxWoW.Me.Inventory.GetItemBySlot(13) != null)
            {
                TrinketTwoGroupBoxArms.Text = StyxWoW.Me.Inventory.GetItemBySlot(13).Name;
                TrinketTwoGroupBoxFury.Text = StyxWoW.Me.Inventory.GetItemBySlot(13).Name;
                TrinketTwoGroupBoxProt.Text = StyxWoW.Me.Inventory.GetItemBySlot(13).Name;
            }
            else
            {
                UseTrinketTwoArmsOnCd.Checked = false;
                UseTrinketTwoArmsOnCd.Enabled = false;
                UseTrinketTwoFuryOnCd.Checked = false;
                UseTrinketTwoFuryOnCd.Enabled = false;
                UseTrinketTwoProtOnCd.Checked = false;
                UseTrinketTwoProtOnCd.Enabled = false;
                UseTrinketTwoArmsHero.Checked = false;
                UseTrinketTwoArmsHero.Enabled = false;
                UseTrinketTwoFuryHero.Checked = false;
                UseTrinketTwoFuryHero.Enabled = false;
                UseTrinketTwoProtHero.Checked = false;
                UseTrinketTwoProtHero.Enabled = false;
                UseTrinketTwoArmsBelow20.Checked = false;
                UseTrinketTwoArmsBelow20.Enabled = false;
                UseTrinketTwoFuryBelow20.Checked = false;
                UseTrinketTwoFuryBelow20.Enabled = false;
                UseTrinketTwoProtBelow20.Checked = false;
                UseTrinketTwoProtBelow20.Enabled = false;
                DoNotUseTrinketTwoArms.Checked = true;
                DoNotUseTrinketTwoArms.Enabled = false;
                DoNotUseTrinketTwoFury.Checked = true;
                DoNotUseTrinketTwoFury.Enabled = false;
                DoNotUseTrinketTwoProt.Checked = true;
                DoNotUseTrinketTwoProt.Enabled = false;
                UseTrinketTwoArmsCC.Checked = false;
                UseTrinketTwoArmsCC.Enabled = false;
                UseTrinketTwoFuryCC.Checked = false;
                UseTrinketTwoFuryCC.Enabled = false;
                UseTrinketTwoProtCC.Checked = false;
                UseTrinketTwoProtCC.Enabled = false;
            }
            #endregion

            DunatanksSettings.Instance.Load();

            if (DisableMovement.Checked == true)
            {
                PullCharge.Checked = false;
                PullCharge.Enabled = false;
                PullHeroicThrow.Checked = false;
                PullHeroicThrow.Enabled = false;
            }
            else
            {
                PullCharge.Enabled = true;
                PullHeroicThrow.Enabled = true;
            }

            // specc
            useArms.Checked = DunatanksSettings.Instance.useArms;
            useFury.Checked = DunatanksSettings.Instance.useFury;
            useProt.Checked = DunatanksSettings.Instance.useProt;
            //shout
            ArmsUseBattleShout.Checked = DunatanksSettings.Instance.ArmsuseBattleShout;
            ArmsUseCommandingShout.Checked = DunatanksSettings.Instance.ArmsuseCommandingShout;
            FuryUseBattleShout.Checked = DunatanksSettings.Instance.FuryuseBattleShout;
            FuryUseCommandingShout.Checked = DunatanksSettings.Instance.FuryuseCommandingShout;
            ProtUseBattleShout.Checked = DunatanksSettings.Instance.ProtuseBattleShout;
            ProtUseCommandingShout.Checked = DunatanksSettings.Instance.ProtuseCommandingShout;
            // healthstone
            ArmsUseHealthStone.Checked = DunatanksSettings.Instance.ArmsuseHealthStone;
            FuryUseHealthStone.Checked = DunatanksSettings.Instance.FuryuseHealthStone;
            ProtUseHealthStone.Checked = DunatanksSettings.Instance.ProtuseHealthStone;
            ArmsHealthStonePercent.Value = new decimal(DunatanksSettings.Instance.ArmsHealthStonePercent);
            FuryHealthStonePercent.Value = new decimal(DunatanksSettings.Instance.FuryHealthStonePercent);
            ProtHealthStonePercent.Value = new decimal(DunatanksSettings.Instance.ProtHealthStonePercent);
            // stances
            ArmsAutoSwitchBattleStance.Checked = DunatanksSettings.Instance.ArmsAutoSwitchBattleStance;
            FuryAutoSwitchBerserkerStance.Checked = DunatanksSettings.Instance.FuryAutoSwitchBerserkerStance;
            ProtAutoSwitchDefensiveStance.Checked = DunatanksSettings.Instance.ProtAutoSwitchDefensiveStance;
            // Fury
            UseTG.Checked = DunatanksSettings.Instance.useTG;
            UseSMF.Checked = DunatanksSettings.Instance.useSMF;
            // Demo Shout
            UseDemoShoutArmsBoss.Checked = DunatanksSettings.Instance.useDemoShoutArmsBoss;
            UseDemoShoutArmsAoE.Checked = DunatanksSettings.Instance.useDemoShoutArmsAoE;
            UseDemoShoutFuryBoss.Checked = DunatanksSettings.Instance.useDemoShoutFuryBoss;
            UseDemoShoutFuryAoE.Checked = DunatanksSettings.Instance.useDemoShoutFuryAoE;
            UseDemoShoutProtBoss.Checked = DunatanksSettings.Instance.useDemoShoutProtBoss;
            UseDemoShoutProtAoE.Checked = DunatanksSettings.Instance.useDemoShoutProtAoE;
            // Pummel
            UsePummelArmsBoss.Checked = DunatanksSettings.Instance.usePummelArmsBoss;
            UsePummelArmsAoE.Checked = DunatanksSettings.Instance.usePummelArmsAoE;
            UsePummelFuryBoss.Checked = DunatanksSettings.Instance.usePummelFuryBoss;
            UsePummelFuryAoE.Checked = DunatanksSettings.Instance.usePummelFuryAoE;
            UsePummelProtBoss.Checked = DunatanksSettings.Instance.usePummelProtBoss;
            UsePummelProtAoE.Checked = DunatanksSettings.Instance.usePummelProtAoE;
            // Victory Rush
            UseVictoryRushArmsBoss.Checked = DunatanksSettings.Instance.useVictoryRushArmsBoss;
            UseVictoryRushArmsAoE.Checked = DunatanksSettings.Instance.useVictoryRushArmsAoE;
            UseVictoryRushFuryBoss.Checked = DunatanksSettings.Instance.useVictoryRushFuryBoss;
            UseVictoryRushFuryAoE.Checked = DunatanksSettings.Instance.useVictoryRushFuryAoE;
            UseVictoryRushProtBoss.Checked = DunatanksSettings.Instance.useVictoryRushProtBoss;
            UseVictoryRushProtAoE.Checked = DunatanksSettings.Instance.useVictoryRushProtAoE;
            // WW
            UseWhirlWindFuryAoE.Checked = DunatanksSettings.Instance.useWhirlWindFuryAoE;
            // Disarm
            UseDisarmProtBoss.Checked = DunatanksSettings.Instance.useDisarmProtBoss;
            UseDisarmProtAoE.Checked = DunatanksSettings.Instance.useDisarmProtAoE;
            // Taunt
            UseTauntProtAoE.Checked = DunatanksSettings.Instance.useTauntProtAoE;
            UseChallengingShoutProtAoE.Checked = DunatanksSettings.Instance.useChallengingShoutProtAoE;
            // ShieldWall
            UseShieldWallProtAoE.Checked = DunatanksSettings.Instance.useShieldWallProtAoE;
            // Vigilance
            UseVigilanceProt.Checked = DunatanksSettings.Instance.useVigilanceProt;
            UseVigilanceOnRandom.Checked = DunatanksSettings.Instance.useVigilanceOnRandom;
            UseVigilanceOnSpecific.Checked = DunatanksSettings.Instance.useVigilanceOnSpecific;
            VigilanceSpecificName.Text = DunatanksSettings.Instance.VigilanceSpecificName;
            // Last Stand
            LastStandPercentBoss.Value = new decimal(DunatanksSettings.Instance.LastStandPercentBoss);
            LastStandPercentAoE.Value = new decimal(DunatanksSettings.Instance.LastStandPercentAoE);
            // Recklessness
            useRecklessnessArmsOnCd.Checked = DunatanksSettings.Instance.useRecklessnessArmsOnCd;
            useRecklessnessArmsBelow20.Checked = DunatanksSettings.Instance.useRecklessnessArmsBelow20;
            useRecklessnessArmsEnrage.Checked = DunatanksSettings.Instance.useRecklessnessArmsEnrage;
            DoNotUseRecklessnessArms.Checked = DunatanksSettings.Instance.DoNotuseRecklessnessArms;
            useRecklessnessFuryOnCd.Checked = DunatanksSettings.Instance.useRecklessnessFuryOnCd;
            useRecklessnessFuryBelow20.Checked = DunatanksSettings.Instance.useRecklessnessFuryBelow20;
            useRecklessnessFuryEnrage.Checked = DunatanksSettings.Instance.useRecklessnessFuryEnrage;
            DoNotUseRecklessnessFury.Checked = DunatanksSettings.Instance.DoNotuseRecklessnessFury;
            UseRecklessnessArmsAoE.Checked = DunatanksSettings.Instance.useRecklessnessArmsAoE;
            useRecklessnessFuryAoE.Checked = DunatanksSettings.Instance.useRecklessnessFuryAoE;
            // Death Wish
            UseDeathWishOnCd.Checked = DunatanksSettings.Instance.useDeathWishOnCd;
            UseDeathWishBelow20.Checked = DunatanksSettings.Instance.useDeathWishBelow20;
            useDeathWishEnrage.Checked = DunatanksSettings.Instance.useDeathWishEnrage;
            DoNotUseDeathWish.Checked = DunatanksSettings.Instance.DoNotuseDeathWish;
            useDeathWishFuryAoE.Checked = DunatanksSettings.Instance.useDeathWishFuryAoE;
            // Bladestorm
            UseBladestormArmsAoE.Checked = DunatanksSettings.Instance.useBladestormArmsAoE;
            UseSweepingStrikesArmsAoE.Checked = DunatanksSettings.Instance.useSweepingStrikesArmsAoE;
            // Deadly Calm
            useDeadlyCalmArmsOnCd.Checked = DunatanksSettings.Instance.useDeadlyCalmArmsOnCd;
            useDeadlyCalmArmsBelow20.Checked = DunatanksSettings.Instance.useDeadlyCalmArmsBelow20;
            useDeadlyCalmArmsEnrage.Checked = DunatanksSettings.Instance.useDeadlyCalmArmsEnrage;
            DoNotUseDeadlyCalm.Checked = DunatanksSettings.Instance.DoNotuseDeadlyCalm;
            // Potions
            ArmsUsePotion.Checked = DunatanksSettings.Instance.ArmsusePotion;
            FuryUsePotion.Checked = DunatanksSettings.Instance.FuryusePotion;
            ProtUsePotion.Checked = DunatanksSettings.Instance.ProtusePotion;
            ArmsPotionPercent.Value = new decimal(DunatanksSettings.Instance.ArmsPotionPercent);
            FuryPotionPercent.Value = new decimal(DunatanksSettings.Instance.FuryPotionPercent);
            ProtPotionPercent.Value = new decimal(DunatanksSettings.Instance.ProtPotionPercent);
            // Stack up Sunder Armor
            StackUpSunderArms.Checked = DunatanksSettings.Instance.StackUpSunderArms;
            StackUpSunderFury.Checked = DunatanksSettings.Instance.StackUpSunderFury;
            // Colossus Smash
            UseColossusSmashOnCdArms.Checked = DunatanksSettings.Instance.UseColossusSmashOnCdArms;
            UseColossusSmashOnCdFury.Checked = DunatanksSettings.Instance.UseColossusSmashOnCdFury;
            UseColossusSmashRageArms.Checked = DunatanksSettings.Instance.UseColossusSmashRageArms;
            UseColossusSmashRageFury.Checked = DunatanksSettings.Instance.UseColossusSmashRageFury;
            ColossusSmashPercentArms.Value = new decimal(DunatanksSettings.Instance.ColossusSmashPercentArms);
            ColossusSmashPercentFury.Value = new decimal(DunatanksSettings.Instance.ColossusSmashPercentFury);
            // Inner Rage
            UseInnerRageArmsAoE.Checked = DunatanksSettings.Instance.UseInnerRageArmsAoE;
            UseInnerRageFuryAoE.Checked = DunatanksSettings.Instance.UseInnerRageFuryAoE;
            PrioCleaveArmsAoE.Checked = DunatanksSettings.Instance.PrioCleaveArmsAoE;
            PrioCleaveFuryAoE.Checked = DunatanksSettings.Instance.PrioCleaveFuryAoE;
            // Trinket One
            UseTrinketOneArmsOnCd.Checked = DunatanksSettings.Instance.UseTrinketOneArmsOnCd;
            UseTrinketOneFuryOnCd.Checked = DunatanksSettings.Instance.UseTrinketOneFuryOnCd;
            UseTrinketOneProtOnCd.Checked = DunatanksSettings.Instance.UseTrinketOneProtOnCd;
            UseTrinketOneArmsBelow20.Checked = DunatanksSettings.Instance.UseTrinketOneArmsBelow20;
            UseTrinketOneFuryBelow20.Checked = DunatanksSettings.Instance.UseTrinketOneFuryBelow20;
            UseTrinketOneProtBelow20.Checked = DunatanksSettings.Instance.UseTrinketOneProtBelow20;
            UseTrinketOneArmsHero.Checked = DunatanksSettings.Instance.UseTrinketOneArmsHero;
            UseTrinketOneFuryHero.Checked = DunatanksSettings.Instance.UseTrinketOneFuryHero;
            UseTrinketOneProtHero.Checked = DunatanksSettings.Instance.UseTrinketOneProtHero;
            DoNotUseTrinketOneArms.Checked = DunatanksSettings.Instance.DoNotUseTrinketOneArms;
            DoNotUseTrinketOneFury.Checked = DunatanksSettings.Instance.DoNotUseTrinketOneFury;
            DoNotUseTrinketOneProt.Checked = DunatanksSettings.Instance.DoNotUseTrinketOneProt;
            // Trinket Two
            UseTrinketTwoArmsOnCd.Checked = DunatanksSettings.Instance.UseTrinketTwoArmsOnCd;
            UseTrinketTwoFuryOnCd.Checked = DunatanksSettings.Instance.UseTrinketTwoFuryOnCd;
            UseTrinketTwoProtOnCd.Checked = DunatanksSettings.Instance.UseTrinketTwoProtOnCd;
            UseTrinketTwoArmsBelow20.Checked = DunatanksSettings.Instance.UseTrinketTwoArmsBelow20;
            UseTrinketTwoFuryBelow20.Checked = DunatanksSettings.Instance.UseTrinketTwoFuryBelow20;
            UseTrinketTwoProtBelow20.Checked = DunatanksSettings.Instance.UseTrinketTwoProtBelow20;
            UseTrinketTwoArmsHero.Checked = DunatanksSettings.Instance.UseTrinketTwoArmsHero;
            UseTrinketTwoFuryHero.Checked = DunatanksSettings.Instance.UseTrinketTwoFuryHero;
            UseTrinketTwoProtHero.Checked = DunatanksSettings.Instance.UseTrinketTwoProtHero;
            DoNotUseTrinketTwoArms.Checked = DunatanksSettings.Instance.DoNotUseTrinketTwoArms;
            DoNotUseTrinketTwoFury.Checked = DunatanksSettings.Instance.DoNotUseTrinketTwoFury;
            DoNotUseTrinketTwoProt.Checked = DunatanksSettings.Instance.DoNotUseTrinketTwoProt;
            // Berserker Rage
            UseBerserkerRageArmsAoE.Checked = DunatanksSettings.Instance.UseBerserkerRageArmsAoE;
            UseBerserkerRageFuryAoE.Checked = DunatanksSettings.Instance.UseBerserkerRageFuryAoE;
            // Flasks
            UseFlaskOfBattle.Checked = DunatanksSettings.Instance.UseFlaskOfBattle;
            UseFlaskOfFlowingWaters.Checked = DunatanksSettings.Instance.UseFlaskOfFlowingWater;
            UseFlaskOfSteelskin.Checked = DunatanksSettings.Instance.UseFlaskOfSteelskin;
            UseFlaskOfTheDraconicMind.Checked = DunatanksSettings.Instance.UseFlaskOfDraconicMind;
            UseFlaskOfTheWinds.Checked = DunatanksSettings.Instance.UseFlaskOfTheWinds;
            UseFlaskOfTitanicStrength.Checked = DunatanksSettings.Instance.UseFlaskOfTitanicStrength;
            DoNotUseFlask.Checked = DunatanksSettings.Instance.DoNotUseFlask;
            // Potions
            UseGolembloodPotionBelow20.Checked = DunatanksSettings.Instance.UseGolembloodPotionBelow20;
            UseGolembloodPotionHero.Checked = DunatanksSettings.Instance.UseGolembloodPotionHeroism;
            DoNotUseGolembloodPotion.Checked = DunatanksSettings.Instance.DoNotUseGolembloodPotion;
            // Movement
            DisableMovement.Checked = DunatanksSettings.Instance.DisableMovement;
            // Pull
            PullHeroicThrow.Checked = DunatanksSettings.Instance.PullHeroicThrow;
            PullCharge.Checked = DunatanksSettings.Instance.PullCharge;
            // Sounds
            UseSounds.Checked = DunatanksSettings.Instance.UseSounds;
            // Combat Distance
            CombatDistance.Value = new decimal(DunatanksSettings.Instance.CombatDistance);
            // Pummel at the end of cast
            usePummelAtEndArms.Checked = DunatanksSettings.Instance.usePummelAtEndArms;
            usePummelAtEndFury.Checked = DunatanksSettings.Instance.usePummelAtEndFury;
            usePummelAtEndProt.Checked = DunatanksSettings.Instance.usePummelAtEndProt;
            // Auto-Target
            useAutoTargetProt.Checked = DunatanksSettings.Instance.useAutoTargetProt;
            // force single target rotation
            forceSingleTargetRotationArms.Checked = DunatanksSettings.Instance.forceSingleTargetRotationArms;
            forceSingleTargetRotationFury.Checked = DunatanksSettings.Instance.forceSingleTargetRotationFury;
            // PvP Trinket
            UseTrinketOneArmsCC.Checked = DunatanksSettings.Instance.useTrinketOneArmsCC;
            UseTrinketTwoArmsCC.Checked = DunatanksSettings.Instance.useTrinketTwoArmsCC;
            UseTrinketOneFuryCC.Checked = DunatanksSettings.Instance.useTrinketOneFuryCC;
            UseTrinketTwoFuryCC.Checked = DunatanksSettings.Instance.useTrinketTwoFuryCC;
            UseTrinketOneProtCC.Checked = DunatanksSettings.Instance.useTrinketOneProtCC;
            UseTrinketTwoProtCC.Checked = DunatanksSettings.Instance.useTrinketTwoProtCC;
            // Hamstring
            useHamstring.Checked = DunatanksSettings.Instance.useHamstring;
            // AoE Pummel
            UsePummelAoEAutoArms.Checked = DunatanksSettings.Instance.usePummelAoEAutoArms;
            UsePummelAoEAutoFury.Checked = DunatanksSettings.Instance.usePummelAoEAutoFury;
            usePummelAoEAutoProt.Checked = DunatanksSettings.Instance.usePummelAoEAutoProt;
            // Intercept
            useInterceptApproachPvP.Checked = DunatanksSettings.Instance.useInterceptApproachPvP;
            useInterceptInterruptPvP.Checked = DunatanksSettings.Instance.useInterceptInterruptPvP;
            DoNotUseIntercept.Checked = DunatanksSettings.Instance.DoNotUseIntercept;
            // Rest
            useRest.Checked = DunatanksSettings.Instance.UseRest;
            RestPercent.Value = new decimal(DunatanksSettings.Instance.RestPercent);

            //Advanced
            // ZA
            chainhealtrash.Checked = DunatanksSettings.Instance.chainhealtrash;
            malafh.Checked = DunatanksSettings.Instance.malafh;
            malahl.Checked = DunatanksSettings.Instance.malahl;
            malahw.Checked = DunatanksSettings.Instance.malahw;
            malapummelfb.Checked = DunatanksSettings.Instance.malapummelfb;
            malareflfb.Checked = DunatanksSettings.Instance.malareflfb;
            malapummelcl.Checked = DunatanksSettings.Instance.malapummelcl;
            malareflcl.Checked = DunatanksSettings.Instance.malareflcl;
            // ZG
            hpvwhisper.Checked = DunatanksSettings.Instance.hpvwhisper;
            hazzapummelwrath.Checked = DunatanksSettings.Instance.hazzapummelwrath;
            hazzareflwrath.Checked = DunatanksSettings.Instance.hazzareflwrath;
            hpktob.Checked = DunatanksSettings.Instance.hpktob;
            hpkpummelsb.Checked = DunatanksSettings.Instance.hpkpummelsb;
            hpkreflsb.Checked = DunatanksSettings.Instance.hpkreflsb;
            zanzilpummelvb.Checked = DunatanksSettings.Instance.zanzilpummelvb;
            zanzilreflvb.Checked = DunatanksSettings.Instance.zanzilreflvb;
            // BoT
            halfussn.Checked = DunatanksSettings.Instance.halfussn;
            felupummelhl.Checked = DunatanksSettings.Instance.felupummelhl;
            felureflhl.Checked = DunatanksSettings.Instance.felureflhl;
            choconversion.Checked = DunatanksSettings.Instance.choconversion;
            chotwisteddevotion.Checked = DunatanksSettings.Instance.chotwisteddevotion;
            // BD
            arcanotronsn.Checked = DunatanksSettings.Instance.arcanotronsn;
            arcanotronpummelaa.Checked = DunatanksSettings.Instance.arcanotronpummelaa;
            arcanotronreflaa.Checked = DunatanksSettings.Instance.arcanotronreflaa;
            maloriakas.Checked = DunatanksSettings.Instance.maloriakas;
            nefarianbn.Checked = DunatanksSettings.Instance.nefarianbn;
            // TotFW
            totfwtrash.Checked = DunatanksSettings.Instance.totfwtrash;
            // FL
            alysrazorf.Checked = DunatanksSettings.Instance.alysrazorf;
            alysrazori.Checked = DunatanksSettings.Instance.alysrazori;
            // HoT
            tyrandes.Checked = DunatanksSettings.Instance.tyrandes;
            queento.Checked = DunatanksSettings.Instance.queento;
            // PvP
            usePvPRota.Checked = DunatanksSettings.Instance.usePvPRota;
            UsePiercingHowlPvP.Checked = DunatanksSettings.Instance.UsePiercingHowlPvP;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // PvP
            DunatanksSettings.Instance.usePvPRota = usePvPRota.Checked;
            DunatanksSettings.Instance.UsePiercingHowlPvP = UsePiercingHowlPvP.Checked;
            // specc
            DunatanksSettings.Instance.useArms = useArms.Checked;
            DunatanksSettings.Instance.useFury = useFury.Checked;
            DunatanksSettings.Instance.useProt = useProt.Checked;
            //shout
            DunatanksSettings.Instance.ArmsuseBattleShout = ArmsUseBattleShout.Checked;
            DunatanksSettings.Instance.ArmsuseCommandingShout = ArmsUseCommandingShout.Checked;
            DunatanksSettings.Instance.FuryuseBattleShout = FuryUseBattleShout.Checked;
            DunatanksSettings.Instance.FuryuseCommandingShout = FuryUseCommandingShout.Checked;
            DunatanksSettings.Instance.ProtuseBattleShout = ProtUseBattleShout.Checked;
            DunatanksSettings.Instance.ProtuseCommandingShout = ProtUseCommandingShout.Checked;
            // healthstone
            DunatanksSettings.Instance.ArmsuseHealthStone = ArmsUseHealthStone.Checked;
            DunatanksSettings.Instance.FuryuseHealthStone = FuryUseHealthStone.Checked;
            DunatanksSettings.Instance.ProtuseHealthStone = ProtUseHealthStone.Checked;
            DunatanksSettings.Instance.ArmsHealthStonePercent = (int)ArmsHealthStonePercent.Value;
            DunatanksSettings.Instance.FuryHealthStonePercent = (int)FuryHealthStonePercent.Value;
            DunatanksSettings.Instance.ProtHealthStonePercent = (int)ProtHealthStonePercent.Value;
            // stances
            DunatanksSettings.Instance.ArmsAutoSwitchBattleStance = ArmsAutoSwitchBattleStance.Checked;
            DunatanksSettings.Instance.FuryAutoSwitchBerserkerStance = FuryAutoSwitchBerserkerStance.Checked;
            DunatanksSettings.Instance.ProtAutoSwitchDefensiveStance = ProtAutoSwitchDefensiveStance.Checked;
            // Fury
            DunatanksSettings.Instance.useTG = UseTG.Checked;
            DunatanksSettings.Instance.useSMF = UseSMF.Checked;
            // Demo Shout
            DunatanksSettings.Instance.useDemoShoutArmsBoss = UseDemoShoutArmsBoss.Checked;
            DunatanksSettings.Instance.useDemoShoutArmsAoE = UseDemoShoutArmsAoE.Checked;
            DunatanksSettings.Instance.useDemoShoutFuryBoss = UseDemoShoutFuryBoss.Checked;
            DunatanksSettings.Instance.useDemoShoutFuryAoE = UseDemoShoutFuryAoE.Checked;
            DunatanksSettings.Instance.useDemoShoutProtBoss = UseDemoShoutProtBoss.Checked;
            DunatanksSettings.Instance.useDemoShoutProtAoE = UseDemoShoutProtAoE.Checked;
            // Pummel
            DunatanksSettings.Instance.usePummelArmsBoss = UsePummelArmsBoss.Checked;
            DunatanksSettings.Instance.usePummelArmsAoE = UsePummelArmsAoE.Checked;
            DunatanksSettings.Instance.usePummelFuryBoss = UsePummelFuryBoss.Checked;
            DunatanksSettings.Instance.usePummelFuryAoE = UsePummelFuryAoE.Checked;
            DunatanksSettings.Instance.usePummelProtBoss = UsePummelProtBoss.Checked;
            DunatanksSettings.Instance.usePummelProtAoE = UsePummelProtAoE.Checked;
            // Victory Rush
            DunatanksSettings.Instance.useVictoryRushArmsBoss = UseVictoryRushArmsBoss.Checked;
            DunatanksSettings.Instance.useVictoryRushArmsAoE = UseVictoryRushArmsAoE.Checked;
            DunatanksSettings.Instance.useVictoryRushFuryBoss = UseVictoryRushFuryBoss.Checked;
            DunatanksSettings.Instance.useVictoryRushFuryAoE = UseVictoryRushFuryAoE.Checked;
            DunatanksSettings.Instance.useVictoryRushProtBoss = UseVictoryRushProtBoss.Checked;
            DunatanksSettings.Instance.useVictoryRushProtAoE = UseVictoryRushProtAoE.Checked;
            // WW
            DunatanksSettings.Instance.useWhirlWindFuryAoE = UseWhirlWindFuryAoE.Checked;
            // Disarm
            DunatanksSettings.Instance.useDisarmProtBoss = UseDisarmProtBoss.Checked;
            DunatanksSettings.Instance.useDisarmProtAoE = UseDisarmProtAoE.Checked;
            // Taunt
            DunatanksSettings.Instance.useTauntProtAoE = UseTauntProtAoE.Checked;
            DunatanksSettings.Instance.useChallengingShoutProtAoE = UseChallengingShoutProtAoE.Checked;
            // ShieldWall
            DunatanksSettings.Instance.useShieldWallProtAoE = UseShieldWallProtAoE.Checked;
            // Vigilance
            DunatanksSettings.Instance.useVigilanceProt = UseVigilanceProt.Checked;
            DunatanksSettings.Instance.useVigilanceOnRandom = UseVigilanceOnRandom.Checked;
            DunatanksSettings.Instance.useVigilanceOnSpecific = UseVigilanceOnSpecific.Checked;
            DunatanksSettings.Instance.VigilanceSpecificName = VigilanceSpecificName.Text;
            // Last Stand
            DunatanksSettings.Instance.LastStandPercentBoss = (int)LastStandPercentBoss.Value;
            DunatanksSettings.Instance.LastStandPercentAoE = (int)LastStandPercentAoE.Value;
            // Recklessness
            DunatanksSettings.Instance.useRecklessnessArmsOnCd = useRecklessnessArmsOnCd.Checked;
            DunatanksSettings.Instance.useRecklessnessArmsBelow20 = useRecklessnessArmsBelow20.Checked;
            DunatanksSettings.Instance.useRecklessnessArmsEnrage = useRecklessnessArmsEnrage.Checked;
            DunatanksSettings.Instance.DoNotuseRecklessnessArms = DoNotUseRecklessnessArms.Checked;
            DunatanksSettings.Instance.useRecklessnessFuryOnCd = useRecklessnessFuryOnCd.Checked;
            DunatanksSettings.Instance.useRecklessnessFuryBelow20 = useRecklessnessFuryBelow20.Checked;
            DunatanksSettings.Instance.useRecklessnessFuryEnrage = useRecklessnessFuryEnrage.Checked;
            DunatanksSettings.Instance.DoNotuseRecklessnessFury = DoNotUseRecklessnessFury.Checked;
            DunatanksSettings.Instance.useRecklessnessArmsAoE = UseRecklessnessArmsAoE.Checked;
            DunatanksSettings.Instance.useRecklessnessFuryAoE = useRecklessnessFuryAoE.Checked;
            // Death Wish
            DunatanksSettings.Instance.useDeathWishOnCd = UseDeathWishOnCd.Checked;
            DunatanksSettings.Instance.useDeathWishBelow20 = UseDeathWishBelow20.Checked;
            DunatanksSettings.Instance.useDeathWishEnrage = useDeathWishEnrage.Checked;
            DunatanksSettings.Instance.DoNotuseDeathWish = DoNotUseDeathWish.Checked;
            DunatanksSettings.Instance.useDeathWishFuryAoE = useDeathWishFuryAoE.Checked;
            // Bladestorm
            DunatanksSettings.Instance.useBladestormArmsAoE = UseBladestormArmsAoE.Checked;
            // Sweeping Strikes
            DunatanksSettings.Instance.useSweepingStrikesArmsAoE = UseSweepingStrikesArmsAoE.Checked;
            // Deadly Calm
            DunatanksSettings.Instance.useDeadlyCalmArmsOnCd = useDeadlyCalmArmsOnCd.Checked;
            DunatanksSettings.Instance.useDeadlyCalmArmsBelow20 = useDeadlyCalmArmsBelow20.Checked;
            DunatanksSettings.Instance.useDeadlyCalmArmsEnrage = useDeadlyCalmArmsEnrage.Checked;
            DunatanksSettings.Instance.DoNotuseDeadlyCalm = DoNotUseDeadlyCalm.Checked;
            // Potions
            DunatanksSettings.Instance.ArmsusePotion = ArmsUsePotion.Checked;
            DunatanksSettings.Instance.FuryusePotion = FuryUsePotion.Checked;
            DunatanksSettings.Instance.ProtusePotion = ProtUsePotion.Checked;
            DunatanksSettings.Instance.ArmsPotionPercent = (int)ArmsPotionPercent.Value;
            DunatanksSettings.Instance.FuryPotionPercent = (int)FuryPotionPercent.Value;
            DunatanksSettings.Instance.ProtPotionPercent = (int)ProtPotionPercent.Value;
            // Stack up Sunder Armor
            DunatanksSettings.Instance.StackUpSunderArms = StackUpSunderArms.Checked;
            DunatanksSettings.Instance.StackUpSunderFury = StackUpSunderFury.Checked;
            // Colossus Smash
            DunatanksSettings.Instance.UseColossusSmashOnCdArms = UseColossusSmashOnCdArms.Checked;
            DunatanksSettings.Instance.UseColossusSmashOnCdFury = UseColossusSmashOnCdFury.Checked;
            DunatanksSettings.Instance.UseColossusSmashRageArms = UseColossusSmashRageArms.Checked;
            DunatanksSettings.Instance.UseColossusSmashRageFury = UseColossusSmashRageFury.Checked;
            DunatanksSettings.Instance.ColossusSmashPercentArms = (int)ColossusSmashPercentArms.Value;
            DunatanksSettings.Instance.ColossusSmashPercentFury = (int)ColossusSmashPercentFury.Value;
            // Inner Rage
            DunatanksSettings.Instance.UseInnerRageArmsAoE = UseInnerRageArmsAoE.Checked;
            DunatanksSettings.Instance.UseInnerRageFuryAoE = UseInnerRageFuryAoE.Checked;
            DunatanksSettings.Instance.PrioCleaveArmsAoE = PrioCleaveArmsAoE.Checked;
            DunatanksSettings.Instance.PrioCleaveFuryAoE = PrioCleaveFuryAoE.Checked;
            // Trinket One
            DunatanksSettings.Instance.UseTrinketOneArmsOnCd = UseTrinketOneArmsOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketOneFuryOnCd = UseTrinketOneFuryOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketOneProtOnCd = UseTrinketOneProtOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketOneArmsBelow20 = UseTrinketOneArmsBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketOneFuryBelow20 = UseTrinketOneFuryBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketOneProtBelow20 = UseTrinketOneProtBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketOneArmsHero = UseTrinketOneArmsHero.Checked;
            DunatanksSettings.Instance.UseTrinketOneFuryHero = UseTrinketOneFuryHero.Checked;
            DunatanksSettings.Instance.UseTrinketOneProtHero = UseTrinketOneProtHero.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketOneArms = DoNotUseTrinketOneArms.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketOneFury = DoNotUseTrinketOneFury.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketOneProt = DoNotUseTrinketOneProt.Checked;
            // Trinket Two
            DunatanksSettings.Instance.UseTrinketTwoArmsOnCd = UseTrinketTwoArmsOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketTwoFuryOnCd = UseTrinketTwoFuryOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketTwoProtOnCd = UseTrinketTwoProtOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketTwoArmsBelow20 = UseTrinketTwoArmsBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketTwoFuryBelow20 = UseTrinketTwoFuryBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketTwoProtBelow20 = UseTrinketTwoProtBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketTwoArmsHero = UseTrinketTwoArmsHero.Checked;
            DunatanksSettings.Instance.UseTrinketTwoFuryHero = UseTrinketTwoFuryHero.Checked;
            DunatanksSettings.Instance.UseTrinketTwoProtHero = UseTrinketTwoProtHero.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketTwoArms = DoNotUseTrinketTwoArms.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketTwoFury = DoNotUseTrinketTwoFury.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketTwoProt = DoNotUseTrinketTwoProt.Checked;
            // Berserker Rage
            DunatanksSettings.Instance.UseBerserkerRageArmsAoE = UseBerserkerRageArmsAoE.Checked;
            DunatanksSettings.Instance.UseBerserkerRageFuryAoE = UseBerserkerRageFuryAoE.Checked;
            // Flasks
            DunatanksSettings.Instance.UseFlaskOfBattle = UseFlaskOfBattle.Checked;
            DunatanksSettings.Instance.UseFlaskOfFlowingWater = UseFlaskOfFlowingWaters.Checked;
            DunatanksSettings.Instance.UseFlaskOfSteelskin = UseFlaskOfSteelskin.Checked;
            DunatanksSettings.Instance.UseFlaskOfDraconicMind = UseFlaskOfTheDraconicMind.Checked;
            DunatanksSettings.Instance.UseFlaskOfTheWinds = UseFlaskOfTheWinds.Checked;
            DunatanksSettings.Instance.UseFlaskOfTitanicStrength = UseFlaskOfTitanicStrength.Checked;
            DunatanksSettings.Instance.DoNotUseFlask = DoNotUseFlask.Checked;
            // Potions
            DunatanksSettings.Instance.UseGolembloodPotionBelow20 = UseGolembloodPotionBelow20.Checked;
            DunatanksSettings.Instance.UseGolembloodPotionHeroism = UseGolembloodPotionHero.Checked;
            DunatanksSettings.Instance.DoNotUseGolembloodPotion = DoNotUseGolembloodPotion.Checked;
            // Movement
            DunatanksSettings.Instance.DisableMovement = DisableMovement.Checked;
            // Pull
            DunatanksSettings.Instance.PullHeroicThrow = PullHeroicThrow.Checked;
            DunatanksSettings.Instance.PullCharge = PullCharge.Checked;
            // Sounds
            DunatanksSettings.Instance.UseSounds = UseSounds.Checked;
            // Combat Distance
            DunatanksSettings.Instance.CombatDistance = (int)CombatDistance.Value;
            // Pummel at the end of cast
            DunatanksSettings.Instance.usePummelAtEndArms = usePummelAtEndArms.Checked;
            DunatanksSettings.Instance.usePummelAtEndFury = usePummelAtEndFury.Checked;
            DunatanksSettings.Instance.usePummelAtEndProt = usePummelAtEndProt.Checked;
            // Auto-Target
            DunatanksSettings.Instance.useAutoTargetProt = useAutoTargetProt.Checked;
            // force single target rotation
            DunatanksSettings.Instance.forceSingleTargetRotationArms = forceSingleTargetRotationArms.Checked;
            DunatanksSettings.Instance.forceSingleTargetRotationFury = forceSingleTargetRotationFury.Checked;
            // PvP Trinket
            DunatanksSettings.Instance.useTrinketOneArmsCC = UseTrinketOneArmsCC.Checked;
            DunatanksSettings.Instance.useTrinketTwoArmsCC = UseTrinketTwoArmsCC.Checked;
            DunatanksSettings.Instance.useTrinketOneFuryCC = UseTrinketOneFuryCC.Checked;
            DunatanksSettings.Instance.useTrinketTwoFuryCC = UseTrinketTwoFuryCC.Checked;
            DunatanksSettings.Instance.useTrinketOneProtCC = UseTrinketOneProtCC.Checked;
            DunatanksSettings.Instance.useTrinketTwoProtCC = UseTrinketTwoProtCC.Checked;
            // Hamstring
            DunatanksSettings.Instance.useHamstring = useHamstring.Checked;
            // AoE Pummel
            DunatanksSettings.Instance.usePummelAoEAutoArms = UsePummelAoEAutoArms.Checked;
            DunatanksSettings.Instance.usePummelAoEAutoFury = UsePummelAoEAutoFury.Checked;
            DunatanksSettings.Instance.usePummelAoEAutoProt = usePummelAoEAutoProt.Checked;
            // Intercept
            DunatanksSettings.Instance.useInterceptApproachPvP = useInterceptApproachPvP.Checked;
            DunatanksSettings.Instance.useInterceptInterruptPvP = useInterceptInterruptPvP.Checked;
            DunatanksSettings.Instance.DoNotUseIntercept = DoNotUseIntercept.Checked;
            // Rest
            DunatanksSettings.Instance.UseRest = useRest.Checked;
            DunatanksSettings.Instance.RestPercent = (int)RestPercent.Value;

            //Advanced
            // ZA
            DunatanksSettings.Instance.chainhealtrash = chainhealtrash.Checked;
            DunatanksSettings.Instance.malafh = malafh.Checked;
            DunatanksSettings.Instance.malahl = malahl.Checked;
            DunatanksSettings.Instance.malahw = malahw.Checked;
            DunatanksSettings.Instance.malapummelfb = malapummelfb.Checked;
            DunatanksSettings.Instance.malareflfb = malareflfb.Checked;
            DunatanksSettings.Instance.malapummelcl = malapummelcl.Checked;
            DunatanksSettings.Instance.malareflcl = malareflcl.Checked;
            // ZG
            DunatanksSettings.Instance.hpvwhisper = hpvwhisper.Checked;
            DunatanksSettings.Instance.hazzapummelwrath = hazzapummelwrath.Checked;
            DunatanksSettings.Instance.hazzareflwrath = hazzareflwrath.Checked;
            DunatanksSettings.Instance.hpktob = hpktob.Checked;
            DunatanksSettings.Instance.hpkpummelsb = hpkpummelsb.Checked;
            DunatanksSettings.Instance.hpkreflsb = hpkreflsb.Checked;
            DunatanksSettings.Instance.zanzilpummelvb = zanzilpummelvb.Checked;
            DunatanksSettings.Instance.zanzilreflvb = zanzilreflvb.Checked;
            // BoT
            DunatanksSettings.Instance.halfussn = halfussn.Checked;
            DunatanksSettings.Instance.felupummelhl = felupummelhl.Checked;
            DunatanksSettings.Instance.felureflhl = felureflhl.Checked;
            DunatanksSettings.Instance.choconversion = choconversion.Checked;
            DunatanksSettings.Instance.chotwisteddevotion = chotwisteddevotion.Checked;
            // BD
            DunatanksSettings.Instance.arcanotronsn = arcanotronsn.Checked;
            DunatanksSettings.Instance.arcanotronpummelaa = arcanotronpummelaa.Checked;
            DunatanksSettings.Instance.arcanotronreflaa = arcanotronreflaa.Checked;
            DunatanksSettings.Instance.maloriakas = maloriakas.Checked;
            DunatanksSettings.Instance.nefarianbn = nefarianbn.Checked;
            // TotFW
            DunatanksSettings.Instance.totfwtrash = totfwtrash.Checked;
            // FL
            DunatanksSettings.Instance.alysrazorf = alysrazorf.Checked;
            DunatanksSettings.Instance.alysrazori = alysrazori.Checked;
            // HoT
            DunatanksSettings.Instance.tyrandes = tyrandes.Checked;
            DunatanksSettings.Instance.queento = queento.Checked;

            DunatanksSettings.Instance.Save();
            Logging.Write("Config saved");
            Close();
        }

        private void UseVigilanceOnRandom_CheckedChanged(object sender, EventArgs e)
        {
            if (UseVigilanceOnRandom.Checked == true)
            {
                VigilanceSpecificName.Enabled = false;
            }
            else
            {
                VigilanceSpecificName.Enabled = true;
            }
        }

        private void useArms_CheckedChanged(object sender, EventArgs e)
        {
            if (useArms.Checked)
            {
                Armsundso.SelectedTab = tabPage1;
            }
            else if (useFury.Checked)
            {
                Armsundso.SelectedTab = tabPage2;
            }
            else if (useProt.Checked)
            {
                Armsundso.SelectedTab = tabPage3;
            }
        }

        private void useFury_CheckedChanged(object sender, EventArgs e)
        {
            if (useArms.Checked)
            {
                Armsundso.SelectedTab = tabPage1;
            }
            else if (useFury.Checked)
            {
                Armsundso.SelectedTab = tabPage2;
            }
            else if (useProt.Checked)
            {
                Armsundso.SelectedTab = tabPage3;
            }
        }

        private void useProt_CheckedChanged(object sender, EventArgs e)
        {
            if (useArms.Checked)
            {
                Armsundso.SelectedTab = tabPage1;
            }
            else if (useFury.Checked)
            {
                Armsundso.SelectedTab = tabPage2;
            }
            else if (useProt.Checked)
            {
                Armsundso.SelectedTab = tabPage3;
            }
        }

        private void DisableMovement_CheckedChanged(object sender, EventArgs e)
        {
            if (DisableMovement.Checked == true)
            {
                PullCharge.Checked = false;
                PullCharge.Enabled = false;
                PullHeroicThrow.Checked = false;
                PullHeroicThrow.Enabled = false;
            }
            else
            {
                PullCharge.Enabled = true;
                PullHeroicThrow.Enabled = true;
                PullHeroicThrow.Checked = true;
            }
        }

        private void useRest_CheckedChanged(object sender, EventArgs e)
        {
            if (useRest.Checked == true)
            {
                RestPercent.Enabled = true;
            }
            else
            {
                RestPercent.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // old
            //Lua.DoString("t,p,a={1,13,22,31,52,63,71,83,91,102,112,122,131,142,153,161,172,182,191,201,2,13,32,71,3,}SetPreviewPrimaryTalentTree(t[1],GetActiveTalentGroup())for i=1,#t do a=t[i]if a<9 then p=a else AddPreviewTalentPoints(p,floor(a/10),a%10)end end");
            Lua.DoString("t,p,a={1,13,32,42,52,63,83,91,102,122,131,142,153,161,172,182,191,201,2,23,32,42,71,3,}SetPreviewPrimaryTalentTree(t[1],GetActiveTalentGroup())for i=1,#t do a=t[i]if a<9 then p=a else AddPreviewTalentPoints(p,floor(a/10),a%10)end end");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lua.DoString("t,p,a={2,23,32,42,52,62,83,91,103,121,131,141,162,172,183,192,201,1,13,22,63,3,12,}SetPreviewPrimaryTalentTree(t[1],GetActiveTalentGroup())for i=1,#t do a=t[i]if a<9 then p=a else AddPreviewTalentPoints(p,floor(a/10),a%10)end end");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Lua.DoString("t,p,a={2,23,32,42,52,62,83,91,103,121,131,141,162,172,183,192,211,1,13,22,63,3,12,}SetPreviewPrimaryTalentTree(t[1],GetActiveTalentGroup())for i=1,#t do a=t[i]if a<9 then p=a else AddPreviewTalentPoints(p,floor(a/10),a%10)end end");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lua.DoString("t,p,a={3,23,32,41,53,62,72,81,91,102,111,122,131,152,161,172,182,193,201,1,11,22,2,13,32,71,}SetPreviewPrimaryTalentTree(t[1],GetActiveTalentGroup())for i=1,#t do a=t[i]if a<9 then p=a else AddPreviewTalentPoints(p,floor(a/10),a%10)end end");
        }




    }
}
