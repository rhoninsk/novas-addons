using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins.PluginClass;

using ObjectManager = Styx.WoWInternals.ObjectManager;
using System.IO;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {
        private Random rGen = new Random();
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public override Version Version { get { return new Version(3, 3); } }
		public override string Name { get { return "Interrupter Ultimate"; } }
		public override string Author { get { return "strix"; } }
        private static string LogName = "Interrupter Ultimate";

        public override bool WantButton { get { return true; } }
        public override string ButtonText {  get { return "Menu"; } }

        public static InterrupterUltimate Instance { get; set; }
        public InterrupterUltimateSettings Settings;

        public InterrupterUltimate()
        {
            Instance = this;
            Settings = new InterrupterUltimateSettings();
        }

        private GUI UI;
        public override void OnButtonPress() { if (UI == null || UI.IsDisposed || UI.Disposing) UI = new GUI(); UI.ShowDialog(); }

        private static bool IsInitialized = false;

        public static InterrupterDatabase InterrupterContainer = new InterrupterDatabase();
        public SpellcastTracker SpellTracker = new SpellcastTracker();
        

        public override void Pulse()
		{
            SpellTracker.Update();
            ChooseSpell();
		}

        #region Initialize, Dispose

        public override void Initialize()
        {
            base.Initialize();
            if (!IsInitialized)
            {
                slog("Initializing...");
                if (File.Exists(DatabasePath))
                {
                    blog(DatabasePath + " found, extracting database...");
                    InterrupterContainer = ReadDatabaseFromFile(DatabasePath);
                    dlogTags(InterrupterContainer);
                    dlogSpells(InterrupterContainer);
                    dlog("Sorting database...");
                    var sortedspells = (from entry in InterrupterContainer.Spells orderby entry.Value.Name ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    var sortedtags = (from entry in InterrupterContainer.Tags orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    var sortedunits = (from entry in InterrupterContainer.Units orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    InterrupterContainer.Spells = sortedspells;
                    InterrupterContainer.Tags = sortedtags;
                    InterrupterContainer.Units = sortedunits;

                    foreach (KeyValuePair<string, bool> unit in InterrupterContainer.Units)
                    {
                        SpellTracker.VerifyTargetToTrack(unit.Key, unit.Value);
                    }
                    dlog("Database sorted.");
                    blog("Database is ready.");
                }
                else
                {
                    blog(DatabasePath + " not found, building default database...");
                    CreateDefaultInterrupterDatabase();
                    dlog("Sorting database...");
                    var sortedspells = (from entry in InterrupterContainer.Spells orderby entry.Value.Name ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    var sortedtags = (from entry in InterrupterContainer.Tags orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    var sortedunits = (from entry in InterrupterContainer.Units orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    InterrupterContainer.Spells = sortedspells;
                    InterrupterContainer.Tags = sortedtags;
                    InterrupterContainer.Units = sortedunits;
                    dlog("Database sorted.");
                    blog("Default database is ready.");
                }

                IsInitialized = true;
                slog("Initialization completed.");
            }
        }

        public override void Dispose()
        {
            if (IsInitialized)
            {
                blog("Closing...");
                dlog("Sorting database...");
                var sortedspells = (from entry in InterrupterContainer.Spells orderby entry.Value.Name ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                var sortedtags = (from entry in InterrupterContainer.Tags orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                var sortedunits = (from entry in InterrupterContainer.Units orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                InterrupterContainer.Spells = sortedspells;
                InterrupterContainer.Tags = sortedtags;
                InterrupterContainer.Units = sortedunits;
                dlog("Database sorted.");
                dlog("Saving to file...");
                SaveDatabaseToFile(InterrupterContainer, DatabasePath);
                blog("Database saved to " + DatabasePath);
                IsInitialized = false;
                blog("Closed.");
            }
            base.Dispose();
        }

        #endregion

    }
}