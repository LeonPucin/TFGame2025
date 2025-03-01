using System;

namespace DoubleDCore.Pooling.Base
{
    public interface IPooler<TPoolingType>
    {
        public int Count { get; }

        public void Push(TPoolingType poolingObject, Action<TPoolingType> onGet = null, Action<TPoolingType> onReturn = null);

        public TPoolingType Get();

        public bool TryGet(out TPoolingType result);

        public void Return(TPoolingType poolingObject);

        public void Clear();
    }
}