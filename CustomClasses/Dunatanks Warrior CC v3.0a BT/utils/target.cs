using System;
using System.Drawing;
using System.Linq;
using System.Threading;

using CommonBehaviors.Actions;

using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using System.Linq;

using TreeSharp;
using Styx.Logic.Combat;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Logic.Pathing;
using Styx.Logic.POI;

using TreeSharp;

using Action = TreeSharp.Action;

namespace Dunatanks3
{

    partial class Warrior
    {
        #region targeting
        //credits to Singular devs
        public bool NeedTankTargeting { get; set; }

        private static readonly WaitTimer targetingTimer = new WaitTimer(TimeSpan.FromSeconds(2));

        protected Composite CreateEnsureTarget()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => NeedTankTargeting && targetingTimer.IsFinished && Me.Combat &&
                               TankTargeting2.Instance.FirstUnit != null && Me.CurrentTarget != TankTargeting2.Instance.FirstUnit,
                        new Action(
                            ret =>
                            {
                                Logging.WriteDebug("Targeting first unit of TankTargeting");
                                TankTargeting2.Instance.FirstUnit.Target();
                                StyxWoW.SleepForLagDuration();
                                targetingTimer.Reset();
                            })),
                    new Decorator(
                        ret => Me.CurrentTarget == null || Me.CurrentTarget.Dead || Me.CurrentTarget.IsFriendly,
                        new PrioritySelector(
                            ctx =>
                            {
                                if (Me.IsInInstance)
                                    return null;
                                if (RaFHelper.Leader != null && RaFHelper.Leader.Combat)
                                {
                                    return RaFHelper.Leader.CurrentTarget;
                                }
                                if (Targeting.Instance.FirstUnit != null && Me.Combat)
                                {
                                    return Targeting.Instance.FirstUnit;
                                }
                                var units =
                                    ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
                                        p => p.IsHostile && !p.IsOnTransport && !p.Dead && p.DistanceSqr <= 70 * 70 && p.Combat);

                                if (Me.Combat && units.Any())
                                {
                                    return units.OrderBy(u => u.DistanceSqr).FirstOrDefault();
                                }

                                return null;
                            },
                            new Decorator(
                                ret => ret != null,
                                new Sequence(
                                    new Action(ret => Logging.Write("Target is invalid. Switching to " + Extensions.SafeName((WoWUnit)ret) + "!")),
                                    new Action(ret => ((WoWUnit)ret).Target()))),
                            new Decorator(
                                ret => Me.CurrentTarget != null,
                                new Action(
                                    ret =>
                                    {
                                        Me.ClearTarget();
                                        return RunStatus.Failure;
                                    })))));
        }

       

        #endregion

        #region movement
        //credits to Singular devs

        protected Composite CreateMoveToAndFace(float maxRange, float coneDegrees, UnitSelectDelegate unit, bool noMovement)
        {
            return new Decorator(
                ret => !DunatanksSettings.Instance.DisableMovement && unit(ret) != null,
                new PrioritySelector(
                    new Decorator(
                        ret => (!unit(ret).InLineOfSightOCD || (!noMovement && unit(ret).DistanceSqr > maxRange * maxRange)),
                        new Action(ret => Navigator.MoveTo(unit(ret).Location))),
                //Returning failure for movestop for smoother movement
                //Rest should return success !
                    new Decorator(
                        ret => Me.IsMoving && unit(ret).DistanceSqr <= maxRange * maxRange,
                        new Action(delegate
                        {
                            Navigator.PlayerMover.MoveStop();
                            return RunStatus.Failure;
                        })),
                    new Decorator(
                        ret => Me.CurrentTarget != null && Me.CurrentTarget.IsAlive && !Me.IsSafelyFacing(Me.CurrentTarget, coneDegrees),
                        new Action(ret => Me.CurrentTarget.Face()))
                    ));
        }

        //edited singular code
        protected Composite MoveToTarget()
        {
            return new Decorator(
                ret => !DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null,
                new PrioritySelector(
                    new Decorator(
                        ret => (!DunatanksSettings.Instance.DisableMovement && (!Me.CurrentTarget.InLineOfSight || Me.CurrentTarget.DistanceSqr > 5 * 5 || !Me.CurrentTarget.IsWithinMeleeRange)),
                        new Action(ret => Navigator.MoveTo(Me.CurrentTarget.Location))),
                //Returning failure for movestop for smoother movement
                //Rest should return success !
                    new Decorator(
                        ret => Me.IsMoving && Me.CurrentTarget.IsWithinMeleeRange,
                        new Action(delegate
                        {
                            Navigator.PlayerMover.MoveStop();
                            return RunStatus.Failure;
                        })))
                    );
        }

        public Composite MoveToTargetProper()
        {
            if (!DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.InLineOfSight || !Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.MoveTo(Me.CurrentTarget.Location);
                        return RunStatus.Success;
                    });
                }
                else if (Me.IsMoving && Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.PlayerMover.MoveStop();
                        CreateAutoAttack();
                        return RunStatus.Success;
                    });
                }
            }
            return new Action(delegate
            {
                return RunStatus.Failure;
            });
        }

        public Composite MoveToTargetProper2()
        {
            if (!DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null && !Me.Mounted && Me.Combat)
            {
                if (Me.CurrentTarget.InLineOfSight || !Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.MoveTo(Me.CurrentTarget.Location);
                        return RunStatus.Success;
                    });
                }
                else if (Me.IsMoving && Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.PlayerMover.MoveStop();
                        CreateAutoAttack();
                        return RunStatus.Success;
                    });
                }
            }
            return new Action(delegate
            {
                return RunStatus.Failure;
            });
        }


        public Composite movetobeta()
        {
            return new Decorator(
                ret => !DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null && !Me.CurrentTarget.IsWithinMeleeRange,
                new Action(ret => Navigator.MoveTo(Me.CurrentTarget.Location
                    )));
        }

        public Composite stopmoving()
        {
            return new Decorator(
                ret => !DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.Distance < 8 && Me.IsMoving,
                new Action(ret => Navigator.PlayerMover.MoveStop()));
        }

        public Composite MTT()
        {
            if (!DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.InLineOfSight || !Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.MoveTo(Me.CurrentTarget.Location);
                        return RunStatus.Success;
                    });
                }
                else if (Me.IsMoving && Me.CurrentTarget.IsWithinMeleeRange)
                {
                    return new Action(delegate
                    {
                        Navigator.PlayerMover.MoveStop();
                        return RunStatus.Success;
                    });
                }
            }
            return new Action(delegate
            {
                return RunStatus.Failure;
            });
        }

        protected Composite CreateMoveToAndFace(float maxRange, float coneDegrees, UnitSelectDelegate unit)
        {
            return CreateMoveToAndFace(maxRange, coneDegrees, unit, false);
        }

        protected Composite CreateMoveToAndFace(float maxRange, UnitSelectDelegate distanceFrom)
        {
            return CreateMoveToAndFace(maxRange, 70, distanceFrom);
        }
        protected Composite CreateMoveToAndFace(UnitSelectDelegate unitToCheck)
        {
            return CreateMoveToAndFace(5f, unitToCheck);
        }

        protected Composite CreateMoveToAndFace()
        {
                return CreateMoveToAndFace(5f, ret => Me.CurrentTarget);
       }

        protected Composite CreateFaceUnit(UnitSelectDelegate unitToCheck)
        {
            return CreateMoveToAndFace(5f, 70, unitToCheck, true);
        }

        protected Composite CreateFaceUnit()
        {
            return CreateFaceUnit(ret => Me.CurrentTarget);
        }
        #endregion
    }

    internal static class Extensions
    {
        public static string SafeName(this WoWObject obj)
        {
            if (obj.IsMe)
            {
                return "Myself";
            }

            string name;
            if (obj is WoWPlayer)
            {
                if (RaFHelper.Leader == obj)
                    return "Tank";

                name = ((WoWPlayer)obj).Class.ToString();
            }
            else if (obj is WoWUnit && obj.ToUnit().IsPet)
            {
                name = "Pet";
            }
            else
            {
                name = obj.Name;
            }

            return name;
        }
    }
}