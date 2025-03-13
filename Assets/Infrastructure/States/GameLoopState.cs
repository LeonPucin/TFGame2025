using DoubleDCore.Automation.Base;
using DoubleDCore.Configuration;
using DoubleDCore.Localization.Base;
using DoubleDCore.Storage.Base;
using Game.Source;
using Game.Source.AI;
using Game.Source.Base;
using Game.Source.Configs;
using Game.Source.Models;
using Infrastructure.Input;
using Infrastructure.Input.Maps;
using UnityEngine;
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

        [Inject]
        public GameLoopState(BootstrapInfo bootstrapInfo, ISaveController saveController, DiContainer diContainer,
            InputService inputServices, IResourcesContainer resourcesContainer)
        {
            _bootstrapInfo = bootstrapInfo;
            _saveController = saveController;
            _diContainer = diContainer;
            _inputServices = inputServices;
            _resourcesContainer = resourcesContainer;
        }

        private LocaleSave _localeSave;

        public void Enter()
        {
            var config = _resourcesContainer.GetResource<ScriptableConfigsResource>().Get<WorldRuleConfig>();

            var worldRule = new WorldRule(GetDefaultTeamMatrix(), config);
            _diContainer.Bind<WorldRule>().FromInstance(worldRule).AsCached();

            _localeSave = _diContainer.Instantiate<LocaleSave>();
            _saveController.Subscribe(_localeSave);

            SceneManager.LoadScene(_bootstrapInfo.NextSceneName, LoadSceneMode.Single);

            _inputServices.SwitchMap<CharacterMap>();
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
    }
}