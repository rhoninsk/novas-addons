using System;
using System.Collections.Generic;
using System.IO;
using Styx;
using Styx.Helpers;
using System.Collections;

namespace Altarboy
{
    public class AltarboySettings : Settings
    {
        public static readonly AltarboySettings Instance = new AltarboySettings();

        public AltarboySettings()
            : base(
                Path.Combine(Logging.ApplicationPath,
                             string.Format(@"CustomClasses/Config/Altarboy-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool ForceShadowFiendOnArchangel { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseMindSpikeRotationWithShadowFiend { get; set; }

        [Setting, DefaultValue(false)]
        public bool AutoShield { get; set; }

        [Setting, DefaultValue(0.4)]
        public double ClippingDuration { get; set; }

        [Setting, DefaultValue(0.3)]
        public double CoolDownDuration { get; set; }

        [Setting, DefaultValue(false)]
        public bool ClipMindFlay { get; set; }

        //Potions
        [Setting, DefaultValue(true)]
        public bool UsePotionsOnLust { get; set; }

        [Setting, DefaultValue(0)]
        public int FavouritePotionID { get; set; }

        //Rest Settings
        [Setting, DefaultValue(true)]
        public bool AllowFoodAndDrinkWhileResting { get; set; }

        [Setting, DefaultValue(80)]
        public Double MinRestHealthPercent { get; set; }

        [Setting, DefaultValue(60)]
        public Double MinRestManaPercent { get; set; }


        //Targeting
        [Setting, DefaultValue(true)]
        public bool EnableTargeting { get; set; }

        [Setting, DefaultValue(true)]
        public bool EnableMultiDotTargeting { get; set; }

        [Setting, DefaultValue(true)]
        public bool EnableMindSearTargeting { get; set; }

        [Setting, DefaultValue(false)]
        public bool PrioritizeRaidMarkerTargeting { get; set; }

        [Setting, DefaultValue(true)]
        public bool RaFLeaderTargeting { get; set; }

        //Fade
        [Setting, DefaultValue(true)]
        public bool EnableFadeOnAggro { get; set; }

        //IS PAUSED
        [Setting, DefaultValue(false)]
        public bool IsPaused { get; set; }

        // ShadowForm
        [Setting, DefaultValue(true)]
        public bool UseShadowForm { get; set; }

        [Setting, DefaultValue(true)]
        public bool ForceShadowFormInCombat { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseVampiricEmbrace { get; set; }

        //Levitate
        [Setting, DefaultValue(true)]
        public bool LevitateWhileFalling { get; set; }

        // Fire Mode
        [Setting, DefaultValue(true)]
        public bool UseInnerFire { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseInnerWill { get; set; }

        [Setting, DefaultValue(true)]
        public bool ChangeInnerSpellWhileResting { get; set; }


        //Foritude
        [Setting, DefaultValue(true)]
        public bool BuffFortitude { get; set; }

       

        //Shadow Protection
        [Setting, DefaultValue(true)]
        public bool BuffShadowProtection { get; set; }

    


        //DPS SETTINGS

        //Mind Blast Settings
        [Setting, DefaultValue(false)]
        public bool UseMindBlastOnCD { get; set; }


        //Shadowfiend Settings
        [Setting, DefaultValue(true)]
        public bool UseShadowFiendOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseShadowFiendForManaRegen { get; set; }

        [Setting, DefaultValue(80)]
        public int UseShadowFiendManaPercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool ForceFiendOnBloodlust { get; set; }

        //Mind Sear

        [Setting, DefaultValue(true)]
        public bool UseMindSear { get; set; }

        [Setting, DefaultValue(4)]
        public int MindSearHostileCount { get; set; }

        //Shadow Word:Death Settings
        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathNever { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathOnCD { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathForManaOnly { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathForExecute { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathCheckForDebuffs { get; set; }

        [Setting, DefaultValue(40)]
        public int ShadowWordDeathManaPercent { get; set; }


        [Setting, DefaultValue(true)]
        public bool ShadowWordDeathWhileMoving { get; set; }


        //Dispersion
        [Setting, DefaultValue(10)]
        public int DispersionManaPercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseDispersionForManaRegen { get; set; }


        //Trinket 1  
        [Setting, DefaultValue(true)]
        public bool Trinket1NeverUse { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket1UseOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket1UseWithArchangel { get; set; }

        //Trinket 2  
        [Setting, DefaultValue(true)]
        public bool Trinket2NeverUse { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket2UseOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket2UseWithArchangel { get; set; }

        //Enginnering
        [Setting, DefaultValue(false)]
        public bool EngUseBelt { get; set; }

        [Setting, DefaultValue(false)]
        public bool EngUseGloves { get; set; }

        [Setting, DefaultValue(false)]
        public bool EngUseGlovesWithArchangel { get; set; }


        // DISC HEALING - Experimental
        [Setting, DefaultValue(90)]
        public int DiscRenewPercent { get; set; }

     

    }
}