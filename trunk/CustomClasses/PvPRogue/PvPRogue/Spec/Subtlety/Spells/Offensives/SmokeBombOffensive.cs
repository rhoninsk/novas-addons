﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class SmokeBombOffensive
    {
        public static bool CanRun
        {
            get
            {

                // If we are attacking a healer
                if ((Spell.HasCanSpell("Smoke Bomb"))
                    && (Managers.BGHealers._Instance.IsHealer(StyxWoW.Me.CurrentTarget))
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
