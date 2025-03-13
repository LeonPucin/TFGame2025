using Game.Source.AI.Base;
using Game.Source.AI.Behaviours;
using UnityEngine;

namespace Game.Source.AI
{
    public class AttackNode : NodeObject
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _damageRadius = 1f;
        [SerializeField] private Transform _damageCenter;
        [SerializeField] private float _detectionRadius = 1f;
        [SerializeField] private Transform _detectionCenter;
        [SerializeField] private float _attackCooldown = 3f;

        [SerializeField] private AttackBehavior _attackBehavior;

        private bool _isAttackingZone;

        public override NodeStatus Status
        {
            get
            {
                bool isDamageRange = _attackBehavior.HasEnemy(_damageCenter.position, _damageRadius);
                bool isDetectionRange = _attackBehavior.HasEnemy(_detectionCenter.position, _detectionRadius);

                if (_isAttackingZone && isDamageRange == false)
                {
                    _isAttackingZone = false;
                    return NodeStatus.Complete;
                }

                if (_isAttackingZone == false && isDetectionRange)
                    _isAttackingZone = true;

                if (_isAttackingZone == false)
                    return NodeStatus.Complete;

                return NodeStatus.Ready;
            }
        }

        public override int UpdateRate => 40;

        public override void Enter()
        {
            _attackBehavior?.StartAttack(_damage, _damageRadius, _damageCenter.position, _attackCooldown);
        }

        public override void Exit()
        {
            _attackBehavior?.StopAttack();
        }

        private void OnDrawGizmosSelected()
        {
            if (_damageCenter != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_damageCenter.position, _damageRadius);
            }

            if (_detectionCenter != null)
            {
                Gizmos.color = new Color(255, 100, 0);
                Gizmos.DrawWireSphere(_detectionCenter.position, _detectionRadius);
            }
        }
    }
}