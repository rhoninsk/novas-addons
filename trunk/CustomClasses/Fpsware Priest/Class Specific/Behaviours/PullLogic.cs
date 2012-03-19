using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;
using IDs = Athena.ClassHelpers.Priest.IDs;

namespace Athena
{
    public partial class Fpsware
    {
        
        #region Pull Timer / Timeout
        public class NeedToCheckPullTimer : Decorator
        {
            public NeedToCheckPullTimer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (IsLazyRaider) return false;

                return Target.PullTimerExpired;
            }
        }

        public class CheckPullTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Unable to pull {0}, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(120);
                Me.ClearTarget();

                return RunStatus.Success;
            }
        }
        #endregion

        #region Combat Timer / Timeout
        public class NeedToCheckCombatTimer : Decorator
        {
            public NeedToCheckCombatTimer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Utils.IsBattleground) return false;
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                if (Target.IsElite) return false;
                if (CT.Name.Contains("Training Dummy")) return false;
                if (Me.IsInInstance) return false;
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Restoration) return false;

                return Target.CombatTimerExpired && Target.IsHealthPercentAbove(98);
            }
        }

        public class CheckCombatTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Combat with {0} is bugged, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(60);
                Utils.LagSleep();

                return RunStatus.Success;
            }
        }
        #endregion

        #region Auto Attack During Pull
        public class NeedToAutoAttackPull : Decorator
        {
            public NeedToAutoAttackPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (Target.IsWithinInteractRange && !Me.IsAutoAttacking) return true;

                return false;
            }
        }

        public class AutoAttackPull : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.AutoAttack(true);

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Casting So Stop Processing
        public class NeedToCastingSoStopMoving : Decorator
        {
            public NeedToCastingSoStopMoving(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                return Me.IsCasting;
            }
        }

        public class CastingSoStopMoving : Action
        {
            protected override RunStatus Run(object context)
            {
                return RunStatus.Success;
            }
        }
        #endregion

        #region Distance Check Other
        public class NeedToDistanceCheckOther : Decorator
        {
            public NeedToDistanceCheckOther(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.ToUpper().Contains("ALWAYS")) return false;
                if (Me.IsCasting) return false;
                if (!Me.GotTarget || Me.GotTarget && Me.CurrentTarget.Dead) return false;

                const string pullSpell = "Smite";

                double minDistance = Spell.MaxDistance(pullSpell) - 7;
                double maxDistance = Spell.MaxDistance(pullSpell) - 1;


                if (!Target.IsFleeing && (!Me.CurrentTarget.IsMoving && !Me.IsMoving && Target.IsDistanceLessThan(Spell.MaxDistance(pullSpell)))) return false;
                if (Target.IsDistanceLessThan(minDistance) && Me.IsMoving) { WoWMovement.MoveStop(); return false; }
                if (Target.IsDistanceMoreThan(maxDistance)) return true;

                return false;
            }
        }

        public class DistanceCheckOther : Action
        {
            protected override RunStatus Run(object context)
            {
                const string pullSpell = "Smite";

                float distanceMoveTo = (float)Spell.MaxDistance(pullSpell) - 5;

                Movement.MoveTo(distanceMoveTo);
                Timers.Reset("Pull");

                return RunStatus.Success;
            }
        }
        #endregion







        #region Navigate Path
        public class NeedToNavigatePath : Decorator
        {
            public NeedToNavigatePath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (Settings.LazyRaider.Contains("always")) return false;

                return !Navigator.CanNavigateFully(Me.Location, CT.Location, 20);
            }
        }

        public class NavigatePath : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log("Can not navigate to target's location. Blacklisting", Utils.Colour("Red"));
                Target.BlackList(Utils.IsBattleground ? 10 : 30);

                return RunStatus.Success;
            }
        }
        #endregion

        #region Pull Spell
        public class NeedToPullSpell : Decorator
        {
            public NeedToPullSpell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.PullSpell;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.LazyRaider.Contains("always"))
                {
                    if (Target.IsDistanceMoreThan(Spell.MaxDistance(Settings.PullSpell)))
                    {
                        Movement.MoveTo((float)Spell.MaxDistance(Settings.PullSpell) - 5);
                        System.Threading.Thread.Sleep(1500);
                        return false;
                    }


                    if (Utils.IsInLineOfSight(CT.Location) && Me.IsMoving)
                    {
                        Movement.StopMoving();
                        Utils.LagSleep();
                    }
                }
                if (Me.IsMoving) return false;

                if (Spell.IsKnown(dpsSpell) && Spell.IsOnCooldown(dpsSpell)) return false;
                if (!Timers.Expired("Pull", 1500)) return false;
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PullSpell : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.PullSpell;

                Target.Face();
                Utils.LagSleep();

                bool result = Spell.Cast(dpsSpell);
                
                Utils.LagSleep();
                Utils.WaitWhileCasting();
                ObjectManager.Update();

                Timers.Reset("Pull");
                if (dpsSpell == "Vampiric Touch") Timers.Reset("VampiricTouch");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Smite Pull
        public class NeedToSmitePull : Decorator
        {
            public NeedToSmitePull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Smite";

                if (!Timers.Expired("Pull",1500)) return false;     // Prevent Smite spam during pull. HB does not recognise we are in combat fast enough. 
                if (Spell.IsKnown(Settings.PullSpell) && !Spell.IsOnCooldown(Settings.PullSpell)) return false;        // If you can cast the selected pull spell bail out here
                if (Spell.CanCast(Settings.PullSpell) && !Spell.IsOnCooldown(Settings.PullSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.IsMoving) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SmitePull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Smite";
                float spellDistance = SpellManager.Spells[dpsSpell].MaxRange;

                if (Target.Distance > spellDistance)
                {
                    Movement.DistanceCheck(spellDistance-1, spellDistance - 5);
                }

                bool result = Spell.Cast(dpsSpell);
                ObjectManager.Update();
                Utils.LagSleep();
                Timers.Reset("Pull");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shield Pull
        public class NeedToShieldPull : Decorator
        {
            public NeedToShieldPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Power Word: Shield";

                if (!CLC.ResultOK(Settings.PWSBeforePull)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(IDs.PowerWordShield, Self.AuraCheck.AllAuras)) return false;
                if (Self.IsBuffOnMe(IDs.WeakenedSoul, Self.AuraCheck.AllAuras)) return false;
                if (Self.IsBuffOnMe("Power Word: Shield")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShieldPull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Power Word: Shield";

                Spell.Cast(dpsSpell, Me);
                ObjectManager.Update();
                //Utils.LagSleep();

                //bool result = Self.IsBuffOnMe(dpsSpell);

                return RunStatus.Success;
            }
        }
        #endregion

        #region Face Pull
        public class NeedToFacePull : Decorator
        {
            public NeedToFacePull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (Me.IsMoving) return false;
                
                return (!Me.IsFacing(Me.CurrentTarget));

            }
        }

        public class FacePull : Action
        {
            protected override RunStatus Run(object context)
            {
                Target.Face();
                return RunStatus.Failure;
            }
        }
        #endregion





    }
}
