using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items.Base
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class TakeableItem : InteractiveItem, ITakeableObject
    {
        [SerializeField] private TakeableType _takeableType = TakeableType.Everything;

        public TakeableType TakeableType => _takeableType;

        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();

            TakeableAwake();
        }

        public override void Interact(object interactor)
        {
            if (interactor is IReceiver<TakeableItem> receiver)
                receiver.Put(this);
        }

        protected virtual void TakeableAwake()
        {
        }

        public virtual void Take()
        {
        }

        public virtual void Drop()
        {
        }

        public override void Select()
        {
        }

        public override void Deselect()
        {
        }
    }
}