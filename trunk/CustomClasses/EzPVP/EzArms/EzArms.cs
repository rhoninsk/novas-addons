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
namespace EzArms
{
    class Warrior : CombatRoutine
    {


        public override sealed string Name { get { return "EzArms - " + ver; } }
        public override WoWClass Class { get { return WoWClass.Warrior; } }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }


        private Version ver = new Version(1, 4, 0);

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
        private float HealthStone = 35.0f;//What percent to use health stone.
        List<String> Rotation = new List<String> { "Victory Rush", "Throwdown", "Colossus Smash", "Mortal Strike", "Execute", "Overpower", "Heroic Throw", "Slam" };
        List<String> SmashRotation = new List<String> { "Mortal Strike", "Overpower", "Execute", "Slam" };
        List<String> Interrupts = new List<String> { "Pummel", "Intimidating Shout", "Arcane Torrent" };
        List<String> CD = new List<String> { "Deadly Calm", "Blood Fury" };
        List<Act> HealRotation = new List<Act> { new Act("Enraged Regeneration", 30.0f), new Act("Rallying Cry", 40.0f), new Act("Retaliation", 60.0f) };
        List<Trinket> Trinkets = new List<Trinket> { new Trinket("Badge of Victory", null, 0, 100.0f), new Trinket("Essence of the Eternal Flame", null, 0, 100.0f), new Trinket("Apparatus of Khaz'goroth", "Titanic Power", 5, 60.0f), new Trinket("Fury of Angerforge", "Raw Fury", 5, 60.0f) };
        List<String> GapCloser = new List<String> { "Charge", "Heroic Leap" };
        List<String> Break = new List<String> { "Ice Block", "Hand of Protection", "Divine Shield" };
        WoWSpell Charge;
        public override void Combat()
        {
            Target = Me.CurrentTarget;

            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;


            if (Target == null || !Target.Attackable)
            {
                return;
            }

            //Lets try some new logic here..lets go for the owner
            else if (Target != null && Target.IsPet)
            {
                WoWUnit Owner = Target.CreatedByUnit;
                if (Owner != null && Owner.IsValid && Owner.IsAlive)
                {
                    Blacklist.Add(Target, new TimeSpan(0, 0, 5));
                    Logging.Write("Changing targets to pet owner");
                    Target = Owner;
                    TargetUnit(Target);
                }
            }
            //Face the target
            Face(Target);
            //Always try and move ontop of the enemy target


            if (Me.IsCasting)
            {
                if (Me.CastingSpell.Name != "Shattering Throw")
                {
                    Stop();
                }
                else
                {
                    Move(Target.Location);
                }
            }
            else
            {
                Move(Target.Location);
            }



            if (Target.Distance < 2)
                Stop();

            if ((Target.Distance > 30 || !Target.IsAlive) && Me.Combat && Manual)
            {
                Logging.Write(Target.Name + " is currently " + Target.Distance.ToString() + " dropping target");
                Me.ClearTarget();
                SeekTarget();
            }
            else if ((Target.Distance >= 8d && Target.Distance < 30d))
            {
                if (CanCast(Target, "Charge"))
                {
                    Cast(Target, "Charge");
                }
                else if (CanCast("Heroic Leap") && Charge.CooldownTimeLeft.Seconds > 0 && Charge.CooldownTimeLeft.Seconds < 8)
                {
                    SpellManager.Cast("Heroic Leap");
                    LegacySpellManager.ClickRemoteLocation(StyxWoW.Me.CurrentTarget.Location);
                }
            }

            else if (Target.IsWithinMeleeRange)
            {


                if (IsTargetInvin(Target) && CanCast("Shattering Throw"))
                {
                    Stop();
                    Cast(Target, "Shattering Throw");
                }
                else if (Target.IsCasting && Target.CanInterruptCurrentSpellCast)
                {
                    foreach (String Ability in Interrupts)
                    {
                        if (cctc(Target, Ability))
                        {
                            Thread.Sleep(50);
                            break;
                        }
                    }
                }

                //if Colossus Smash is up, use trinkets and skip everything else
                else if (IsMyAuraActive(Target, "Colossus Smash"))
                {
                    Usetrinkets();
                    //HS is off the global, use it first so we can use another ability
                    if (Me.RagePercent > 85.0f && CanCast("Heroic Strike"))
                        Cast(Target, "Heroic Strike");

                    foreach (String Ability in SmashRotation)
                    {
                        if (cctc(Target, Ability))
                        {
                            break;
                        }
                    }

                }
                else if (Target.MovementInfo.CurrentSpeed >= 7 && Slowable(Target))
                {
                    //Check if theres multltiple targets nearby

                    Cast("Piercing Howl");
                }
                else if (!IsMyAuraActive(Target, "Rend") && CanCast("Rend"))
                {
                    Cast(Target, "Rend");
                }
                else if (isAuraActive("Juggernaut") && (CanCast("Mortal Strike") || CanCast("Slam")))
                {
                    if (CanCast("Mortal Strike"))
                    {
                        Cast(Target, "Mortal Strike");
                    }
                    else
                    {
                        Cast(Target, "Slam");
                    }
                }

                else if (IsMyAuraActive(Target, "Colossus Smash") && CanCast("Recklessness") && CanCast("Bladestorm") && Manual) // && CanCast("Sweeping Strikes")
                {
                    cctc("Recklessness");
                    cctc("Sweeping Strikes");
                    cctc("Bladestorm");
                }
                else
                {



                    foreach (String Ability in CD)
                    {
                        cctc(Ability);
                    }

                    if (Me.RagePercent > 85.0f)
                    {
                        if (CanCast("Inner Rage") && !CanCast("Execute") && !CanCast("Heroic Strike") && !CanCast("Overpower") && !CanCast("Mortal Strike"))
                        {
                            Cast("Inner Rage");
                        }
                        cctc("Heroic Strike");

                    }


                    foreach (String Ability in Rotation)
                    {
                        if (cctc(Target, Ability))
                        {
                            break;
                        }
                    }
                }
            }


        }


