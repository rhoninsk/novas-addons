using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using TreeSharp;
using ObjectManager = Styx.WoWInternals.ObjectManager;

namespace Glue
{
    public class Settings : Styx.Helpers.Settings
    {
        public static Settings _Instance;

        public Settings()
            : base(Path.Combine(Path.Combine(Logging.ApplicationPath, "Settings"), string.Format("Glue_{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting(Explanation = "Enable Movement - For use with people who want to use only Honorbuddy TSP"),
        DefaultValue(true)]
        public bool MovementEnabled { get; set; }

        [Setting(Explanation = "How often to pulse Honorbuddy [ Changes Honorbuddy - Ticks per second ]"),
            DefaultValue(25)]
        public int HonorbuddyTPS { get; set; }

        [Setting(Explanation = "Players Only = true; NPC + humans = false"),
            DefaultValue(true)]
        public bool PlayersOnly { get; set; }
    }
}
