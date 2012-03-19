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
    public static class ShadowDance
    {
        public static bool CanRun
        {
            get
            {
                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if ((Spell.HasCanSpell("Shadow Dance")) 
                    && (Target.IsWithinMeleeRange) 
                    && (Target.MeIsBehind) 
                    && (StyxWoW.Me.HealthPercent > 70)
                    && (Target.IsMoving == false)
                    && (Spell.HasMyAuraTimeLeft("Recuperate") > 5000)
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            
            Combat._LastMove = "Shadow Dance";
            return Spell.Cast("Shadow Dance", StyxWoW.Me.CurrentTarget);
        }
    }
}
