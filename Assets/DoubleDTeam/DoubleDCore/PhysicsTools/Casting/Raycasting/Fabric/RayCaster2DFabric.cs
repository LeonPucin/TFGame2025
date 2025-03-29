using UnityEngine;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting.Fabric
{
    public class RayCaster2DFabric : BaseRayCasterFabric<Collider>
    {
        protected override IRayCaster<Collider> GetCaster()
        {
            return DiContainer.Instantiate<RayCaster>();
        }
    }
}