using System;

namespace DoubleDCore.Automation.Base
{
    public interface IBaseStateMachine
    {
        public IExitableState CurrentState { get; }

        public void BindState<TState>(Func<TState> creator) where TState : IExitableState;

        public TState GetState<TState>() where TState : class, IExitableState;
    }
}