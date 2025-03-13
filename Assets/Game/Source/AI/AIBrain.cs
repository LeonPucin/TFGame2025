using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Game.Source.AI.Base;
using UnityEngine;

namespace Game.Source.AI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] private TreeInfo _rootNode;
        [SerializeField] private TreeInfo[] _nodes;

        private BehaviourTree _behaviourTree;

        protected virtual void Awake()
        {
            var rootNode = GetRootNode();

            if (rootNode == null)
            {
                Debug.LogWarning("Root node is null");
                return;
            }

            _behaviourTree = new BehaviourTree(rootNode);
        }

        protected async virtual void OnEnable()
        {
            await UniTask.NextFrame();

            _tactIsRunning = true;
            _routine = StartCoroutine(StartTactGenerator(_behaviourTree));
        }

        protected virtual void OnDisable()
        {
            _tactIsRunning = false;

            if (_routine != null)
                StopCoroutine(_routine);
        }

        private bool _tactIsRunning;
        private Coroutine _routine;

        private IEnumerator StartTactGenerator(BehaviourTree tree)
        {
            while (_tactIsRunning)
            {
                tree.Update();

                if (tree.CurrentNode.UpdateRate <= 0)
                    yield return null;
                else
                    yield return new WaitForSeconds(1 / (float)tree.CurrentNode.UpdateRate);
            }
        }

        private INode GetRootNode()
        {
            var result = _rootNode;

            InitNode(result);

            foreach (var child in _nodes)
                InitNode(child);

            return result.Node;

            void InitNode(TreeInfo node)
            {
                foreach (var childNode in node.Children)
                    node.Node.AddChild(childNode);
            }
        }

        [Serializable]
        private class TreeInfo
        {
            public NodeObject Node;
            public NodeObject[] Children;
        }
    }
}