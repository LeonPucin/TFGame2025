using System.Collections.Generic;

namespace Game.Source.AI.Base
{
    public abstract class Node : INode
    {
        private readonly List<INode> _children = new();

        public abstract NodeStatus Status { get; }

        public abstract int UpdateRate { get; }

        public IReadOnlyList<INode> Children => _children;

        public void AddChild(INode child)
        {
            _children.Add(child);
        }

        public void AddChildren(IEnumerable<INode> child)
        {
            _children.AddRange(child);
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}