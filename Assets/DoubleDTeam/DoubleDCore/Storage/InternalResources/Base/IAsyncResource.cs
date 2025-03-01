using Cysharp.Threading.Tasks;

namespace DoubleDCore.Storage.Base
{
    public interface IAsyncResource : IReleasable
    {
        public UniTask Load();
    }
}