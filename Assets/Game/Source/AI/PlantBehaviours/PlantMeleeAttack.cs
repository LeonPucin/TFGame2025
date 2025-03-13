using System;
using Game.Source.AI.Behaviours;

namespace Game.Source.AI.PlantBehaviours
{
    public class PlantMeleeAttack : MeleeAttackBehavior
    {
        protected override void ExecuteAttack(Action performAttack)
        {
            performAttack?.Invoke();
        }
    }
}