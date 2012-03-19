using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Timers;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.BehaviorTree;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;


using CommonBehaviors.Actions;

using TreeSharp;
using Action = TreeSharp.Action;



namespace Dunatanks3
{
    public partial class Warrior : CombatRoutine
    {

        public override sealed string Name { get { return "Dunatank's Warrior CC v3.0a BT"; } }
        public override WoWClass Class { get { return WoWClass.Warrior; } }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }


        #region Buttons
        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            Dunatanks3.ding cfg = new Dunatanks3.ding();
            cfg.ShowDialog();
        }
        #endregion

        #region Pulse
        public override void Pulse()
        {
            if (IsCrowdControlled(Me) && (!DunatanksSettings.Instance.useTrinketOneArmsCC || (DunatanksSettings.Instance.useTrinketOneArmsCC && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown > 0)) && (!DunatanksSettings.Instance.useTrinketTwoArmsCC || (DunatanksSettings.Instance.useTrinketTwoArmsCC && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown > 0)) &&
(!DunatanksSettings.Instance.useTrinketOneFuryCC || (DunatanksSettings.Instance.useTrinketOneFuryCC && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown > 0)) &&
(!DunatanksSettings.Instance.useTrinketTwoFuryCC || (DunatanksSettings.Instance.useTrinketTwoFuryCC && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown > 0)) &&
(!DunatanksSettings.Instance.useTrinketOneProtCC || (DunatanksSettings.Instance.useTrinketOneProtCC && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown > 0)) &&
(!DunatanksSettings.Instance.useTrinketTwoProtCC || (DunatanksSettings.Instance.useTrinketTwoProtCC && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown > 0)))
            {
                CreateBuffCheckAndCast("Berserker Rage");
            }
            LowHealth();
            Shout();
            StanceCheck();
            MoveToTargetProper2();
            UseFlask();
            CSCDGreaterFive();
            UseGBPotion();
            Vigilance();
            AutoTankTargeting();
            if (NeedTankTargeting && (Me.IsInParty || Me.IsInRaid))
            {
                TankTargeting2.Instance.Pulse();
            }
            //detectAdds();
            //Logging.Write(Color.Green, PvPTargets.Count.ToString());
        }
        #endregion

        #region Add Detection
        //Credit to CodeNameGamma for detectAdds code
        private List<WoWUnit> detectAdds()
            {
                List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                            unit.Guid != Me.Guid &&
                            unit.Distance < DunatanksSettings.Instance.CombatDistance &&
                            unit.IsAlive &&
                            (unit.Combat || unit.Name == "Training Dummy" || unit.Name == "Raider's Training Dummy" || unit.IsTotem) &&
                            //(unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember || unit.IsTargetingMeOrPet) &&
                            //(!unit.IsFriendly || Me.MapName == "Blade's Edge Arena" || Me.MapName == "Dalaran Arena" || Me.MapName == "Nagrand Arena" || Me.MapName == "Ruins of Lordaeron" || Me.MapName == "The Ring of Valor") &&
                            !unit.IsPet &&
                            !Styx.Logic.Blacklist.Contains(unit.Guid));

                return addList;
            }
        #endregion

        #region Rest
        public override bool NeedRest
        {
            get
            {
                if (Me.HealthPercent <= DunatanksSettings.Instance.RestPercent && DunatanksSettings.Instance.UseRest)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public override void Rest()
        {
            if (Me.HealthPercent <= DunatanksSettings.Instance.RestPercent && DunatanksSettings.Instance.UseRest)
            {
                Styx.Logic.Common.Rest.Feed();
            }

        }
        #endregion

        #region aoecast
        private List<WoWUnit> AoECastingAdds()
        {
                List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                            unit.Guid != Me.Guid &&
                            unit.Distance < DunatanksSettings.Instance.CombatDistance &&
                            unit.IsAlive &&
                            unit.Combat &&
                            unit.IsCasting &&    //(unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember || unit.IsTargetingMeOrPet) &&
                            !unit.IsFriendly &&
                            !unit.IsPet &&
                            !Styx.Logic.Blacklist.Contains(unit.Guid));

                return addList;
        }
        #endregion

        #region CastSpell Method
        // Credit to Apoc for the below CastSpell code
        // Used for calling CastSpell in the Combat Rotation
        public bool CastSpell(string spellName)
        {
            if (SpellManager.CanCast(spellName))
            {
                SpellManager.Cast(spellName);
                // We managed to cast the spell, so return true, saying we were able to cast it.
                return true;
            }
            // Can't cast the spell right now, so return false.
            return false;
        }
        #endregion

        #region Shout
        public bool Shout()
        {
            if (DunatanksSettings.Instance.useArms == true)
            {
                if (DunatanksSettings.Instance.ArmsuseCommandingShout == true)
                {
                    if (!Me.Auras.ContainsKey("Commanding Shout") && !Me.Auras.ContainsKey("Power Word: Fortitude") && !Me.Auras.ContainsKey("Blood Pact") && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                }
                else if (DunatanksSettings.Instance.ArmsuseBattleShout == true)
                {
                    if (!Me.Auras.ContainsKey("Battle Shout") && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                }
            }
            else if (DunatanksSettings.Instance.useFury)
            {
                if (DunatanksSettings.Instance.FuryuseCommandingShout == true)
                {
                    if (!Me.Auras.ContainsKey("Commanding Shout") && !Me.Auras.ContainsKey("Power Word: Fortitude") && !Me.Auras.ContainsKey("Blood Pact") && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                }
                else if (DunatanksSettings.Instance.FuryuseBattleShout == true)
                {
                    if (!Me.Auras.ContainsKey("Battle Shout") && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                }
            }
            else if (DunatanksSettings.Instance.useProt)
            {
                if (DunatanksSettings.Instance.ProtuseCommandingShout == true)
                {
                    if (!Me.Auras.ContainsKey("Commanding Shout") && !Me.Auras.ContainsKey("Power Word: Fortitude") && !Me.Auras.ContainsKey("Blood Pact") && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Commanding Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Commanding Shout <--");
                        }
                        return true;
                    }
                }
                else if (DunatanksSettings.Instance.ProtuseBattleShout)
                {
                    if (!Me.Auras.ContainsKey("Battle Shout") && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                    else if (Me.Combat && Me.CurrentRage < 40 && !Me.Mounted)
                    {
                        if (CastSpell("Battle Shout") == true)
                        {
                            Logging.Write(Color.Lime, "--> Battle Shout <--");
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region StanceCheck
        public bool StanceCheck()
        {
            if (DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.ProtAutoSwitchDefensiveStance == true && ChargePull().IsRunning == false)
            {
                if (!Me.HasAura("Defensive Stance")) //If we don't have Defensive Stance active, we will switch
                {
                    if (CastSpell("Defensive Stance") == true)
                    {
                        Logging.Write(Color.Lime, "--> Defensive Stance <--");
                    }
                    return true;
                }
                return false;
            }
            else if (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.FuryAutoSwitchBerserkerStance == true && ChargePull().IsRunning == false)
            {
                if (!Me.HasAura("Berserker Stance")) //If we don't have Berserker Stance active, we will switch
                {
                    if (CastSpell("Berserker Stance") == true)
                    {
                        Logging.Write(Color.Lime, "--> Berserker Stance <--");
                    }
                    return true;
                }
                return false;
            }
            else if (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.ArmsAutoSwitchBattleStance == true)
            {
                if (!Me.HasAura("Battle Stance")) //If we don't have Battle Stance active, we will switch
                {
                    if (CastSpell("Battle Stance") == true)
                    {
                        Logging.Write(Color.Lime, "--> Battle Stance <--");
                    }
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion

        #region Vigilance
        public string playername;
        public WoWUnit bufftarget;
        public bool Vigilance()
        {
            if (DunatanksSettings.Instance.useProt == true)
            {
                if (DunatanksSettings.Instance.useVigilanceProt == true)
            {
                if (Me.IsInParty || Me.IsInRaid) //Trying to buff Vigilance on partymember #2
                {
                    if (DunatanksSettings.Instance.useVigilanceOnRandom)
                    {
                        if (Me.PartyMember2 != null)
                        {
                            if (!Me.PartyMember2.HasAura("Vigilance") && Me.PartyMember2.Dead == false && Me.PartyMember2.Distance <= 20 && !Me.Mounted)
                            {
                                if (SpellManager.CanBuff("Vigilance", Me.PartyMember2) == true)
                                {
                                    if (SpellManager.Buff("Vigilance", Me.PartyMember2) == true)
                                    {
                                        Logging.Write(Color.Lime, "--> Vigilance <-- on " + Me.PartyMember2.Name);
                                    }
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            if (Me.PartyMember1 != null && !Me.PartyMember1.HasAura("Vigilance") && Me.PartyMember1.Dead == false && Me.PartyMember1.Distance <= 20 && !Me.Mounted)
                            {
                                if (SpellManager.CanBuff("Vigilance", Me.PartyMember1) == true)
                                {
                                    if (SpellManager.Buff("Vigilance", Me.PartyMember1) == true)
                                    {
                                        Logging.Write(Color.Lime, "--> Vigilance <-- on " + Me.PartyMember1.Name);
                                    }
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                    else
                    {
                        playername = DunatanksSettings.Instance.VigilanceSpecificName;
                        if (playername != "")
                        {
                            bufftarget = ObjectManager.GetObjectsOfType<WoWPlayer>().Where(Player => Player.Name == playername).FirstOrDefault();
                            if (bufftarget != null)
                            {
                                if (bufftarget.IsFriendly && bufftarget.IsPlayer)
                                {
                                    if (!bufftarget.HasAura("Vigilance") && bufftarget.Dead == false && bufftarget.Distance <= 20 && !Me.Mounted)
                                    {
                                        if (SpellManager.CanBuff("Vigilance", bufftarget))
                                        {
                                            if (SpellManager.Buff("Vigilance", bufftarget) == true)
                                            {
                                                Logging.Write(Color.Lime, "--> Vigilance <-- on " + playername);
                                            }
                                            return false;
                                        }
                                        return true;
                                    }
                                    return false;
                                }
                                return false;
                            }
                            else
                            {
                                Logging.Write(Color.Orange, "WARNING! Your desired --> Vigilance <-- target \"" + playername + "\" could not be found! Are you sure it is correct?");
                            }
                        }

                        else
                        {
                            Logging.Write(Color.Orange, "WARNING: You did not define a target to buff Vigilance on! Can't buff Vigilance!");
                            return false;
                        }
                    }
                }
                else
                {
                    //Logging.Write(Color.White, "We are not in a group or in a raid - neglecting \"Vigilance\" for now!");
                    return false;
                }
            }
            return true;
        }
        return true;
        }
        #endregion

        #region Enraged Regen
        protected Composite LowHealth()
        {
            return new PrioritySelector(
            new Decorator(ret => !StyxWoW.Me.Combat || StyxWoW.Me.Mounted || StyxWoW.Me.HealthPercent >= DunatanksSettings.Instance.LastStandPercentBoss,
                new ActionIdle()),
                                CreateBuffCheckAndCast("Berserker Rage", ret => DunatanksSettings.Instance.useProt && !StyxWoW.Me.HasAura("Enrage") && StyxWoW.Me.HealthPercent <= DunatanksSettings.Instance.LastStandPercentBoss),
                                CreateBuffCheckAndCast("Enraged Regeneration", ret => DunatanksSettings.Instance.useProt && StyxWoW.Me.HasAura("Enrage") || StyxWoW.Me.HasAura("Berserker Rage"))
                                );
        }
        #endregion

        #region Combat Buffs
        public override bool NeedCombatBuffs
        {
            get
            {
                Shout();
                StanceCheck();
                Vigilance();
                return false;
            }
        }

        public override void CombatBuff()
        {
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            Logging.Write(Color.Lime, "--> Dunatank's Warrior CC v3.0a BT <-- will do the job for you!");
            Logging.Write(Color.Lime, "created by Wownerds!");
            if (DunatanksSettings.Instance.UseSounds == true)
            {
                System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer();
                myPlayer.SoundLocation = Logging.ApplicationPath + @"\CustomClasses\Dunatanks Warrior CC v3.0a BT\utils\warriors.wav";
                myPlayer.Play();
            }
            CharacterSettings.Instance.PullDistance = 24;
            wtCharge.Reset();
        }
        #endregion

        #region ColossusSmash
        private WaitTimer wtColossusSmash = new WaitTimer(System.TimeSpan.FromSeconds(20));
        public bool CSCDGreaterFive()
        {
            if (wtColossusSmash.TimeLeft.TotalSeconds > 5)
            {
                //Logging.Write(Color.Orchid, "CSCD > 5");
                return true;
            }
            else if (wtColossusSmash.TimeLeft.TotalSeconds > 0)
            {
                //Logging.Write(Color.Orchid, "CSCD < 5");
                return false;
            }
            else
            {
                return false;
            }
        }
        
        #endregion

        #region BloodThristCD
        private WaitTimer wtBloodThirst = new WaitTimer(System.TimeSpan.FromSeconds(3));
        public bool BLCDGreaterOneFive()
        {
            if (wtColossusSmash.TimeLeft.TotalSeconds > 1.5)
            {
                //Logging.Write(Color.Orchid, "CSCD > 5");
                return true;
            }
            else if (wtColossusSmash.TimeLeft.TotalSeconds > 0)
            {
                //Logging.Write(Color.Orchid, "CSCD < 5");
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Pull
        private Composite _pullBehavior;
        public override Composite PullBehavior
        {
            get
            {
                if (_pullBehavior == null)
                {
                    Logging.Write(Color.Orange, "Creating 'Pull' behavior");
                    _pullBehavior = CreatePullBehavior();
                }
                _pullBehavior = CreatePullBehavior();
                return _pullBehavior;
            }
        }
        // Credit to CodenameG - edited for use with DWCC
        private static readonly WaitTimer PullTimer = WaitTimer.TenSeconds;
        private WaitTimer wtCharge = new WaitTimer(System.TimeSpan.FromSeconds(10));
        private WaitTimer wtHamstring = new WaitTimer(System.TimeSpan.FromSeconds(8));
        private WaitTimer wtRend = new WaitTimer(System.TimeSpan.FromSeconds(15));
        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(

                 new Decorator(ret => DunatanksSettings.Instance.DisableMovement && !Me.GotTarget && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.Dead && Me.CurrentTarget.Attackable,
                    new Action(ctx => RunStatus.Success)),

                // Use leaders target
                new Decorator(
                    ret =>
                    !DunatanksSettings.Instance.DisableMovement && Me.IsInParty && RaFHelper.Leader != null && RaFHelper.Leader.GotTarget && Me.GotTarget &&
                    Me.CurrentTargetGuid != RaFHelper.Leader.CurrentTargetGuid,
                    new Action(ret =>
                               RaFHelper.Leader.CurrentTarget.Target())),

                // Clear target and return failure if it's tagged by someone else
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && !Me.IsInParty && Me.GotTarget && Me.CurrentTarget.TaggedByOther,
                              new Action(delegate
                              {
                                  SpellManager.StopCasting();
                                  Logging.Write(Color.Orange, "Someone else tagged your target! We are so not going to help him!");
                                  Blacklist.Add(Me.CurrentTarget, TimeSpan.FromMinutes(30));
                                  Me.ClearTarget();
                                  return RunStatus.Failure;
                              })
                    ),

                // If we are casting we assume we are already pulling so let it 'return' smoothly. 
                // if we are in combat pull suceeded and the combat behavior should run
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && (Me.IsCasting || Me.Combat) && Me.CurrentTarget.Distance < PullDistance + 3,
                              new Action(delegate { return RunStatus.Success; })),

                // Make sure we got a proper target
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && !Me.GotTarget && !Me.IsInParty,
                              new Action(delegate
                              {
                                  Targeting.Instance.TargetList[0].Target();
                                  WoWMovement.Face();
                                  Thread.Sleep(100);
                                  return RunStatus.Success;
                              })),

                // Blacklist target's we can't move to
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && Navigator.GeneratePath(Me.Location, Me.CurrentTarget.Location).Length <= 0,
                              new Action(delegate
                              {
                                  Blacklist.Add(Me.CurrentTargetGuid, TimeSpan.FromDays(365));
                                  Logging.Write(Color.Orange, "Can't move to: {0} blacklisted!",
                                      Me.CurrentTarget.Name);
                                  return RunStatus.Success;
                              })
                    ),

                // Move closer to the target if we are too far away or in !Los
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && Me.GotTarget && (Me.CurrentTarget.Distance > PullDistance || !Me.CurrentTarget.InLineOfSight),
                              new Action(delegate
                              {
                                  if (!DunatanksSettings.Instance.DisableMovement)
                                  {
                                      Logging.Write(Color.Orange, "Approaching:{0}", Me.CurrentTarget);
                                      Navigator.MoveTo(Me.CurrentTarget.Location);
                                  }
                              })),

                // Stop moving if we are moving
                new Decorator(ret => DunatanksSettings.Instance.DisableMovement && Me.IsMoving,
                              new Action(ret => WoWMovement.MoveStop())),

                // Face the target if we aren't
                new Decorator(ret => !DunatanksSettings.Instance.DisableMovement && Me.GotTarget && !Me.IsFacing(Me.CurrentTarget),
                              new Action(ret => WoWMovement.Face())
                    ),

                new PrioritySelector(
                   new Sequence(
                        //new Action(ret => Logging.Write(Color.Orange, "Engaging {0}", Me.CurrentTarget.Name)),
                        new PrioritySelector(
                            new PrioritySelector(
                                CreateAutoAttack(),
                                MoveToTargetProper(),
                                WritePullDebug(),
                                FaceTarget(ret => !DunatanksSettings.Instance.DisableMovement),
                                ChargePull(),
                                CreateSpellCheckAndCast("Heroic Throw", ret => Me.Level >= 20 && DunatanksSettings.Instance.PullHeroicThrow && !SpellManager.Spells["Heroic Throw"].Cooldown && Me.CurrentTarget.Distance <= 30),
                                CreateSpellCheckAndCast("Intercept", ret => Me.Level >= 50 && DunatanksSettings.Instance.useInterceptApproachPvP && Styx.Logic.Battlegrounds.IsInsideBattleground && Me.CurrentTarget.Distance <= 25),
                                CreateSpellCheckAndCast("Shoot", ret => Me.Inventory.Equipped.Ranged != null && SpellManager.CanCast("Shoot") && !Styx.Logic.Battlegrounds.IsInsideBattleground && Me.CurrentTarget.Distance <= 30 && Me.CurrentTarget.Distance > 15),
                                CreateSpellCheckAndCast("Throw", ret => Me.Inventory.Equipped.Ranged != null && SpellManager.CanCast("Throw") && !Styx.Logic.Battlegrounds.IsInsideBattleground && Me.CurrentTarget.Distance <= 30 && Me.CurrentTarget.Distance > 15)),
                                
                                MoveToTargetProper(),
                            //stopmoving(),
                            CreateAutoAttack()
                        ))));
        }
        #endregion

        #region Targeting
        public void AutoTankTargeting()
        {
            if (DunatanksSettings.Instance.useProt == true && NeedTankTargeting == false && DunatanksSettings.Instance.useAutoTargetProt == true)
            {
                NeedTankTargeting = true;
            }
            else if (DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useAutoTargetProt == false)
            {
                NeedTankTargeting = false;
            }
        }
        #endregion

        #region Combat
        private Composite _combatBehavior;

        public override Composite CombatBehavior
        {
            get { if (_combatBehavior == null) { slog("Creating an awesome Combat behavior"); _combatBehavior = CreateCombatBehavior(); } return _combatBehavior; }
        }

        private Composite CreateCombatBehavior()
        {
            return CreateBossBehavior();
        }
        #endregion
    }
}
