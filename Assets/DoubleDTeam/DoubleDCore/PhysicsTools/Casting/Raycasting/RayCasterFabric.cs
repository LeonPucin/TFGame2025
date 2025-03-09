using Zenject;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting
{
    public class RayCasterFabric : IRayCasterFabric
    {
        private readonly DiContainer _diContainer;
        private readonly TickableManager _tickableManager;

        [Inject]
        private RayCasterFabric(DiContainer diContainer, TickableManager tickableManager)
        {
            _diContainer = diContainer;
            _tickableManager = tickableManager;
        }

        public IRayCaster Create()
        {
            var rayCaster = _diContainer.Instantiate<RayCaster>();
            _tickableManager.AddFixed(rayCaster);
            return rayCaster;
        }
    }
}