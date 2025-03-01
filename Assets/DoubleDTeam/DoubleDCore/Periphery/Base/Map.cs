namespace DoubleDCore.Periphery.Base
{
    public abstract class Map
    {
        public bool IsActive { get; private set; }

        public void Enable()
        {
            IsActive = true;
            Activate();
        }

        public void Disable()
        {
            IsActive = false;
            Deactivate();
        }

        protected abstract void Activate();
        protected abstract void Deactivate();
    }
}