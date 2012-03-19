using System;
using System.Drawing;
using System.Linq;
using CommonBehaviors.Actions;
using Altarboy;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Altarboy
{
    class AltarboyComposites
    {
    

        #region Delegates
            public delegate bool SimpleBoolReturnDelegate(object context);
            public delegate WoWUnit UnitSelectionDelegate(object context);
        #endregion

        // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite CastMindSear(SimpleBoolReturnDelegate extra)
            {

                //Todo - Should add logic to check for CC'ed targets within Mindsear Range.
                return new Decorator(
                 ret => extra(ret),  
                 new Sequence(
                            //new Action(ret => Navigator.PlayerMover.MoveStop()), | Beyond annoying.                           
                            new Action(ret => TargetManager.ActiveTarget .Face()),
                            new Action(ret => CastManager.CastSpell("Mind Sear", TargetManager.ActiveTarget))
                  )              
                );
            }

        public static Composite UseSlottedItem(SimpleBoolReturnDelegate extra, WoWItem slottedItem )
            {              
                   return new Decorator(
                    ret => extra(ret) && (
                        slottedItem != null &&
                        slottedItem.BaseAddress != 0 &&                     
                        slottedItem.Cooldown == 0                                                                                                     
                    ),                   
                    new Action(delegate{                    
                     slottedItem.Use();
                     Logger.CastLog(slottedItem.Name, Me.Name);
                    } )                    
                 );

            }
            

        public static Composite CastSpell(string spellName, SimpleBoolReturnDelegate extra,  WoWUnit target)
            {
                //using current target no matter what... just testing something
                return new Decorator(
                    ret => extra(ret) && CastManager.CanCast(spellName, TargetManager.ActiveTarget ),
                    new Action(delegate { CastManager.CastSpell(spellName, TargetManager.ActiveTarget); })
                      
                );
           }

        public static Composite CastHeal(string spellName, SimpleBoolReturnDelegate extra, WoWUnit target)
        {
            //using current target no matter what... just testing something
            return new Decorator(
                ret => extra(ret) && CastManager.CanCast(spellName, target),
                new Action(delegate
            {
                CastManager.CastSpell(spellName, target);
            })

            );
        }
     
        public static Composite BuffSelf(string buffName, SimpleBoolReturnDelegate extra)
        {
            return new Decorator(
                ret => extra(ret) && !Me.Auras.Values.Any(a => a.Spell.Name == buffName) && CastManager.CanCast(buffName, Me),
                new Action(delegate
                    {
                        CastManager.CastSpell(buffName, Me); 
                    })
             );
        }

        public static Composite BuffFortitude()
        {
            return new Decorator(
               ret => (AltarboySettings.Instance.BuffFortitude && PartyManager.NearbyGroupMembers.Any(CanCastFortitudeOn) && CastManager.CanCast("Power Word: Fortitude", Me)),
                   new Action(delegate {  CastManager.CastSpell("Power Word: Fortitude", Me); })
            );
        }


        public static Composite BuffShadowProtection()
        {
            return new Decorator(
               ret => (AltarboySettings.Instance.BuffShadowProtection &&  CastManager.CanCast("Shadow Protection", Me) &&
                   PartyManager.NearbyGroupMembers.Any(CanCastShadowProtection)),
                  new Action(delegate { CastManager.CastSpell("Shadow Protection", Me); })
            );
        }

        //credits singular
        public static bool CanCastFortitudeOn(WoWUnit unit)
        {
            return !AuraManager.UnitHasAura(unit, "Power Word: Fortitude");
        }

        public static bool CanCastShadowProtection(WoWUnit unit)
        {
            return !AuraManager.UnitHasAura(unit, "Shadow Protection");
        }

        public static bool CanPowerWordShield(WoWUnit target)
        {
            return !AuraManager.HasAuraStacks("Weakened Soul", 0, target) && CastManager.CanCast("Power Word: Shield", target) ;
        }

        public static Composite CommonPriestBuffs()
        {
          return new Decorator(ret => !CastManager.IsHymning() && (!Me.HasAura("Food") || !Me.HasAura ("Drink")),
             new PrioritySelector(
                HandleFalling(),
                BuffFortitude(),
                BuffShadowProtection(),
                BuffSelf("Shadowform", ret => (AltarboySettings.Instance.UseShadowForm)),
                BuffSelf("Vampiric Embrace", ret => (AltarboySettings.Instance.UseVampiricEmbrace)),
                BuffSelf("Inner Fire", ret => (AltarboySettings.Instance.UseInnerFire)),
                BuffSelf("Inner Will", ret => (AltarboySettings.Instance.UseInnerWill))
            ) );
        }

        //We should add a glyph check here. If someone doesn't have the levitate glphy this will kill them.
        public static Composite HandleFalling()
        {
            return new Decorator(ret => AltarboySettings.Instance.LevitateWhileFalling && Me.IsFalling,
             
                new PrioritySelector(
                    
                    //Do I already have levitate
                    new Decorator(ret => AuraManager.HasMyAura("Levitate", Me),
                        new Sequence(
                            new WaitContinue(TimeSpan.FromSeconds(2), ret => AuraManager.HasMyAura("Levitate", Me), new ActionAlwaysSucceed()),
                            new Action(delegate{Lua.DoString("CancelUnitBuff(\"player\", \"Levitate\")");})
                        )
                    ),

                    //I don't have levitate.
                    new Decorator(ret => !AuraManager.HasMyAura("Levitate", Me),
                        new Sequence(
                            new WaitContinue(TimeSpan.FromSeconds(4), ret => Me.IsFalling,new ActionAlwaysSucceed()),
                            new Decorator(ret => Me.IsFalling, BuffSelf("Levitate", ret => (Me.IsFalling)))
                        )
                   )
                )
           );
        }

        public static Composite UseItemById(int itemId, SimpleBoolReturnDelegate extra)
        {
            return new PrioritySelector(
                new Decorator(
                     ret => extra(ret),
                     new Action(delegate{
                         CastManager.UseItemById(itemId);
                         Logger.Log("Using " + CastManager.GetItemById(itemId) + ". Lusting? " + AuraManager.IsLusting(Me));
                     })
                ));
        }

        #region Healing

        public static Composite HealSelf()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.HealthPercent <= 20, CastSpell("Flash Heal", ret => (true), Me)),
                new Decorator(ret =>  Me.HealthPercent <= 30, CastSpell("Penance", ret =>(true), Me)),
                new Decorator(
                    ret => AuraManager.GetAuraTimeLeft("Renew", Me, true).TotalSeconds <= 1 && Me.HealthPercent <= 80,
                    CastSpell("Renew",ret =>(true),Me)
                ),

                new Decorator(
                    ret => CanPowerWordShield(Me) && Me.HealthPercent <= 90,
                    CastSpell("Power Word: Shield",ret =>(true),Me)
                )
            );
          
        }
              
               
        #endregion

        public static Composite CreateMoveToAndFace(float maxRange, float coneDegrees, UnitSelectionDelegate unit, bool noMovement)
        {
            return new Decorator(
                ret => unit(ret) != null,
                new PrioritySelector(
                    new Decorator(
                        ret => (!unit(ret).InLineOfSightOCD || (!noMovement && unit(ret).DistanceSqr > maxRange * maxRange)),
                        new Action(ret => Navigator.MoveTo(unit(ret).Location))),
                //Returning failure for movestop for smoother movement
                //Rest should return success !
                    new Decorator(
                        ret => Me.IsMoving && unit(ret).DistanceSqr <= maxRange * maxRange,
                        new Action(delegate
                        {
                            Navigator.PlayerMover.MoveStop();
                            return RunStatus.Failure;
                        })),
                    new Decorator(
                        ret => TargetManager.ActiveTarget != null && TargetManager.ActiveTarget.IsAlive && !Me.IsSafelyFacing(TargetManager.ActiveTarget, coneDegrees),
                        new Action(ret => TargetManager.ActiveTarget.Face()))
                    ));
        }


    }
}
