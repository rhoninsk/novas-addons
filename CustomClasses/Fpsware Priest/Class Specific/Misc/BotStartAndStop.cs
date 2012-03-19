namespace Athena
{
    public partial class Fpsware
    {
        public static void ClassAndSpecInfo()
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Priest.ClassSpec = (ClassHelpers.Priest.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Priest.ClassSpec, Me.Class));

        }

        private static void BotStarted()
        {
            
            // Timers, timers and more timers
            Utils.Log("Creating CC timers...");

            Timers.Add("ArcSmiteCombat");
            Timers.Add("DistanceCheck");
            Timers.Add("FaceTarget");
            Timers.Add("PullSpellCast");
            Timers.Add("EnvironmentSettings");
            Timers.Add("TimersTest");
            Timers.Add("Interact");
            Timers.Add("Spam");
            Timers.Add("Pull");
            Timers.Add("SpamPull");
            Timers.Add("Retarget");
            Timers.Add("Pulse");          // Only do certain things in the Pulse check every 1 second
            Timers.Add("DistanceCheck");
            Timers.Add("MassDispel");
            Timers.Add("HealingSpells"); // HB has an issue where it does not update health fast enough. This is a workaround to prevent heal spamming
            Timers.Add("SWPain");           // So we don't spam SWP on immune targets
            Timers.Add("VampiricTouch");    // VT is being cast more than it should. I don't think HB is seing this debuff properly
            Timers.Add("MindFlayChannel");
            //Timers.Add("MapSpam");

            Utils.Log("All CC timers created");
        }



        private static void BotStopped()
        {
            // Nothing to do here
        }
    }
}