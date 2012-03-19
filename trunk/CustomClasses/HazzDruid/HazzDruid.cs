using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx;
using Styx.Logic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Drawing;

namespace HazzDruid
{
    class class2 : CombatRoutine
    {
        private WoWUnit lastCast = null;
        private WoWPlayer tank = null;
        private WoWPlayer lastTank = null;

        public override void Pulse()
        {
            if (!Me.IsInParty && !Me.IsInRaid)
            {
                Combat();
            }
            else if (Battlegrounds.IsInsideBattleground)
            {
                Combat();
            }
            else if (!CheckUnit(tank) || (CheckUnit(tank) && (tank.IsMe || !tank.InLineOfSight || tank.Distance2D > 40)))
            {
                tank = GetTank();

                if (tank == null) tank = Me;

                if (lastTank == null || lastTank.Guid != tank.Guid)
                {
                    lastTank = tank;
                }
                if (CheckUnit(tank) && tank.Combat)
                {
                    Combat();
                }
            }
        }

        public override void Initialize()
        {
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            HazzDruid.HazzDruidConfig f1 = new HazzDruid.HazzDruidConfig();
            f1.ShowDialog();
        }

        public override void Combat()
        {
            if (StyxWoW.GlobalCooldown)
                return;
            else if (SelfBuff())
                return;
            else if (CatForm())
                return;
            else if (BearForm())
                return;
            else if (Resting())
                return;
            else if (Revive())
                return;
            else if (Tranquility())
                return;
            else if (TreeForm())
                return;
            else if (Innervate())
                return;
            else if (Lifebloom())
                return;
            else if (Cleansing())
                return;
            else if (Healing())
                return;
            else if (Harmony())
                return;
            else if (Defense())
                return;
            else if (MoonkinDoT())
                return;
            else if (Mushrooms())
                return;
            else if (Moonkin())
                return;
            else if (MoonkinHeal())
                return;
            else if (Rebirth())
                return;
            else if (Buff())
                return;
            else if (Forms())
                return;
        }

        private bool Mounted()
        {
            if (Me.Mounted)
            {
                if (Me.IsInParty)
                {
                }
                if (Me.IsInRaid)
                {
                }
            }
            return false;
        }

