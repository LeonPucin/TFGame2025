using DoubleDCore.Extensions;
using DoubleDCore.TimeTools;
using Game.Source.AI.Base;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Source.AI
{
    public class RandomPatrolsNode : NodeObject
    {
        [SerializeField] private float _patrolsRadius = 10f;
        [SerializeField] private float _newPointTime = 15f;
        [SerializeField] private int _updateRate = 30;
        [SerializeField] private float _patrolsSpeed = 1;
        [SerializeField] private float _pointRadius = 0.5f;

        private readonly Timer _changePointTimer = new(TimeBindingType.ScaledTime);

        private NavMeshAgent _agent;

        private bool _isReachedPoint;

        public override NodeStatus Status
        {
            get
            {
                if (_isReachedPoint && _changePointTimer.IsWorked)
                    return NodeStatus.Complete;

                return NodeStatus.Ready;
            }
        }

        public override int UpdateRate => _updateRate;

        public override void Enter()
        {
            if (_changePointTimer.IsWorked == false)
            {
                _agent.speed = _patrolsSpeed;

                Vector2 randomPoint = Random.insideUnitCircle * _patrolsRadius;
                _agent.SetDestination(transform.position + randomPoint.ToVector3(CoordinatePlane.XZ));

                _changePointTimer.Start(_newPointTime);
            }

            _isReachedPoint = Vector3.Distance(transform.position, _agent.destination) < _pointRadius;
        }

        public override void Exit()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _patrolsRadius);
        }
    }
}