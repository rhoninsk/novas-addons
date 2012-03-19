
using System;
using System.Linq;
using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;


namespace Altarboy
{
    class CastManager
    {
        public static string LastSpellCast;
    
        // Current Player   
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Boolean CastSpell(string spellName, WoWUnit target)
        {
             Logger.CastLog(spellName, target.Name);   
             LastSpellCast = spellName;
             return  SpellManager.Cast(spellName, target);   
                
        }    
    
      

        public static void UseItemById(int itemId)
        {
            var item = GetItemById(itemId);
            if (item != null) {
                item.UseContainerItem(); 
            }
                      
        }


        public static void UseItemBySlot(uint slotnumber)
        {
            var slottedItem = Me.Inventory.GetItemBySlot(slotnumber);
            {
                Logger.SpecialLog("use - " + Me.Inventory.Equipped.GetItemBySlot(slotnumber));
                slottedItem.Use();
            }

        }        
        
        public static Boolean CanUseItem(int itemId) {           
            WoWItem item = GetItemById(itemId);          
            return item != null &&item.Usable &&  item.Cooldown == 0 && item.ItemInfo.RequiredLevel <= StyxWoW.Me.Level;
        }

        public static Boolean CanUseSlottedItem(uint slotnumber)
        {
            var slottedItem = Me.Inventory.GetItemBySlot(slotnumber);
            return slottedItem != null && slottedItem.Usable && slottedItem.Cooldown == 0 && slottedItem.ItemInfo.RequiredLevel <= StyxWoW.Me.Level;
        }

        public static bool IsHymning() {
            if (Me.ChanneledCastingSpellId == 64901 || Me.ChanneledCastingSpellId == 64843 || Me.ChanneledCastingSpellId == 605) { return true; }
            return false;
        }


        public static WoWItem GetItemById(int itemId) {
            return Me.CarriedItems.FirstOrDefault(i => i.Entry == itemId);
        }



        public static bool CanCast(string spellName, WoWUnit onUnit)
        {

            if (onUnit == null) { return false;}

            //trying to prevent vt from doublecasting....
            
            
            // Do we have spell?
            if (!SpellManager.Spells.ContainsKey(spellName))
            {
                return false;
            }

            WoWSpell spell = SpellManager.Spells[spellName];

            if (spellName == "Vampiric Touch" && spell.Cooldown ) {
                return false;
            }

           
            // are we casting or channeling ?
            if (Me.ChanneledCastingSpellId == 15407 && spellName != "Vampiric Touch" &&
                AuraManager.GetAuraTimeLeft("Mind Flay", onUnit, true).TotalSeconds >= AltarboySettings.Instance.ClippingDuration)
            {
                return false;
            }

            if (Priest.ActiveSpec == TalentSpec.DisciplinePriest && (Me.IsCasting || Me.ChanneledCastingSpellId != 0))
            {
                return false;
            }


            if (SpellManager.GlobalCooldownLeft.TotalSeconds > AltarboySettings.Instance.CoolDownDuration)
           {
               return false;
           }

            // is spell in CD?
            if (spell.CooldownTimeLeft.TotalSeconds >= AltarboySettings.Instance.CoolDownDuration )
            {
                return false;
            }

            // minrange check
            if (spell.MinRange != 0 && onUnit.DistanceSqr < spell.MinRange * spell.MinRange)
            {
                return false;
            }                     
            
            // do we have enough power?
            if (Me.GetCurrentPower(spell.PowerType) < spell.PowerCost)
            {
                return false;
            }
                 

            return true;
        }


     }


    

}
