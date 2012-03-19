using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace PvPRogue.Managers
{
    class PlayerObjects
    {
        /// <summary>
        /// returns number of enemys focused on me
        /// </summary>
        public static int EnemysFocusedOnMe
        {
            get
            {
                return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                        where Unit.IsAlive
                        where Unit.Distance < 35
                        where !Unit.IsFriendly
                        where Unit.InLineOfSight
                        where Unit.CurrentTarget != null
                        where Unit.CurrentTarget.Guid == StyxWoW.Me.Guid
                        select Unit).Count();
            }
        }

        /// <summary>
        /// returns number of enemys around
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public static int EnemysAround(float Distance)
        {
            return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                    where Unit.IsAlive
                    where Unit.Distance <= Distance
                    where !Unit.IsFriendly
                    select Unit).Count();
        }

        /// <summary>
        /// Returns number of team mates around
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public static int TeamAround(int Distance)
        {
            return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false, true)
                    where Unit.IsAlive
                    where Unit.Distance < Distance
                    where Unit.IsFriendly
                    select Unit).Count();
        }

        public static int MeleeTargeting
        {
            get
            {
                return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                        where Unit.IsAlive
                        where Unit.IsWithinMeleeRange
                        where Unit.Class != WoWClass.Priest 
                        where Unit.Class != WoWClass.Hunter
                        where Unit.Class != WoWClass.Warlock
                        where Unit.Class != WoWClass.Mage
                        where !Unit.IsFriendly
                        select Unit).Count();
            }
        }

        /// <summary>
        /// Enemys percent, 50 > means more enemys than team mates
        /// 1 means 1 team - 10 enemys etc
        /// </summary>
        public static double EnemysPercent
        {
            get 
            { 
                int Team = TeamAround(35);
                int Enemy = EnemysAround(35);
                int Max = Math.Max(Team, Enemy);

                return (double)((double)((double)Enemy / (double)Max) * (double)100); 
            }
        }
    }
}
