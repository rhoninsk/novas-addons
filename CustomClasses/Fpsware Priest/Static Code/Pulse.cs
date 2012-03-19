using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace Athena
{
    public partial class Fpsware
    {
        public override void Pulse()
        {
            base.Pulse();

            if (!_isCCLoaded) { _isCCLoaded = true; Settings.DirtyData = true; }
            if (Settings.DirtyData) LoadSettings(true);


            if (!Timers.Expired("Pulse", 500)) return;
            Timers.Reset("Pulse");

            // Environmental Settings
            /*
            if (Timers.Expired("EnvironmentSettings", 5000))
            {
                if (Settings.MultipleEnvironment.Contains("never"))
                {
                    ConfigSettings.CurrentEnvironment = "PVE";
                }
                else
                {
                    Timers.Reset("EnvironmentSettings");
                    string environment = Utils.IsBattleground ? "PVP" : "PVE";
                    environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
                    if (!ConfigSettings.UIActive && environment != ConfigSettings.CurrentEnvironment)
                    {
                        ConfigSettings.CurrentEnvironment = environment;
                        Utils.Log(string.Format("*** Environment has changed. Loading {0} settings.", environment),
                                  Utils.Colour("Red"));
                        LoadSettings(false);
                    }
                }
            }
             */



            // Make sure we have a target - Instance only
            // Sometimes IB was not selecting a target when we were in combat. This fucked up things immensely!
            if (Me.IsInInstance && !Me.GotTarget && RAF.PartyTankRole != null)
            {
                WoWUnit tank = RAF.PartyTankRole;
                if (tank.GotTarget && tank.Combat) RAF.PartyTankRole.CurrentTarget.Target();
            }


            // Try and grab a target all the time, if we're sitting around doing nothing check if anyone in our party has a target and take it. 
            if (!Me.Combat && (Me.IsInParty || Me.IsInRaid) && !Me.GotTarget)
            {
                foreach (WoWPlayer player in Me.PartyMembers)
                {
                    if (!player.Combat) continue;
                    if (player.Distance > 80) continue;
                    if (!player.GotTarget) continue;

                    if (Navigator.CanNavigateFully(Me.Location, player.CurrentTarget.Location, 5))
                        player.CurrentTarget.Target();
                }
            }


            // Buffs
            InCombatBuffs();
            OutOfCombatBuffs();
            MiscOtherPulse();

        }



    }
}
