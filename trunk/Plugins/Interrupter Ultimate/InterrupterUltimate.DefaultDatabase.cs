using Styx.Plugins.PluginClass;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {	
		private void CreateDefaultInterrupterDatabase()
        {
            blog("Loading default database...");
            blog("Adding tags groups...");
        //////////////////////////////////////////////////////////////////////////////////////////
        ////    Add/Edit Tags below this place      //////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //
        //  AddSpellTag(*shouldInterrupt, *removeFromInterrupts, tagGroupID, aliases)
        //  
        //  shouldInterrupt - should it interrupt spells with this tag?
        //  
        //  removeFromInterrupts - should completely remove group from interrupts? If set to true
        //                         will ignore shouldInterrupt and remove group from interrupts
        //                         entirely.
        //
        // * can be skipped both at once, or none at all. If skipped will work like you wrote "true, false"
        //  
        //  tagGroupID  - tag group identifier, must be unique to all groups identifiers,
        //                if added group's ID is contained in any of the previous group's aliases,
        //                all of the current group's aliases will be added to that previous group,
        //                eg.: default "druid" contains both of it's specializations - balance
        //                     and restoration
        //
        //  aliases - tag group aliases, automatically include tagGroupID,
        //
            AddSpellTagGroup(true, false, "offensive", "dmg", "damage", "off");
            AddSpellTagGroup(true, false, "defensive", "def", "defense");
            AddSpellTagGroup(true, false, "healing", "heal");
            AddSpellTagGroup(true, false, "mana restoration", "mana", "mana resto");
            AddSpellTagGroup(true, false, "cc", "control");
            AddSpellTagGroup(true, false, "summoning", "summon", "pet");

            AddSpellTagGroup(true, false, "druid", "druid balance", "druid restoration");
            AddSpellTagGroup(true, false, "druid balance", "moonkin", "boomkin", "owl");
            AddSpellTagGroup(true, false, "druid restoration", "resto druid", "tree");

            AddSpellTagGroup(true, false, "mage", "mage arcane", "mage fire", "mage frost");
            AddSpellTagGroup(true, false, "mage arcane", "arcane");
            AddSpellTagGroup(true, false, "mage fire", "firemage", "fire");
            AddSpellTagGroup(true, false, "mage frost", "frost", "ice mage", "ice");

            AddSpellTagGroup(true, false, "paladin", "loladin", "pala", "paladin holy");
            AddSpellTagGroup(true, false, "paladin holy", "holy pala", "holy pally");

            AddSpellTagGroup(true, false, "priest", "priest discipline", "priest holy", "priest shadow");
            AddSpellTagGroup(true, false, "priest discipline", "disc", "discipline");
            AddSpellTagGroup(true, false, "priest holy", "holy priest");
            AddSpellTagGroup(true, false, "priest shadow", "shadowpriest", "shadow");

            AddSpellTagGroup(true, false, "shaman", "shammie", "shammy", "shaman elemental combat", "shaman enhancement", "shaman restoration");
            AddSpellTagGroup(true, false, "shaman elemental combat", "ele", "elemental", "elemental shaman");
            AddSpellTagGroup(true, false, "shaman enhancement", "enh", "enhancement");
            AddSpellTagGroup(true, false, "shaman restoration", "resto shaman", "resto shammy");

            AddSpellTagGroup(true, false, "warlock", "lock", "warlock affliction", "warlock demonology", "warlock destruction");
            AddSpellTagGroup(true, false, "warlock affliction", "affli", "affliction");
            AddSpellTagGroup(true, false, "warlock demonology", "demonology", "demo");
            AddSpellTagGroup(true, false, "warlock destruction", "destro");

        //////////////////////////////////////////////////////////////////////////////////////////
            blog("Adding tag groups finished.");//////////////////////////////////////////////////
            blog("Adding spells to interruption list...");////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //// Add/Edit Spells List below this place  //////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //
        // How to read it:
        //
        // AddSpellToInterrupt(spellID, *shouldInterrupt, tags);
        //
        // spellID -> you can get it for example from wowhead link, for example:
        // http://www.wowhead.com/spell=12345 - 12345 is spellID
        //
        // shouldInterrupt -> true/false
        //
        // * can be skipped, default value is true
        //  
        // tags - list of sub-tags defined above
        //
        //
            // Druid - Balance
            AddSpellToInterrupt(33786, true, "moonkin", "cc");              // Cyclone
            AddSpellToInterrupt(339  , true, "moonkin", "cc");              // Entangling Roots
            AddSpellToInterrupt(16914, true, "moonkin", "dmg");             // Hurricane
            AddSpellToInterrupt(2912 , true, "moonkin", "dmg");             // Starfire
            AddSpellToInterrupt(78674, true, "moonkin", "dmg");             // Starsurge
            AddSpellToInterrupt(5176 , true, "moonkin", "dmg");             // Wrath

            // Druid - Restoration
            AddSpellToInterrupt(5185 , true, "tree", "heal");               // Healing Touch
            AddSpellToInterrupt(50464, true, "tree", "heal");               // Nourish
            AddSpellToInterrupt(20484, true, "tree", "heal");               // Rebirth
            AddSpellToInterrupt(8936 , true, "tree", "heal");               // Regrowth
            AddSpellToInterrupt(740  , true, "tree", "heal");               // Tranquility

            // Mage - Arcane
            AddSpellToInterrupt(30451, true, "arcane", "dmg");               // Arcane Blast
            AddSpellToInterrupt(5143 , true, "arcane", "dmg");               // Arcane Missiles
            AddSpellToInterrupt(759  , false,"arcane", "mana");              // Conjure Mana Gem
            AddSpellToInterrupt(12051, true, "arcane", "mana");              // Evocation

            // Mage - Fire
            AddSpellToInterrupt(133  , true, "fire", "dmg");                 // Fireball
            AddSpellToInterrupt(2120 , true, "fire", "dmg");                 // Flamestrike
            AddSpellToInterrupt(11366, true, "fire", "dmg");                 // Pyroblast
            AddSpellToInterrupt(2948 , true, "fire", "dmg");                 // Scorch

            AddSpellToInterrupt(44614, true, "fire", "frost", "dmg");        // Frostfire Bolt

            // Mage - Frost
            AddSpellToInterrupt(10   , true, "frost", "dmg");                // Blizzard
            AddSpellToInterrupt(116  , true, "frost", "dmg");                // Frostbolt
            AddSpellToInterrupt(82676, true, "frost", "dmg");                // Ring of Frost

            // Polymorph bool, to use with all 6 Polymorph spells at once
            bool interruptPolymorph = true;                                  // Polymorph

            // Paladin - Holy
            AddSpellToInterrupt(82326, true, "holy pala", "heal");           // Divine Light
            AddSpellToInterrupt(879  , true, "holy pala", "dmg");            // Exorcism
            AddSpellToInterrupt(19750, true, "holy pala", "heal");           // Flash of Light
            AddSpellToInterrupt(635  , true, "holy pala", "heal");           // Holy Light

            // Priest - Discipline
            AddSpellToInterrupt(8129 , true, "discipline", "dmg","cc");     // Mana Burn
            AddSpellToInterrupt(32375, true, "discipline", "def");          // Mass Dispel
            AddSpellToInterrupt(47540, true, "discipline", "dmg", "heal");  // Penance
            AddSpellToInterrupt(9484 , true, "discipline", "cc");           // Shackle Undead

            // Priest - Holy
            AddSpellToInterrupt(32546, true, "holy priest", "heal");        // Binding Heal
            AddSpellToInterrupt(64843, true, "holy priest", "heal");        // Divine Hymn
            AddSpellToInterrupt(2061 , true, "holy priest", "heal");        // Flash Heal
            AddSpellToInterrupt(2060 , true, "holy priest", "heal");        // Greater Heal
            AddSpellToInterrupt(2050 , true, "holy priest", "heal");        // Heal
            AddSpellToInterrupt(14914, true, "holy priest", "dmg");         // Holy Fire
            AddSpellToInterrupt(64901, true, "holy priest", "mana");        // Hymn of Hope
            AddSpellToInterrupt(596  , true, "holy priest", "heal");        // Prayer of Healing
            AddSpellToInterrupt(585  , true, "holy priest", "dmg");         // Smite

            // Priest - Shadow Magic
            AddSpellToInterrupt(8092 , true, "shadow", "dmg");              // Mind Blast
            AddSpellToInterrupt(605  , true, "shadow", "cc");               // Mind Control
            AddSpellToInterrupt(15407, true, "shadow", "dmg");              // Mind Flay
            AddSpellToInterrupt(73510, true, "shadow", "dmg");              // Mind Spike
            AddSpellToInterrupt(2096 , false,"shadow");                     // Mind Vision

            // Shaman - Elemental Combat
            AddSpellToInterrupt(76780, false,"elemental", "cc");            // Bind Elemental
            AddSpellToInterrupt(421  , true, "elemental", "dmg");           // Chain Lightning
            AddSpellToInterrupt(61882, true, "elemental", "dmg");           // Earthquake
            AddSpellToInterrupt(51514, true, "elemental", "cc");            // Hex
            AddSpellToInterrupt(51505, true, "elemental", "dmg");           // Lava Burst
            AddSpellToInterrupt(403  , true, "elemental", "dmg");           // Lightning Bolt

            // Shaman - Enhancement
            AddSpellToInterrupt(6196 , false,"enhancement");                // Far Sight
            AddSpellToInterrupt(2645 , false,"enhancement");                // Ghost Wolf

            // Shaman - Restoration
            AddSpellToInterrupt(1064 , true, "resto shaman", "heal");       // Chain Heal
            AddSpellToInterrupt(77472, true, "resto shaman", "heal");       // Greater Healing Wave
            AddSpellToInterrupt(73920, true, "resto shaman", "heal");       // Healing Rain
            AddSpellToInterrupt(8004 , true, "resto shaman", "heal");       // Healing Surge
            AddSpellToInterrupt(331  , true, "resto shaman", "heal");       // Healing Wave

            // Warlock - Affliction
            AddSpellToInterrupt(689  , true, "affli", "dmg","heal");        // Drain Life
            AddSpellToInterrupt(89420, true, "affli", "dmg", "heal");       // Drain Life (Soulburn)
            AddSpellToInterrupt(1120 , true, "affli", "dmg");               // Drain Soul
            AddSpellToInterrupt(5782 , true, "affli", "cc");                // Fear
            AddSpellToInterrupt(48181, true, "affli", "dmg", "heal");       // Haunt
            AddSpellToInterrupt(5484 , true, "affli", "cc");                // Howl of Terror
            AddSpellToInterrupt(27243, true, "affli", "dmg");               // Seed of Corruption
            AddSpellToInterrupt(30108, true, "affli", "dmg");               // Unstable Affliction

            // Warlock - Demonology
            AddSpellToInterrupt(710  , false,"demonology", "cc");           // Banish
            AddSpellToInterrupt(6201 , true, "demonology");                 // Create Healthstone
            AddSpellToInterrupt(1098 , false,"demonology", "cc");           // Enslave Demon
            AddSpellToInterrupt(126  , false,"demonology");                 // Eye of Kilrogg
            AddSpellToInterrupt(71521, true, "demonology", "dmg");          // Hand of Gul'dan
            AddSpellToInterrupt(755  , true, "demonology", "pet", "heal");  // Health Funnel
            AddSpellToInterrupt(30146, true, "demonology", "pet");          // Summon Felguard
            AddSpellToInterrupt(691  , true, "demonology", "pet");          // Summon Felhunter
            AddSpellToInterrupt(688  , true, "demonology", "pet");          // Summon Imp
            AddSpellToInterrupt(1122 , true, "demonology", "pet");          // Summon Infernal
            AddSpellToInterrupt(712  , true, "demonology", "pet");          // Summon Succubus
            AddSpellToInterrupt(697  , true, "demonology", "pet");          // Summon Voidwalker
            AddSpellToInterrupt(693  , true, "demonology", "def");          // Create Soulstone
            
            // Warlock - Destruction
            AddSpellToInterrupt(50796, true, "destruction", "dmg");         // Chaos Bolt
            AddSpellToInterrupt(348  , true, "destruction", "dmg");         // Immolate
            AddSpellToInterrupt(29722, true, "destruction", "dmg");         // Incinerate
            AddSpellToInterrupt(5740 , true, "destruction", "dmg");         // Rain of Fire
            AddSpellToInterrupt(5676 , true, "destruction", "dmg");         // Searing Pain
            AddSpellToInterrupt(686  , true, "destruction", "dmg");         // Shadow Bolt
            AddSpellToInterrupt(6353 , true, "destruction", "dmg");         // Soul Fire

            // Warlock - misc
            AddSpellToInterrupt(85403, true, "demonology", "dmg");          // Hellfire - Demonology    - no idea what is that
            AddSpellToInterrupt(1949 , true, "destruction", "dmg");         // Hellfire - Destruction   - theoretically right one
            AddSpellToInterrupt(50589, true, "demonology", "dmg");          // Immolation Aura - Metamorphosis

            // Polymorphs
            AddSpellToInterrupt(118  , interruptPolymorph, "mage", "cc");   // Sheep
            AddSpellToInterrupt(28272, interruptPolymorph, "mage", "cc");   // Pig
            AddSpellToInterrupt(61721, interruptPolymorph, "mage", "cc");   // Rabbit
            AddSpellToInterrupt(61305, interruptPolymorph, "mage", "cc");   // Black Cat
            AddSpellToInterrupt(61780, interruptPolymorph, "mage", "cc");   // Turkey
            AddSpellToInterrupt(28271, interruptPolymorph, "mage", "cc");   // Turtle

        //////////////////////////////////////////////////////////////////////////////////////////
        ////  Add/Edit Spell List above this place  //////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////

            InterrupterContainer.AddUnit("target", true);
            InterrupterContainer.AddUnit("focus", true);
            InterrupterContainer.AddUnit("arena1", true);
            InterrupterContainer.AddUnit("arena2", true);
            InterrupterContainer.AddUnit("arena3", true);
            InterrupterContainer.AddUnit("arena4", true);
            InterrupterContainer.AddUnit("arena5", true);

            blog("Finished adding spells to interruption list.");
            slog("Default database loaded.");
        }
	}
}