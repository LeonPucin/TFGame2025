using Game.Source.AI.Base;
using Game.Source.AI.Behaviours;
using Game.Source.Base;
using UnityEngine;

namespace Game.Source.AI
{
    public class PursuitNode : NodeObject
    {
        [SerializeField] private int _updateRate = -1;
        [SerializeField] private float _visibilityRadius = 50f;
        [Range(0.01f, 1f), SerializeField] private float _farTargetValueModificator = 0.5f;

        [SerializeField] private PursuitBehavior _pursuitBehavior;

        private ITarget _currentTarget;

        public override NodeStatus Status
        {
            get
            {
                var target = _pursuitBehavior?.GetPreferredTarget(transform.position, _visibilityRadius,
                    _farTargetValueModificator);

                if (target == null)
                    return NodeStatus.Complete;

                if (target != _currentTarget)
                    _currentTarget = target;

                return NodeStatus.Ready;
            }
        }

        public override int UpdateRate => _updateRate;

        public override void Enter()
        {
            _pursuitBehavior?.StartPursuit(_currentTarget);
        }

        public override void Exit()
        {
            _pursuitBehavior?.StopPursuit();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _visibilityRadius);
        }
    }
}