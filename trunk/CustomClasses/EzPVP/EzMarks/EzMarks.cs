
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.POI;
using TreeSharp;



namespace EzMarks
{
    class Hunter : CombatRoutine
    {


        public override sealed string Name { get { return "EzMarks - " + ver; } }
        public override WoWClass Class { get { return WoWClass.Hunter; } }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }


        private Version ver = new Version(0, 8, 9);

        public override bool NeedRest
        {
            get
            {
                return false;
            }
        }



        public override void Rest()
        {

        }

        public override bool NeedPullBuffs { get { return false; } }
        public override bool NeedCombatBuffs { get { return false; } }

        public struct Act
        {
            public string Spell;
            public float Trigger;
            public Act(String s, float t)
            {
                Spell = s;
                Trigger = t;
            }
        }
        public struct Trinket
        {
            public string TrinketName;
            public string StackName;
            public int StackNumber;
            public float Trigger;
            public Trinket(String s, String sn, int snn, float t)
            {
                TrinketName = s;
                StackName = sn;
                StackNumber = snn;
                Trigger = t;
            }
        }

        WoWUnit Target;





        //You can modify these lists in order to affect how the bot acts
        List<String> Rotation = new List<String> { };
        List<String> Interrupts = new List<String> { "Silencing Shot" };
        List<String> CD = new List<String> { "Rapid Fire" };
        List<Act> HealRotation = new List<Act> { };
        List<Trinket> Trinkets = new List<Trinket> { };
        bool UsescatterShot = true;



        //Variables - Don't Touch
        WoWSpell AspectoftheHawk;
        int InTrapFMode = 0;
        int InTrapTMode = 0;
        string WhatTrap;
        bool _usedScatter = false;
        WoWSpell _Trap;

        public void ResetTrap(string Which)
        {
            Lua.DoString(Which + " = 0;");
        }
        public void Log(String what)
        {
            Logging.Write(what);
        }


