using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Plugins.PluginClass;

namespace Glue
{
    public class Glue : HBPlugin
    {
        public override string Name { get { return "Glue"; } }
        public override string Author { get { return "SwInY"; } }
        public override Version Version { get { return new Version(3, 0, 0, 0); } }


        public override void Initialize()
        {
            // Create our forms and settings and init's
            fSettings._Instance = new fSettings();
            Settings._Instance = new Settings();
            fInfo._Instance = new fInfo();
            TreeRoot.TicksPerSecond = (byte)Settings._Instance.HonorbuddyTPS;

            Utils.Write("Loaded Successfully!");
            Utils.Write("Version: {0}", Version);
        }

        public override void Dispose()
        {
            TreeRoot.TicksPerSecond = (byte)15;
        }

        public override void Pulse()
        {
            // a couple quick checks to see if thread is already created, and our offsets are up-todate 
            if (Settings._Instance.MovementEnabled == false) return;    // If we not doing anything

            Movement.PulseMovement();
        }


        public override string ButtonText
        {
            get
            {
                return "SwInY - Like a baws";
            }
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            fSettings._Instance.Show();
        }

    }
}
