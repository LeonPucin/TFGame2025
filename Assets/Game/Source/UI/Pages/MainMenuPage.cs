using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Infrastructure;
using Infrastructure.States;
using Zenject;

namespace Game.Source.UI.Pages
{
    public class MainMenuPage : MonoPage, IUIPage
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Init(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public override void Initialize()
        {
            SetCanvasState(true);
        }

        public void Open()
        {
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void ExitGame()
        {
            _gameStateMachine.Enter<EndPointState>();
        }
    }
}