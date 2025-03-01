using System;
using System.Collections.Generic;
using DoubleDCore.Automation.Base;

namespace DoubleDCore.Automation
{
    public class StateMachine : IFullStateMachine, IDisposable
    {
        private readonly Dictionary<Type, object> _states = new();

        private IExitableState _currentState;
        public IExitableState CurrentState => _currentState;

        public void BindState<TState>(Func<TState> creator) where TState : IExitableState
        {
            var type = typeof(TState);
            _states.Add(type, creator);
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = LoadState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = LoadState<TState>();
            state.Enter(payload);
        }

        private TState LoadState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableState
        {
            var type = typeof(TState);

            if (_states.TryGetValue(type, out var creator) == false)
                throw new Exception($"State {type.Name} does not bind");

            return (creator as Func<TState>)!.Invoke();
        }

        public void Dispose()
        {
            _currentState?.Exit();
            _currentState = null;

            _states.Clear();
        }
    }
}