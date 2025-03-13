using System.Collections.Generic;
using UnityEngine;

namespace Game.Source.AI.Base
{
    public abstract class NodeObject : MonoBehaviour, INode
    {
        private readonly List<INode> _children = new();

        public abstract NodeStatus Status { get; }
        public abstract int UpdateRate { get; }

        public IReadOnlyList<INode> Children => _children;

        public abstract void Enter();
        public abstract void Exit();

        public void AddChild(INode child)
        {
            _children.Add(child);
        }

        public void AddChildren(IEnumerable<INode> child)
        {
            _children.AddRange(child);
        }
    }
}