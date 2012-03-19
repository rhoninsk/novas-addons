using TreeSharp;

namespace Athena
{
    public partial class Fpsware
    {
        
        private Composite CreateRestBehavior()
        {
            return new PrioritySelector(

                // If we are casting then just stop doing stuff. 
                new Decorator(ret => Me.IsCasting, new AlwaysSucceed()),

                // Dispersion Rest
                new NeedToDispersionRest(new DispersionRest()),

                // Cancel Food
                new NeedToCancelFood(new CancelFood()),

                // Cancel Drink
                new NeedToCancelDrink(new CancelDrink()),

                // Heal (Flash Heal) before drinking
                new NeedToRestHeal(new RestHeal()),

                // Drink
                new NeedToDrink(new Drink()),

                // Eat
                new NeedToEat(new Eat()),

                // Shadowform
                new NeedToShadowform(new Shadowform())

                );
        }

        

        private static Composite CreateHealBehavior()
        {
            return new PrioritySelector(

                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

                // Pain Suppression
                new NeedToPainSuppression(new PainSuppression()),

                // Renew
                new NeedToRenew(new Renew()),

                // Power Word Shield
                new NeedToPowerWordShield(new PowerWordShield()),

                // Flash Heal
                new NeedToFlashHeal(new FlashHeal()),

                // Use Mana Potion
                new NeedToUseManaPot(new UseManaPot())

                );
        }

    }
}
