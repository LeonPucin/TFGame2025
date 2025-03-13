using System;
using Cysharp.Threading.Tasks;
using DoubleDCore.TimeTools;
using UnityEngine;

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

        public Timer Timer => _timer;

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
            SetInterval();
            GasEventFinished?.Invoke();
        }

        public async void TurnGas()
        {
            _timer.Stop();

            GasEventStarting?.Invoke();

            await UniTask.WaitForSeconds(_eventDelay);

            GasEvent?.Invoke();
        }
    }
}