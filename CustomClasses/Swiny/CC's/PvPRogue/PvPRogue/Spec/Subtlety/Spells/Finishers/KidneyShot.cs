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
    public static class KidneyShot
    {
        public static bool CanRun
        {
            get
            {
                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if (ClassSettings._Instance.SubtletyFinisher != eSubFinisher.Kidney_Shot) return false;

                if (StyxWoW.Me.RawComboPoints >= 5) return true;

                if ((Spell.HasCanSpell("Kidney Shot"))
                    && (Target.IsWithinMeleeRange)
                    && (StyxWoW.Me.RawComboPoints > 1)
                    && (Spell.HasMyAuraTimeLeft("Recuperate") > 3000)
                    && (Spell.HasMyAuraTimeLeft("Recuperate") < 4000)
                    ) return true;


                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Kidney Shot";
            return Spell.Cast("Kidney Shot", StyxWoW.Me.CurrentTarget);
        }
    }
}
