using Game.Source.AI.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Source.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class IdleNode : NodeObject
    {
        [SerializeField] private int _updateRate = 20;

        private NavMeshAgent _agent;

        public override NodeStatus Status => NodeStatus.Ready;

        public override int UpdateRate => _updateRate;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            _agent.isStopped = false;
        }

        public override void Exit()
        {
            _agent.isStopped = true;
        }
    }
}