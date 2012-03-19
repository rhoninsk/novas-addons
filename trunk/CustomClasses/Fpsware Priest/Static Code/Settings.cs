using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Athena
{
    public static partial class Settings
    {
        internal static List<string> IgnoreSettings = new List<string>(new[] { "ConfigFile", "ConfigFolder", "DirtyData", "Environment", "EnvironmentLoading", "_debugKey" });
        public static string ConfigFolder
        {
            get
            {
                string basePath = string.Format(@"CustomClasses\{0}\Class Specific\Settings\", Fpsware.CCName);
                string hbPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string workingPath = Path.Combine(hbPath, basePath);

                return workingPath;
            }
        }


        public static string ConfigFile
        {
            get
            {
                string basePath = string.Format(@"CustomClasses\{0}\Class Specific\Settings\Settings.xml", Fpsware.CCName);

                string hbPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string workingPath = Path.Combine(hbPath, basePath);

                return workingPath;
            }
        }


        #region Save and load settings
        public static void Save()
        {
            ConfigSettings.FileName = Settings.ConfigFile;

            if (ConfigSettings.Open())
            {
                foreach (PropertyInfo p in typeof(Settings).GetProperties())
                {
                    if (p.Name.StartsWith("_") || IgnoreSettings.Contains(p.Name)) continue;

                    object propValue = typeof(Settings).GetProperty(p.Name).GetValue(p.Name, null);
                    ConfigSettings.SetProperty(String.Format("//{0}/{1}", Fpsware.CCClass, p.Name), propValue.ToString());
                }

                ConfigSettings.Save();
            }
        }

        public static void Load()
        {
            //string environment = Utils.IsBattleground ? "PVP" : "PVE";
            //environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            //ConfigSettings.CurrentEnvironment = environment;

            ConfigSettings.FileName = Settings.ConfigFile;


            try
            {
                if (ConfigSettings.Open())
                {
                    foreach (PropertyInfo p in typeof(Settings).GetProperties())
                    {
                        if (p.Name.StartsWith("_") || IgnoreSettings.Contains(p.Name)) continue;
                        _debugKey = p.Name;

                        switch (typeof(Settings).GetProperty(p.Name).PropertyType.Name)
                        {
                            case "Boolean": { p.SetValue(typeof(Settings), Convert.ToBoolean(ConfigSettings.GetBoolProperty(String.Format("//{0}/{1}", Fpsware.CCClass, p.Name))), null); } break;
                            case "String": { p.SetValue(typeof(Settings), Convert.ToString(ConfigSettings.GetStringProperty(String.Format("//{0}/{1}", Fpsware.CCClass, p.Name))), null); } break;
                            case "Int32": { p.SetValue(typeof(Settings), Convert.ToInt16(ConfigSettings.GetIntProperty(String.Format("//{0}/{1}", Fpsware.CCClass, p.Name))), null); } break;
                            case "Double": { p.SetValue(typeof(Settings), Convert.ToDouble(ConfigSettings.GetIntProperty(String.Format("//{0}/{1}", Fpsware.CCClass, p.Name))), null); } break;
                        }
                    }
                }

                if (_loadErrorCount > 0)
                {
                    _loadErrorCount += 1;
                    Utils.Log("It looks like the error has been fixed. Save the cheerleader, save the ... oh wait.", Utils.Colour("Green"));
                    System.IO.File.Delete(ConfigSettings.UserFileName);
                    _loadErrorCount = 0;
                    Load();

                }
            }
            catch (Exception e)
            {
                if (_loadErrorCount < 1)
                {
                    _loadErrorCount += 1;
                    Utils.Log("There appears to be an issue with the character specific settings file. Trying to fix it...", Utils.Colour("Red"));
                    System.IO.File.Delete(ConfigSettings.UserFileName);
                    Utils.Log("It needs more cowbell...", Utils.Colour("Red"));
                    Load();
                }
                else
                {
                    Utils.Log("**********************************************************************");
                    Utils.Log("**********************************************************************");
                    Utils.Log(" ");
                    Utils.Log(" ");
                    Utils.Log(String.Format("Exception in settings load \"{0}\"", e.Message));
                    Utils.Log(string.Format("Last key attempted to be read was \"{0}\"", _debugKey));
                    Utils.Log(" ");
                    Utils.Log(" ");
                    Utils.Log(" THE CC WAS NOT LOADED. THE END IS NEIGH! REPENT YOUR SINS NOW!");
                    Utils.Log(" ");
                    Utils.Log("**********************************************************************");
                    Utils.Log("**********************************************************************");
                }
            }

        }


        public static void PopulateRangedCapableMobs()
        {
            Utils.Log("-Populating ranged capable database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("RangedCapableMobs").Elements("Mob"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("EntryID").Value);
                    Utils.RangedCapableMobs.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateRangedCapableMobs' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulatePriorityMobs()
        {
            Utils.Log("-Populating priority mobs database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("PriorityMobs").Elements("Mob"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("EntryID").Value);
                    Utils.PriorityMobs.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulatePriorityMobs' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulateHealingSpells()
        {
            Utils.Log("-Populating healing spells database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("HealingSpells").Elements("Spell"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("SpellID").Value);
                    Utils.HealingSpells.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateHealingSpells' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulateImportantInterruptSpells()
        {
            Utils.Log("-Populating important interrupt spells database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("ImportantInterruptSpells").Elements("Spell"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("SpellID").Value);
                    Utils.ImportantInterruptSpells.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateImportantInterruptSpells' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        #endregion


        private static string _debugKey = "nothing has been read yet";

        // Backing fields
        private static int _loadErrorCount;
        private static string _lazyRaider;
        private static int _maximumPullDistance;
        private static int _minimumPullDistance;
        private static int _combatTimeout;
        
        public static int Timermarker;

        public static string LazyRaider { get { return _lazyRaider; } set { _lazyRaider = value; Target.LazyRaider = value; Movement.LazyRaider = value; } }
        public static int MaximumPullDistance { get { return _maximumPullDistance; } set { _maximumPullDistance = value; Movement.MaximumDistance = _maximumPullDistance; } }
        public static int MinimumPullDistance { get { return _minimumPullDistance; } set { _minimumPullDistance = value; Movement.MinimumDistance = _minimumPullDistance; } }
        public static int CombatTimeout { get { return _combatTimeout; } set { _combatTimeout = value; Target.CombatTimeout = _combatTimeout; } }

        public static bool DirtyData { get; set; }
        public static int RestHealth { get; set; }
        public static int RestMana { get; set; }
        public static string Debug { get; set; }
        public static string RAFTarget { get; set; }
        public static string ShowUI { get; set; }
        public static string SmartEatDrink { get; set; }
        public static int ManaPotion { get; set; }
        public static int HealthPotion { get; set; }
        public static int LifebloodHealth { get; set; }
        public static string Cleanse { get; set; }
        //public static int RestHealPercent { get; set; }
        //public static string Version { get; set; }
        public static string MultipleEnvironment { get; set; }


    }
}
