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
    public static class SliceandDice
    {
        public static bool CanRun
        {
            get
            {
                // If we are doing our INIT Pull Burst
                if ((ClassSettings._Instance.SubtletyInitBurst) && (PvPRogue.PullTimer.ElapsedMilliseconds < PvPRogue.PullLengthMS)) return false;


                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if ((Spell.HasCanSpell("Slice and Dice"))
                    && (Target.IsWithinMeleeRange)
                    && (StyxWoW.Me.RawComboPoints >= 1)
                    && (!Spell.HasMyAura("Slice and Dice"))
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Slice and Dice";
            return Spell.Cast("Slice and Dice");
        }
    }
}
