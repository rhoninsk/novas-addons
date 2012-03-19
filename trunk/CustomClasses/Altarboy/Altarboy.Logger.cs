using System;
using System.Linq;
using Styx.Helpers;
using System.Drawing;



namespace Altarboy
{
    public static class Logger
    {
             

        /// <summary>
        /// Use this for general cc logging.
        /// </summary>      
        public static void Log (string msg, Color color  )
        {          
            Logging.Write(color, msg);
        }
               
         public static void Log (string msg )
        {          
           Log(msg, Color.WhiteSmoke);
        }


         public static void SpecialLog(string msg)
         {
             Log(msg, Color.Violet);
         }

         /// <summary>
         /// This is meant to be used only for spell cast loggin.
         /// </summary>     
         public static void CastLog(string Spellname, string TargetName)
         {
             Logging.Write(GetSpellColor(Spellname), String.Format("[Altarboy] Casting {0}", Spellname, TargetName));
         }

         public static Color GetSpellColor(string SpellName)
         {             
            
             if (AltarboyHashes.PriestSpellTypes.Keys.Any(x=> x == SpellName))
             {
                 switch (AltarboyHashes.PriestSpellTypes[SpellName])
                 {

                     case AltarboyHashes.SpellTypes.DamageOverTime:
                         return Color.LightSteelBlue;

                     case AltarboyHashes.SpellTypes.Channeled:
                         return Color.LightCyan;

                     case AltarboyHashes.SpellTypes.DirectDamage:
                         return Color.PaleTurquoise;

                     case AltarboyHashes.SpellTypes.Utility:
                         return Color.Gainsboro;

                     case AltarboyHashes.SpellTypes.Cooldowns:
                         return Color.Plum;

                     case AltarboyHashes.SpellTypes.Healing:
                         return Color.MistyRose;

                     case AltarboyHashes.SpellTypes.Buff:
                         return Color.PaleGreen;

                     case AltarboyHashes.SpellTypes.Racial:
                         return Color.Thistle;

                     case AltarboyHashes.SpellTypes.Unknown:
                         return Color.White;

                     default:
                         return Color.White;
                 }
             }
             //should only returns this for "casting items" e.g. potions, trinkets, tinkers...
             return Color.Plum;            
         }

               
    }
}
