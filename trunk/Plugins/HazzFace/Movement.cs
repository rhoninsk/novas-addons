using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HazzFace
{
    class Movement
    {
        public static int FPSPulse = 30;
        public static int Distance = 35;
        public static Thread tMovement;
        private static WoWPlayer Me = ObjectManager.Me;
        private static WoWUnit Target;

        public static void MovementThread()
        {

            while (TreeRoot.IsRunning)
            {
                try
                {
                    Thread.Sleep(1000 / FPSPulse);

                    if (!StyxWoW.IsInGame) continue;
                    if (!ObjectManager.Me.IsValid) continue;
                    if (ObjectManager.Me.CurrentTarget == null) continue;
                    if (!StyxWoW.Me.GotTarget) continue;
                    if (Me.CurrentTarget.IsFriendly) continue;
                    if (ObjectManager.Me.Mounted) continue;

                    Target = Me.CurrentTarget;

                    if (Me.MovementInfo.CurrentSpeed == 0)
                    {
                        Me.CurrentTarget.Face();
                    }
                }
                catch (System.Exception) { }
            }

            tMovement = null;
        }
    }
}
