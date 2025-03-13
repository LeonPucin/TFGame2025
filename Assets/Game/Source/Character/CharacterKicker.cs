using Game.Source.Items;
using UnityEngine;

namespace Game.Source.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterKicker : MonoBehaviour
    {
        [Min(0), SerializeField] private float _kickPower = 20f;
        [SerializeField] private float _upKickDegree = 45f;
        [SerializeField] private CharacterController _characterController;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null || collision.rigidbody.TryGetComponent(out ProduceTray _) == false)
                return;

            Vector3 baseDirection = collision.GetContact(0).point - transform.position;
            Vector3 horizontalDirection = new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

            float angleRad = _upKickDegree * Mathf.Deg2Rad;

            Vector3 kickDirection = horizontalDirection * Mathf.Cos(angleRad) + Vector3.up * Mathf.Sin(angleRad);

            if (_characterController.velocity.magnitude > 0.1f)
                collision.rigidbody.AddForce(_characterController.velocity, ForceMode.VelocityChange);

            collision.rigidbody.AddForce(kickDirection * _kickPower, ForceMode.Force);
        }
    }
}