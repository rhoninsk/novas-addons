 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Bugzproxy;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Altarboy
{
  static class TargetManager
    {
      
        public enum TargetMode
          {
              SingleTarget = 0,
              SingleTargetBurst = 1,
              MultiDot = 2,
              MindSear = 3,
              FocusTarget = 4,
              ExecuteTarget = 5,
              HealPlayer = 6
        }

        public static List<WoWUnit> PossibleTargets { get; set; }
        public static void AcquireTargets()
        {
            PossibleTargets = DetectHostileTargets(40);
        }

        public static void ResetTargets()
        {
            PossibleTargets = null;
        }

        public static TargetMode ActiveTargetMode = TargetMode.SingleTarget;

     //Current I can't get it to attack a target other than the currently selected target. 
      private static WoWUnit _activeTarget;
      public static WoWUnit ActiveTarget
      {
          get { return Me.CurrentTarget; }
          set
          {
               value.Target() ;
             
          }
      }

      public static void SetTarget(WoWUnit target, TargetMode targetMode)
        {
            if ( target == null || ActiveTarget == target ) return;
            ActiveTargetMode = targetMode;
            ActiveTarget = target;
         
        }

      public static Composite SelectFocusTarget()
      {
          return new Decorator(ret => PossibleTargets.Any(TargetManager.IsFocusTarget),
            new TreeSharp.Action(delegate
                {
                    //target any focus mobs, attack the one with the least amount of health. 
                    SetTarget(PossibleTargets.OrderBy(unit => unit.CurrentHealth).First(IsFocusTarget), TargetMode.FocusTarget);
                })
          );
      }

      public static Composite SelectExecuteTarget()
      {
          return new Decorator(
            ret => AltarboySettings.Instance.ShadowWordDeathForExecute &&
                    !SpellManager.Spells["Shadow Word: Death"].Cooldown &&
                    PossibleTargets.Any(unit => unit.HealthPercent <= 25 && SafeToShadowWordDeath(unit)),

            new TreeSharp.Action(
                delegate { SetTarget(PossibleTargets.First(unit => unit.HealthPercent <= 25), TargetMode.ExecuteTarget); }
            )
         );
      }

      public static Composite SelectFriendlyMindSearTarget()
      {
            //We should AOE (Mind Sear) - And use the RafLeader as the target.
             return  new Decorator(ret => AltarboySettings.Instance.UseMindSear &&
                               AltarboySettings.Instance.EnableMindSearTargeting &&
                               ( AuraManager.GetAuraTimeLeft("Mind Sear", TargetManager.ActiveTarget, true).TotalSeconds <= 1) &&
                               !Me.IsMoving &&
                               BestFriendlyMindSearTarget() != null &&
                               NumOfHostileWithinRange(BestFriendlyMindSearTarget(), 10) >=
                               AltarboySettings.Instance.MindSearHostileCount &&
                               BestFriendlyMindSearTarget().Distance2D <= 40,

                      new TreeSharp.Action(delegate { SetTarget(BestFriendlyMindSearTarget(), TargetMode.MindSear); })
              );

      }

      public static Composite SelectMultiDotTarget()
      {
           //We should MultiDot 
          return new Decorator(
              ret => (AltarboySettings.Instance.EnableMultiDotTargeting && TargetManager.NumOfHostileWithinRange(Me, 40) > 1),
              new PrioritySelector(

                  //Dot the current target first...
                  new Decorator(ret => TargetManager.TargetNeedsMultiDot(ActiveTarget ),
                         new TreeSharp.Action(delegate { SetTarget(ActiveTarget, TargetMode.MultiDot); })
                  ),

                  //Dot any other targets next
                  new Decorator(ret => TargetManager.PossibleTargets.Any(TargetManager.TargetNeedsMultiDot),
                        new TreeSharp.Action(delegate{
                            var theTarget = TargetManager.PossibleTargets.OrderByDescending(tar => tar.CurrentHealth).First(TargetManager.TargetNeedsMultiDot);
                            SetTarget(theTarget, TargetMode.MultiDot);
                        })
                      )
                  )
           );
      }
      
      public static Composite SelectLeadersTarget()
      {
          // Just use the leader's target if it's in range.
         return  new Decorator(ret => RaFHelper.Leader != null  && AltarboySettings.Instance.RaFLeaderTargeting ,
            new PrioritySelector(
                new Decorator(ret => ( RaFHelper.Leader.CurrentTarget != null && TargetManager.IsUnitInRange(RaFHelper.Leader.CurrentTarget, 40)),
                        new TreeSharp.Action(delegate { SetTarget(RaFHelper.Leader.CurrentTarget, TargetMode.SingleTarget); })
                )
            )
         );
      }

      public static Composite SelectClosestTarget()
      {
          // Just use the leader's target if it's in range.
          return new Decorator(ret => (PossibleTargets.Count > 0),
               new TreeSharp.Action(delegate { SetTarget(PossibleTargets.OrderBy(tar => tar.DistanceSqr).FirstOrDefault(),TargetMode.SingleTarget ); })
          );
      }

      public static Composite SelectUserTarget()
      {
          // Just use the leader's target if it's in range.
          return new Decorator(ret => (Me.CurrentTarget != null),
               new TreeSharp.Action(delegate { SetTarget(Me.CurrentTarget, TargetMode.SingleTarget ); })
          );
      }


      // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; }}

        #region Add Detection
     
        public static List<WoWUnit> DetectHostileTargets(int range)
        {
            var addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Distance <= range &&
                        ShouldAttack(unit))
                        .OrderBy(unit => unit.CurrentHealth).ToList();
            return addList;
        }
        #endregion
                
        public static bool IsUnitInRange(WoWUnit unit, double range)
        {
            return (unit != null && unit.Distance < range && unit.InLineOfSight);
        }

        public static bool ShouldAttack(WoWUnit unit)
        {
         
            if (unit == null)
            {
                return false;
            }

            if (IsFocusTarget(unit) && unit.IsAlive) {
                return true;
            }
            if (unit.Name == "Training Dummy" || unit.Name == "Raider's Training Dummy" )
            {
                return true;
            }
            return unit.Attackable
                   && unit.IsValid
                   && !unit.IsFriendly 
                   && !IsCrowdControlled(unit)
                   && unit.IsAlive
                   && unit.Combat
                   && (!unit.IsPlayer || unit.ToPlayer().IsHorde != Me.IsHorde)
                   && !IsMeOrMyGroup(unit)
                  // && (IsTargetingMeOrMyGroup(unit) || unit.CreatureType == WoWCreatureType.Totem)
                   && !Blacklist.Contains(unit.Guid)
                   && !IsIgnoreTarget(unit);
        }

        public static int NumOfHostileWithinRange(WoWUnit target, double range) { 
            if (target == null)
            {
                return 0;
            }
            var mobList = (from o in ObjectManager.ObjectList
                                   where o is WoWUnit && o.Distance <= 50
                                   let unit = o.ToUnit()
                                   where ShouldAttack(unit)
                                   select unit
                                    ).ToList();
          return mobList.Count(unit => target.Location.Distance(unit.Location) <= range);
        }

      public static WoWPlayer BestFriendlyMindSearTarget()
      {
          if (PartyManager.GroupMembers != null && PartyManager.GroupMembers.Count >= 1)
          {
              var MindSearTargets =
                  PartyManager.GroupMembers.Where(
                      players =>
                      players != Me &&
                      NumOfHostileWithinRange(players, 10) >= AltarboySettings.Instance.MindSearHostileCount).
                      OrderBy(players => NumOfHostileWithinRange(players, 10));

              return MindSearTargets.FirstOrDefault();
          }

          return null;
      }

      public static bool CrowdControllUnitInRange(WoWUnit target, double range)
          {
              var mobList = (from o in ObjectManager.ObjectList
                             where o is WoWUnit && o.Distance <= 50
                             let unit = o.ToUnit()
                             where unit.IsAlive
                             select unit
                            ).ToList();
              return mobList.Any(unit => IsCrowdControlled(unit) && target.Location.Distance(unit.Location) <= range);
        }

        public static bool IsCrowdControlled(WoWUnit unit)
      {
          return unit.GetAllAuras().Any(
              a => a.IsHarmful &&
                   (a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                    a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                    a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                    a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                    a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                   // a.Spell.Mechanic == WoWSpellMechanic.Stunned ||
                    a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                    a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                    a.Spell.Mechanic == WoWSpellMechanic.Sapped));
      }

        public static bool IsMeOrMyStuff(WoWUnit unit)
        {
            if (unit == null)
                return false;

            // find topmost unit in CreatedByUnit chain
            while (unit.CreatedByUnit != null)
                unit = unit.CreatedByUnit;

            // check if this unit was created by me
            return unit.IsMe;
        }

        public static bool IsTargetingMeOrMyStuff(WoWUnit unit)
        {
            return unit != null && IsMeOrMyStuff(unit.CurrentTarget);
        }

        public static bool IsMeOrMyGroup(WoWUnit unit)
        {
            if (unit != null)
            {
                // find topmost unit in CreatedByUnit chain
                while (unit.CreatedByUnit != null)
                    unit = unit.CreatedByUnit;

                if (unit.IsMe)
                    return true;

                if (unit.IsPlayer)
                {
                    WoWPlayer p = unit.ToPlayer();
                    if (p.IsHorde == Me.IsHorde && PartyManager.GroupMembers.Contains(unit.ToPlayer()))
                        return true;
                }
            }

            return false;
        }

        public static bool IsTargetingMeOrMyGroup(WoWUnit unit)
        {
            return unit != null && IsMeOrMyGroup(unit.CurrentTarget);
        }

        public enum RaidTargetIcon
        {
            None = 0,
            Star = 1,
            Circle = 2,
            Diamond = 3,
            Triangle = 4,
            Moon = 5,
            Square = 6,
            Cross = 7,
            Skull = 8,
        }
       
               private static BindingList<SpecialTarget> _AltarboySpecialTargets;
        public static BindingList<SpecialTarget> AltarboySpecialTargets
        {
            get
            {
                if(_AltarboySpecialTargets == null )
                {
                    string sPath = Process.GetCurrentProcess().MainModule.FileName;
                    sPath = Path.GetDirectoryName(sPath) + "/CustomClasses/Altarboy/utils/SpecialTargets.xml";

                    if (File.Exists(sPath))
                     {
                         var serializer = new XmlSerializer(typeof(BindingList<SpecialTarget>));
                         StreamReader reader = new StreamReader(sPath);
                         _AltarboySpecialTargets = (BindingList<SpecialTarget>)serializer.Deserialize(reader);

                     } else
                     {
                         _AltarboySpecialTargets = new BindingList<SpecialTarget>();
                     }
                }
                return _AltarboySpecialTargets;
            }
            set { _AltarboySpecialTargets = value; }
        }

        public static void SaveSpecialTargets()
      {
          if (_AltarboySpecialTargets != null)
          {
              string sPath = Process.GetCurrentProcess().MainModule.FileName;
              sPath = Path.GetDirectoryName(sPath) + "/CustomClasses/Altarboy/utils/SpecialTargets.xml";
              XmlSerializer serializer = new XmlSerializer(typeof (BindingList<SpecialTarget>));
              TextWriter tw = new StreamWriter(sPath);
              serializer.Serialize(tw, _AltarboySpecialTargets);
              tw.Close();
          }
      }

        public static bool IsSpecialTarget(WoWUnit unit)
      {
          return unit != null && unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry);
      }

        public static bool IsFocusTarget(WoWUnit unit)
      {
          return unit != null && unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.Focus);
      }

        public static bool IsIgnoreTarget(WoWUnit unit)
      {
          return unit != null && unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.Ignore);
      }
      
        public static bool IsMindSearTarget(WoWUnit unit)
      {
          return unit != null && unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.MindSear);
      }

      public static bool IsUnsafeShadowWordDeath(WoWUnit unit)
      {
          return unit != null && unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.UnsafeSWD);
      }

      public static bool IsShackleTarget(WoWUnit unit)
      {
          //Can only shackle undead.
          if (unit == null||!unit.IsUndead ) { return false; } 
          // if its already shackled - just ignore it
          if (AuraManager.GetAuraTimeLeft("Shackle Undead", unit, true).TotalSeconds  >= 3) { return false; }
          return unit.IsValid && AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.Shackle );
      }
         #region Conditions

         public static Boolean SafeToShadowWordDeath(WoWUnit unit)
         {
             if (AltarboySettings.Instance.ShadowWordDeathCheckForDebuffs)
             {
                 if (AuraManager.HasMyAura("Levitate", Me, 0)) { return false; }
                 if (Me.HealthPercent <= 25) { return false; } // Low Health (better to be alive and not have mana)
                 if (AuraManager.HasAuraStacks("Tormented", 0, Me)) { return false; }  //Baleroc FL Crystal Debuff (150% damage)
                 if (AuraManager.HasAuraStacks("Torment", 0, Me)) { return false; }  //Baleroc FL Crystal Debuff (150% damage)              
                 if (TargetManager.IsUnsafeShadowWordDeath(unit)) { return false; }

             }
             return true;
         }

         public static Boolean MSShadowFiend()
         {
             if (AltarboySettings.Instance.UseMindSpikeRotationWithShadowFiend)
             {
                 if (Me.GotAlivePet) { return true; }
             }
             return false;
         }

         public static Boolean ShouldApplyDevouringPlague()
         {
             if (TargetManager.ActiveTarget.IsMechanical && TargetManager.ActiveTarget.Level <= Me.Level )
             {
                 return false;
             }

             return !TargetManager.DetectHostileTargets(200).Any(target => AuraManager.GetAuraTimeLeft("Devouring Plague", target, true).TotalSeconds >= 1.2);
         }

         public static Boolean ShouldMindSear(WoWUnit unit)
         {

             if (unit == null)
             {
                 return false;
             }

             if (Me.IsMoving) { return false; }
             // Friendly target - in combat - mob count check.
             if (!AltarboySettings.Instance.UseMindSear) { return false; }

             //Make sure nothing is cced.
             if (TargetManager.CrowdControllUnitInRange(unit, 10)) return false;

             //Check targets against hash targets -dev// lets still make sure there are enough targets to justify searing in range (10yrds)
             if (TargetManager.IsMindSearTarget(unit) &&
                 TargetManager.NumOfHostileWithinRange(unit, 10) >= AltarboySettings.Instance.MindSearHostileCount)
             {
                 return true;
             }

             if (unit.IsFriendly && unit.Combat && TargetManager.NumOfHostileWithinRange(unit, 10) >= AltarboySettings.Instance.MindSearHostileCount) { return true; }

             return false;
         }

         public static Boolean IsMindSpikeTarget(WoWUnit unit)
         {
         
             if (unit == null)
             {
                 return false;
             }

             if (Me.IsMoving || !Me.IsSafelyFacing(unit)) { return false; }
             if (unit.IsFriendly) { return false; }


             //Check targets against hash targets
             if (AltarboySpecialTargets.Any(u => u.TargetId == unit.Entry && u.MindSpike))
             {
                 return true;
             }

             if (Me.IsInRaid && ShouldMindSpikeOverDoTs(unit) && (unit.CurrentHealth <= ((PartyManager.GroupMembers.Count * Me.MaxHealth) / 5))) { return true; }
             if ((Me.IsInParty && !Me.IsInRaid) && ShouldMindSpikeOverDoTs(unit) && (unit.CurrentHealth <= (Me.MaxHealth - (Me.MaxHealth / 4)))) { return true; }
             if (!Me.IsInParty && unit.CurrentHealth <= (Me.MaxHealth - (Me.MaxHealth / 4))) { return true; }
             return false;
         }

         // Logic to prevent ripping all DoTs off of target when casting Mind Spike.
         public static Boolean ShouldMindSpikeOverDoTs(WoWUnit unit)
         {
             if (AuraManager.GetAuraTimeLeft("Devouring Plague", unit, true).TotalSeconds >= 1.5)
             {
                 return false;
             }
             if (!AuraManager.HasMyAura("Vampiric Touch", unit))
             {
                 return true;
             }
             if (!AuraManager.HasMyAura("Shadow Word: Pain", unit))
             {
                 return true;
             }
             // The clauses below estimates a 1.2 second cast of Mind Spike
             return AuraManager.GetAuraTimeLeft("Shadow Word: Pain", unit, true).TotalSeconds <= 1.1;
         }

         public static Boolean TargetNeedsMultiDot(WoWUnit unit)
         {
             if (unit == null)
             {
                 return false;
             }
             if (unit.Distance2D > 40 || !unit.InLineOfSight) { return false; }
             return AuraManager.GetAuraTimeLeft("Vampiric Touch", unit, true).TotalSeconds < 3 || AuraManager.GetAuraTimeLeft("Shadow Word: Pain", unit, true).TotalSeconds < 1.5;
         }

         #endregion


       
    }
  
    [Serializable()]  
     public class SpecialTarget
    {
        public uint TargetId { get; set; }
        public string TargetName { get; set; }
        public bool Focus { get; set; }
        public bool Ignore { get; set; }
        public bool MindSpike { get; set; }
        public bool MindSear { get; set; }
        public bool UnsafeSWD { get; set; }
        public bool Shackle { get; set; }
    }
    

}

