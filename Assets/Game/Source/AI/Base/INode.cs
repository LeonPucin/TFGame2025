using System.Collections.Generic;
using DoubleDCore.Automation.Base;

namespace Game.Source.AI.Base
{
    public interface INode : IState
    {
        public NodeStatus Status { get; }

        public int UpdateRate { get; }

        public IReadOnlyList<INode> Children { get; }

        public void AddChild(INode child);

        public void AddChildren(IEnumerable<INode> child);
    }
}