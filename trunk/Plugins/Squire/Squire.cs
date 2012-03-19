/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS PLUGIN WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * Squire Plugin
 * 
 * Author:  Bobby53
 * 
 */

// #define HIDE_PLAYER_NAMES

using Styx.Logic.Combat;   
using System.Drawing;
using Styx.Logic;
using System;
using Styx.Helpers;
using Styx.Logic.Pathing;
using System.Threading;
using System.Diagnostics;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using Styx.Plugins.PluginClass;
using Styx.Combat.CombatRoutine;
using Styx;
using System.Windows.Forms;

namespace Bobby53
{

    public class Squire : HBPlugin
    {
        public override string Name { get { return "Squire"; } }
        public override string Author { get { return "Bobby53"; } }
        public override Version Version { get { return new Version(1, 0, 01); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Squire Options..."; } }

        private void slog(string format, params object[] args)
        {
            Logging.Write(Color.Olive, "[Squire] " + format, args);
        }

        private void dlog(string format, params object[] args)
        {
            Logging.WriteDebug(Color.Olive, "<Squire> " + format, args);
        }

        private static LocalPlayer Me { get { return ObjectManager.Me; }}
        private static WoWPlayer leader;
        private static Stopwatch lastLeadCheck;
        private Stopwatch needRestDelay;
        private Stopwatch movePointDelay;

        private int lastLeadMovement = 0;

        private bool haveInitialized = false;

        private int followDistance = 0;
        private int levelPulse = 0;

        public override void Pulse()
        {
            try
            {
                while (IsGameStable() && Styx.Logic.BehaviorTree.TreeRoot.IsRunning && Battlegrounds.IsInsideBattleground)
                {
                    if (Me.CurrentHealth <= 1 || Me.Combat || Me.IsCasting || Battlegrounds.Finished)
                        return;

                    if (!haveInitialized)
                    {
                        Initialize();
                    }

                    if (Battlegrounds.Current == BattlegroundType.IoC || Battlegrounds.Current == BattlegroundType.SotA)
                    {
                        slog("battleground '{0}' not supported, running profile only", Battlegrounds.Current.ToString());
                        return;
                    }

                    // during prep period of bg let bot handle buffs, timing, etc
                    if (Me.Auras.ContainsKey("Preparation"))
                    {
                        while (IsGameStable() && Me.CurrentHealth > 1 && RoutineManager.Current.NeedPullBuffs)
                        {
                            slog("Applying Pull Buffs");
                            RoutineManager.Current.PullBuff();
                            StyxWoW.SleepForLagDuration();
                        }

                        while (IsGameStable() && Me.CurrentHealth > 1 && RoutineManager.Current.NeedPreCombatBuffs)
                        {
                            slog("Applying Pre-Combat Buffs");
                            RoutineManager.Current.PreCombatBuff();
                            StyxWoW.SleepForLagDuration();
                        }

                        return;
                    }

                    // battleground is running, so validate our leader
                    if (leader != null)
                    {
                        if (!ObjectManager.ObjectList.Contains(leader) || !leader.IsValid)
                        {
                            slog("leader is no longer in group");
                            leader = null;
                        }
                        else if (leader.CurrentHealth <= 1)
                        {
                            slog("leader died");
                            leader = null;
                        }
                        else if (leader.OnTaxi)
                        {
                            slog("leader is hitching a ride");
                            leader = null;
                        }
                        else if (leader.IsStealthed)
                        {
                            slog("leader is stealthed");
                            leader = null;
                        }
                        else if (leader.Distance > (SquireSettings.Instance.LeaderScanRange + SquireSettings.Instance.LeaderOutOfRange))
                        {
                            slog("leader out of range at {0:F1} yds", leader.Distance);
                            leader = null;
                        }
                    }

                    // no leader? then find one
                    if (leader == null)
                    {
                        if (lastLeadCheck.IsRunning && lastLeadCheck.ElapsedMilliseconds < 2000)
                            return;

                        lastLeadCheck.Reset();
                        lastLeadCheck.Start();

                        leader = FindLeader();
                        SetFocus(leader);       // null clears focus
                        if (leader == null)
                        {
                            slog(">>> no leader found, running profile till we find one...");
                            return;
                        }

                        lastLeadMovement = System.Environment.TickCount + (SquireSettings.Instance.StillTimeout * 1000);
                    }

                    // update movement timeout if they are doing something
                    if (leader.IsMoving || leader.IsCasting)
                        lastLeadMovement = System.Environment.TickCount + (SquireSettings.Instance.StillTimeout * 1000);

                    // TODO:  check if near an objective, since its okay to wait there in a group
                    // if they havent done anything, bail on the current leader
                    if (lastLeadMovement < System.Environment.TickCount)
                    {
                        slog("leader still for {0} seconds, blacklisting for {1} seconds", SquireSettings.Instance.StillTimeout, SquireSettings.Instance.BlacklistTime);
                        Blacklist.Add(leader, TimeSpan.FromSeconds(SquireSettings.Instance.BlacklistTime));
                        leader = null;
                        continue;
                    }

                    if ((leader.Mounted || leader.Distance > 50) && Me.IsOutdoors && !Me.Mounted && Mount.CanMount())
                    {
                        WaitForMount();
                    }

                    WoWUnit target = null;

                    if (!IsUnitInRange(leader, Me.IsIndoors ? 10 : followDistance ))
                    {
                        // slog("leader out of range, moving towards now...");
                        if (movePointDelay.ElapsedMilliseconds > SquireSettings.Instance.MoveDelayMS || (!Me.IsMoving && leader.IsMoving))
                        {
                            movePointDelay.Reset();
                            movePointDelay.Start();
                            if (!MoveToUnit(leader, 5))
                            {
                                slog("Unable to move to leader {0}, resetting...", Safe_UnitName(leader));
                                Blacklist.Add(leader, TimeSpan.FromSeconds(10));
                                leader = null;
                            }
                        }

                        if (!Me.Mounted)
                            target = FindTarget();

                        if (target == null)
                            continue;   // moving now, so just loop and check again in awhile
                    }

                    if (!leader.Mounted && Me.Mounted)
                    {
                        slog("leader not mounted, dismounting...");
                        WaitForDismount();
                    }


                    if (!leader.Combat && !leader.IsMoving && Me.IsMoving)
                    {
                        slog("leader not moving, stopping...");
                        WaitForStop();
                    }

                    CallPulse();
                    CallRest();
                    CallHeal();
                    // if (GetActualStrategy() == SquireSettings.CombatStrategy.DPS)
                    {
                        // if (!Me.Combat && leader.Combat)
                        {

                            // have a target? Then PULLL!!!!!!!!
                            if (target != null )
                            {
                                CallPull(target);
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException )
            {
                slog("Leader is no longer valid, resetting...");
                leader = null;
            }
            catch (Exception e)
            {
                slog( "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> EXCEPTION");
                Logging.WriteException(e);
            }

            // at this point, they either stopped the bot or we are in Combat

            // now fall through and let default behavior take place
        }


        public override void Initialize()
        {
            haveInitialized = true;
            slog("Version {0} Running", Version);

            if (!LevelbotSettings.Instance.UseMount)
            {
                slog("HonorBuddy setup for [Use Mount]={0}, [Mount Name]='{1}'", 
                    LevelbotSettings.Instance.UseMount ? "ON" : "OFF",
                    LevelbotSettings.Instance.MountName
                    );
            }

#if COMMENT
            if (StyxSettings.Instance.UseExperimentalPathFollowing)
            {
                StyxSettings.Instance.UseExperimentalPathFollowing = false;
                slog("Disabling HonorBuddy setting 'Use Experimental Path Following'");
            }
#endif
            bool isCaster = IsRangedFollower();
            followDistance = isCaster ? SquireSettings.Instance.FollowDistanceRanged : SquireSettings.Instance.FollowDistanceMelee;
            slog("Using follow distance of {0} yds for your {1} {2} as a {3}", followDistance, isCaster ? "Ranged" : "Melee", Me.Class, GetActualStrategy());

            lastLeadCheck = new Stopwatch();
            needRestDelay = new Stopwatch();
            movePointDelay = new Stopwatch();

            lastLeadCheck.Start();
            needRestDelay.Start();
            movePointDelay.Start();
        }

        private static List<WoWPlayer> GroupList
        {
            get
            {
                return Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            }
        }

        private bool IsGameStable()
        {
            return Me != null && ObjectManager.IsInGame;
        }

        private bool IsHostile(WoWUnit unit)
        {
            if (unit == null)
                return false;

            if (unit.IsPlayer)
                return Me.IsHorde != unit.ToPlayer().IsHorde;

            return !unit.IsFriendly && unit.Attackable;
        }


        private static string Safe_UnitName(WoWUnit unit)
        {
            if (unit == null)
                return "(null)";

#if HIDE_PLAYER_NAMES
			if (unit.IsMe)
				return "-me-";
			else if (unit.IsPlayer ) // && Safe_IsFriendly(unit)) // !unit.IsHostile ) // unit.IsFriendly)
				return unit.Class.ToString() + "." + unit.MaxHealth.ToString();
#else
            if (unit.IsPlayer) // && Safe_IsFriendly(unit)) // !unit.IsHostile ) // unit.IsFriendly)
                return unit.Name + "." + unit.Class.ToString() + (unit.ToPlayer().IsHorde ? "[H]" : "[A]");
#endif

            return unit.Name;
        }

        private static string Right(string s, int c)
        {
            return s.Substring(c > s.Length ? 0 : s.Length - c);
        }

        private void BufferedNeedRest()
        {
            return;
        }

        private WoWPlayer FindLeader()
        {
            WoWPlayer leader = null;

            if (SquireSettings.Instance.Method == SquireSettings.FollowMethod.Focus)
            {
                leader = GetFocus();
                if ( leader != null )
                    slog(">>> following current focus [{0}]", Safe_UnitName(leader));
            }
            else if ( SquireSettings.Instance.Method == SquireSettings.FollowMethod.Health )
            {
                leader = (from p in GroupList
                    where p.Distance <= SquireSettings.Instance.LeaderScanRange
                        && p.CurrentHealth > 1
                        && !p.IsMe 
                        && !p.OnTaxi
                        && !p.IsStealthed 
                        && !Blacklist.Contains(p)
                        && p.Class != WoWClass.Rogue 
                        && p.Class != WoWClass.Druid 
                    orderby 
                        p.MaxHealth descending
                    select p
                    ).FirstOrDefault();

                if ( leader != null )
                    slog(">>> following {0} with max health {1}", Safe_UnitName(leader), leader.MaxHealth);
            }
            else
            {
                var t =(from o in ObjectManager.ObjectList
                        where o is WoWPlayer && o.Distance <= SquireSettings.Instance.LeaderScanRange 
                        let p = o.ToPlayer()
                        where p.IsHorde == Me.IsHorde
                            && p.CurrentHealth > 1
                            && !p.IsMe
                            && p.Class != WoWClass.Rogue
                            && p.Class != WoWClass.Druid
                            && !Blacklist.Contains(p)
                        let c = (from oo in ObjectManager.ObjectList
                                    where oo is WoWPlayer
                                        let pp = oo.ToPlayer()
                                    where pp.Location.Distance( p.Location ) < 40 
                                        && pp.IsHorde == p.IsHorde
                                        && pp.CurrentHealth > 1
                                    select pp).Count()
                        orderby c descending, p.Distance ascending 
                        select new {Player = p, Count = c}).FirstOrDefault();

                if ( t != null && t.Count >= SquireSettings.Instance.FollowMinDensity)
                {
                    leader = t.Player;
                    slog(">>> following {0} with player density {1}", Safe_UnitName(leader), t.Count );
                }
            }

            if (leader == null)
                slog(">>> no leader found, running profile till we find one...");

            return leader;
        }

        private static bool IsUnitInRange(WoWUnit unit, double range)
        {
            return (unit != null && unit.Distance < range && unit.InLineOfSight);
        }

        private bool MoveTo(WoWPoint newPoint)
        {
            float distToMove = Me.Location.Distance(newPoint);

//          if (Navigator.CanNavigateFully(Me.Location, leader.Location, (int)searchDist * 3))
            Navigator.MoveTo(newPoint);
            return true;

//            slog("Cannot generate navigation path to new position");
//            return false;
        }

        private bool MoveToUnit(WoWUnit unit, int howClose)
        {
            // slog("MoveToUnit: moving to {0}:{1} thats {2:F2} yds away", unit.IsPlayer ? "player" : "npc", Safe_UnitName(unit), unit.Distance);
            WoWPoint newPoint = WoWMovement.CalculatePointFrom(unit.Location, howClose);
            return MoveTo(unit.Location);      // WoWMovement.ClickToMove(newPoint);
        }

        private void SetTarget( WoWUnit unit)
        {
            if (unit != null)
            {
                unit.Target();
                while (IsGameStable() && ObjectManager.ObjectList.Contains(unit) && Me.CurrentTarget != unit)
                {
                    Thread.Sleep(10);
                }
            }
        }

        private WoWUnit FindTarget()
        {
            WoWUnit target = null;

            if (Me.GotTarget && IsHostile(Me.CurrentTarget))
                target = Me.CurrentTarget;

            // check to assist leader
            if (target == null && leader.Combat && leader.GotTarget && IsHostile(leader.CurrentTarget) && 40 >= leader.CurrentTarget.Distance)
            {
                target = leader.CurrentTarget;
                slog("Target: assisting the leader on {0}", Safe_UnitName(target));
            }

            // check to assist nearby teammate
            if (target == null)
            {
                // find nearby teammate and /assist
                target = (from p in GroupList
                          where p.CurrentHealth > 1 
                          && p.GotTarget 
                          && p.CurrentTarget.Distance <= 40 
                          && p.InLineOfSight 
                          && IsHostile(p.CurrentTarget)
                          orderby p.Distance ascending
                          select p.CurrentTarget).FirstOrDefault();
                if (target != null)
                    slog("Target: assisting a nearby friendly with {0}", Safe_UnitName(target));
            }

            // check to find any nearby enemy and attack
            if (target == null)
            {
                target = (from o in ObjectManager.ObjectList
                          where o is WoWPlayer
                          let p = o.ToPlayer()
                          where p.Distance <= 40 
                          && p.CurrentHealth > 1 
                          && p.IsHorde != Me.IsHorde
                          && p.InLineOfSight
                          orderby p.Distance ascending
                          select p).FirstOrDefault();
                if (target != null)
                    slog("Target: selected a nearby hostile {0}", Safe_UnitName(target));
            }

            return target;
        }

        private void WaitForCurrentSpell()
        {
            int castWait = System.Environment.TickCount + 10000;
            while (IsGameStable() && Me.CurrentHealth > 1 && Me.IsCasting)
            {
                if (castWait < System.Environment.TickCount)
                {
                    slog("ERROR:  Waited 10+ secs for cast to finish-- moving anyway");
                    break;
                }
            }
        }

        public void WaitForStop()
        {
            // excessive attempt to make sure HB doesn't have any cached movement
            WoWMovement.MoveStop(WoWMovement.MovementDirection.AutoRun);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards );
            WoWMovement.MoveStop(WoWMovement.MovementDirection.ClickToMove );
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Descend );
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward );
            WoWMovement.MoveStop(WoWMovement.MovementDirection.ForwardBackMovement);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.PitchDown);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.PitchUp);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeLeft);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeRight);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.TurnLeft);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.TurnRight);

            WoWMovement.MoveStop(WoWMovement.MovementDirection.All);

            WoWMovement.MoveStop();

            do 
            {
                StyxWoW.SleepForLagDuration();
            } while (IsGameStable() && Me.CurrentHealth > 1 && Me.IsMoving);
        }

        public void WaitForDismount()
        {
            while (IsGameStable() && Me.CurrentHealth > 1 && Me.Mounted)
            {
                Lua.DoString("Dismount()");
                // Mount.Dismount();  // HB API forces Stop also, so use LUA to keep running and let Squire or CC stop if needed
                StyxWoW.SleepForLagDuration();
            }
        }

        public void WaitForMount()
        {
            if (Me.Combat || Me.IsIndoors || !LevelbotSettings.Instance.UseMount)      
                return;

            WaitForStop();
            WoWPoint ptStop = Me.Location;

            var timeOut = new Stopwatch();
            timeOut.Start();    

            if (!Mount.CanMount())
                return;

            slog("Attempting to mount via HB...");
            Mount.MountUp();   
            StyxWoW.SleepForLagDuration();
#if MOUNT_HACKS
            if ( !Me.IsCasting && !Me.Mounted )
            {
                // HACK #1 
                string sMount = LevelbotSettings.Instance.MountName;
                if (string.IsNullOrEmpty(sMount))
                    slog("No mount name entered in HonorBuddy General Settings");
                else
                {
                    slog("Attempting to mount via LUA and configured {0}", sMount);
                    Lua.DoString("/use " + sMount);
                    StyxWoW.SleepForLagDuration();
                }
            }

            if ( !Me.IsCasting && !Me.Mounted )
            {
                // HACK #2
                MountHelper.MountWrapper mnt = MountHelper.GroundMounts.FirstOrDefault();
                if ( mnt != null )
                {
                    slog("Attempting to mount via LUA and found mount {0}", mnt.Name );
                    Lua.DoString( "/use " + mnt.Name );
                    StyxWoW.SleepForLagDuration();
                }
            }
#endif
            while (IsGameStable() && Me.CurrentHealth > 1 && Me.IsCasting)
            {
                Thread.Sleep(75);
            }

            if (!Me.Mounted)
            {
                slog("unable to mount after {0} ms", timeOut.ElapsedMilliseconds);
                if (ptStop.Distance(Me.Location) != 0)
                    slog("character was stopped but somehow moved {0:F3} yds while trying to mount", ptStop.Distance(Me.Location));
            }
            else
            {
                slog("Mounted");
            }
        }

        public void SetFocus(WoWUnit unit)
        {
            if (unit != null)
                Lua.DoString("FocusUnit(\"" + unit.Name + "\")");
            else if ( SquireSettings.Instance.Method != SquireSettings.FollowMethod.Focus)
                Lua.DoString("ClearFocus()");
        }

        public WoWPlayer GetFocus()
        {
            string sUnitNameInfo = Lua.GetReturnVal<string>("return GetUnitName(\"focus\", 0)", 0);
            if (String.IsNullOrEmpty(sUnitNameInfo))
                return null;

            string[] parts = sUnitNameInfo.Split(' ');
            string sUnitName = parts[0];

            WoWPlayer player = 
               (from p in GroupList
                where !p.IsMe && p.Name.ToUpper() == sUnitName.ToUpper()
                select p
               ).FirstOrDefault();

            if (player == null)
                dlog("GetFocus:  no focus set by user");

            return player;
        }

        public bool IsRangedFollower()
        {
            switch (Me.Class)
            {
                case WoWClass.Druid:
                    return !SpellManager.HasSpell("Mangle");
                case WoWClass.Hunter:
                    return true;
                case WoWClass.Mage:
                    return true;
                case WoWClass.Paladin:
                    return SpellManager.HasSpell("Walk in the Light");
                case WoWClass.Priest:
                    return true;
                case WoWClass.Shaman:
                    return SpellManager.HasSpell("Shamanism") || SpellManager.HasSpell("Meditation");
                case WoWClass.Warlock:
                    return true;
            }

            return false;
        }

        public bool IsHealer()
        {
            return SpellManager.HasSpell("Meditation");
        }

        private SquireSettings.CombatStrategy GetActualStrategy()
        {
            if (SquireSettings.Instance.Strategy != SquireSettings.CombatStrategy.Auto )
                return SquireSettings.Instance.Strategy;

            if ( SpellManager.HasSpell("Meditation"))
                return SquireSettings.CombatStrategy.Healer;

            return SquireSettings.CombatStrategy.DPS;
        }

        public override void OnButtonPress()
        {
            MessageBox.Show(
                "To configure Squire, edit the following file and click Recompile:" + Environment.NewLine
                + Environment.NewLine + string.Format(@"Plugins/Squire/Squire_{0}.xml", StyxWoW.Me.Name) + Environment.NewLine
                + Environment.NewLine
                ,
                "Squire v" + Version.ToString());
        }


        private static int tickRest;

        private bool CallRest()
        {
            if (tickRest > Environment.TickCount )
                return false;

            if (RoutineManager.Current.RestBehavior == null)
            {
                while (IsGameStable() && !Me.Combat && Me.CurrentHealth > 1 && RoutineManager.Current.NeedRest)
                {
                    slog("Invoking Rest");
                    RoutineManager.Current.Rest();
                    StyxWoW.SleepForLagDuration();
                }
            }
            else
            {
                Run( "Rest", RoutineManager.Current.RestBehavior);
            }

            tickRest = Environment.TickCount + 150;
            return false;
        }

        private bool CallBuff()
        {
            return false;
        }

        private bool CallHeal()
        {
            if (RoutineManager.Current.HealBehavior == null)
            {
                while (IsGameStable() && !Me.Combat && Me.CurrentHealth > 1 && RoutineManager.Current.NeedHeal)
                {
                    slog("Invoking Heal");
                    RoutineManager.Current.Heal();
                    StyxWoW.SleepForLagDuration();
                }
            }
            else
            {
                Run( "Heal", RoutineManager.Current.HealBehavior);
            }

            return false;
        }

        private bool CallPulse()
        {
            levelPulse++;
            if (levelPulse == 1)
            {
                dlog("Pulse");
                RoutineManager.Current.Pulse();
            }
            else
                dlog("reentrant Pulse call... don't propogate");
            levelPulse--;
            return false;
        }

        private bool CallPull( WoWUnit target)
        {
            if (Me.Combat)
                return false;

            SetTarget(target);

            if (RoutineManager.Current.PullBuffBehavior == null)
            {
                while (IsGameStable() && Me.CurrentHealth > 1 && !Me.Combat && RoutineManager.Current.NeedPullBuffs)
                {
                    dlog("PullBuff");
                    RoutineManager.Current.PullBuff();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Run( "Pull Buff", RoutineManager.Current.PullBuffBehavior);
            }

            int tickStop = Environment.TickCount + 8000;
            if (RoutineManager.Current.PullBehavior == null)
            {
                while (IsGameStable() && Me.CurrentHealth > 1 && !Me.Combat && tickStop > Environment.TickCount)
                {
                    slog("Pull {0} @ {1:F1} yds, health={2:F1}%", Safe_UnitName(target), target.Distance, target.HealthPercent );
                    RoutineManager.Current.Pull();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Run( "Pull", RoutineManager.Current.PullBehavior);
            }

            return false;
        }

        private bool Run(string s, TreeSharp.Composite bt)
        {
            int nLoops = 0;
            bt.Start(this);
            do
            {
                bt.Tick(this);
                nLoops++;
            } while (bt.IsRunning);
            bt.Stop(this);
            if (nLoops > 1)
                slog("Run('{0}'):  performed {1} iterations", s, nLoops);
            return false;
        }
    }
}

