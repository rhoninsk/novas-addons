using System;
using Styx.WoWInternals;

namespace Athena
{
    public partial class Fpsware
    {
        private void BotEvents_OnBotStarted(EventArgs args)
        {
            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", TalentPointEventHander);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", TalentPointEventHander);
            //Lua.Events.AttachEvent("PARTY_MEMBER_DISABLE", PlayerDeadEventHandler);

            Timers.Add("Pulse"); // Only do certain things in the Pulse check every 1 second


            // Everything inside here is class specific
            //------------------------------------------------------------------------------------------------------



            // Class specific information 
            ClassAndSpecInfo();

            // Class specific bot started 
            BotStarted();

            






            //------------------------------------------------------------------------------------------------------


            // Environmental Settings
            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;

            LoadSettings(false);
            Settings.PopulateRangedCapableMobs();
            Settings.PopulatePriorityMobs();
            Settings.PopulateHealingSpells();
            Settings.PopulateImportantInterruptSpells();

        }
    }
}