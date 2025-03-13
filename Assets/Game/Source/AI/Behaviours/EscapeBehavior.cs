using DoubleDCore.TimeTools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Source.AI.Behaviours
{
    [RequireComponent(typeof(EscapeNode))]
    public class EscapeBehavior : NodeBehavior
    {
        [SerializeField] private Vector3 _deviationDegrees = new(0f, 10f, 0f);
        [SerializeField] private float _changeDegreeTime = 1f;

        private Vector3 _deviation;

        private readonly Timer _changeDegreeCooldown = new(TimeBindingType.ScaledTime);

        public void StartEscape(float detectionRadius, float speed, float nextPointDistance,
            float farTargetValueModificator)
        {
            var enemy = GetPreferredTarget(transform.position, detectionRadius, farTargetValueModificator);

            Agent.speed = speed;

            Vector3 escapeDirection = (transform.position - enemy.Position).normalized;

            if (_changeDegreeCooldown.IsWorked == false)
            {
                _deviation = new Vector3(
                    Random.Range(-_deviationDegrees.x, _deviationDegrees.x),
                    Random.Range(-_deviationDegrees.y, _deviationDegrees.y),
                    Random.Range(-_deviationDegrees.z, _deviationDegrees.z));

                _changeDegreeCooldown.Start(_changeDegreeTime);
            }

            escapeDirection = Quaternion.Euler(_deviation) * escapeDirection;

            Agent.SetDestination(transform.position + nextPointDistance * escapeDirection);
        }
    }
}