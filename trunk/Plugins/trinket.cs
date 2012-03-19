using System.Collections.Generic;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;

namespace TrinketRacial
{
    class TrinketRacial : HBPlugin
    {

        #region variables

        public List<WoWSpellMechanic> EscapeArtist = new List<WoWSpellMechanic>(){
            WoWSpellMechanic.Rooted,
            WoWSpellMechanic.Snared,
            WoWSpellMechanic.Slowed,
            WoWSpellMechanic.Frozen
        };

        public List<WoWSpellMechanic> Shapeshift = new List<WoWSpellMechanic>(){
            WoWSpellMechanic.Polymorphed
        };
        
        public List<WoWSpellMechanic> Trinket = new List<WoWSpellMechanic>(){
            WoWSpellMechanic.Asleep,
            WoWSpellMechanic.Charmed,
            WoWSpellMechanic.Disoriented,
            WoWSpellMechanic.Fleeing,
            WoWSpellMechanic.Frozen,
            WoWSpellMechanic.Horrified,
            WoWSpellMechanic.Incapacitated,
            WoWSpellMechanic.Polymorphed,
            WoWSpellMechanic.Rooted,
            WoWSpellMechanic.Sapped,
            WoWSpellMechanic.Slowed,
            WoWSpellMechanic.Snared,
            WoWSpellMechanic.Stunned
        };

        public List<WoWSpellMechanic> WillOfTheForsaken = new List<WoWSpellMechanic>(){
            WoWSpellMechanic.Asleep,
            WoWSpellMechanic.Charmed,
            WoWSpellMechanic.Horrified
        };

        public List<string> blacklist = new List<string>()
        {
            "Hamstring",
            "Crippling Poison",
            "Infected Wounds",
            "Piercing Howl",
            "Frostbolt"
        };

        public List<string> whitelist = new List<string>()
        {
            "Frost Nova"
        };

        #endregion

        #region overrides

        public override string Author
        {
            get { return "eXemplar"; }
        }

        public override string Name
        {
            get { return "TrinketRacial"; }
        }

        public override System.Version Version
        {
            get { return new System.Version(1, 0, 0, 6); }
        }

        #endregion

        #region pulse

        public override void Pulse()
        {
            if (!StyxWoW.Me.Combat)
            {
                return;   
            }
            if (StyxWoW.Me.HealthPercent < 50 && SpellManager.CanCast("Gift of the Naaru"))
            {
                Logging.Write("Health < 50% - Gift of the Naaru");
                SpellManager.Cast("Gift of the Naaru");
            }
            if (StyxWoW.Me.HealthPercent < 50 && SpellManager.CanCast("Lifeblood"))
            {
                Logging.Write("Health < 50% - Lifeblood");
                SpellManager.Cast("Lifeblood");
            }
            for(int i = 1; i <= 40; i++) {
                int s = Lua.GetReturnVal<int>("_, _, _, _, _, _, _, _, _, _, s = UnitDebuff(\"player\"," + i + "); return s", 0);
                if (s == null || s <= 0)
                {
                    return;
                }
                WoWSpell spell = WoWSpell.FromId(s);
                if (Shapeshift.Contains(spell.Mechanic) && SpellManager.CanCast("Cat Form"))
                {
                    Logging.Write("Crowd Control - " + spell.Mechanic + " - Cat Form");
                    SpellManager.Cast("Cat Form");
                    return;
                }
                if ((Trinket.Contains(spell.Mechanic) || whitelist.Contains(spell.Name)) && !blacklist.Contains(spell.Name))
                {
                    if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && (StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains(" of the ") || StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains(" Medallion of ") || StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains("Titan-Forged Rune of ")) && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                    {
                        Logging.Write("Crowd Control - " + spell.Mechanic + " - " + StyxWoW.Me.Inventory.Equipped.Trinket1.Name);
                        StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                        return;
                    }
                    if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && (StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains(" of the ") || StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains(" Medallion of ") || StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains("Titan-Forged Rune of ")) && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                    {
                        Logging.Write("Crowd Control - " + spell.Mechanic + " - " + StyxWoW.Me.Inventory.Equipped.Trinket2.Name);
                        StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                        return;
                    }

                    if (SpellManager.CanCast("Every Man for Himself"))
                    {
                        Logging.Write("Crowd Control - " + spell.Mechanic + " - Every Man for Himself");
                        SpellManager.Cast("Every Man for Himself");
                        return;
                    }
                }
                if (WillOfTheForsaken.Contains(spell.Mechanic) && SpellManager.CanCast("Will of the Forsaken"))
                {
                    Logging.Write("Crowd Control - " + spell.Mechanic + " - Will of the Forsaken");
                    SpellManager.Cast("Will of the Forsaken");
                    return;
                }
                if (EscapeArtist.Contains(spell.Mechanic) && SpellManager.CanCast("Escape Artist"))
                {
                    Logging.Write("Crowd Control - " + spell.Mechanic + " - Escape Artist");
                    SpellManager.Cast("Escape Artist");
                    return;
                }
                if ((spell.DispelType == WoWDispelType.Disease || spell.DispelType == WoWDispelType.Poison) && SpellManager.CanCast("Stoneform"))
                {
                    Logging.Write("Crowd Control - " + spell.DispelType + " - Stoneform");
                    SpellManager.Cast("Stoneform");
                    return;
                }
            }
        }

        #endregion

    }
}
