using DoubleDCore.Automation.Base;
using DoubleDCore.Configuration;
using DoubleDCore.Economy;
using DoubleDCore.Storage.Base;
using Zenject;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IResourcesContainer _resourcesContainer;

        [Inject]
        public BootstrapState(GameStateMachine stateMachine, IResourcesContainer resourcesContainer)
        {
            _stateMachine = stateMachine;
            _resourcesContainer = resourcesContainer;
        }

        public void Enter()
        {
            _resourcesContainer.AddResource(new ScriptableConfigsResource());
            _resourcesContainer.AddResource(new ItemsResource());

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}