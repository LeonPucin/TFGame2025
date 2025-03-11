using Game.Source.Items.Base;
using UnityEngine;

namespace Game.Source.Items
{
    public class SeedsBag : TakeableItem
    {
        [SerializeField] private int _startSeedsAmount = 10;

        private int _seedsAmount;

        public int Seeds
        {
            get => _seedsAmount;
            set => _seedsAmount = value;
        }

        protected override void TakeableAwake()
        {
            _seedsAmount = _startSeedsAmount;
        }
    }
}