using Cysharp.Threading.Tasks;
using DoubleDCore.Economy.Base;
using DoubleDCore.Fabrics.Base;
using Game.Source.Items;
using UnityEngine;
using Zenject;

namespace Game.Source.Entity
{
    public class BuySeedsButton : BuyButton
    {
        [SerializeField] private SeedsBag _prefab;
        [SerializeField] private Transform _spawnPoint;
        [Min(0), SerializeField] private float _spawnDelay = 1f;
        [SerializeField] protected AudioClip _fallSound;

        private IPrefabFabric _prefabFabric;

        [Inject]
        private void Init(IPrefabFabric prefabFabric)
        {
            _prefabFabric = prefabFabric;
        }

        public async override void Interact(object interactor)
        {
            base.Interact(interactor);

            if (interactor is not IWallet<int> wallet)
                return;

            if (wallet.TrySpend(Cost) == false)
                return;

            AudioSource.PlayClipAtPoint(_fallSound, _spawnPoint.position);

            await UniTask.WaitForSeconds(_spawnDelay);

            _prefabFabric.Create(_prefab, _spawnPoint.position, Quaternion.identity, null);
        }
    }
}