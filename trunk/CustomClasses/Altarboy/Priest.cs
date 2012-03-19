
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Globalization;
using CommonBehaviors.Actions;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using System.Drawing;


namespace Altarboy
{
    partial class Priest : CombatRoutine
    {
        #region Meta Information
        public static string Version { get { return "Version 1.2 - Dragon Soul Friendly"; } }
            public override WoWClass Class {  get { return WoWClass.Priest; } }
            public override string Name  { get { return "Altarboy : The Priest Companion : " + Version ; } }
            public static string Credits = "Created by spriestdev & altarboydev";
        #endregion

          public static Altarboy.TalentSpec ActiveSpec;
          public static Boolean IsLazyraider = false;

          WaitTimer _movementCheckTimer = WaitTimer.OneSecond;
          WoWPoint _lastLocation = StyxWoW.Me.Location;
          public static  bool _amIMoving;


          public override void Pulse()
          {
              if (_movementCheckTimer.IsFinished)
              {
                  if (StyxWoW.Me.Location.Distance(_lastLocation) > 1) // You'd have to play with this # i'm sure... 
                  {
                      _amIMoving = true;
                  }
                  _lastLocation = StyxWoW.Me.Location;
                  _movementCheckTimer.Reset();
              }
          }

           // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Boolean IsHealer
        {
            get
            {
                return GetGroupRoleAssigned(Me) == "HEALER";
            }
        }

        //Credits Lazy Raider
        public static string GetGroupRoleAssigned(WoWPlayer p)
        {
            string sRole = "NONE";
            if (ObjectManager.Me.IsInParty || ObjectManager.Me.IsInRaid)
            {
                try
                {
                    string luaCmd = "return UnitGroupRolesAssigned(\"" + p.Name + "\")";
                    sRole = Lua.GetReturnVal<string>(luaCmd, 0);
                }
                catch
                {
                    sRole = "NONE";
                }
            }

            return sRole;
        }


        public override void Initialize()
        {
            base.Initialize();

            //Version check
            Logger.Log(Honorbuddy.Resources.HonorbuddyResources.VersionLabelText);


            Altarboy.Logger.Log("----------------------------------------------------");
            Altarboy.Logger.Log( string.Format("Welcome to {0}!", Name));
            Altarboy.Logger.Log(Credits);

            //Check to see what bot they are using.
            IsLazyraider = TreeRoot.Current != null && "LAZYRAIDER" == TreeRoot.Current.Name.ToUpper();
            Logger.Log(IsLazyraider
                            ? "LazyRaider Enabled"
                            : "LazyRaider Disabled - Currently Altarboy has only been tested using LazyRaider. Use at yourown risk.");

            //if (File.Exists(Logging.ApplicationPath + @"\CustomClasses\Altarboy\utils\For_The_Horde.wav"))
            //{ 
            //    var myPlayer = new System.Media.SoundPlayer();
            //    myPlayer.SoundLocation = Logging.ApplicationPath + @"\CustomClasses\Altarboy\utils\For_The_Horde.wav";
            //    myPlayer.Play();
            //}

            if(Me.Class != WoWClass.Priest ){StopBot("Altarboy is a Priest Custom Class - Stopping Bot!");}

            Altarboy.TalentManager.Update();
            switch (Altarboy.TalentManager.CurrentSpec) { 
                
                case Altarboy.TalentSpec.ShadowPriest:
                   ActiveSpec = Altarboy.TalentSpec.ShadowPriest;
                   Altarboy.Logger.Log( "You're a Shadow Priest!", Color.MediumPurple);                 
                 break;
               
                case Altarboy.TalentSpec.HolyPriest:
                    ActiveSpec = Altarboy.TalentSpec.HolyPriest;
                 Altarboy.Logger.Log("You're a Holy Priest!", Color.MediumPurple);   
                 break;
               
                case Altarboy.TalentSpec.DisciplinePriest:
                 ActiveSpec = Altarboy.TalentSpec.DisciplinePriest;
                 Altarboy.Logger.Log("You're a Discipline Priest!", Color.MediumPurple);   
                 break;
               
                default:
                     ActiveSpec = Altarboy.TalentSpec.Lowbie;
                   Altarboy.Logger.Log( "You're a Lowbie Priest!", Color.MediumPurple);   
                 break;
            }
            Altarboy.Logger.Log("----------------------------------------------------");
          

            //Creating Behaviours
            if (!CreateBehaviors()){return;}

            Logging.WriteDebug("Altarboy Init...");

        }
     
