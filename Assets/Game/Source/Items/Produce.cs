using Game.Source.Items.Base;
using UnityEngine;

namespace Game.Source.Items
{
    public class Produce : TakeableItem
    {
        [Min(0), SerializeField] private int _startCost;

        public int StartCost => _startCost;
        public float Mass => Rigidbody.mass;
    }
}