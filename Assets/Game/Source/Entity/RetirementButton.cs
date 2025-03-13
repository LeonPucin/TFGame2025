using Game.Source.Entity.Base;
using Infrastructure;
using Infrastructure.States;
using Zenject;

namespace Game.Source.Entity
{
    public class RetirementButton : EntityButton
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Init(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public override bool CanInteract(object interactor)
        {
            return true;
        }

        public override void Interact(object interactor)
        {
            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}