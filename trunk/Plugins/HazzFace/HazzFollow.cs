using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HazzFace
{
    using Styx.Logic;
    using System;
    using Styx.Helpers;
    using Styx.Logic.Pathing;
    using System.Threading;
    using System.Diagnostics;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;
    using System.Net;
    using Styx.Plugins.PluginClass;
    using Styx;

    public class HazzFollow : HBPlugin
    {
        public override string Name { get { return "HazzFace"; } }
        public override string Author { get { return "Hazard"; } }
        public override Version Version { get { return new Version(1, 1); } }

        public override void Pulse()
        {

            if (Movement.tMovement != null) return;

            Movement.tMovement = new Thread(new ThreadStart(Movement.MovementThread));
            Movement.tMovement.IsBackground = true;
            Movement.tMovement.Priority = ThreadPriority.Highest;
            Movement.tMovement.Start();

        }
    }
}
