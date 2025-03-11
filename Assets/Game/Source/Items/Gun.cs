using Game.Source.Base;
using Game.Source.Items.Base;
using UnityEngine;

namespace Game.Source.Items
{
    public class Gun : ActionItem
    {
        [Range(0.1f, 1000f), SerializeField] private float _shootDistance = 100f;
        [Range(0.1f, 1000f), SerializeField] private float _damage = 10f;
        [SerializeField] private LayerMask _shootLayerMask;

        public override void Action(object actor)
        {
            if (actor is not IGunActor gunActor)
                return;

            Physics.Raycast(gunActor.ShootRay, out var hit, _shootDistance, _shootLayerMask);

            if (hit.collider == null || hit.collider.TryGetComponent(out IDamageable damageable) == false)
                return;

            damageable.TakeDamage(_damage);
        }
    }
}