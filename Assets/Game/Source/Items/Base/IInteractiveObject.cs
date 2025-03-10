namespace Game.Source.Items.Base
{
    public interface IInteractiveObject : ISelectableObject
    {
        public float InteractDelay { get; }

        public void Interact(object interactor);
    }
}