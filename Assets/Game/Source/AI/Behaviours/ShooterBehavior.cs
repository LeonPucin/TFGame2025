using System;
using DoubleDCore.TimeTools;
using Game.Source.Base;
using UnityEngine;

namespace Game.Source.AI.Behaviours
{
    public abstract class ShooterBehavior : AttackBehavior
    {
        [Range(0.01f, 1f), SerializeField] private float _farTargetModifier = 0.5f;

        private readonly Timer _attackTimer = new(TimeBindingType.ScaledTime);

        private float _damage;
        private float _detectionRadius;
        private Vector3 _detectionCenter;

        public override void StartAttack(float damage, float detectionRadius, Vector3 detectionCenter,
            float attackCooldown)
        {
            Agent.isStopped = true;

            _damage = damage;
            _detectionRadius = detectionRadius;
            _detectionCenter = detectionCenter;

            if (_attackTimer.IsWorked)
                return;

            ExecuteAim();

            _attackTimer.Start(attackCooldown, onEnd: Shoot);
        }

        public override void StopAttack()
        {
            ExecuteUnAim();
            Agent.isStopped = false;
        }

        private void Shoot()
        {
            ExecuteShoot(PerformAttack);
        }

        private void PerformAttack()
        {
            var target = GetPreferredTarget(_detectionCenter, _detectionRadius, _farTargetModifier);

            if (target is IDamageable damageable)
                damageable.TakeDamage(_damage);
        }

        protected abstract void ExecuteAim();

        protected abstract void ExecuteUnAim();

        protected abstract void ExecuteShoot(Action performAttack);
    }
}