        private Composite _restBehavior;
        public override Composite RestBehavior { get { return _restBehavior; } }
        
        private Composite _preCombatBuffBehavior;
        public override Composite PullBuffBehavior{get { return _preCombatBuffBehavior; }}
        
        private Composite _pullBehaviour;
        public override Composite PullBehavior{get { return _pullBehaviour; }}

        private Composite _combatBehavior;
        public override Composite CombatBehavior { get { return _combatBehavior; } }

        private Composite _combatBuffBehavior;
        public override Composite CombatBuffBehavior { get { return _combatBuffBehavior; } }


        public bool CreateBehaviors(){
            _restBehavior = CreateRestBehaviour();
            _preCombatBuffBehavior = CreatePreCombatBuffBehavior();
            _pullBehaviour = CreatePullBehaviour();
            _combatBehavior = CreateCombatBehaviour();
            _combatBuffBehavior = CreateCombatBuffBehavior();
            return true;
        }

        private static Composite CreateRestBehaviour()
        {
            return new Decorator(ret => !Me.Combat && !StyxWoW.Me.Mounted,
                new PrioritySelector(                   
                 
                    AltarboyComposites.CommonPriestBuffs() 
           ));
         
        } 

       private Composite CreatePreCombatBuffBehavior()
        {
            return new PrioritySelector(
                  
                  new Decorator(
                         ret => (ActiveSpec == Altarboy.TalentSpec.ShadowPriest),
                         Shadowpriest.Pull()
                   )
              );
        }

        private Composite CreatePullBehaviour()
        {
            return new PrioritySelector(
                  new Decorator(
                         ret => (ActiveSpec == Altarboy.TalentSpec.ShadowPriest),
                         Shadowpriest.Pull()
                   )
              );  
        }
        
        private static Composite CreateCombatBehaviour()
        {
            return new PrioritySelector(              

                new Decorator(
                       ret => (ActiveSpec == TalentSpec.ShadowPriest),
                       Shadowpriest.Combat()
                )
            );   
        }

        private Composite CreateCombatBuffBehavior()
        {
           
            return new Decorator(
                ret => Me.Combat && !CastManager.IsHymning(),
                new PrioritySelector(
                   AltarboyComposites.BuffSelf("Inner Fire", ret => (AltarboySettings.Instance.UseInnerFire)),
                   AltarboyComposites.BuffSelf("Inner Will", ret => (AltarboySettings.Instance.UseInnerWill)),
                   
                   ////Is there something I should dispell from my self?
                   // new Decorator(
                   //     ret => (Me.Debuffs).Any(aura => AltarboyHashes.TargetHasAuraType(aura.Value.SpellId, AltarboyHashes.AuraTypes.DispelAsap)),
                   //     new Decorator(ret => CastManager.CanCast("Dipel Magic", Me),
                   //         new TreeSharp.Action(delegate
                   //         {
                   //             CastManager.CastSpell("Dipel Magic", Me);
                   //         })
                   //     )
                   // ),

                    //Do I need to fade? If something is aggroing me and within a specific range.                     
                    AltarboyComposites.BuffSelf("Fade", ret => (Me.IsInParty || Me.IsInMyRaid) && AltarboySettings.Instance.EnableFadeOnAggro &&
                        TargetManager.DetectHostileTargets(10).Any(Unit => TargetManager.IsTargetingMeOrMyStuff(Unit))
                    ),

                   AltarboyComposites.CommonPriestBuffs()

                ));
        }
       
      
        public override bool WantButton{get{return true;}}
        
        public override void OnButtonPress()
        {
        
           var cfg = new AltarboyConfig();
           cfg.ShowDialog();
        }

        private static void StopBot(string reason)
        {
            Logger.Log(reason);
            TreeRoot.Stop();
        }
        
    }
}
