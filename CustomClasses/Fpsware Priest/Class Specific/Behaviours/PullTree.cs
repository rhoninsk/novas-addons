using Styx.WoWInternals;
using TreeSharp;

namespace Athena
{
    public partial class Fpsware
    {
        #region Pull
        

        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(

                // If we can't navigate to the target blacklist it.
                new NeedToNavigatePath(new NavigatePath()),

                // Check if the target is suitable for pulling, if not blacklist it
                new NeedToBlacklistPullTarget(new BlacklistPullTarget()),

                // Check pull timers and blacklist bad pulls where required
                new NeedToCheckPullTimer(new BlacklistPullTarget()),

                // Pull dependant upon your SPEC. Not your currently shapeshifted form
                new Switch<ClassHelpers.Priest.ClassType>(ret => ClassHelpers.Priest.ClassSpec,
                                                         new SwitchArgument<ClassHelpers.Priest.ClassType>(DisciplinePull, ClassHelpers.Priest.ClassType.Discipline),
                                                         new SwitchArgument<ClassHelpers.Priest.ClassType>(ShadowPull, ClassHelpers.Priest.ClassType.Shadow),
                                                         new SwitchArgument<ClassHelpers.Priest.ClassType>(DisciplinePull, ClassHelpers.Priest.ClassType.None),
                                                         new SwitchArgument<ClassHelpers.Priest.ClassType>(HolyPull, ClassHelpers.Priest.ClassType.Holy)


                    )
                );
        }


        #region Pull Shadow
        private Composite ShadowPull
        {
            get
            {
                return new PrioritySelector(

                    // Shadowform
                    new NeedToShadowform(new Shadowform()),

                    // Casting so stop moving
                    new NeedToCastingSoStopMoving(new CastingSoStopMoving()),

                    // Distance Check Other
                    new NeedToDistanceCheckOther(new DistanceCheckOther()),

                    // Face Pull
                    new NeedToFacePull(new FacePull()),

                    // Shield Before Pull
                    new NeedToShieldPull(new ShieldPull()),

                    // Pull Spell
                    new NeedToPullSpell(new PullSpell()),

                    // Smite Pull
                    new NeedToSmitePull(new SmitePull())

                    // Move To
                    //new NeedToMoveTo(new MoveTo())

                    // Update ObjectManager
                    //new Action(ret => ObjectManager.Update())

                    );
            }
        }
        #endregion

        #region Pull Holy
        private Composite HolyPull
        {
            get
            {
                return new PrioritySelector(

                    // Casting so stop moving
                    new NeedToCastingSoStopMoving(new CastingSoStopMoving()),

                    // Distance Check Other
                    new NeedToDistanceCheckOther(new DistanceCheckOther()),

                    // Shield Before Pull
                    new NeedToShieldPull(new ShieldPull()),

                    // Pull Spell
                    new NeedToPullSpell(new PullSpell())

                    // Move To
                    //new NeedToMoveTo(new MoveTo())

                    // Update ObjectManager
                    //new Action(ret => ObjectManager.Update())


                    );
            }
        }
        #endregion

        #region Pull Discipline
        private Composite DisciplinePull
        {
            get
            {
                return new PrioritySelector(

                    // Casting so stop moving
                    new NeedToCastingSoStopMoving(new CastingSoStopMoving()),

                    // Distance Check Other
                    new NeedToDistanceCheckOther(new DistanceCheckOther()),

                    // Face Pull
                    new NeedToFacePull(new FacePull()),

                    // Shield Before Pull
                    new NeedToShieldPull(new ShieldPull()),

                    // Pull Spell
                    new NeedToPullSpell(new PullSpell()),

                    // Smite Pull
                    new NeedToSmitePull(new SmitePull())

                    // Move To
                    //new NeedToMoveTo(new MoveTo())

                    // Update ObjectManager
                    //new Action(ret => ObjectManager.Update())


                    );
            }
        }
        #endregion
        #endregion

    }
}
