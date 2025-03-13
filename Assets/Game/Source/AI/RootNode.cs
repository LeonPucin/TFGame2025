using Game.Source.AI.Base;
using UnityEngine;

namespace Game.Source.AI
{
    public class RootNode : NodeObject
    {
        [SerializeField] protected int _updateRate = 40;

        public override NodeStatus Status => NodeStatus.Ready;

        public override int UpdateRate => _updateRate;

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}