        private bool CancelHeal()
        {
            if (Me.IsCasting && (lastCast != null && !lastCast.Dead && lastCast.HealthPercent >= 90))
            {
                lastCast = null;
                SpellManager.StopCasting();
                return true;
            }
            else if (Me.IsCasting)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckUnit(WoWUnit unit)
        {
            return unit != null && unit.IsValid && unit.IsAlive;
        }

        private void ReTarget(WoWUnit target)
        {
            if (CheckUnit(Me) && CheckUnit(target) && (!Me.GotTarget || Me.IsTargetingMeOrPet || Me.CurrentTarget.IsFriendly))
            {
                target.Target();
            }
        }

        private WoWPlayer GetTank()
        {
            foreach (WoWPlayer p in Me.PartyMembers)
            {
                if (IsTank(p))
                {
                    return p;
                }
            }
            return null;
        }

        private string DeUnicodify(string s)
        {

            StringBuilder sb = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            foreach (byte b in bytes)
            {
                if (b != 0)
                    sb.Append("\\" + b);
            }
            return sb.ToString();
        }

        private bool IsTank(WoWPlayer p)
        {
            return Lua.GetReturnValues("return UnitGroupRolesAssigned('" + DeUnicodify(p.Name) + "')").First() == "TANK";
        }

        private bool Forms()
        {
            if (HazzDruidSettings.Instance.UseTravel && Me.IsOutdoors && !Me.Mounted && !Me.Combat && !Me.Dead && !Me.HasAura("Travel Form"))
            {
                Logging.Write("Travel Form");
                C("Travel Form");
                return false;
            }
            if (HazzDruidSettings.Instance.UseTravel && Me.IsIndoors && !Me.Mounted && !Me.Combat && !Me.Dead && !Me.HasAura("Cat Form"))
            {
                Logging.Write("Cat Form");
                C("Cat Form");
                return false;
            }
            else
            {
                return false;
            }
        }

        private bool Innervate()
        {
            if (Me.ManaPercent < HazzDruidSettings.Instance.InnervatePercent && CC("Innervate"))
            {
                Logging.Write("Innervate");
                C("Innervate", Me);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Mushrooms()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecBalance)
                {
                    return false;
                }
                else if (!HazzDruidSettings.Instance.UseMushroom)
                {
                    return false;
                }
                else
                {
                    if (HazzDruidSettings.Instance.MushroomPercent == 0)
                    {
                        return false;
                    }
                    if (CC("Wild Mushroom: Detonate", Me) && HazzDruidSettings.Instance.MushroomPercent == 1)
                    {
                        Logging.Write("Wild Mushroom 1");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Detonate Mushrooms");
                        C("Wild Mushroom: Detonate", Me.CurrentTarget);
                        return true;
                    }
                    if (CC("Wild Mushroom: Detonate", Me) && HazzDruidSettings.Instance.MushroomPercent == 2)
                    {
                        Logging.Write("Wild Mushroom 1");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Wild Mushroom 2");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Detonate Mushrooms");
                        C("Wild Mushroom: Detonate", Me.CurrentTarget);
                        return true;
                    }
                    if (CC("Wild Mushroom: Detonate", Me) && HazzDruidSettings.Instance.MushroomPercent == 3)
                    {
                        Logging.Write("Wild Mushroom 1");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Wild Mushroom 2");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Wild Mushroom 3");
                        C("Wild Mushroom");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Thread.Sleep(1000);
                        Logging.Write("Detonate Mushrooms");
                        C("Wild Mushroom: Detonate", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool TreeForm()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                else
                {
                    if (FriendlyCount(50) >= 3 && HazzDruidSettings.Instance.UseTree && CC("Tree of Life"))
                    {
                        Logging.Write("Tree of Life");
                        C("Tree of Life", Me);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Tranquility()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                else
                {
                    if (FriendlyCount(40) >= 3 && HazzDruidSettings.Instance.UseTranquility && CC("Tranquility"))
                    {
                        Logging.Write("Tranquility");
                        C("Tranquility", Me); ;
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Healing()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                else
                {
                    if (!CheckUnit(Me))
                    {
                        return false;
                    }
                    if (tar.HealthPercent < HazzDruidSettings.Instance.SwiftmendPercent && CC("Swiftmend", tar) && isAuraActive("Rejuvenation", tar))
                    {
                        Logging.Write("Swiftmend");
                        C("Swiftmend", tar);
                        return true;
                    }
                    if (tar.HealthPercent < HazzDruidSettings.Instance.NaturesPercent && CC("Nature's Swiftness", tar))
                    {
                        Logging.Write("Nature's Swiftness");
                        C("Nature's Swiftness", tar);
                        return true;
                    }
                    if (tar.HealthPercent < HazzDruidSettings.Instance.RejuvenationPercent && !isAuraActive("Rejuvenation", tar))
                    {
                        Logging.Write("Rejuvenation");
                        C("Rejuvenation", tar);
                        return true;
                    }
                    if (tar.HealthPercent < HazzDruidSettings.Instance.WildGrowthPercent && CC("Wild Growth", tar) && !isAuraActive("Wild Growth", tar))
                    {
                        Logging.Write("Wild Growth");
                        C("Wild Growth", tar);
                        return true;
                    }
                    if (!Me.IsInRaid && tar.HealthPercent < HazzDruidSettings.Instance.RegrowthPercent && !isAuraActive("Regrowth", tar))
                    {
                        Logging.Write("Regrowth");
                        C("Regrowth", tar);
                        Thread.Sleep(1000);
                        return true;
                    }
                    if (Battlegrounds.IsInsideBattleground && tar.HealthPercent < HazzDruidSettings.Instance.RegrowthPercent && !isAuraActive("Regrowth", tar))
                    {
                        Logging.Write("Regrowth");
                        C("Regrowth", tar);
                        Thread.Sleep(1000);
                        return true;
                    }
                    if (tar.HealthPercent < HazzDruidSettings.Instance.HealingTouchPercent && CC("Nourish", tar) && Me.ActiveAuras.ContainsKey("Harmony"))
                    {
                        Logging.Write("Nourish");
                        C("Nourish", tar);
                        Thread.Sleep(3000);
                        Logging.Write("Healing Touch");
                        C("Healing Touch", tar);
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool MoonkinHeal()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecBalance)
                {
                    return false;
                }
                else
                {
                    if (HazzDruidSettings.Instance.UseMoonkinHeal && !Me.Combat && tar.HealthPercent < 90 && !isAuraActive("Rejuvenation", Me))
                    {
                        Logging.Write("Rejuvenation");
                        C("Rejuvenation", Me);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseMoonkinHeal && !Me.Combat && tar.HealthPercent < 70 && !isAuraActive("Regrowth", Me))
                    {
                        Logging.Write("Regrowth");
                        C("Regrowth", Me);
                        Thread.Sleep(1000);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseMoonkinHeal && !Me.Combat && tar.HealthPercent < 50)
                    {
                        Logging.Write("Healing Touch");
                        C("Healing Touch", Me);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Defense()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                else
                {
                    if (HazzDruidSettings.Instance.UseCombat && !isAuraActive("Insect Swarm", Me.CurrentTarget))
                    {
                        Logging.Write("Insect Swarm");
                        C("Insect Swarm", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseCombat && CC("Moonfire", Me.CurrentTarget) && !isAuraActive("Moonfire", Me.CurrentTarget))
                    {
                        Logging.Write("Moonfire");
                        C("Moonfire", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseCombat)
                    {
                        Logging.Write("Wrath");
                        C("Wrath", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool MoonkinDoT()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecBalance)
                {
                    return false;
                }
                else
                {
                    if (!Me.HasAura("Moonkin Form"))
                    {
                        Logging.Write("Moonkin Form");
                        C("Moonkin Form");
                    }
                    if (HazzDruidSettings.Instance.UseSwarm && !isAuraActive("Insect Swarm", Me.CurrentTarget))
                    {
                        Logging.Write("Insect Swarm");
                        C("Insect Swarm", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseCombat && CC("Moonfire", Me.CurrentTarget) && !isAuraActive("Moonfire", Me.CurrentTarget) && Me.HasAura("Eclipse (Lunar)"))
                    {
                        Logging.Write("Moonfire");
                        C("Moonfire", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseCombat && CC("Moonfire", Me.CurrentTarget) && !isAuraActive("Sunfire", Me.CurrentTarget) && Me.HasAura("Eclipse (Solar)"))
                    {
                        Logging.Write("Sunfire");
                        C("Moonfire", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseFoN && CC("Force of Nature", Me.CurrentTarget))
                    {
                        Logging.Write("Force of Nature");
                        C("Force of Nature");
                        LegacySpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseSolarBeam && CC("Solar Beam", Me.CurrentTarget))
                    {
                        Logging.Write("Solar Beam");
                        C("Solar Beam", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseStarsurge && CC("Starsurge", Me.CurrentTarget))
                    {
                        Logging.Write("Starsurge");
                        C("Starsurge", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Moonkin()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecBalance)
                {
                    return false;
                }
                else
                {
                    if (!Me.HasAura("Moonkin Form"))
                    {
                        Logging.Write("Moonkin Form");
                        C("Moonkin Form");
                    }
                    if (CC("Wrath", Me.CurrentTarget) && Me.HasAura("Eclipse (Solar)"))
                    {
                        Logging.Write("Wrath");
                        C("Wrath", Me.CurrentTarget);
                        return true;
                    }
                    if (Me.HasAura("Eclipse (Lunar)"))
                    {
                        Logging.Write("Starfire");
                        C("Starfire", Me.CurrentTarget);
                        return true;
                    }
                    if (CC("Starfire", Me.CurrentTarget) && Me.CurrentEclipse >= 1 && !Me.HasAura("Eclipse (Solar)"))
                    {
                        Logging.Write("Starfire");
                        C("Starfire", Me.CurrentTarget);
                        return true;
                    }
                    if (Me.CurrentEclipse <= 0 && !Me.HasAura("Eclipse (Lunar)"))
                    {
                        Logging.Write("Wrath");
                        C("Wrath", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool CatForm()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecFeral)
                {
                    return false;
                }
                else
                {
                    if (HazzDruidSettings.Instance.UseCat && !Me.HasAura("Cat Form"))
                    {
                        Logging.Write("Cat Form");
                        C("Cat Form");
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseSkullBash && CC("Skull Bash", Me.CurrentTarget) && Me.CurrentTarget.Distance2D > 8)
                    {
                        Logging.Write("Skull Bash");
                        C("Skull Bash", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseFaerieFire && CC("Faerie Fire (Feral)", Me.CurrentTarget) && !isAuraActive("Faerie Fire", Me.CurrentTarget))
                    {
                        Logging.Write("Faerie Fire (Feral)");
                        C("Faerie Fire (Feral)", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseMangle && CC("Mangle", Me.CurrentTarget) && !isAuraActive("Mangle", Me.CurrentTarget))
                    {
                        Logging.Write("Mangle");
                        C("Mangle", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseTigersFury && CC("Tiger's Fury", Me))
                    {
                        Logging.Write("Tiger's Fury");
                        C("Tiger's Fury", Me);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseBerserk && CC("Berserk", Me))
                    {
                        Logging.Write("Berserk");
                        C("Berserk", Me);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseShred && Me.BehindTarget && Me.HasAura("Omen of Clarity") && Me.HasAura("Primal Madness"))
                    {
                        Logging.Write("Shred");
                        C("Shred", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && Me.ComboPoints >= HazzDruidSettings.Instance.RipCount)
                    {
                        Logging.Write("Rip");
                        C("Rip", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseRake && !isAuraActive("Rake", Me.CurrentTarget))
                    {
                        Logging.Write("Rake");
                        C("Rake", Me.CurrentTarget);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && HazzDruidSettings.Instance.UseSavageRoar && CC("Savage Roar", Me) && !Me.HasAura("Savage Roar"))
                    {
                        Logging.Write("Savage Roar");
                        C("Savage Roar", Me);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseCat && Me.ComboPoints >= HazzDruidSettings.Instance.BiteCount)
                    {
                        Logging.Write("Ferocious Bite");
                        C("Ferocious Bite", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool BearForm()
        {
            adds = detectAdds();

            if (Me.CurrentTarget != null)
            {
                if (Me.CurrentTarget.IsFriendly)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.SpecFeral)
                {
                    return false;
                }
                else
                {
                    if (HazzDruidSettings.Instance.UseBear && !Me.HasAura("Bear Form"))
                    {
                        Logging.Write("Bear Form");
                        C("Bear Form");
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseBSkullBash && CC("Skull Bash", Me.CurrentTarget) && Me.CurrentTarget.Distance2D > 8)
                    {
                        Logging.Write("Skull Bash");
                        C("Skull Bash", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseSurvivalInstincts && CC("Survival Instincts", Me.CurrentTarget) && Me.HealthPercent <= 50)
                    {
                        Logging.Write("Survival Instincts");
                        C("Survival Instincts", Me);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseEnrage && CC("Enrage", Me) && Me.RagePercent <= 40)
                    {
                        Logging.Write("Enrage");
                        C("Enrage", Me);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseBMangle && CC("Mangle", Me.CurrentTarget) && !isAuraActive("Mangle", Me.CurrentTarget))
                    {
                        Logging.Write("Mangle");
                        C("Mangle", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseBBeserk && CC("Berserk", Me))
                    {
                        Logging.Write("Berserk");
                        C("Berserk", Me);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseDemoralizingRoar && CC("Demoralizing Roar", Me.CurrentTarget) && !isAuraActive("Demoralizing Roar", Me.CurrentTarget))
                    {
                        Logging.Write("Demoralizing Roar");
                        C("Demoralizing Roar", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseMaul && CC("Maul", Me.CurrentTarget) && Me.RagePercent >= 50)
                    {
                        Logging.Write("Maul");
                        C("Maul", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && CC("Lacerate", Me.CurrentTarget) && !isAuraActive("Lacerate", Me.CurrentTarget))
                    {
                        Logging.Write("Lacerate");
                        C("Lacerate", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && CC("Lacerate", Me.CurrentTarget) && StyxWoW.Me.CurrentTarget.Auras["Lacerate"].StackCount < HazzDruidSettings.Instance.LacerateCount)
                    {
                        Logging.Write("Lacerate");
                        C("Lacerate", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseSwipe && CC("Swipe", Me.CurrentTarget) && !isAuraActive("Swipe", Me.CurrentTarget))
                    {
                        Logging.Write("Swipe");
                        C("Swipe", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseThrash && CC("Thrash", Me.CurrentTarget) && !isAuraActive("Thrash", Me.CurrentTarget))
                    {
                        Logging.Write("Thrash");
                        C("Thrash", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UsePulverize && CC("Pulverize", Me.CurrentTarget) && !isAuraActive("Pulverize", Me.CurrentTarget))
                    {
                        Logging.Write("Pulverize");
                        C("Pulverize", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseBFaerieFire && !isAuraActive("Faerie Fire (Feral)", Me.CurrentTarget))
                    {
                        Logging.Write("Faerie Fire (Feral)");
                        C("Faerie Fire (Feral)", Me.CurrentTarget);
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseBear && HazzDruidSettings.Instance.UseBFaerieFire && StyxWoW.Me.CurrentTarget.Auras["Faerie Fire (Feral)"].StackCount < 3)
                    {
                        Logging.Write("Faerie Fire (Feral)");
                        C("Faerie Fire (Feral)", Me.CurrentTarget);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool SelfBuff()
        {
            WoWPlayer tar = Threat();

            if (tar != null)
            {
                if (HazzDruidSettings.Instance.UseBarkskin && CC("Barkskin") && !Me.HasAura("Barkskin"))
                {
                    Logging.Write("Barkskin");
                    C("Barkskin", Me);
                    return true;
                }
                if (!HazzDruidSettings.Instance.SpecFeral && HazzDruidSettings.Instance.UseThorns && CC("Thorns") && !Me.HasAura("Thorns"))
                {
                    Logging.Write("Thorns");
                    C("Thorns", Me);
                    return true;
                }
                if (HazzDruidSettings.Instance.UseThorns && CC("Typhoon"))
                {
                    Logging.Write("Typhoon");
                    C("Typhoon", tar);
                    return true;
                }
                if (HazzDruidSettings.Instance.UseGrasp && CC("Nature's Grasp") && !Me.HasAura("Nature's Grasp"))
                {
                    Logging.Write("Nature's Grasp");
                    C("Nature's Grasp", Me);
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool Lifebloom()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                if (!HazzDruidSettings.Instance.UseLifebloom)
                {
                    return false;
                }
                else
                {
                    String s = null;
                    bool needCast = false;
                    double hp = tar.HealthPercent;

                    if (!CheckUnit(Me))
                    {
                        return false;
                    }
                    if (HazzDruidSettings.Instance.UseLifebloom && tar.Guid == tank.Guid && tar.HealthPercent > 75 && CC("Lifebloom") && !isAuraActive("Lifebloom", tar))
                    {
                        Logging.Write("Lifebloom");
                        C("Lifebloom", tar);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseLifebloom && tar.Guid == tank.Guid && tar.HealthPercent > 75 && CC("Lifebloom") && isAuraActive("Lifebloom", tar) && tar.ActiveAuras["Lifebloom"].StackCount < 3)
                    {
                        Logging.Write("Lifebloom");
                        C("Lifebloom", tar);
                        return true;
                    }
                    if (HazzDruidSettings.Instance.UseLifebloom && tar.Guid == tank.Guid && CC("Lifebloom") && isAuraActive("Lifebloom", tar) && tar.Auras["Lifebloom"].TimeLeft.TotalSeconds < 3)
                    {
                        Logging.Write("Lifebloom");
                        C("Lifebloom", tar);
                        return true;
                    }
                    if (s != null && CC(s, tar))
                    {
                        if (!C(s, tar))
                        {
                        }
                        if (!needCast)
                        {
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private bool Harmony()
        {
            WoWPlayer tar = GetHealTarget();

            if (tar != null)
            {
                if (!HazzDruidSettings.Instance.SpecRestoration)
                {
                    return false;
                }
                else
                {
                    if (!CheckUnit(Me))
                    {
                        return false;
                    }
                    if (Battlegrounds.IsInsideBattleground && Me.ActiveAuras.ContainsKey("Harmony") && Me.ActiveAuras["Harmony"].TimeLeft.TotalSeconds < 2)
                    {
                        Logging.Write("Regrowth");
                        C("Regrowth", tar);
                        Thread.Sleep(1000);
                        return true;
                    }
                    else if (Me.ActiveAuras.ContainsKey("Harmony") && Me.ActiveAuras["Harmony"].TimeLeft.TotalSeconds < 3)
                    {
                        Logging.Write("Harmony Nourish");
                        C("Nourish", tar);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private bool CC(string spell, WoWUnit target)
        {
            if (!CheckUnit(Me) && !CheckUnit(target))
            {
                return false;
            }

            ReTarget(target);
            return SpellManager.CanCast(spell, target);
        }

        private bool CC(string spell)
        {
            if (!CheckUnit(Me))
            {
                return false;
            }

            return SpellManager.CanCast(spell);
        }

        private bool C(string spell, WoWUnit target)
        {
            if (Me.IsCasting)
                return false;
            if (SpellManager.Cast(spell, target))
            {
                lastCast = target;
                return true;
            }
            return false;
        }

        private bool C(string spell)
        {
            if (Me.IsCasting)
                return false;
            lastCast = null;
            return SpellManager.Cast(spell);
        }

        private bool Cleansing()
        {
            if (HazzDruidSettings.Instance.UseRemoveCurse)
            {
                WoWPlayer p = GetCleanseTarget();
                if (p != null)
                {
                    if (p.Distance2D > 40 || !p.InLineOfSight)
                    {
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.SpecRestoration && CC("Remove Corruption", p))
                    {
                        Logging.Write("Remove Corruption");
                        C("Remove Corruption", p);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            } return false;
        }

        private bool NeedsCleanse(WoWPlayer p)
        {
            foreach (WoWAura a in p.ActiveAuras.Values)
            {
                if (a.IsHarmful && Me.ManaPercent > 50)
                {
                    WoWDispelType t = a.Spell.DispelType;
                    if (t == WoWDispelType.Curse || t == WoWDispelType.Magic || t == WoWDispelType.Poison)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private WoWPlayer GetHealTarget()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
                    orderby unit.HealthPercent ascending
                    where unit.IsInMyPartyOrRaid || unit.IsMe
                    where !unit.Dead
                    where !unit.IsGhost
                    where unit.Distance < 40
                    where unit.InLineOfSight
                    where unit.HealthPercent < 99
                    select unit).FirstOrDefault();

        }

        private WoWPlayer GetCleanseTarget()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(false, true)
                    orderby unit.HealthPercent ascending
                    where unit.IsInMyPartyOrRaid || unit.IsMe
                    where !unit.Dead
                    where !unit.IsGhost
                    where unit.Distance2D < 40
                    where unit.InLineOfSight
                    where unit.HealthPercent > 75
                    where NeedsCleanse(unit)
                    select unit).FirstOrDefault();
        }

        private WoWPlayer Threat()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(false, true)
                    orderby unit.Distance2D ascending
                    where unit.IsAutoAttacking
                    where unit.Distance2D < 5
                    where unit.IsHostile
                    select unit).FirstOrDefault();
        }

        private IEnumerable<WoWPlayer> GetResurrectTargets()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                    orderby unit.Distance2D ascending
                    where unit.Dead
                    where unit.IsInMyPartyOrRaid
                    where !unit.IsGhost
                    where unit.Distance2D < 40
                    select unit);
        }

        private bool Rebirth()
        {
            foreach (WoWPlayer p in GetResurrectTargets())
            {
                if (Blacklist.Contains(p.Guid, true))
                {
                    continue;
                }
                else
                {
                    if (p.Distance2D > 40 || !p.InLineOfSight)
                    {
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseRebirth && CC("Rebirth", p))
                    {
                        Logging.Write("Rebirth" + p);
                        C("Rebirth", p);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private int FriendlyCount(int hp)
        {
            int count = 0;
            foreach (WoWPlayer p in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true))
            {
                if (p.IsFriendly
                    && p.IsAlive
                    && p.Distance2D < 40
                    && p.HealthPercent <= hp)
                {
                    count++;
                }
            }
            return count;
        }

        private bool Revive()
        {
            foreach (WoWPlayer p in GetResurrectTargets())
            {
                if (Blacklist.Contains(p.Guid, true))
                {
                    continue;
                }
                else
                {
                    if (p.Distance2D > 40 || !p.InLineOfSight)
                    {
                        return true;
                    }
                    else if (HazzDruidSettings.Instance.UseRevive && CC("Revive", p))
                    {
                        Logging.Write("Revive " + p);
                        C("Revive", p);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private bool Buff()
        {
            if (HazzDruidSettings.Instance.UseMoTW && !Me.IsGhost && !Me.Mounted && !isAuraActive("Mark of the Wild"))
            {
                Logging.Write("Mark of the Wild");
                C("Mark of the Wild", Me);
                return false;
            }
            foreach (WoWPlayer p in Me.PartyMembers)
            {
                if (p.Distance2D > 40 || p.Dead || p.IsGhost || p.InLineOfSight)
                    continue;
                else if (HazzDruidSettings.Instance.UseMoTW && !isAuraActive("Blessing of Kings", p) && !isAuraActive("Mark of the Wild", p))
                {
                    Logging.Write("Mark of the Wild");
                    C("Mark of the Wild", p);
                    return false;
                }
            }
            return false;
        }

        private List<WoWUnit> adds = new List<WoWUnit>();

        private List<WoWUnit> detectAdds()
        {
            List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Distance < 35.00 &&
                        (unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember || unit.IsTargetingMeOrPet) &&
                        unit.InLineOfSight &&
                        !unit.IsFriendly &&
                        !unit.IsPet &&
                        !Styx.Logic.Blacklist.Contains(unit.Guid));

            if (addList.Count > 2)
            {
                Logging.Write("Detected " + addList.Count.ToString() + " Switchting to AoE!");
            }
            return addList;
        }

        private bool isAuraActive(string name)
        {
            return isAuraActive(name, Me);
        }

        private bool isAuraActive(string name, WoWUnit u)
        {
            return u.ActiveAuras.ContainsKey(name);
        }

        public override sealed string Name { get { return "HazzDruid EliT3.9.2"; } }

        public override WoWClass Class { get { return WoWClass.Druid; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        private bool Resting()
        {
            if (Me.HasAura("Drink"))
            {
                return true;
            }
            if (Me.HasAura("Eat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool NeedPullBuffs { get { Pulse(); return false; } }

        public override bool NeedCombatBuffs { get { Pulse(); return false; } }

        public override bool NeedPreCombatBuffs { get { Pulse(); return false; } }

    }
}