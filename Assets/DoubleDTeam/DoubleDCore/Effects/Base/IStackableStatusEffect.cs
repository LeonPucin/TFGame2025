namespace DoubleDCore.Effects.Base
{
    public interface IStackableStatusEffect<in TStackType> : IStatusEffect where TStackType : IStatusEffect
    {
        public void Stack(TStackType statusEffect);
    }
}