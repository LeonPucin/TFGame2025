using DoubleDCore.TimeTools;
using DoubleDCore.UI.Base;
using Game.Source.Base;
using Game.Source.Items.Base;
using Game.Source.Models;
using Game.Source.UI.Pages;
using UnityEngine;
using Zenject;

namespace Game.Source.Items
{
    public class Gun : ActionItem
    {
        [Range(0.1f, 1000f), SerializeField] private float _shootDistance = 100f;
        [Range(0.1f, 1000f), SerializeField] private float _damage = 10f;
        [Range(1, 500), SerializeField] private int _fireRate = 3;

        [Space, SerializeField] private AudioClip _fireSound;

        private readonly Timer _fireDelay = new(TimeBindingType.ScaledTime);

        private LayerMask _shootLayerMask;

        private IUIManager _uiManager;

        [Inject]
        private void Init(WorldRule worldRule, IUIManager uiManager)
        {
            _shootLayerMask = worldRule.Config.ShootMask;
            _uiManager = uiManager;
        }

        public override void Take()
        {
            _uiManager.OpenPage<GunSightPage>();
        }

        public override void Drop()
        {
            if (_uiManager.PageIsOpened<GunSightPage>())
                _uiManager.ClosePage<GunSightPage>();
        }

        public override void StartAction(object actor)
        {
            if (actor is not IGunActor gunActor)
                return;

            _isFire = true;
            StartFire(gunActor);
        }

        public override void StopAction(object actor)
        {
            _isFire = false;
        }

        private void Fire(IGunActor gunActor)
        {
            Physics.Raycast(gunActor.ShootRay, out var hit, _shootDistance, _shootLayerMask,
                QueryTriggerInteraction.Ignore);

            if (hit.collider == null || hit.collider.TryGetComponent(out IDamageable damageable) == false)
                return;

            damageable.TakeDamage(_damage);
        }

        private bool _isFire;

        private async void StartFire(IGunActor gunActor)
        {
            if (_fireDelay.IsWorked)
                return;

            do
            {
                Fire(gunActor);
                AudioSource.PlayClipAtPoint(_fireSound, transform.position);

                await _fireDelay.Start(1f / _fireRate);
            } while (_isFire);
        }
    }
}