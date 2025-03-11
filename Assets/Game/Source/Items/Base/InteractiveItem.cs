using UnityEngine;

namespace Game.Source.Items.Base
{
    public abstract class InteractiveItem : SelectableItem, IInteractiveObject
    {
        [Range(0f, 30f), SerializeField] private float _interactDelay = 0.5f;

        public float InteractDelay => _interactDelay;

        public abstract bool CanInteract(object interactor);

        public abstract void Interact(object interactor);

        public override void Select()
        {
        }

        public override void Deselect()
        {
        }
    }
}