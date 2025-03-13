using System;
using Cinemachine;
using DoubleDCore.Economy.Base;
using DoubleDCore.UI.Base;
using Game.Source.Base;
using Game.Source.Extensions;
using Game.Source.Items.Base;
using Game.Source.Models;
using Game.Source.Storage;
using Game.Source.UI.Pages;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    [RequireComponent(typeof(CharacterWallet))]
    public class Player : MonoBehaviour, IReceiver<TakeableItem>, IGunActor, ITarget, IWallet<int>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Team _team = Team.Red;
        [Min(1), SerializeField] private int _neuronWeight = 10;

        public readonly Receiver<TakeableItem> Receiver = new();

        private CharacterWallet _wallet;

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

        private void Awake()
        {
            _wallet = GetComponent<CharacterWallet>();
        }

        private void Start()
        {
            _uiManager.OpenPage<WalletPage, IWallet<int>>(this);
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
                _input.Character.Action.started += OnStartAction;
                _input.Character.Action.canceled += OnEndAction;

                _uiManager.OpenPage<ActionTipPage, ActionItem>(actionItem);
            }
        }

        private void OnTake(TakeableItem obj)
        {
            _input.Character.Throw.started -= OnThrow;
            _uiManager.ClosePage<ThrowTipPage>();

            if (obj is ActionItem actionItem)
            {
                _input.Character.Action.started -= OnStartAction;
                _input.Character.Action.canceled -= OnEndAction;

                EndAction(actionItem);

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

        private void OnStartAction(InputAction.CallbackContext obj)
        {
            if (Peek() is ActionItem actionItem)
                actionItem.StartAction(this);
        }

        private void OnEndAction(InputAction.CallbackContext obj)
        {
            if (Peek() is ActionItem actionItem)
                EndAction(actionItem);
        }

        private void EndAction(ActionItem actionItem)
        {
            actionItem.StopAction(this);
        }

        public Vector3 Position => transform.position;

        public int Weight => _neuronWeight;

        public Team Team => _team;

        public event Action<int, int> ValueChanged;

        public int Value => _wallet.Value;

        public void Add(int value, object provider = null)
        {
            _wallet.Add(value, provider);

            ValueChanged?.Invoke(_wallet.Value, value);
        }

        public bool TrySpend(int value, object provider = null)
        {
            bool isSuccess = _wallet.TrySpend(value, provider);

            if (isSuccess)
                ValueChanged?.Invoke(_wallet.Value, -value);

            return isSuccess;
        }
    }
}