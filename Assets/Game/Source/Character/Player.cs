using Cinemachine;
using DoubleDCore.UI.Base;
using Game.Source.Base;
using Game.Source.Extensions;
using Game.Source.Items.Base;
using Game.Source.Storage;
using Game.Source.UI.Pages;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    public class Player : MonoBehaviour, IReceiver<TakeableItem>, IGunActor
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        public readonly Receiver<TakeableItem> Receiver = new();

        private InputLevers _input;
        private IUIManager _uiManager;

        public Ray ShootRay
        {
            get
            {
                var origin = _camera.GetPosition();
                var direction = _camera.GetForward();
                return new Ray(origin, direction);
            }
        }

        [Inject]
        private void Init(InputService inputService, IUIManager uiManager)
        {
            _input = inputService.GetInputProvider();
            _uiManager = uiManager;
        }

        private void OnEnable()
        {
            Receiver.OnPut += OnPut;
            Receiver.OnTake += OnTake;
        }

        private void OnDisable()
        {
            Receiver.OnPut -= OnPut;
            Receiver.OnTake -= OnTake;
        }

        private void OnPut(TakeableItem obj)
        {
            _input.Character.Throw.started += OnThrow;
            _uiManager.OpenPage<ThrowTipPage>();

            if (obj is ActionItem actionItem)
            {
                _input.Character.Action.started += OnAction;
                _uiManager.OpenPage<ActionTipPage, ActionItem>(actionItem);
            }
        }

        private void OnTake(TakeableItem obj)
        {
            _input.Character.Throw.started -= OnThrow;
            _uiManager.ClosePage<ThrowTipPage>();

            if (obj is ActionItem actionItem)
            {
                _input.Character.Action.started -= OnAction;
                _uiManager.ClosePage<ActionTipPage>();
            }
        }

        public void Put(TakeableItem obj)
        {
            Receiver.Put(obj);
        }

        public TakeableItem Take()
        {
            return Receiver.Take();
        }

        public TakeableItem Peek()
        {
            return Receiver.Peek();
        }

        public void TransferFrom(IReceiver<TakeableItem> receiver)
        {
            Receiver.TransferFrom(receiver);
        }

        private void OnThrow(InputAction.CallbackContext obj)
        {
            Take();
        }

        private void OnAction(InputAction.CallbackContext obj)
        {
            if (Peek() is ActionItem actionItem)
                actionItem.Action(this);
        }
    }
}