using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class Shiv
    {
        public static bool CanRun
        {
            get
            {
                // Make sure we have the spell
                if (!Spell.HasCanSpell("Shiv")) return false;

                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if (!Target.IsWithinMeleeRange) return false;

                // Check for target has spells.
                if (Target.HasAura("Enrage")) return true;
                if (Target.HasAura("Wrecking Crew")) return true;
                if (Target.HasAura("Savage Roar")) return true;
                if (Target.HasAura("Unholy Frenzy")) return true;
                if (Target.HasAura("Berserker Rage")) return true;
                if (Target.HasAura("Death Wish")) return true;
                if (Target.HasAura("Owlkin Frenzy")) return true;
                if (Target.HasAura("Bastion of Defense")) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Shiv";
            return Spell.Cast("Shiv", StyxWoW.Me.CurrentTarget);
        }

    }
}
