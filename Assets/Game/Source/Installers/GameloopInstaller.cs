using UnityEngine;
using Zenject;

namespace Game.Source.Installers
{
    public class GameloopInstaller : MonoInstaller
    {
        [SerializeField] private GasManager _gasManager;

        public override void InstallBindings()
        {
            Container.Bind<GasManager>().FromInstance(_gasManager).AsCached();
        }
    }
}