using System.Linq;
using DoubleDCore.Fabrics.Base;
using DoubleDCore.Identification;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;
using Zenject;

namespace Game.Source.Entity
{
    public class Dispenser : InteractiveItem
    {
        [Min(0), SerializeField] private float _dispenseDelay = 1f;
        [SerializeField] private TakeableItem _prefab;

        [Space, Range(0.01f, 1000f), SerializeField]
        private float _checkRadius = 0.5f;

        [SerializeField] private LayerMask _checkLayer;
        [Min(1), SerializeField] private int _maxItems = 5;

        public override float InteractDelay => _dispenseDelay;

        private IPrefabFabric _fabric;

        [Inject]
        private void Init(IPrefabFabric fabric)
        {
            _fabric = fabric;
        }

        public override bool CanInteract(object interactor)
        {
            if (interactor is not IReceiver<TakeableItem> receiver)
                return false;

            int countInstance = GetInstanceCount(interactor);

            return countInstance < _maxItems;
        }

        private readonly Collider[] _buffer = new Collider[32];

        public override void Interact(object interactor)
        {
            if (interactor is not IReceiver<TakeableItem> receiver)
                return;

            int countInstance = GetInstanceCount(interactor);

            if (countInstance >= _maxItems)
                return;

            var item = _fabric.Create(_prefab, transform.position, Quaternion.identity, null);

            receiver.Put(item);
        }

        private int GetInstanceCount(object interactor)
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _checkRadius, _buffer, _checkLayer,
                QueryTriggerInteraction.Ignore);

            int countInstance = _buffer
                .Take(count)
                .Count(c => c.TryGetComponent(out TakeableItem item)
                            && item is IIdentifying identifiableItem
                            && _prefab is IIdentifying identifiablePrefab
                            && identifiableItem.ID == identifiablePrefab.ID);

            if (interactor is IReceiver<TakeableItem> receiver)
            {
                countInstance += receiver.Peek() is IIdentifying identifyingItem
                                 && _prefab is IIdentifying identifyingPrefab
                                 && identifyingItem.ID == identifyingPrefab.ID
                    ? 1
                    : 0;
            }

            return countInstance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _checkRadius);
        }
    }
}