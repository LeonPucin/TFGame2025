using UnityEngine;

namespace DoubleDCore.PhysicsTools.Casting
{
    public interface ITargetListener<TTarget>
    {
        public TTarget GetTarget(Collider target);

        public bool IsTarget(TTarget target);

        public void OnCastEnter(TTarget target);

        public void OnCastExit(TTarget target);
    }
}