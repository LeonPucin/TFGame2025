using Game.Source.Base;
using Game.Source.Models;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Source.AI.Behaviours
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ITarget))]
    public abstract class NodeBehavior : MonoBehaviour
    {
        protected ITarget SelfTarget { get; private set; }
        protected NavMeshAgent Agent { get; private set; }

        protected LayerMask TargetLayer { get; private set; }
        protected TeamMatrix TeamMatrix { get; private set; }
        protected bool FriendlyFire { get; private set; }

        [Inject]
        private void Init(WorldRule worldRule)
        {
            FriendlyFire = worldRule.Config.FriendlyFire;
            TargetLayer = worldRule.Config.TargetMask;
            TeamMatrix = worldRule.TeamMatrix;
        }

        public void Awake()
        {
            SelfTarget = GetComponent<ITarget>();
            Agent = GetComponent<NavMeshAgent>();
        }

        protected readonly Collider[] Buffer = new Collider[255];

        public bool HasEnemy(Vector3 position, float radius)
        {
            var count = Physics.OverlapSphereNonAlloc(position, radius, Buffer, TargetLayer);

            for (int i = 0; i < count; i++)
            {
                var hit = Buffer[i];

                if (hit.TryGetComponent(out ITarget target) == false)
                    continue;

                if (target == SelfTarget)
                    continue;

                if (TeamMatrix.IsIntractable(target.Team, SelfTarget.Team) == false)
                    continue;

                return true;
            }

            return false;
        }

        public ITarget GetPreferredTarget(Vector3 center, float radius, float farTargetValueModificator)
        {
            var count = Physics.OverlapSphereNonAlloc(center, radius, Buffer, TargetLayer);

            ITarget result = null;
            float maxValue = float.MinValue;

            for (int i = 0; i < count; i++)
            {
                var hit = Buffer[i];

                if (hit.TryGetComponent(out ITarget target) == false)
                    continue;

                if (target == SelfTarget)
                    continue;

                if (TeamMatrix.IsIntractable(target.Team, SelfTarget.Team) == false)
                    continue;

                float linearization = Vector3.Distance(center, hit.transform.position) / radius;
                float value = target.Weight * Mathf.Lerp(1f, farTargetValueModificator, linearization);

                if (value > 0 && value > maxValue)
                {
                    result = target;
                    maxValue = value;
                }
            }

            return result;
        }
    }
}