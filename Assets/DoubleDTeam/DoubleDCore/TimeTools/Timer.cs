using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DoubleDCore.TimeTools
{
    public class Timer
    {
        private readonly TimeBindingType _timeBinding;

        public float RemainingTime { get; private set; }

        private bool _isWorked;
        public bool IsWorked => _isWorked;

        public event Action<float> Started;
        public event Action<float> Stopped;
        public event Action Performed;

        private CancellationTokenSource _cancellationTokenSource;

        public Timer(TimeBindingType timeBindingType)
        {
            _timeBinding = timeBindingType;
        }

        public async UniTask Start(float time, Action<float> onTick = null, Action onEnd = null)
        {
            if (_isWorked)
                Stop();

            _cancellationTokenSource = new CancellationTokenSource();

            _isWorked = true;
            RemainingTime = time;

            float initialTime = time;

            Started?.Invoke(time);

            try
            {
                while (RemainingTime > 0)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);

                    float delta = _timeBinding switch
                    {
                        TimeBindingType.RealTime => Time.unscaledDeltaTime,
                        TimeBindingType.ScaledTime => Time.deltaTime,
                        TimeBindingType.FixedTime => Time.fixedDeltaTime,
                        _ => throw new ArgumentOutOfRangeException($"{_timeBinding} is not supported")
                    };

                    RemainingTime -= delta;
                    onTick?.Invoke(1 - RemainingTime / initialTime);
                }

                RemainingTime = 0;

                onEnd?.Invoke();
                Performed?.Invoke();
            }
            finally
            {
                _isWorked = false;
                Stopped?.Invoke(RemainingTime);
            }
        }

        public void Stop()
        {
            if (_isWorked == false)
                return;

            _cancellationTokenSource?.Cancel();
        }
    }
}