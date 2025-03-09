using Cinemachine;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [Range(0, 100f), SerializeField] private float _speed = 5f;
        [Range(0, 10f), SerializeField] private float _runBoost = 10f;
        [Range(0, 100f), SerializeField] private float _maxVerticalVelocity = 55f;

        private bool _isRunBoost;
        private float _weightModifier;
        private InputLevers _inputProvider;
        private CharacterController _characterController;

        private float _verticalVelocity;

        [Inject]
        private void Init(InputService inputService)
        {
            _inputProvider = inputService.GetInputProvider();
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            ResetWeightModifier();
        }

        private void OnEnable()
        {
            _inputProvider.Character.RunBoost.started += OnRunBoost;
            _inputProvider.Character.RunBoost.canceled += OnCancelRunBoost;
        }

        private void OnDisable()
        {
            _inputProvider.Character.RunBoost.started -= OnRunBoost;
            _inputProvider.Character.RunBoost.canceled -= OnCancelRunBoost;
        }

        private void OnRunBoost(InputAction.CallbackContext obj)
        {
            _isRunBoost = true;
        }

        private void OnCancelRunBoost(InputAction.CallbackContext obj)
        {
            _isRunBoost = false;
        }

        private void Update()
        {
            Vector2 input = _inputProvider.Character.Movement.ReadValue<Vector2>();

            Vector3 forward = _camera.State.FinalOrientation * Vector3.forward;
            forward.y = 0f;

            Vector3 right = _camera.State.FinalOrientation * Vector3.right;

            Vector3 direction = (right * input.x + forward * input.y).normalized;
            Vector3 move = direction * _speed;

            if (_isRunBoost)
                move *= _runBoost;

            move *= _weightModifier;

            _verticalVelocity += Physics.gravity.magnitude * Time.deltaTime;

            if (_characterController.isGrounded)
                _verticalVelocity = 0f;

            Vector3 gravityMove = Physics.gravity.normalized * _verticalVelocity;

            Vector3 finalMove = move + gravityMove;
            _characterController.Move(finalMove * Time.deltaTime);
        }

        public void SetWeightModifier(float modifier)
        {
            _weightModifier = modifier;
        }

        public void ResetWeightModifier()
        {
            _weightModifier = 1f;
        }
    }
}