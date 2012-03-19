using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;

namespace Marksman
{
    public class MarksmanSettings : Settings
    {
        public static readonly MarksmanSettings Instance = new MarksmanSettings();

        public MarksmanSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/Bowman-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool AimedROT { get; set; }

        [Setting, DefaultValue(false)]
        public bool ArcaneROT { get; set; }

        [Setting, DefaultValue(false)]
        public bool MixedROT { get; set; }

        [Setting, DefaultValue(false)]
        public bool ExploROT { get; set; }

        [Setting, DefaultValue(true)]
        public bool MMSPEC { get; set; }

        [Setting, DefaultValue(false)]
        public bool SSPEC { get; set; }

        [Setting, DefaultValue(true)]
        public bool Party { get; set; }

        [Setting, DefaultValue(false)]
        public bool CP { get; set; }

        [Setting, DefaultValue(false)]
        public bool RP { get; set; }

        [Setting, DefaultValue(false)]
        public bool MP { get; set; }

        [Setting, DefaultValue(false)]
        public bool MS { get; set; }

        [Setting, DefaultValue(true)]
        public bool TL { get; set; }

        [Setting, DefaultValue(false)]
        public bool DSLFR { get; set; }

        [Setting, DefaultValue(false)]
        public bool DSNOR { get; set; }

        [Setting, DefaultValue(false)]
        public bool DSHC { get; set; }

        [Setting, DefaultValue(false)]
        public bool T1 { get; set; }

        [Setting, DefaultValue(false)]
        public bool T2 { get; set; }

        [Setting, DefaultValue(false)]
        public bool RF { get; set; }

        [Setting, DefaultValue(false)]
        public bool LB { get; set; }

        [Setting, DefaultValue(false)]
        public bool GE { get; set; }

        [Setting, DefaultValue(false)]
        public bool RS { get; set; }

        [Setting, DefaultValue(4)]
        public int Mobs { get; set; }

        [Setting, DefaultValue(1)]
        public int PET { get; set; }

    }
}
