using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.Plugins.PluginClass;

namespace ChatSpam
{
    public class ChatSpam : HBPlugin
    {
        public override string Name { get { return "ChatSpam"; } }
        public override string Author { get { return "SwInY"; } }
        public override Version Version { get { return new Version(1, 0, 0, 0); } }


        int MinTime = 20;       // Min time [ Seconds ]
        int MaxTime = 45;      // Max time [ Seconds ]  - DO NOT GO HIGHER THAN 59 SECONDS OR IT WILL NOT WORK [ Will fix later ]
        string Message = "WTB elementium ore 70g Stack COD to 'CodToMe'";

        int RandomTimer;
        private static Stopwatch ChatTimer = new Stopwatch();
        public override void Pulse()
        {
            // If we not running, Start it!
            if (ChatTimer.IsRunning == false)
            {
                // Get our random time
                Random random = new Random();
                RandomTimer = random.Next(MinTime, MaxTime);

                Write("Chat Timer: {0}", RandomTimer);
                // Restart and start
                ChatTimer.Reset();
                ChatTimer.Start();

                // We shouldnt need to do anything else so return
                return;
            }

            if ((int)ChatTimer.Elapsed.Seconds >= (int)RandomTimer)
            {
                ChatTimer.Stop();

                // Do our string
                Write("Message: {0}", Message);
                Lua.DoString("SendChatMessage(\"" + Message + "\", \"CHANNEL\", nil, 2)");


            }
        }


        public static Color LogColour = Color.Red;
        public static string PreText = "[ChatSpam] ";

        public static void Write(string Value, params object[] args)
        {
            Value = string.Format(Value, args);

            Logging.Write(LogColour, PreText + Value);
        }
    }
}
