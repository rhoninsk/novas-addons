using System.IO;
using Styx;
using Styx.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bobby53
{
    public sealed class SquireSettings : Settings
    {
        private static SquireSettings _singleton;
        public static SquireSettings Instance
        {
            get
            {
                return _singleton ?? (_singleton = new SquireSettings());
            }
        }

        private SquireSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"Settings/Squire_{0}.xml", StyxWoW.Me.Name)))
        {
        }


        public enum CombatStrategy
        {
            Auto,
            DPS,
            Healer
        }

        [Setting, DefaultValue(CombatStrategy.Auto)]
        public CombatStrategy Strategy { get; set; }


        public enum FollowMethod
        {
            Health,
            Density,
            Focus
        }

        [Setting, DefaultValue(FollowMethod.Density )]
        public FollowMethod Method { get; set; }

        [Setting, DefaultValue(1000)]
        public int LeaderScanRange { get; set; }

        [Setting, DefaultValue(200)]
        public int LeaderOutOfRange { get; set; }

        [Setting, DefaultValue(10)]
        public int FollowDistanceMelee { get; set; }

        [Setting, DefaultValue(30)]
        public int FollowDistanceRanged { get; set; }

        [Setting, DefaultValue(25)]
        public int StillTimeout { get; set; }

        [Setting, DefaultValue(120)]
        public int BlacklistTime { get; set; }

        [Setting, DefaultValue(1000)]
        public int MoveDelayMS { get; set; }

        [Setting, DefaultValue(3)]
        public int FollowMinDensity { get; set; }



    }
}
