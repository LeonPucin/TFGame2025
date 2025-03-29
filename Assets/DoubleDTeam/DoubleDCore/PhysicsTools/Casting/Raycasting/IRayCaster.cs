using System;
using Zenject;

namespace DoubleDCore.PhysicsTools.Casting.Raycasting
{
    public interface IRayCaster<out TColliderType> : ICaster<RayCastInfo, TColliderType>, IFixedTickable, IDisposable
    {
    }
}