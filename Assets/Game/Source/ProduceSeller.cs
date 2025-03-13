using DoubleDCore.PhysicsTools.CollisionImpacts;
using Game.Source.Base;
using Game.Source.Items.Base;
using UnityEngine;

namespace Game.Source
{
    public class ProduceSeller : TriggerListener<TakeableItem>
    {
        protected override bool IsTarget(Collider col, out TakeableItem target)
        {
            return col.TryGetComponent(out target);
        }

        protected override void OnTriggerStart(TakeableItem target)
        {
            if (target is not IValuableObject valuable)
            {
                Destroy(target.gameObject);
                return;
            }

            if (valuable.Owner == null)
            {
                Destroy(target.gameObject);
                Debug.LogWarning(valuable.Owner);
                return;
            }

            valuable.Owner.Add(valuable.Cost);
            Destroy(target.gameObject);
        }
    }
}