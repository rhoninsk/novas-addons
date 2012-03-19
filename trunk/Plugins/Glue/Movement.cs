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
        internal static Thread tMovement;                   // Thread for our movement
        private static WoWPlayer Me = ObjectManager.Me;     // Just easyer to call
        private static WoWUnit Target;                      // My target

        /// <summary>
        /// We will want to put this on a movement thread
        /// So we can call this every Frame needed by [FPSPulse]
        /// </summary>
        internal static void MovementThread()
        {
            // While bot is running.
            while ((TreeRoot.IsRunning) && (Settings._Instance.MovementEnabled == true))
            {
                try
                {
                    // Cause were in a thread, Guess we just sleep
                    // This regulates the FPS to pulse
                    Thread.Sleep(1000 / Settings._Instance.MovementTPS);

                    // bunch of fuckoff checks to make sure we can even do the movements.
                    if (StyxWoW.IsInGame == false) continue;
                    if (ObjectManager.Me.IsValid == false) continue;
                    if (ObjectManager.Me.CurrentTarget == null) continue;
                    if (StyxWoW.Me.GotTarget == false) continue;
                    if (Me.CurrentTarget.IsFriendly) continue;
                    if (ObjectManager.Me.Mounted) continue;

                    // If we are only attacking Players.
                    if (Settings._Instance.PlayersOnly)
                        if (ObjectManager.Me.CurrentTarget.IsPlayer == false) continue;

                    // Just set our target to be easyer
                    Target = Me.CurrentTarget;

                    // Distance check - Dont want anything to do with navigation
                    if (Target.Distance > Settings._Instance.MovementDistance) continue;             

                    // Do our movement stuff
                    DoMovement(Target);

                } catch (System.Exception)  { }
            }

            // Destroy our thread
            Utils.WriteDebug("Destroying Thread");
            tMovement = null;
        }





        private static void DoMovement(WoWUnit Target)
        {
            // Thought i might try add this, just to see if this might catch wow's error
            // prob a longggggggggg shot, but worth the try.
            // -> my thoughts, if theres something to catch, maybe it wont crash wow it self?, but im guessing hb will still cause crash
            try
            {
                // If we are > 2, we need to get closer to them at all costs.
                if (Target.Distance > 3)
                {
                    ClickToMove.MoveTo(Target.Location);
                    return;
                }

                //// If player not moving and we not behind, lets get behind
                //if ((Target.MovementInfo.CurrentSpeed == 0) && (!Target.MeIsSafelyBehind))
                //{
                //    WoWPoint BehindLocation = WoWMathHelper.CalculatePointBehind(Target.Location, Target.Rotation, 1.5f);
                //    ClickToMove.MoveTo(BehindLocation);
                //}

                //// If where behind them and not facing them, Face them
                //if ((Target.MeIsSafelyBehind) && (!Me.IsSafelyFacing(Target)))
                //{
                //    Target.Face();
                //}

            } catch (Exception) { }

        }
    }
}
