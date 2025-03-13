namespace Game.Source.Items.Base
{
    public abstract class InteractiveItem : SelectableItem, IInteractiveObject
    {
        public abstract float InteractDelay { get; }

        public abstract bool CanInteract(object interactor);

        public abstract void Interact(object interactor);

        public override void Select(object selector)
        {
        }

        public override void Deselect(object selector)
        {
        }
    }
}