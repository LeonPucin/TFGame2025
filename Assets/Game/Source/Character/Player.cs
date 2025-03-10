using DoubleDCore.UI.Base;
using Game.Source.Items.Base;
using Game.Source.Storage;
using Game.Source.UI.Pages;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Source.Character
{
    [RequireComponent(typeof(CharacterReceiver))]
    public class Player : MonoBehaviour, IReceiver<TakeableItem>
    {
        private CharacterReceiver _characterReceiver;

        private InputLevers _input;
        private IUIManager _uiManager;

        [Inject]
        private void Init(InputService inputService, IUIManager uiManager)
        {
            _input = inputService.GetInputProvider();
            _uiManager = uiManager;
        }

        private void Awake()
        {
            _characterReceiver = GetComponent<CharacterReceiver>();
        }

        public void Put(TakeableItem obj)
        {
            _characterReceiver.Receiver.Put(obj);

            _input.Character.Throw.started += OnThrow;
            _uiManager.OpenPage<ThrowTipPage>();
        }

        public TakeableItem Take()
        {
            _input.Character.Throw.started -= OnThrow;
            _uiManager.ClosePage<ThrowTipPage>();

            return _characterReceiver.Receiver.Take();
        }

        private void OnThrow(InputAction.CallbackContext obj)
        {
            Take();
        }
    }
}