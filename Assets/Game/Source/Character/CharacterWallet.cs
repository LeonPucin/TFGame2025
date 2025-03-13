using System;
using DoubleDCore.Economy;
using DoubleDCore.Economy.Base;
using UnityEngine;

namespace Game.Source.Character
{
    public class CharacterWallet : MonoBehaviour, IWallet<int>
    {
        [SerializeField] private int _startValue = 100;

        private readonly IntegerWallet _wallet = new("none");

        private void Start()
        {
            Add(_startValue);
        }

        public event Action<int, int> ValueChanged; // current value, delta

        public int Value => _wallet.Value;

        public void Add(int value, object provider = null)
        {
            _wallet.Add(value, provider);

            ValueChanged?.Invoke(_wallet.Value, value);
        }

        public bool TrySpend(int value, object provider = null)
        {
            bool isSuccess = _wallet.TrySpend(value, provider);

            if (isSuccess)
                ValueChanged?.Invoke(_wallet.Value, -value);

            return isSuccess;
        }
    }
}