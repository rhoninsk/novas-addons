using Styx.WoWInternals;
using TreeSharp;

namespace Athena
{
    public partial class Fpsware
    {
        private PrioritySelector CreateCombatBehavior()
        {
            return new PrioritySelector(

                // Check if we get aggro during the pull
                // This is in here and not the pull because we are in combat at this point
                new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                // Retarget
                new NeedToRetarget(new Retarget()),

                // Retarget Priority Based
                new NeedToRetargetPriorityBased(new RetargetPriorityBased()),

                // Dead Target Check
                new NeedToDeadTargetCheck(new DeadTargetCheck()),

                // LoS 
                new NeedToLoS(new LoS()),

                // Abort combat is the target's health is 95% + after 30 seconds of combat
                new NeedToCheckCombatTimer(new CheckCombatTimer()),

                // Make sure we always have a target. If we don't have a target, take the pet's target - if they have one.
                 new Decorator(ret => !Me.GotTarget && Me.GotAlivePet && Me.Pet.GotTarget, new Action(ret => Me.Pet.CurrentTarget.Target())),

                 // Auto Attack
                 new NeedToAutoAttack(new AutoAttack()),

                 // Shadowfiend
                 new NeedToShadowfiend(new Shadowfiend()),


                // Always Face Moving Target
                new NeedToAlwaysFaceMovingTarget(new AlwaysFaceMovingTarget()),


                new Switch<ClassHelpers.Priest.ClassType>(ret => ClassHelpers.Priest.ClassSpec,
                                                          new SwitchArgument<ClassHelpers.Priest.ClassType>(DisciplineCombat, ClassHelpers.Priest.ClassType.Discipline),
                                                          new SwitchArgument<ClassHelpers.Priest.ClassType>(ShadowCombat, ClassHelpers.Priest.ClassType.Shadow),
                                                          new SwitchArgument<ClassHelpers.Priest.ClassType>(DisciplineCombat, ClassHelpers.Priest.ClassType.None), 
                                                          new SwitchArgument<ClassHelpers.Priest.ClassType>(HolyCombat, ClassHelpers.Priest.ClassType.Holy)


                    ));
        }

        #region Combat Discipline
        private Composite DisciplineCombat
        {
            get
            {
                return new PrioritySelector(

                    // Face Target
                 new NeedToFaceTarget(new FaceTarget()),

                    // Shadow Word Death
                    new NeedToShadowWordDeath(new ShadowWordDeath()),

                    // Shadowfiend
                    new NeedToShadowfiend(new Shadowfiend()),

                    // Shackle
                    new NeedToShackle(new Shackle()),



                    // ************************************************************************************
                    // Other spells here

                    // Archangle
                    new NeedToArchangle(new Archangle()),

                    // Power Word: Barrier
                    new NeedToPowerWordBarrier(new PowerWordBarrier()),

                    // Power Infusion
                    new NeedToPowerInfusion(new PowerInfusion()),

                    // Holy Nova - AoE only
                    new NeedToHolyNova(new HolyNova()),

                    // Holy Fire
                    new NeedToHolyFire(new HolyFire()),

                    // Devouring Plague
                    new NeedToDevouringPlague(new DevouringPlague()),

                    // Shadow Word Pain
                    new NeedToShadowWordPain(new ShadowWordPain()),

                    // Psychic Scream
                    new NeedToPsychicScream(new PsychicScream()),

                    // Mind Sear
                    new NeedToMindSear(new MindSear()),

                    // Mind Blast
                    new NeedToMindBlast(new MindBlast()),

                    // Mind Spike
                    new NeedToMindSpike(new MindSpike()),

                    // Chastise
                    new NeedToChastise(new Chastise()),

                    // Penance
                    new NeedToPenance(new Penance()),

                    // Chakra
                    new NeedToChakra(new Chakra()),

                    // Smite
                    new NeedToSmite(new Smite()),

                    // Mind Flay
                    new NeedToMindFlay(new MindFlay()),

                    // Fade
                    new NeedToFade(new Fade()),

                    // Hymn of Hope
                    new NeedToHymnOfHope(new HymnOfHope()),

                    // Party Healer
                    new NeedToPartyHealer(new PartyHealer()),

                    // Party Decurse
                    new NeedToPartyDecurse(new PartyDecurse()),

                    // Bounce Prayer of Mending
                    new NeedToBouncePOM(new BouncePOM()),

                    // Heal Pets
                    new NeedToHealPets(new HealPets()),

                    // Smite Evangelism
                    new NeedToSmiteEvangelism(new SmiteEvangelism()),

                    // Wand Party
                    new NeedToWandParty(new WandParty()),

                    // Move To
                    new NeedToMoveTo(new MoveTo()),

                    // Finally just perform an update
                    new Action(ret => ObjectManager.Update())


                    );
            }
        }
        #endregion


        #region Combat Holy
        private Composite HolyCombat
        {
            get
            {
                return new PrioritySelector(

                    );
            }
        }
        #endregion


        #region Combat Shadow
        private Composite ShadowCombat
        {
            get
            {
                return new PrioritySelector(

                    // Move To
                    new NeedToMoveTo(new MoveTo()),

                    // Face Target
                    new NeedToFaceTarget(new FaceTarget()),

                    // Shadowform
                    new NeedToShadowform(new Shadowform()),

                    // Dispersion
                    new NeedToDispersion(new Dispersion()),

                    // Shadow Word Death
                    new NeedToShadowWordDeath(new ShadowWordDeath()),

                    // Silence
                    new NeedToSilence(new Silence()),

                    // Shadowfiend
                    new NeedToShadowfiend(new Shadowfiend()),

                    // Shackle
                    new NeedToShackle(new Shackle()),

                    // Holy Nova
                    new NeedToHolyNova(new HolyNova()),

                    // Vampiric Touch
                    new NeedToVampiricTouch(new VampiricTouch()),

                    // Shadow Word Pain
                    new NeedToShadowWordPain(new ShadowWordPain()),

                    // Devouring Plague
                    new NeedToDevouringPlague(new DevouringPlague()),
                    
                    // Psychic Scream
                    new NeedToPsychicScream(new PsychicScream()),

                    // Mind Sear
                    new NeedToMindSear(new MindSear()),

                    // Mind Blast
                    new NeedToMindBlast(new MindBlast()),

                    // Mind Spike
                    new NeedToMindSpike(new MindSpike()),

                    // Smite
                    new NeedToSmite(new Smite()),

                    
                    // Mind Flay
                    new NeedToMindFlay(new MindFlay()),

                    // Fade
                    new NeedToFade(new Fade()),

                    // Smite Evangelism
                    new NeedToSmiteEvangelism(new SmiteEvangelism()),

                    // Mind Flay Priority
                    new NeedToMindFlayPriority(new MindFlayPriority()),

                    // Wand Party
                    new NeedToWandParty(new WandParty()),

                    // Move To
                    new NeedToMoveTo(new MoveTo()),

                    // Finally just perform an update
                    new Action(ret => ObjectManager.Update())


                    );
            }
        }
        #endregion
    }
}
