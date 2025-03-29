using Zenject;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting.Fabric
{
    public interface IRayCasterFabric<out TCollierType> : IFactory<IRayCaster<TCollierType>>
    {
    }
}