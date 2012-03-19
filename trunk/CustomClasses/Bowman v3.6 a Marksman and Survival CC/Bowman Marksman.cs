using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using System.Drawing;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Marksman
{
    class Classname : CombatRoutine
    {
        public override sealed string Name { get { return "Bowman a Marksmanship CC v3.6"; } }

        public override WoWClass Class { get { return WoWClass.Hunter; } }


        private static LocalPlayer Me { get { return ObjectManager.Me; } }


        #region Log
        private void slog(string format, params object[] args) //use for slogging
        {
            Logging.Write(format, args);
        }
        #endregion


        #region Initialize
        public override void Initialize()
        {
            Logging.Write(Color.White, "___________________________________________________");
            Logging.Write(Color.Crimson, "Bowman v3.6 by Shaddar, Venus112 and Jasf10");
            Logging.Write(Color.Crimson, "---  Remember to comment on the forum! ---");
            Logging.Write(Color.Crimson, "-------- /like and +rep if you like this CC! ------");
            Logging.Write(Color.White, "___________________________________________________");
        }
        #endregion



        #region Settings

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {

            Marksman.MarksForm1 f1 = new Marksman.MarksForm1();
            f1.ShowDialog();
        }
        #endregion

        #region rest
        public override bool NeedRest
        {
            get
            {
                if (!Me.GotAlivePet && MarksmanSettings.Instance.CP) 
                //Credits to Falldown for this code! Cheers mate!
                {
                    {
                        if (MarksmanSettings.Instance.PET == 1 && SpellManager.HasSpell("Call Pet 1"))
                        {
                            SpellManager.Cast("Call Pet 1");
                        }
                    }
                    {
                        if (MarksmanSettings.Instance.PET == 2 && SpellManager.HasSpell("Call Pet 2"))
                        {
                            SpellManager.Cast("Call Pet 2");
                        }
                    }
                    {
                        if (MarksmanSettings.Instance.PET == 3 && SpellManager.HasSpell("Call Pet 3"))
                        {
                            SpellManager.Cast("Call Pet 3");
                        }
                    }
                    {
                        if (MarksmanSettings.Instance.PET == 4 && SpellManager.HasSpell("Call Pet 4"))
                        {
                            SpellManager.Cast("Call Pet 4");
                        }
                    }
                    {
                        if (MarksmanSettings.Instance.PET == 5 && SpellManager.HasSpell("Call Pet 5"))
                        {
                            SpellManager.Cast("Call Pet 5");
                        }
                    }
                    StyxWoW.SleepForLagDuration();
                }
                {
                if (MarksmanSettings.Instance.RP && !Me.GotAlivePet && SpellManager.HasSpell("Revive Pet"))
                    {
                        if (CastSpell("Revive Pet")) 
                        StyxWoW.SleepForLagDuration();
                    }
                }
                return true;

            }
              
        }
        #endregion

        #region AimedShot
        public bool AimedShot()
        {
            using (new FrameLock())
            {
                if (MarksmanSettings.Instance.MixedROT)
                {
                    if (Me.ActiveAuras.ContainsKey("Chronohunter") || Me.ActiveAuras.ContainsKey("Ancient Hysteria") || Me.ActiveAuras.ContainsKey("Time Warp") || Me.ActiveAuras.ContainsKey("Bloodlust")
                    || Me.ActiveAuras.ContainsKey("Heroism") || Me.ActiveAuras.ContainsKey("Rapid Fire") || (Me.ActiveAuras.ContainsKey("Improved Steady Shot") && (Me.ActiveAuras.ContainsKey("Hunting Party") 
                    || Me.ActiveAuras.ContainsKey("Improved Icy Talons") || Me.ActiveAuras.ContainsKey("Windfury Totem") || Me.ActiveAuras.ContainsKey("Blessing of Khaz'goroth")
                    || Me.ActiveAuras.ContainsKey("Arrow of Time") || Me.ActiveAuras.ContainsKey("Race Against Death") || Me.ActiveAuras.ContainsKey("Heart's Judgement") || Me.ActiveAuras.ContainsKey("Matrix Restabilizer")
                    || Me.ActiveAuras.ContainsKey("Nefarious Plot") || Me.ActiveAuras.ContainsKey("Velocity") || Me.ActiveAuras.ContainsKey("Devour"))))
                return true;
                }
                
            }
            return false;
        } 
        #endregion

        #region Dragon Soul
        public bool Ultra()
        {
            using (new FrameLock())
            {
                if (MarksmanSettings.Instance.DSNOR || MarksmanSettings.Instance.DSHC || MarksmanSettings.Instance.DSLFR)
                {
                    foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
                    {
                        if (u.IsAlive
                            && u.Guid != Me.Guid
                            && u.IsHostile
                            && u.IsCasting
                            && u.CastingSpell.Name == "Hour of Twilight"
                            && u.CurrentCastTimeLeft.TotalMilliseconds <= 800)
                            return true;
                    }

                }

            }
            return false;
        }
        public bool UltraFL()
        {
            using (new FrameLock())
            {
                if (MarksmanSettings.Instance.DSNOR || MarksmanSettings.Instance.DSHC)
                {
                    foreach (WoWUnit u in ObjectManager.GetObjectsOfType<LocalPlayer>(true, true))
                    {
                        if (u.Debuffs.ContainsKey("Fading Light")
                            && u.Debuffs["Fading Light"].IsActive
                            && u.Debuffs["Fading Light"].TimeLeft.TotalMilliseconds <= 2000)
                            return true;
                    }

                }

            }
            return false;
        }
        public bool DW()
        {
            using (new FrameLock())
            {
                if (MarksmanSettings.Instance.DSNOR || MarksmanSettings.Instance.DSHC || MarksmanSettings.Instance.DSLFR)
                {
                    foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
                    {
                        if (u.IsAlive
                            && u.Guid != Me.Guid
                            && u.IsHostile
                            && (u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember || u.IsTargetingMeOrPet || u.IsTargetingAnyMinion)
                            && u.IsCasting
                            && u.CastingSpell.Name == "Shrapnel"
                            && u.CurrentCastTimeLeft.TotalMilliseconds <= 2000)
                            return true;
                    }

                }

            }
            return false;
        }
        #endregion

        #region Add Detection
        private int addCount()
        {
            int count = 0;
            foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
            {
                if (u.IsAlive
                    && u.Guid != Me.Guid
                    && u.IsHostile
                    && !u.IsCritter
                    && (u.Location.Distance(Me.CurrentTarget.Location) <= 12 || u.Location.Distance2D(Me.CurrentTarget.Location) <= 12)
                    && (u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember || u.IsTargetingMeOrPet || u.IsTargetingAnyMinion)
                    && !u.IsFriendly)
                {
                    count++;
                }
            }
            return count;
        }
        private bool IsTargetBoss()
        {
            using (new FrameLock())
            {
                if (Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss ||
                (Me.CurrentTarget.Level >= 85 && Me.CurrentTarget.Elite) && Me.CurrentTarget.MaxHealth > 3500000)
                  return true;

                    else return false;
            }
        }
        #endregion

        #region CastSpell Method
        // Credit to Apoc for the below CastSpell code
        // Used for calling CastSpell in the Combat Rotation
        //Credit to Wulf!
        public bool CastSpell(string spellName)
        {
            using (new FrameLock())
            {
                if (SpellManager.CanCast(spellName))
                {
                    SpellManager.Cast(spellName);
                    // We managed to cast the spell, so return true, saying we were able to cast it.
                    return true;
                }
            }
            // Can't cast the spell right now, so return false.
            return false;
        }
        #endregion

        #region MyDebuffTime
        //Used for checking how the time left on "my" debuff
        private int MyDebuffTime(String spellName, WoWUnit unit)
        {
            using (new FrameLock())
            {
                if (unit.HasAura(spellName))
                {
                    var auras = unit.GetAllAuras();
                    foreach (var a in auras)
                    {
                        if (a.Name == spellName && a.CreatorGuid == Me.Guid)
                        {
                            return a.TimeLeft.Seconds;
                        }
                    }
                }
            }
            return 0;
        }
        #endregion

        #region DebuffTime
        //Used for checking debuff timers
        private int DebuffTime(String spellName, WoWUnit unit)
        {
            using (new FrameLock())
            {
                if (unit.HasAura(spellName))
                {
                    var auras = unit.GetAllAuras();
                    foreach (var b in auras)
                    {
                        if (b.Name == spellName)
                        {
                            return b.TimeLeft.Seconds;
                        }
                    }
                }
            }
            return 0;
        }
        #endregion

        #region IsMyAuraActive
        //Used for checking auras that has no time
        private bool IsMyAuraActive(WoWUnit Who, String What)
        {
            using (new FrameLock())
            {
                return Who.GetAllAuras().Where(p => p.CreatorGuid == Me.Guid && p.Name == What).FirstOrDefault() != null;
            }
        }
        #endregion

        #region CombatStart

        private void AutoAttack()
        {
            if (!Me.IsAutoAttacking)
            {
                Lua.DoString("StartAttack()");
            }

        }
        #endregion

        #region Combat

        public override void Combat()
        {
            using (new FrameLock())

            if (Me.GotTarget && Me.CurrentTarget.IsAlive && !Me.Mounted)
            {

                    {
                        if (Ultra())
                        {
                            Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                            SpellManager.StopCasting();
                            {
                                Logging.Write(Color.Aqua, ">> Heroic Will! <<");
                            }
                        }   
                    }
                    {
                        if (UltraFL())
                        {
                            Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                            SpellManager.StopCasting();
                            {
                                Logging.Write(Color.Aqua, ">> Heroic Will! <<");
                            }
                        }
                    }
                    {
                        if (DW())
                        {
                            Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                            SpellManager.StopCasting();
                            {
                                Logging.Write(Color.Aqua, ">> Enter the dream! <<");
                            }
                        }
                    }
                    {
                        if (MarksmanSettings.Instance.MP && Me.GotAlivePet && Me.Pet.HealthPercent <= 50 && !Me.Pet.ActiveAuras.ContainsKey("Mend Pet"))
                        {
                        if(CastSpell("Mend Pet"))
                            {
                            Logging.Write(Color.Aqua, ">> Mend Pet <<");
                            }
                        }
                    }
                
                /////////////////////////////////////////////Cooldowns are here/////////////////////////////////////////////////////////////////////////////////////////////

            }
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())
                {
                    if (MarksmanSettings.Instance.RF && !Me.ActiveAuras.ContainsKey("Rapid Fire") && IsTargetBoss())
                    {
                        if (CastSpell("Rapid Fire"))
                        {
                            Logging.Write(Color.Aqua, ">> Rapid Fire <<");
                        }
                    }
                }
                {
                    if (MarksmanSettings.Instance.MMSPEC && MarksmanSettings.Instance.RF && IsTargetBoss() && ((Me.ActiveAuras.ContainsKey("Rapid Fire") && SpellManager.Spells["Rapid Fire"].CooldownTimeLeft.TotalSeconds > 10) || SpellManager.Spells["Rapid Fire"].CooldownTimeLeft.TotalSeconds > 120))
                    {
                        if (CastSpell("Readiness"))
                        {
                            Logging.Write(Color.Aqua, ">> Readiness <<");
                        }
                    }

                }
                {
                    if (MarksmanSettings.Instance.GE && IsTargetBoss())
                    {
                        Lua.DoString("RunMacroText('/use 10');");
                    }
                }
                {
                    if (MarksmanSettings.Instance.T1 && IsTargetBoss() && StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                    {
                        Lua.DoString("RunMacroText('/use 13');");
                    }
                }
                {
                    if (MarksmanSettings.Instance.T2 && IsTargetBoss() && StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                    {
                        Lua.DoString("RunMacroText('/use 14');");
                    }
                }

                //////////////////////////////////////////////////Racial Skills here/////////////////////////////////////////////////////////////////////////////////////////
                {
                    if (MarksmanSettings.Instance.RS && Me.Race == WoWRace.Troll && IsTargetBoss())
                    {
                        Lua.DoString("RunMacroText('/Cast Berserking');");
                    }
                }
                {
                    if (MarksmanSettings.Instance.RS && Me.Race == WoWRace.Orc && IsTargetBoss())
                    {
                        Lua.DoString("RunMacroText('/Cast Blood Fury');");
                    }
                }

                /////////////////////////////////////////////Moving Rotation here//////////////////////////////////////////////////////////////////////////////////////////
                {
                    if (Me.Combat && !Me.IsMoving && !Me.Auras.ContainsKey("Aspect of the Hawk"))
                    {
                        if (CastSpell("Aspect of the Hawk"))
                        {
                            Logging.Write(Color.Aqua, ">> Aspect of the Hawk <<");
                        }
                    }
                }
                {
                    if (Me.Combat && Me.IsMoving && Me.Auras.ContainsKey("Aspect of the Hawk"))
                    {
                        if (CastSpell("Aspect of the Fox"))
                        {
                            Logging.Write(Color.Aqua, ">> Aspect of the Fox <<");
                        }
                    }
                }
            }
            if (!MarksmanSettings.Instance.MMSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                {
                    if (Me.IsMoving && Me.CurrentFocus >= 50)
                    {
                        Lua.DoString("RunMacroText('/cast Explosive Shot');");
                        {
                            Logging.Write(Color.Aqua, ">> Explosive Shot <<");
                        }
                    }
                }
                {
                    if (Me.IsMoving && Me.Auras.ContainsKey("Aspect of the Fox") && (Me.CurrentFocus >= 44 && SpellManager.Spells["Explosive Shot"].Cooldown))
                    {
                        Lua.DoString("RunMacroText('/cast Cobra Shot');");
                        {
                            Logging.Write(Color.Red, ">> Cobra Shot <<");
                        }
                    }
                }
            }
            if (!MarksmanSettings.Instance.SSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                {
                    if (Me.IsMoving && Me.CurrentFocus >= 50)
                    {
                        Lua.DoString("RunMacroText('/cast Arcane Shot');");
                        {
                            Logging.Write(Color.Aqua, ">> Arcane Shot <<");
                        }
                    }
                }
                {
                    if (Me.IsMoving && Me.Auras.ContainsKey("Aspect of the Fox"))
                    {
                        Lua.DoString("RunMacroText('/cast Steady Shot');");
                        {
                            Logging.Write(Color.Red, ">> Steady Shot <<");
                        }
                    }
                }
                /////////////////////////////////////////////Aimed Shot Rotation////////////////////////////////////////////////////////////////////////////////////////////
            }
            if (addCount() < MarksmanSettings.Instance.Mobs && MarksmanSettings.Instance.AimedROT && MarksmanSettings.Instance.MMSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())
                        
                        {
                        if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) <= 1)
                            {
                            if(CastSpell("Serpent Sting"))
                                {
                                Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                                }
                            }
                        }
                        {
                        if(MyDebuffTime("Serpent Sting", Me.CurrentTarget) >=1)
                            {
                            if(CastSpell("Chimera Shot"))
                                {
                                    Logging.Write(Color.Aqua, ">> Chimera Shot <<");
                                }
                            }
                        }
                        {
                        if (SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalMilliseconds >= 500 && Me.Auras.ContainsKey("Fire!"))
                            {
                                Lua.DoString(String.Format("RunMacroText(\"/use Aimed Shot!\")"));
                                SpellManager.StopCasting();
                                {
                                Logging.Write(Color.Aqua, ">> Aimed Shot <<");
                                }
                            }
                        }
                        {
                        if (Me.CurrentTarget.HealthPercent <=20)
                            {
                            if(CastSpell("Kill Shot"))
                                {
                                Logging.Write(Color.Aqua, ">> Kill Shot <<");
                                }
                            }
                        }
                        {
                        if (SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds > 1)
                            {
                            if (CastSpell("Aimed Shot"))
                                {
                                Logging.Write(Color.Aqua, ">> Aimed Shot <<");
                                }
                            }
				        }
                        {
				        if (Me.CurrentFocus <= 45 && !Me.ActiveAuras.ContainsKey("Fire!"))
                                {
                                if (CastSpell("Steady Shot"))
                                    {
                                    Logging.Write(Color.Aqua, ">> Steady Shot <<");
                                    }
                                }
				        }

                /////////////////////////////////////////////Arcane Shot Rotation///////////////////////////////////////////////////////////////////////////////////////////

            }
            if (addCount() < MarksmanSettings.Instance.Mobs && MarksmanSettings.Instance.ArcaneROT && MarksmanSettings.Instance.MMSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())

                {
                    if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Serpent Sting"))
                        {
                            Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                        }
                    }
                }
                {
                    if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) >= 1)
                    {
                        if (CastSpell("Chimera Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                        }
                    }
                }
                {
                    if (SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalMilliseconds >= 500 && Me.Auras.ContainsKey("Fire!"))
                    {
                        Lua.DoString(String.Format("RunMacroText(\"/use Aimed Shot!\")"));
                        SpellManager.StopCasting();
                        {
                            Logging.Write(Color.Aqua, ">> Aimed Shot <<");
                        }
                    }
                }
                {
                    if (Me.CurrentTarget.HealthPercent <= 20)
                    {
                        if (CastSpell("Kill Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Kill Shot <<");
                        }
                    }
                }
                {
                    if (Me.CurrentFocus >= 50 && SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds > 1 || SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds >= 4)
                    {
                        if (CastSpell("Arcane Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Arcane Shot <<");
                        }
                    }
                }
                {
                    if (Me.CurrentFocus <= 45 && !Me.ActiveAuras.ContainsKey("Fire!"))
                    {
                        if (CastSpell("Steady Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Steady Shot <<");
                        }
                    }
                }
                    

                /////////////////////////////////////////////Aimed + Arcane shot Rotation/////Aimed shot when haste bonus, else arcane shot/////////////////////////////////

            }
            if (addCount() < MarksmanSettings.Instance.Mobs && MarksmanSettings.Instance.MixedROT && MarksmanSettings.Instance.MMSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())
                {
                    if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Serpent Sting"))
                        {
                            Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                        }
                    }
                }
                {
                    if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) >= 1)
                    {
                        if (CastSpell("Chimera Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                        }
                    }
                }
                {
                    if (SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalMilliseconds >= 500 && Me.ActiveAuras.ContainsKey("Fire!"))
                    {
                        Lua.DoString(String.Format("RunMacroText(\"/use Aimed Shot!\")"));
                        SpellManager.StopCasting();
                        {
                            Logging.Write(Color.Aqua, ">> Aimed Shot <<");
                        }
                    }
                }
                {
                    if (Me.CurrentTarget.HealthPercent <= 20)
                    {
                        if (CastSpell("Kill Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Kill Shot <<");
                        }
                    }
                }
                {
                    if (AimedShot() && SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds >= 1)
                    {
                        if (CastSpell("Aimed Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Aimed Shot <<");
                        }
                    }
                }
                {
                    if (!AimedShot() && Me.CurrentFocus >= 50 && SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds > 1 || !AimedShot() && SpellManager.Spells["Chimera Shot"].CooldownTimeLeft.TotalSeconds >= 4)
                    {
                        if (CastSpell("Arcane Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Arcane Shot <<");
                        }
                    }
                }
                {
                    if (Me.CurrentFocus <= 45 && !Me.ActiveAuras.ContainsKey("Fire!"))
                    {
                        if (CastSpell("Steady Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Steady Shot <<");
                        }
                    }
                }
                    

                /////////////////////////////////////////////Survival Spec Rotation/////////////////////////////////////////////////////////////////////////////////////////
                        
            }
            if (addCount() < MarksmanSettings.Instance.Mobs && MarksmanSettings.Instance.ExploROT && MarksmanSettings.Instance.SSPEC && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())
                {
                    if (addCount() < 2 && MyDebuffTime("Hunter's Mark", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Hunter's Mark"))
                        {
                            Logging.Write(Color.Aqua, ">> Hunter's Mark <<");
                        }
                    }
                }
                {
                    if (!SpellManager.Spells["Explosive Shot"].Cooldown && MyDebuffTime("Explosive Shot", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Explosive Shot"))
                        
                        {
                            Logging.Write(Color.Aqua, ">> Explosive Shot <<");
                        }
                    }
                }
                {
                    if (MyDebuffTime("Serpent Sting", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Serpent Sting"))
                        {
                            Logging.Write(Color.Aqua, ">> Serpent Sting <<");
                        }
                    }
                }
                {
                    if (Me.CurrentTarget.HealthPercent < 20)
                    {
                        if (CastSpell("Kill Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Kill Shot <<");
                        }
                    }
                }
                {
                    if (MyDebuffTime("Black Arrow", Me.CurrentTarget) <= 1)
                    {
                        if (CastSpell("Black Arrow"))
                        {
                            Logging.Write(Color.Aqua, ">> Black Arrow <<");
                        }
                    }
                }
                {
                    if (Me.CurrentFocus >= 62)
                    {
                        if (CastSpell("Arcane Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Arcane Shot <<");
                        }
                    }
                }
                {
                    if (!Me.ActiveAuras.ContainsKey("Lock and Load"))
                    {
                        if (CastSpell("Cobra Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Cobra Shot <<");
                        }
                    }
                }

                //////////////////////////////////////////////AoE Rotation here/////////////////////////////////////////////////////////////////////////////////////////////////
            }
            if (addCount() >= MarksmanSettings.Instance.Mobs && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {
                using (new FrameLock())
                {
                    if (SpellManager.Spells["Explosive Trap"].CooldownTimeLeft.TotalSeconds < 1)
                    {
                        if (CastSpell("Trap Launcher"))
                        {
                            Logging.Write(Color.Red, ">> Trap Launcher Activated! <<");
                        }
                    }
                }
                {
                    if (Me.HasAura("Trap Launcher"))
                    {
                        Lua.DoString("CastSpellByName('Explosive Trap');");
                        {
                            LegacySpellManager.ClickRemoteLocation(StyxWoW.Me.CurrentTarget.Location);
                        }
                    }
                }
                {
                    if (!Me.HasAura("Trap Launcher"))
                    {
                        if (CastSpell("Multi-Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Multi-Shot <<");
                        }
                    }
                }
                {
                    if (!Me.HasAura("Trap Launcher") && !MarksmanSettings.Instance.MMSPEC && Me.CurrentFocus <= 42)
                    {
                        if (CastSpell("Cobra Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Cobra Shot <<");
                        }
                    }
                }
                {
                    if (!Me.HasAura("Trap Launcher") && !MarksmanSettings.Instance.SSPEC && Me.CurrentFocus <= 42)
                    {
                        if (CastSpell("Steady Shot"))
                        {
                            Logging.Write(Color.Aqua, ">> Steady Shot <<");
                        }
                    }
                }



            }

        }

        #endregion

    }
}