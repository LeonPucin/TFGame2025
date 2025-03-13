using System;
using Game.Source.Base;
using UnityEngine;

namespace Game.Source.AI.Behaviours
{
    public abstract class ExplosionBehavior : AttackBehavior
    {
        private float _damage;
        private Vector3 _damageCenter;
        private float _damageRadius;

        private bool _isAttacking;

        public override void StartAttack(float damage, float damageRadius, Vector3 damageCenter, float attackCooldown)
        {
            if (_isAttacking)
                return;

            _isAttacking = true;

            Agent.isStopped = true;

            _damage = damage;
            _damageCenter = damageCenter;
            _damageRadius = damageRadius;

            ExecuteExplosion(PerformAttack);
        }

        public override void StopAttack()
        {
            // Agent.isStopped = false;
        }

        private void PerformAttack()
        {
            var targets = GetTargets(_damageCenter, _damageRadius);

            foreach (var target in targets)
                target.TakeDamage(_damage);

            if (SelfTarget is IDamageable selfDamageable)
                selfDamageable.Kill();
        }

        protected abstract void ExecuteExplosion(Action performAction);
    }
}