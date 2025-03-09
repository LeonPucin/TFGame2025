namespace Game.Source.Interactive
{
    public interface IInteractiveObject
    {
        public InteractiveInfo InteractiveInfo { get; }

        public void Interact();
    }
}