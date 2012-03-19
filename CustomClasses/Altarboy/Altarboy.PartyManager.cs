using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Altarboy
{
    internal class PartyManager
    {
        // Current Player   
        public static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        /// <summary>
        /// Return a list of group other wow players in your party or raid.
        /// </summary>
        public static List<WoWPlayer> GroupMembers
        {
            get
            {
                var groupMembers = new List<WoWPlayer>();
                groupMembers = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                groupMembers.Add(Me);
                return groupMembers;
            }
        }

        public static List<WoWPlayer> NearbyGroupMembers
        {
            get { return GroupMembers.Where(p => p.DistanceSqr <= 40 * 40 && p.IsFriendly &&! p.Dead && !p.IsGhost).ToList(); 
            }
        }

        public static WoWPlayer getNearbyPlayerByName(string playerName) { 
             return NearbyGroupMembers.Where(p => p.Name == playerName).FirstOrDefault();          
        }

        public static Boolean IsInParty(string targetName)
        {
            bool inGroup = (from members in GroupMembers
                            where members.Name == targetName
                            select members).Any();

            return inGroup;
        }
    }
}