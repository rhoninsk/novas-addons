using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Styx.Helpers;
using Styx.Logic.Inventory;
using Styx.Plugins.PluginClass;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.POI;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using Sequence = TreeSharp.Sequence;
using Styx.WoWInternals;
namespace Glue
{
    class Movement
    {
        private static WoWPlayer Me = ObjectManager.Me;     // Just easyer to call
        private static WoWUnit Target;                      // My target


        internal static void PulseMovement()
        {
            try
            {
                // Experimenting with Facing
                if (ObjectManager.Me.CurrentTarget == null) WoWMovement.StopFace();

                // bunch of fuckoff checks to make sure we can even do the movements.
                if (StyxWoW.IsInGame == false) return;
                if (Me.IsValid == false) return;
                if (Me.CurrentTarget == null) return;
                if (Me.GotTarget == false) return;
                if (Me.Mounted) return;
                if (Me.Dead) return;
                if (Settings._Instance.PlayersOnly) if (Me.CurrentTarget.IsPlayer == false) return;

                // Target stuff
                Target = Me.CurrentTarget;
                if (Target.Distance > 10) return;
                if (Target.Dead) return;
                //if (Target.IsFriendly) return;


                // Do our movement stuff
                CheckFace();
                if (CheckMoving()) return;
                if (CheckStop()) return;
            }
            catch (System.Exception) { }
        }


        /// <summary>
        /// Check to see if we are constantly facing.
        /// </summary>
        private static void CheckFace()
        {
            if (!WoWMovement.IsFacing)
            {
                WoWMovement.Face(Target.Guid);
            }
        }

        private static bool CheckMoving()
        {

            if (Target.Distance >= 1.5 && Target.IsMoving && !StyxWoW.Me.MovementInfo.MovingForward)
            {
                WoWMovement.Move(WoWMovement.MovementDirection.Forward);
                return true;
            }


            if (Target.Distance < 1.5 && Target.IsMoving && StyxWoW.Me.MovementInfo.MovingForward)
            {
                WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
                return true;
            }



            //////// OLD
            //if (Target.MovementInfo.IsMoving && !StyxWoW.Me.MovementInfo.MovingForward)
            //{
            //    WoWMovement.Move(WoWMovement.MovementDirection.Forward);
            //    return true;
            //}

            //if (!Target.MovementInfo.IsMoving && StyxWoW.Me.MovementInfo.MovingForward)
            //{
            //    WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
            //    return true;
            //}

            return false;
        }

        private static bool CheckStop()
        {

                if (Target.IsMoving) return false;
                float Distance = 3.2f;

                if (Target.Distance >= Distance && StyxWoW.Me.IsMoving == false)
                {
                    WoWMovement.ClickToMove(Target.Location);
                    return true;
                }

                // To stop from spinning
                if (Target.Distance < 2 && StyxWoW.Me.IsMoving)
                {
                    WoWMovement.MoveStop();
                }

                return false;
        }

        private static void CheckStrafe()
        {
        }

        enum eStrafeDirection
        {
            Left,
            Right
        }
    }
}
