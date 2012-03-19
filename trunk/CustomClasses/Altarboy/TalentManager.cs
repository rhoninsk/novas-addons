#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: apoc $
// $Date: 2011-03-18 17:36:36 +0100 (fr, 18 mar 2011) $
// $HeadURL: http://svn.apocdev.com/singular/tags/v1/Singular/TalentManager.cs $
// $LastChangedBy: apoc $
// $LastChangedDate: 2011-03-18 17:36:36 +0100 (fr, 18 mar 2011) $
// $LastChangedRevision: 190 $
// $Revision: 190 $

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Altarboy;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals;

//Credit Singular
namespace Altarboy
{
    public enum TalentSpec
    {
        Lowbie = 0,
        DisciplinePriest = ((int) WoWClass.Priest << 8) + 0,
        DisciplineHealingPriest = TalentManager.TalentFlagIsextraspec + ((int) WoWClass.Priest << 8) + 0,
        HolyPriest = ((int) WoWClass.Priest << 8) + 1,
        ShadowPriest = ((int) WoWClass.Priest << 8) + 2,
    }

    internal class TalentManager
    {
        public const int TalentFlagIsextraspec = 0x10000;

        static TalentManager()
        {
            Talents = new List<Talent>();
            Glyphs = new HashSet<string>();
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("GLYPH_UPDATED", UpdateTalentManager);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
        }

        public static TalentSpec CurrentSpec { get; private set; }

        public static List<Talent> Talents { get; private set; }

        public static HashSet<string> Glyphs { get; private set; }

        public static int GetCount(int tab, int index)
        {
            return Talents.FirstOrDefault(t => t.Tab == tab && t.Index == index).Count;
        }

        /// <summary>
        ///   Checks if we have a glyph or not
        /// </summary>
        /// <param name = "glyphName">Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form")</param>
        /// <returns></returns>
        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Count > 0 && Glyphs.Contains(glyphName);
        }

        private static void UpdateTalentManager(object sender, LuaEventArgs args)
        {
            TalentSpec oldSpec = CurrentSpec;

            Update();

            if (CurrentSpec != oldSpec)
            {
                Logger.Log("Your spec has been changed. Rebuilding behaviors");
            }
        }

        public static void Update()
        {
            // Don't bother if we're < 10
            if (StyxWoW.Me.Level < 10)
            {
                CurrentSpec = TalentSpec.Lowbie;
                return;
            }
            WoWClass myClass = StyxWoW.Me.Class;
            int treeOne = 0, treeTwo = 0, treeThree = 0;
            bool isExtraSpec = false;

            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (new FrameLock())
            {
                Talents.Clear();
                for (int tab = 1; tab <= 3; tab++)
                {
                    var numTalents = Lua.GetReturnVal<int>("return GetNumTalents(" + tab + ")", 0);
                    for (int index = 1; index <= numTalents; index++)
                    {
                        var rank = Lua.GetReturnVal<int>(string.Format("return GetTalentInfo({0}, {1})", tab, index), 4);
                        var t = new Talent {Tab = tab - 1, Index = index - 1, Count = rank};
                        Talents.Add(t);

                        // Thick Hide - Only used by tanking druids
                        if (myClass == WoWClass.Druid && tab == 2 && index == 11 && rank != 0)
                        {
                            isExtraSpec = true;
                        }

                        // Renewed Hope
                        if (myClass == WoWClass.Priest && tab == 1 && index == 8 && rank != 0)
                        {
                            isExtraSpec = true;
                        }

                        switch (tab)
                        {
                            case 1:
                                treeOne += rank;
                                break;
                            case 2:
                                treeTwo += rank;
                                break;
                            case 3:
                                treeThree += rank;
                                break;
                        }
                    }
                }

                Glyphs.Clear();

                var glyphCount = Lua.GetReturnVal<int>("return GetNumGlyphSockets()", 0);

                if (glyphCount != 0)
                {
                    for (int i = 1; i <= glyphCount; i++)
                    {
                        List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                        if (glyphInfo != null && glyphInfo[3] != "nil" && !string.IsNullOrEmpty(glyphInfo[3]))
                        {
                            Glyphs.Add(WoWSpell.FromId(int.Parse(glyphInfo[3])).Name.Replace("Glyph of ", ""));
                        }
                    }
                }
            }

            if (treeOne == 0 && treeTwo == 0 && treeThree == 0)
            {
                CurrentSpec = TalentSpec.Lowbie;
                return;
            }

            int max = Math.Max(Math.Max(treeOne, treeTwo), treeThree);
            // Altarboy.Logger.Log("[Talents] Best Tree: " + max);
            // Altarboy.Logger.Log("[Talents] Is Special Spec: " + isExtraSpec);
            int specMask = ((int) StyxWoW.Me.Class << 8);

            // Bear tanks, healing disc priests, etc.
            if (isExtraSpec)
            {
                specMask += TalentFlagIsextraspec;
            }

            if (max == treeOne)
            {
                CurrentSpec = (TalentSpec) (specMask + 0);
            }
            else if (max == treeTwo)
            {
                CurrentSpec = (TalentSpec) (specMask + 1);
            }
            else
            {
                CurrentSpec = (TalentSpec) (specMask + 2);
            }
        }

        #region Nested type: Talent

        public struct Talent
        {
            public int Count;

            public int Index;

            public int Tab;
        }

        #endregion
    }
}