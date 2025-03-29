using System;

namespace DoubleDCore.PhysicsTools.CollisionImpacts
{
    public interface ITargetListener<out TTargetType>
    {
        public event Action<TTargetType> TargetEnter;
        public event Action<TTargetType> TargetExit;
    }
}