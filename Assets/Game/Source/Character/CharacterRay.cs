using Cinemachine;
using DoubleDCore.EventActions;
using DoubleDCore.PhysicsTools.Casting;
using DoubleDCore.PhysicsTools.Casting.Raycasting;
using DoubleDCore.TimeTools;
using DoubleDCore.UI.Base;
using Game.Source.Extensions;
using Game.Source.Interactive;
using Game.Source.UI.Pages;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    public class CharacterRay : MonoBehaviour, ITargetListener<IInteractiveObject>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [Range(0f, 100f), SerializeField] private float _rayDistance = 5f;
        [SerializeField] private LayerMask _rayMask;

        private RayCastInfo _rayCastInfo;

        private IUIManager _uiManager;
        private IRayCaster _rayCaster;
        private InputLevers _input;

        [Inject]
        private void Init(IUIManager uiManager, IRayCasterFabric rayCasterFabric, InputService inputService,
            ITimersFactory timersFactory)
        {
            _uiManager = uiManager;
            _rayCaster = rayCasterFabric.Create();
            _input = inputService.GetInputProvider();
            _interactionTimer = timersFactory.Create(TimeBindingType.ScaledTime);
        }

        private void Awake()
        {
            _rayCastInfo = new RayCastInfo(new Ray(transform.position, transform.forward), _rayDistance, _rayMask);
            _rayCaster.AddListener(this, _rayCastInfo);
        }

        private void OnDestroy()
        {
            _rayCaster.RemoveListener(this);
            _rayCaster.Dispose();
        }

        private void OnEnable()
        {
            _rayCaster.StartCast();

            _input.Character.Interact.started += OnInteract;
            _input.Character.Interact.canceled += OnInteractCanceled;
        }

        private void OnDisable()
        {
            _rayCaster.StopCast();

            _input.Character.Interact.started -= OnInteract;
            _input.Character.Interact.canceled -= OnInteractCanceled;
        }

        private void FixedUpdate()
        {
            _rayCastInfo.Ray = new Ray(_camera.State.FinalPosition, _camera.GetForward());
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            if (_currentTarget == null || _interactionTimer.IsWorked)
                return;

            _interactionTimer.Start(_currentTarget.InteractiveInfo.InteractDelay,
                progress => { _onInteractProgress?.Invoke(progress); }, () => { _currentTarget.Interact(); });
        }

        private void OnInteractCanceled(InputAction.CallbackContext obj)
        {
            CancelInteract();
        }

        private IInteractiveObject _currentTarget;

        private readonly ActionReference<float> _onInteractProgress = new();

        private Timer _interactionTimer;

        public IInteractiveObject GetTarget(Collider target)
        {
            return target.GetComponent<IInteractiveObject>();
        }

        public bool IsTarget(IInteractiveObject target)
        {
            return true;
        }

        public void OnCastEnter(IInteractiveObject target)
        {
            _currentTarget = target;

            var argument = new InteractiveObjectPageArgument(target.InteractiveInfo, _onInteractProgress);

            _uiManager.OpenPage<InteractiveObjectPage, InteractiveObjectPageArgument>(argument);
        }

        public void OnCastExit(IInteractiveObject target)
        {
            CancelInteract();

            _currentTarget = null;
            _uiManager.ClosePage<InteractiveObjectPage>();
        }

        private void CancelInteract()
        {
            _interactionTimer.Stop();
            _onInteractProgress?.Invoke(0);
        }

        private void OnDrawGizmos()
        {
            if (_rayCastInfo == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(_rayCastInfo.Ray.origin, _rayCastInfo.Ray.direction * _rayCastInfo.RayMaxDistance);
        }
    }
}