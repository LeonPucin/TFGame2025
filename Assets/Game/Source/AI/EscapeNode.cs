using Game.Source.AI.Base;
using Game.Source.AI.Behaviours;
using UnityEngine;

namespace Game.Source.AI
{
    public class EscapeNode : NodeObject
    {
        [SerializeField] private float _speed = 7f;
        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private float _nextPointDistance = 10f;
        [Range(0.01f, 1f), SerializeField] private float _farTargetValueModificator = 0.5f;

        [SerializeField] private EscapeBehavior _escapeBehavior;

        public override NodeStatus Status
        {
            get
            {
                if (_escapeBehavior.HasEnemy(transform.position, _detectionRadius))
                    return NodeStatus.Ready;

                return NodeStatus.Complete;
            }
        }

        public override int UpdateRate => -1;

        public override void Enter()
        {
            _escapeBehavior?.StartEscape(_detectionRadius, _speed, _nextPointDistance, _farTargetValueModificator);
        }

        public override void Exit()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}