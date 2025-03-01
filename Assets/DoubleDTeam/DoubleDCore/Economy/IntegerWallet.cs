using DoubleDCore.Economy.Base;
using UnityEngine;

namespace DoubleDCore.Economy
{
    public class IntegerWallet : Wallet<int>
    {
        public IntegerWallet(string resourceID) : base(resourceID)
        {
        }

        public override void Add(int value, object provider = null)
        {
            SetValue(value + Value);
        }

        public override bool TrySpend(int value, object provider = null)
        {
            if (Value - value < 0)
            {
                Debug.LogWarning($"Not enough {ResourceID} in wallet! Value: {Value}, value to spend: {value}");
                return false;
            }

            SetValue(Value - value);
            return true;
        }

        public override string GetData()
        {
            return Value.ToString();
        }

        public override string GetDefaultData()
        {
            return 0.ToString();
        }

        public override void OnLoad(string data)
        {
            bool isSuccess = int.TryParse(data, out var result);

            if (isSuccess)
                SetValue(result);
            else
                Debug.LogError($"Can't parse data to int! Data: {data}");
        }

        protected override bool IsValidValue(int value)
        {
            if (value >= 0)
                return true;

            Debug.LogError($"Value in {ResourceID} wallet can't be negative! Value: {value}");
            return false;
        }
    }
}