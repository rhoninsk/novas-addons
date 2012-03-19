using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.BehaviorTree;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;
using CommonBehaviors.Actions;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Dunatanks3
{
    public partial class Warrior
    {

        protected Composite CreateBossBehavior()
        {


            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.CurrentTarget == null || StyxWoW.Me.CurrentTarget.Dead || !StyxWoW.Me.CurrentTarget.Attackable || Me.Dead || StyxWoW.Me.Mounted,
                new ActionIdle()),

                    new Decorator(ret => ((DunatanksSettings.Instance.ArmsuseHealthStone && Me.HealthPercent <= DunatanksSettings.Instance.ArmsHealthStonePercent) || (DunatanksSettings.Instance.FuryuseHealthStone && Me.HealthPercent <= DunatanksSettings.Instance.FuryHealthStonePercent) || (DunatanksSettings.Instance.ProtuseHealthStone && Me.HealthPercent <= DunatanksSettings.Instance.ProtHealthStonePercent)) && HaveHealthStone() && HealthStoneNotCooldown(),
                              new Action(ret => UseHealthStone())),
                    new Decorator(ret => ((DunatanksSettings.Instance.ArmsusePotion && Me.HealthPercent <= DunatanksSettings.Instance.ArmsPotionPercent) || (DunatanksSettings.Instance.FuryusePotion && Me.HealthPercent <= DunatanksSettings.Instance.FuryPotionPercent) || (DunatanksSettings.Instance.ProtusePotion && Me.HealthPercent <= DunatanksSettings.Instance.ProtPotionPercent)) && HaveHealthPotion() && HealthPotionReady(),
                              new Action(ret => UseHealthPotion())),
                              MoveToTargetProper(),
                              CreateAutoAttack(),
                              CreateSpellCheckAndCast("Intercept", ret => DunatanksSettings.Instance.useInterceptInterruptPvP && Me.CurrentTarget.IsCasting && Me.CurrentTarget.Distance > 8 && Me.CurrentTarget.Distance < 25 && Me.IsFacing(Me.CurrentTarget) && Styx.Logic.Battlegrounds.IsInsideBattleground),
                              CreateSpellCheckAndCast("Intercept", ret => DunatanksSettings.Instance.useInterceptApproachPvP && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.Distance < 25 && Me.IsFacing(Me.CurrentTarget) && Styx.Logic.Battlegrounds.IsInsideBattleground),
                              FaceTarget(ret => !DunatanksSettings.Instance.DisableMovement),
                             UseTrinketOne(),
                             UseTrinketTwo(),
              // adv cfg
                // BD specific
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5094 && DunatanksSettings.Instance.arcanotronsn == true && (Me.CurrentTarget.Name == "Arcanotron") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Shadow Nova")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5094 && DunatanksSettings.Instance.arcanotronpummelaa == true && (Me.CurrentTarget.Name == "Arcanotron") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Arcane Annihilator")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 5094 && DunatanksSettings.Instance.arcanotronreflaa == true && (Me.CurrentTarget.Name == "Arcanotron") && Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Arcane Annihilator")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5094 && DunatanksSettings.Instance.maloriakas == true && (Me.CurrentTarget.Name == "Maloriak") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Arcane Storm")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5094 && DunatanksSettings.Instance.nefarianbn == true && (Me.CurrentTarget.Name == "Chromatic Prototype") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Blast Nova")),
                // FL specific
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5723 && DunatanksSettings.Instance.alysrazorf == true && (Me.CurrentTarget.Name == "Blazing Talon Initiate") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Fieroblast")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5723 && DunatanksSettings.Instance.alysrazori == true && (Me.CurrentTarget.Name == "Blazing Talon Clawshaper") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Ignition")),
                // BoT specific
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5334 && DunatanksSettings.Instance.halfussn == true && (Me.CurrentTarget.Name == "Halfus Wyrmbreaker") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Shadow Nova")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5334 && DunatanksSettings.Instance.felupummelhl == true && (Me.CurrentTarget.Name == "Feludius") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Hydro Lance")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 5334 && DunatanksSettings.Instance.felureflhl == true && (Me.CurrentTarget.Name == "Feludius") && Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Hydro Lance")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5334 && DunatanksSettings.Instance.choconversion == true && (Me.CurrentTarget.Name == "Cho'Gall") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Conversion")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5334 && DunatanksSettings.Instance.chotwisteddevotion == true && (Me.CurrentTarget.Name == "Cho'Gall") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Twisted Devotion")),
                // TotFW
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5638 && DunatanksSettings.Instance.totfwtrash == true && StyxWoW.Me.CurrentTarget.IsCasting && detectAdds().Count > 1),
                // ZA specific
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.chainhealtrash == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Chain Heal")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.malahl == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && (Me.CurrentTarget.CastingSpell.Name == "Holy Light")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.malafh == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && (Me.CurrentTarget.CastingSpell.Name == "Flash Heal")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.malahw == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && (Me.CurrentTarget.CastingSpell.Name == "Healing Wave")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 3805 && DunatanksSettings.Instance.malareflfb == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && Me.CurrentTarget.IsTargetingMeOrPet && (Me.CurrentTarget.CastingSpell.Name == "Frostbolt")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.malapummelfb == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && !Me.CurrentTarget.IsTargetingMeOrPet && (Me.CurrentTarget.CastingSpell.Name == "Frostbolt")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 3805 && DunatanksSettings.Instance.malareflcl == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && Me.CurrentTarget.IsTargetingMeOrPet && (Me.CurrentTarget.CastingSpell.Name == "Chain Lightning")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 3805 && DunatanksSettings.Instance.malapummelcl == true && StyxWoW.Me.CurrentTarget.IsCasting && (Me.CurrentTarget.Name == "Hex Lord Malacrass") && !Me.CurrentTarget.IsTargetingMeOrPet && (Me.CurrentTarget.CastingSpell.Name == "Chain Lightning")),
                // ZG specific
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 1977 && DunatanksSettings.Instance.hpvwhisper == true && (Me.CurrentTarget.Name == "High Priest Venoxis") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Whispers of Hethiss")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 1977 && DunatanksSettings.Instance.hazzapummelwrath == true && (Me.CurrentTarget.Name == "Hazza'rah") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Wrath")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 1977 && DunatanksSettings.Instance.hazzareflwrath == true && (Me.CurrentTarget.Name == "Hazza'rah") && Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Wrath")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 1977 && DunatanksSettings.Instance.hpktob == true && (Me.CurrentTarget.Name == "High Priestess Kilnara") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Tears of Blood")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 1977 && DunatanksSettings.Instance.hpkpummelsb == true && (Me.CurrentTarget.Name == "High Priestess Kilnara") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Shadow Bolt")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 1977 && DunatanksSettings.Instance.hpkreflsb == true && (Me.CurrentTarget.Name == "High Priestess Kilnara") && Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Shadow Bolt")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 1977 && DunatanksSettings.Instance.zanzilpummelvb == true && (Me.CurrentTarget.Name == "Zanzil") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Voodoo Bolt")),
                CreateSpellCheckAndCast("Spell Reflection", ret => DunatanksSettings.Instance.useProt == true && Me.ZoneId == 1977 && DunatanksSettings.Instance.zanzilreflvb == true && (Me.CurrentTarget.Name == "Zanzil") && Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Voodoo Bolt")),
                // HoT
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5789 && DunatanksSettings.Instance.tyrandes == true && (Me.CurrentTarget.Name == "Echo of Tyrandes") && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Stardust")),
                CreateSpellCheckAndCast("Pummel", ret => Me.ZoneId == 5788 && DunatanksSettings.Instance.queento == true && (Me.CurrentTarget.Name == "Queen Azshara") && !Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting && (Me.CurrentTarget.CastingSpell.Name == "Total Obedience")),

                 CreateEnsureTarget(),                 

                 // LOW LEVEL ROTATION START

                 // Rotation 1-9
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level < 10 && Me.HasAura("Victorious")),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 10 && !Me.CurrentTarget.HasAura("Rend")),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 10 && !Me.CurrentTarget.HasAura("Thunder Clap")),
CreateSpellCheckAndCast("Strike", ret => Me.Level < 10),
// Rotations 10-19
// Arms 10-19
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 10 && Me.Level <= 19 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useArms == true),
// Fury 10-19
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 10 && Me.Level <= 19 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useFury == true),
// Prot 10-19
// Change stance up to lvl 84
CreateSpellCheckAndCast("Defensive Stance", ret => Me.Level > 10 && Me.Level <= 84 && !Me.HasAura("Defensive Stance") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 10 && Me.Level <= 19 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 10 && Me.Level <= 19 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 10 && Me.Level <= 19 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 10 && Me.Level <= 19 && DunatanksSettings.Instance.useProt == true),
// Rotations 20-29
// Arms 20-29
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 20 && Me.Level <= 29 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
// Fury 20-29
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 20 && Me.Level <= 29 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
// Prot 20-29
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 20 && Me.Level <= 29 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 20 && Me.Level <= 29 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 20 && Me.Level <= 29 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 20 && Me.Level <= 29 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 20 && Me.Level <= 29 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 30-39
// No need to check for specc or max level
CreateSpellCheckAndCast("Pummel", ret => Me.Level >= 30 && Me.Level <= 84 && Me.CurrentTarget.CanInterruptCurrentSpellCast && !DunatanksSettings.Instance.usePummelAtEndArms && !DunatanksSettings.Instance.usePummelAtEndFury && !DunatanksSettings.Instance.usePummelAtEndProt && Me.CurrentTarget.IsCasting == true),
// Arms 30-39
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 30 && Me.Level <= 39 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 30 && detectAdds().Count > 2 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 30 && Me.Level <= 84 && Me.Level < 39 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
// Fury 30-39
// Stance
CreateSpellCheckAndCast("Berserker Stance", ret => Me.Level > 10 && Me.Level <= 84 && !Me.HasAura("Berserker Stance") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 30 && Me.Level <= 39 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentTarget.HealthPercent < 30 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useFury == true),
// Prot 30-39
// Last Stand, just check minimum level
CreateSpellCheckAndCast("Last Stand", ret => Me.Level >= 30 && Me.Level <= 84 && Me.HealthPercent < 20),
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 30 && Me.Level <= 39 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 30 && Me.Level <= 39 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 30 && Me.Level <= 39 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 30 && Me.Level <= 39 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 30 && Me.Level <= 39 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 40-49
// Arms 40-49
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 40 && Me.Level <= 49 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 40 && Me.Level <= 84 && detectAdds().Count > 1),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
// Fury 40-49
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 40 && Me.Level <= 49 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentTarget.HealthPercent < 40 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Raging Blow", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useFury == true),
// Prot 40-49
// Shield Wall, just check once
CreateSpellCheckAndCast("Shield Wall", ret => Me.Level >= 40 && Me.Level <= 84 && Me.HealthPercent < 20),
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 40 && Me.Level <= 49 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 40 && Me.Level <= 49 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 40 && Me.Level <= 49 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 40 && Me.Level <= 49 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 40 && Me.Level <= 49 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 50-59
// Arms 50-59
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 50 && Me.Level <= 59 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 50 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useArms == true),
// Fury 50-59
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 50 && Me.Level <= 59 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentTarget.HealthPercent < 50 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Raging Blow", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useFury == true),
// Prot 50-59
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 50 && Me.Level <= 59 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 50 && Me.Level <= 59 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Revenge", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Devastate", ret => Me.Level >= 50 && Me.Level <= 59 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 50 && Me.Level <= 59 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 50 && Me.Level <= 59 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 60-69
// Arms 60-69
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 60 && Me.Level <= 69 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Throwdown", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentTarget.IsCasting && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useArms == true),
// Fury 60-69
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 60 && Me.Level <= 69 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentTarget.HealthPercent < 60 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Slam", ret => Me.Level >= 60 && Me.Level <= 69 && Bloodsurge != null && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Raging Blow", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useFury == true),
// Prot 60-69
// Spell reflect, just check once
CreateSpellCheckAndCast("Spell Reflection", ret => Me.Level >= 60 && Me.Level <= 84 && Me.CurrentTarget.IsCasting == true && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 60 && Me.Level <= 69 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 60 && Me.Level <= 69 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Revenge", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Devastate", ret => Me.Level >= 60 && Me.Level <= 69 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 60 && Me.Level <= 69 && Me.CurrentRage >= 60 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 60 && Me.Level <= 69 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 70-79
// Arms 70-79
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 70 && Me.Level <= 79 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Throwdown", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentTarget.IsCasting && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 70 && Me.Level <= 84 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Bladestorm", ret => Me.Level >= 70 && detectAdds().Count > 2 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 70 && Me.Level <= 84 && Me.Level < 79 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useArms == true),
// Fury 70-79
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 70 && Me.Level <= 79 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentTarget.HealthPercent < 70 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Slam", ret => Me.Level >= 70 && Me.Level <= 79 && Bloodsurge != null && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Raging Blow", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useFury == true),
// Prot 70-79
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 70 && Me.Level <= 79 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 70 && Me.Level <= 79 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Revenge", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shockwave", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Devastate", ret => Me.Level >= 70 && Me.Level <= 79 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 70 && Me.Level <= 79 && Me.CurrentRage >= 70 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 70 && Me.Level <= 79 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),
// Rotations 80-84
// Arms 80-84
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 80 && Me.Level <= 84 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Throwdown", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentTarget.IsCasting && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentTarget.HealthPercent < 20 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Sweeping Strikes", ret => Me.Level >= 80 && Me.Level <= 84 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Bladestorm", ret => Me.Level >= 80 && detectAdds().Count > 2 && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useBladestormArmsAoE == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 80 && Me.Level <= 84 && Me.Level < 84 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Colossus Smash", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Mortal Strike", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && detectAdds().Count > 1 && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useArms == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useArms == true),
// Fury 80-84
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 80 && Me.Level <= 84 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Execute", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentTarget.HealthPercent < 80 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Death Wish", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Recklessness", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Bloodthirst", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Slam", ret => Me.Level >= 80 && Me.Level <= 84 && Bloodsurge != null && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Whirlwind", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Raging Blow", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && detectAdds().Count > 1 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Thunder Clap") && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Strike", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useFury == true),
CreateSpellCheckAndCast("Demoralizing Shout", ret => Me.Level >= 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Demoralizing Shout") && DunatanksSettings.Instance.useFury == true),
// Prot 80-84
CreateSpellCheckAndCast("Victory Rush", ret => Me.Level >= 80 && Me.Level <= 84 && Me.HasAura("Victorious") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Block", ret => Me.Level >= 80 && Me.Level <= 84 && detectAdds().Count > 2 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Overpower", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shield Slam", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Revenge", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Shockwave", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Devastate", ret => Me.Level >= 80 && Me.Level <= 84 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Heroic Strike", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Cleave", ret => Me.Level >= 80 && Me.Level <= 84 && Me.CurrentRage >= 80 && detectAdds().Count > 1 && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Rend", ret => Me.Level < 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true),
CreateSpellCheckAndCast("Thunder Clap", ret => Me.Level < 80 && Me.Level <= 84 && !Me.CurrentTarget.HasAura("Thunder Clap") && Me.CurrentTarget.HasAura("Rend") && DunatanksSettings.Instance.useProt == true && detectAdds().Count > 1),

// END OF LOW LEVEL ROTATION


                 AoEPummel(),


                 // PvP Arms
CastHamstringPvP(),
                // Pummel
CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.usePvPRota && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.usePummelFuryBoss && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentTarget.IsCasting && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),
                // Victory Rush
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Victory Rush"),
CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                // Overpower Miss Check
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && SpellManager.Spells["Overpower"].CooldownTimeLeft.Milliseconds < 700, "Battle Stance", "Overpower"),
CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Battle Stance")),
// Recklessness (on CD)
CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessArmsOnCd),
                // Recklessness (below 20%)
CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessFuryBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),
                // Rend
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && (DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && (!StyxWoW.Me.CurrentTarget.HasAura("Rend"))), "Battle Stance", "Rend"),
CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.usePvPRota && (DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && (!StyxWoW.Me.CurrentTarget.HasAura("Rend"))) && Me.HasAura("Battle Stance")),
                // CS
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Colossus Smash"),
CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                // Overpower
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Battle Stance", "Overpower"),
CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Battle Stance")),
                // MS
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Mortal Strike"),
CreateSpellCheckAndCast("Mortal Strike", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                // Execute
CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms && DunatanksSettings.Instance.usePvPRota && Me.CurrentTarget.HealthPercent < 20 && Me.CurrentRage > 60),
CastSequenceSimple(ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 50 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Heroic Strike"),
CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 50 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
  
// PvP Fury
// == Fury TG rota + pvp abilities
CreateSpellCheckAndCast("Piercing Howl", ret=> DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury && DunatanksSettings.Instance.UsePiercingHowlPvP && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.Distance <= 15 && !Me.CurrentTarget.HasAura("Piercing Howl")),
CastHamstringPvP(),
// Pummel
CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.usePvPRota && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.usePummelFuryBoss && StyxWoW.Me.CurrentTarget.IsCasting && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),
// Victory Rush
CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useVictoryRushFuryBoss && StyxWoW.Me.HasAura("Victorious")),
// Death Wish (on CD)
CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && StyxWoW.Me.CurrentRage > 60 && DunatanksSettings.Instance.useDeathWishOnCd),
// Death Wish (below 20%)
CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useDeathWishBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20),
// Recklessness (on CD)
CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useRecklessnessFuryOnCd && StyxWoW.Me.HasAura("Death Wish")),
// Recklessness (below 20%)
CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useRecklessnessFuryBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20),
CastCSonCD(),
CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && Bloodsurge != null && StyxWoW.Me.CurrentTarget.HealthPercent >= 20),
// Bloodthirst
CastBloodThirst(),
// Heroic Strike
CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && ((StyxWoW.Me.CurrentRage > 65 && wtColossusSmash.TimeLeft.TotalSeconds > 5) || (wtColossusSmash.TimeLeft.TotalSeconds < 6 && StyxWoW.Me.CurrentRage > 85))),
// Raging Blow
CreateSpellCheckAndCast("Raging Blow", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true),
// Execute
CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && StyxWoW.Me.CurrentTarget.HealthPercent < 20),
// Demo Shout
CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useDemoShoutFuryBoss && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout")),

