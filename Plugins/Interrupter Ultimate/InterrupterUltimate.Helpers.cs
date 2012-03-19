using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using ObjectManager = Styx.WoWInternals.ObjectManager;
using Styx;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {
        private string GetTagID(string tag)
        {
            return InterrupterContainer.Tags.First(g => g.Value.Aliases.Contains(tag)).Key;
        }

        private WoWUnit GetUnitByWoWID(string unitID)
        {
            if (Lua.GetReturnValues("return UnitExists(\"{0}\")", unitID)[0].ToString() == "1")
            {
                string str = Lua.GetReturnVal<string>(
                    String.Format("return UnitGUID(\"{0}\")", unitID), 
                    0).Replace("0x", "");
                return ObjectManager.GetObjectByGuid<WoWUnit>(ulong.Parse(str, System.Globalization.NumberStyles.HexNumber));
            }
            return null;
        }


        #region Logging
        private static void slog(String sText)
        {
            Logging.Write("[" + LogName + "] " + sText);
        }

        private static void slog(String sText, bool shouldRun)
        {
            if (shouldRun) slog(sText);
        }

        private static void dlog(String sText)
        {
            Logging.WriteDebug("<" + LogName + "> " + sText);
        }

        private static void dlog(String sText, bool shouldRun)
        {
            if (shouldRun) dlog(sText);
        }

        private static void blog(String sText)
        {
            slog(sText);
            dlog(sText);
        }
        #endregion

        #region ForceCast

        private bool ForceCast(string spellName)
        {
            return ForceCast(spellName, true);
        }

        private bool ForceCast(string spellName, bool extraRun)
        {
            if (extraRun)
            {
                if (SpellManager.HasSpell(spellName) && SpellManager.CanCast(SpellManager.Spells[spellName]))
                {
                    if (Settings.ShouldForceCast)
                        SpellManager.StopCasting();
                    if (SpellManager.Cast(spellName))
                    {
                        slog("Force casted " + spellName);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ForceCast(string spellName, WoWUnit target)
        {
            return ForceCast(spellName, target, true);
        }

        private bool ForceCast(string spellName, string wowtarget)
        {
            WoWUnit target = GetUnitByWoWID(wowtarget);
            if (target != null)
                return ForceCast(spellName, target);
            return false;
        }

        private bool ForceCast(string spellName, WoWUnit target, bool extraRun)
        {
            if (extraRun)
            {
                if (SpellManager.HasSpell(spellName) && SpellManager.CanCast(SpellManager.Spells[spellName], target, true, false, false))
                {
                    if (Settings.ShouldForceCast)
                        SpellManager.StopCasting();
                    if (SpellManager.Cast(spellName, target))
                    {
                        slog("Force casted " + spellName + " on " + target.Name);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ForceCast(string spellName, string wowtarget, bool extraRun)
        {
            WoWUnit target = GetUnitByWoWID(wowtarget);
            if (target != null)
                return ForceCast(spellName, target, extraRun);
            return false;
        }

        #endregion

        private bool Immune(WoWUnit u)
        {
            return (u.ActiveAuras.ContainsKey("Deterrence") ||
                   u.ActiveAuras.ContainsKey("Divine Shield") ||
                   u.ActiveAuras.ContainsKey("Hand of Protection") ||
                   u.ActiveAuras.ContainsKey("Ice Block") ||
                   u.ActiveAuras.ContainsKey("Cyclone"));
        }

        private void RunMacroText(string macroText, bool extraRun)
        {
            if (extraRun)
                Lua.DoString("RunMacroText(\"{0}\")", macroText);
        }

        private bool IsInsideArena
        {
            get
            {
                return Me.RealZoneText == "Blade's Edge Arena" ||
                        Me.RealZoneText == "Dalaran Arena" ||
                        Me.RealZoneText == "Nagrand Arena" ||
                        Me.RealZoneText == "Ruins of Lordaeron" ||
                        Me.RealZoneText == "The Ring of Valor";
            }
        }

        private bool IsInsideBattleground
        {
            get
            {
                return Styx.Logic.Battlegrounds.IsInsideBattleground;
            }
        }
        
        private WoWUnit GetSpellcastingUnit
        {
            get
            {
                var result = (from UnitSpellcastingInfo sc in SpellTracker.Targets.Values.Where(v =>
                        v.UnitExists &&
                        v.Unit != null &&
                        v.Included && v.IsUnitEnemy &&
                        ((v.IsCasting && v.msCastTimeLeft < Settings.CastMillisecondsLeft) ||
                            (v.IsChannelling && v.msCastTimeElapsed > Settings.ChannelMillisecondsElapsed)) &&
                        v.Interruptible &&
                        (Settings.InterruptAll ||
                            (InterrupterContainer.Spells.ContainsKey(v.SpellID) &&
                            InterrupterContainer.Spells[v.SpellID].Include &&
                            InterrupterContainer.Spells[v.SpellID].TagGroupIDs.Any(tag =>
                                InterrupterContainer.Tags.ContainsKey(tag) && InterrupterContainer.Tags[tag].Include) &&
                            !InterrupterContainer.Spells[v.SpellID].TagGroupIDs.Any(tag =>
                            InterrupterContainer.Tags.ContainsKey(tag) && InterrupterContainer.Tags[tag].EntirelyExclude)))
                        )
                    where sc.Unit.IsValid
                    orderby sc.Unit.DistanceSqr ascending
                    select sc.Unit);

                return result.Count() > 0 ? result.FirstOrDefault() : null;
            }
        }

        private bool IsTargetAnyOfUnitTypes( params string[] types )
        {
            foreach (string type in types)
            {
                if(IsTargetOfUnitType(type))
                    return true;
            }
            return false;
        }

        private bool IsTargetOfUnitType( string type )
        {
            using(new FrameLock())
            {
                if
                (
                    Lua.GetReturnVal<Boolean>(
                        String.Format("return UnitExists(\"{0}\")", type), 0
                    ) &&
                    ulong.Parse(
                        Lua.GetReturnVal<string>(
                            String.Format("return UnitGUID(\"{0}\")", type),
                            0).Replace("0x", ""),
                        System.Globalization.NumberStyles.HexNumber
                    ) == CastingTarget.Guid
                )
                    return true;
                return false;
            }
        }
    }
}