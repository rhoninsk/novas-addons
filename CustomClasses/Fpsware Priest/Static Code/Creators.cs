using TreeSharp;

namespace Athena
{
    public partial class Fpsware
    {
        private Composite _combatBehavior;
        public override Composite CombatBehavior
        {
            get
            {
                if (_combatBehavior == null)
                {
                    Utils.Log("Creating combat behavior...");
                    _combatBehavior = CreateCombatBehavior();
                }
                return _combatBehavior;
            }
        }


        private Composite _pullBehavior;
        public override Composite PullBehavior
        {
            get
            {
                if (_pullBehavior == null)
                {
                    Utils.Log("Creating pull behavior...");
                    _pullBehavior = CreatePullBehavior();
                }
                return _pullBehavior;
            }
        }



        private Composite _restBehavior;
        public override Composite RestBehavior
        {
            get
            {
                if (_restBehavior == null)
                {
                    Utils.Log("Creating rest behavior...");
                    _restBehavior = CreateRestBehavior();
                }
                return _restBehavior;
            }
        }


        private Composite _healBehavior;
        public override Composite HealBehavior
        {
            get
            {
                if (_healBehavior == null)
                {
                    Utils.Log("Creating heal behavior...");
                    _healBehavior = CreateHealBehavior();
                }
                return _healBehavior;
            }
        }



    }
}
