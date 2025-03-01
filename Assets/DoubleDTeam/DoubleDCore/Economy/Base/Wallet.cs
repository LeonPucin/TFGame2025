using System;
using DoubleDCore.Storage.Base;

namespace DoubleDCore.Economy.Base
{
    public abstract class Wallet<TValueType> : ISaveObject
    {
        /// <summary>
        /// Event that is called when value in wallet is changed.
        /// param1: old value
        /// param2: new value
        /// </summary>
        public event Action<TValueType, TValueType> ValueChanged;

        public Wallet(string resourceID)
        {
            ResourceID = resourceID;
        }

        public string ResourceID { get; }

        public string Key => ResourceID;

        public TValueType Value { get; private set; }

        public abstract void Add(TValueType value, object provider = null);

        public abstract bool TrySpend(TValueType value, object provider = null);

        public abstract string GetData();

        public abstract string GetDefaultData();

        public abstract void OnLoad(string data);

        protected abstract bool IsValidValue(TValueType value);

        protected void SetValue(TValueType value)
        {
            if (IsValidValue(value) == false)
                return;

            TValueType oldValue = Value;
            Value = value;
            ValueChanged?.Invoke(oldValue, value);
        }
    }
}