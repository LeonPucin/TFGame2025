using DoubleDCore.Debugging;
using UnityEngine;

namespace Game.Source.AI.Base
{
    public class BehaviourTree
    {
        private readonly INode _rootNode;

        private INode _currentNode;
        public INode CurrentNode => _currentNode;

        public BehaviourTree(INode rootNode)
        {
            _rootNode = rootNode;

            _currentNode = rootNode;
            _currentNode.Enter();
        }

        public void Update()
        {
            var runNode = GetRunNode(_rootNode);

            if (_currentNode != runNode)
            {
                _currentNode.Exit();
                _currentNode = runNode;

                if (_currentNode is NodeObject nodeObject)
                {
                    ScreenDebug.Log(nodeObject.gameObject.GetInstanceID(),
                        nodeObject.GetType().Name,
                        nodeObject.transform.position,
                        Color.yellow, 3f, 2f);
                }
            }

            _currentNode.Enter();
        }

        private INode GetRunNode(INode rootNode)
        {
            foreach (var child in rootNode.Children)
            {
                var node = GetRunNode(child);

                if (node != null)
                    return node;
            }

            return rootNode.Status == NodeStatus.Ready ? rootNode : null;
        }
    }
}