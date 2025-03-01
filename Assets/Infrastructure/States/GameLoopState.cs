using DoubleDCore.Automation.Base;
using DoubleDCore.Jurisdiction.Base;
using DoubleDCore.Localization.Base;
using DoubleDCore.Storage.Base;
using Infrastructure.Input;
using Infrastructure.Input.Maps;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly BootstrapInfo _bootstrapInfo;
        private readonly ISaveController _saveController;
        private readonly DiContainer _diContainer;
        private readonly InputService _inputServices;

        [Inject]
        public GameLoopState(BootstrapInfo bootstrapInfo, ISaveController saveController, DiContainer diContainer,
            InputService inputServices)
        {
            _bootstrapInfo = bootstrapInfo;
            _saveController = saveController;
            _diContainer = diContainer;
            _inputServices = inputServices;
        }

        private LocaleSave _localeSave;
        private IBuild _gameplayBuild;

        public void Enter()
        {
            _localeSave = _diContainer.Instantiate<LocaleSave>();
            _saveController.Subscribe(_localeSave);

            SceneManager.LoadScene(_bootstrapInfo.NextSceneName, LoadSceneMode.Single);

            _inputServices.SwitchMap<CharacterMap>();
        }

        public void Exit()
        {
            _saveController.SaveAll();
            _saveController.Unsubscribe(_localeSave);
        }
    }
}