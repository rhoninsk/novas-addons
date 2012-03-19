using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;

namespace GuildAccept
{
    public class GuildAccept : HBPlugin
    {
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Guild level minimum to join
        /// </summary>
        public static int LevelToJoin = 25;

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        public override string Name { get { return "Guild Accept"; } }
        public override string Author { get { return "SwInY"; } }
        public override Version Version { get { return new Version(1, 0, 0, 0); } }
        public override void Pulse() { }

        public override void Initialize()
        {
            Lua.Events.AttachEvent("GUILD_INVITE_REQUEST", GuildInvite);
        }

        public override void Dispose()
        {
            Lua.Events.DetachEvent("GUILD_INVITE_REQUEST", GuildInvite);
        }

        private void GuildInvite(object sender, LuaEventArgs e)
        {
            // Our Var's

            string GuildName;
            int GuildLevel;

            GuildName = e.Args[1].ToString();
            GuildLevel =  Convert.ToInt32(e.Args[2]);


            if (GuildLevel >= LevelToJoin)
            {
                Write("Accepting - Guild Name: {0}  Guild Level: {1}", GuildName, GuildLevel);
                Lua.DoString("AcceptGuild()");
            }
            else
            {
                Write("Declining - Guild Name: {0}  Guild Level: {1}", GuildName, GuildLevel);
                Lua.DoString("DeclineGuild()");
            }
            Lua.DoString("StaticPopup_Hide(\"GUILD_INVITE_REQUEST\")");
        }


        /// <summary>
        /// Used for our logging
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="args"></param>
        public static void Write(string Value, params object[] args)
        {
            Value = string.Format(Value, args);

            Logging.WriteDebug(Color.Cyan, "[Guild Accept] " + Value);
            Logging.Write(Color.Cyan, "[Guild Accept] " + Value);
        }

    }
}
