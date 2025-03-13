using System;

namespace DoubleDCore.Economy.Base
{
    public interface IWallet<TValueType>
    {
        public event Action<TValueType, TValueType> ValueChanged;

        public TValueType Value { get; }

        public abstract void Add(TValueType value, object provider = null);

        public abstract bool TrySpend(TValueType value, object provider = null);
    }
}