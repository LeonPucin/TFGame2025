using DoubleDCore.Economy.Base;
using Game.Source.Base;
using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items
{
    public class Produce : TakeableItem, IValuableObject
    {
        [Min(0), SerializeField] private int _startCost;

        public int StartCost => _startCost;
        public float Mass => Rigidbody.mass;

        public int Cost => StartCost;

        public IWallet<int> Owner { get; set; }

        public override void Interact(object interactor)
        {
            if (interactor is IReceiver<TakeableItem> receiver && receiver.Peek() is ProduceTray tray)
            {
                if (tray.Content.Count < tray.Capacity)
                {
                    tray.AddContent(this);
                    return;
                }
            }

            base.Interact(interactor);
        }
    }
}