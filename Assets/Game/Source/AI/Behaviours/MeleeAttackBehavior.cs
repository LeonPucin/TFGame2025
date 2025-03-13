using System;
using DoubleDCore.TimeTools;
using UnityEngine;

namespace Game.Source.AI.Behaviours
{
    public abstract class MeleeAttackBehavior : AttackBehavior
    {
        private readonly Timer _attackTimer = new(TimeBindingType.ScaledTime);

        private float _damage;
        private Vector3 _damageCenter;
        private float _damageRadius;

        public override void StartAttack(float damage, float damageRadius, Vector3 damageCenter, float attackCooldown)
        {
            Agent.isStopped = true;

            _damage = damage;
            _damageCenter = damageCenter;
            _damageRadius = damageRadius;

            if (_attackTimer.IsWorked == false)
            {
                ExecuteAttack(PerformAttack);
                _attackTimer.Start(attackCooldown);
            }
        }

        public override void StopAttack()
        {
            Agent.isStopped = false;
        }

        private void PerformAttack()
        {
            var targets = GetTargets(_damageCenter, _damageRadius);

            foreach (var target in targets)
                target.TakeDamage(_damage);
        }

        protected abstract void ExecuteAttack(Action performAttack);
    }
}