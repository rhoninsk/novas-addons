using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.BehaviorTree;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;
using Styx.Logic.Pathing;

namespace Dunatanks3
{
    public partial class Warrior
    {

        public delegate bool SimpleBooleanDelegate(object context);

        public static Composite CreateAutoAttack()
        {

            return new PrioritySelector(
                new Decorator(
                    ret => !StyxWoW.Me.IsAutoAttacking && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange,
                    new Action(ret => StyxWoW.Me.ToggleAttack()))
            );
        }

        WoWItem Gloves = StyxWoW.Me.Inventory.GetItemBySlot(9);
        public void UseSynapseSprings()
        {
            if (Me.Inventory.GetItemBySlot(9) != null && Me.Inventory.GetItemBySlot(9).Usable == true)
            {
                Gloves.Use();
            }
        }

        WoWItem Flask;
        public void GetFlask()
        {
            if (DunatanksSettings.Instance.UseFlaskOfBattle)
            {
                List<WoWItem> FlaskOfBattle =
                        (from obj in
                             Me.BagItems.Where(
                                 ret => ret != null && ret.BaseAddress != 0 &&
                                 (ret.ItemInfo.Name == "Flask of Battle"))
                         select obj).ToList();
                {
                    Flask = FlaskOfBattle.FirstOrDefault();
                    Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfBattle.FirstOrDefault().Name);
                }
            }
            else if (DunatanksSettings.Instance.UseFlaskOfFlowingWater)
            {
                {
                    List<WoWItem> FlaskOfFlowingWater =
                            (from obj in
                                 Me.BagItems.Where(
                                     ret => ret != null && ret.BaseAddress != 0 &&
                                     (ret.ItemInfo.Name == "Flask of Flowing Water"))
                             select obj).ToList();
                    {
                        Flask = FlaskOfFlowingWater.FirstOrDefault();
                        Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfFlowingWater.FirstOrDefault().Name);
                    }
                }
            }
            else if (DunatanksSettings.Instance.UseFlaskOfSteelskin)
            {
                {
                    List<WoWItem> FlaskOfSteelskin =
                            (from obj in
                                 Me.BagItems.Where(
                                     ret => ret != null && ret.BaseAddress != 0 &&
                                     (ret.ItemInfo.Name == "Flask of Steelskin"))
                             select obj).ToList();
                    {
                        Flask = FlaskOfSteelskin.FirstOrDefault();
                        Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfSteelskin.FirstOrDefault().Name);
                    }
                }
            }
            else if (DunatanksSettings.Instance.UseFlaskOfDraconicMind)
            {
                {
                    List<WoWItem> FlaskOfDraconicMind =
                            (from obj in
                                 Me.BagItems.Where(
                                     ret => ret != null && ret.BaseAddress != 0 &&
                                     (ret.ItemInfo.Name == "Flask of Draconic Mind"))
                             select obj).ToList();
                    {
                        Flask = FlaskOfDraconicMind.FirstOrDefault();
                        Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfDraconicMind.FirstOrDefault().Name);
                    }
                }
            }
            else if (DunatanksSettings.Instance.UseFlaskOfTitanicStrength)
            {
                {
                    List<WoWItem> FlaskOfTitanicStrength =
                            (from obj in
                                 Me.BagItems.Where(
                                     ret => ret != null && ret.BaseAddress != 0 &&
                                     (ret.ItemInfo.Name == "Flask of Titanic Strength"))
                             select obj).ToList();
                    {
                        Flask = FlaskOfTitanicStrength.FirstOrDefault();
                        Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfTitanicStrength.FirstOrDefault().Name);
                    }
                }
            }
            else if (DunatanksSettings.Instance.UseFlaskOfTheWinds)
            {
                {
                    List<WoWItem> FlaskOfTheWinds =
                            (from obj in
                                 Me.BagItems.Where(
                                     ret => ret != null && ret.BaseAddress != 0 &&
                                     (ret.ItemInfo.Name == "Flask of the Winds"))
                             select obj).ToList();
                    {
                        Flask = FlaskOfTheWinds.FirstOrDefault();
                        Logging.Write(Color.Orange, "Flask Found {0}", FlaskOfTheWinds.FirstOrDefault().Name);
                    }
                }
            }
            else
            {
                Flask = null;
            }
        }
        public Composite UseFlask()
            {
                GetFlask();
                return new Decorator(ret => !Me.HasAura(Flask.Name) && Flask != null && !Me.Dead && !DunatanksSettings.Instance.DoNotUseFlask && Flask.StackCount > 0,
                                new Action(delegate
                                    {
                                        Flask.Use();
                                        Logging.Write(Color.Lime, "Using --> " + Flask.Name + " <--");
                                    }
            ));
        }
        WoWItem GBPotion;
        public bool HaveGBPotion()
        {
            if (GBPotion == null)
            {
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item.Entry == 58146)
                    {
                        GBPotion = item;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return true;
            }
        }
        public Composite UseGBPotion()
        {
            return new Decorator(ret => GBPotion != null && GBPotion.BaseAddress != 0 && Me.CurrentTarget.MaxHealth > 9000000 && ((DunatanksSettings.Instance.UseGolembloodPotionBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.UseGolembloodPotionHeroism && Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp"))),
                                 new Action(delegate
                                 {
                                     GBPotion.Use();
                                     Logging.Write(Color.Lime, "--> Using " + GBPotion.Name + " <--");
                                 }
                                     ));
        }

        public bool CheckTrinketOne()
        {
            if (Me.Inventory.Equipped.Trinket1 != null)
                            {
                                if (StyxWoW.Me.Inventory.Equipped.Trinket1.Usable == true)
                                {
                                    WoWItem TrinketOne = StyxWoW.Me.Inventory.Equipped.Trinket1;
                                    //Logging.Write(StyxWoW.Me.Inventory.Equipped.Trinket1.Name + "check 2");
                                    return true;
                                }
                                //Logging.Write(StyxWoW.Me.Inventory.Equipped.Trinket1.Name + "check 1");
                                return false;
                            }
            return false;
        }

        WoWItem TrinketOne = null;

        public Composite UseTrinketOne()
        {
            return new Decorator(ret => CheckTrinketOne() && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown == 0 && ((DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.UseTrinketOneProtOnCd) || (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseTrinketOneFuryOnCd) || (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.UseTrinketOneArmsOnCd) || (DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.UseTrinketOneArmsBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseTrinketOneFuryBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.UseTrinketOneArmsBelow20 && Me.CurrentTarget.HealthPercent < 20 || (DunatanksSettings.Instance.useArms && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketOneArmsCC) || (DunatanksSettings.Instance.useFury && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketOneFuryCC) || (DunatanksSettings.Instance.useProt && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketOneProtCC) || (DunatanksSettings.Instance.useProt && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketOneProtHero) || (DunatanksSettings.Instance.useFury && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketOneFuryHero) || (DunatanksSettings.Instance.useArms && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketOneArmsHero))),
                                 new Action(delegate
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                                     Logging.Write(Color.Lime, "--> Using " + TrinketOne.Name + " <--");
                                 }
                                     ));
        }

        WoWItem TrinketTwo;

        public bool CheckTrinketTwo()
        {
            if (Me.Inventory.Equipped.Trinket2 != null)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2.Usable == true)
                {
                    WoWItem TrinketTwo = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    //Logging.Write(TrinketTwo.Name + "check 2");
                    return true;
                }
//Logging.Write(StyxWoW.Me.Inventory.Equipped.Trinket2.Name + "check 1");
                return false;
            }
            return false;
        }

        

        public Composite UseTrinketTwo()
        {

            return new Decorator(ret => CheckTrinketTwo() && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown == 0 && ((DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.UseTrinketTwoProtOnCd) || (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseTrinketTwoFuryOnCd) || (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.UseTrinketTwoArmsOnCd) || (DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.UseTrinketTwoArmsBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseTrinketTwoFuryBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.UseTrinketTwoArmsBelow20 && Me.CurrentTarget.HealthPercent < 20) || (DunatanksSettings.Instance.useArms && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketTwoArmsCC) || (DunatanksSettings.Instance.useFury && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketTwoFuryCC) || (DunatanksSettings.Instance.useProt && IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketTwoProtCC) || (DunatanksSettings.Instance.useProt && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketTwoProtHero) || (DunatanksSettings.Instance.useFury && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketTwoFuryHero) || (DunatanksSettings.Instance.useArms && (Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketTwoArmsHero)),
                                 new Action(delegate
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                                     Logging.Write(Color.Lime, "--> Using " + TrinketTwo.Name + " <--");
                                 }
                                     ));
        }

        public bool TrinketOneReady()
        {
            if (StyxWoW.Me.Inventory.GetItemBySlot(12) != null && StyxWoW.Me.Inventory.GetItemBySlot(12).BaseAddress != 0)
            {
                if (StyxWoW.Me.Inventory.GetItemBySlot(12).Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TrinketTwoReady()
        {
            if (StyxWoW.Me.Inventory.GetItemBySlot(13) != null && StyxWoW.Me.Inventory.GetItemBySlot(13).BaseAddress != 0)
            {
                if (StyxWoW.Me.Inventory.GetItemBySlot(13).Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Credits to Apoc and Singular devs
        public bool IsCrowdControlled(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (//a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                      //a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                      a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                      //a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                      //a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                     // a.Spell.Mechanic == WoWSpellMechanic.Stunned ||
                      a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                      //a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                      a.Spell.Mechanic == WoWSpellMechanic.Sapped
                      ));
        }

        public bool IsEnraged(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.Spell.Mechanic == WoWSpellMechanic.Enraged
                      );
        }

        public bool IsSlowed(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                    a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                    a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                      a.Spell.Mechanic == WoWSpellMechanic.Snared
                      ));
        }

        public bool IsPvPCrowdControlled(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                      a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                      a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                      a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                      a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                      a.Spell.Mechanic == WoWSpellMechanic.Stunned ||
                      a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                      a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                      a.Spell.Mechanic == WoWSpellMechanic.Sapped
                      ));
        }

        // credits to CodenameG
        public static ulong LastTarget;
        public static ulong LastTargetHPPot;
        WoWItem CurrentHealthPotion;
        public bool HaveHealthPotion()
        {
            //whole idea is to make sure CurrentHealthPotion is not null, and to check once every battle. 
            if (CurrentHealthPotion == null)
            {
                if (LastTargetHPPot == null || Me.CurrentTarget.Guid != LastTargetHPPot) //Meaning they are not the same. 
                {
                    LastTarget = Me.CurrentTarget.Guid; // set guid to current target. 
                    List<WoWItem> HPPot =
                    (from obj in
                         Me.BagItems.Where(
                             ret => ret != null && ret.BaseAddress != 0 &&
                             (ret.ItemInfo.ItemClass == WoWItemClass.Consumable) &&
                             (ret.ItemInfo.ContainerClass == WoWItemContainerClass.Potion) &&
                             (ret.ItemSpells[0].ActualSpell.SpellEffect1.EffectType == WoWSpellEffectType.Heal))
                     select obj).ToList();
                    if (HPPot.Count > 0)
                    {

                        //on first check, set CurrentHealthPotion so we dont keep running the list looking for one, 
                        CurrentHealthPotion = HPPot.FirstOrDefault();
                        Logging.Write(Color.Orange, "Potion Found {0}", HPPot.FirstOrDefault().Name);
                        return true;

                    }
                }


                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HealthPotionReady()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != 0)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void UseHealthPotion()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != 0)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    Logging.Write(Color.Orange, "Low HP! Using --> {0} <--!", CurrentHealthPotion.Name.ToString());
                    CurrentHealthPotion.Use();
                }
            }
        }

        // credits to CodenameG
        public static WoWItem HealthStone;
        public static bool HaveHealthStone()
        {

            if (HealthStone == null)
            {
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item.Entry == 5512)
                    {
                        HealthStone = item;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HealthStoneNotCooldown()
        {
            if (HealthStone != null && HealthStone.BaseAddress != 0)
            {

                if (HealthStone.Cooldown == 0)
                {
                    return true;
                }

            }
            return false;
        }
        public static void UseHealthStone()
        {
            if (HealthStone != null && HealthStoneNotCooldown())
            {
                Logging.Write(Color.Lime, "Swallowing the green pill! Using --> Healthstone <--");
                HealthStone.Use();
            }
        }

        public static WoWAura Bloodsurge 
        { 
            get 
            { 
                return StyxWoW.Me.GetAuraById(46916); 
            } 
        }

        public static WoWAura TFB
        {
            get
            {
                return StyxWoW.Me.GetAuraById(56636);
            }
        }

        public static List<WoWUnit> NearbyUnfriendlyUnits
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsHostile && !p.Dead && !p.IsPet && p.DistanceSqr <= 40 * 40).ToList();
            }
        }


        private void slog(string format, params object[] args) //use for slogging
        {
            Logging.Write(format, args);
        }

        #region Casting Helpers
        /* credit to iLyaLS
        * modified by Wownerds
        * to work with
        * Dunatank's Warrior CC v2.0 BT
        */
        public delegate WoWUnit UnitSelectDelegate(object context);

        public Composite CreateBuffCheckAndCast(string name)
        {
            return new Decorator(ret => SpellManager.CanBuff(name),
                                 new Action(delegate
                                     {
                                         SpellManager.Buff(name);
                                         Logging.Write(Color.Lime, "--> " + name + " <--");
                                     }                                     
                                     ));
        }

        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit)
        {
            return new Decorator(ret => SpellManager.CanBuff(name, onUnit(ret)),
                                 new Action(ret => SpellManager.Buff(name, onUnit(ret))));
        }

        public Composite cbac(string name, WoWPlayer onUnit)
        {
            return new Decorator(ret => SpellManager.CanBuff(name, onUnit),
                                 new Action(delegate
                                     {
                                         SpellManager.Buff(name, onUnit);
                                        Logging.Write(Color.Lime, "--> " + name + " <-- on " + onUnit + "!");    
                                     }
                                     ));
        }



        public Composite CreateBuffCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanBuff(name),
                                 new Action(delegate
                                     {
                                         SpellManager.Buff(name);
                                          Logging.Write(Color.Lime, "--> " + name + " <--");    
                                     }
                                     ));
        }

        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit, CanRunDecoratorDelegate extra)
        {
            return CreateBuffCheckAndCast(name, onUnit, extra, false);
        }

        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit, CanRunDecoratorDelegate extra, bool targetLast)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanBuff(name, onUnit(ret)),
                                 new Action(ret => SpellManager.Buff(name, onUnit(ret))));
        }

        public Composite CreateSpellCheckAndCast(string name)
        {
            return new Decorator(ret => SpellManager.CanCast(name),
                                 new Action(delegate
                                     {
                                         SpellManager.Cast(name); 
                                         Logging.Write(Color.Red, "--> " + name + " <--");
                                         return RunStatus.Success;
                                     }
                                     ));

        }


        public Composite CastCSonCD()
        {
            return new Decorator(ret => SpellManager.CanCast("Colossus Smash") && ((DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseColossusSmashOnCdFury) && !Me.CurrentTarget.HasAura("Colossus Smash")),
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Colossus Smash");
                                     Logging.Write(Color.Red, "--> " + "Colossus Smash" + " <--");
                                     wtColossusSmash.Reset();
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Sequence ChargePull()
        {
            return new Sequence(ret => Me.Level >= 3 && SpellManager.Spells["Charge"].CooldownTimeLeft.Milliseconds < 2000 && DunatanksSettings.Instance.PullCharge && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.Distance > 8 && Me.CurrentTarget.Distance <= 25 && Navigator.CanNavigateFully(Me.Location, Me.CurrentTarget.Location),
                                 new Sequence(delegate
                                 {
                                     Navigator.PlayerMover.MoveStop();
                                     CreateAutoAttack();
                                     new Decorator(ret => !Me.HasAura("Battle Stance"),
                                        new Action(delegate
                                            {
                                                Navigator.PlayerMover.MoveStop();
                                                SpellManager.Cast("Battle Stance");
                                                Logging.Write(Color.Red, "--> " + "Switching to Battle Stance to cast Charge" + " <--");
                                            }));
                                     SpellManager.Cast("Charge");
                                     Logging.Write("Charge");
                                     ResetChargeTimer();
                                     MoveToTargetProper();
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Sequence ArmsVRBersi()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Victory Rush" + " <--");
                                        }));
                                     new Decorator(ret => SpellManager.CanCast("Victory Rush"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Victory Rush");
                                            Logging.Write("--> Victory Rush <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Composite ArmsMSBersi()
        {
            return new Decorator(ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Mortal Strike" + " <--");
                                        }));
                                     new Decorator(ret => Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Mortal Strike");
                                            Logging.Write("--> Mortal Strike <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Composite ArmsCSCDBersi()
        {
            return new Decorator(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 { 
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Colossus Smash" + " <--");
                                        }));
                                     new Decorator(ret => Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Colossus Smash");
                                            Logging.Write("--> Colossus Smash <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Sequence ArmsCSPerBersi()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashRageArms && Me.CurrentRage > DunatanksSettings.Instance.ColossusSmashPercentArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Colossus Smash" + " <--");
                                        }));
                                     new Decorator(ret => SpellManager.CanCast("Colossus Smash"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Colossus Smash");
                                            Logging.Write("--> Colossus Smash <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Sequence ArmsSlamBersi()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && (!Me.HasAura("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.Level > 84,
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Slam" + " <--");
                                        }));
                                     new Decorator(ret => SpellManager.CanCast("Slam"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Slam");
                                            Logging.Write("--> Slam <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Sequence ArmsHSBersi()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 70 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Berserker Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Berserker Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Berserker Stance to cast Heroic Strike" + " <--");
                                        }));
                                     new Decorator(ret => SpellManager.CanCast("Heroic Strike"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Heroic Strike");
                                            Logging.Write("--> Heroic Strike <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Composite CastSequence(CanRunDecoratorDelegate extra, string stance, string spell)
        {
            return new Sequence(ret => extra(ret),
                                 new Action(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura(stance),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast(stance);
                                            Logging.Write(Color.Red, "--> " + "Switching to " + stance + " to cast " + spell + " <--");
                                        }));
                                     new Decorator(ret => Me.HasAura(stance),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast(spell);
                                            Logging.Write("--> " + spell + " <--");
                                            return RunStatus.Success;
                                        }));
                                     return RunStatus.Failure;
                                 }
                                     ));

        }

        public Composite CastSequenceSimple(CanRunDecoratorDelegate extra, string stance, string spell)
        {
            return new Decorator(ret => extra(ret) && !Me.HasAura(stance),
                                 new Sequence(delegate
                                 {
                                     /*new Decorator(ret => !Me.HasAura(stance),
                                        new Action(delegate
                                            {*/
                                     SpellManager.Cast(stance);
                                     Logging.Write(Color.Red, "--> " + "Switching to " + stance + " to cast " + spell + " <--");
                                     /*      }
                                    ));
                                    */
                                     SpellManager.Cast(spell);
                                     Logging.Write(Color.Red, "--> " + spell + " <--");
                                     return RunStatus.Success;
                                 }
                                     ));


        }

        public Composite CastTwoThings(CanRunDecoratorDelegate extra, string spell1, string spell2)
        {
            return new Decorator(ret => extra(ret),
                                 new Sequence(delegate
                                 {
                                     SpellManager.Cast(spell1);
                                     Logging.Write(Color.Red, "--> " + "Casting " + spell1 + " to cast " + spell2 + " <--");
                                     SpellManager.Cast(spell2);
                                     Logging.Write(Color.Red, "--> " + spell2 + " <--");
                                     return RunStatus.Success;
                                 }
                                     ));


        }

        public Sequence ArmsRendBattle()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && (Me.CurrentTarget.Debuffs["Rend"].TimeLeft.Milliseconds < 1000 || !StyxWoW.Me.CurrentTarget.HasAura("Rend")),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Battle Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Battle Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Battle Stance to cast Rend" + " <--");
                                        }));
                                     /*new Decorator(ret => SpellManager.CanCast("Rend"),
                                        new Action(delegate
                                        {*/
                                            SpellManager.Cast("Rend");
                                            Logging.Write("--> Rend <--");
                                        //}));
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Sequence ArmsOPBattle()
        {
            return new Sequence(ret => DunatanksSettings.Instance.useArms == true && Me.HasAura("Taste of Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms),
                                 new Sequence(delegate
                                 {
                                     new Decorator(ret => !Me.HasAura("Battle Stance"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Battle Stance");
                                            Logging.Write(Color.Red, "--> " + "Switching to Battle Stance to cast Overpower" + " <--");
                                        }));
                                     new Decorator(ret => SpellManager.CanCast("Overpower"),
                                        new Action(delegate
                                        {
                                            SpellManager.Cast("Overpower");
                                            Logging.Write("--> Overpower <--");
                                        }));
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Composite NeedBS()
        {
            return new Decorator(ret => !Me.HasAura("Battle Stance"),
            new Action(delegate
                {
                    SpellManager.Cast("Battle Stance");
                }));
        }

        public bool SwitchToBattleStanceForCharge()
        {
            if (!Me.HasAura("Battle Stance"))
            {
                CreateSpellCheckAndCast("Battle Stance");
                return true;
            }
            else
            {
                CreateSpellCheckAndCast("Battle Stance");
                return false;
            }
        }

        public Composite CastHamstringPvP()
        {
            return new Decorator(ret => SpellManager.CanCast("Hamstring") && DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useHamstring && (!Me.CurrentTarget.HasAura("Hamstring") && !Me.CurrentTarget.HasAura("Chains of Ice") && !Me.CurrentTarget.HasAura("Wing Clip") && !Me.CurrentTarget.HasAura("Slow") && !Me.CurrentTarget.HasAura("Crippling Posion") && !Me.CurrentTarget.HasAura("Curse of Exhaustion") && !IsSlowed(Me.CurrentTarget) && !Me.CurrentTarget.HasAura("Hand of Protection") && !Me.CurrentTarget.HasAura("Hand of Freedom")),
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Hamstring");
                                     Logging.Write(Color.Red, "--> " + "Hamstring" + " <--");
                                     wtHamstring.Reset();
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Composite CastRendPvP()
        {
            return new Decorator(ret => SpellManager.CanCast("Rend") && DunatanksSettings.Instance.usePvPRota  && (!Me.CurrentTarget.HasAura("Rend") || wtRend.TimeLeft.Milliseconds < 1200),
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Rend");
                                     Logging.Write(Color.Red, "--> " + "Rend" + " <--");
                                     wtRend.Reset();
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Composite CastCSonRage()
        {
            return new Decorator(ret => SpellManager.CanCast("Colossus Smash") && ((DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UseColossusSmashRageFury && Me.CurrentRage >= DunatanksSettings.Instance.ColossusSmashPercentFury) && !Me.CurrentTarget.HasAura("Colossus Smash")),
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Colossus Smash");
                                     Logging.Write(Color.Red, "--> " + "Colossus Smash" + " <--");
                                     wtColossusSmash.Reset();
                                     return RunStatus.Success;
                                 }
                                     ));

        }

        public Composite CastBloodThirst()
        {
            return new Decorator(ret => SpellManager.CanCast("Bloodthirst") && Me.CurrentTarget.IsWithinMeleeRange,
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Bloodthirst");
                                     Logging.Write(Color.Red, "--> Bloodthirst <--");
                                     wtBloodThirst.Reset();
                                     return RunStatus.Success;
                                 }
                                     ));

        }


        public WoWUnit AoEPummelLastTarget = null;

        public Composite AoEPummel()
        {
            SetCurrentTarget();
            return new Decorator(ret => (DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.usePummelAoEAutoArms && DunatanksSettings.Instance.usePummelArmsAoE) || (DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.usePummelAoEAutoFury && DunatanksSettings.Instance.usePummelFuryAoE) || (DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.usePummelAoEAutoProt && DunatanksSettings.Instance.usePummelProtAoE) && SpellManager.CanCast("Pummel", (WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null,
                                 new Sequence(
                                     TargetAoEPummel(ret => Me.CurrentTarget != ((WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null),
                                     FaceAoEPummel(ret => !Me.IsFacing((WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null),
                                     new Action(ret => SpellManager.Cast("Pummel", (WoWUnit)AoECastingAdds().FirstOrDefault())),
                                     new Action(ret => AoEPummelLastTarget.Target()),
                                     new Action (ret => AoEPummelLastTarget.Face())));
        }

        public void SetCurrentTarget()
        {
            AoEPummelLastTarget = Me.CurrentTarget;
        }

        public Composite FaceAoEPummel(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && !Me.IsFacing((WoWUnit)AoECastingAdds().FirstOrDefault()),
                                 new Action(delegate
                                 {
                                     ((WoWUnit)AoECastingAdds().FirstOrDefault()).Face();
                                     Logging.Write(Color.Red, "--> Facing AoE Pummel target <--");
                                 }
                                     ));
        }

        public Composite TargetAoEPummel(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && Me.CurrentTarget != ((WoWUnit)AoECastingAdds().FirstOrDefault()),
                                 new Action(delegate
                                 {
                                     ((WoWUnit)AoECastingAdds().FirstOrDefault()).Target();
                                     Logging.Write(Color.Red, "--> Targeting AoE Pummel target <--");
                                 }
                                     ));
        }

        public Composite SelectBehave(Composite behaviour, CanRunDecoratorDelegate specc)
        {
            return new Decorator(ret => specc(ret),
                new Action(delegate
                    {
                        
                    }
            ));
        }

        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanCast(name),
                                 new Action(delegate
                                     {
                                         SpellManager.Cast(name);
                                         Logging.Write(Color.Red, "--> " + name + " <--");
                                     }
                                     ));
        }

        public Composite FaceTarget(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && (!Me.IsFacing(Me.CurrentTarget) || !Me.IsSafelyFacing(Me.CurrentTarget, 5f)),
                                 new Action(delegate
                                 {
                                     Me.CurrentTarget.Face();
                                     //Logging.Write(Color.Red, "--> Facing target <--");
                                 }
                                     ));
        }

        public Composite CreateSpellCheckAndCast(string name, bool checkRange)
        {
            return new Decorator(ret => SpellManager.CanCast(name, checkRange),
                                 new Action(delegate
                                     {
                                         SpellManager.Cast(name);
                                         Logging.Write(Color.Red, "--> " + name + " <--");
                                      }
                                      ));
        }

        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra, ActionDelegate extraAction)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanCast(name),
                                 new Action(delegate(object ctx)
                                 {
                                     SpellManager.Cast(name);
                                     extraAction(ctx);
                                     Logging.Write(Color.Red, "--> " + name + " <--");
                                     return RunStatus.Success;
                                 }));
        }

        public Composite ChargePull(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanCast("Charge") && wtCharge.TimeLeft.Seconds > 0,
                                 new Action(delegate
                                 {
                                     SpellManager.Cast("Charge");
                                     Logging.Write(Color.Red, "--> Charge <--");
                                     return RunStatus.Success;
                                 }));
        }

        public Composite ResetChargeTimer()
        {
            return new Decorator(ret => wtCharge.TimeLeft.Seconds == 0 || wtCharge.TimeLeft.Seconds > 15 || wtCharge.IsFinished,
                                 new Action(delegate
                                 {
                                     wtCharge.Reset();
                                     return RunStatus.Failure;
                                 }));
        }

        public Composite WritePullDebug()
        {
            return new Decorator(ret => Styx.Logic.Combat.RoutineManager.Current.PullBehavior.IsRunning,
                                 new Action(delegate
                                 {
                                     Logging.WriteDebug("Entering pull section");
                                     return RunStatus.Failure;
                                 }));
        }

        public Composite ResetHamstringTimer()
        {
            return new Decorator(ret => wtHamstring.TimeLeft.Seconds == 0 || wtHamstring.TimeLeft.Seconds > 8 || wtHamstring.IsFinished,
                                 new Action(delegate
                                 {
                                     wtHamstring.Reset();
                                     return RunStatus.Failure;
                                 }));
        }

        public Composite ResetRendTimer()
        {
            return new Decorator(ret => wtRend.TimeLeft.Seconds == 0 || wtRend.TimeLeft.Seconds > 15 || wtRend.IsFinished,
                                 new Action(delegate
                                 {
                                     wtRend.Reset();
                                     return RunStatus.Failure;
                                 }));
        }
        #endregion
    }
}
