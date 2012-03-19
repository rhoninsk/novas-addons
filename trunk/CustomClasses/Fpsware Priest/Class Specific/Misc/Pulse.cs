using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using IDs = Athena.ClassHelpers.Priest.IDs;

namespace Athena
{
    public partial class Fpsware
    {
        // These buffs are OK to cast when you are IN COMBAT
        private void InCombatBuffs()
        {
            if (!Me.Combat) return;
            if (Me.IsFlying) return;
            if (Me.Mounted) return;

            if (Me.IsInParty && CLC.ResultOK(Settings.ShadowProtection) && Spell.CanCast("Shadow Protection"))
            {
                const string buffName = "Shadow Protection";
                WoWUnit target = RAF.PartyMemberWithoutBuff(buffName);
                if (target != null) Spell.Cast(buffName, target);
            }
            if (!Me.IsInParty && !Self.IsBuffOnMe("Shadow Protection") && CLC.ResultOK(Settings.ShadowProtection) && Spell.CanCast("Shadow Protection")) Spell.Cast("Shadow Protection", Me);


            // Fear Ward
            if (!Self.IsBuffOnMe("Fear Ward") && CLC.ResultOK(Settings.FearWard) && Spell.CanCast("Fear Ward")) Spell.Cast("Fear Ward", Me);


            // Class specific buffs
            //if (!Self.IsBuffOnMe(ClassHelpers.Druid.IDs.MarkOfTheWild, Self.AuraCheck.AllAuras) && Spell.CanCast("Mark of the Wild")) Spell.Cast("Mark of the Wild", Me);
            //if (!Self.IsBuffOnMe(IDs.MoltenCore, Self.AuraCheck.ActiveAuras) && Spell.CanCast("Power Word: Fortitude")) Spell.Cast("Power Word: Fortitude");
        }


        // All buffs check here. Only cast when you are OUT OF COMBAT
        private void OutOfCombatBuffs()
        {
            if (Me.Combat) return;
            if (Me.IsFlying) return;
            if (Me.Mounted) return;
            if (Self.IsBuffOnMe("Drink")) return;
            if (Self.IsBuffOnMe("Food")) return;
            if (Me.IsCasting) return;

            // Resurrection
            if (Settings.ResurrectPlayers.Contains("always") && Spell.CanCast("Resurrection"))
            {
                foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.Dead && !p.IsGhost && p.InLineOfSight))
                {
                    if (Timers.Exists(p.Guid.ToString()) && !Timers.Expired(p.Guid.ToString(), 15000)) continue;

                    Spell.Cast("Resurrection", p);
                    Utils.LagSleep();
                    Timers.Add(p.Guid.ToString());  // Prevent spamming resurrection on th same target
                    System.Threading.Thread.Sleep(1500);
                    if (!Me.IsCasting) Spell.StopCasting();
                    while (Me.IsCasting)
                    {
                        if (!p.Dead)
                        {
                            Utils.Log("-Emmm.... it appears our dead party member is now alive. So why are we still trying to rez them?");
                            Spell.StopCasting();
                        }
                    }
                    break;
                }
            }


            // Inner Fire or Inner Will
            if (!Self.IsBuffOnMe("Inner Fire") && Settings.InnerFireWill.Contains("Inner Fire") && Spell.CanCast("Inner Fire")) Spell.Cast("Inner Fire", Me);
            if (!Self.IsBuffOnMe("Inner Will") && Settings.InnerFireWill.Contains("Inner Will") && Spell.CanCast("Inner Will")) Spell.Cast("Inner Will", Me);

            // Power Word Fortitude
            if (Me.IsInParty && CLC.ResultOK(Settings.PowerWordFortitude) && Spell.CanCast("Power Word: Fortitude"))
            {
                const string buffName = "Power Word: Fortitude";
                WoWUnit target = RAF.PartyMemberWithoutBuff(buffName);
                if (target != null && !target.Auras.ContainsKey("Blood Pact")) Spell.Cast(buffName, target);
            }

            // Vampiric Embrace
            if (!Self.IsBuffOnMe("Vampiric Embrace") && Spell.CanCast("Vampiric Embrace")) Spell.Cast("Vampiric Embrace");


            // Class specific buffs
            //if (!Self.IsBuffOnMe(ClassHelpers.Druid.IDs.MarkOfTheWild, Self.AuraCheck.AllAuras) && Spell.CanCast("Mark of the Wild")) Spell.Cast("Mark of the Wild", Me);
        }


        // Anything else to happen in the PULSE goes here
        public static void MiscOtherPulse()
        {
            const string dispelMagic = "Dispel Magic";
            const string cureDisease = "Cure Disease";
            const string evangelism = "Evangelism";
            const string archangel = "Archangel";


            // Evangelism / Archangel 
            if (!Me.Combat && Timers.Exists("ArcSmiteCombat")) Timers.Reset("ArcSmiteCombat");

            // Archangel - Use it or loose it
            if (!Me.Mounted && !Me.IsFlying && (!Me.Auras.ContainsKey("Drink") || !Me.Auras.ContainsKey("Food")) && Me.ActiveAuras.ContainsKey("Evangelism") && !Me.Combat)
            {
                
                double getTime = Convert.ToDouble(Self.GetTimeLUA());
                double buffTime = Convert.ToDouble(Self.BuffTimeLeftLUA(evangelism));
                double secondsRemaining = buffTime - getTime;

                if (secondsRemaining < 4.5 && Spell.CanCastLUA(archangel))
                {
                    Utils.Log("-Evangelism buff about to expire. Casting Archangel buff to consume it", Utils.Colour("Red"));
                    Spell.Cast(archangel);
                }
            }
            



            // Dispel Magic - You and all party members);
            if ((!Settings.Cleanse.Contains("never") || !Settings.PartyCleanse.Contains("never")) && Spell.CanCast(dispelMagic))
            {
                List<int> urgentRemoval = new List<int> { 17173 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse) || CLC.ResultOK(Settings.PartyCleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Magic };

                    var p = ClassHelpers.Common.DecursePlayer(cureableList, CLC.ResultOK(Settings.PartyCleanse));
                    if (p != null)
                    {
                        if (Spell.CanCast(dispelMagic)) Spell.Cast(dispelMagic, p);
                    }
                }
            }

            // Cure Disease - You and all party members);
            if ((!Settings.Cleanse.Contains("never") || !Settings.PartyCleanse.Contains("never")) && Spell.CanCast(cureDisease))
            {
                List<int> urgentRemoval = new List<int> { 3427 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Disease };
                    var p = ClassHelpers.Common.DecursePlayer(cureableList, CLC.ResultOK(Settings.PartyCleanse));
                    if (p != null) { if (Spell.CanCast(cureDisease)) Spell.Cast(cureDisease, p); }
                }
            }



            // Clean up ressurection timers
            foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.IsAlive && Timers.Exists(p.Guid.ToString())))
            {
                Timers.Remove(p.Guid.ToString());
            }

        }
    }
}