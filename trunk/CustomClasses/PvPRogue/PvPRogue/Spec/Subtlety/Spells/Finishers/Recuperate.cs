﻿using System;
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
    public static class Recuperate
    {
        //Recuperate
        public static bool CanRun
        {
            get
            {
                // High level
                if ((Spell.HasCanSpell("Recuperate"))
                    && (StyxWoW.Me.RawComboPoints >= 2)
                    && (Spell.HasMyAuraTimeLeft("Recuperate") < 3000)
                    ) return true;
                    

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Recuperate";
            return Spell.Cast("Recuperate");
        }
    }
}