        String[] Immunes = { "Hand of Freedom", "Divine Shield", "Ice Block", "Hand of Protection" };
        public Boolean Slowable(WoWUnit target)
        {

            foreach (string spell in Immunes)
            {
                if (target.HasAura(spell))
                {
                    return false;
                }
            }
            return true;
        }

        public void Usetrinkets()
        {
            foreach (Trinket t in Trinkets)
            {

                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains(t.TrinketName) && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    if (Target.HealthPercent <= t.Trigger)
                    {
                        if (string.IsNullOrEmpty(t.StackName))
                        {
                            Logging.Write("Trinket one");
                            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                        }
                        else if (HasAuraStacks(t.StackName, t.StackNumber, Me))
                        {
                            Logging.Write("Trinket one");
                            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                        }
                    }

                }
                else if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains(t.TrinketName) && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    if (Target.HealthPercent <= t.Trigger)
                    {
                        if (string.IsNullOrEmpty(t.StackName))
                        {
                            Logging.Write("Trinket two");
                            StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                        }
                        else if (HasAuraStacks(t.StackName, t.StackNumber, Me))
                        {
                            Logging.Write("Trinket two");
                            StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                        }
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
            SpellManager.Spells.TryGetValue("Charge", out Charge);
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

        /// <summary>
        /// Tricky latency stuff, will work on later
        /// </summary>
        /// <returns></returns>
        bool MeleeLatency()
        {
            return false;
        }



        public override void Pull()
        {

            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;

            Target = Me.CurrentTarget;
            if (!Me.GotTarget || !Me.CurrentTarget.IsAlive)
            {
                SeekTarget();
            }
            else
            {
                cctc(Target, "Attack");
                if (Target.Distance > 8d && Target.Distance < 40)
                {
                    if (CanCast("Charge"))
                    {
                        Cast("Charge");
                        //Thread.Sleep(150);
                    }
                    else if (CanCast("Heroic Leap") && Charge.CooldownTimeLeft.Seconds > 0 && Charge.CooldownTimeLeft.Seconds < 8)
                    {
                        SpellManager.Cast("Heroic Leap");
                        LegacySpellManager.ClickRemoteLocation(StyxWoW.Me.CurrentTarget.Location);
                    }
                }
                cctc(Target, "Heroic Throw");
                Move(Me.CurrentTarget.Location);
            }

        }

        private void SeekTarget()
        {

            if (BotManager.Current.Name != "BGBuddy" || !Battlegrounds.IsInsideBattleground)
                return;

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
            //BotPoi.Current = new BotPoi(unit, PoiType.Kill);
            Tar(unit);
        }

        private void Move(WoWPoint loc)
        {
            if (Manual)
            {
                Navigator.MoveTo(loc);
            }
        }

        private void Tar(WoWUnit tar)
        {
            if (Manual)
            {
                tar.Target();
                WoWMovement.ConstantFace(tar.Guid);
            }
        }

        private void Face(WoWUnit tar)
        {
            if (Manual)
            {
                tar.Face();
            }
        }

        private void Stop()
        {
            if (Manual)
            {
                Navigator.PlayerMover.MoveStop();
            }
        }

        #endregion

        private bool Manual
        {
            get { return (BotManager.Current.Name != "LazyRaider" && BotManager.Current.Name != "Raid Bot"); }
        }

        public override void PreCombatBuff()
        {
            cctc(Me, Buffcheck());
        }


        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (Me.Mounted)
                    return false;

                return (Buffcheck() != null);
            }
        }

        public override bool NeedHeal
        {
            get
            {
                return HealCheck() || (AmIFeared() && CanCast("Berserker Rage"));
            }
        }


        public override void Heal()
        {

            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;

            if (Me.HealthPercent <= HealthStone)
            {
                WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512);
                if (hs != null)
                {
                    hs.Use();
                }

            }

            foreach (Act action in HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    Cast(action.Spell);
                }
            }
            if (AmIFeared() && CanCast("Berserker Rage"))
            {
                Cast("Berserker Rage");
            }

        }



        /// <summary>
        /// Return the name of the buff we need to use
        /// </summary>
        /// <returns></returns>
        private string Buffcheck()
        {
            if (CanCast("Battle Shout") && !isAuraActive("Battle Shout") && !isAuraActive("Horn of Winter") && !isAuraActive("Strength Of Earth Totem") && !isAuraActive("Roar of Courage"))
            {
                return "Battle Shout";
            }


            return null;
        }

        private bool AmIFeared()
        {
            WoWAuraCollection Auras = ObjectManager.Me.GetAllAuras();
            foreach (WoWAura a in Auras)
            {
                if (!a.IsHarmful)
                {
                    continue;
                }
                if (a.Spell.Mechanic == WoWSpellMechanic.Fleeing)
                {
                    return true;
                }
            }
            return false;
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

            if (Me.HealthPercent <= HealthStone)
            {
                WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512);
                if (hs != null)
                {
                    return true;
                }

            }


            return false;
        }
        private bool isAuraActive(string name)
        {
            //Me.Auras.con
            return Me.ActiveAuras.ContainsKey(name);
        }
        private bool IsTargetInvin(WoWUnit Who)
        {
            foreach (String Ability in Break)
            {
                if (Who.ActiveAuras.ContainsKey(Ability))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsMyAuraActive(WoWUnit Who, String What)
        {
            return Who.GetAllAuras().Where(p => p.CreatorGuid == Me.Guid && p.Name == What).FirstOrDefault() != null;
        }


    }
}