using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.ComponentModel;
using Styx.Helpers;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace PvPRogue
{
    class ClassSettings : Styx.Helpers.Settings
    {

        public static ClassSettings _Instance;

        public ClassSettings()
            : base(Path.Combine(Path.Combine(Logging.ApplicationPath, "Settings"), string.Format("PvPRogue_{0}.xml", Styx.StyxWoW.Me.Name)))
        {
        }


        [Setting]
        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Always Stealthed")]
        [Description("true/false - Enabling this will make your rogue walk around the BG in stealth, [Disabling Mount's]")]
        public bool GeneralAlwaysStealthed { get; set; }

[Setting]
        [DefaultValue(false)]
        [Category("Movement")]
        [DisplayName("Disable Movement")]
        [Description("Disable all movement within the CC. This will NOT stop it from charging, blinking, etc. Only moving towards units, and facing will be disabled.")]
        public bool DisableAllMovement { get; set; }
    
[Setting]
        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Try to Sap")]
        [Description("true/false - To try initial Sap on pulling")]
        public bool GeneralToSap { get; set; }


        [Setting]
        [DefaultValue(eSubOpener.Ambush)]
        [Category("Subtlety")]
        [DisplayName("Sub Opener")]
        [Description("Move to open with as subtlety")]
        public eSubOpener SubtletyOpener { get; set; }


        [Setting]
        [DefaultValue(ePoison.Instant)]
        [Category("Subtlety")]
        [DisplayName("Main Hand Poison")]
        [Description("Poison to apply to main hand")]
        public ePoison SubtletyMainPoison { get; set; }


        [Setting]
        [DefaultValue(ePoison.Crippling)]
        [Category("Subtlety")]
        [DisplayName("Off Hand Poison")]
        [Description("Poison to apply to off hand")]
        public ePoison SubtletyOffHandPoison { get; set; }

    }


    /// <summary>
    /// Enum for Sub Opening Move
    /// </summary>
    public enum eSubOpener
    {
        Garrote,
        Ambush
    }

    /// <summary>
    /// List of poison
    /// </summary>
    public enum ePoison
    {
        Instant,
        Crippling,
        MindNumbing,
        Deadly,
        Wound
    }
}
