namespace Game.Source.Items.Base
{
    public interface IInteractiveObject : ISelectableObject
    {
        public float InteractDelay { get; }

        public bool CanInteract(object interactor);

        public void Interact(object interactor);
    }
}