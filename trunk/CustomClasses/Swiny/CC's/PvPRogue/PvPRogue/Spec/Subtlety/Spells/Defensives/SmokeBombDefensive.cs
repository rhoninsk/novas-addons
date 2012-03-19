using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class SmokeBombDefensive
    {
        public static bool CanRun
        {
            get
            {
                if ((Spell.HasCanSpell("Smoke Bomb"))
                    && (StyxWoW.Me.HealthPercent < 75)
                    && (Managers.PlayerObjects.MeleeTargeting >= 2)
                ) return true;

                // If its 1 on 1 
                if ((Managers.PlayerObjects.EnemysFocusedOnMe == 1)
                    && (Managers.PlayerObjects.TeamAround(15) == 1)
                    && (StyxWoW.Me.HealthPercent < 45)
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Smoke Bomb";
            return Spell.Cast("Smoke Bomb");
        }
    }
}
