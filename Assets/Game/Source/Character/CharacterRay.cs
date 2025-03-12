using Cinemachine;
using Cysharp.Threading.Tasks;
using DoubleDCore.EventActions;
using DoubleDCore.PhysicsTools.Casting;
using DoubleDCore.PhysicsTools.Casting.Raycasting;
using DoubleDCore.TimeTools;
using DoubleDCore.UI.Base;
using Game.Source.Extensions;
using Game.Source.Items.Base;
using Game.Source.UI.Pages;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    public class CharacterRay : MonoBehaviour, ITargetListener<ISelectableObject>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [Range(0f, 100f), SerializeField] private float _rayDistance = 5f;
        [SerializeField] private LayerMask _rayMask;
        [Range(0.01f, 5f), SerializeField] private float _reinterpretDelay = 0.05f;

        private RayCastInfo _rayCastInfo;
        private Player _player;

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
            _player = GetComponent<Player>();

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

        private void OnInteract(InputAction.CallbackContext obj = default)
        {
            if (_currentTarget == null || _interactionTimer.IsWorked)
                return;

            if (_currentTarget is not IInteractiveObject interactiveTarget)
                return;

            bool canInteract = interactiveTarget.CanInteract(_player);
            var argument = new InteractiveObjectPageArgument(interactiveTarget.Name, canInteract, _onInteractProgress);
            _uiManager.OpenPage<InteractiveObjectPage, InteractiveObjectPageArgument>(argument);

            if (canInteract == false)
                return;

            _interactionTimer.Start(interactiveTarget.InteractDelay,
                progress => { _onInteractProgress?.Invoke(progress); },
                async () =>
                {
                    interactiveTarget.Interact(_player);

                    await UniTask.WaitForSeconds(_reinterpretDelay);

                    if (_input.Character.Interact.IsPressed())
                        OnInteract();
                });
        }

        private void OnInteractCanceled(InputAction.CallbackContext obj)
        {
            CancelInteract();
        }

        private ISelectableObject _currentTarget;
        private readonly ActionReference<float> _onInteractProgress = new();
        private Timer _interactionTimer;

        public ISelectableObject GetTarget(Collider target)
        {
            return target.GetComponent<ISelectableObject>();
        }

        public bool IsTarget(ISelectableObject target)
        {
            return true;
        }

        public void OnCastEnter(ISelectableObject target)
        {
            _currentTarget = target;
            _currentTarget.Select();

            bool canInteract = _currentTarget is IInteractiveObject interactiveTarget
                               && interactiveTarget.CanInteract(_player);

            var argument = new InteractiveObjectPageArgument(target.Name, canInteract, _onInteractProgress);
            _uiManager.OpenPage<InteractiveObjectPage, InteractiveObjectPageArgument>(argument);
        }

        public void OnCastExit(ISelectableObject target)
        {
            CancelInteract();

            _currentTarget?.Deselect();
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