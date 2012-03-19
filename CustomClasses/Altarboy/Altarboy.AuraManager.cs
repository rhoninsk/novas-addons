
using System;
using System.Linq;
using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;

namespace Altarboy
{
    class AuraManager
    {

        // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; } }


        /// <summary>
        ///   Checks if there is an aura created by you on the target. Useful for DoTs.
        /// 	
        ///   Warning: This only checks your own auras on the unit !
        /// </summary>
        /// <param name = "aura">Name of the spell</param>
        /// <param name = "unit">Unit to check</param>
        /// <returns></returns>
        public static bool HasMyAura(string aura, WoWUnit unit)
        {
            return HasMyAura(aura, unit, TimeSpan.Zero, 0);
        }

        /// <summary>
        ///   Checks if there is an aura created by you on the target. Useful for DoTs.
        ///   This will return false even while you have the aura on the unit but the timeleft is lower then the expire time.
        ///   Useful to cast DoTs before expiring
        /// 
        ///   Warning: This only checks your own auras on the unit !
        /// </summary>
        /// <param name = "aura">Name of the spell</param>
        /// <param name = "unit">Unit to check</param>
        /// <param name = "timeLeft">Time left for the aura.</param>
        /// <returns></returns>
        public static bool HasMyAura(string aura, WoWUnit unit, TimeSpan timeLeft)
        {
            return HasMyAura(aura, unit, timeLeft, 0);
        }

        /// <summary>
        ///   Checks if there is an aura created by you on the target. Useful for DoTs.
        ///   This will return false even while you have the aura on the unit but the stackcount is lower then provided value.
        ///   Useful to stack more aura on the unit
        /// </summary>
        /// <param name = "aura">Name of the spell</param>
        /// <param name = "unit">Unit to check</param>
        /// <param name = "stackCount">Stack count</param>
        /// <returns></returns>
        public static bool HasMyAura(string aura, WoWUnit unit, int stackCount)
        {
            return HasMyAura(aura, unit, TimeSpan.Zero, stackCount);
        }

        /// <summary>
        ///   Checks if there is an aura created by you on the target. Useful for DoTs.
        ///   This will return false even while you have the aura on the unit but the stackcount is lower then provided value and
        ///   timeleft is lower then the expire time.
        ///   Useful to stack more dots or redot before the aura expires.
        /// </summary>
        /// <param name = "aura">Name of the spell</param>
        /// <param name = "unit">Unit to check</param>
        /// <param name = "timeLeft">Time left for the aura.</param>
        /// <param name = "stackCount">Stack count</param>
        /// <returns></returns>
        public static bool HasMyAura(string aura, WoWUnit unit, TimeSpan timeLeft, int stackCount)
        {
            // Check for unit being null first, so we don't end up with an exception
            if (unit == null)
            {
                return false;
            }

            // If the unit has that aura and it has been created by us return true
            if (unit.ActiveAuras.ContainsKey(aura))
            {
                var _aura = unit.ActiveAuras[aura];

                if (_aura.CreatorGuid == Me.Guid && _aura.TimeLeft > timeLeft && _aura.StackCount >= stackCount)
                {
                    return true;
                }
            }

            return false;
        }

        public static TimeSpan GetAuraTimeLeft(string auraName, WoWUnit onUnit, bool fromMyAura)
        {
            if (onUnit == null)
            {
                return TimeSpan.Zero;
            }
            
            WoWAura wantedAura =
                onUnit.GetAllAuras().Where(a => a.Name == auraName && (!fromMyAura || a.CreatorGuid == Me.Guid)).FirstOrDefault();

            if (wantedAura != null)
            {
                return wantedAura.TimeLeft;
            }
            return TimeSpan.Zero;
        }

        public static bool UnitHasAura(WoWUnit unit, string auraName)
        {
            if (unit == null)
            {
                return false;
            }
            return unit.Auras.Values.Any(a => a.Spell.Name == auraName);
        }


        public static bool HasAuraStacks(string aura, int stacks, WoWUnit unit)
        {
            // Active auras first.
            if (unit.ActiveAuras.ContainsKey(aura))
            {
                return unit.ActiveAuras[aura].StackCount >= stacks;
            }

            // Check passive shit. (Yep)
            if (unit.Auras.ContainsKey(aura))
            {
                return unit.Auras[aura].StackCount >= stacks;
            }

            // Try just plain old auras...
            if (stacks == 0)
            {
                return unit.HasAura(aura);
            }

            return false;
        }

        public static bool IsLusting(WoWUnit unit)
        {
            if (HasAuraStacks("Bloodlust", 0, unit) || HasAuraStacks("Time Warp", 0, unit) || HasAuraStacks("Ancient Hysteria", 0, unit) || HasAuraStacks("Heroism", 0, unit)) { return true; }
                return false;
            }


    }
}
