using DoubleDCore.Economy.Base;

namespace Game.Source.Base
{
    public interface IValuableObject
    {
        public int Cost { get; }

        public IWallet<int> Owner { get; set; }
    }
}