// Prot PvP

// Last Stand (Berserker Rage + Enraged Regen Pulse)
CreateBuffCheckAndCast("Last Stand", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HealthPercent <= DunatanksSettings.Instance.LastStandPercentBoss),
// Shield Wall (Frenzy or Enraged)
CreateBuffCheckAndCast("Shield Wall", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && Me.HealthPercent < 40),
// Pummel
CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.usePvPRota && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.usePummelProtBoss == true && StyxWoW.Me.CurrentTarget.IsCasting && ((!DunatanksSettings.Instance.usePummelAtEndProt) || (DunatanksSettings.Instance.usePummelAtEndProt && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),
// Victory Rush
CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useVictoryRushProtBoss == true && StyxWoW.Me.HasAura("Victorious")),
// Shield Slam + Proc
CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HasAura("Sword and Board")),
// Heroic Strike (Rage Dump)
CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentRage >= 60),
// Shield Block
CreateBuffCheckAndCast("Shield Block", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true),
// Revenge
CreateSpellCheckAndCast("Revenge", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true),
// Devastate 3x
CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && (!StyxWoW.Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.Auras["Sunder Armor"].StackCount < 3)),
// Rend
CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend")),
// Thunder Clap
CreateSpellCheckAndCast("Thunder Clap", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Thunder Clap")),
// Demo Shout      
CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDemoShoutProtBoss && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout")),
// Shield Slam (without Proc)
CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true),
// Devastate
CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true),
// Shockwave
CreateSpellCheckAndCast("Shockwave", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true),
// Disarm
CreateSpellCheckAndCast("Disarm", ret => DunatanksSettings.Instance.usePvPRota && DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDisarmProtBoss && !StyxWoW.Me.CurrentTarget.HasAura("Disarm")),
                        


                    // Boss Tanking



                        // Last Stand (Berserker Rage + Enraged Regen Pulse)
                        CreateBuffCheckAndCast("Last Stand", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HealthPercent <= DunatanksSettings.Instance.LastStandPercentBoss && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
                    
                        // Shield Wall (Frenzy or Enraged)
                        CreateBuffCheckAndCast("Shield Wall", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentTarget.HasAura("Enrage") || StyxWoW.Me.CurrentTarget.HasAura("Frenzy") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
                    
                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useProt == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelProtBoss == true && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndProt) || (DunatanksSettings.Instance.usePummelAtEndProt && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useVictoryRushProtBoss == true && StyxWoW.Me.HasAura("Victorious") &&  (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
                    
                        // Shield Slam + Proc
                        CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HasAura("Sword and Board") && detectAdds().Count ==1),

                        // Heroic Strike (Rage Dump)
                        CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentRage >= 60  && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Block
                        CreateBuffCheckAndCast("Shield Block", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Revenge
                        CreateSpellCheckAndCast("Revenge", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Devastate 3x
                        CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.useProt == true && (!StyxWoW.Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.Auras["Sunder Armor"].StackCount <3) && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Rend
                        CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Thunder Clap
                        CreateSpellCheckAndCast("Thunder Clap", ret => DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Thunder Clap") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Demo Shout      
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDemoShoutProtBoss && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Slam (without Proc)
                        CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Devastate
                        CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shockwave
                        CreateSpellCheckAndCast("Shockwave", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Disarm
                        CreateSpellCheckAndCast("Disarm", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDisarmProtBoss && !StyxWoW.Me.CurrentTarget.HasAura("Disarm") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
                        
                

                    // Group-Tanking
                        CreateEnsureTarget(),
                        // Taunt
                        CreateSpellCheckAndCast("Taunt", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useTauntProtAoE && !StyxWoW.Me.CurrentTarget.Aggro && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Wall
                        CreateBuffCheckAndCast("Shield Wall", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useShieldWallProtAoE && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Last Stand (Berserker Rage + Enraged Regen Pulse)
                        CreateBuffCheckAndCast("Last Stand", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HealthPercent <= DunatanksSettings.Instance.LastStandPercentAoE && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
 
                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useProt == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelProtAoE == true && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndProt) || (DunatanksSettings.Instance.usePummelAtEndProt && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useVictoryRushProtAoE && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Thunder Clap
                        CreateSpellCheckAndCast("Thunder Clap", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentTarget.HasAura("Rend") && !StyxWoW.Me.CurrentTarget.HasAura("Thunder Clap") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)), 

                        // Shockwave
                        CreateSpellCheckAndCast("Shockwave", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Cleave + (Rage Dump)
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentRage >= 60 && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Rend
                        CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Slam + Proc
                        CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.HasAura("Sword and Board") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Block
                        CreateBuffCheckAndCast("Shield Block", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Disarm
                        CreateSpellCheckAndCast("Disarm", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDisarmProtAoE && !StyxWoW.Me.CurrentTarget.HasAura("Disarm") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Devastate 3x
                        CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.useProt == true && (!StyxWoW.Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.Auras["Sunder Armor"].StackCount <3) && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)), 

                        // Revenge
                        CreateSpellCheckAndCast("Revenge", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useDemoShoutProtAoE && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Devastate
                        CreateSpellCheckAndCast("Devastate", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

                        // Shield Slam without Proc
                        CreateSpellCheckAndCast("Shield Slam", ret => DunatanksSettings.Instance.useProt == true && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground)),



                    // Mega AoE Tanking

                        // Taunt
                        CreateSpellCheckAndCast("Taunt", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useTauntProtAoE && !StyxWoW.Me.CurrentTarget.Aggro && detectAdds().Count >= 6),

                        // AoE Taunt
                        CreateSpellCheckAndCast("Challenging Shout", ret => DunatanksSettings.Instance.useProt == true && DunatanksSettings.Instance.useChallengingShoutProtAoE && detectAdds().Count >= 6),

                        // Thunder Clap
                        CreateSpellCheckAndCast("Thunder Clap", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentTarget.HasAura("Rend") && detectAdds().Count >= 6),

                        // Cleave (Rage Dump)
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useProt == true && StyxWoW.Me.CurrentRage >= 60 && detectAdds().Count >= 6),

                        // Rend
                        CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && detectAdds().Count >= 6),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useProt == true && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && detectAdds().Count >= 6),

                        // Shockwave
                        CreateSpellCheckAndCast("Shockwave", ret => DunatanksSettings.Instance.useProt == true && detectAdds().Count >= 6),



                                        // Arms Boss Rotation



                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useArms == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelArmsBoss == true && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndArms) || (DunatanksSettings.Instance.usePummelAtEndArms && Me.CurrentTarget.CurrentCastTimeLeft.TotalMilliseconds < 350))),

						// Execute 5x in Bersistance DARF NICHT auslaufen
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (!StyxWoW.Me.HasAura("Executioner") || StyxWoW.Me.Auras["Executioner"].StackCount < 5) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),
						
                        // Stack up Sunder Armor
                        CreateSpellCheckAndCast("Sunder Armor", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.StackUpSunderArms && (!Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.ActiveAuras["Sunder Armor"].StackCount < 2 || Me.CurrentTarget.ActiveAuras["Sunder Armor"].TimeLeft.Milliseconds < 600) && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Overpower in Battlestance MISS CHECK
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && SpellManager.Spells["Overpower"].CooldownTimeLeft.Milliseconds < 700, "Battle Stance", "Overpower"),
                        CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.ActiveAuras.ContainsKey("Battle Stance")),
                        

                        // Victory Rush in Bersi
                        //ArmsVRBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Victory Rush"),
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms)), 

                        // Recklessness (on CD) 
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessArmsOnCd && StyxWoW.Me.CurrentRage > 50 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Recklessness (Enrage)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessArmsEnrage && (StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage")) && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Recklessness (below 20%)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessArmsBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Deadly Calm (on CD)
                        CreateSpellCheckAndCast("Deadly Calm", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useDeadlyCalmArmsOnCd && StyxWoW.Me.HasAura("Deadly Calm") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Deadly Calm (below 20%)
                        CreateSpellCheckAndCast("Deadly Calm", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useDeadlyCalmArmsBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Deadly Calm (Enrage)
                        CreateSpellCheckAndCast("Deadly Calm", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useDeadlyCalmArmsEnrage && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

						// Rend in Battlestance DARF NICHT auslaufen!
                        //ArmsRendBattle(),
                        CastSequenceSimple(ret => (DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && (!StyxWoW.Me.CurrentTarget.HasAura("Rend"))), "Battle Stance", "Rend"),
                        CreateSpellCheckAndCast("Rend", ret => (DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && (!StyxWoW.Me.CurrentTarget.HasAura("Rend"))) && Me.HasAura("Battle Stance")),
                        //CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.useArms == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),
                        
						// Overpower in Battlestance
						//ArmsOPBattle(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && SpellManager.Spells["Overpower"].CooldownTimeLeft.Milliseconds < 700, "Battle Stance", "Overpower"),
                        CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.useArms == true && Me.ActiveAuras.ContainsKey("Taste for Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Battle Stance")),
                        //CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.useArms == true && Me.HasAura("Taste of Blood") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),
					
                        // Mortal Strike in Bersistance
                        //ArmsMSBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds < 300, "Berserker Stance", "Mortal Strike"),
                        CreateSpellCheckAndCast("Mortal Strike", ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Mortal Strike", ret => DunatanksSettings.Instance.useArms == true && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),
						
						// Colossus Smash in Bersistance
                        //ArmsCSCDBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Colossus Smash"),
                        CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashOnCdArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

                        // Colossus Smash in Bersistance
                        //ArmsCSPerBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashRageArms && Me.CurrentRage > DunatanksSettings.Instance.ColossusSmashPercentArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Colossus Smash"),
                        CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashRageArms && Me.CurrentRage > DunatanksSettings.Instance.ColossusSmashPercentArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseColossusSmashRageArms && Me.CurrentRage > DunatanksSettings.Instance.ColossusSmashPercentArms && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),

						// Slam in Bersistance
                        //ArmsSlamBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && !Me.ActiveAuras.ContainsKey("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Slam"),
                        CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useArms == true && !Me.ActiveAuras.ContainsKey("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useArms == true && (!Me.HasAura("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.Level > 84),
						
                        // Heroic Strike in Bersistance
                        //ArmsHSBersi(),
                        CastSequenceSimple(ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 50 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms), "Berserker Stance", "Heroic Strike"),
                        CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 50 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.HasAura("Berserker Stance")),
                        //CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 70 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms)),



                    // Arms AoE Rotation

                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useArms == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelArmsAoE && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count > 1 && detectAdds().Count < 6 || Styx.Logic.Battlegrounds.IsInsideBattleground) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndArms) || (DunatanksSettings.Instance.usePummelAtEndArms && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),

                        // Victory Rush in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsAoE && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground), "Berserker Stance", "Victory Rush"),
                        //CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useVictoryRushArmsAoE && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),                                          

                        // Recklessness
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useRecklessnessArmsAoE && StyxWoW.Me.CurrentRage > 70 && !DunatanksSettings.Instance.forceSingleTargetRotationArms && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
						
                        // Berserker Rage
                        CreateSpellCheckAndCast("Berserker Rage", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseBerserkerRageArmsAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
                        
                        // Bladestorm in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useBladestormArmsAoE && detectAdds().Count > 3, "Berserker Stance", "Bladestorm"),
                        //CreateSpellCheckAndCast("Bladestorm", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useBladestormArmsAoE && detectAdds().Count > 3),

                        // Sweeping Strikes in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useSweepingStrikesArmsAoE && StyxWoW.Me.CurrentRage > 50 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground), "Berserker Stance", "Sweeping Strikes"),
                        //CreateSpellCheckAndCast("Sweeping Strikes", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useSweepingStrikesArmsAoE && StyxWoW.Me.CurrentRage > 50 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

						// Inner Rage in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseInnerRageArmsAoE && !SpellManager.Spells["Cleave"].Cooldown && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground), "Berserker Stance", "Inner Rage"),
                        //CreateSpellCheckAndCast("Inner Rage", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.UseInnerRageArmsAoE && !SpellManager.Spells["Cleave"].Cooldown && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),

						// Cleave Prio in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.PrioCleaveArmsAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground), "Berserker Stance", "Cleave"),
                        //CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.PrioCleaveArmsAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),
						
                        // Rend in Battle
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Battle Stance", "Rend"),
                        //CreateSpellCheckAndCast("Rend", ret => DunatanksSettings.Instance.useArms == true && !StyxWoW.Me.CurrentTarget.HasAura("Rend") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms), 

						// Overpower in Battle
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && Me.HasAura("Taste of Blood") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Battle Stance", "Overpower"),
                        //CreateSpellCheckAndCast("Overpower", ret => DunatanksSettings.Instance.useArms == true && Me.HasAura("Taste of Blood") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms),

                        // Mortal Strike in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Berserker Stance", "Mortal Strike"),                        
                        //CreateSpellCheckAndCast("Mortal Strike", ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms),
						
                        // Colossus Smash in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Berserker Stance", "Colossus Smash"),
                        //CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useArms == true && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms),         

                        // Cleave in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 80 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Berserker Stance", "Cleave"),
                        //CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentRage >= 80 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms),

                        // Slam in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && (!Me.HasAura("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.Level > 84, "Berserker Stance", "Slam"),
                        //CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useArms == true && (!Me.HasAura("Taste for Blood") && SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.Milliseconds > 560 && Me.CurrentTarget.HealthPercent >= 20) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms) && Me.Level > 84),

                        // Execute in Bersi
                        CastSequence(ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms, "Berserker Stance", "Execute"),
                        //CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useArms == true && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationArms),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useArms == true && DunatanksSettings.Instance.useDemoShoutArmsAoE && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground)),



                    // Fury TG Boss Rotation

                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useFury == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelFuryBoss && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentTarget.IsCasting && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),

                        // Stack up Sunder Armor
                        CreateSpellCheckAndCast("Sunder Armor", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.StackUpSunderFury && (!Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.ActiveAuras["Sunder Armor"].StackCount < 2 || Me.CurrentTarget.ActiveAuras["Sunder Armor"].TimeLeft.Milliseconds < 600) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useVictoryRushFuryBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (on CD)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDeathWishOnCd && StyxWoW.Me.CurrentRage > 70 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (Enrage)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDeathWishEnrage && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (below 20%)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDeathWishBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (on CD)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useRecklessnessFuryOnCd && StyxWoW.Me.HasAura("Death Wish") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (Enrage)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useRecklessnessFuryEnrage && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (below 20%)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useRecklessnessFuryBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),


                        // Execute 5x
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && (!StyxWoW.Me.HasAura("Executioner") || StyxWoW.Me.Auras["Executioner"].StackCount < 5) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Bloodthirst
                        CastBloodThirst(),

                        // Colossus Smash
                        CastCSonCD(),
                        CastCSonRage(),

                        // Colossus Smash
                //CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationArms),

                        // Raging Blow
                        CreateSpellCheckAndCast("Raging Blow", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && wtBloodThirst.TimeLeft.TotalSeconds > 0.5),

                        // Raging Blow without Enrage
                        CastTwoThings(ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && wtBloodThirst.TimeLeft.TotalSeconds > 0.5 && SpellManager.Spells["Berserker Rage"].Cooldown == false && IsEnraged(Me) == false && SpellManager.Spells["Colossus Smash"].Cooldown == true && SpellManager.Spells["Raging Blow"].CooldownTimeLeft.Milliseconds < 300, "Berserker Rage", "Raging Blow"),

                        // Slam
                        CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && Bloodsurge != null && StyxWoW.Me.CurrentTarget.HealthPercent >= 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && Bloodsurge != null),

                        // Heroic Strike
                        CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && ((StyxWoW.Me.CurrentRage > 65 && wtColossusSmash.TimeLeft.TotalSeconds > 5) || (wtColossusSmash.TimeLeft.TotalSeconds < 6 && StyxWoW.Me.CurrentRage > 85))),

                        // Execute
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDemoShoutFuryBoss && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),


                    // Fury TG AoE Rotation

                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useFury == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelFuryAoE && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350)) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useVictoryRushFuryBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Death Wish
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDeathWishFuryAoE && StyxWoW.Me.CurrentRage > 70 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Recklessness
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useRecklessnessFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Berserker Rage
                        CreateSpellCheckAndCast("Berserker Rage", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.UseBerserkerRageFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Inner Rage
                        CreateSpellCheckAndCast("Inner Rage", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.UseInnerRageFuryAoE && !SpellManager.Spells["Cleave"].Cooldown && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Cleave Prio
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.PrioCleaveFuryAoE && Me.HasAura("Inner Rage") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Whirlwind
                        CreateSpellCheckAndCast("Whirlwind", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useWhirlWindFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Cleave
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentRage > 65 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Bloodthirst
                        CreateSpellCheckAndCast("Bloodthirst", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Colossus Smash
                        CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentRage > 40 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Raging Blow
                        CreateSpellCheckAndCast("Raging Blow", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Slam
                        CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && Bloodsurge != null && StyxWoW.Me.CurrentTarget.HealthPercent >= 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury && Bloodsurge != null),

                        // Execute
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useTG && DunatanksSettings.Instance.useDemoShoutFuryAoE && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),




                    // Fury SMF Boss Rotation

                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useFury == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelFuryBoss && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.IsCasting && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350))),

                        // Stack up Sunder Armor
                        CreateSpellCheckAndCast("Sunder Armor", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.StackUpSunderFury && (!Me.CurrentTarget.HasAura("Sunder Armor") || StyxWoW.Me.CurrentTarget.ActiveAuras["Sunder Armor"].StackCount < 2 || Me.CurrentTarget.ActiveAuras["Sunder Armor"].TimeLeft.Milliseconds < 600) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useVictoryRushFuryBoss && StyxWoW.Me.HasAura("Victorious") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (on CD)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDeathWishOnCd && StyxWoW.Me.CurrentRage > 70 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (Enrage)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDeathWishEnrage && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Death Wish (below 20%)
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDeathWishBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (on CD)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useRecklessnessFuryOnCd && StyxWoW.Me.HasAura("Death Wish") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (Enrage)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useRecklessnessFuryEnrage && StyxWoW.Me.CurrentTarget.HasAura("Frenzy") || StyxWoW.Me.CurrentTarget.HasAura("Enrage") && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Recklessness (below 20%)
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useRecklessnessFuryBelow20 && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Execute 5x
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && (!StyxWoW.Me.HasAura("Executioner") || StyxWoW.Me.Auras["Executioner"].StackCount < 5) && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Bloodthirst
                        CastBloodThirst(),

                        // Colossus Smash
                        CastCSonCD(),
                        CastCSonRage(),

                        // Colossus Smash
                //CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentRage > 40 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Slam 
                        CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && Bloodsurge != null && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && Bloodsurge != null),

                        // Raging Blow
                        CreateSpellCheckAndCast("Raging Blow", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.HealthPercent >= 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && wtBloodThirst.TimeLeft.TotalSeconds > 0.5),

                        // Raging Blow without Enrage
                        CastTwoThings(ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && wtBloodThirst.TimeLeft.TotalSeconds > 0.5 && SpellManager.Spells["Berserker Rage"].Cooldown == false && IsEnraged(Me) == false && SpellManager.Spells["Colossus Smash"].Cooldown == true && SpellManager.Spells["Raging Blow"].CooldownTimeLeft.Milliseconds < 300, "Berserker Rage", "Raging Blow"),

                        // Heroic Strike
                        CreateSpellCheckAndCast("Heroic Strike", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury) && ((StyxWoW.Me.CurrentRage > 65 && wtColossusSmash.TimeLeft.TotalSeconds > 5) || (wtColossusSmash.TimeLeft.TotalSeconds < 6 && StyxWoW.Me.CurrentRage > 85))),

                        // Execute
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDemoShoutFuryBoss && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && ((detectAdds().Count == 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) || DunatanksSettings.Instance.forceSingleTargetRotationFury)),



                    // Fury SMF AoE Rotation

                        // Pummel
                        CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.useFury == true && Me.CurrentTarget.CanInterruptCurrentSpellCast && DunatanksSettings.Instance.usePummelFuryAoE && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.IsCasting && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && StyxWoW.Me.CurrentTarget.Name != "Maloriak" && ((!DunatanksSettings.Instance.usePummelAtEndFury) || (DunatanksSettings.Instance.usePummelAtEndFury && Me.CurrentTarget.CurrentCastTimeLeft.Milliseconds < 350)) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Victory Rush
                        CreateSpellCheckAndCast("Victory Rush", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useVictoryRushFuryBoss && StyxWoW.Me.HasAura("Victorious") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Death Wish
                        CreateSpellCheckAndCast("Death Wish", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDeathWishFuryAoE && StyxWoW.Me.CurrentRage > 70 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Recklessness
                        CreateSpellCheckAndCast("Recklessness", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useRecklessnessFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Berserker Rage
                        CreateSpellCheckAndCast("Berserker Rage", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.UseBerserkerRageFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Inner Rage
                        CreateSpellCheckAndCast("Inner Rage", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.UseInnerRageFuryAoE && !SpellManager.Spells["Cleave"].Cooldown && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Cleave Prio
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.PrioCleaveFuryAoE && Me.HasAura("Inner Rage") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Whirlwind
                        CreateSpellCheckAndCast("Whirlwind", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useWhirlWindFuryAoE && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Cleave
                        CreateSpellCheckAndCast("Cleave", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentRage > 65 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Bloodthirst
                        CreateSpellCheckAndCast("Bloodthirst", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Colossus Smash
                        CreateSpellCheckAndCast("Colossus Smash", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentRage > 40 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Slam
                        CreateSpellCheckAndCast("Slam", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && Bloodsurge != null && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury && Bloodsurge != null),

                        // Raging Blow
                        CreateSpellCheckAndCast("Raging Blow", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.HealthPercent >= 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Execute
                        CreateSpellCheckAndCast("Execute", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && StyxWoW.Me.CurrentTarget.HealthPercent < 20 && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),

                        // Demo Shout
                        CreateSpellCheckAndCast("Demoralizing Shout", ret => DunatanksSettings.Instance.useFury == true && DunatanksSettings.Instance.useSMF && DunatanksSettings.Instance.useDemoShoutFuryAoE && !StyxWoW.Me.CurrentTarget.HasAura("Demoralizing Shout") && (detectAdds().Count > 1 || Styx.Logic.Battlegrounds.IsInsideBattleground) && !DunatanksSettings.Instance.forceSingleTargetRotationFury),
                 MoveToTargetProper()
   
            );

        }
    }

}