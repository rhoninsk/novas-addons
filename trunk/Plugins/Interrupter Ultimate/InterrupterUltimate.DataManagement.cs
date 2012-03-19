using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using ObjectManager = Styx.WoWInternals.ObjectManager;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {
        private static string DatabasePath =
            Logging.ApplicationPath + "\\Plugins\\Interrupter Ultimate\\InterrupterUltimate.Database.txt";

        #region Classes

        public class InterrupterSpell
        {
            public int ID;
            public string Name;
            public bool Include;
            public List<string> TagGroupIDs = new List<string>();

            public InterrupterSpell(int spellID, bool interrupt, Dictionary<string, SpellTagGroup> tagsAvailable, params string[] tags)
            {
                this.Name = WoWSpell.FromId(spellID) != null ? WoWSpell.FromId(spellID).Name : "not found";
                if (this.Name == "not found")
                {
                    dlog("Spell ID == " + spellID + ", spellName not found, skipping this spell.");
                    return;
                }
                else
                    dlog("Creating " + this.Name + " (" + spellID + ")");

                this.ID = spellID;
                this.Include = interrupt;
                AddTagIDs(tagsAvailable, tags);
            }

            public void SetInclude(bool value)
            {
                this.Include = value;
            }

            public void AddTagIDs(Dictionary<string, SpellTagGroup> tagsAvailable, params string[] tags)
            {
                foreach (string tag in tags)
                {
                    if (!tagsAvailable.Values.Any(val => val.Aliases.Contains(tag)))
                    {
                        dlog("Tag: " + tag + " does not match any of the available tag group aliases, skipping it.");
                        continue;
                    }
                    foreach (KeyValuePair<string, SpellTagGroup> match in tagsAvailable.Where(mtag => mtag.Value.Aliases.Contains(tag)))
                    {
                        if (this.TagGroupIDs.Any(stagid => stagid == match.Key))
                        {
                            dlog("Tag group: " + match.Key + " is already assigned to spell " + this.Name + " (" + ID + "), skipping it.");
                            continue;
                        }
                        this.TagGroupIDs.Add(match.Key);
                        dlog(this.Name + " (" + ID + ") tagged as " + this.TagGroupIDs.Last());
                    }
                }
            }

            public void RemoveTagIDs(params string[] tagIDs)
            {
                foreach (string id in tagIDs)
                {
                    if (!this.TagGroupIDs.Contains(id))
                    {
                        dlog("Invalid tag id: " + id);
                        continue;
                    }
                    this.TagGroupIDs.Remove(id);
                }
            }
        }

        public class SpellTagGroup
        {
            public string GroupID;
            public bool Include;
            public bool EntirelyExclude;
            public List<string> Aliases = new List<string>();

            public SpellTagGroup(bool interrupt, bool removeFromInterrupts, string tagGroupID, params string[] aliases)
            {
                this.Include = interrupt;
                this.EntirelyExclude = removeFromInterrupts;
                this.GroupID = tagGroupID;
                this.Aliases.Add(this.GroupID);
                AddAliases(aliases);
            }

            public SpellTagGroup(string tagGroupID, params string[] aliases)
            {
                this.Include = true;
                this.EntirelyExclude = false;
                this.GroupID = tagGroupID;
                this.Aliases.Add(this.GroupID);
                AddAliases(aliases);
            }

            public void AddAliases(params string[] aliases)
            {
                foreach (string alias in aliases)
                {
                    if (this.Aliases.Exists(a => a == alias))
                    {
                        dlog("Alias: " + alias + " already exists in group " + this.GroupID);
                        continue;
                    }
                    dlog("Adding new alias: " + alias + " to group " + this.GroupID);
                    this.Aliases.Add(alias);
                }
            }

            public void RemoveAliases(params string[] aliases)
            {
                foreach (string alias in aliases)
                {
                    if (!this.Aliases.Exists(a => a == alias))
                    {
                        dlog("Alias: " + alias + " does not exist in group " + this.GroupID);
                        continue;
                    }
                    dlog("Removing alias: " + alias + " from group " + this.GroupID);
                    this.Aliases.Remove(alias);
                }

            }

            public void SetInterrupt(bool interrupt)
            {
                this.Include = interrupt;
            }

            public void SetRemoveFromInterrupts(bool removeFromInterrupts)
            {
                this.EntirelyExclude = removeFromInterrupts;
            }
        }

        public class InterrupterDatabase
        {
            public Dictionary<int, InterrupterSpell> Spells = new Dictionary<int, InterrupterSpell>();
            public Dictionary<string, SpellTagGroup> Tags = new Dictionary<string, SpellTagGroup>();
            public Dictionary<string, bool> Units = new Dictionary<string, bool>();

            public void AddSpellTagGroup(bool interrupt, bool removeFromInterrupts, string tagGroupID, params string[] aliases)
            {
                if (this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Tag group " + tagGroupID + " already exists, skipping this.");
                    return;
                }
                SpellTagGroup spellTag = new SpellTagGroup(interrupt, removeFromInterrupts, tagGroupID, aliases);

                foreach (KeyValuePair<string, SpellTagGroup> match in this.Tags.Where(mtag => mtag.Value.Aliases.Contains(spellTag.GroupID)))
                {
                    AddTagAliases(match.Key, aliases);
                }

                dlog("Adding new tag group with: ID = " + spellTag.GroupID + ", should interrupt: " + spellTag.Include + ", aliases:" + aliases.ToRealString());
                this.Tags.Add(spellTag.GroupID, spellTag);
            }

            public void AddSpellTagGroup(string tagGroupID, params string[] aliases)
            {
                this.AddSpellTagGroup(true, false, tagGroupID, aliases);
            }

            public void AddTagAliases(string tagGroupID, params string[] aliases)
            {
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Invalid tag group ID.");
                    return;
                }
                this.Tags[tagGroupID].AddAliases(aliases);
            }

            public void RemoveTagAliases(string tagGroupID, params string[] aliases)
            {
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Invalid tag group ID.");
                    return;
                }
                this.Tags[tagGroupID].AddAliases(aliases);
            }

            public void SetSpellTagGroupInterrupt(string tagGroupID, bool interrupt)
            {
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Invalid tag group ID.");
                    return;
                }
                this.Tags[tagGroupID].SetInterrupt(interrupt);
            }

            public void SetSpellTagGroupRemoveFromInterrupts(string tagGroupID, bool removeFromInterrupts)
            {
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Invalid tag group ID.");
                    return;
                }
                this.Tags[tagGroupID].SetRemoveFromInterrupts(removeFromInterrupts);
            }

            public void RemoveSpellTagGroup(string tagGroupID)
            {
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Invalid tag group ID.");
                    return;
                }
                this.Tags.Remove(tagGroupID);
            }

            public void VerifyTagGroupToInterrupt(string tagGroupID, bool interrupt, bool removeFromInterrupts, params string[] aliases)
            {
                dlog("Verifying tag group ID: " + tagGroupID + "...");
                if (!this.Tags.ContainsKey(tagGroupID))
                {
                    dlog("Tag group ID: " + tagGroupID + " not found, adding...");
                    AddSpellTagGroup(interrupt, removeFromInterrupts, tagGroupID, aliases);
                }
                if (this.Tags[tagGroupID].GroupID != tagGroupID)
                {
                    dlog("Tag group ID does not match Key, fixing group ID.");
                    this.Tags[tagGroupID].GroupID = tagGroupID;
                }
                if (this.Tags[tagGroupID].Include != interrupt)
                {
                    dlog("Interrupt does not match, setting new value.");
                    this.Tags[tagGroupID].Include = interrupt;
                }
                if (this.Tags[tagGroupID].EntirelyExclude != removeFromInterrupts)
                {
                    dlog("Remove from interrupts does not match, setting new value.");
                    this.Tags[tagGroupID].EntirelyExclude = removeFromInterrupts;
                }
                dlog("Removing all existing aliases and setting new ones...");
                this.Tags[tagGroupID].Aliases.RemoveRange(0, this.Tags[tagGroupID].Aliases.Count);
                this.Tags[tagGroupID].AddAliases(aliases);
            }

            public void AddSpellToInterrupt(int spellID, bool interrupt, params string[] tags)
            {
                if (this.Spells.ContainsKey(spellID))
                {
                    dlog("Spell with ID = " + spellID + " already exists, skipping.");
                    return;
                }

                InterrupterSpell spell = new InterrupterSpell(spellID, interrupt, this.Tags, tags);
                
                this.Spells.Add(spell.ID, spell);
                dlog("Succesfully added: " + this.Spells[spell.ID].Name + " (" + this.Spells.Last().Key + ") ShouldInterrupt = " + this.Spells[spell.ID].Include + ", tag groups used: " + this.Spells[spell.ID].TagGroupIDs.ToRealString());
            }

            public void AddSpellToInterrupt(int spellID, params string[] tags)
            {
                if (this.Spells.ContainsKey(spellID))
                {
                    dlog("Spell with ID = " + spellID + " already exists, skipping.");
                    return;
                }

                InterrupterSpell spell = new InterrupterSpell(spellID, true, this.Tags, tags);

                this.Spells.Add(spell.ID, spell);
                dlog("Succesfully added: " + this.Spells[spell.ID].Name + " (" + this.Spells.Last().Key + ") ShouldInterrupt = " + this.Spells[spell.ID].Include + ", tag groups used: " + this.Spells[spell.ID].TagGroupIDs.ToRealString());
            }

            public void RemoveSpellToInterrupt(int spellID)
            {
                if (!this.Spells.ContainsKey(spellID))
                {
                    dlog("Spell ID not found.");
                    return;
                }
                this.Spells.Remove(spellID);
            }

            public void VerifySpellToInterrupt(int spellID, bool interrupt, params string[] tags)
            {
                dlog("Verifying spell ID : " + spellID + "...");
                if (!this.Spells.ContainsKey(spellID))
                {
                    dlog("Spell with ID : " + spellID + " not found, adding...");
                    this.AddSpellToInterrupt(spellID, interrupt, tags);
                    return;
                }
                if (this.Spells[spellID].ID != spellID)
                {
                    dlog("SpellID does not match Key, fixing spell's ID.");
                    this.Spells[spellID].ID = spellID;
                }
                if (this.Spells[spellID].Include != interrupt)
                {
                    dlog("Interrupt does not match, setting new value.");
                    this.Spells[spellID].Include = interrupt;
                }

                dlog("Removing all existing tags and setting new ones...");
                this.Spells[spellID].TagGroupIDs.RemoveRange(0, this.Spells[spellID].TagGroupIDs.Count);
                this.Spells[spellID].AddTagIDs(this.Tags, tags);

                dlog("Spell ID : " + spellID + " verification finished.");
            }

            public void AddUnit(string unitID, bool included)
            {
                if (!this.Units.ContainsKey(unitID))
                    this.Units.Add(unitID, included);
            }

            public void VerifyUnit(string unitID, bool included)
            {
                if (!this.Units.ContainsKey(unitID))
                {
                    this.Units.Add(unitID, included);
                    return;
                }
                this.Units[unitID] = included;
            }

            public void RemoveUnit(string unitID)
            {
                if (this.Units.ContainsKey(unitID))
                    this.Units.Remove(unitID);
            }

            public void dlogTags()
            {
                foreach (KeyValuePair<string, SpellTagGroup> g in this.Tags)
                {
                    dlog("Tag group: " + g.Key + ", should interrupt :" + g.Value.Include + ", aliases: " + g.Value.Aliases.ToRealString());
                }
            }

            public void dlogSpells()
            {
                foreach (KeyValuePair<int, InterrupterSpell> s in this.Spells)
                {
                    dlog(s.Value.Name + " (" + s.Key + "), should interrupt: " + s.Value.Include + ", tag groups assigned: " + s.Value.TagGroupIDs.ToRealString());
                }
            }
        }

        #endregion

        #region InterrupterContainer Management

        private void AddSpellTagGroup(bool interrupt, bool removeFromInterrupts, string tagGroupID, params string[] aliases)
        {
            InterrupterContainer.AddSpellTagGroup(interrupt, removeFromInterrupts, tagGroupID, aliases);
        }

        private void RemoveSpellTagGroup(string tagGroupID)
        {
            InterrupterContainer.RemoveSpellTagGroup(tagGroupID);
        }

        private void AddNewTagAliases(string tagGroupID, params string[] aliases)
        {
            InterrupterContainer.AddTagAliases(tagGroupID, aliases);
        }

        private void RemoveTagAliases(string tagGroupID, params string[] aliases)
        {
            InterrupterContainer.RemoveTagAliases(tagGroupID, aliases);
        }

        private void EditSpellTagGroupInterrupt(string tagGroupID, bool interrupt)
        {
            InterrupterContainer.SetSpellTagGroupInterrupt(tagGroupID, interrupt);
        }

        private void EditSpellTagGroupRemoveFromInterrupts(string tagGroupID, bool removeFromInterrupts)
        {
            InterrupterContainer.SetSpellTagGroupRemoveFromInterrupts(tagGroupID, removeFromInterrupts);
        }

        private void AddSpellToInterrupt(int spellID, bool shouldInterruptIt, params string[] tags)
        {
            InterrupterContainer.AddSpellToInterrupt(spellID, shouldInterruptIt, tags);
        }

        public void VerifySpellToInterrupt(int spellID, bool interrupt, params string[] tags)
        {
            InterrupterContainer.VerifySpellToInterrupt(spellID, interrupt, tags);
        }

        private void dlogTags(InterrupterDatabase database)
        {
            database.dlogTags();
        }

        private void dlogSpells(InterrupterDatabase database)
        {
            database.dlogSpells();
        }

        #endregion

        static public void SaveDatabaseToFile(InterrupterDatabase database, string path)
        {
            StreamWriter file = new StreamWriter(path);
            file.WriteLine("// ");
            file.WriteLine("// Interrupter Ultimate - Spells and Tag Groups - Database file");
            file.WriteLine("// How to read:");
            file.WriteLine("// \"fields\" are separated by Tab Stop character (the one appearing after pressing Tab key on left edge of the keyboard), ");
            file.WriteLine("// all other characters (except commas for TagIDs and Aliases) including whitespace are parts of those fields.");
            file.WriteLine("// ");
            file.WriteLine("// Tag group:<tab>tagGroupID<tab>Interrupt<tab>RemoveFromInterrupts<tab>Aliases separated by \",\" (comma)");
            file.WriteLine("// Spell:<tab>spellName<tab>tspellID<tab>Interrupt<tab>TagIDs separated by commas");
            file.WriteLine("// Unit:<tab>unitID<tab>included");
            file.WriteLine("// Lines starting with \"//\" are ignored.");
            file.WriteLine("// ");
            file.WriteLine("// Do not manually edit unless you are 100% sure of what you are doing, this file is NOT verified when loading");
            file.WriteLine("// ");
            file.WriteLine("// ");
            foreach (KeyValuePair<string, SpellTagGroup> entry in database.Tags)
            {
                file.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", "Tag group:", entry.Key.ToString().Normalize(), entry.Value.Include.ToString().Normalize(), entry.Value.EntirelyExclude.ToString().Normalize(), entry.Value.Aliases.ToRealString().Normalize());
            }
            foreach (KeyValuePair<int, InterrupterSpell> entry in database.Spells)
            {
                file.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", "Spell:", entry.Value.Name.Normalize(), entry.Key.ToString().Normalize(), entry.Value.Include.ToString().Normalize(), entry.Value.TagGroupIDs.ToRealString().Normalize());
            }
            foreach (KeyValuePair<string, bool> unit in database.Units)
            {
                file.WriteLine("{0}\t{1}\t{2}", "Unit:", unit.Key, unit.Value);
            }
            file.Close();

        }

        static public InterrupterDatabase ReadDatabaseFromFile(string path)
        {
            string line;
            InterrupterDatabase output = new InterrupterDatabase();
            using (StreamReader file = new StreamReader(path))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    if (parts[0] == "Tag group:")
                    {
                        SpellTagGroup tag = new SpellTagGroup(parts[2].ToBoolean(), parts[3].ToBoolean(), parts[1], parts[4].Split(','));
                        
                        output.Tags.Add(tag.GroupID, tag);
                    }
                    else if (parts[0] == "Spell:")
                    {
                        InterrupterSpell spell = new InterrupterSpell(parts[2].ToInt32(), parts[3].ToBoolean(), output.Tags, parts[4].Split(','));
                        output.Spells.Add(spell.ID, spell);
                    }
                    else if (parts[0] == "Unit:")
                    {
                        output.Units.Add(parts[1].ToString(), parts[2].ToBoolean());
                    }
                    else if (parts[0].StartsWith("//"))
                    {
                        continue;
                    }
                    else
                    {
                        Logging.WriteDebug("<Interrupter Ultimate> Invalid database line: " + line);
                    }
                }
            }
            return output;
        }

    }
}
