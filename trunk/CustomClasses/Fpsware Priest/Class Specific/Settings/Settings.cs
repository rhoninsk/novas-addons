namespace Athena
{
    public static partial class Settings
    {


        // Misc 
        public static string PullSpell { get; set; }


        // Hidden Settings, not visible in the UI
        public static string PriorityTargeting { get; set; }

        public static int RestHealPercent { get; set; }

        // Self Healing
        public static int FlashHealHealth { get; set; }
        public static int PowerWordShieldHealth { get; set; }
        public static int RenewHealth { get; set; }
        public static int PainSuppressionHealth { get; set; }

        // Party Healing
        public static int PartyHealerOOM { get; set; }
        public static string PartyCleanse { get; set; }
        public static string PartyHealWhen { get; set; }
        public static string PartyHealWhenSpec { get; set; }
        private static int _partyGuardianSpirit;
        public static int PartyGuardianSpirit { get { return _partyGuardianSpirit; } set { _partyGuardianSpirit = value; ClassHelpers.Priest.PartyGuardianSpirit = value; } }
        private static int _partyPrayerOfMending;
        public static int PartyPrayerOfMending { get { return _partyPrayerOfMending; } set { _partyPrayerOfMending = value; ClassHelpers.Priest.PartyPrayerOfMending = value; } }
        private static int _partyPenance;
        public static int PartyPenance { get { return _partyPenance; } set { _partyPenance = value; ClassHelpers.Priest.PartyPenance = value; } }
        private static int _partyPainSuppression;
        public static int PartyPainSuppression { get { return _partyPainSuppression; } set { _partyPainSuppression = value; ClassHelpers.Priest.PartyPainSuppression = value; } }
        private static int _partyPWS;
        public static int PartyPWS { get { return _partyPWS; } set { _partyPWS = value; ClassHelpers.Priest.PartyPWS = value; } }
        private static int _partyRenew;
        public static int PartyRenew { get { return _partyRenew; } set { _partyRenew = value; ClassHelpers.Priest.PartyRenew = value; } }
        private static int _partyFlashHeal;
        public static int PartyFlashHeal { get { return _partyFlashHeal; } set { _partyFlashHeal = value; ClassHelpers.Priest.PartyFlashHeal = value; } }
        private static int _partyGreaterHeal;
        public static int PartyGreaterHeal { get { return _partyGreaterHeal; } set { _partyGreaterHeal = value; ClassHelpers.Priest.PartyGreaterHeal = value; } }
        private static string _verboseHealing;
        public static string VerboseHealing { get { return _verboseHealing; } set { _verboseHealing = value; ClassHelpers.Priest.VerboseHealing = value; } }
        public static string PrayerOfHealingCount { get; set; }
        public static int PrayerOfHealingHealth { get; set; }
        public static string CircleOfHealingCount { get; set; }
        public static int CircleOfHealingHealth { get; set; }
        public static string HealPets { get; set; }

        public static string InnerFireWill { get; set; }
        public static string PowerWordFortitude { get; set; }
        public static string ShadowProtection { get; set; }

        public static string SmiteEvangelism { get; set; }
        public static int SmiteEvangelismHealth { get; set; }
        public static string Archangel { get; set; }
        public static string ArchangelParty { get; set; }
        public static string ResurrectPlayers { get; set; }
        public static string PowerWordBarrier { get; set; }
        public static string BouncePoM { get; set; }
        public static string WandParty { get; set; }
        public static int WandMana { get; set; }


        // Combat
        public static string Smite { get; set; }
        public static string ShadowWordPain { get; set; }
        public static string MindBlast { get; set; }
        public static string MindFlay { get; set; }
        public static string HolyFire { get; set; }
        public static string DevouringPlague { get; set; }
        public static string ShadowWordDeath { get; set; }
        public static string Silence { get; set; }
        public static string VampiricTouch { get; set; }
        public static string Shadowform { get; set; }
        public static int ShadowfiendMana { get; set; }
        public static int ReserveMana { get; set; }
        public static int DispersionMana { get; set; }
        public static int HymnOfHopeMana { get; set; }
        public static string FearWard { get; set; }
        public static string MindSpike { get; set; }
        public static string Chastise { get; set; }
        public static string Penance { get; set; }
        public static string PowerInfusion { get; set; }
        public static string ShackleUndead { get; set; }
        public static string PWSBeforePull { get; set; }
        public static string PsychicScream { get; set; }
        public static string MindSear { get; set; }
        public static string HolyNova { get; set; }


        // Hidden Settings, not visible in the UI
        public static int HealingSpellTimer { get; set; }
        public static double HealingModifierSolo { get; set; }
        public static int HealingAbsoluteMinimum { get; set; }
        public static int InnerFocusMana { get; set; }

        
    }
}