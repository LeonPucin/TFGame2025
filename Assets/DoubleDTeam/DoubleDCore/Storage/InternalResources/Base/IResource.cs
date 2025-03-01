namespace DoubleDCore.Storage.Base
{
    public interface IResource : IReleasable
    {
        public void Load();
    }
}