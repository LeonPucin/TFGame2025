using DoubleDCore.Fabrics.Base;
using Game.Source.Entity;
using Game.Source.Items.Base;
using UnityEngine;
using Zenject;

namespace Game.Source.Items
{
    public class SeedsBag : TakeableItem
    {
        [SerializeField] private int _startSeedsAmount = 10;
        [SerializeField] private Plant _plantPrefab;

        private int _seedsAmount;

        public int Seeds
        {
            get => _seedsAmount;
            set => _seedsAmount = value;
        }

        private IPrefabFabric _prefabFabric;

        [Inject]
        private void Init(IPrefabFabric prefabFabric)
        {
            _prefabFabric = prefabFabric;
        }

        protected override void TakeableAwake()
        {
            _seedsAmount = _startSeedsAmount;
        }

        public Plant GetPlant()
        {
            return _prefabFabric.Create(_plantPrefab, transform.position, Quaternion.identity, null);
        }
    }
}