
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins.PluginClass;
using System.Collections.Generic;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {
        WoWUnit CastingTarget = null;
        private void ChooseSpell()
        {
            CastingTarget = GetSpellcastingUnit;
            if (CastingTarget != null)
            {
                //
                // Interrupting any valid spell starts here:
                //

                // Blood Elf
                if (!ForceCast("Arcane Torrent", CastingTarget.Distance < 8))
                {
                    // Death Knight
                    if (!ForceCast("Mind Freeze", CastingTarget))
                        ForceCast("Strangulate", CastingTarget);
                    // Druid
                    if (!ForceCast("Skull Bash", CastingTarget))
                        ForceCast("Solar Beam", CastingTarget);
                    // Hunter
                    ForceCast("Silencing Shot", CastingTarget);
                    // Mage
                    ForceCast("Counterspell", CastingTarget);
                    // Paladin
                    if (!ForceCast("Avenger's Shield", CastingTarget))
                        ForceCast("Rebuke", CastingTarget);
                    // Priest
                    ForceCast("Silence", CastingTarget);
                    // Rogue
                    ForceCast("Kick", CastingTarget);
                    // Shaman
                    ForceCast("Wind Shear", CastingTarget);
                    // Warrior
                    ForceCast("Pummel", CastingTarget);
                }

                //
                // Interrupting any valid spell ends here.
                //
            }

            //
            // Custom actions start here,
            //

        }
	}
}