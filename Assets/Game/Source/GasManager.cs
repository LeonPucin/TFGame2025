using System;
using Cysharp.Threading.Tasks;
using DoubleDCore.TimeTools;
using DoubleDCore.UI.Base;
using Game.Source.UI.Pages;
using UnityEngine;
using Zenject;

namespace Game.Source
{
    public class GasManager : MonoBehaviour
    {
        [Min(0), SerializeField] private float _eventInterval = 60f * 2f;
        [Min(0), SerializeField] private float _eventDelay = 10f;

        public event Action GasEvent;
        public event Action GasEventStarting;
        public event Action GasEventFinished;

        private readonly Timer _timer = new(TimeBindingType.ScaledTime);

        private IUIManager _uiManager;

        public Timer Timer => _timer;

        [Inject]
        private void Init(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        private void Start()
        {
            SetInterval();
        }

        private void SetInterval()
        {
            _timer.Start(_eventInterval, onEnd: TurnGas);
        }

        public void StopGas()
        {
            _uiManager.ClosePage<GasAlertPage>();

            SetInterval();
            GasEventFinished?.Invoke();
        }

        public async void TurnGas()
        {
            _timer.Stop();

            _uiManager.OpenPage<GasAlertPage>();

            GasEventStarting?.Invoke();

            await UniTask.WaitForSeconds(_eventDelay);

            GasEvent?.Invoke();
        }
    }
}