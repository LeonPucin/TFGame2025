using Cysharp.Threading.Tasks;
using DoubleDCore.Automation.Base;
using DoubleDCore.Configuration;
using DoubleDCore.Localization.Base;
using DoubleDCore.Storage.Base;
using DoubleDCore.UI.Base;
using Game.Source.Configs;
using Game.Source.Models;
using Game.Source.UI.Pages;
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
        private readonly IResourcesContainer _resourcesContainer;
        private readonly IUIManager _uiManager;

        [Inject]
        public GameLoopState(BootstrapInfo bootstrapInfo, ISaveController saveController, DiContainer diContainer,
            InputService inputServices, IResourcesContainer resourcesContainer, IUIManager uiManager)
        {
            _bootstrapInfo = bootstrapInfo;
            _saveController = saveController;
            _diContainer = diContainer;
            _inputServices = inputServices;
            _resourcesContainer = resourcesContainer;
            _uiManager = uiManager;
        }

        private LocaleSave _localeSave;

        public void Enter()
        {
            var config = _resourcesContainer.GetResource<ScriptableConfigsResource>().Get<WorldRuleConfig>();

            var worldRule = new WorldRule(GetDefaultTeamMatrix(), config);
            _diContainer.Bind<WorldRule>().FromInstance(worldRule).AsCached();

            _localeSave = _diContainer.Instantiate<LocaleSave>();
            _saveController.Subscribe(_localeSave);

            SceneManager.LoadScene(_bootstrapInfo.GameloopSceneName, LoadSceneMode.Single);

            _inputServices.SwitchMap<CharacterMap>();

            Start();
        }

        public void Exit()
        {
            _saveController.SaveAll();
            _saveController.Unsubscribe(_localeSave);

            _diContainer.Unbind<WorldRule>();
        }

        private TeamMatrix GetDefaultTeamMatrix()
        {
            var teamMatrix = new TeamMatrix();

            teamMatrix.SetInteraction(Team.Red, Team.Blue, true);
            teamMatrix.SetInteraction(Team.Red, Team.None, true);

            return teamMatrix;
        }

        private async void Start()
        {
            await UniTask.NextFrame();

            _uiManager.OpenPage<DefaultPointerPage>();
        }
    }
}