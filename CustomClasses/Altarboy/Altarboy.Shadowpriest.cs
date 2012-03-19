using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Altarboy
{
    class Shadowpriest
    {

        #region Delegates
        public delegate bool SimpleBoolReturnDelegate(object context);
        public delegate WoWUnit UnitSelectionDelegate(object context);
        #endregion
        
        // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite Target()
        {
           return new PrioritySelector(                        
            
            new Decorator(ret => AltarboySettings.Instance.EnableTargeting,
                new Sequence(  
                    //cache possible targets
                    new Action(delegate {
                       TargetManager.AcquireTargets();
                    }),
                    new PrioritySelector(
                       TargetManager.SelectFocusTarget(),
                       TargetManager.SelectExecuteTarget(),
                       TargetManager.SelectFriendlyMindSearTarget(),
                       TargetManager.SelectMultiDotTarget(),
                       TargetManager.SelectLeadersTarget(),
                       TargetManager.SelectClosestTarget()
                       
                    ),
                    //reset target cache
                      new Action(delegate {
                       TargetManager.ResetTargets();
                    }),

                    //If we still dont have a target here, fail this shit.
                     new Decorator(ret => (TargetManager.ActiveTarget == null), new ActionAlwaysFail() )
                 )
             ),
             
             TargetManager.SelectUserTarget()
           

           );
        }

        public static Composite Pull()
        {

            return AltarboyComposites.CastSpell("Devouring Plague", ret => true, TargetManager.ActiveTarget);
             
        }
             
        public static Composite Combat() {
            return new Sequence(
              
                //Select a target
                Target(),                              
                
                //Then kill it.
                new Decorator(
                      ret => TargetManager.ActiveTarget  != null 
                      && !CastManager.IsHymning() 
                      && !Me.IsFalling
                      && !TargetManager.IsIgnoreTarget(TargetManager.ActiveTarget), //Added falling clause cause was killing me on Ragnaros.
                       new PrioritySelector(
                           MindSearDps(ret => TargetManager.ShouldMindSear(TargetManager.ActiveTarget) && AuraManager.GetAuraTimeLeft("Mind Sear", TargetManager.ActiveTarget, true).TotalSeconds <= AltarboySettings.Instance.ClippingDuration),
                           SingleTargetDps()
                       )             
                )           
                    
            );
                    
        }

       #region CoolDowns

        private static Composite UseCoolDowns()
        {
            return new PrioritySelector(

                AltarboyComposites.CastSpell("Power Word: Shield", ret => (AltarboySettings.Instance.AutoShield && !Me.HasAura("Weakened Soul")), Me),
                
                //Volcanic Potion during Bloodlust (will move this out when I can)
                //This should be using user settings now - untested.
                AltarboyComposites.UseItemById(AltarboySettings.Instance.FavouritePotionID, ret => (
                    AltarboySettings.Instance.UsePotionsOnLust && 
                    AltarboySettings.Instance.FavouritePotionID > 0 &&
                    CastManager.CanUseItem(AltarboySettings.Instance.FavouritePotionID) &&
                    AuraManager.IsLusting(Me))
                ), 

                AltarboyComposites.UseSlottedItem(
                        ret => AltarboySettings.Instance.Trinket1UseOnCD || (AltarboySettings.Instance.Trinket1UseWithArchangel && AuraManager.HasMyAura("Dark Archangel",Me)),
                     StyxWoW.Me.Inventory.Equipped.Trinket1
                ),
                 AltarboyComposites.UseSlottedItem(
                        ret => AltarboySettings.Instance.Trinket2UseOnCD || (AltarboySettings.Instance.Trinket2UseWithArchangel && AuraManager.HasMyAura("Dark Archangel", Me)),
                     StyxWoW.Me.Inventory.Equipped.Trinket2
                ),

                 AltarboyComposites.UseSlottedItem(
                        ret => AltarboySettings.Instance.EngUseBelt,
                         StyxWoW.Me.Inventory.Equipped.Waist 
                ),
               
                 AltarboyComposites.UseSlottedItem(
                        ret => AltarboySettings.Instance.EngUseGloves ||  (AltarboySettings.Instance.EngUseGlovesWithArchangel && AuraManager.HasMyAura("Dark Archangel", Me)),
                         StyxWoW.Me.Inventory.Equipped.Hands 
                )
              

            );
        }
        #endregion

       #region DPS Composites
      
       private static Composite SingleTargetDps()
        {
           //Never Attack Anything That we shouldnt
           return new Decorator(
               ret => TargetManager.ShouldAttack(TargetManager.ActiveTarget),
                    new PrioritySelector(                   
                      UseCoolDowns(), //cant seem to detect if a trinket can but used or not... gonna have to use user settings.
                      new Decorator(ret => AuraManager.HasAuraStacks("Molten Feather", 2, Me), NormalDpsRotation(ret => true)), // Alysrazor
                      new Decorator(ret => TargetManager.IsShackleTarget(TargetManager.ActiveTarget), ShackleTarget()),
                      new Decorator(ret => Me.IsMoving , MovementDps()),
                      new Decorator(ret => TargetManager.MSShadowFiend(), MindSpikeRotation()),
                      new Decorator(ret => TargetManager.IsMindSpikeTarget(TargetManager.ActiveTarget), MindSpikeRotation()),
                      new Decorator(ret => !TargetManager.IsMindSpikeTarget(TargetManager.ActiveTarget) && !TargetManager.MSShadowFiend(), NormalDpsRotation(ret => true))
                      //,
                      //new ActionAlwaysFail()
                     )
               );
         }


    
       private static Composite MovementDps() {
           return new PrioritySelector(                  
               
                //Use dispersion when low on mana.
                AltarboyComposites.CastSpell("Dispersion", ret => (Me.ManaPercent <= AltarboySettings.Instance.DispersionManaPercent) , Me),

                 // I think it always makes sense to use pain first... this cans generate orbs, and apparitions.
                  AltarboyComposites.CastSpell("Shadow Word: Pain", ret => (
                      AuraManager.GetAuraTimeLeft("Shadow Word: Pain", TargetManager.ActiveTarget, true).TotalSeconds <= .4)
                  , TargetManager.ActiveTarget),

                   //Shadow Word Death While Moving               
                   AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                       TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) &&
                       AltarboySettings.Instance.ShadowWordDeathWhileMoving
                   ), TargetManager.ActiveTarget),


                   //Shadowfiend on CD or as mana regen.
                   AltarboyComposites.CastSpell("Shadowfiend", ret => (
                       (AltarboySettings.Instance.UseShadowFiendOnCD || (AltarboySettings.Instance.UseShadowFiendForManaRegen && Me.ManaPercent <= AltarboySettings.Instance.UseShadowFiendManaPercent))                       )
                       , TargetManager.ActiveTarget
                   ),

                   //This will spam cause its good dps...
                   AltarboyComposites.CastSpell("Devouring Plague", ret => true, TargetManager.ActiveTarget)              

           );
       }

       private static Composite NormalDpsRotation(SimpleBoolReturnDelegate extra){

          // Altarboy.Logger.Slog(Color.Lime, "--- Single Target Max Dps Rotation  --- ");
           return new Sequence(
               
            //this is the stationary composite, so it shouldn't be an issue to face the target.
            new TreeSharp.Action(delegate { if (!Me.IsMoving && !Me.IsSafelyFacing(TargetManager.ActiveTarget)) { TargetManager.ActiveTarget.Face(); } }),

            new Decorator(
                ret => extra(ret),
                new PrioritySelector(

                    // This doesnt belong here but Im using it for raiding at the moment... will move later.
                    AltarboyComposites.CastSpell("Dispersion", ret => (AuraManager.HasAuraStacks("Tormented", 14, Me)), Me),

                   //Shadow Word Death On Exectue                  
                   AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                       TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) &&
                       AltarboySettings.Instance.ShadowWordDeathForExecute &&
                      TargetManager.ActiveTarget.HealthPercent <= 25
                   ), TargetManager.ActiveTarget),

                   //Shadowfiend on Bloodlust.
                   AltarboyComposites.CastSpell("Shadowfiend", ret => (
                       AltarboySettings.Instance.ForceFiendOnBloodlust &&
                       AuraManager.IsLusting(Me)), TargetManager.ActiveTarget
                   ),

                     //Shadowfiend on Bloodlust.
                   AltarboyComposites.CastSpell("Shadowfiend", ret => (
                       AltarboySettings.Instance.ForceShadowFiendOnArchangel  &&
                        AuraManager.HasMyAura("Dark Archangel", Me)), TargetManager.ActiveTarget
                   ),

                   // I think it always makes sense to use pain first... this can generate orbs, and apparitions.
                   AltarboyComposites.CastSpell("Shadow Word: Pain", ret => (
                       AuraManager.GetAuraTimeLeft("Shadow Word: Pain", TargetManager.ActiveTarget, true).TotalSeconds <= .4),
                     TargetManager.ActiveTarget
                   ),
                   
                  

                    AltarboyComposites.CastSpell("Mind Flay", ret => (
                       !Altarboy.AuraManager.HasAuraStacks("Dark Evangelism", 5, Me) &&
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.GetAuraTimeLeft("Mind Flay", TargetManager.ActiveTarget, true).TotalSeconds <= AltarboySettings.Instance.ClippingDuration)
                   ), TargetManager.ActiveTarget),

                   // If there is an orb to use, and ES is close to running out... Refresh it before applying dots.
                   AltarboyComposites.CastSpell("Mind Blast", ret =>
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       AuraManager.HasMyAura("Shadow Orb", Me, 1) && 
                       AuraManager.GetAuraTimeLeft("Empowered Shadow", Me, true).TotalSeconds < 4 ,
                      TargetManager.ActiveTarget
                   ),

                   AltarboyComposites.CastSpell("Vampiric Touch", ret => (                      
                       CastManager.LastSpellCast != "Vampiric Touch" &&
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.GetAuraTimeLeft("Vampiric Touch", TargetManager.ActiveTarget, true).TotalSeconds <= 2.6)
                   ), TargetManager.ActiveTarget),


                   // DP goes second cause you can cast while moving and it does instant damage. 
                   AltarboyComposites.CastSpell("Devouring Plague", ret => (                     
                       TargetManager.ShouldApplyDevouringPlague()
                   ), TargetManager.ActiveTarget),


                   // --- Trying to delay mana regen stuff untill dots are up if possible.  ---//

                   //Use dispersion when low on mana.
                   AltarboyComposites.CastSpell("Dispersion", ret => (Me.ManaPercent <= AltarboySettings.Instance.DispersionManaPercent), Me),

                   //Shadowfiend on CD or as mana regen.
                   AltarboyComposites.CastSpell("Shadowfiend", ret => (
                       (AltarboySettings.Instance.UseShadowFiendOnCD || (AltarboySettings.Instance.UseShadowFiendForManaRegen && Me.ManaPercent <= AltarboySettings.Instance.UseShadowFiendManaPercent)) &&
                       AuraManager.GetAuraTimeLeft("Mind Flay", TargetManager.ActiveTarget, true).TotalSeconds <= 0.5)
                       , TargetManager.ActiveTarget
                   ),

                   //Only use Archangel when there are 5 stacks up.
               //TODO - Want to add logic to make sure this isn't cast when dots are close to needing a refresh
               // - the goal is to make sure that right after this we can maximize mindblasts and mindflays.
                   AltarboyComposites.CastSpell("Archangel", ret => (
                       AuraManager.HasMyAura("Dark Evangelism", Me, 5) &&
                       AuraManager.GetAuraTimeLeft("Vampiric Touch", TargetManager.ActiveTarget, true).TotalSeconds > 8 &&
                       AuraManager.GetAuraTimeLeft("Mind Flay", TargetManager.ActiveTarget, true).TotalSeconds <= 0.5)
                       , Me
                   ),

                   //Shadow Word Death For Mana                 
                   AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                      TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) &&
                       AltarboySettings.Instance.ShadowWordDeathForManaOnly &&
                       Me.ManaPercent <= AltarboySettings.Instance.ShadowWordDeathManaPercent
                   ), TargetManager.ActiveTarget),


                   //Shadow Word on CD (Not sure who would want this)
                    AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                       TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) &&
                       AltarboySettings.Instance.ShadowWordDeathOnCD
                   ), TargetManager.ActiveTarget),

                   //Mind blast freely if we have at least ten seconds of ES left or if use wants to fire it on cooldown
                   AltarboyComposites.CastSpell("Mind Blast", ret =>
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.GetAuraTimeLeft("Empowered Shadow", Me, true).TotalSeconds > 9 || AltarboySettings.Instance.UseMindBlastOnCD)
                       , TargetManager.ActiveTarget
                    ),

                   //Mind blast if we have at least 1 orb up and no ES at all. (Want to get ES up as fast as possible)
                   AltarboyComposites.CastSpell("Mind Blast", ret =>
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       AuraManager.HasMyAura("Shadow Orb", Me, 1)
                    , TargetManager.ActiveTarget),

                   //Always use MB when you have 3 orbs
                   AltarboyComposites.CastSpell("Mind Blast",
                   ret => Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.HasMyAura("Shadow Orb", Me, 3)
                   ), TargetManager.ActiveTarget),


                   //Mind flay when when you have nothing else to do.. Try and clip to account for lag.
                   AltarboyComposites.CastSpell("Mind Flay", ret => (
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.GetAuraTimeLeft("Mind Flay", TargetManager.ActiveTarget, true).TotalSeconds <= AltarboySettings.Instance.ClippingDuration)
                   ), TargetManager.ActiveTarget)

                )
           )

         );
            
       }

       private static Composite ShackleTarget()
       {
           return new Sequence(            
           //this is the stationary composite, so it shouldn't be an issue to face the target.
           // new TreeSharp.Action(delegate { if (!Me.IsMoving) { TargetManager.ActiveTarget.Face(); } }),          
               new PrioritySelector(
                   AltarboyComposites.CastSpell("Shackle Undead", ret => AuraManager.HasMyAura("Shackle Undead", TargetManager.ActiveTarget, 3), TargetManager.ActiveTarget)
          ));
       }

       private static Composite MindSpikeRotation()
       {
            return new Sequence(
            //this is the stationary composite, so it shouldn't be an issue to face the target.
           new TreeSharp.Action(delegate { if (!Me.IsMoving) { TargetManager.ActiveTarget.Face(); } }),

           //Need to add code here to respect the SWD logic specified by settings..
        
                new PrioritySelector(

                  AltarboyComposites.CastSpell("Archangel", ret => (
                       AuraManager.HasMyAura("Dark Evangelism", Me, 5)), Me ),
                   
                    //Shadow Word Death On Exectue                  
                   AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                       TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) &&
                       AltarboySettings.Instance.ShadowWordDeathForExecute &&
                       TargetManager.ActiveTarget.HealthPercent <= 25
                   ), TargetManager.ActiveTarget),

                    AltarboyComposites.CastSpell("Mind Blast", ret => AuraManager.HasMyAura("Mind Spike", TargetManager.ActiveTarget, 3), TargetManager.ActiveTarget),
                    AltarboyComposites.CastSpell("Shadow Word: Death", ret => (TargetManager.SafeToShadowWordDeath(TargetManager.ActiveTarget) && AltarboySettings.Instance.ShadowWordDeathForManaOnly && Me.ManaPercent <= AltarboySettings.Instance.ShadowWordDeathManaPercent), TargetManager.ActiveTarget),
                    AltarboyComposites.CastSpell("Mind Spike", ret => true, TargetManager.ActiveTarget),
                    AltarboyComposites.CastSpell("Mind Flay", ret => (
                       Me.IsSafelyFacing(TargetManager.ActiveTarget) &&
                       (AuraManager.GetAuraTimeLeft("Mind Flay", TargetManager.ActiveTarget, true).TotalSeconds <= AltarboySettings.Instance.ClippingDuration)
                   ), TargetManager.ActiveTarget)
               
           ));                  
       }

       private static Composite MindSearDps(SimpleBoolReturnDelegate extra)
       {
           
          //FOR NOW WE WILL ASSUME THAT WE ONLY MIND SEAR ON FRIENDLY TARGETS
          // EVENTUALLY THIS WONT BE THE CASE.
           return new Sequence(

            //this is the stationary composite, so it shouldn't be an issue to face the target.
            new TreeSharp.Action(delegate { if (!Me.IsMoving) { TargetManager.ActiveTarget.Face(); } }),

            new Decorator(
                ret => extra(ret) ,
                new PrioritySelector(

                    // lets trigger ES first if possible
                   //  AltarboyComposites.CastSpell("Mind Blast", ret =>
                   //    Me.CurrentTarget.CurrentTarget != null &&
                   //    Me.IsSafelyFacing(Me.CurrentTarget.CurrentTarget ) &&
                   //    !Me.IsMoving &&
                   //    AuraManager.HasMyAura("Shadow Orb", Me, 1)
                   // , Me.CurrentTarget.CurrentTarget),

                   // // Mind sear is expensive... lets get some mana back.
                   //AltarboyComposites.CastSpell("Shadowfiend", ret => (
                   //      Me.CurrentTarget.CurrentTarget != null &&
                   //    (AltarboySettings.Instance.UseShadowFiendOnCD || (AltarboySettings.Instance.UseShadowFiendForManaRegen && Me.ManaPercent <= AltarboySettings.Instance.UseShadowFiendManaPercent)) &&
                   //    AuraManager.GetAuraTimeLeft("Mind Sear", Me.CurrentTarget, true).TotalSeconds <= 0.5),
                   //    Me.CurrentTarget.CurrentTarget
                   //),

                    ////Lets Execute with SWD if we can.    
                   
                    //AltarboyComposites.CastSpell("Shadow Word: Death", ret => (                          
                    //        SafeToShadowWordDeath() &&
                    //        AltarboySettings.Instance.ShadowWordDeathForExecute &&
                    //        TargetManager.DetectHostileTargets(40).Any(target => (target.HealthPercent <= 25 && target.Distance <= 35))
                    // ), TargetManager.DetectHostileTargets(40).First(target => (target.HealthPercent <= 25 && target.Distance <= 35))),
                   
                                   

                    //// lets use some SWD if possible... good time to get some mana back
                    //AltarboyComposites.CastSpell("Shadow Word: Death", ret => (
                    //                    SafeToShadowWordDeath() &&
                    //                    Me.ManaPercent <= AltarboySettings.Instance.ShadowWordDeathManaPercent &&
                    //                    AltarboySettings.Instance.ShadowWordDeathForManaOnly
                    //), false, Me.CurrentTarget.CurrentTarget),

                    AltarboyComposites.CastMindSear(ret => (AuraManager.GetAuraTimeLeft("Mind Sear", TargetManager.ActiveTarget, true).TotalSeconds <= 0.4))
                )
               // new TreeSharp.Action(delegate { CastManager.CastSpell("Mind Sear"); })
           ));
       }
       
       #endregion

       
     
    }
}
