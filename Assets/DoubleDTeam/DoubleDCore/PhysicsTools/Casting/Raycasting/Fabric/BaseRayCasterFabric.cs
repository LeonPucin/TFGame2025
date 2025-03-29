using Zenject;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting.Fabric
{
    public abstract class BaseRayCasterFabric<TColliderType> : IRayCasterFabric<TColliderType>
    {
        protected DiContainer DiContainer;
        private TickableManager _tickableManager;

        [Inject]
        private void Init(DiContainer diContainer, TickableManager tickableManager)
        {
            DiContainer = diContainer;
            _tickableManager = tickableManager;
        }

        public IRayCaster<TColliderType> Create()
        {
            var rayCaster = GetCaster();
            _tickableManager.AddFixed(rayCaster);
            return rayCaster;
        }

        protected abstract IRayCaster<TColliderType> GetCaster();
    }
}