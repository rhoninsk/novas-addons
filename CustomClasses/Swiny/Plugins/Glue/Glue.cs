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
        public override Version Version { get { return new Version(2, 0, 0, 5); } }
        public int WoWVersion = 15354;  //  For our offsets.


        public override void Pulse()
        {
            // a couple quick checks to see if thread is already created, and our offsets are up-todate
            if (Movement.tMovement != null) return;                     // If we already have a thread.
            if (StyxWoW.GameVersion.Revision != WoWVersion) return;     // If wow version not same ( offsets ) 
            if (Settings._Instance.MovementEnabled == false) return;    // If we not doing anything
            

            // Just some info displayed
            Utils.Write("Movement Pulse: [{0} TPS]", Settings._Instance.MovementTPS);
            Utils.Write("Honorbuddy Pulse: [{0} TPS]", TreeRoot.TicksPerSecond);


            // Create our movement Thread.
            Utils.Write("Starting - Movement Thread");
            Movement.tMovement = new Thread(new ThreadStart(Movement.MovementThread));
            Movement.tMovement.IsBackground = true;
            Movement.tMovement.Priority = ThreadPriority.Highest;
            Movement.tMovement.Start();
        }


        public override void Initialize()
        {

            // Checking our WoWVersion == Our Offset Version of glue
            if (StyxWoW.GameVersion.Revision != WoWVersion)
            {
                Utils.Write("--------------------------------------------------------");
                Utils.Write("--------------------- Glue ERROR ---------------------");
                Utils.Write("--------------------------------------------------------");

                Utils.Write("Could Not Load");
                Utils.Write("Disabling Plugin - Versions do not match! [ Offsets ]");
                Utils.Write("WoW Version: {0}  -  Glue Version: {1}", StyxWoW.GameVersion.Revision, WoWVersion);

                Utils.Write("--------------------------------------------------------");
                Utils.Write("--------------------- Glue ERROR ---------------------");
                Utils.Write("--------------------------------------------------------");

                throw new Exception("Glue Terminating");
            }
            else
            {
                Utils.Write("Loaded Successfully!");
                Utils.Write("Version: {0}", Version);
            }

            // Create our forms and settings and init's
            fSettings._Instance = new fSettings();
            Settings._Instance = new Settings();
            TreeRoot.TicksPerSecond = (byte)Settings._Instance.HonorbuddyTPS;
        }

        public override void Dispose()
        {
            // once we no longer need the thread,
            // null the thread, and reset HB Ticks back to default
            Utils.WriteDebug("Destroying Thread");
            Movement.tMovement.Abort();
            Movement.tMovement = null;
            TreeRoot.TicksPerSecond = (byte) 15;
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
