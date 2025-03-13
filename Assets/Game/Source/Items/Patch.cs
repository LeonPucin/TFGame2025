using Game.Source.Items.Base;
using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items
{
    public class Patch : InteractiveItem
    {
        [Range(0f, 30f), SerializeField] private float _interactDelay = 0.5f;

        public override float InteractDelay => _interactDelay;

        public override bool CanInteract(object interactor)
        {
            if (interactor is not IReceiver<TakeableItem> receiver)
                return false;

            if (receiver.Peek() is not SeedsBag seedsBag)
                return false;

            return seedsBag.Seeds > 0;
        }

        public override void Interact(object interactor)
        {
            if (interactor is not IReceiver<TakeableItem> receiver)
                return;

            if (receiver.Peek() is not SeedsBag seedsBag)
                return;

            seedsBag.Seeds--;

            Debug.Log("Seeds planted: " + seedsBag.Seeds);
        }
    }
}