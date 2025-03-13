using Infrastructure.Input;
using Infrastructure.Input.Maps;
using UnityEngine.SceneManagement;
using Zenject;
using IState = DoubleDCore.Automation.Base.IState;

namespace Infrastructure.States
{
    public class MainMenuState : IState
    {
        private readonly BootstrapInfo _bootstrapInfo;
        private readonly InputService _inputService;

        [Inject]
        public MainMenuState(BootstrapInfo bootstrapInfo, InputService inputService)
        {
            _bootstrapInfo = bootstrapInfo;
            _inputService = inputService;
        }

        public void Enter()
        {
            SceneManager.LoadScene(_bootstrapInfo.MainMenuSceneName, LoadSceneMode.Single);

            _inputService.SwitchMap<UIMap>();
        }

        public void Exit()
        {
        }
    }
}