using System.Collections.Generic;
using Game.Source.Base;
using UnityEngine;

namespace Game.Source.AI.Behaviours
{
    public abstract class AttackBehavior : NodeBehavior
    {
        public abstract void StartAttack(float damage, float damageRadius, Vector3 damageCenter, float attackCooldown);

        public abstract void StopAttack();

        protected IDamageable[] GetTargets(Vector3 position, float radius)
        {
            var count = Physics.OverlapSphereNonAlloc(position, radius, Buffer, TargetLayer);

            var result = new List<IDamageable>();

            for (int i = 0; i < count; i++)
            {
                var hit = Buffer[i];

                if (hit.TryGetComponent(out ITarget target) == false)
                    continue;

                if (target is not IDamageable damageable)
                    continue;

                if (target == SelfTarget)
                    continue;

                if (FriendlyFire == false &&
                    TeamMatrix.IsIntractable(target.Team, SelfTarget.Team) == false)
                    continue;

                result.Add(damageable);
            }

            return result.ToArray();
        }
    }
}