        WoWPoint RotatePoint;
        bool Rotating = false;
        void FindPoint()
        {
            WoWPlayer unit = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).
                    Where(p => !p.IsHostile && p.InLineOfSight).
                    OrderBy(u => u.Distance).FirstOrDefault();
            double T = Math.Sqrt(Math.Pow((unit.Location.X - Me.Location.X), 2) + Math.Pow((unit.Location.Y - Me.Location.Y), 2));
            double P3x = Me.Location.X - 50 * (unit.Location.X - Me.Location.X) / T;
            double P3y = Me.Location.Y - 50 * (unit.Location.Y - Me.Location.Y) / T;
            RotatePoint = new WoWPoint(P3x, P3y, 0);
            //WoWMovement.ClickToMove(unit.Guid, WoWMovement.ClickToMoveType.ConstantFace);
            Me.SetFacing(RotatePoint);
            Rotating = true;

        }

        public void ResetBothTraps()
        {
            using (new FrameLock())
            {
                ResetTrap("TrapTMode");
                ResetTrap("TrapFMode");
            }
        }

        public override void Combat()
        {




            Target = Me.CurrentTarget;
            if (Target == null || !Target.Attackable)
            {
                return;
            }

            //Lets try some new logic here..lets go for the owner
            else if (Target != null && Target.IsPet)
            {
                WoWUnit Owner = Target.CreatedByUnit;
                if (Owner != null && Owner.IsValid)
                {
                    Blacklist.Add(Target, new TimeSpan(0, 0, 5));
                    Logging.Write("Changing targets to pet owner");
                    Target = Owner;
                    TargetUnit(Target);
                }

            }
            #region Trapcode
            using(new FrameLock())
            {
                InTrapTMode = Lua.GetReturnVal<int>("return TrapTMode", 0);
                InTrapFMode = Lua.GetReturnVal<int>("return TrapFMode", 0);
                WhatTrap = Lua.GetReturnVal<String>("return TrapType", 0);

                /*if (WhatTrap == null)
                    WhatTrap = "";
                Log(InTrapTMode.ToString() + " - " + InTrapFMode.ToString() + " - " + WhatTrap.ToString());*/
            }

            if (InTrapTMode > 0 || InTrapFMode > 0)
            {


                if (WhatTrap == null)
                {
                    Log("Invalid Trap");
                    ResetBothTraps();
                }
                else
                {

                    SpellManager.Spells.TryGetValue(WhatTrap, out _Trap);
                    if (!_Trap.IsValid || _Trap.CooldownTimeLeft > SpellManager.GlobalCooldownLeft)
                    {
                        Log("Can't cast trap");
                            ResetBothTraps();
                    }
                    else if (Me.CurrentFocus <= 20 && !isAuraActive("Trap Launcher"))
                    {
                        Log("Can't cast trap launcher");

                            ResetBothTraps();
             
                    }
                }
            }



            


            //Magic trap modes
            if (InTrapTMode > 0)
            {

                if (SpellManager.GlobalCooldown || Me.IsCasting)
                    return;

                if (Target.InLineOfSightOCD)
                {
                    if (_usedScatter == false && UsescatterShot == true && CanCast(Target, "Scatter Shot"))
                    {
                        _usedScatter = true;
                        Cast(Target, "Scatter Shot");
                        return;
                    }


                    if (CanCast("Trap Launcher"))
                    {
                        Cast("Trap Launcher");
                        return;
                    }

                    if (CanCast(WhatTrap) && isAuraActive("Trap Launcher"))
                    {
                        //Cast(WhatTrap);
                        Lua.DoString("RunMacroText(\"/cast " + WhatTrap + "\")");
                        LegacySpellManager.ClickRemoteLocation(StyxWoW.Me.CurrentTarget.Location);
                        ResetTrap("TrapTMode");
                        _usedScatter = false;
                    }

                }
                else
                {
                    ResetTrap("TrapTMode");
                    _usedScatter = false;
                    //Maybe out to the error frame that target wasnt in line of sight or whatever the problem was
                }


            }
            else if (InTrapFMode > 0)
            {


                if (SpellManager.GlobalCooldown || Me.IsCasting)
                    return;

                if (Me.FocusedUnit.InLineOfSightOCD)
                {
                    if (_usedScatter == false && UsescatterShot == true && CanCast(Me.FocusedUnit, "Scatter Shot"))
                    {
                        _usedScatter = true;
                        Cast(Me.FocusedUnit, "Scatter Shot");
                        return;
                    }


                    if (CanCast("Trap Launcher"))
                    {
                        Cast("Trap Launcher");
                        return;
                    }

                    if (CanCast(WhatTrap) && isAuraActive("Trap Launcher"))
                    {
                        //Cast(WhatTrap);
                        Lua.DoString("RunMacroText(\"/cast " + WhatTrap + "\")");
                        LegacySpellManager.ClickRemoteLocation(StyxWoW.Me.FocusedUnit.Location);
                        //Lua.DoString("TrapFMode = 0;");
                        ResetTrap("TrapFMode");
                        _usedScatter = false;
                    }

                }
                else
                {
                    //Lua.DoString("TrapFMode = 0;");
                    ResetTrap("TrapFMode");
                    _usedScatter = false;
                    //Maybe out to the error frame that target wasnt in line of sight or whatever the problem was
                }
            }
#endregion


            if (isAuraActive("Feign Death") || isAuraActive("Trap Launcher"))
                return;


            if (!Me.GotAlivePet)
            {
                Cast("Call Pet 1");
                Thread.Sleep(1000);
                if (!Me.GotAlivePet && CanCast("Revive Pet") && !Me.Combat)
                {
                    Cast("Revive Pet");
                    return;
                }
                return;
            }
            else if (Me.Pet.HealthPercent < 70 && !Me.Pet.HasAura("Mend Pet"))
            {
                Cast("Mend Pet");
            }

            if (Styx.BotManager.Current.Name != "LazyRaider")
            {
                if (Rotating)
                {
                    Me.SetFacing(RotatePoint);
                    if (Me.IsFacing(RotatePoint))
                    {
                      cctc("Disengage");
                            Rotating = false;
                        
                    }
                }
                else
                {
                    //Face the target
                    Face(Target);

                    if (Target.Distance > 35.0f && Target.InLineOfSightOCD)
                    {
                        Move(Target.Location);
                    }
                    else if (StyxWoW.Me.CurrentTarget.Distance <= SafeMeleeRange + 3f && StyxWoW.Me.CurrentTarget.IsAlive && (StyxWoW.Me.CurrentTarget.CurrentTarget == null || StyxWoW.Me.CurrentTarget.CurrentTarget != StyxWoW.Me))
                    {
                        backpedal();
                    }
                    else if (Target.Distance < 35.0f && Target.Distance > 10.0f)
                    {
                        WoWMovement.MoveStop();
                    }


                }
            }

            if (Target.IsWithinMeleeRange)
            {
                if ((!IsMyAuraActive(Target, "Wing Clip") || Target.MovementInfo.ForwardSpeed >= 7.0f) && CanCast("Wing Clip"))
                {
                    Cast(Target, "Wing Clip");
                }

                if (CanCast("Disengage") && !Rotating)
                {
                    FindPoint();
                }

            }
            else if (Target.Distance <= 40d && Target.InLineOfSight)
            {

                if (Me.IsCasting)
                {
                    return;
                }
                else if (CanCast("Kill Shot"))
                {
                    AuraSwap("Aspect of the Hawk");
                    Cast(Target, "Kill Shot");
                }
                else if (Target.IsCasting && CanCast("Silencing Shot"))
                {
                    Cast(Target, "Silencing Shot");
                }
                else if (isAuraActive("Fire!") && CanCast("Aimed Shot"))
                {
                    AuraSwap("Aspect of the Hawk");
                    Cast(Target, "Aimed Shot");
                }
                else if (CanCast("Chimera Shot"))
                {
                    AuraSwap("Aspect of the Hawk");
                    Cast(Target, "Chimera Shot");
                }
                else if ((Target.HealthPercent > 80 && !Me.IsMoving) && CanCast("Aimed Shot"))
                {
                    AuraSwap("Aspect of the Hawk");
                    Cast(Target, "Aimed Shot");
                }
                else if (Me.FocusPercent > 50)
                {
                    AuraSwap("Aspect of the Hawk");
                    cctc(Target, "Arcane Shot");
                }
                else if (Me.FocusPercent <= 50)
                {
                    if (Me.IsMoving)
                    {
                        AuraSwap("Aspect of the Fox");
                    }
                    cctc("Rapid Fire");
                    Cast(Target, "Steady Shot");
                }

            }


        }
        private void AuraSwap(String Aura)
        {
            if (!isAuraActive(Aura))
            {
                //Check and see if aspects are on CD..
                if (AspectoftheHawk.CooldownTimeLeft.Milliseconds > 0)
                {
                    Logging.Write("Sleeping for " + AspectoftheHawk.CooldownTimeLeft.Milliseconds.ToString() + " Milliseconds");
                    Thread.Sleep(AspectoftheHawk.CooldownTimeLeft.Milliseconds);
                }

                Cast(Aura);
            }
        }
        public void Usetrinkets()
        {
            foreach (Trinket t in Trinkets)
            {

                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains(t.TrinketName) && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    if ((t.StackName != null) && (HasAuraStacks(t.StackName, t.StackNumber, Me)) && Target.HealthPercent < t.Trigger)
                    {
                        Logging.Write("Trinket one");
                        StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                    }
                    else if (t.StackName == null)
                    {
                        Logging.Write("Trinket one");
                        StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                    }
                }
                else if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Name == t.TrinketName && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    if ((t.StackName != null) && (HasAuraStacks(t.StackName, t.StackNumber, Me)) && Target.HealthPercent < t.Trigger)
                    {
                        Logging.Write("Trinket two");
                        StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                    }
                    else if (t.StackName == null)
                    {
                        Logging.Write("Trinket two");
                        StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                    }
                }

            }
        }

        System.Timers.Timer Heartbeat;
        /// <summary>
        /// Gonna try something interesting, using a timer rather then pulse to check for target usage, as it seems sometimes pulse stops getting called
        /// </summary>
        public override void Initialize()
        {
            Logging.Write("Init gets called");
            Heartbeat = new System.Timers.Timer(200);
            Heartbeat.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            Heartbeat.Enabled = true;
            
            SpellManager.Spells.TryGetValue("Aspect of the Hawk", out AspectoftheHawk);
            //base.Initialize();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Me.Mounted && !Me.GotTarget && !Me.Dead)
            {
                SeekTarget();
            }
        }


        public bool HasAuraStacks(String aura, int stacks, WoWUnit unit)
        {
            if (unit.ActiveAuras.ContainsKey(aura))
            {
                return unit.ActiveAuras[aura].StackCount >= stacks;
            }
            return false;
        }


        #region Casting

        void Cast(string Name)
        {
            Logging.Write(Name);
            SpellManager.Cast(Name);
        }
        void Cast(WoWUnit Who, string Name)
        {
            Logging.Write(Name + "@" + Who);
            SpellManager.Cast(Name, Who);
        }

        bool CanCast(string Name)
        {
            return SpellManager.CanCast(Name);
        }
        bool CanCast(WoWUnit Target, string Name)
        {
            return SpellManager.CanCast(Name, Target);
        }


        /// <summary>
        /// Can cast, then cast
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        bool cctc(string Name)
        {
            if (SpellManager.CanCast(Name))
            {
                Logging.Write(Name);
                SpellManager.Cast(Name);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool cctc(WoWUnit Who, String Name)
        {
            if (SpellManager.CanCast(Name))
            {
                Logging.Write(Name + "@" + Who);
                SpellManager.Cast(Name, Who);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

  
        public override void Pull()
        {


            if (!Me.GotTarget || !Me.CurrentTarget.IsAlive)
            {
                SeekTarget();
            }
            else
            {

                //cctc(Target, "Attack");

                if (!StyxWoW.Me.IsAutoAttacking)
                {
                    Me.ToggleAttack();
                }
                /*if (!IsMyAuraActive(Target, "Hunter's Mark"))
                {
                    cctc(Target, "Hunter's Mark");
                }
                else
                {
                    cctc(Target, "Aimed Shot");
                    cctc(Target, "Attack");
                }*/
            }

        }

        private void SeekTarget()
        {
            WoWPlayer unit = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).
                Where(p => p.IsHostile && !p.IsTotem && !p.IsPet && !p.Dead && p.DistanceSqr <= (10 * 10)).
                OrderBy(u => u.HealthPercent).FirstOrDefault();

            if (unit == null)
            {
                unit = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).Where(
                                       p => p.IsHostile && !p.IsTotem && !p.IsPet && !p.Dead && p.DistanceSqr <= (35 * 35)).OrderBy(
                                           u => u.DistanceSqr).FirstOrDefault();
            }
            if (unit != null)
            {
                TargetUnit(unit);
                Move(unit.Location);
            }


        }

        #region Targeting

        /// <summary>
        /// Taken from singulars PVP helper function file, and then modified
        /// </summary>
        /// <param name="unit"></param>
        protected void TargetUnit(WoWUnit unit)
        {
            Logging.Write("Targeting " + unit.Name);
            BotPoi.Current = new BotPoi(unit, PoiType.Kill);
            Tar(unit);
        }


        //From singular
        public static float MeleeRange
        {
            get
            {
                // If we have no target... then give nothing.
                if (StyxWoW.Me.CurrentTargetGuid == 0)
                    return 0f;

                return Math.Max(5f, StyxWoW.Me.CombatReach + 1.3333334f + StyxWoW.Me.CurrentTarget.CombatReach);
            }
        }


        public static float SafeMeleeRange { get { return Math.Max(MeleeRange - 1f, 5f); } }

        void backpedal()
        {
            var moveTo = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, StyxWoW.Me.CurrentTarget.Location, SafeMeleeRange + 10f);

            if (Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo))
            {
                Navigator.MoveTo(moveTo);
            }

        }
        //End singular


        private void Move(WoWPoint loc)
        {
            if (Styx.BotManager.Current.Name != "LazyRaider")
            {
                Navigator.MoveTo(loc);
            }
        }


        private void Tar(WoWUnit tar)
        {
            if (Styx.BotManager.Current.Name != "LazyRaider")
            {
                Target = tar;
                tar.Target();
                WoWMovement.ConstantFace(tar.Guid);
            }
        }

        private void Face(WoWUnit tar)
        {
            if (Styx.BotManager.Current.Name != "LazyRaider")
            {
                tar.Face();
            }
        }
        #endregion



        public override void PreCombatBuff()
        {
            cctc(Me, Buffcheck());
        }


        public override bool NeedPreCombatBuffs
        {
            get
            {
                return (Buffcheck() != null);
            }
        }

        public override bool NeedHeal
        {
            get
            {
                return HealCheck();
            }
        }



        public override void Heal()
        {

            foreach (Act action in HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    Cast(action.Spell);
                }
            }


        }



        /// <summary>
        /// Return the name of the buff we need to use
        /// </summary>
        /// <returns></returns>
        private string Buffcheck()
        {
            return null;
        }



        /// <summary>
        /// Runs through all the checks in the healrotation
        /// </summary>
        /// <returns></returns>
        private bool HealCheck()
        {
            foreach (Act action in HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    return true;
                }
            }
            return false;
        }



        private bool isAuraActive(string name)
        {
            //Me.Auras.con
            return Me.HasAura(name);
        }

        private bool IsMyAuraActive(WoWUnit Who, String What)
        {
            if (Who == null)
                return false;

            foreach (KeyValuePair<String, WoWAura> x in Who.ActiveAuras)
            {
                if (x.Value.Name.Equals(What) && x.Value.CreatorGuid == Me.Guid)
                {
                    return true;
                }
            }
            return false;
        }


    }
}