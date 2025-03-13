using Game.Source.Items.Base;
using UnityEngine;

namespace Game.Source.Entity.Base
{
    public abstract class EntityButton : InteractiveItem
    {
        [Min(0), SerializeField] private float _pressDelay = 1f;

        public override float InteractDelay => _pressDelay;

        public abstract override bool CanInteract(object interactor);

        public abstract override void Interact(object interactor);
    }
}