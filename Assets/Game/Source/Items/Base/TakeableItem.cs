using Game.Source.Storage;
using UnityEngine;

namespace Game.Source.Items.Base
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class TakeableItem : InteractiveItem, ITakeableObject
    {
        [Range(0f, 30f), SerializeField] private float _takeDelay = 0.5f;
        [SerializeField] private TakeableType _takeableType = TakeableType.Everything;

        public TakeableType TakeableType => _takeableType;

        public Rigidbody Rigidbody { get; private set; }

        private Collider[] _colliders;

        public override float InteractDelay => _takeDelay;
        
        public float TakeDelay => _takeDelay;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _colliders = GetComponents<Collider>();

            TakeableAwake();
        }

        public void SetKinematic(bool value)
        {
            Rigidbody.isKinematic = value;
            Rigidbody.interpolation = value ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;

            foreach (var col in _colliders)
                col.enabled = !value;
        }

        public override void Interact(object interactor)
        {
            if (interactor is IReceiver<TakeableItem> receiver)
                receiver.Put(this);
        }

        public override bool CanInteract(object interactor)
        {
            return interactor is IReceiver<TakeableItem>;
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

        public override void Select(object selector)
        {
        }

        public override void Deselect(object selector)
        {
        }
    }
}