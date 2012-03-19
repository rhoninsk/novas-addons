using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using CommonBehaviors.Actions;
using TreeSharp;
using Action = TreeSharp.Action;

namespace InterrupterUltimate
{
    public class InterrupterUltimateSettings : Styx.Helpers.Settings
    {
        public InterrupterUltimateSettings()
            : base(Logging.ApplicationPath + "\\Settings\\InterrupterSettings\\IUSettings_" + StyxWoW.Me.Name + ".xml")
        {
            Load();
        }

        ~InterrupterUltimateSettings()
        {
            Save();
        }

        [Setting, DefaultValue(false)]
        public bool DebugMode { get; set; }

        [Setting, DefaultValue(false)]
        public bool InterruptAll { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShouldForceCast { get; set; }

        [Setting, DefaultValue(500)]
        public int CastMillisecondsLeft { get; set; }

        [Setting, DefaultValue(500)]
        public int ChannelMillisecondsElapsed { get; set; }
    }
}
