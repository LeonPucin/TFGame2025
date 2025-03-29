using System;
using UnityEngine;

namespace DoubleDCore.PhysicsTools.CollisionImpacts
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TriggerListener2D<TTargetType> : MonoBehaviour, ITargetListener<TTargetType>
    {
        public event Action<TTargetType> TargetEnter;
        public event Action<TTargetType> TargetExit;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (IsTarget(other, out var target) == false)
                return;

            OnTriggerStart(target);

            TargetEnter?.Invoke(target);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (IsTarget(other, out var target) == false)
                return;

            OnTriggerEnd(target);

            TargetExit?.Invoke(target);
        }

        protected abstract bool IsTarget(Collider2D col, out TTargetType target);

        protected virtual void OnTriggerStart(TTargetType target)
        {
        }

        protected virtual void OnTriggerEnd(TTargetType target)
        {
        }
    }
}